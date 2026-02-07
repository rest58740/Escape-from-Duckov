using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200007B RID: 123
	public sealed class AnySerializer : Serializer
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x0001B9E8 File Offset: 0x00019BE8
		public AnySerializer(Type serializedType)
		{
			this.SerializedType = serializedType;
			this.IsEnum = this.SerializedType.IsEnum;
			this.IsValueType = this.SerializedType.IsValueType;
			this.MayBeBoxedValueType = (this.SerializedType.IsInterface || this.SerializedType == typeof(object) || this.SerializedType == typeof(ValueType) || this.SerializedType == typeof(Enum));
			this.IsAbstract = (this.SerializedType.IsAbstract || this.SerializedType.IsInterface);
			this.IsNullable = (this.SerializedType.IsGenericType && this.SerializedType.GetGenericTypeDefinition() == typeof(Nullable));
			this.AllowDeserializeInvalidData = this.SerializedType.IsDefined(typeof(AllowDeserializeInvalidDataAttribute), true);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001BB08 File Offset: 0x00019D08
		public override object ReadValueWeak(IDataReader reader)
		{
			if (this.IsEnum)
			{
				string text;
				EntryType entryType = reader.PeekEntry(out text);
				if (entryType == EntryType.Integer)
				{
					ulong num;
					if (!reader.ReadUInt64(out num))
					{
						reader.Context.Config.DebugContext.LogWarning("Failed to read entry '" + text + "' of type " + entryType.ToString());
					}
					return Enum.ToObject(this.SerializedType, num);
				}
				reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
				{
					"Expected entry of type ",
					EntryType.Integer.ToString(),
					", but got entry '",
					text,
					"' of type ",
					entryType.ToString()
				}));
				reader.SkipEntry();
				return Activator.CreateInstance(this.SerializedType);
			}
			else
			{
				DeserializationContext context = reader.Context;
				if (context.Config.SerializationPolicy.AllowNonSerializableTypes || this.SerializedType.IsSerializable)
				{
					bool flag = true;
					string text2;
					EntryType entryType2 = reader.PeekEntry(out text2);
					if (this.IsValueType)
					{
						if (entryType2 == EntryType.Null)
						{
							context.Config.DebugContext.LogWarning("Expecting complex struct of type " + this.SerializedType.GetNiceFullName() + " but got null value.");
							reader.ReadNull();
							return Activator.CreateInstance(this.SerializedType);
						}
						if (entryType2 != EntryType.StartOfNode)
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Unexpected entry '",
								text2,
								"' of type ",
								entryType2.ToString(),
								", when ",
								EntryType.StartOfNode.ToString(),
								" was expected. A value has likely been lost."
							}));
							reader.SkipEntry();
							return Activator.CreateInstance(this.SerializedType);
						}
						try
						{
							Type serializedType = this.SerializedType;
							Type type;
							if (!reader.EnterNode(out type))
							{
								context.Config.DebugContext.LogError("Failed to enter node '" + text2 + "'.");
								return Activator.CreateInstance(this.SerializedType);
							}
							if (!(type != serializedType))
							{
								return this.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
							}
							if (type != null)
							{
								context.Config.DebugContext.LogWarning(string.Concat(new string[]
								{
									"Expected complex struct value ",
									serializedType.Name,
									" but the serialized value is of type ",
									type.Name,
									"."
								}));
								if (type.IsCastableTo(serializedType, false))
								{
									object obj = FormatterLocator.GetFormatter(type, context.Config.SerializationPolicy).Deserialize(reader);
									bool flag2 = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable);
									Func<object, object> func = (!this.IsNullable && !flag2) ? type.GetCastMethodDelegate(serializedType, false) : null;
									if (func != null)
									{
										return func.Invoke(obj);
									}
									return obj;
								}
								else
								{
									if (this.AllowDeserializeInvalidData || reader.Context.Config.AllowDeserializeInvalidData)
									{
										context.Config.DebugContext.LogWarning(string.Concat(new string[]
										{
											"Can't cast serialized type ",
											type.GetNiceFullName(),
											" into expected type ",
											serializedType.GetNiceFullName(),
											". Attempting to deserialize with possibly invalid data. Value may be lost or corrupted for node '",
											text2,
											"'."
										}));
										return this.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
									}
									context.Config.DebugContext.LogWarning(string.Concat(new string[]
									{
										"Can't cast serialized type ",
										type.GetNiceFullName(),
										" into expected type ",
										serializedType.GetNiceFullName(),
										". Value lost for node '",
										text2,
										"'."
									}));
									return Activator.CreateInstance(this.SerializedType);
								}
							}
							else
							{
								if (this.AllowDeserializeInvalidData || reader.Context.Config.AllowDeserializeInvalidData)
								{
									context.Config.DebugContext.LogWarning(string.Concat(new string[]
									{
										"Expected complex struct value ",
										serializedType.GetNiceFullName(),
										" but the serialized type could not be resolved. Attempting to deserialize with possibly invalid data. Value may be lost or corrupted for node '",
										text2,
										"'."
									}));
									return this.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
								}
								context.Config.DebugContext.LogWarning(string.Concat(new string[]
								{
									"Expected complex struct value ",
									serializedType.Name,
									" but the serialized type could not be resolved. Value lost for node '",
									text2,
									"'."
								}));
								return Activator.CreateInstance(this.SerializedType);
							}
						}
						catch (SerializationAbortException ex)
						{
							flag = false;
							throw ex;
						}
						catch (Exception exception)
						{
							context.Config.DebugContext.LogException(exception);
							return Activator.CreateInstance(this.SerializedType);
						}
						finally
						{
							if (flag)
							{
								reader.ExitNode();
							}
						}
					}
					switch (entryType2)
					{
					case EntryType.String:
						if (this.MayBeBoxedValueType)
						{
							string result;
							reader.ReadString(out result);
							return result;
						}
						goto IL_B46;
					case EntryType.Guid:
						if (this.MayBeBoxedValueType)
						{
							Guid guid;
							reader.ReadGuid(out guid);
							return guid;
						}
						goto IL_B46;
					case EntryType.Integer:
						if (this.MayBeBoxedValueType)
						{
							long num2;
							reader.ReadInt64(out num2);
							return num2;
						}
						goto IL_B46;
					case EntryType.FloatingPoint:
						if (this.MayBeBoxedValueType)
						{
							double num3;
							reader.ReadDouble(out num3);
							return num3;
						}
						goto IL_B46;
					case EntryType.Boolean:
						break;
					case EntryType.Null:
						reader.ReadNull();
						return null;
					case EntryType.StartOfNode:
						try
						{
							Type serializedType2 = this.SerializedType;
							Type type2;
							if (reader.EnterNode(out type2))
							{
								int currentNodeId = reader.CurrentNodeId;
								object obj2;
								if (type2 != null && serializedType2 != type2)
								{
									bool flag3 = false;
									bool flag4 = FormatterUtilities.IsPrimitiveType(type2);
									if (this.MayBeBoxedValueType && flag4)
									{
										Serializer serializer = Serializer.Get(type2);
										obj2 = serializer.ReadValueWeak(reader);
										flag3 = true;
									}
									else
									{
										bool flag5;
										if ((flag5 = serializedType2.IsAssignableFrom(type2)) || type2.HasCastDefined(serializedType2, false))
										{
											try
											{
												object obj3;
												if (flag4)
												{
													Serializer serializer2 = Serializer.Get(type2);
													obj3 = serializer2.ReadValueWeak(reader);
												}
												else
												{
													IFormatter formatter = FormatterLocator.GetFormatter(type2, context.Config.SerializationPolicy);
													obj3 = formatter.Deserialize(reader);
												}
												if (flag5)
												{
													obj2 = obj3;
												}
												else
												{
													Func<object, object> castMethodDelegate = type2.GetCastMethodDelegate(serializedType2, false);
													if (castMethodDelegate != null)
													{
														obj2 = castMethodDelegate.Invoke(obj3);
													}
													else
													{
														obj2 = obj3;
													}
												}
												flag3 = true;
												goto IL_9D2;
											}
											catch (SerializationAbortException ex2)
											{
												flag = false;
												throw ex2;
											}
											catch (InvalidCastException)
											{
												flag3 = false;
												obj2 = null;
												goto IL_9D2;
											}
										}
										if (!this.IsAbstract && (this.AllowDeserializeInvalidData || reader.Context.Config.AllowDeserializeInvalidData))
										{
											context.Config.DebugContext.LogWarning(string.Concat(new string[]
											{
												"Can't cast serialized type ",
												type2.GetNiceFullName(),
												" into expected type ",
												serializedType2.GetNiceFullName(),
												". Attempting to deserialize with invalid data. Value may be lost or corrupted for node '",
												text2,
												"'."
											}));
											obj2 = this.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
											flag3 = true;
										}
										else
										{
											IFormatter formatter2 = FormatterLocator.GetFormatter(type2, context.Config.SerializationPolicy);
											object reference = formatter2.Deserialize(reader);
											if (currentNodeId >= 0)
											{
												context.RegisterInternalReference(currentNodeId, reference);
											}
											obj2 = null;
										}
									}
									IL_9D2:
									if (!flag3)
									{
										context.Config.DebugContext.LogWarning(string.Concat(new string[]
										{
											"Can't cast serialized type ",
											type2.GetNiceFullName(),
											" into expected type ",
											serializedType2.GetNiceFullName(),
											". Value lost for node '",
											text2,
											"'."
										}));
										obj2 = null;
									}
								}
								else if (this.IsAbstract)
								{
									obj2 = null;
								}
								else
								{
									obj2 = this.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
								}
								if (currentNodeId >= 0)
								{
									context.RegisterInternalReference(currentNodeId, obj2);
								}
								return obj2;
							}
							context.Config.DebugContext.LogError("Failed to enter node '" + text2 + "'.");
							return null;
						}
						catch (SerializationAbortException ex3)
						{
							flag = false;
							throw ex3;
						}
						catch (Exception exception2)
						{
							context.Config.DebugContext.LogException(exception2);
							return null;
						}
						finally
						{
							if (flag)
							{
								reader.ExitNode();
							}
						}
						break;
					case EntryType.EndOfNode:
					case EntryType.StartOfArray:
					case EntryType.EndOfArray:
					case EntryType.PrimitiveArray:
					case EntryType.EndOfStream:
						goto IL_B46;
					case EntryType.InternalReference:
					{
						int id;
						reader.ReadInternalReference(out id);
						object internalReference = context.GetInternalReference(id);
						if (internalReference != null && !this.SerializedType.IsAssignableFrom(internalReference.GetType()))
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast internal reference type ",
								internalReference.GetType().GetNiceFullName(),
								" into expected type ",
								this.SerializedType.GetNiceFullName(),
								". Value lost for node '",
								text2,
								"'."
							}));
							return null;
						}
						return internalReference;
					}
					case EntryType.ExternalReferenceByIndex:
					{
						int index;
						reader.ReadExternalReference(out index);
						object externalObject = context.GetExternalObject(index);
						if (externalObject != null && !this.SerializedType.IsAssignableFrom(externalObject.GetType()))
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast external reference type ",
								externalObject.GetType().GetNiceFullName(),
								" into expected type ",
								this.SerializedType.GetNiceFullName(),
								". Value lost for node '",
								text2,
								"'."
							}));
							return null;
						}
						return externalObject;
					}
					case EntryType.ExternalReferenceByGuid:
					{
						Guid guid2;
						reader.ReadExternalReference(out guid2);
						object externalObject2 = context.GetExternalObject(guid2);
						if (externalObject2 != null && !this.SerializedType.IsAssignableFrom(externalObject2.GetType()))
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast external reference type ",
								externalObject2.GetType().GetNiceFullName(),
								" into expected type ",
								this.SerializedType.GetNiceFullName(),
								". Value lost for node '",
								text2,
								"'."
							}));
							return null;
						}
						return externalObject2;
					}
					case EntryType.ExternalReferenceByString:
					{
						string id2;
						reader.ReadExternalReference(out id2);
						object externalObject3 = context.GetExternalObject(id2);
						if (externalObject3 != null && !this.SerializedType.IsAssignableFrom(externalObject3.GetType()))
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast external reference type ",
								externalObject3.GetType().GetNiceFullName(),
								" into expected type ",
								this.SerializedType.GetNiceFullName(),
								". Value lost for node '",
								text2,
								"'."
							}));
							return null;
						}
						return externalObject3;
					}
					default:
						goto IL_B46;
					}
					if (this.MayBeBoxedValueType)
					{
						bool flag6;
						reader.ReadBoolean(out flag6);
						return flag6;
					}
					IL_B46:
					context.Config.DebugContext.LogWarning("Unexpected entry of type " + entryType2.ToString() + ", when a reference or node start was expected. A value has been lost.");
					reader.SkipEntry();
					return null;
				}
				context.Config.DebugContext.LogError("The type " + this.SerializedType.Name + " is not marked as serializable.");
				if (!this.IsValueType)
				{
					return null;
				}
				return Activator.CreateInstance(this.SerializedType);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001C758 File Offset: 0x0001A958
		public override void WriteValueWeak(string name, object value, IDataWriter writer)
		{
			if (this.IsEnum)
			{
				ulong value2;
				try
				{
					value2 = Convert.ToUInt64(value as Enum);
				}
				catch (OverflowException)
				{
					value2 = (ulong)Convert.ToInt64(value as Enum);
				}
				writer.WriteUInt64(name, value2);
				return;
			}
			SerializationContext context = writer.Context;
			ISerializationPolicy serializationPolicy = context.Config.SerializationPolicy;
			if (!serializationPolicy.AllowNonSerializableTypes && !this.SerializedType.IsSerializable)
			{
				context.Config.DebugContext.LogError("The type " + this.SerializedType.Name + " is not marked as serializable.");
				return;
			}
			if (this.IsValueType)
			{
				bool flag = true;
				try
				{
					writer.BeginStructNode(name, this.SerializedType);
					this.GetBaseFormatter(serializationPolicy).Serialize(value, writer);
					return;
				}
				catch (SerializationAbortException ex)
				{
					flag = false;
					throw ex;
				}
				finally
				{
					if (flag)
					{
						writer.EndNode(name);
					}
				}
			}
			bool flag2 = true;
			if (value == null)
			{
				writer.WriteNull(name);
				return;
			}
			int index;
			if (context.TryRegisterExternalReference(value, out index))
			{
				writer.WriteExternalReference(name, index);
				return;
			}
			Guid guid;
			if (context.TryRegisterExternalReference(value, out guid))
			{
				writer.WriteExternalReference(name, guid);
				return;
			}
			string id;
			if (context.TryRegisterExternalReference(value, out id))
			{
				writer.WriteExternalReference(name, id);
				return;
			}
			int id2;
			if (context.TryRegisterInternalReference(value, out id2))
			{
				Type type = value.GetType();
				if (this.MayBeBoxedValueType && FormatterUtilities.IsPrimitiveType(type))
				{
					try
					{
						writer.BeginReferenceNode(name, type, id2);
						Serializer serializer = Serializer.Get(type);
						serializer.WriteValueWeak(value, writer);
						return;
					}
					catch (SerializationAbortException ex2)
					{
						flag2 = false;
						throw ex2;
					}
					finally
					{
						if (flag2)
						{
							writer.EndNode(name);
						}
					}
				}
				IFormatter formatter;
				if (type == this.SerializedType)
				{
					formatter = this.GetBaseFormatter(serializationPolicy);
				}
				else
				{
					formatter = FormatterLocator.GetFormatter(type, serializationPolicy);
				}
				try
				{
					writer.BeginReferenceNode(name, type, id2);
					formatter.Serialize(value, writer);
					return;
				}
				catch (SerializationAbortException ex3)
				{
					flag2 = false;
					throw ex3;
				}
				finally
				{
					if (flag2)
					{
						writer.EndNode(name);
					}
				}
			}
			writer.WriteInternalReference(name, id2);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001C974 File Offset: 0x0001AB74
		private IFormatter GetBaseFormatter(ISerializationPolicy serializationPolicy)
		{
			if (serializationPolicy == AnySerializer.UnityPolicy)
			{
				if (this.UnityPolicyFormatter == null)
				{
					this.UnityPolicyFormatter = FormatterLocator.GetFormatter(this.SerializedType, AnySerializer.UnityPolicy);
				}
				return this.UnityPolicyFormatter;
			}
			if (serializationPolicy == AnySerializer.EverythingPolicy)
			{
				if (this.EverythingPolicyFormatter == null)
				{
					this.EverythingPolicyFormatter = FormatterLocator.GetFormatter(this.SerializedType, AnySerializer.EverythingPolicy);
				}
				return this.EverythingPolicyFormatter;
			}
			if (serializationPolicy == AnySerializer.StrictPolicy)
			{
				if (this.StrictPolicyFormatter == null)
				{
					this.StrictPolicyFormatter = FormatterLocator.GetFormatter(this.SerializedType, AnySerializer.StrictPolicy);
				}
				return this.StrictPolicyFormatter;
			}
			object formattersByPolicy_LOCK = this.FormattersByPolicy_LOCK;
			IFormatter formatter;
			lock (formattersByPolicy_LOCK)
			{
				if (!this.FormattersByPolicy.TryGetValue(serializationPolicy, ref formatter))
				{
					formatter = FormatterLocator.GetFormatter(this.SerializedType, serializationPolicy);
					this.FormattersByPolicy.Add(serializationPolicy, formatter);
				}
			}
			return formatter;
		}

		// Token: 0x04000165 RID: 357
		private static readonly ISerializationPolicy UnityPolicy = SerializationPolicies.Unity;

		// Token: 0x04000166 RID: 358
		private static readonly ISerializationPolicy StrictPolicy = SerializationPolicies.Strict;

		// Token: 0x04000167 RID: 359
		private static readonly ISerializationPolicy EverythingPolicy = SerializationPolicies.Everything;

		// Token: 0x04000168 RID: 360
		private readonly Type SerializedType;

		// Token: 0x04000169 RID: 361
		private readonly bool IsEnum;

		// Token: 0x0400016A RID: 362
		private readonly bool IsValueType;

		// Token: 0x0400016B RID: 363
		private readonly bool MayBeBoxedValueType;

		// Token: 0x0400016C RID: 364
		private readonly bool IsAbstract;

		// Token: 0x0400016D RID: 365
		private readonly bool IsNullable;

		// Token: 0x0400016E RID: 366
		private readonly bool AllowDeserializeInvalidData;

		// Token: 0x0400016F RID: 367
		private IFormatter UnityPolicyFormatter;

		// Token: 0x04000170 RID: 368
		private IFormatter StrictPolicyFormatter;

		// Token: 0x04000171 RID: 369
		private IFormatter EverythingPolicyFormatter;

		// Token: 0x04000172 RID: 370
		private readonly Dictionary<ISerializationPolicy, IFormatter> FormattersByPolicy = new Dictionary<ISerializationPolicy, IFormatter>(ReferenceEqualityComparer<ISerializationPolicy>.Default);

		// Token: 0x04000173 RID: 371
		private readonly object FormattersByPolicy_LOCK = new object();
	}
}

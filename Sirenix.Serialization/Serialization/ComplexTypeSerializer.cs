using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200007F RID: 127
	public class ComplexTypeSerializer<T> : Serializer<T>
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x0001CCF0 File Offset: 0x0001AEF0
		public override T ReadValue(IDataReader reader)
		{
			DeserializationContext context = reader.Context;
			if (!context.Config.SerializationPolicy.AllowNonSerializableTypes && !ComplexTypeSerializer<T>.TypeOf_T.IsSerializable)
			{
				context.Config.DebugContext.LogError("The type " + ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName() + " is not marked as serializable.");
				T result = default(T);
				return result;
			}
			bool flag = true;
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (ComplexTypeSerializer<T>.ComplexTypeIsValueType)
			{
				if (entryType == EntryType.Null)
				{
					context.Config.DebugContext.LogWarning("Expecting complex struct of type " + ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName() + " but got null value.");
					reader.ReadNull();
					T result = default(T);
					return result;
				}
				if (entryType != EntryType.StartOfNode)
				{
					context.Config.DebugContext.LogWarning(string.Concat(new string[]
					{
						"Unexpected entry '",
						text,
						"' of type ",
						entryType.ToString(),
						", when ",
						EntryType.StartOfNode.ToString(),
						" was expected. A value has likely been lost."
					}));
					reader.SkipEntry();
					T result = default(T);
					return result;
				}
				try
				{
					Type typeOf_T = ComplexTypeSerializer<T>.TypeOf_T;
					Type type;
					if (!reader.EnterNode(out type))
					{
						context.Config.DebugContext.LogError("Failed to enter node '" + text + "'.");
						return default(T);
					}
					if (!(type != typeOf_T))
					{
						return ComplexTypeSerializer<T>.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
					}
					if (type != null)
					{
						context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Expected complex struct value ",
							typeOf_T.GetNiceFullName(),
							" but the serialized value is of type ",
							type.GetNiceFullName(),
							"."
						}));
						if (type.IsCastableTo(typeOf_T, false))
						{
							object obj = FormatterLocator.GetFormatter(type, context.Config.SerializationPolicy).Deserialize(reader);
							bool flag2 = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable);
							Func<object, object> func = (!ComplexTypeSerializer<T>.ComplexTypeIsNullable && !flag2) ? type.GetCastMethodDelegate(typeOf_T, false) : null;
							if (func != null)
							{
								return (T)((object)func.Invoke(obj));
							}
							return (T)((object)obj);
						}
						else
						{
							if (ComplexTypeSerializer<T>.AllowDeserializeInvalidDataForT || reader.Context.Config.AllowDeserializeInvalidData)
							{
								context.Config.DebugContext.LogWarning(string.Concat(new string[]
								{
									"Can't cast serialized type ",
									type.GetNiceFullName(),
									" into expected type ",
									typeOf_T.GetNiceFullName(),
									". Attempting to deserialize with possibly invalid data. Value may be lost or corrupted for node '",
									text,
									"'."
								}));
								return ComplexTypeSerializer<T>.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
							}
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast serialized type ",
								type.GetNiceFullName(),
								" into expected type ",
								typeOf_T.GetNiceFullName(),
								". Value lost for node '",
								text,
								"'."
							}));
							return default(T);
						}
					}
					else
					{
						if (ComplexTypeSerializer<T>.AllowDeserializeInvalidDataForT || reader.Context.Config.AllowDeserializeInvalidData)
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Expected complex struct value ",
								typeOf_T.GetNiceFullName(),
								" but the serialized type could not be resolved. Attempting to deserialize with possibly invalid data. Value may be lost or corrupted for node '",
								text,
								"'."
							}));
							return ComplexTypeSerializer<T>.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
						}
						context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Expected complex struct value ",
							typeOf_T.GetNiceFullName(),
							" but the serialized type could not be resolved. Value lost for node '",
							text,
							"'."
						}));
						return default(T);
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
					return default(T);
				}
				finally
				{
					if (flag)
					{
						reader.ExitNode();
					}
				}
			}
			switch (entryType)
			{
			case EntryType.String:
				if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType)
				{
					string text2;
					reader.ReadString(out text2);
					return (T)((object)text2);
				}
				goto IL_A8B;
			case EntryType.Guid:
				if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType)
				{
					Guid guid;
					reader.ReadGuid(out guid);
					return (T)((object)guid);
				}
				goto IL_A8B;
			case EntryType.Integer:
				if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType)
				{
					long num;
					reader.ReadInt64(out num);
					return (T)((object)num);
				}
				goto IL_A8B;
			case EntryType.FloatingPoint:
				if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType)
				{
					double num2;
					reader.ReadDouble(out num2);
					return (T)((object)num2);
				}
				goto IL_A8B;
			case EntryType.Boolean:
				goto IL_9FC;
			case EntryType.Null:
				reader.ReadNull();
				return default(T);
			case EntryType.StartOfNode:
				goto IL_705;
			case EntryType.EndOfNode:
			case EntryType.StartOfArray:
			case EntryType.EndOfArray:
			case EntryType.PrimitiveArray:
			case EntryType.EndOfStream:
				goto IL_A8B;
			case EntryType.InternalReference:
				goto IL_677;
			case EntryType.ExternalReferenceByIndex:
			{
				int index;
				reader.ReadExternalReference(out index);
				object externalObject = context.GetExternalObject(index);
				try
				{
					return (T)((object)externalObject);
				}
				catch (InvalidCastException)
				{
					context.Config.DebugContext.LogWarning(string.Concat(new string[]
					{
						"Can't cast external reference type ",
						externalObject.GetType().GetNiceFullName(),
						" into expected type ",
						ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName(),
						". Value lost for node '",
						text,
						"'."
					}));
					return default(T);
				}
				break;
			}
			case EntryType.ExternalReferenceByGuid:
				break;
			case EntryType.ExternalReferenceByString:
				goto IL_5E9;
			default:
				goto IL_A8B;
			}
			Guid guid2;
			reader.ReadExternalReference(out guid2);
			object externalObject2 = context.GetExternalObject(guid2);
			try
			{
				return (T)((object)externalObject2);
			}
			catch (InvalidCastException)
			{
				context.Config.DebugContext.LogWarning(string.Concat(new string[]
				{
					"Can't cast external reference type ",
					externalObject2.GetType().GetNiceFullName(),
					" into expected type ",
					ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName(),
					". Value lost for node '",
					text,
					"'."
				}));
				return default(T);
			}
			IL_5E9:
			string id;
			reader.ReadExternalReference(out id);
			object externalObject3 = context.GetExternalObject(id);
			try
			{
				return (T)((object)externalObject3);
			}
			catch (InvalidCastException)
			{
				context.Config.DebugContext.LogWarning(string.Concat(new string[]
				{
					"Can't cast external reference type ",
					externalObject3.GetType().GetNiceFullName(),
					" into expected type ",
					ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName(),
					". Value lost for node '",
					text,
					"'."
				}));
				return default(T);
			}
			IL_677:
			int id2;
			reader.ReadInternalReference(out id2);
			object internalReference = context.GetInternalReference(id2);
			try
			{
				return (T)((object)internalReference);
			}
			catch (InvalidCastException)
			{
				context.Config.DebugContext.LogWarning(string.Concat(new string[]
				{
					"Can't cast internal reference type ",
					internalReference.GetType().GetNiceFullName(),
					" into expected type ",
					ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName(),
					". Value lost for node '",
					text,
					"'."
				}));
				return default(T);
			}
			IL_705:
			try
			{
				Type typeOf_T2 = ComplexTypeSerializer<T>.TypeOf_T;
				Type type2;
				if (reader.EnterNode(out type2))
				{
					int currentNodeId = reader.CurrentNodeId;
					T t;
					if (type2 != null && typeOf_T2 != type2)
					{
						bool flag3 = false;
						bool flag4 = FormatterUtilities.IsPrimitiveType(type2);
						if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType && flag4)
						{
							Serializer serializer = Serializer.Get(type2);
							t = (T)((object)serializer.ReadValueWeak(reader));
							flag3 = true;
						}
						else
						{
							bool flag5;
							if ((flag5 = typeOf_T2.IsAssignableFrom(type2)) || type2.HasCastDefined(typeOf_T2, false))
							{
								try
								{
									object obj2;
									if (flag4)
									{
										Serializer serializer2 = Serializer.Get(type2);
										obj2 = serializer2.ReadValueWeak(reader);
									}
									else
									{
										IFormatter formatter = FormatterLocator.GetFormatter(type2, context.Config.SerializationPolicy);
										obj2 = formatter.Deserialize(reader);
									}
									if (flag5)
									{
										t = (T)((object)obj2);
									}
									else
									{
										Func<object, object> castMethodDelegate = type2.GetCastMethodDelegate(typeOf_T2, false);
										if (castMethodDelegate != null)
										{
											t = (T)((object)castMethodDelegate.Invoke(obj2));
										}
										else
										{
											t = (T)((object)obj2);
										}
									}
									flag3 = true;
									goto IL_8EA;
								}
								catch (SerializationAbortException ex2)
								{
									flag = false;
									throw ex2;
								}
								catch (InvalidCastException)
								{
									flag3 = false;
									t = default(T);
									goto IL_8EA;
								}
							}
							if (!ComplexTypeSerializer<T>.ComplexTypeIsAbstract && (ComplexTypeSerializer<T>.AllowDeserializeInvalidDataForT || reader.Context.Config.AllowDeserializeInvalidData))
							{
								context.Config.DebugContext.LogWarning(string.Concat(new string[]
								{
									"Can't cast serialized type ",
									type2.GetNiceFullName(),
									" into expected type ",
									typeOf_T2.GetNiceFullName(),
									". Attempting to deserialize with invalid data. Value may be lost or corrupted for node '",
									text,
									"'."
								}));
								t = ComplexTypeSerializer<T>.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
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
								t = default(T);
							}
						}
						IL_8EA:
						if (!flag3)
						{
							context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Can't cast serialized type ",
								type2.GetNiceFullName(),
								" into expected type ",
								typeOf_T2.GetNiceFullName(),
								". Value lost for node '",
								text,
								"'."
							}));
							t = default(T);
						}
					}
					else if (ComplexTypeSerializer<T>.ComplexTypeIsAbstract)
					{
						t = default(T);
					}
					else
					{
						t = ComplexTypeSerializer<T>.GetBaseFormatter(context.Config.SerializationPolicy).Deserialize(reader);
					}
					if (currentNodeId >= 0)
					{
						context.RegisterInternalReference(currentNodeId, t);
					}
					return t;
				}
				context.Config.DebugContext.LogError("Failed to enter node '" + text + "'.");
				return default(T);
			}
			catch (SerializationAbortException ex3)
			{
				flag = false;
				throw ex3;
			}
			catch (Exception exception2)
			{
				context.Config.DebugContext.LogException(exception2);
				return default(T);
			}
			finally
			{
				if (flag)
				{
					reader.ExitNode();
				}
			}
			IL_9FC:
			if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType)
			{
				bool flag6;
				reader.ReadBoolean(out flag6);
				return (T)((object)flag6);
			}
			IL_A8B:
			context.Config.DebugContext.LogWarning("Unexpected entry of type " + entryType.ToString() + ", when a reference or node start was expected. A value has been lost.");
			reader.SkipEntry();
			return default(T);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001D8EC File Offset: 0x0001BAEC
		private static IFormatter<T> GetBaseFormatter(ISerializationPolicy serializationPolicy)
		{
			if (serializationPolicy == ComplexTypeSerializer<T>.UnityPolicy)
			{
				if (ComplexTypeSerializer<T>.UnityPolicyFormatter == null)
				{
					ComplexTypeSerializer<T>.UnityPolicyFormatter = FormatterLocator.GetFormatter<T>(ComplexTypeSerializer<T>.UnityPolicy);
				}
				return ComplexTypeSerializer<T>.UnityPolicyFormatter;
			}
			if (serializationPolicy == ComplexTypeSerializer<T>.EverythingPolicy)
			{
				if (ComplexTypeSerializer<T>.EverythingPolicyFormatter == null)
				{
					ComplexTypeSerializer<T>.EverythingPolicyFormatter = FormatterLocator.GetFormatter<T>(ComplexTypeSerializer<T>.EverythingPolicy);
				}
				return ComplexTypeSerializer<T>.EverythingPolicyFormatter;
			}
			if (serializationPolicy == ComplexTypeSerializer<T>.StrictPolicy)
			{
				if (ComplexTypeSerializer<T>.StrictPolicyFormatter == null)
				{
					ComplexTypeSerializer<T>.StrictPolicyFormatter = FormatterLocator.GetFormatter<T>(ComplexTypeSerializer<T>.StrictPolicy);
				}
				return ComplexTypeSerializer<T>.StrictPolicyFormatter;
			}
			object formattersByPolicy_LOCK = ComplexTypeSerializer<T>.FormattersByPolicy_LOCK;
			IFormatter<T> formatter;
			lock (formattersByPolicy_LOCK)
			{
				if (!ComplexTypeSerializer<T>.FormattersByPolicy.TryGetValue(serializationPolicy, ref formatter))
				{
					formatter = FormatterLocator.GetFormatter<T>(serializationPolicy);
					ComplexTypeSerializer<T>.FormattersByPolicy.Add(serializationPolicy, formatter);
				}
			}
			return formatter;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001D9B4 File Offset: 0x0001BBB4
		public override void WriteValue(string name, T value, IDataWriter writer)
		{
			SerializationContext context = writer.Context;
			ISerializationPolicy serializationPolicy = context.Config.SerializationPolicy;
			if (!serializationPolicy.AllowNonSerializableTypes && !ComplexTypeSerializer<T>.TypeOf_T.IsSerializable)
			{
				context.Config.DebugContext.LogError("The type " + ComplexTypeSerializer<T>.TypeOf_T.GetNiceFullName() + " is not marked as serializable.");
				return;
			}
			if (ComplexTypeSerializer<T>.ComplexTypeIsValueType)
			{
				bool flag = true;
				try
				{
					writer.BeginStructNode(name, ComplexTypeSerializer<T>.TypeOf_T);
					ComplexTypeSerializer<T>.GetBaseFormatter(serializationPolicy).Serialize(value, writer);
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
				if (ComplexTypeSerializer<T>.ComplexTypeMayBeBoxedValueType && FormatterUtilities.IsPrimitiveType(type))
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
				if (type == ComplexTypeSerializer<T>.TypeOf_T)
				{
					formatter = ComplexTypeSerializer<T>.GetBaseFormatter(serializationPolicy);
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

		// Token: 0x04000174 RID: 372
		private static readonly bool ComplexTypeMayBeBoxedValueType = typeof(T).IsInterface || typeof(T) == typeof(object) || typeof(T) == typeof(ValueType) || typeof(T) == typeof(Enum);

		// Token: 0x04000175 RID: 373
		private static readonly bool ComplexTypeIsAbstract = typeof(T).IsAbstract || typeof(T).IsInterface;

		// Token: 0x04000176 RID: 374
		private static readonly bool ComplexTypeIsNullable = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable);

		// Token: 0x04000177 RID: 375
		private static readonly bool ComplexTypeIsValueType = typeof(T).IsValueType;

		// Token: 0x04000178 RID: 376
		private static readonly Type TypeOf_T = typeof(T);

		// Token: 0x04000179 RID: 377
		private static readonly bool AllowDeserializeInvalidDataForT = typeof(T).IsDefined(typeof(AllowDeserializeInvalidDataAttribute), true);

		// Token: 0x0400017A RID: 378
		private static readonly Dictionary<ISerializationPolicy, IFormatter<T>> FormattersByPolicy = new Dictionary<ISerializationPolicy, IFormatter<T>>(ReferenceEqualityComparer<ISerializationPolicy>.Default);

		// Token: 0x0400017B RID: 379
		private static readonly object FormattersByPolicy_LOCK = new object();

		// Token: 0x0400017C RID: 380
		private static readonly ISerializationPolicy UnityPolicy = SerializationPolicies.Unity;

		// Token: 0x0400017D RID: 381
		private static readonly ISerializationPolicy StrictPolicy = SerializationPolicies.Strict;

		// Token: 0x0400017E RID: 382
		private static readonly ISerializationPolicy EverythingPolicy = SerializationPolicies.Everything;

		// Token: 0x0400017F RID: 383
		private static IFormatter<T> UnityPolicyFormatter;

		// Token: 0x04000180 RID: 384
		private static IFormatter<T> StrictPolicyFormatter;

		// Token: 0x04000181 RID: 385
		private static IFormatter<T> EverythingPolicyFormatter;
	}
}

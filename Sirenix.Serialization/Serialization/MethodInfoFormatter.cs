using System;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x0200003E RID: 62
	public class MethodInfoFormatter<T> : BaseFormatter<T> where T : MethodInfo
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x00013A48 File Offset: 0x00011C48
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			string text;
			EntryType entryType = reader.PeekEntry(out text);
			if (entryType == EntryType.StartOfArray)
			{
				IFormatter formatter = new WeakSerializableFormatter(typeof(T));
				value = (T)((object)formatter.Deserialize(reader));
				return;
			}
			Type type = null;
			string text2 = null;
			Type[] array = null;
			Type[] array2 = null;
			while ((entryType = reader.PeekEntry(out text)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
			{
				if (!(text == "declaringType"))
				{
					if (!(text == "methodName"))
					{
						if (!(text == "signature"))
						{
							if (!(text == "genericArguments"))
							{
								reader.SkipEntry();
							}
							else
							{
								array2 = MethodInfoFormatter<T>.TypeArraySerializer.ReadValue(reader);
							}
						}
						else
						{
							array = MethodInfoFormatter<T>.TypeArraySerializer.ReadValue(reader);
						}
					}
					else
					{
						text2 = MethodInfoFormatter<T>.StringSerializer.ReadValue(reader);
					}
				}
				else
				{
					Type type2 = MethodInfoFormatter<T>.TypeSerializer.ReadValue(reader);
					if (type2 != null)
					{
						type = type2;
					}
				}
			}
			if (type == null)
			{
				reader.Context.Config.DebugContext.LogWarning("Missing declaring type of MethodInfo on deserialize.");
				return;
			}
			if (text2 == null)
			{
				reader.Context.Config.DebugContext.LogError("Missing method name of MethodInfo on deserialize.");
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (array != null)
			{
				flag = true;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == null)
					{
						flag = false;
						break;
					}
				}
			}
			MethodInfo methodInfo;
			if (flag)
			{
				try
				{
					methodInfo = type.GetMethod(text2, 124, null, array, null);
					goto IL_189;
				}
				catch (AmbiguousMatchException)
				{
					methodInfo = null;
					flag2 = true;
					goto IL_189;
				}
			}
			try
			{
				methodInfo = type.GetMethod(text2, 124);
			}
			catch (AmbiguousMatchException)
			{
				methodInfo = null;
				flag2 = true;
			}
			IL_189:
			if (!(methodInfo == null))
			{
				if (methodInfo.IsGenericMethodDefinition)
				{
					if (array2 == null)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Method '",
							type.GetNiceFullName(),
							".",
							methodInfo.GetNiceName(),
							"' to deserialize is a generic method definition, but no generic arguments were in the serialization data."
						}));
						return;
					}
					int num = methodInfo.GetGenericArguments().Length;
					if (array2.Length != num)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Method '",
							type.GetNiceFullName(),
							".",
							methodInfo.GetNiceName(),
							"' to deserialize is a generic method definition, but there is the wrong number of generic arguments in the serialization data."
						}));
						return;
					}
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[j] == null)
						{
							reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Method '",
								type.GetNiceFullName(),
								".",
								methodInfo.GetNiceName(),
								"' to deserialize is a generic method definition, but one of the serialized generic argument types failed to bind on deserialization."
							}));
							return;
						}
					}
					try
					{
						methodInfo = methodInfo.MakeGenericMethod(array2);
					}
					catch (Exception ex)
					{
						DebugContext debugContext = reader.Context.Config.DebugContext;
						string[] array3 = new string[10];
						array3[0] = "Method '";
						array3[1] = type.GetNiceFullName();
						array3[2] = ".";
						array3[3] = methodInfo.GetNiceName();
						array3[4] = "' to deserialize is a generic method definition, but failed to create generic method from definition, using generic arguments '";
						array3[5] = string.Join(", ", (from p in array2
						select p.GetNiceFullName()).ToArray<string>());
						array3[6] = "'. Method creation failed with an exception of type ";
						array3[7] = ex.GetType().GetNiceFullName();
						array3[8] = ", with the message: ";
						array3[9] = ex.Message;
						debugContext.LogWarning(string.Concat(array3));
						return;
					}
				}
				try
				{
					value = (T)((object)methodInfo);
				}
				catch (InvalidCastException)
				{
					reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
					{
						"The serialized method '",
						type.GetNiceFullName(),
						".",
						methodInfo.GetNiceName(),
						"' was successfully resolved into a MethodInfo reference of the runtime type '",
						methodInfo.GetType().GetNiceFullName(),
						"', but failed to be cast to expected type '",
						typeof(T).GetNiceFullName(),
						"'."
					}));
					return;
				}
				base.RegisterReferenceID(value, reader);
				return;
			}
			if (flag)
			{
				DebugContext debugContext2 = reader.Context.Config.DebugContext;
				string[] array4 = new string[8];
				array4[0] = "Could not find method with signature ";
				array4[1] = text;
				array4[2] = "(";
				array4[3] = string.Join(", ", (from p in array
				select p.GetNiceFullName()).ToArray<string>());
				array4[4] = ") on type '";
				array4[5] = type.FullName;
				array4[6] = (flag2 ? "; resolution was ambiguous between multiple methods" : string.Empty);
				array4[7] = ".";
				debugContext2.LogWarning(string.Concat(array4));
				return;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Could not find method with name ",
				text,
				" on type '",
				type.GetNiceFullName(),
				flag2 ? "; resolution was ambiguous between multiple methods" : string.Empty,
				"."
			}));
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00013FA8 File Offset: 0x000121A8
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			MethodInfo methodInfo = value;
			if (methodInfo.GetType().Name.Contains("DynamicMethod"))
			{
				DebugContext debugContext = writer.Context.Config.DebugContext;
				string text = "Cannot serialize a dynamically emitted method ";
				MethodInfo methodInfo2 = methodInfo;
				debugContext.LogWarning(text + ((methodInfo2 != null) ? methodInfo2.ToString() : null) + ".");
				return;
			}
			if (methodInfo.IsGenericMethodDefinition)
			{
				writer.Context.Config.DebugContext.LogWarning("Serializing a MethodInfo for a generic method definition '" + methodInfo.GetNiceName() + "' is not currently supported.");
				return;
			}
			MethodInfoFormatter<T>.TypeSerializer.WriteValue("declaringType", methodInfo.DeclaringType, writer);
			MethodInfoFormatter<T>.StringSerializer.WriteValue("methodName", methodInfo.Name, writer);
			ParameterInfo[] parameters;
			if (methodInfo.IsGenericMethod)
			{
				parameters = methodInfo.GetGenericMethodDefinition().GetParameters();
			}
			else
			{
				parameters = methodInfo.GetParameters();
			}
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			MethodInfoFormatter<T>.TypeArraySerializer.WriteValue("signature", array, writer);
			if (methodInfo.IsGenericMethod)
			{
				Type[] genericArguments = methodInfo.GetGenericArguments();
				MethodInfoFormatter<T>.TypeArraySerializer.WriteValue("genericArguments", genericArguments, writer);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000140DC File Offset: 0x000122DC
		protected override T GetUninitializedObject()
		{
			return default(T);
		}

		// Token: 0x040000D5 RID: 213
		private static readonly Serializer<string> StringSerializer = Serializer.Get<string>();

		// Token: 0x040000D6 RID: 214
		private static readonly Serializer<Type> TypeSerializer = Serializer.Get<Type>();

		// Token: 0x040000D7 RID: 215
		private static readonly Serializer<Type[]> TypeArraySerializer = Serializer.Get<Type[]>();
	}
}

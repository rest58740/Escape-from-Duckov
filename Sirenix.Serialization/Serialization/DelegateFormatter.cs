using System;
using System.Linq;
using System.Reflection;
using Sirenix.Serialization.Utilities;
using UnityEngine;

namespace Sirenix.Serialization
{
	// Token: 0x02000028 RID: 40
	public class DelegateFormatter<T> : BaseFormatter<T> where T : class
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000FEA2 File Offset: 0x0000E0A2
		public DelegateFormatter() : this(typeof(T))
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000FEB4 File Offset: 0x0000E0B4
		public DelegateFormatter(Type delegateType)
		{
			if (!typeof(Delegate).IsAssignableFrom(delegateType))
			{
				throw new ArgumentException("The type " + ((delegateType != null) ? delegateType.ToString() : null) + " is not a delegate.");
			}
			this.DelegateType = delegateType;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000FF04 File Offset: 0x0000E104
		protected override void DeserializeImplementation(ref T value, IDataReader reader)
		{
			Type type = this.DelegateType;
			Type type2 = null;
			object obj = null;
			string text = null;
			Type[] array = null;
			Type[] array2 = null;
			Delegate[] array3 = null;
			string text2;
			EntryType entryType;
			while ((entryType = reader.PeekEntry(out text2)) != EntryType.EndOfNode && entryType != EntryType.EndOfArray && entryType != EntryType.EndOfStream)
			{
				if (text2 != null)
				{
					switch (text2.Length)
					{
					case 6:
						if (text2 == "target")
						{
							obj = DelegateFormatter<T>.ObjectSerializer.ReadValue(reader);
							continue;
						}
						break;
					case 9:
						if (text2 == "signature")
						{
							array = DelegateFormatter<T>.TypeArraySerializer.ReadValue(reader);
							continue;
						}
						break;
					case 10:
						if (text2 == "methodName")
						{
							text = DelegateFormatter<T>.StringSerializer.ReadValue(reader);
							continue;
						}
						break;
					case 12:
						if (text2 == "delegateType")
						{
							Type type3 = DelegateFormatter<T>.TypeSerializer.ReadValue(reader);
							if (type3 != null)
							{
								type = type3;
								continue;
							}
							continue;
						}
						break;
					case 13:
						if (text2 == "declaringType")
						{
							Type type4 = DelegateFormatter<T>.TypeSerializer.ReadValue(reader);
							if (type4 != null)
							{
								type2 = type4;
								continue;
							}
							continue;
						}
						break;
					case 14:
						if (text2 == "invocationList")
						{
							array3 = DelegateFormatter<T>.DelegateArraySerializer.ReadValue(reader);
							continue;
						}
						break;
					case 16:
						if (text2 == "genericArguments")
						{
							array2 = DelegateFormatter<T>.TypeArraySerializer.ReadValue(reader);
							continue;
						}
						break;
					}
				}
				reader.SkipEntry();
			}
			if (array3 != null)
			{
				Delegate @delegate = null;
				try
				{
					@delegate = Delegate.Combine(array3);
				}
				catch (Exception ex)
				{
					reader.Context.Config.DebugContext.LogError("Recombining delegate invocation list upon deserialization failed with an exception of type " + ex.GetType().GetNiceFullName() + " with the message: " + ex.Message);
				}
				if (@delegate != null)
				{
					try
					{
						value = (T)((object)@delegate);
					}
					catch (InvalidCastException)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Could not cast recombined delegate of type ",
							@delegate.GetType().GetNiceFullName(),
							" to expected delegate type ",
							this.DelegateType.GetNiceFullName(),
							"."
						}));
					}
				}
				return;
			}
			if (type2 == null)
			{
				reader.Context.Config.DebugContext.LogWarning("Missing declaring type of delegate on deserialize.");
				return;
			}
			if (text == null)
			{
				reader.Context.Config.DebugContext.LogError("Missing method name of delegate on deserialize.");
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
					methodInfo = type2.GetMethod(text, 124, null, array, null);
					goto IL_2EE;
				}
				catch (AmbiguousMatchException)
				{
					methodInfo = null;
					flag2 = true;
					goto IL_2EE;
				}
			}
			try
			{
				methodInfo = type2.GetMethod(text, 124);
			}
			catch (AmbiguousMatchException)
			{
				methodInfo = null;
				flag2 = true;
			}
			IL_2EE:
			if (!(methodInfo == null))
			{
				if (methodInfo.IsGenericMethodDefinition)
				{
					if (array2 == null)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Method '",
							type2.GetNiceFullName(),
							".",
							methodInfo.GetNiceName(),
							"' of delegate to deserialize is a generic method definition, but no generic arguments were in the serialization data."
						}));
						return;
					}
					int num = methodInfo.GetGenericArguments().Length;
					if (array2.Length != num)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Method '",
							type2.GetNiceFullName(),
							".",
							methodInfo.GetNiceName(),
							"' of delegate to deserialize is a generic method definition, but there is the wrong number of generic arguments in the serialization data."
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
								type2.GetNiceFullName(),
								".",
								methodInfo.GetNiceName(),
								"' of delegate to deserialize is a generic method definition, but one of the serialized generic argument types failed to bind on deserialization."
							}));
							return;
						}
					}
					try
					{
						methodInfo = methodInfo.MakeGenericMethod(array2);
					}
					catch (Exception ex2)
					{
						DebugContext debugContext = reader.Context.Config.DebugContext;
						string[] array4 = new string[10];
						array4[0] = "Method '";
						array4[1] = type2.GetNiceFullName();
						array4[2] = ".";
						array4[3] = methodInfo.GetNiceName();
						array4[4] = "' of delegate to deserialize is a generic method definition, but failed to create generic method from definition, using generic arguments '";
						array4[5] = string.Join(", ", (from p in array2
						select p.GetNiceFullName()).ToArray<string>());
						array4[6] = "'. Method creation failed with an exception of type ";
						array4[7] = ex2.GetType().GetNiceFullName();
						array4[8] = ", with the message: ";
						array4[9] = ex2.Message;
						debugContext.LogWarning(string.Concat(array4));
						return;
					}
				}
				if (methodInfo.IsStatic)
				{
					value = (T)((object)Delegate.CreateDelegate(type, methodInfo, false));
				}
				else
				{
					Type declaringType = methodInfo.DeclaringType;
					if (typeof(Object).IsAssignableFrom(declaringType))
					{
						if (obj as Object == null)
						{
							reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
							{
								"Method '",
								type2.GetNiceFullName(),
								".",
								methodInfo.GetNiceName(),
								"' of delegate to deserialize is an instance method, but Unity object target of type '",
								declaringType.GetNiceFullName(),
								"' was null on deserialization. Did something destroy it, or did you apply a delegate value targeting a scene-based UnityEngine.Object instance to a prefab?"
							}));
							return;
						}
					}
					else if (obj == null)
					{
						reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
						{
							"Method '",
							type2.GetNiceFullName(),
							".",
							methodInfo.GetNiceName(),
							"' of delegate to deserialize is an instance method, but no valid instance target of type '",
							declaringType.GetNiceFullName(),
							"' was in the serialization data. Has something been renamed since serialization?"
						}));
						return;
					}
					value = (T)((object)Delegate.CreateDelegate(type, obj, methodInfo, false));
				}
				if (value == null)
				{
					reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
					{
						"Failed to create delegate of type ",
						type.GetNiceFullName(),
						" from method '",
						type2.GetNiceFullName(),
						".",
						methodInfo.GetNiceName(),
						"'."
					}));
					return;
				}
				base.RegisterReferenceID(value, reader);
				base.InvokeOnDeserializingCallbacks(ref value, reader.Context);
				return;
			}
			if (flag)
			{
				DebugContext debugContext2 = reader.Context.Config.DebugContext;
				string[] array5 = new string[8];
				array5[0] = "Could not find method with signature ";
				array5[1] = text2;
				array5[2] = "(";
				array5[3] = string.Join(", ", (from p in array
				select p.GetNiceFullName()).ToArray<string>());
				array5[4] = ") on type '";
				array5[5] = type2.FullName;
				array5[6] = (flag2 ? "; resolution was ambiguous between multiple methods" : string.Empty);
				array5[7] = ".";
				debugContext2.LogWarning(string.Concat(array5));
				return;
			}
			reader.Context.Config.DebugContext.LogWarning(string.Concat(new string[]
			{
				"Could not find method with name ",
				text2,
				" on type '",
				type2.GetNiceFullName(),
				flag2 ? "; resolution was ambiguous between multiple methods" : string.Empty,
				"."
			}));
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000106DC File Offset: 0x0000E8DC
		protected override void SerializeImplementation(ref T value, IDataWriter writer)
		{
			Delegate @delegate = (Delegate)((object)value);
			Delegate[] invocationList = @delegate.GetInvocationList();
			if (invocationList.Length > 1)
			{
				DelegateFormatter<T>.DelegateArraySerializer.WriteValue("invocationList", invocationList, writer);
				return;
			}
			MethodInfo method = @delegate.Method;
			if (method.GetType().Name.Contains("DynamicMethod"))
			{
				DebugContext debugContext = writer.Context.Config.DebugContext;
				string text = "Cannot serialize delegate made from dynamically emitted method ";
				MethodInfo methodInfo = method;
				debugContext.LogError(text + ((methodInfo != null) ? methodInfo.ToString() : null) + ".");
				return;
			}
			if (method.IsGenericMethodDefinition)
			{
				DebugContext debugContext2 = writer.Context.Config.DebugContext;
				string text2 = "Cannot serialize delegate made from the unresolved generic method definition ";
				MethodInfo methodInfo2 = method;
				debugContext2.LogError(text2 + ((methodInfo2 != null) ? methodInfo2.ToString() : null) + "; how did this even happen? It should not even be possible to have a delegate for a generic method definition that hasn't been turned into a generic method yet.");
				return;
			}
			if (@delegate.Target != null)
			{
				DelegateFormatter<T>.ObjectSerializer.WriteValue("target", @delegate.Target, writer);
			}
			DelegateFormatter<T>.TypeSerializer.WriteValue("declaringType", method.DeclaringType, writer);
			DelegateFormatter<T>.StringSerializer.WriteValue("methodName", method.Name, writer);
			DelegateFormatter<T>.TypeSerializer.WriteValue("delegateType", @delegate.GetType(), writer);
			ParameterInfo[] parameters;
			if (method.IsGenericMethod)
			{
				parameters = method.GetGenericMethodDefinition().GetParameters();
			}
			else
			{
				parameters = method.GetParameters();
			}
			Type[] array = new Type[parameters.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = parameters[i].ParameterType;
			}
			DelegateFormatter<T>.TypeArraySerializer.WriteValue("signature", array, writer);
			if (method.IsGenericMethod)
			{
				Type[] genericArguments = method.GetGenericArguments();
				DelegateFormatter<T>.TypeArraySerializer.WriteValue("genericArguments", genericArguments, writer);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00010880 File Offset: 0x0000EA80
		protected override T GetUninitializedObject()
		{
			return default(T);
		}

		// Token: 0x040000A1 RID: 161
		private static readonly Serializer<object> ObjectSerializer = Serializer.Get<object>();

		// Token: 0x040000A2 RID: 162
		private static readonly Serializer<string> StringSerializer = Serializer.Get<string>();

		// Token: 0x040000A3 RID: 163
		private static readonly Serializer<Type> TypeSerializer = Serializer.Get<Type>();

		// Token: 0x040000A4 RID: 164
		private static readonly Serializer<Type[]> TypeArraySerializer = Serializer.Get<Type[]>();

		// Token: 0x040000A5 RID: 165
		private static readonly Serializer<Delegate[]> DelegateArraySerializer = Serializer.Get<Delegate[]>();

		// Token: 0x040000A6 RID: 166
		public readonly Type DelegateType;
	}
}

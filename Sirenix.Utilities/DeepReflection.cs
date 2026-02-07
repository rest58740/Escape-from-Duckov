using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sirenix.Utilities
{
	// Token: 0x02000013 RID: 19
	public static class DeepReflection
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00007888 File Offset: 0x00005A88
		public static Func<object> CreateWeakStaticValueGetter(Type rootType, Type resultType, string path, bool allowEmit = true)
		{
			if (rootType == null)
			{
				throw new ArgumentNullException("rootType");
			}
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(rootType, ref resultType, path, out flag, false);
			if (!flag)
			{
				throw new ArgumentException("Given path root is not static.");
			}
			if (!allowEmit)
			{
				return DeepReflection.CreateSlowDeepStaticValueGetterDelegate(memberPath);
			}
			Delegate @delegate = DeepReflection.CreateEmittedDeepValueGetterDelegate(path, rootType, resultType, memberPath, flag);
			MethodInfo methodInfo = DeepReflection.CreateWeakAliasForStaticGetDelegateMethodInfo.MakeGenericMethod(new Type[]
			{
				resultType
			});
			return (Func<object>)methodInfo.Invoke(null, new object[]
			{
				@delegate
			});
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00007908 File Offset: 0x00005B08
		public static Func<object, object> CreateWeakInstanceValueGetter(Type rootType, Type resultType, string path, bool allowEmit = true)
		{
			if (rootType == null)
			{
				throw new ArgumentNullException("rootType");
			}
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(rootType, ref resultType, path, out flag, false);
			if (flag)
			{
				throw new ArgumentException("Given path root is static.");
			}
			if (!allowEmit)
			{
				return DeepReflection.CreateSlowDeepInstanceValueGetterDelegate(memberPath);
			}
			Delegate @delegate = DeepReflection.CreateEmittedDeepValueGetterDelegate(path, rootType, resultType, memberPath, flag);
			MethodInfo methodInfo = DeepReflection.CreateWeakAliasForInstanceGetDelegate1MethodInfo.MakeGenericMethod(new Type[]
			{
				rootType,
				resultType
			});
			return (Func<object, object>)methodInfo.Invoke(null, new object[]
			{
				@delegate
			});
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000798C File Offset: 0x00005B8C
		public static Action<object, object> CreateWeakInstanceValueSetter(Type rootType, Type argType, string path, bool allowEmit = true)
		{
			if (rootType == null)
			{
				throw new ArgumentNullException("rootType");
			}
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(rootType, ref argType, path, out flag, true);
			if (flag)
			{
				throw new ArgumentException("Given path root is static.");
			}
			if (!false)
			{
				return DeepReflection.CreateSlowDeepInstanceValueSetterDelegate(memberPath);
			}
			Delegate @delegate = null;
			MethodInfo methodInfo = DeepReflection.CreateWeakAliasForInstanceSetDelegate1MethodInfo.MakeGenericMethod(new Type[]
			{
				rootType,
				argType
			});
			return (Action<object, object>)methodInfo.Invoke(null, new object[]
			{
				@delegate
			});
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00007A0C File Offset: 0x00005C0C
		public static Func<object, TResult> CreateWeakInstanceValueGetter<TResult>(Type rootType, string path, bool allowEmit = true)
		{
			if (rootType == null)
			{
				throw new ArgumentNullException("rootType");
			}
			Type typeFromHandle = typeof(TResult);
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(rootType, ref typeFromHandle, path, out flag, false);
			if (flag)
			{
				throw new ArgumentException("Given path root is static.");
			}
			if (!allowEmit)
			{
				Func<object, object> del = DeepReflection.CreateSlowDeepInstanceValueGetterDelegate(memberPath);
				return (object obj) => (TResult)((object)del.Invoke(obj));
			}
			Delegate @delegate = DeepReflection.CreateEmittedDeepValueGetterDelegate(path, rootType, typeFromHandle, memberPath, flag);
			MethodInfo methodInfo = DeepReflection.CreateWeakAliasForInstanceGetDelegate2MethodInfo.MakeGenericMethod(new Type[]
			{
				rootType,
				typeFromHandle
			});
			return (Func<object, TResult>)methodInfo.Invoke(null, new object[]
			{
				@delegate
			});
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public static Func<TResult> CreateValueGetter<TResult>(Type rootType, string path, bool allowEmit = true)
		{
			if (rootType == null)
			{
				throw new ArgumentNullException("rootType");
			}
			Type typeFromHandle = typeof(TResult);
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(rootType, ref typeFromHandle, path, out flag, false);
			if (!flag)
			{
				throw new ArgumentException("Given path root is not static; use the generic overload with a target type.");
			}
			if (!allowEmit)
			{
				Func<object> slowDelegate = DeepReflection.CreateSlowDeepStaticValueGetterDelegate(memberPath);
				return () => (TResult)((object)slowDelegate.Invoke());
			}
			Delegate @delegate = DeepReflection.CreateEmittedDeepValueGetterDelegate(path, rootType, typeFromHandle, memberPath, flag);
			return (Func<TResult>)@delegate;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00007B38 File Offset: 0x00005D38
		public static Func<TTarget, TResult> CreateValueGetter<TTarget, TResult>(string path, bool allowEmit = true)
		{
			Type typeFromHandle = typeof(TResult);
			bool flag;
			List<DeepReflection.PathStep> memberPath = DeepReflection.GetMemberPath(typeof(TTarget), ref typeFromHandle, path, out flag, false);
			if (flag)
			{
				throw new ArgumentException("Given path root is static; use the generic overload without a target type.");
			}
			if (!allowEmit)
			{
				Func<object, object> slowDelegate = DeepReflection.CreateSlowDeepInstanceValueGetterDelegate(memberPath);
				return (TTarget target) => (TResult)((object)slowDelegate.Invoke(target));
			}
			Delegate @delegate = DeepReflection.CreateEmittedDeepValueGetterDelegate(path, typeof(TTarget), typeFromHandle, memberPath, flag);
			return (Func<TTarget, TResult>)@delegate;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00007BB4 File Offset: 0x00005DB4
		private static Func<object, object> CreateWeakAliasForInstanceGetDelegate1<TTarget, TResult>(Func<TTarget, TResult> func)
		{
			return (object obj) => func.Invoke((TTarget)((object)obj));
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00007BDC File Offset: 0x00005DDC
		private static Func<object, TResult> CreateWeakAliasForInstanceGetDelegate2<TTarget, TResult>(Func<TTarget, TResult> func)
		{
			return (object obj) => func.Invoke((TTarget)((object)obj));
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00007C04 File Offset: 0x00005E04
		private static Func<object> CreateWeakAliasForStaticGetDelegate<TResult>(Func<TResult> func)
		{
			return () => func.Invoke();
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00007C2C File Offset: 0x00005E2C
		private static Action<object, object> CreateWeakAliasForInstanceSetDelegate1<TTarget, TArg1>(Action<TTarget, TArg1> func)
		{
			return delegate(object obj, object arg)
			{
				func.Invoke((TTarget)((object)obj), (TArg1)((object)arg));
			};
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00007C54 File Offset: 0x00005E54
		private static Action<object, TArg1> CreateWeakAliasForInstanceSetDelegate2<TTarget, TArg1>(Action<TTarget, TArg1> func)
		{
			return delegate(object obj, TArg1 arg)
			{
				func.Invoke((TTarget)((object)obj), arg);
			};
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00007C7C File Offset: 0x00005E7C
		private static Action<object> CreateWeakAliasForStaticSetDelegate<TArg1>(Action<TArg1> func)
		{
			return delegate(object arg)
			{
				func.Invoke((TArg1)((object)arg));
			};
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00007CA4 File Offset: 0x00005EA4
		private static Delegate CreateEmittedDeepValueGetterDelegate(string path, Type rootType, Type resultType, List<DeepReflection.PathStep> memberPath, bool rootIsStatic)
		{
			DynamicMethod dynamicMethod;
			if (rootIsStatic)
			{
				dynamicMethod = new DynamicMethod(rootType.FullName + "_getter<" + path + ">", resultType, new Type[0], true);
			}
			else
			{
				dynamicMethod = new DynamicMethod(rootType.FullName + "_getter<" + path + ">", resultType, new Type[]
				{
					rootType
				}, true);
			}
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (!rootIsStatic)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
			}
			for (int i = 0; i < memberPath.Count; i++)
			{
				DeepReflection.PathStep pathStep = memberPath[i];
				switch (pathStep.StepType)
				{
				case DeepReflection.PathStepType.Member:
				{
					MemberInfo member = pathStep.Member;
					FieldInfo fieldInfo = member as FieldInfo;
					if (fieldInfo != null)
					{
						if (fieldInfo.IsLiteral)
						{
							DeepReflection.EmitConstant(ilgenerator, fieldInfo.GetRawConstantValue(), null);
						}
						else if (fieldInfo.IsStatic)
						{
							ilgenerator.Emit(OpCodes.Ldsfld, fieldInfo);
						}
						else
						{
							ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
						}
					}
					PropertyInfo propertyInfo = member as PropertyInfo;
					if (propertyInfo != null)
					{
						MethodInfo getMethod = propertyInfo.GetGetMethod(true);
						if (getMethod.IsStatic)
						{
							ilgenerator.Emit(OpCodes.Call, getMethod);
						}
						else if (getMethod.DeclaringType.IsValueType)
						{
							LocalBuilder localBuilder = ilgenerator.DeclareLocal(getMethod.DeclaringType);
							ilgenerator.Emit(OpCodes.Stloc, localBuilder);
							ilgenerator.Emit(OpCodes.Ldloca, localBuilder);
							ilgenerator.Emit(OpCodes.Call, getMethod);
						}
						else
						{
							ilgenerator.Emit(OpCodes.Callvirt, getMethod);
						}
					}
					MethodInfo methodInfo = member as MethodInfo;
					if (methodInfo != null)
					{
						if (methodInfo.IsStatic)
						{
							ilgenerator.Emit(OpCodes.Call, methodInfo);
						}
						else if (methodInfo.DeclaringType.IsValueType)
						{
							LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(methodInfo.DeclaringType);
							ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
							ilgenerator.Emit(OpCodes.Ldloca, localBuilder2);
							ilgenerator.Emit(OpCodes.Call, methodInfo);
						}
						else
						{
							ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
						}
					}
					Type returnType = member.GetReturnType();
					if ((resultType == typeof(object) || returnType.IsInterface) && returnType.IsValueType)
					{
						ilgenerator.Emit(OpCodes.Box, returnType);
					}
					break;
				}
				case DeepReflection.PathStepType.WeakListElement:
					ilgenerator.Emit(OpCodes.Ldc_I4, pathStep.ElementIndex);
					ilgenerator.Emit(OpCodes.Callvirt, DeepReflection.WeakListGetItem);
					break;
				case DeepReflection.PathStepType.StrongListElement:
				{
					Type type = typeof(IList).MakeGenericType(new Type[]
					{
						pathStep.ElementType
					});
					MethodInfo method = type.GetMethod("get_Item");
					ilgenerator.Emit(OpCodes.Ldc_I4, pathStep.ElementIndex);
					ilgenerator.Emit(OpCodes.Callvirt, method);
					break;
				}
				case DeepReflection.PathStepType.ArrayElement:
					ilgenerator.Emit(OpCodes.Ldc_I4, pathStep.ElementIndex);
					ilgenerator.Emit(OpCodes.Ldelem, pathStep.ElementType);
					break;
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			if (rootIsStatic)
			{
				return dynamicMethod.CreateDelegate(typeof(Func).MakeGenericType(new Type[]
				{
					resultType
				}));
			}
			return dynamicMethod.CreateDelegate(typeof(Func).MakeGenericType(new Type[]
			{
				rootType,
				resultType
			}));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00007FE8 File Offset: 0x000061E8
		private static Func<object> CreateSlowDeepStaticValueGetterDelegate(List<DeepReflection.PathStep> memberPath)
		{
			return delegate()
			{
				object obj = null;
				for (int i = 0; i < memberPath.Count; i++)
				{
					obj = DeepReflection.SlowGetMemberValue(memberPath[i], obj);
				}
				return obj;
			};
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00008010 File Offset: 0x00006210
		private static Func<object, object> CreateSlowDeepInstanceValueGetterDelegate(List<DeepReflection.PathStep> memberPath)
		{
			return delegate(object instance)
			{
				object obj = instance;
				for (int i = 0; i < memberPath.Count; i++)
				{
					obj = DeepReflection.SlowGetMemberValue(memberPath[i], obj);
				}
				return obj;
			};
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00008038 File Offset: 0x00006238
		private static Action<object, object> CreateSlowDeepInstanceValueSetterDelegate(List<DeepReflection.PathStep> memberPath)
		{
			return delegate(object instance, object arg)
			{
				object instance2 = instance;
				int num = memberPath.Count - 1;
				for (int i = 0; i < num; i++)
				{
					instance2 = DeepReflection.SlowGetMemberValue(memberPath[i], instance2);
				}
				DeepReflection.SlowSetMemberValue(memberPath[memberPath.Count - 1], instance2, arg);
			};
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008060 File Offset: 0x00006260
		private static object SlowGetMemberValue(DeepReflection.PathStep step, object instance)
		{
			switch (step.StepType)
			{
			case DeepReflection.PathStepType.Member:
			{
				FieldInfo fieldInfo = step.Member as FieldInfo;
				if (fieldInfo != null)
				{
					if (fieldInfo.IsLiteral)
					{
						return fieldInfo.GetRawConstantValue();
					}
					return fieldInfo.GetValue(instance);
				}
				else
				{
					PropertyInfo propertyInfo = step.Member as PropertyInfo;
					if (propertyInfo != null)
					{
						return propertyInfo.GetValue(instance, null);
					}
					MethodInfo methodInfo = step.Member as MethodInfo;
					if (methodInfo != null)
					{
						return methodInfo.Invoke(instance, null);
					}
					throw new NotSupportedException(step.Member.GetType().GetNiceName());
				}
				break;
			}
			case DeepReflection.PathStepType.WeakListElement:
				return DeepReflection.WeakListGetItem.Invoke(instance, new object[]
				{
					step.ElementIndex
				});
			case DeepReflection.PathStepType.StrongListElement:
				return step.StrongListGetItemMethod.Invoke(instance, new object[]
				{
					step.ElementIndex
				});
			case DeepReflection.PathStepType.ArrayElement:
				return (instance as Array).GetValue(step.ElementIndex);
			default:
				throw new NotImplementedException(step.StepType.ToString());
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000817C File Offset: 0x0000637C
		private static void SlowSetMemberValue(DeepReflection.PathStep step, object instance, object value)
		{
			switch (step.StepType)
			{
			case DeepReflection.PathStepType.Member:
			{
				FieldInfo fieldInfo = step.Member as FieldInfo;
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(instance, value);
					return;
				}
				PropertyInfo propertyInfo = step.Member as PropertyInfo;
				if (propertyInfo != null)
				{
					propertyInfo.SetValue(instance, value, null);
					return;
				}
				throw new NotSupportedException(step.Member.GetType().GetNiceName());
			}
			case DeepReflection.PathStepType.WeakListElement:
				DeepReflection.WeakListSetItem.Invoke(instance, new object[]
				{
					step.ElementIndex,
					value
				});
				return;
			case DeepReflection.PathStepType.StrongListElement:
			{
				MethodInfo method = typeof(IList).MakeGenericType(new Type[]
				{
					step.ElementType
				}).GetMethod("set_Item");
				method.Invoke(instance, new object[]
				{
					step.ElementIndex,
					value
				});
				return;
			}
			case DeepReflection.PathStepType.ArrayElement:
				(instance as Array).SetValue(value, step.ElementIndex);
				return;
			default:
				throw new NotImplementedException(step.StepType.ToString());
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000829C File Offset: 0x0000649C
		private static List<DeepReflection.PathStep> GetMemberPath(Type rootType, ref Type resultType, string path, out bool rootIsStatic, bool isSet)
		{
			if (path.IsNullOrWhitespace())
			{
				throw new ArgumentException("Invalid path; is null or whitespace.");
			}
			rootIsStatic = false;
			List<DeepReflection.PathStep> list = new List<DeepReflection.PathStep>();
			string[] array = path.Split(new char[]
			{
				'.'
			});
			Type type = rootType;
			foreach (string text in array)
			{
				bool flag = false;
				if (text.StartsWith("[", 2) && text.EndsWith("]", 2))
				{
					string text2 = text.Substring(1, text.Length - 2);
					int elementIndex;
					if (!int.TryParse(text2, ref elementIndex))
					{
						throw new ArgumentException("Couldn't parse an index from the path step '" + text + "'.");
					}
					if (type.IsArray)
					{
						Type elementType = type.GetElementType();
						list.Add(new DeepReflection.PathStep(elementIndex, elementType, true));
						type = elementType;
					}
					else if (type.ImplementsOpenGenericInterface(typeof(IList)))
					{
						Type type2 = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(IList))[0];
						list.Add(new DeepReflection.PathStep(elementIndex, type2, false));
						type = type2;
					}
					else
					{
						if (!typeof(IList).IsAssignableFrom(type))
						{
							throw new ArgumentException("Cannot get elements by index from the type '" + type.Name + "'.");
						}
						list.Add(new DeepReflection.PathStep(elementIndex));
						type = typeof(object);
					}
				}
				else
				{
					if (text.EndsWith("()", 2))
					{
						flag = true;
						text = text.Substring(0, text.Length - 2);
					}
					MemberInfo stepMember = DeepReflection.GetStepMember(type, text, flag);
					if (stepMember.IsStatic())
					{
						if (!(type == rootType))
						{
							throw new ArgumentException("The non-root member '" + text + "' is static; use that member as the path root instead.");
						}
						rootIsStatic = true;
					}
					type = stepMember.GetReturnType();
					if (flag && (type == null || type == typeof(void)))
					{
						throw new ArgumentException("The method '" + stepMember.Name + "' has no return type and cannot be part of a deep reflection path.");
					}
					list.Add(new DeepReflection.PathStep(stepMember));
				}
			}
			if (resultType == null)
			{
				resultType = type;
			}
			else if (type != typeof(object) && !resultType.IsAssignableFrom(type))
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Last member '",
					list[list.Count - 1].Member.Name,
					"' of path '",
					path,
					"' contains type '",
					type.AssemblyQualifiedName,
					"', which is not assignable to expected type '",
					resultType.AssemblyQualifiedName,
					"'."
				}));
			}
			return list;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00008548 File Offset: 0x00006748
		private static MemberInfo GetStepMember(Type owningType, string name, bool expectMethod)
		{
			MemberInfo memberInfo = null;
			MemberInfo[] array = owningType.GetAllMembers(name, 124).ToArray<MemberInfo>();
			int num = int.MaxValue;
			int i = 0;
			while (i < array.Length)
			{
				MemberInfo memberInfo2 = array[i];
				if (expectMethod)
				{
					MethodInfo methodInfo = memberInfo2 as MethodInfo;
					if (methodInfo != null)
					{
						int num2 = methodInfo.GetParameters().Length;
						if (memberInfo == null || num2 < num)
						{
							memberInfo = methodInfo;
							num = num2;
						}
					}
					i++;
				}
				else
				{
					if (memberInfo2 is MethodInfo)
					{
						throw new ArgumentException("Found method member for name '" + name + "', but expected a field or property.");
					}
					memberInfo = memberInfo2;
					break;
				}
			}
			if (memberInfo == null)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Could not find expected ",
					expectMethod ? "method" : "field or property",
					" '",
					name,
					"' on type '",
					owningType.GetNiceName(),
					"' while parsing reflection path."
				}));
			}
			if (expectMethod && num > 0)
			{
				throw new NotSupportedException(string.Concat(new string[]
				{
					"Method '",
					memberInfo.GetNiceName(),
					"' has ",
					num.ToString(),
					" parameters, but method parameters are currently not supported."
				}));
			}
			if (!(memberInfo is FieldInfo) && !(memberInfo is PropertyInfo) && !(memberInfo is MethodInfo))
			{
				throw new NotSupportedException("Members of type " + memberInfo.GetType().GetNiceName() + " are not support; only fields, properties and methods are supported.");
			}
			return memberInfo;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x000086B0 File Offset: 0x000068B0
		private static void EmitConstant(ILGenerator il, object constant, Type type = null)
		{
			if (constant == null)
			{
				il.Emit(OpCodes.Ldnull);
				return;
			}
			if (type == null)
			{
				type = constant.GetType();
			}
			if (type == typeof(int) || type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort))
			{
				il.Emit(OpCodes.Ldc_I4, Convert.ToInt32(constant));
				return;
			}
			if (type == typeof(uint))
			{
				il.Emit(OpCodes.Ldc_I4, (int)((uint)constant));
				return;
			}
			if (type == typeof(long))
			{
				il.Emit(OpCodes.Ldc_I8, (long)constant);
				return;
			}
			if (type == typeof(ulong))
			{
				il.Emit(OpCodes.Ldc_I8, (long)((ulong)constant));
				return;
			}
			if (type == typeof(float))
			{
				il.Emit(OpCodes.Ldc_R4, (float)constant);
				return;
			}
			if (type == typeof(double))
			{
				il.Emit(OpCodes.Ldc_R8, (double)constant);
				return;
			}
			if (type == typeof(string))
			{
				il.Emit(OpCodes.Ldstr, (string)constant);
				return;
			}
			if (type == typeof(char))
			{
				il.Emit(OpCodes.Ldc_I4, (int)((char)constant));
				return;
			}
			if (type == typeof(decimal))
			{
				int[] bits = decimal.GetBits((decimal)constant);
				ConstructorInfo constructor = typeof(decimal).GetConstructor(new Type[]
				{
					typeof(int[])
				});
				LocalBuilder localBuilder = il.DeclareLocal(typeof(int[]));
				il.Emit(OpCodes.Ldc_I4, bits.Length);
				il.Emit(OpCodes.Newarr, typeof(int));
				il.Emit(OpCodes.Stloc, localBuilder);
				for (int i = 0; i < bits.Length; i++)
				{
					il.Emit(OpCodes.Ldloc, localBuilder);
					il.Emit(OpCodes.Ldc_I4, i);
					il.Emit(OpCodes.Ldc_I4, bits[i]);
					il.Emit(OpCodes.Stelem_I4);
				}
				il.Emit(OpCodes.Ldloc, localBuilder);
				il.Emit(OpCodes.Newobj, constructor);
				return;
			}
			if (type == typeof(bool))
			{
				il.Emit(((bool)constant) ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
				return;
			}
			if (type.IsEnum)
			{
				DeepReflection.EmitConstant(il, constant, Enum.GetUnderlyingType(type));
				return;
			}
			throw new NotSupportedException("Type " + type.GetNiceFullName() + " is not supported as a constant.");
		}

		// Token: 0x04000035 RID: 53
		private static MethodInfo WeakListGetItem = typeof(IList).GetMethod("get_Item");

		// Token: 0x04000036 RID: 54
		private static MethodInfo WeakListSetItem = typeof(IList).GetMethod("set_Item");

		// Token: 0x04000037 RID: 55
		private static MethodInfo CreateWeakAliasForInstanceGetDelegate1MethodInfo = typeof(DeepReflection).GetMethod("CreateWeakAliasForInstanceGetDelegate1", 40);

		// Token: 0x04000038 RID: 56
		private static MethodInfo CreateWeakAliasForInstanceGetDelegate2MethodInfo = typeof(DeepReflection).GetMethod("CreateWeakAliasForInstanceGetDelegate2", 40);

		// Token: 0x04000039 RID: 57
		private static MethodInfo CreateWeakAliasForStaticGetDelegateMethodInfo = typeof(DeepReflection).GetMethod("CreateWeakAliasForStaticGetDelegate", 40);

		// Token: 0x0400003A RID: 58
		private static MethodInfo CreateWeakAliasForInstanceSetDelegate1MethodInfo = typeof(DeepReflection).GetMethod("CreateWeakAliasForInstanceSetDelegate1", 40);

		// Token: 0x02000061 RID: 97
		private enum PathStepType
		{
			// Token: 0x0400019D RID: 413
			Member,
			// Token: 0x0400019E RID: 414
			WeakListElement,
			// Token: 0x0400019F RID: 415
			StrongListElement,
			// Token: 0x040001A0 RID: 416
			ArrayElement
		}

		// Token: 0x02000062 RID: 98
		private struct PathStep
		{
			// Token: 0x06000371 RID: 881 RVA: 0x000114BB File Offset: 0x0000F6BB
			public PathStep(MemberInfo member)
			{
				this.StepType = DeepReflection.PathStepType.Member;
				this.Member = member;
				this.ElementIndex = -1;
				this.ElementType = null;
				this.StrongListGetItemMethod = null;
			}

			// Token: 0x06000372 RID: 882 RVA: 0x000114E0 File Offset: 0x0000F6E0
			public PathStep(int elementIndex)
			{
				this.StepType = DeepReflection.PathStepType.WeakListElement;
				this.Member = null;
				this.ElementIndex = elementIndex;
				this.ElementType = null;
				this.StrongListGetItemMethod = null;
			}

			// Token: 0x06000373 RID: 883 RVA: 0x00011508 File Offset: 0x0000F708
			public PathStep(int elementIndex, Type strongListElementType, bool isArray)
			{
				this.StepType = (isArray ? DeepReflection.PathStepType.ArrayElement : DeepReflection.PathStepType.StrongListElement);
				this.Member = null;
				this.ElementIndex = elementIndex;
				this.ElementType = strongListElementType;
				this.StrongListGetItemMethod = typeof(IList).MakeGenericType(new Type[]
				{
					strongListElementType
				}).GetMethod("get_Item");
			}

			// Token: 0x040001A1 RID: 417
			public readonly DeepReflection.PathStepType StepType;

			// Token: 0x040001A2 RID: 418
			public readonly MemberInfo Member;

			// Token: 0x040001A3 RID: 419
			public readonly int ElementIndex;

			// Token: 0x040001A4 RID: 420
			public readonly Type ElementType;

			// Token: 0x040001A5 RID: 421
			public readonly MethodInfo StrongListGetItemMethod;
		}
	}
}

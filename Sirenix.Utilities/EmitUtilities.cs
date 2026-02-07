using System;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Sirenix.Utilities
{
	// Token: 0x0200001B RID: 27
	public static class EmitUtilities
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00008CC8 File Offset: 0x00006EC8
		public static bool CanEmit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00008CCB File Offset: 0x00006ECB
		private static bool EmitIsIllegalForMember(MemberInfo member)
		{
			return member.DeclaringType != null && member.DeclaringType.Assembly == EmitUtilities.EngineAssembly;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008CF4 File Offset: 0x00006EF4
		public static Func<FieldType> CreateStaticFieldGetter<FieldType>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (fieldInfo.IsLiteral)
			{
				FieldType value = (FieldType)((object)fieldInfo.GetValue(null));
				return () => value;
			}
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return () => (FieldType)((object)fieldInfo.GetValue(null));
			}
			string text = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(FieldType), new Type[0], true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<FieldType>)dynamicMethod.CreateDelegate(typeof(Func<FieldType>));
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008E1C File Offset: 0x0000701C
		public static Func<object> CreateWeakStaticFieldGetter(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return () => fieldInfo.GetValue(null);
			}
			string text = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(object), new Type[0], true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldsfld, fieldInfo);
			if (fieldInfo.FieldType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008F34 File Offset: 0x00007134
		public static Action<FieldType> CreateStaticFieldSetter<FieldType>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (fieldInfo.IsLiteral)
			{
				throw new ArgumentException("Field cannot be constant.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(FieldType value)
				{
					fieldInfo.SetValue(null, value);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(FieldType)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Stsfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<FieldType>)dynamicMethod.CreateDelegate(typeof(Action<FieldType>));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00009048 File Offset: 0x00007248
		public static Action<object> CreateWeakStaticFieldSetter(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (!fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field must be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(object value)
				{
					fieldInfo.SetValue(null, value);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			if (fieldInfo.FieldType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
			}
			ilgenerator.Emit(OpCodes.Stsfld, fieldInfo);
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<object>)dynamicMethod.CreateDelegate(typeof(Action<object>));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00009184 File Offset: 0x00007384
		public static ValueGetter<InstanceType, FieldType> CreateInstanceFieldGetter<InstanceType, FieldType>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref InstanceType classInstance)
				{
					return (FieldType)((object)fieldInfo.GetValue(classInstance));
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(FieldType), new Type[]
			{
				typeof(InstanceType).MakeByRefType()
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (typeof(InstanceType).IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (ValueGetter<InstanceType, FieldType>)dynamicMethod.CreateDelegate(typeof(ValueGetter<InstanceType, FieldType>));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x000092C8 File Offset: 0x000074C8
		public static WeakValueGetter<FieldType> CreateWeakInstanceFieldGetter<FieldType>(Type instanceType, FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref object classInstance)
				{
					return (FieldType)((object)fieldInfo.GetValue(classInstance));
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(FieldType), new Type[]
			{
				typeof(object).MakeByRefType()
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueGetter<FieldType>)dynamicMethod.CreateDelegate(typeof(WeakValueGetter<FieldType>));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000943C File Offset: 0x0000763C
		public static WeakValueGetter CreateWeakInstanceFieldGetter(Type instanceType, FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref object classInstance)
				{
					return fieldInfo.GetValue(classInstance);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".get_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(object), new Type[]
			{
				typeof(object).MakeByRefType()
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				if (fieldInfo.FieldType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
				}
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				if (fieldInfo.FieldType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Box, fieldInfo.FieldType);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueGetter)dynamicMethod.CreateDelegate(typeof(WeakValueGetter));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00009600 File Offset: 0x00007800
		public static ValueSetter<InstanceType, FieldType> CreateInstanceFieldSetter<InstanceType, FieldType>(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref InstanceType classInstance, FieldType value)
				{
					if (typeof(InstanceType).IsValueType)
					{
						object obj = classInstance;
						fieldInfo.SetValue(obj, value);
						classInstance = (InstanceType)((object)obj);
						return;
					}
					fieldInfo.SetValue(classInstance, value);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(InstanceType).MakeByRefType(),
				typeof(FieldType)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (typeof(InstanceType).IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (ValueSetter<InstanceType, FieldType>)dynamicMethod.CreateDelegate(typeof(ValueSetter<InstanceType, FieldType>));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00009760 File Offset: 0x00007960
		public static WeakValueSetter<FieldType> CreateWeakInstanceFieldSetter<FieldType>(Type instanceType, FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref object classInstance, FieldType value)
				{
					fieldInfo.SetValue(classInstance, value);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object).MakeByRefType(),
				typeof(FieldType)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
				ilgenerator.Emit(OpCodes.Box, instanceType);
				ilgenerator.Emit(OpCodes.Stind_Ref);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueSetter<FieldType>)dynamicMethod.CreateDelegate(typeof(WeakValueSetter<FieldType>));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00009944 File Offset: 0x00007B44
		public static WeakValueSetter CreateWeakInstanceFieldSetter(Type instanceType, FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			if (fieldInfo.IsStatic)
			{
				throw new ArgumentException("Field cannot be static.");
			}
			fieldInfo = fieldInfo.DeAliasField(false);
			if (EmitUtilities.EmitIsIllegalForMember(fieldInfo))
			{
				return delegate(ref object classInstance, object value)
				{
					fieldInfo.SetValue(classInstance, value);
				};
			}
			string text = fieldInfo.ReflectedType.FullName + ".set_" + fieldInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object).MakeByRefType(),
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				if (fieldInfo.FieldType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
				}
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
				ilgenerator.Emit(OpCodes.Box, instanceType);
				ilgenerator.Emit(OpCodes.Stind_Ref);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				if (fieldInfo.FieldType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, fieldInfo.FieldType);
				}
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueSetter)dynamicMethod.CreateDelegate(typeof(WeakValueSetter));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00009BA8 File Offset: 0x00007DA8
		public static WeakValueGetter CreateWeakInstancePropertyGetter(Type instanceType, PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property must have a getter.");
			}
			if (getMethod.IsStatic)
			{
				throw new ArgumentException("Property cannot be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return delegate(ref object classInstance)
				{
					return propertyInfo.GetValue(classInstance, null);
				};
			}
			string text = propertyInfo.ReflectedType.FullName + ".get_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(object), new Type[]
			{
				typeof(object).MakeByRefType()
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				if (getMethod.IsVirtual || getMethod.IsAbstract)
				{
					ilgenerator.Emit(OpCodes.Callvirt, getMethod);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Call, getMethod);
				}
				if (propertyInfo.PropertyType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
				}
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				if (getMethod.IsVirtual || getMethod.IsAbstract)
				{
					ilgenerator.Emit(OpCodes.Callvirt, getMethod);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Call, getMethod);
				}
				if (propertyInfo.PropertyType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Box, propertyInfo.PropertyType);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueGetter)dynamicMethod.CreateDelegate(typeof(WeakValueGetter));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00009DE4 File Offset: 0x00007FE4
		public static WeakValueSetter CreateWeakInstancePropertySetter(Type instanceType, PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			if (instanceType == null)
			{
				throw new ArgumentNullException("instanceType");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod.IsStatic)
			{
				throw new ArgumentException("Property cannot be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return delegate(ref object classInstance, object value)
				{
					propertyInfo.SetValue(classInstance, value, null);
				};
			}
			string text = propertyInfo.ReflectedType.FullName + ".set_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object).MakeByRefType(),
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (instanceType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Unbox_Any, instanceType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				if (propertyInfo.PropertyType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
				}
				if (setMethod.IsVirtual || setMethod.IsAbstract)
				{
					ilgenerator.Emit(OpCodes.Callvirt, setMethod);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Call, setMethod);
				}
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
				ilgenerator.Emit(OpCodes.Box, instanceType);
				ilgenerator.Emit(OpCodes.Stind_Ref);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Castclass, instanceType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				if (propertyInfo.PropertyType.IsValueType)
				{
					ilgenerator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
				}
				if (setMethod.IsVirtual || setMethod.IsAbstract)
				{
					ilgenerator.Emit(OpCodes.Callvirt, setMethod);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Call, setMethod);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (WeakValueSetter)dynamicMethod.CreateDelegate(typeof(WeakValueSetter));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000A0B4 File Offset: 0x000082B4
		public static Action<PropType> CreateStaticPropertySetter<PropType>(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException("Property must have a set method.");
			}
			if (!setMethod.IsStatic)
			{
				throw new ArgumentException("Property must be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return delegate(PropType value)
				{
					propertyInfo.SetValue(null, value, null);
				};
			}
			string text = propertyInfo.ReflectedType.FullName + ".set_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(PropType)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, setMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<PropType>)dynamicMethod.CreateDelegate(typeof(Action<PropType>));
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000A1E4 File Offset: 0x000083E4
		public static Func<PropType> CreateStaticPropertyGetter<PropType>(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property must have a get method.");
			}
			if (!getMethod.IsStatic)
			{
				throw new ArgumentException("Property must be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return () => (PropType)((object)propertyInfo.GetValue(null, null));
			}
			string text = propertyInfo.ReflectedType.FullName + ".get_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(PropType), new Type[0], true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Call, getMethod);
			Type returnType = propertyInfo.GetReturnType();
			if (returnType.IsValueType && !typeof(PropType).IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, returnType);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<PropType>)dynamicMethod.CreateDelegate(typeof(Func<PropType>));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000A33C File Offset: 0x0000853C
		public static ValueSetter<InstanceType, PropType> CreateInstancePropertySetter<InstanceType, PropType>(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("fieldInfo");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo setMethod = propertyInfo.GetSetMethod(true);
			if (setMethod == null)
			{
				throw new ArgumentException("Property must have a set method.");
			}
			if (setMethod.IsStatic)
			{
				throw new ArgumentException("Property cannot be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return delegate(ref InstanceType classInstance, PropType value)
				{
					if (typeof(InstanceType).IsValueType)
					{
						object obj = classInstance;
						propertyInfo.SetValue(obj, value, null);
						classInstance = (InstanceType)((object)obj);
						return;
					}
					propertyInfo.SetValue(classInstance, value, null);
				};
			}
			string text = propertyInfo.ReflectedType.FullName + ".set_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(InstanceType).MakeByRefType(),
				typeof(PropType)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (typeof(InstanceType).IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Callvirt, setMethod);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Callvirt, setMethod);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (ValueSetter<InstanceType, PropType>)dynamicMethod.CreateDelegate(typeof(ValueSetter<InstanceType, PropType>));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A4D0 File Offset: 0x000086D0
		public static ValueGetter<InstanceType, PropType> CreateInstancePropertyGetter<InstanceType, PropType>(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}
			propertyInfo = propertyInfo.DeAliasProperty(false);
			if (propertyInfo.GetIndexParameters().Length != 0)
			{
				throw new ArgumentException("Property must not have any index parameters");
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod == null)
			{
				throw new ArgumentException("Property must have a get method.");
			}
			if (getMethod.IsStatic)
			{
				throw new ArgumentException("Property cannot be static.");
			}
			if (EmitUtilities.EmitIsIllegalForMember(propertyInfo))
			{
				return delegate(ref InstanceType classInstance)
				{
					return (PropType)((object)propertyInfo.GetValue(classInstance, null));
				};
			}
			string text = propertyInfo.ReflectedType.FullName + ".get_" + propertyInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(PropType), new Type[]
			{
				typeof(InstanceType).MakeByRefType()
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (typeof(InstanceType).IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Callvirt, getMethod);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldind_Ref);
				ilgenerator.Emit(OpCodes.Callvirt, getMethod);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (ValueGetter<InstanceType, PropType>)dynamicMethod.CreateDelegate(typeof(ValueGetter<InstanceType, PropType>));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000A648 File Offset: 0x00008848
		public static Func<InstanceType, ReturnType> CreateMethodReturner<InstanceType, ReturnType>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			return (Func<InstanceType, ReturnType>)Delegate.CreateDelegate(typeof(Func<InstanceType, ReturnType>), methodInfo);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000A6AC File Offset: 0x000088AC
		public static Action CreateStaticMethodCaller(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (!methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is an instance method when it has to be static.");
			}
			if (methodInfo.GetParameters().Length != 0)
			{
				throw new ArgumentException("Given method cannot have any parameters.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			return (Action)Delegate.CreateDelegate(typeof(Action), methodInfo);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000A724 File Offset: 0x00008924
		public static Action<object, TArg1> CreateWeakInstanceMethodCaller<TArg1>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' must have exactly one parameter.");
			}
			if (parameters[0].ParameterType != typeof(TArg1))
			{
				string[] array = new string[5];
				array[0] = "The first parameter of the method '";
				array[1] = methodInfo.Name;
				array[2] = "' must be of type ";
				int num = 3;
				Type typeFromHandle = typeof(TArg1);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[4] = ".";
				throw new ArgumentException(string.Concat(array));
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			if (EmitUtilities.EmitIsIllegalForMember(methodInfo))
			{
				return delegate(object classInstance, TArg1 arg)
				{
					methodInfo.Invoke(classInstance, new object[]
					{
						arg
					});
				};
			}
			Type declaringType = methodInfo.DeclaringType;
			string text = methodInfo.ReflectedType.FullName + ".call_" + methodInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object),
				typeof(TArg1)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (declaringType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Unbox_Any, declaringType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<object, TArg1>)dynamicMethod.CreateDelegate(typeof(Action<object, TArg1>));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A970 File Offset: 0x00008B70
		public static Action<object> CreateWeakInstanceMethodCaller(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.GetParameters().Length != 0)
			{
				throw new ArgumentException("Given method cannot have any parameters.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			if (EmitUtilities.EmitIsIllegalForMember(methodInfo))
			{
				return delegate(object classInstance)
				{
					methodInfo.Invoke(classInstance, null);
				};
			}
			Type declaringType = methodInfo.DeclaringType;
			string text = methodInfo.ReflectedType.FullName + ".call_" + methodInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, null, new Type[]
			{
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (declaringType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Unbox_Any, declaringType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, declaringType);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			if (methodInfo.ReturnType != null && methodInfo.ReturnType != typeof(void))
			{
				ilgenerator.Emit(OpCodes.Pop);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Action<object>)dynamicMethod.CreateDelegate(typeof(Action<object>));
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000AB50 File Offset: 0x00008D50
		public static Func<object, TArg1, TResult> CreateWeakInstanceMethodCaller<TResult, TArg1>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.ReturnType != typeof(TResult))
			{
				string[] array = new string[5];
				array[0] = "Given method '";
				array[1] = methodInfo.Name;
				array[2] = "' must return type ";
				int num = 3;
				Type typeFromHandle = typeof(TResult);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[4] = ".";
				throw new ArgumentException(string.Concat(array));
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' must have exactly one parameter.");
			}
			if (!typeof(TArg1).InheritsFrom(parameters[0].ParameterType))
			{
				string[] array2 = new string[5];
				array2[0] = "The first parameter of the method '";
				array2[1] = methodInfo.Name;
				array2[2] = "' must be of type ";
				int num2 = 3;
				Type typeFromHandle2 = typeof(TArg1);
				array2[num2] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
				array2[4] = ".";
				throw new ArgumentException(string.Concat(array2));
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			if (EmitUtilities.EmitIsIllegalForMember(methodInfo))
			{
				return (object classInstance, TArg1 arg1) => (TResult)((object)methodInfo.Invoke(classInstance, new object[]
				{
					arg1
				}));
			}
			Type declaringType = methodInfo.DeclaringType;
			string text = methodInfo.ReflectedType.FullName + ".call_" + methodInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(TResult), new Type[]
			{
				typeof(object),
				typeof(TArg1)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (declaringType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Unbox_Any, declaringType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<object, TArg1, TResult>)dynamicMethod.CreateDelegate(typeof(Func<object, TArg1, TResult>));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000AE14 File Offset: 0x00009014
		public static Func<object, TResult> CreateWeakInstanceMethodCallerFunc<TResult>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.ReturnType != typeof(TResult))
			{
				string[] array = new string[5];
				array[0] = "Given method '";
				array[1] = methodInfo.Name;
				array[2] = "' must return type ";
				int num = 3;
				Type typeFromHandle = typeof(TResult);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[4] = ".";
				throw new ArgumentException(string.Concat(array));
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 0)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' must have no parameter.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			if (EmitUtilities.EmitIsIllegalForMember(methodInfo))
			{
				return (object classInstance) => (TResult)((object)methodInfo.Invoke(classInstance, null));
			}
			Type declaringType = methodInfo.DeclaringType;
			string text = methodInfo.ReflectedType.FullName + ".call_" + methodInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(TResult), new Type[]
			{
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (declaringType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Unbox_Any, declaringType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, declaringType);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<object, TResult>)dynamicMethod.CreateDelegate(typeof(Func<object, TResult>));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000B048 File Offset: 0x00009248
		public static Func<object, TArg, TResult> CreateWeakInstanceMethodCallerFunc<TArg, TResult>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.ReturnType != typeof(TResult))
			{
				string[] array = new string[5];
				array[0] = "Given method '";
				array[1] = methodInfo.Name;
				array[2] = "' must return type ";
				int num = 3;
				Type typeFromHandle = typeof(TResult);
				array[num] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
				array[4] = ".";
				throw new ArgumentException(string.Concat(array));
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' must have one parameter.");
			}
			if (!parameters[0].ParameterType.IsAssignableFrom(typeof(TArg)))
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' has an invalid parameter type.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			if (EmitUtilities.EmitIsIllegalForMember(methodInfo))
			{
				return (object classInstance, TArg arg) => (TResult)((object)methodInfo.Invoke(classInstance, new object[]
				{
					arg
				}));
			}
			Type declaringType = methodInfo.DeclaringType;
			string text = methodInfo.ReflectedType.FullName + ".call_" + methodInfo.Name;
			DynamicMethod dynamicMethod = new DynamicMethod(text, typeof(TResult), new Type[]
			{
				typeof(object),
				typeof(TArg)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (declaringType.IsValueType)
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Unbox_Any, declaringType);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				ilgenerator.Emit(OpCodes.Ldloca_S, localBuilder);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, methodInfo);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Castclass, declaringType);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Callvirt, methodInfo);
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (Func<object, TArg, TResult>)dynamicMethod.CreateDelegate(typeof(Func<object, TArg, TResult>));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000B2DC File Offset: 0x000094DC
		public static Action<InstanceType> CreateInstanceMethodCaller<InstanceType>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.GetParameters().Length != 0)
			{
				throw new ArgumentException("Given method cannot have any parameters.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			return (Action<InstanceType>)Delegate.CreateDelegate(typeof(Action<InstanceType>), methodInfo);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000B354 File Offset: 0x00009554
		public static Action<InstanceType, Arg1> CreateInstanceMethodCaller<InstanceType, Arg1>(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}
			if (methodInfo.IsStatic)
			{
				throw new ArgumentException("Given method '" + methodInfo.Name + "' is static when it has to be an instance method.");
			}
			if (methodInfo.GetParameters().Length != 1)
			{
				throw new ArgumentException("Given method must have only one parameter.");
			}
			methodInfo = methodInfo.DeAliasMethod(false);
			return (Action<InstanceType, Arg1>)Delegate.CreateDelegate(typeof(Action<InstanceType, Arg1>), methodInfo);
		}

		// Token: 0x0400003C RID: 60
		private static Assembly EngineAssembly = typeof(Object).Assembly;
	}
}

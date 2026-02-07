using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
	// Token: 0x020001EB RID: 491
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Attribute))]
	[ComVisible(true)]
	[Serializable]
	public abstract class Attribute : _Attribute
	{
		// Token: 0x06001509 RID: 5385 RVA: 0x00052961 File Offset: 0x00050B61
		private static Attribute[] InternalGetCustomAttributes(PropertyInfo element, Type type, bool inherit)
		{
			return (Attribute[])MonoCustomAttrs.GetCustomAttributes(element, type, inherit);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00052961 File Offset: 0x00050B61
		private static Attribute[] InternalGetCustomAttributes(EventInfo element, Type type, bool inherit)
		{
			return (Attribute[])MonoCustomAttrs.GetCustomAttributes(element, type, inherit);
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00052970 File Offset: 0x00050B70
		private static Attribute[] InternalParamGetCustomAttributes(ParameterInfo parameter, Type attributeType, bool inherit)
		{
			if (parameter.Member.MemberType != MemberTypes.Method)
			{
				return null;
			}
			MethodInfo methodInfo = (MethodInfo)parameter.Member;
			MethodInfo baseDefinition = methodInfo.GetBaseDefinition();
			if (attributeType == null)
			{
				attributeType = typeof(Attribute);
			}
			if (methodInfo == baseDefinition)
			{
				return (Attribute[])parameter.GetCustomAttributes(attributeType, inherit);
			}
			List<Type> list = new List<Type>();
			List<Attribute> list2 = new List<Attribute>();
			for (;;)
			{
				foreach (Attribute attribute in (Attribute[])methodInfo.GetParametersInternal()[parameter.Position].GetCustomAttributes(attributeType, false))
				{
					Type type = attribute.GetType();
					if (!list.Contains(type))
					{
						list.Add(type);
						list2.Add(attribute);
					}
				}
				MethodInfo baseMethod = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
				if (baseMethod == methodInfo)
				{
					break;
				}
				methodInfo = baseMethod;
			}
			Attribute[] array2 = (Attribute[])Array.CreateInstance(attributeType, list2.Count);
			list2.CopyTo(array2, 0);
			return array2;
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00052A6A File Offset: 0x00050C6A
		private static bool InternalIsDefined(PropertyInfo element, Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(element, attributeType, inherit);
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00052A6A File Offset: 0x00050C6A
		private static bool InternalIsDefined(EventInfo element, Type attributeType, bool inherit)
		{
			return MonoCustomAttrs.IsDefined(element, attributeType, inherit);
		}

		// Token: 0x0600150E RID: 5390 RVA: 0x00052A74 File Offset: 0x00050C74
		private static bool InternalParamIsDefined(ParameterInfo parameter, Type attributeType, bool inherit)
		{
			if (parameter.IsDefined(attributeType, inherit))
			{
				return true;
			}
			if (!inherit)
			{
				return false;
			}
			MemberInfo member = parameter.Member;
			if (member.MemberType != MemberTypes.Method)
			{
				return false;
			}
			MethodInfo methodInfo = ((RuntimeMethodInfo)((MethodInfo)member)).GetBaseMethod();
			for (;;)
			{
				ParameterInfo[] parametersInternal = methodInfo.GetParametersInternal();
				if ((parametersInternal != null && parametersInternal.Length == 0) || parameter.Position < 0)
				{
					break;
				}
				if (parametersInternal[parameter.Position].IsDefined(attributeType, false))
				{
					return true;
				}
				MethodInfo baseMethod = ((RuntimeMethodInfo)methodInfo).GetBaseMethod();
				if (baseMethod == methodInfo)
				{
					return false;
				}
				methodInfo = baseMethod;
			}
			return false;
		}

		// Token: 0x0600150F RID: 5391 RVA: 0x00052AFB File Offset: 0x00050CFB
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type)
		{
			return Attribute.GetCustomAttributes(element, type, true);
		}

		// Token: 0x06001510 RID: 5392 RVA: 0x00052B08 File Offset: 0x00050D08
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsSubclassOf(typeof(Attribute)) && type != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, type, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, type, inherit);
			}
			return element.GetCustomAttributes(type, inherit) as Attribute[];
		}

		// Token: 0x06001511 RID: 5393 RVA: 0x00052BAA File Offset: 0x00050DAA
		public static Attribute[] GetCustomAttributes(MemberInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x06001512 RID: 5394 RVA: 0x00052BB4 File Offset: 0x00050DB4
		public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, typeof(Attribute), inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, typeof(Attribute), inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x00052C29 File Offset: 0x00050E29
		public static bool IsDefined(MemberInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x00052C34 File Offset: 0x00050E34
		public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalIsDefined((EventInfo)element, attributeType, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalIsDefined((PropertyInfo)element, attributeType, inherit);
			}
			return element.IsDefined(attributeType, inherit);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x00052CD1 File Offset: 0x00050ED1
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x00052CDC File Offset: 0x00050EDC
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("Multiple custom attributes of the same type found."));
		}

		// Token: 0x06001517 RID: 5399 RVA: 0x00052D14 File Offset: 0x00050F14
		public static Attribute[] GetCustomAttributes(ParameterInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x00052D1D File Offset: 0x00050F1D
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x00052D28 File Offset: 0x00050F28
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			if (element.Member == null)
			{
				throw new ArgumentException(Environment.GetResourceString("The ParameterInfo object is not valid."), "element");
			}
			if (element.Member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes(element, attributeType, inherit);
			}
			return element.GetCustomAttributes(attributeType, inherit) as Attribute[];
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x00052DD8 File Offset: 0x00050FD8
		public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element.Member == null)
			{
				throw new ArgumentException(Environment.GetResourceString("The ParameterInfo object is not valid."), "element");
			}
			if (element.Member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes(element, null, inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x00052E47 File Offset: 0x00051047
		public static bool IsDefined(ParameterInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x00052E54 File Offset: 0x00051054
		public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			MemberTypes memberType = element.Member.MemberType;
			if (memberType == MemberTypes.Constructor)
			{
				return element.IsDefined(attributeType, false);
			}
			if (memberType == MemberTypes.Method)
			{
				return Attribute.InternalParamIsDefined(element, attributeType, inherit);
			}
			if (memberType != MemberTypes.Property)
			{
				throw new ArgumentException(Environment.GetResourceString("Invalid type for ParameterInfo member in Attribute class."));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x00052EFC File Offset: 0x000510FC
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00052F08 File Offset: 0x00051108
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("Multiple custom attributes of the same type found."));
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x00052F46 File Offset: 0x00051146
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x06001520 RID: 5408 RVA: 0x00052F50 File Offset: 0x00051150
		public static Attribute[] GetCustomAttributes(Module element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00052F59 File Offset: 0x00051159
		public static Attribute[] GetCustomAttributes(Module element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00052F88 File Offset: 0x00051188
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00052FFE File Offset: 0x000511FE
		public static bool IsDefined(Module element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, false);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x00053008 File Offset: 0x00051208
		public static bool IsDefined(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00053079 File Offset: 0x00051279
		public static Attribute GetCustomAttribute(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x00053084 File Offset: 0x00051284
		public static Attribute GetCustomAttribute(Module element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("Multiple custom attributes of the same type found."));
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x000530BC File Offset: 0x000512BC
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000530C8 File Offset: 0x000512C8
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0005313E File Offset: 0x0005133E
		public static Attribute[] GetCustomAttributes(Assembly element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x00053147 File Offset: 0x00051347
		public static Attribute[] GetCustomAttributes(Assembly element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00053173 File Offset: 0x00051373
		public static bool IsDefined(Assembly element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x00053180 File Offset: 0x00051380
		public static bool IsDefined(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Type passed in must be derived from System.Attribute or System.Attribute itself."));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x000531F1 File Offset: 0x000513F1
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x000531FC File Offset: 0x000513FC
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("Multiple custom attributes of the same type found."));
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x00053234 File Offset: 0x00051434
		[SecuritySafeCritical]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			RuntimeType runtimeType = (RuntimeType)base.GetType();
			if ((RuntimeType)obj.GetType() != runtimeType)
			{
				return false;
			}
			FieldInfo[] fields = runtimeType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				object thisValue = ((RtFieldInfo)fields[i]).UnsafeGetValue(this);
				object thatValue = ((RtFieldInfo)fields[i]).UnsafeGetValue(obj);
				if (!Attribute.AreFieldValuesEqual(thisValue, thatValue))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000532B0 File Offset: 0x000514B0
		private static bool AreFieldValuesEqual(object thisValue, object thatValue)
		{
			if (thisValue == null && thatValue == null)
			{
				return true;
			}
			if (thisValue == null || thatValue == null)
			{
				return false;
			}
			if (thisValue.GetType().IsArray)
			{
				if (!thisValue.GetType().Equals(thatValue.GetType()))
				{
					return false;
				}
				Array array = thisValue as Array;
				Array array2 = thatValue as Array;
				if (array.Length != array2.Length)
				{
					return false;
				}
				for (int i = 0; i < array.Length; i++)
				{
					if (!Attribute.AreFieldValuesEqual(array.GetValue(i), array2.GetValue(i)))
					{
						return false;
					}
				}
			}
			else if (!thisValue.Equals(thatValue))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00053344 File Offset: 0x00051544
		[SecuritySafeCritical]
		public override int GetHashCode()
		{
			Type type = base.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object obj = null;
			for (int i = 0; i < fields.Length; i++)
			{
				object obj2 = ((RtFieldInfo)fields[i]).UnsafeGetValue(this);
				if (obj2 != null && !obj2.GetType().IsArray)
				{
					obj = obj2;
				}
				if (obj != null)
				{
					break;
				}
			}
			if (obj != null)
			{
				return obj.GetHashCode();
			}
			return type.GetHashCode();
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x00047214 File Offset: 0x00045414
		public virtual object TypeId
		{
			get
			{
				return base.GetType();
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000533A9 File Offset: 0x000515A9
		public virtual bool Match(object obj)
		{
			return this.Equals(obj);
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsDefaultAttribute()
		{
			return false;
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Attribute.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Attribute.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Attribute.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Attribute.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}

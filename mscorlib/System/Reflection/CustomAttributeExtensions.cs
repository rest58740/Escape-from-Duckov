using System;
using System.Collections.Generic;

namespace System.Reflection
{
	// Token: 0x020008E0 RID: 2272
	public static class CustomAttributeExtensions
	{
		// Token: 0x06004BA8 RID: 19368 RVA: 0x000F10E1 File Offset: 0x000EF2E1
		public static Attribute GetCustomAttribute(this Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType);
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x000F10EA File Offset: 0x000EF2EA
		public static Attribute GetCustomAttribute(this Module element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x000F10F3 File Offset: 0x000EF2F3
		public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType);
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x000F10FC File Offset: 0x000EF2FC
		public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType);
		}

		// Token: 0x06004BAC RID: 19372 RVA: 0x000F1105 File Offset: 0x000EF305
		public static T GetCustomAttribute<T>(this Assembly element) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T)));
		}

		// Token: 0x06004BAD RID: 19373 RVA: 0x000F111C File Offset: 0x000EF31C
		public static T GetCustomAttribute<T>(this Module element) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T)));
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x000F1133 File Offset: 0x000EF333
		public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T)));
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x000F114A File Offset: 0x000EF34A
		public static T GetCustomAttribute<T>(this ParameterInfo element) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T)));
		}

		// Token: 0x06004BB0 RID: 19376 RVA: 0x000F1161 File Offset: 0x000EF361
		public static Attribute GetCustomAttribute(this MemberInfo element, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttribute(element, attributeType, inherit);
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x000F116B File Offset: 0x000EF36B
		public static Attribute GetCustomAttribute(this ParameterInfo element, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttribute(element, attributeType, inherit);
		}

		// Token: 0x06004BB2 RID: 19378 RVA: 0x000F1175 File Offset: 0x000EF375
		public static T GetCustomAttribute<T>(this MemberInfo element, bool inherit) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T), inherit));
		}

		// Token: 0x06004BB3 RID: 19379 RVA: 0x000F118D File Offset: 0x000EF38D
		public static T GetCustomAttribute<T>(this ParameterInfo element, bool inherit) where T : Attribute
		{
			return (T)((object)element.GetCustomAttribute(typeof(T), inherit));
		}

		// Token: 0x06004BB4 RID: 19380 RVA: 0x000F11A5 File Offset: 0x000EF3A5
		public static IEnumerable<Attribute> GetCustomAttributes(this Assembly element)
		{
			return Attribute.GetCustomAttributes(element);
		}

		// Token: 0x06004BB5 RID: 19381 RVA: 0x000F11AD File Offset: 0x000EF3AD
		public static IEnumerable<Attribute> GetCustomAttributes(this Module element)
		{
			return Attribute.GetCustomAttributes(element);
		}

		// Token: 0x06004BB6 RID: 19382 RVA: 0x000F11B5 File Offset: 0x000EF3B5
		public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element)
		{
			return Attribute.GetCustomAttributes(element);
		}

		// Token: 0x06004BB7 RID: 19383 RVA: 0x000F11BD File Offset: 0x000EF3BD
		public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element)
		{
			return Attribute.GetCustomAttributes(element);
		}

		// Token: 0x06004BB8 RID: 19384 RVA: 0x000F11C5 File Offset: 0x000EF3C5
		public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, bool inherit)
		{
			return Attribute.GetCustomAttributes(element, inherit);
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x000F11CE File Offset: 0x000EF3CE
		public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, bool inherit)
		{
			return Attribute.GetCustomAttributes(element, inherit);
		}

		// Token: 0x06004BBA RID: 19386 RVA: 0x000F11D7 File Offset: 0x000EF3D7
		public static IEnumerable<Attribute> GetCustomAttributes(this Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType);
		}

		// Token: 0x06004BBB RID: 19387 RVA: 0x000F11E0 File Offset: 0x000EF3E0
		public static IEnumerable<Attribute> GetCustomAttributes(this Module element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType);
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x000F11E9 File Offset: 0x000EF3E9
		public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType);
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x000F11F2 File Offset: 0x000EF3F2
		public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType);
		}

		// Token: 0x06004BBE RID: 19390 RVA: 0x000F11FB File Offset: 0x000EF3FB
		public static IEnumerable<T> GetCustomAttributes<T>(this Assembly element) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T));
		}

		// Token: 0x06004BBF RID: 19391 RVA: 0x000F1212 File Offset: 0x000EF412
		public static IEnumerable<T> GetCustomAttributes<T>(this Module element) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T));
		}

		// Token: 0x06004BC0 RID: 19392 RVA: 0x000F1229 File Offset: 0x000EF429
		public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T));
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x000F1240 File Offset: 0x000EF440
		public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T));
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x000F1257 File Offset: 0x000EF457
		public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo element, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttributes(element, attributeType, inherit);
		}

		// Token: 0x06004BC3 RID: 19395 RVA: 0x000F1261 File Offset: 0x000EF461
		public static IEnumerable<Attribute> GetCustomAttributes(this ParameterInfo element, Type attributeType, bool inherit)
		{
			return Attribute.GetCustomAttributes(element, attributeType, inherit);
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x000F126B File Offset: 0x000EF46B
		public static IEnumerable<T> GetCustomAttributes<T>(this MemberInfo element, bool inherit) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T), inherit);
		}

		// Token: 0x06004BC5 RID: 19397 RVA: 0x000F1283 File Offset: 0x000EF483
		public static IEnumerable<T> GetCustomAttributes<T>(this ParameterInfo element, bool inherit) where T : Attribute
		{
			return (IEnumerable<T>)element.GetCustomAttributes(typeof(T), inherit);
		}

		// Token: 0x06004BC6 RID: 19398 RVA: 0x000F129B File Offset: 0x000EF49B
		public static bool IsDefined(this Assembly element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType);
		}

		// Token: 0x06004BC7 RID: 19399 RVA: 0x000F12A4 File Offset: 0x000EF4A4
		public static bool IsDefined(this Module element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType);
		}

		// Token: 0x06004BC8 RID: 19400 RVA: 0x000F12AD File Offset: 0x000EF4AD
		public static bool IsDefined(this MemberInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType);
		}

		// Token: 0x06004BC9 RID: 19401 RVA: 0x000F12B6 File Offset: 0x000EF4B6
		public static bool IsDefined(this ParameterInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType);
		}

		// Token: 0x06004BCA RID: 19402 RVA: 0x000F12BF File Offset: 0x000EF4BF
		public static bool IsDefined(this MemberInfo element, Type attributeType, bool inherit)
		{
			return Attribute.IsDefined(element, attributeType, inherit);
		}

		// Token: 0x06004BCB RID: 19403 RVA: 0x000F12C9 File Offset: 0x000EF4C9
		public static bool IsDefined(this ParameterInfo element, Type attributeType, bool inherit)
		{
			return Attribute.IsDefined(element, attributeType, inherit);
		}
	}
}

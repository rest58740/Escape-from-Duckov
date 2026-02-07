using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000B9 RID: 185
	internal static class MemberInfoExtensions
	{
		// Token: 0x06000517 RID: 1303 RVA: 0x00023AFC File Offset: 0x00021CFC
		public static bool IsDefined<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			bool result;
			try
			{
				result = member.IsDefined(typeof(T), inherit);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00023B34 File Offset: 0x00021D34
		public static bool IsDefined<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.IsDefined(false);
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00023B40 File Offset: 0x00021D40
		public static T GetAttribute<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			T[] array = member.GetAttributes(inherit).ToArray<T>();
			if (array != null && array.Length != 0)
			{
				return array[0];
			}
			return default(T);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00023B72 File Offset: 0x00021D72
		public static T GetAttribute<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.GetAttribute(false);
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x00023B7B File Offset: 0x00021D7B
		public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member) where T : Attribute
		{
			return member.GetAttributes(false);
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00023B84 File Offset: 0x00021D84
		public static IEnumerable<T> GetAttributes<T>(this ICustomAttributeProvider member, bool inherit) where T : Attribute
		{
			IEnumerable<T> result;
			try
			{
				result = member.GetCustomAttributes(typeof(T), inherit).Cast<T>();
			}
			catch
			{
				result = new T[0];
			}
			return result;
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00023BC8 File Offset: 0x00021DC8
		public static Attribute[] GetAttributes(this ICustomAttributeProvider member)
		{
			Attribute[] result;
			try
			{
				result = member.GetAttributes<Attribute>().ToArray<Attribute>();
			}
			catch
			{
				result = new Attribute[0];
			}
			return result;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00023C00 File Offset: 0x00021E00
		public static Attribute[] GetAttributes(this ICustomAttributeProvider member, bool inherit)
		{
			Attribute[] result;
			try
			{
				result = member.GetAttributes(inherit).ToArray<Attribute>();
			}
			catch
			{
				result = new Attribute[0];
			}
			return result;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00023C38 File Offset: 0x00021E38
		public static string GetNiceName(this MemberInfo member)
		{
			MethodBase methodBase = member as MethodBase;
			string input;
			if (methodBase != null)
			{
				input = methodBase.GetFullName();
			}
			else
			{
				input = member.Name;
			}
			return input.ToTitleCase();
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00023C6C File Offset: 0x00021E6C
		public static bool IsStatic(this MemberInfo member)
		{
			FieldInfo fieldInfo = member as FieldInfo;
			if (fieldInfo != null)
			{
				return fieldInfo.IsStatic;
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				if (!propertyInfo.CanRead)
				{
					return propertyInfo.GetSetMethod(true).IsStatic;
				}
				return propertyInfo.GetGetMethod(true).IsStatic;
			}
			else
			{
				MethodBase methodBase = member as MethodBase;
				if (methodBase != null)
				{
					return methodBase.IsStatic;
				}
				EventInfo eventInfo = member as EventInfo;
				if (eventInfo != null)
				{
					return eventInfo.GetRaiseMethod(true).IsStatic;
				}
				Type type = member as Type;
				if (type != null)
				{
					return type.IsSealed && type.IsAbstract;
				}
				string text = string.Format(CultureInfo.InvariantCulture, "Unable to determine IsStatic for member {0}.{1}MemberType was {2} but only fields, properties, methods, events and types are supported.", member.DeclaringType.FullName, member.Name, member.GetType().FullName);
				throw new NotSupportedException(text);
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00023D4F File Offset: 0x00021F4F
		public static bool IsAlias(this MemberInfo memberInfo)
		{
			return memberInfo is MemberAliasFieldInfo || memberInfo is MemberAliasPropertyInfo || memberInfo is MemberAliasMethodInfo;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00023D6C File Offset: 0x00021F6C
		public static MemberInfo DeAlias(this MemberInfo memberInfo, bool throwOnNotAliased = false)
		{
			MemberAliasFieldInfo memberAliasFieldInfo = memberInfo as MemberAliasFieldInfo;
			if (memberAliasFieldInfo != null)
			{
				return memberAliasFieldInfo.AliasedField;
			}
			MemberAliasPropertyInfo memberAliasPropertyInfo = memberInfo as MemberAliasPropertyInfo;
			if (memberAliasPropertyInfo != null)
			{
				return memberAliasPropertyInfo.AliasedProperty;
			}
			MemberAliasMethodInfo memberAliasMethodInfo = memberInfo as MemberAliasMethodInfo;
			if (memberAliasMethodInfo != null)
			{
				return memberAliasMethodInfo.AliasedMethod;
			}
			if (throwOnNotAliased)
			{
				throw new ArgumentException("The member " + memberInfo.GetNiceName() + " was not aliased.");
			}
			return memberInfo;
		}
	}
}

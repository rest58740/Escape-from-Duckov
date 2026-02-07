using System;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x0200000C RID: 12
	public static class PropertyInfoExtensions
	{
		// Token: 0x06000051 RID: 81 RVA: 0x000035C8 File Offset: 0x000017C8
		public static bool IsAutoProperty(this PropertyInfo propInfo, bool allowVirtual = false)
		{
			if (!propInfo.CanWrite || !propInfo.CanRead)
			{
				return false;
			}
			if (!allowVirtual)
			{
				MethodInfo getMethod = propInfo.GetGetMethod(true);
				MethodInfo setMethod = propInfo.GetSetMethod(true);
				if ((getMethod != null && (getMethod.IsAbstract || getMethod.IsVirtual)) || (setMethod != null && (setMethod.IsAbstract || setMethod.IsVirtual)))
				{
					return false;
				}
			}
			BindingFlags bindingFlags = 44;
			string text = "<" + propInfo.Name + ">";
			FieldInfo[] fields = propInfo.DeclaringType.GetFields(bindingFlags);
			for (int i = 0; i < fields.Length; i++)
			{
				if (fields[i].Name.Contains(text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000367E File Offset: 0x0000187E
		public static bool IsAliasProperty(this PropertyInfo propertyInfo)
		{
			return propertyInfo is MemberAliasPropertyInfo;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000368C File Offset: 0x0000188C
		public static PropertyInfo DeAliasProperty(this PropertyInfo propertyInfo, bool throwOnNotAliased = false)
		{
			MemberAliasPropertyInfo memberAliasPropertyInfo = propertyInfo as MemberAliasPropertyInfo;
			if (memberAliasPropertyInfo != null)
			{
				while (memberAliasPropertyInfo.AliasedProperty is MemberAliasPropertyInfo)
				{
					memberAliasPropertyInfo = (memberAliasPropertyInfo.AliasedProperty as MemberAliasPropertyInfo);
				}
				return memberAliasPropertyInfo.AliasedProperty;
			}
			if (throwOnNotAliased)
			{
				throw new ArgumentException("The property " + propertyInfo.GetNiceName() + " was not aliased.");
			}
			return propertyInfo;
		}
	}
}

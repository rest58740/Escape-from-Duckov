using System;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000BD RID: 189
	internal static class PropertyInfoExtensions
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x00023FE8 File Offset: 0x000221E8
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

		// Token: 0x0600052B RID: 1323 RVA: 0x0002409E File Offset: 0x0002229E
		public static bool IsAliasProperty(this PropertyInfo propertyInfo)
		{
			return propertyInfo is MemberAliasPropertyInfo;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000240AC File Offset: 0x000222AC
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

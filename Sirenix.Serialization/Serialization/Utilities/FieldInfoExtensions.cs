using System;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000B6 RID: 182
	internal static class FieldInfoExtensions
	{
		// Token: 0x0600050E RID: 1294 RVA: 0x000239BC File Offset: 0x00021BBC
		public static bool IsAliasField(this FieldInfo fieldInfo)
		{
			return fieldInfo is MemberAliasFieldInfo;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000239C8 File Offset: 0x00021BC8
		public static FieldInfo DeAliasField(this FieldInfo fieldInfo, bool throwOnNotAliased = false)
		{
			MemberAliasFieldInfo memberAliasFieldInfo = fieldInfo as MemberAliasFieldInfo;
			if (memberAliasFieldInfo != null)
			{
				while (memberAliasFieldInfo.AliasedField is MemberAliasFieldInfo)
				{
					memberAliasFieldInfo = (memberAliasFieldInfo.AliasedField as MemberAliasFieldInfo);
				}
				return memberAliasFieldInfo.AliasedField;
			}
			if (throwOnNotAliased)
			{
				throw new ArgumentException("The field " + fieldInfo.GetNiceName() + " was not aliased.");
			}
			return fieldInfo;
		}
	}
}

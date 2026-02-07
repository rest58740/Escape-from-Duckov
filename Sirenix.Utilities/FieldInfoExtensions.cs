using System;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x02000004 RID: 4
	public static class FieldInfoExtensions
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00002569 File Offset: 0x00000769
		public static bool IsAliasField(this FieldInfo fieldInfo)
		{
			return fieldInfo is MemberAliasFieldInfo;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002574 File Offset: 0x00000774
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

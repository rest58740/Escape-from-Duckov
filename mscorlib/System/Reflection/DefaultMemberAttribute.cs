using System;

namespace System.Reflection
{
	// Token: 0x02000897 RID: 2199
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public sealed class DefaultMemberAttribute : Attribute
	{
		// Token: 0x06004898 RID: 18584 RVA: 0x000EE247 File Offset: 0x000EC447
		public DefaultMemberAttribute(string memberName)
		{
			this.MemberName = memberName;
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06004899 RID: 18585 RVA: 0x000EE256 File Offset: 0x000EC456
		public string MemberName { get; }
	}
}

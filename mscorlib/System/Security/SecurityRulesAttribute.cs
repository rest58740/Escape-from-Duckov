using System;

namespace System.Security
{
	// Token: 0x020003D8 RID: 984
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
	public sealed class SecurityRulesAttribute : Attribute
	{
		// Token: 0x0600286C RID: 10348 RVA: 0x00092A85 File Offset: 0x00090C85
		public SecurityRulesAttribute(SecurityRuleSet ruleSet)
		{
			this.m_ruleSet = ruleSet;
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x00092A94 File Offset: 0x00090C94
		// (set) Token: 0x0600286E RID: 10350 RVA: 0x00092A9C File Offset: 0x00090C9C
		public bool SkipVerificationInFullTrust
		{
			get
			{
				return this.m_skipVerificationInFullTrust;
			}
			set
			{
				this.m_skipVerificationInFullTrust = value;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x00092AA5 File Offset: 0x00090CA5
		public SecurityRuleSet RuleSet
		{
			get
			{
				return this.m_ruleSet;
			}
		}

		// Token: 0x04001EA6 RID: 7846
		private SecurityRuleSet m_ruleSet;

		// Token: 0x04001EA7 RID: 7847
		private bool m_skipVerificationInFullTrust;
	}
}

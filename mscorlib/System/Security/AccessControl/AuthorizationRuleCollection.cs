using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x0200050D RID: 1293
	public sealed class AuthorizationRuleCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06003358 RID: 13144 RVA: 0x000BCA3F File Offset: 0x000BAC3F
		public AuthorizationRuleCollection()
		{
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x000BCA47 File Offset: 0x000BAC47
		internal AuthorizationRuleCollection(AuthorizationRule[] rules)
		{
			base.InnerList.AddRange(rules);
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x000BCA5B File Offset: 0x000BAC5B
		public void AddRule(AuthorizationRule rule)
		{
			base.InnerList.Add(rule);
		}

		// Token: 0x170006F8 RID: 1784
		public AuthorizationRule this[int index]
		{
			get
			{
				return (AuthorizationRule)base.InnerList[index];
			}
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x000BCA7D File Offset: 0x000BAC7D
		public void CopyTo(AuthorizationRule[] rules, int index)
		{
			base.InnerList.CopyTo(rules, index);
		}
	}
}

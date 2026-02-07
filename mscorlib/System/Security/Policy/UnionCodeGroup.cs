using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000424 RID: 1060
	[ComVisible(true)]
	[Serializable]
	public sealed class UnionCodeGroup : CodeGroup
	{
		// Token: 0x06002B5F RID: 11103 RVA: 0x00099ED2 File Offset: 0x000980D2
		public UnionCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) : base(membershipCondition, policy)
		{
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00099BFB File Offset: 0x00097DFB
		internal UnionCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x0009CBF4 File Offset: 0x0009ADF4
		public override CodeGroup Copy()
		{
			return this.Copy(true);
		}

		// Token: 0x06002B62 RID: 11106 RVA: 0x0009CC00 File Offset: 0x0009AE00
		internal CodeGroup Copy(bool childs)
		{
			UnionCodeGroup unionCodeGroup = new UnionCodeGroup(base.MembershipCondition, base.PolicyStatement);
			unionCodeGroup.Name = base.Name;
			unionCodeGroup.Description = base.Description;
			if (childs)
			{
				foreach (object obj in base.Children)
				{
					CodeGroup codeGroup = (CodeGroup)obj;
					unionCodeGroup.AddChild(codeGroup.Copy());
				}
			}
			return unionCodeGroup;
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x0009CC8C File Offset: 0x0009AE8C
		public override PolicyStatement Resolve(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			if (!base.MembershipCondition.Check(evidence))
			{
				return null;
			}
			PermissionSet permissionSet = base.PolicyStatement.PermissionSet.Copy();
			if (base.Children.Count > 0)
			{
				foreach (object obj in base.Children)
				{
					PolicyStatement policyStatement = ((CodeGroup)obj).Resolve(evidence);
					if (policyStatement != null)
					{
						permissionSet = permissionSet.Union(policyStatement.PermissionSet);
					}
				}
			}
			PolicyStatement policyStatement2 = base.PolicyStatement.Copy();
			policyStatement2.PermissionSet = permissionSet;
			return policyStatement2;
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x0009CD44 File Offset: 0x0009AF44
		public override CodeGroup ResolveMatchingCodeGroups(Evidence evidence)
		{
			if (evidence == null)
			{
				throw new ArgumentNullException("evidence");
			}
			if (!base.MembershipCondition.Check(evidence))
			{
				return null;
			}
			CodeGroup codeGroup = this.Copy(false);
			if (base.Children.Count > 0)
			{
				foreach (object obj in base.Children)
				{
					CodeGroup codeGroup2 = ((CodeGroup)obj).ResolveMatchingCodeGroups(evidence);
					if (codeGroup2 != null)
					{
						codeGroup.AddChild(codeGroup2);
					}
				}
			}
			return codeGroup;
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06002B65 RID: 11109 RVA: 0x00099C94 File Offset: 0x00097E94
		public override string MergeLogic
		{
			get
			{
				return "Union";
			}
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x0200040F RID: 1039
	[ComVisible(true)]
	[Serializable]
	public sealed class FirstMatchCodeGroup : CodeGroup
	{
		// Token: 0x06002A82 RID: 10882 RVA: 0x00099ED2 File Offset: 0x000980D2
		public FirstMatchCodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy) : base(membershipCondition, policy)
		{
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00099BFB File Offset: 0x00097DFB
		internal FirstMatchCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002A84 RID: 10884 RVA: 0x00099EDC File Offset: 0x000980DC
		public override string MergeLogic
		{
			get
			{
				return "First Match";
			}
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x00099EE4 File Offset: 0x000980E4
		public override CodeGroup Copy()
		{
			FirstMatchCodeGroup firstMatchCodeGroup = this.CopyNoChildren();
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				firstMatchCodeGroup.AddChild(codeGroup.Copy());
			}
			return firstMatchCodeGroup;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x00099F4C File Offset: 0x0009814C
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
			foreach (object obj in base.Children)
			{
				PolicyStatement policyStatement = ((CodeGroup)obj).Resolve(evidence);
				if (policyStatement != null)
				{
					return policyStatement;
				}
			}
			return base.PolicyStatement;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x00099FD4 File Offset: 0x000981D4
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
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				if (codeGroup.Resolve(evidence) != null)
				{
					return codeGroup.Copy();
				}
			}
			return this.CopyNoChildren();
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x0009A060 File Offset: 0x00098260
		private FirstMatchCodeGroup CopyNoChildren()
		{
			return new FirstMatchCodeGroup(base.MembershipCondition, base.PolicyStatement)
			{
				Name = base.Name,
				Description = base.Description
			};
		}
	}
}

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x0200040E RID: 1038
	[ComVisible(true)]
	[Serializable]
	public sealed class FileCodeGroup : CodeGroup
	{
		// Token: 0x06002A76 RID: 10870 RVA: 0x00099BEA File Offset: 0x00097DEA
		public FileCodeGroup(IMembershipCondition membershipCondition, FileIOPermissionAccess access) : base(membershipCondition, null)
		{
			this.m_access = access;
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x00099BFB File Offset: 0x00097DFB
		internal FileCodeGroup(SecurityElement e, PolicyLevel level) : base(e, level)
		{
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x00099C08 File Offset: 0x00097E08
		public override CodeGroup Copy()
		{
			FileCodeGroup fileCodeGroup = new FileCodeGroup(base.MembershipCondition, this.m_access);
			fileCodeGroup.Name = base.Name;
			fileCodeGroup.Description = base.Description;
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				fileCodeGroup.AddChild(codeGroup.Copy());
			}
			return fileCodeGroup;
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06002A79 RID: 10873 RVA: 0x00099C94 File Offset: 0x00097E94
		public override string MergeLogic
		{
			get
			{
				return "Union";
			}
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x00099C9C File Offset: 0x00097E9C
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
			PermissionSet permissionSet = null;
			if (base.PolicyStatement == null)
			{
				permissionSet = new PermissionSet(PermissionState.None);
			}
			else
			{
				permissionSet = base.PolicyStatement.PermissionSet.Copy();
			}
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
			PolicyStatement policyStatement2;
			if (base.PolicyStatement != null)
			{
				policyStatement2 = base.PolicyStatement.Copy();
			}
			else
			{
				policyStatement2 = PolicyStatement.Empty();
			}
			policyStatement2.PermissionSet = permissionSet;
			return policyStatement2;
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x00099D80 File Offset: 0x00097F80
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
			FileCodeGroup fileCodeGroup = new FileCodeGroup(base.MembershipCondition, this.m_access);
			foreach (object obj in base.Children)
			{
				CodeGroup codeGroup = ((CodeGroup)obj).ResolveMatchingCodeGroups(evidence);
				if (codeGroup != null)
				{
					fileCodeGroup.AddChild(codeGroup);
				}
			}
			return fileCodeGroup;
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06002A7C RID: 10876 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string AttributeString
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06002A7D RID: 10877 RVA: 0x00099E14 File Offset: 0x00098014
		public override string PermissionSetName
		{
			get
			{
				return "Same directory FileIO - " + this.m_access.ToString();
			}
		}

		// Token: 0x06002A7E RID: 10878 RVA: 0x00099E31 File Offset: 0x00098031
		public override bool Equals(object o)
		{
			return o is FileCodeGroup && this.m_access == ((FileCodeGroup)o).m_access && base.Equals((CodeGroup)o, false);
		}

		// Token: 0x06002A7F RID: 10879 RVA: 0x00099E5F File Offset: 0x0009805F
		public override int GetHashCode()
		{
			return this.m_access.GetHashCode();
		}

		// Token: 0x06002A80 RID: 10880 RVA: 0x00099E74 File Offset: 0x00098074
		protected override void ParseXml(SecurityElement e, PolicyLevel level)
		{
			string text = e.Attribute("Access");
			if (text != null)
			{
				this.m_access = (FileIOPermissionAccess)Enum.Parse(typeof(FileIOPermissionAccess), text, true);
				return;
			}
			this.m_access = FileIOPermissionAccess.NoAccess;
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x00099EB4 File Offset: 0x000980B4
		protected override void CreateXml(SecurityElement element, PolicyLevel level)
		{
			element.AddAttribute("Access", this.m_access.ToString());
		}

		// Token: 0x04001F96 RID: 8086
		private FileIOPermissionAccess m_access;
	}
}

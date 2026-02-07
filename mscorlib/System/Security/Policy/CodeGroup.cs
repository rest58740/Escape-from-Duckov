using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000407 RID: 1031
	[ComVisible(true)]
	[Serializable]
	public abstract class CodeGroup
	{
		// Token: 0x06002A23 RID: 10787 RVA: 0x00098A8E File Offset: 0x00096C8E
		protected CodeGroup(IMembershipCondition membershipCondition, PolicyStatement policy)
		{
			if (membershipCondition == null)
			{
				throw new ArgumentNullException("membershipCondition");
			}
			if (policy != null)
			{
				this.m_policy = policy.Copy();
			}
			this.m_membershipCondition = membershipCondition.Copy();
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x00098ACA File Offset: 0x00096CCA
		internal CodeGroup(SecurityElement e, PolicyLevel level)
		{
			this.FromXml(e, level);
		}

		// Token: 0x06002A25 RID: 10789
		public abstract CodeGroup Copy();

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06002A26 RID: 10790
		public abstract string MergeLogic { get; }

		// Token: 0x06002A27 RID: 10791
		public abstract PolicyStatement Resolve(Evidence evidence);

		// Token: 0x06002A28 RID: 10792
		public abstract CodeGroup ResolveMatchingCodeGroups(Evidence evidence);

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06002A29 RID: 10793 RVA: 0x00098AE5 File Offset: 0x00096CE5
		// (set) Token: 0x06002A2A RID: 10794 RVA: 0x00098AED File Offset: 0x00096CED
		public PolicyStatement PolicyStatement
		{
			get
			{
				return this.m_policy;
			}
			set
			{
				this.m_policy = value;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06002A2B RID: 10795 RVA: 0x00098AF6 File Offset: 0x00096CF6
		// (set) Token: 0x06002A2C RID: 10796 RVA: 0x00098AFE File Offset: 0x00096CFE
		public string Description
		{
			get
			{
				return this.m_description;
			}
			set
			{
				this.m_description = value;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06002A2D RID: 10797 RVA: 0x00098B07 File Offset: 0x00096D07
		// (set) Token: 0x06002A2E RID: 10798 RVA: 0x00098B0F File Offset: 0x00096D0F
		public IMembershipCondition MembershipCondition
		{
			get
			{
				return this.m_membershipCondition;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentException("value");
				}
				this.m_membershipCondition = value;
			}
		}

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06002A2F RID: 10799 RVA: 0x00098B26 File Offset: 0x00096D26
		// (set) Token: 0x06002A30 RID: 10800 RVA: 0x00098B2E File Offset: 0x00096D2E
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06002A31 RID: 10801 RVA: 0x00098B37 File Offset: 0x00096D37
		// (set) Token: 0x06002A32 RID: 10802 RVA: 0x00098B3F File Offset: 0x00096D3F
		public IList Children
		{
			get
			{
				return this.m_children;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_children = new ArrayList(value);
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x00098B5B File Offset: 0x00096D5B
		public virtual string AttributeString
		{
			get
			{
				if (this.m_policy != null)
				{
					return this.m_policy.AttributeString;
				}
				return null;
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x00098B72 File Offset: 0x00096D72
		public virtual string PermissionSetName
		{
			get
			{
				if (this.m_policy == null)
				{
					return null;
				}
				if (this.m_policy.PermissionSet is NamedPermissionSet)
				{
					return ((NamedPermissionSet)this.m_policy.PermissionSet).Name;
				}
				return null;
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00098BA7 File Offset: 0x00096DA7
		public void AddChild(CodeGroup group)
		{
			if (group == null)
			{
				throw new ArgumentNullException("group");
			}
			this.m_children.Add(group.Copy());
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00098BCC File Offset: 0x00096DCC
		public override bool Equals(object o)
		{
			CodeGroup codeGroup = o as CodeGroup;
			return codeGroup != null && this.Equals(codeGroup, false);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00098BF0 File Offset: 0x00096DF0
		public bool Equals(CodeGroup cg, bool compareChildren)
		{
			if (cg.Name != this.Name)
			{
				return false;
			}
			if (cg.Description != this.Description)
			{
				return false;
			}
			if (!cg.MembershipCondition.Equals(this.m_membershipCondition))
			{
				return false;
			}
			if (compareChildren)
			{
				int count = cg.Children.Count;
				if (this.Children.Count != count)
				{
					return false;
				}
				for (int i = 0; i < count; i++)
				{
					if (!((CodeGroup)this.Children[i]).Equals((CodeGroup)cg.Children[i], false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x00098C94 File Offset: 0x00096E94
		public void RemoveChild(CodeGroup group)
		{
			if (group != null)
			{
				this.m_children.Remove(group);
			}
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x00098CA8 File Offset: 0x00096EA8
		public override int GetHashCode()
		{
			int num = this.m_membershipCondition.GetHashCode();
			if (this.m_policy != null)
			{
				num += this.m_policy.GetHashCode();
			}
			return num;
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x00098CD8 File Offset: 0x00096ED8
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x00098CE4 File Offset: 0x00096EE4
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			string text = e.Attribute("PermissionSetName");
			PermissionSet permissionSet;
			if (text != null && level != null)
			{
				permissionSet = level.GetNamedPermissionSet(text);
			}
			else
			{
				SecurityElement securityElement = e.SearchForChildByTag("PermissionSet");
				if (securityElement != null)
				{
					permissionSet = (PermissionSet)Activator.CreateInstance(Type.GetType(securityElement.Attribute("class")), true);
					permissionSet.FromXml(securityElement);
				}
				else
				{
					permissionSet = new PermissionSet(new PermissionSet(PermissionState.None));
				}
			}
			this.m_policy = new PolicyStatement(permissionSet);
			this.m_children.Clear();
			if (e.Children != null && e.Children.Count > 0)
			{
				foreach (object obj in e.Children)
				{
					SecurityElement securityElement2 = (SecurityElement)obj;
					if (securityElement2.Tag == "CodeGroup")
					{
						this.AddChild(CodeGroup.CreateFromXml(securityElement2, level));
					}
				}
			}
			this.m_membershipCondition = null;
			SecurityElement securityElement3 = e.SearchForChildByTag("IMembershipCondition");
			if (securityElement3 != null)
			{
				string text2 = securityElement3.Attribute("class");
				Type type = Type.GetType(text2);
				if (type == null)
				{
					type = Type.GetType("System.Security.Policy." + text2);
				}
				this.m_membershipCondition = (IMembershipCondition)Activator.CreateInstance(type, true);
				this.m_membershipCondition.FromXml(securityElement3, level);
			}
			this.m_name = e.Attribute("Name");
			this.m_description = e.Attribute("Description");
			this.ParseXml(e, level);
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void ParseXml(SecurityElement e, PolicyLevel level)
		{
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x00098E8C File Offset: 0x0009708C
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x00098E98 File Offset: 0x00097098
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("CodeGroup");
			securityElement.AddAttribute("class", base.GetType().AssemblyQualifiedName);
			securityElement.AddAttribute("version", "1");
			if (this.Name != null)
			{
				securityElement.AddAttribute("Name", this.Name);
			}
			if (this.Description != null)
			{
				securityElement.AddAttribute("Description", this.Description);
			}
			if (this.MembershipCondition != null)
			{
				securityElement.AddChild(this.MembershipCondition.ToXml());
			}
			if (this.PolicyStatement != null && this.PolicyStatement.PermissionSet != null)
			{
				securityElement.AddChild(this.PolicyStatement.PermissionSet.ToXml());
			}
			foreach (object obj in this.Children)
			{
				CodeGroup codeGroup = (CodeGroup)obj;
				securityElement.AddChild(codeGroup.ToXml());
			}
			this.CreateXml(securityElement, level);
			return securityElement;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x00004BF9 File Offset: 0x00002DF9
		protected virtual void CreateXml(SecurityElement element, PolicyLevel level)
		{
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x00098FA8 File Offset: 0x000971A8
		internal static CodeGroup CreateFromXml(SecurityElement se, PolicyLevel level)
		{
			string text = se.Attribute("class");
			string text2 = text;
			int num = text2.IndexOf(",");
			if (num > 0)
			{
				text2 = text2.Substring(0, num);
			}
			num = text2.LastIndexOf(".");
			if (num > 0)
			{
				text2 = text2.Substring(num + 1);
			}
			if (text2 == "FileCodeGroup")
			{
				return new FileCodeGroup(se, level);
			}
			if (text2 == "FirstMatchCodeGroup")
			{
				return new FirstMatchCodeGroup(se, level);
			}
			if (text2 == "NetCodeGroup")
			{
				return new NetCodeGroup(se, level);
			}
			if (!(text2 == "UnionCodeGroup"))
			{
				CodeGroup codeGroup = (CodeGroup)Activator.CreateInstance(Type.GetType(text), true);
				codeGroup.FromXml(se, level);
				return codeGroup;
			}
			return new UnionCodeGroup(se, level);
		}

		// Token: 0x04001F6A RID: 8042
		private PolicyStatement m_policy;

		// Token: 0x04001F6B RID: 8043
		private IMembershipCondition m_membershipCondition;

		// Token: 0x04001F6C RID: 8044
		private string m_description;

		// Token: 0x04001F6D RID: 8045
		private string m_name;

		// Token: 0x04001F6E RID: 8046
		private ArrayList m_children = new ArrayList();
	}
}

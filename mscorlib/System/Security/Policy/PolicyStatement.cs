using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x0200041D RID: 1053
	[ComVisible(true)]
	[Serializable]
	public sealed class PolicyStatement : ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06002B0A RID: 11018 RVA: 0x0009BFEC File Offset: 0x0009A1EC
		public PolicyStatement(PermissionSet permSet) : this(permSet, PolicyStatementAttribute.Nothing)
		{
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x0009BFF6 File Offset: 0x0009A1F6
		public PolicyStatement(PermissionSet permSet, PolicyStatementAttribute attributes)
		{
			if (permSet != null)
			{
				this.perms = permSet.Copy();
				this.perms.SetReadOnly(true);
			}
			this.attrs = attributes;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x0009C020 File Offset: 0x0009A220
		// (set) Token: 0x06002B0D RID: 11021 RVA: 0x0009C048 File Offset: 0x0009A248
		public PermissionSet PermissionSet
		{
			get
			{
				if (this.perms == null)
				{
					this.perms = new PermissionSet(PermissionState.None);
					this.perms.SetReadOnly(true);
				}
				return this.perms;
			}
			set
			{
				this.perms = value;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x0009C051 File Offset: 0x0009A251
		// (set) Token: 0x06002B0F RID: 11023 RVA: 0x0009C059 File Offset: 0x0009A259
		public PolicyStatementAttribute Attributes
		{
			get
			{
				return this.attrs;
			}
			set
			{
				if (value <= PolicyStatementAttribute.All)
				{
					this.attrs = value;
					return;
				}
				throw new ArgumentException(string.Format(Locale.GetText("Invalid value for {0}."), "PolicyStatementAttribute"));
			}
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x0009C080 File Offset: 0x0009A280
		public string AttributeString
		{
			get
			{
				switch (this.attrs)
				{
				case PolicyStatementAttribute.Exclusive:
					return "Exclusive";
				case PolicyStatementAttribute.LevelFinal:
					return "LevelFinal";
				case PolicyStatementAttribute.All:
					return "Exclusive LevelFinal";
				default:
					return string.Empty;
				}
			}
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x0009C0C1 File Offset: 0x0009A2C1
		public PolicyStatement Copy()
		{
			return new PolicyStatement(this.perms, this.attrs);
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x0009C0D4 File Offset: 0x0009A2D4
		public void FromXml(SecurityElement et)
		{
			this.FromXml(et, null);
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x0009C0E0 File Offset: 0x0009A2E0
		[SecuritySafeCritical]
		public void FromXml(SecurityElement et, PolicyLevel level)
		{
			if (et == null)
			{
				throw new ArgumentNullException("et");
			}
			if (et.Tag != "PolicyStatement")
			{
				throw new ArgumentException(Locale.GetText("Invalid tag."));
			}
			string text = et.Attribute("Attributes");
			if (text != null)
			{
				this.attrs = (PolicyStatementAttribute)Enum.Parse(typeof(PolicyStatementAttribute), text);
			}
			SecurityElement et2 = et.SearchForChildByTag("PermissionSet");
			this.PermissionSet.FromXml(et2);
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x0009C15F File Offset: 0x0009A35F
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x0009C168 File Offset: 0x0009A368
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = new SecurityElement("PolicyStatement");
			securityElement.AddAttribute("version", "1");
			if (this.attrs != PolicyStatementAttribute.Nothing)
			{
				securityElement.AddAttribute("Attributes", this.attrs.ToString());
			}
			securityElement.AddChild(this.PermissionSet.ToXml());
			return securityElement;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x0009C1C8 File Offset: 0x0009A3C8
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			PolicyStatement policyStatement = obj as PolicyStatement;
			return policyStatement != null && this.PermissionSet.Equals(obj) && this.attrs == policyStatement.attrs;
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x0009C204 File Offset: 0x0009A404
		[ComVisible(false)]
		public override int GetHashCode()
		{
			return this.PermissionSet.GetHashCode() ^ (int)this.attrs;
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000985C4 File Offset: 0x000967C4
		internal static PolicyStatement Empty()
		{
			return new PolicyStatement(new PermissionSet(PermissionState.None));
		}

		// Token: 0x04001FB0 RID: 8112
		private PermissionSet perms;

		// Token: 0x04001FB1 RID: 8113
		private PolicyStatementAttribute attrs;
	}
}

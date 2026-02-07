using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Security.Policy
{
	// Token: 0x02000421 RID: 1057
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongNameMembershipCondition : IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable, IConstantMembershipCondition
	{
		// Token: 0x06002B3F RID: 11071 RVA: 0x0009C808 File Offset: 0x0009AA08
		public StrongNameMembershipCondition(StrongNamePublicKeyBlob blob, string name, Version version)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			this.blob = blob;
			this.name = name;
			if (version != null)
			{
				this.assemblyVersion = (Version)version.Clone();
			}
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x0009C858 File Offset: 0x0009AA58
		internal StrongNameMembershipCondition(SecurityElement e)
		{
			this.FromXml(e);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x0009C86E File Offset: 0x0009AA6E
		internal StrongNameMembershipCondition()
		{
		}

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06002B42 RID: 11074 RVA: 0x0009C87D File Offset: 0x0009AA7D
		// (set) Token: 0x06002B43 RID: 11075 RVA: 0x0009C885 File Offset: 0x0009AA85
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06002B44 RID: 11076 RVA: 0x0009C88E File Offset: 0x0009AA8E
		// (set) Token: 0x06002B45 RID: 11077 RVA: 0x0009C896 File Offset: 0x0009AA96
		public Version Version
		{
			get
			{
				return this.assemblyVersion;
			}
			set
			{
				this.assemblyVersion = value;
			}
		}

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x06002B46 RID: 11078 RVA: 0x0009C89F File Offset: 0x0009AA9F
		// (set) Token: 0x06002B47 RID: 11079 RVA: 0x0009C8A7 File Offset: 0x0009AAA7
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				return this.blob;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PublicKey");
				}
				this.blob = value;
			}
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x0009C8C0 File Offset: 0x0009AAC0
		public bool Check(Evidence evidence)
		{
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				StrongName strongName = obj as StrongName;
				if (strongName != null)
				{
					return strongName.PublicKey.Equals(this.blob) && (this.name == null || !(this.name != strongName.Name)) && (!(this.assemblyVersion != null) || this.assemblyVersion.Equals(strongName.Version));
				}
			}
			return false;
		}

		// Token: 0x06002B49 RID: 11081 RVA: 0x0009C94A File Offset: 0x0009AB4A
		public IMembershipCondition Copy()
		{
			return new StrongNameMembershipCondition(this.blob, this.name, this.assemblyVersion);
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x0009C964 File Offset: 0x0009AB64
		public override bool Equals(object o)
		{
			StrongNameMembershipCondition strongNameMembershipCondition = o as StrongNameMembershipCondition;
			if (strongNameMembershipCondition == null)
			{
				return false;
			}
			if (!strongNameMembershipCondition.PublicKey.Equals(this.PublicKey))
			{
				return false;
			}
			if (this.name != strongNameMembershipCondition.Name)
			{
				return false;
			}
			if (this.assemblyVersion != null)
			{
				return this.assemblyVersion.Equals(strongNameMembershipCondition.Version);
			}
			return strongNameMembershipCondition.Version == null;
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x0009C9D3 File Offset: 0x0009ABD3
		public override int GetHashCode()
		{
			return this.blob.GetHashCode();
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x0009C9E0 File Offset: 0x0009ABE0
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x0009C9EC File Offset: 0x0009ABEC
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			MembershipConditionHelper.CheckSecurityElement(e, "e", this.version, this.version);
			this.blob = StrongNamePublicKeyBlob.FromString(e.Attribute("PublicKeyBlob"));
			this.name = e.Attribute("Name");
			string text = e.Attribute("AssemblyVersion");
			if (text == null)
			{
				this.assemblyVersion = null;
				return;
			}
			this.assemblyVersion = new Version(text);
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x0009CA5C File Offset: 0x0009AC5C
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("StrongName - ");
			stringBuilder.Append(this.blob);
			if (this.name != null)
			{
				stringBuilder.AppendFormat(" name = {0}", this.name);
			}
			if (this.assemblyVersion != null)
			{
				stringBuilder.AppendFormat(" version = {0}", this.assemblyVersion);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002B4F RID: 11087 RVA: 0x0009CAC1 File Offset: 0x0009ACC1
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x0009CACC File Offset: 0x0009ACCC
		public SecurityElement ToXml(PolicyLevel level)
		{
			SecurityElement securityElement = MembershipConditionHelper.Element(typeof(StrongNameMembershipCondition), this.version);
			if (this.blob != null)
			{
				securityElement.AddAttribute("PublicKeyBlob", this.blob.ToString());
			}
			if (this.name != null)
			{
				securityElement.AddAttribute("Name", this.name);
			}
			if (this.assemblyVersion != null)
			{
				string text = this.assemblyVersion.ToString();
				if (text != "0.0")
				{
					securityElement.AddAttribute("AssemblyVersion", text);
				}
			}
			return securityElement;
		}

		// Token: 0x04001FB8 RID: 8120
		private readonly int version = 1;

		// Token: 0x04001FB9 RID: 8121
		private StrongNamePublicKeyBlob blob;

		// Token: 0x04001FBA RID: 8122
		private string name;

		// Token: 0x04001FBB RID: 8123
		private Version assemblyVersion;
	}
}

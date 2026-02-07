using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x02000420 RID: 1056
	[ComVisible(true)]
	[Serializable]
	public sealed class StrongName : EvidenceBase, IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06002B33 RID: 11059 RVA: 0x0009C65C File Offset: 0x0009A85C
		public StrongName(StrongNamePublicKeyBlob blob, string name, Version version)
		{
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Locale.GetText("Empty"), "name");
			}
			this.publickey = blob;
			this.name = name;
			this.version = version;
		}

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002B34 RID: 11060 RVA: 0x0009C6D1 File Offset: 0x0009A8D1
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002B35 RID: 11061 RVA: 0x0009C6D9 File Offset: 0x0009A8D9
		public StrongNamePublicKeyBlob PublicKey
		{
			get
			{
				return this.publickey;
			}
		}

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x06002B36 RID: 11062 RVA: 0x0009C6E1 File Offset: 0x0009A8E1
		public Version Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x06002B37 RID: 11063 RVA: 0x0009C6E9 File Offset: 0x0009A8E9
		public object Copy()
		{
			return new StrongName(this.publickey, this.name, this.version);
		}

		// Token: 0x06002B38 RID: 11064 RVA: 0x0009C702 File Offset: 0x0009A902
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new StrongNameIdentityPermission(this.publickey, this.name, this.version);
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x0009C71C File Offset: 0x0009A91C
		public override bool Equals(object o)
		{
			StrongName strongName = o as StrongName;
			return strongName != null && !(this.name != strongName.Name) && this.Version.Equals(strongName.Version) && this.PublicKey.Equals(strongName.PublicKey);
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x0009C770 File Offset: 0x0009A970
		public override int GetHashCode()
		{
			return this.publickey.GetHashCode();
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x0009C780 File Offset: 0x0009A980
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement(typeof(StrongName).Name);
			securityElement.AddAttribute("version", "1");
			securityElement.AddAttribute("Key", this.publickey.ToString());
			securityElement.AddAttribute("Name", this.name);
			securityElement.AddAttribute("Version", this.version.ToString());
			return securityElement.ToString();
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x0009C7F3 File Offset: 0x0009A9F3
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return (verbose ? 5 : 1) + this.name.Length;
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return 0;
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO("IBuiltInEvidence")]
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			return 0;
		}

		// Token: 0x04001FB5 RID: 8117
		private StrongNamePublicKeyBlob publickey;

		// Token: 0x04001FB6 RID: 8118
		private string name;

		// Token: 0x04001FB7 RID: 8119
		private Version version;
	}
}

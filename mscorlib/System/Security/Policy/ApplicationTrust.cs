using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using Mono.Security.Cryptography;

namespace System.Security.Policy
{
	// Token: 0x02000403 RID: 1027
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationTrust : EvidenceBase, ISecurityEncodable
	{
		// Token: 0x060029F0 RID: 10736 RVA: 0x00098206 File Offset: 0x00096406
		public ApplicationTrust()
		{
			this.fullTrustAssemblies = new List<StrongName>(0);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0009821A File Offset: 0x0009641A
		public ApplicationTrust(ApplicationIdentity applicationIdentity) : this()
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this._appid = applicationIdentity;
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00098238 File Offset: 0x00096438
		public ApplicationTrust(PermissionSet defaultGrantSet, IEnumerable<StrongName> fullTrustAssemblies)
		{
			if (defaultGrantSet == null)
			{
				throw new ArgumentNullException("defaultGrantSet");
			}
			this._defaultPolicy = new PolicyStatement(defaultGrantSet);
			if (fullTrustAssemblies == null)
			{
				throw new ArgumentNullException("fullTrustAssemblies");
			}
			this.fullTrustAssemblies = new List<StrongName>();
			foreach (StrongName strongName in fullTrustAssemblies)
			{
				if (strongName == null)
				{
					throw new ArgumentException("fullTrustAssemblies contains an assembly that does not have a StrongName");
				}
				this.fullTrustAssemblies.Add((StrongName)strongName.Copy());
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000982D8 File Offset: 0x000964D8
		// (set) Token: 0x060029F4 RID: 10740 RVA: 0x000982E0 File Offset: 0x000964E0
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return this._appid;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("ApplicationIdentity");
				}
				this._appid = value;
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060029F5 RID: 10741 RVA: 0x000982F7 File Offset: 0x000964F7
		// (set) Token: 0x060029F6 RID: 10742 RVA: 0x00098313 File Offset: 0x00096513
		public PolicyStatement DefaultGrantSet
		{
			get
			{
				if (this._defaultPolicy == null)
				{
					this._defaultPolicy = this.GetDefaultGrantSet();
				}
				return this._defaultPolicy;
			}
			set
			{
				this._defaultPolicy = value;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x0009831C File Offset: 0x0009651C
		// (set) Token: 0x060029F8 RID: 10744 RVA: 0x00098324 File Offset: 0x00096524
		public object ExtraInfo
		{
			get
			{
				return this._xtranfo;
			}
			set
			{
				this._xtranfo = value;
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060029F9 RID: 10745 RVA: 0x0009832D File Offset: 0x0009652D
		// (set) Token: 0x060029FA RID: 10746 RVA: 0x00098335 File Offset: 0x00096535
		public bool IsApplicationTrustedToRun
		{
			get
			{
				return this._trustrun;
			}
			set
			{
				this._trustrun = value;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060029FB RID: 10747 RVA: 0x0009833E File Offset: 0x0009653E
		// (set) Token: 0x060029FC RID: 10748 RVA: 0x00098346 File Offset: 0x00096546
		public bool Persist
		{
			get
			{
				return this._persist;
			}
			set
			{
				this._persist = value;
			}
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x00098350 File Offset: 0x00096550
		public void FromXml(SecurityElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (element.Tag != "ApplicationTrust")
			{
				throw new ArgumentException("element");
			}
			string text = element.Attribute("FullName");
			if (text != null)
			{
				this._appid = new ApplicationIdentity(text);
			}
			else
			{
				this._appid = null;
			}
			this._defaultPolicy = null;
			SecurityElement securityElement = element.SearchForChildByTag("DefaultGrant");
			if (securityElement != null)
			{
				for (int i = 0; i < securityElement.Children.Count; i++)
				{
					SecurityElement securityElement2 = securityElement.Children[i] as SecurityElement;
					if (securityElement2.Tag == "PolicyStatement")
					{
						this.DefaultGrantSet.FromXml(securityElement2, null);
						break;
					}
				}
			}
			if (!bool.TryParse(element.Attribute("TrustedToRun"), out this._trustrun))
			{
				this._trustrun = false;
			}
			if (!bool.TryParse(element.Attribute("Persist"), out this._persist))
			{
				this._persist = false;
			}
			this._xtranfo = null;
			SecurityElement securityElement3 = element.SearchForChildByTag("ExtraInfo");
			if (securityElement3 != null)
			{
				text = securityElement3.Attribute("Data");
				if (text != null)
				{
					using (MemoryStream memoryStream = new MemoryStream(CryptoConvert.FromHex(text)))
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						this._xtranfo = binaryFormatter.Deserialize(memoryStream);
					}
				}
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000984B4 File Offset: 0x000966B4
		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("ApplicationTrust");
			securityElement.AddAttribute("version", "1");
			if (this._appid != null)
			{
				securityElement.AddAttribute("FullName", this._appid.FullName);
			}
			if (this._trustrun)
			{
				securityElement.AddAttribute("TrustedToRun", "true");
			}
			if (this._persist)
			{
				securityElement.AddAttribute("Persist", "true");
			}
			SecurityElement securityElement2 = new SecurityElement("DefaultGrant");
			securityElement2.AddChild(this.DefaultGrantSet.ToXml());
			securityElement.AddChild(securityElement2);
			if (this._xtranfo != null)
			{
				byte[] input = null;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					new BinaryFormatter().Serialize(memoryStream, this._xtranfo);
					input = memoryStream.ToArray();
				}
				SecurityElement securityElement3 = new SecurityElement("ExtraInfo");
				securityElement3.AddAttribute("Data", CryptoConvert.ToHex(input));
				securityElement.AddChild(securityElement3);
			}
			return securityElement;
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x000985BC File Offset: 0x000967BC
		public IList<StrongName> FullTrustAssemblies
		{
			get
			{
				return this.fullTrustAssemblies;
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000985C4 File Offset: 0x000967C4
		private PolicyStatement GetDefaultGrantSet()
		{
			return new PolicyStatement(new PermissionSet(PermissionState.None));
		}

		// Token: 0x04001F5B RID: 8027
		private ApplicationIdentity _appid;

		// Token: 0x04001F5C RID: 8028
		private PolicyStatement _defaultPolicy;

		// Token: 0x04001F5D RID: 8029
		private object _xtranfo;

		// Token: 0x04001F5E RID: 8030
		private bool _trustrun;

		// Token: 0x04001F5F RID: 8031
		private bool _persist;

		// Token: 0x04001F60 RID: 8032
		private IList<StrongName> fullTrustAssemblies;
	}
}

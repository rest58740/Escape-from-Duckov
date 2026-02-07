using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Cryptography;

namespace System.Security.Permissions
{
	// Token: 0x02000450 RID: 1104
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Serializable]
	public sealed class PublisherIdentityPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06002CC6 RID: 11462 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public PublisherIdentityPermissionAttribute(SecurityAction action) : base(action)
		{
		}

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x06002CC7 RID: 11463 RVA: 0x000A0BA1 File Offset: 0x0009EDA1
		// (set) Token: 0x06002CC8 RID: 11464 RVA: 0x000A0BA9 File Offset: 0x0009EDA9
		public string CertFile
		{
			get
			{
				return this.certFile;
			}
			set
			{
				this.certFile = value;
			}
		}

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06002CC9 RID: 11465 RVA: 0x000A0BB2 File Offset: 0x0009EDB2
		// (set) Token: 0x06002CCA RID: 11466 RVA: 0x000A0BBA File Offset: 0x0009EDBA
		public string SignedFile
		{
			get
			{
				return this.signedFile;
			}
			set
			{
				this.signedFile = value;
			}
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06002CCB RID: 11467 RVA: 0x000A0BC3 File Offset: 0x0009EDC3
		// (set) Token: 0x06002CCC RID: 11468 RVA: 0x000A0BCB File Offset: 0x0009EDCB
		public string X509Certificate
		{
			get
			{
				return this.x509data;
			}
			set
			{
				this.x509data = value;
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000A0BD4 File Offset: 0x0009EDD4
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new PublisherIdentityPermission(PermissionState.Unrestricted);
			}
			if (this.x509data != null)
			{
				return new PublisherIdentityPermission(new X509Certificate(CryptoConvert.FromHex(this.x509data)));
			}
			if (this.certFile != null)
			{
				return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(this.certFile));
			}
			if (this.signedFile != null)
			{
				return new PublisherIdentityPermission(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromSignedFile(this.signedFile));
			}
			return new PublisherIdentityPermission(PermissionState.None);
		}

		// Token: 0x04002076 RID: 8310
		private string certFile;

		// Token: 0x04002077 RID: 8311
		private string signedFile;

		// Token: 0x04002078 RID: 8312
		private string x509data;
	}
}

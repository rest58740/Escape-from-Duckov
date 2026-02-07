using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Mono.Security.Cryptography;

namespace System.Security.Permissions
{
	// Token: 0x0200044F RID: 1103
	[ComVisible(true)]
	[Serializable]
	public sealed class PublisherIdentityPermission : CodeAccessPermission, IBuiltInPermission
	{
		// Token: 0x06002CBA RID: 11450 RVA: 0x0009EC58 File Offset: 0x0009CE58
		public PublisherIdentityPermission(PermissionState state)
		{
			CodeAccessPermission.CheckPermissionState(state, false);
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000A0979 File Offset: 0x0009EB79
		public PublisherIdentityPermission(X509Certificate certificate)
		{
			this.Certificate = certificate;
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000A0988 File Offset: 0x0009EB88
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x000A0990 File Offset: 0x0009EB90
		public X509Certificate Certificate
		{
			get
			{
				return this.x509;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("X509Certificate");
				}
				this.x509 = value;
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000A09A8 File Offset: 0x0009EBA8
		public override IPermission Copy()
		{
			PublisherIdentityPermission publisherIdentityPermission = new PublisherIdentityPermission(PermissionState.None);
			if (this.x509 != null)
			{
				publisherIdentityPermission.Certificate = this.x509;
			}
			return publisherIdentityPermission;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000A09D4 File Offset: 0x0009EBD4
		public override void FromXml(SecurityElement esd)
		{
			CodeAccessPermission.CheckSecurityElement(esd, "esd", 1, 1);
			string text = esd.Attributes["X509v3Certificate"] as string;
			if (text != null)
			{
				byte[] data = CryptoConvert.FromHex(text);
				this.x509 = new X509Certificate(data);
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000A0A1C File Offset: 0x0009EC1C
		public override IPermission Intersect(IPermission target)
		{
			PublisherIdentityPermission publisherIdentityPermission = this.Cast(target);
			if (publisherIdentityPermission == null)
			{
				return null;
			}
			if (this.x509 != null && publisherIdentityPermission.x509 != null && this.x509.GetRawCertDataString() == publisherIdentityPermission.x509.GetRawCertDataString())
			{
				return new PublisherIdentityPermission(publisherIdentityPermission.x509);
			}
			return null;
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000A0A70 File Offset: 0x0009EC70
		public override bool IsSubsetOf(IPermission target)
		{
			PublisherIdentityPermission publisherIdentityPermission = this.Cast(target);
			return publisherIdentityPermission != null && (this.x509 == null || (publisherIdentityPermission.x509 != null && this.x509.GetRawCertDataString() == publisherIdentityPermission.x509.GetRawCertDataString()));
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000A0ABC File Offset: 0x0009ECBC
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = base.Element(1);
			if (this.x509 != null)
			{
				securityElement.AddAttribute("X509v3Certificate", this.x509.GetRawCertDataString());
			}
			return securityElement;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000A0AF0 File Offset: 0x0009ECF0
		public override IPermission Union(IPermission target)
		{
			PublisherIdentityPermission publisherIdentityPermission = this.Cast(target);
			if (publisherIdentityPermission == null)
			{
				return this.Copy();
			}
			if (this.x509 != null && publisherIdentityPermission.x509 != null)
			{
				if (this.x509.GetRawCertDataString() == publisherIdentityPermission.x509.GetRawCertDataString())
				{
					return new PublisherIdentityPermission(this.x509);
				}
			}
			else
			{
				if (this.x509 == null && publisherIdentityPermission.x509 != null)
				{
					return new PublisherIdentityPermission(publisherIdentityPermission.x509);
				}
				if (this.x509 != null && publisherIdentityPermission.x509 == null)
				{
					return new PublisherIdentityPermission(this.x509);
				}
			}
			return null;
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000481C6 File Offset: 0x000463C6
		int IBuiltInPermission.GetTokenIndex()
		{
			return 10;
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000A0B81 File Offset: 0x0009ED81
		private PublisherIdentityPermission Cast(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			PublisherIdentityPermission publisherIdentityPermission = target as PublisherIdentityPermission;
			if (publisherIdentityPermission == null)
			{
				CodeAccessPermission.ThrowInvalidPermission(target, typeof(PublisherIdentityPermission));
			}
			return publisherIdentityPermission;
		}

		// Token: 0x04002074 RID: 8308
		private const int version = 1;

		// Token: 0x04002075 RID: 8309
		private X509Certificate x509;
	}
}

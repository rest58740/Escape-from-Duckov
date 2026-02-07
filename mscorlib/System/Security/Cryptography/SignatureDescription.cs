using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004B8 RID: 1208
	[ComVisible(true)]
	public class SignatureDescription
	{
		// Token: 0x06003074 RID: 12404 RVA: 0x0000259F File Offset: 0x0000079F
		public SignatureDescription()
		{
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000B0B10 File Offset: 0x000AED10
		public SignatureDescription(SecurityElement el)
		{
			if (el == null)
			{
				throw new ArgumentNullException("el");
			}
			this._strKey = el.SearchForTextOfTag("Key");
			this._strDigest = el.SearchForTextOfTag("Digest");
			this._strFormatter = el.SearchForTextOfTag("Formatter");
			this._strDeformatter = el.SearchForTextOfTag("Deformatter");
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06003076 RID: 12406 RVA: 0x000B0B75 File Offset: 0x000AED75
		// (set) Token: 0x06003077 RID: 12407 RVA: 0x000B0B7D File Offset: 0x000AED7D
		public string KeyAlgorithm
		{
			get
			{
				return this._strKey;
			}
			set
			{
				this._strKey = value;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06003078 RID: 12408 RVA: 0x000B0B86 File Offset: 0x000AED86
		// (set) Token: 0x06003079 RID: 12409 RVA: 0x000B0B8E File Offset: 0x000AED8E
		public string DigestAlgorithm
		{
			get
			{
				return this._strDigest;
			}
			set
			{
				this._strDigest = value;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x0600307A RID: 12410 RVA: 0x000B0B97 File Offset: 0x000AED97
		// (set) Token: 0x0600307B RID: 12411 RVA: 0x000B0B9F File Offset: 0x000AED9F
		public string FormatterAlgorithm
		{
			get
			{
				return this._strFormatter;
			}
			set
			{
				this._strFormatter = value;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600307C RID: 12412 RVA: 0x000B0BA8 File Offset: 0x000AEDA8
		// (set) Token: 0x0600307D RID: 12413 RVA: 0x000B0BB0 File Offset: 0x000AEDB0
		public string DeformatterAlgorithm
		{
			get
			{
				return this._strDeformatter;
			}
			set
			{
				this._strDeformatter = value;
			}
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000B0BB9 File Offset: 0x000AEDB9
		public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(this._strDeformatter);
			asymmetricSignatureDeformatter.SetKey(key);
			return asymmetricSignatureDeformatter;
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000B0BD2 File Offset: 0x000AEDD2
		public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(this._strFormatter);
			asymmetricSignatureFormatter.SetKey(key);
			return asymmetricSignatureFormatter;
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000B0BEB File Offset: 0x000AEDEB
		public virtual HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(this._strDigest);
		}

		// Token: 0x040021E9 RID: 8681
		private string _strKey;

		// Token: 0x040021EA RID: 8682
		private string _strDigest;

		// Token: 0x040021EB RID: 8683
		private string _strFormatter;

		// Token: 0x040021EC RID: 8684
		private string _strDeformatter;
	}
}

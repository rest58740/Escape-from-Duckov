using System;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography
{
	// Token: 0x020004CE RID: 1230
	[ComVisible(true)]
	public class RSAPKCS1SignatureFormatter : AsymmetricSignatureFormatter
	{
		// Token: 0x0600313D RID: 12605 RVA: 0x000B5EB8 File Offset: 0x000B40B8
		public RSAPKCS1SignatureFormatter()
		{
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x000B5EC0 File Offset: 0x000B40C0
		public RSAPKCS1SignatureFormatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x000B5ED0 File Offset: 0x000B40D0
		public override byte[] CreateSignature(byte[] rgbHash)
		{
			if (this.rsa == null)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("No key pair available."));
			}
			if (this.hash == null)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("Missing hash algorithm."));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			return PKCS1.Sign_v15(this.rsa, this.hash, rgbHash);
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x000B5F2D File Offset: 0x000B412D
		public override void SetHashAlgorithm(string strName)
		{
			if (strName == null)
			{
				throw new ArgumentNullException("strName");
			}
			this.hash = strName;
		}

		// Token: 0x06003141 RID: 12609 RVA: 0x000B5F44 File Offset: 0x000B4144
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.rsa = (RSA)key;
		}

		// Token: 0x0400226F RID: 8815
		private RSA rsa;

		// Token: 0x04002270 RID: 8816
		private string hash;
	}
}

using System;
using System.Runtime.InteropServices;
using Mono.Security.Cryptography;

namespace System.Security.Cryptography
{
	// Token: 0x020004CD RID: 1229
	[ComVisible(true)]
	public class RSAPKCS1SignatureDeformatter : AsymmetricSignatureDeformatter
	{
		// Token: 0x06003138 RID: 12600 RVA: 0x000B5E01 File Offset: 0x000B4001
		public RSAPKCS1SignatureDeformatter()
		{
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000B5E09 File Offset: 0x000B4009
		public RSAPKCS1SignatureDeformatter(AsymmetricAlgorithm key)
		{
			this.SetKey(key);
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x000B5E18 File Offset: 0x000B4018
		public override void SetHashAlgorithm(string strName)
		{
			if (strName == null)
			{
				throw new ArgumentNullException("strName");
			}
			this.hashName = strName;
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x000B5E2F File Offset: 0x000B402F
		public override void SetKey(AsymmetricAlgorithm key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.rsa = (RSA)key;
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x000B5E4C File Offset: 0x000B404C
		public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
		{
			if (this.rsa == null)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("No public key available."));
			}
			if (this.hashName == null)
			{
				throw new CryptographicUnexpectedOperationException(Locale.GetText("Missing hash algorithm."));
			}
			if (rgbHash == null)
			{
				throw new ArgumentNullException("rgbHash");
			}
			if (rgbSignature == null)
			{
				throw new ArgumentNullException("rgbSignature");
			}
			return PKCS1.Verify_v15(this.rsa, this.hashName, rgbHash, rgbSignature);
		}

		// Token: 0x0400226D RID: 8813
		private RSA rsa;

		// Token: 0x0400226E RID: 8814
		private string hashName;
	}
}

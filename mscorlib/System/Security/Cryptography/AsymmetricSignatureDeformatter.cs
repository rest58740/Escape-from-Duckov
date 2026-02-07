using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200047B RID: 1147
	[ComVisible(true)]
	public abstract class AsymmetricSignatureDeformatter
	{
		// Token: 0x06002E6D RID: 11885
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06002E6E RID: 11886
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06002E6F RID: 11887 RVA: 0x000A6245 File Offset: 0x000A4445
		public virtual bool VerifySignature(HashAlgorithm hash, byte[] rgbSignature)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.VerifySignature(hash.Hash, rgbSignature);
		}

		// Token: 0x06002E70 RID: 11888
		public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);
	}
}

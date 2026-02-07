using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200047C RID: 1148
	[ComVisible(true)]
	public abstract class AsymmetricSignatureFormatter
	{
		// Token: 0x06002E72 RID: 11890
		public abstract void SetKey(AsymmetricAlgorithm key);

		// Token: 0x06002E73 RID: 11891
		public abstract void SetHashAlgorithm(string strName);

		// Token: 0x06002E74 RID: 11892 RVA: 0x000A626E File Offset: 0x000A446E
		public virtual byte[] CreateSignature(HashAlgorithm hash)
		{
			if (hash == null)
			{
				throw new ArgumentNullException("hash");
			}
			this.SetHashAlgorithm(hash.ToString());
			return this.CreateSignature(hash.Hash);
		}

		// Token: 0x06002E75 RID: 11893
		public abstract byte[] CreateSignature(byte[] rgbHash);
	}
}

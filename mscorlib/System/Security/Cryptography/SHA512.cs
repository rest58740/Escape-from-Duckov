using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004B6 RID: 1206
	[ComVisible(true)]
	public abstract class SHA512 : HashAlgorithm
	{
		// Token: 0x06003060 RID: 12384 RVA: 0x000B0392 File Offset: 0x000AE592
		protected SHA512()
		{
			this.HashSizeValue = 512;
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000B03A5 File Offset: 0x000AE5A5
		public new static SHA512 Create()
		{
			return SHA512.Create("System.Security.Cryptography.SHA512");
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000B03B1 File Offset: 0x000AE5B1
		public new static SHA512 Create(string hashName)
		{
			return (SHA512)CryptoConfig.CreateFromName(hashName);
		}
	}
}

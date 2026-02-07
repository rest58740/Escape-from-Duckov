using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004B2 RID: 1202
	[ComVisible(true)]
	public abstract class SHA256 : HashAlgorithm
	{
		// Token: 0x06003038 RID: 12344 RVA: 0x000AF498 File Offset: 0x000AD698
		protected SHA256()
		{
			this.HashSizeValue = 256;
		}

		// Token: 0x06003039 RID: 12345 RVA: 0x000AF4AB File Offset: 0x000AD6AB
		public new static SHA256 Create()
		{
			return SHA256.Create("System.Security.Cryptography.SHA256");
		}

		// Token: 0x0600303A RID: 12346 RVA: 0x000AF4B7 File Offset: 0x000AD6B7
		public new static SHA256 Create(string hashName)
		{
			return (SHA256)CryptoConfig.CreateFromName(hashName);
		}
	}
}

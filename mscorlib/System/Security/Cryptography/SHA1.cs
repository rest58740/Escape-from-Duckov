using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004B0 RID: 1200
	[ComVisible(true)]
	public abstract class SHA1 : HashAlgorithm
	{
		// Token: 0x0600302C RID: 12332 RVA: 0x000AAFC6 File Offset: 0x000A91C6
		protected SHA1()
		{
			this.HashSizeValue = 160;
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000AECD4 File Offset: 0x000ACED4
		public new static SHA1 Create()
		{
			return SHA1.Create("System.Security.Cryptography.SHA1");
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000AECE0 File Offset: 0x000ACEE0
		public new static SHA1 Create(string hashName)
		{
			return (SHA1)CryptoConfig.CreateFromName(hashName);
		}
	}
}

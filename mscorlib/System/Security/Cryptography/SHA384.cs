using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004B4 RID: 1204
	[ComVisible(true)]
	public abstract class SHA384 : HashAlgorithm
	{
		// Token: 0x0600304C RID: 12364 RVA: 0x000AFBFA File Offset: 0x000ADDFA
		protected SHA384()
		{
			this.HashSizeValue = 384;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000AFC0D File Offset: 0x000ADE0D
		public new static SHA384 Create()
		{
			return SHA384.Create("System.Security.Cryptography.SHA384");
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000AFC19 File Offset: 0x000ADE19
		public new static SHA384 Create(string hashName)
		{
			return (SHA384)CryptoConfig.CreateFromName(hashName);
		}
	}
}

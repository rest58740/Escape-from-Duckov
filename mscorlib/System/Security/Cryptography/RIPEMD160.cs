using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004A7 RID: 1191
	[ComVisible(true)]
	public abstract class RIPEMD160 : HashAlgorithm
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x000AAFC6 File Offset: 0x000A91C6
		protected RIPEMD160()
		{
			this.HashSizeValue = 160;
		}

		// Token: 0x06002FA3 RID: 12195 RVA: 0x000AAFD9 File Offset: 0x000A91D9
		public new static RIPEMD160 Create()
		{
			return RIPEMD160.Create("System.Security.Cryptography.RIPEMD160");
		}

		// Token: 0x06002FA4 RID: 12196 RVA: 0x000AAFE5 File Offset: 0x000A91E5
		public new static RIPEMD160 Create(string hashName)
		{
			return (RIPEMD160)CryptoConfig.CreateFromName(hashName);
		}
	}
}

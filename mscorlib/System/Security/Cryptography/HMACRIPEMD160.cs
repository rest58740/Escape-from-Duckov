using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000490 RID: 1168
	[ComVisible(true)]
	public class HMACRIPEMD160 : HMAC
	{
		// Token: 0x06002EFB RID: 12027 RVA: 0x000A7CAE File Offset: 0x000A5EAE
		public HMACRIPEMD160() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x000A7CBD File Offset: 0x000A5EBD
		public HMACRIPEMD160(byte[] key)
		{
			this.m_hashName = "RIPEMD160";
			this.m_hash1 = new RIPEMD160Managed();
			this.m_hash2 = new RIPEMD160Managed();
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}

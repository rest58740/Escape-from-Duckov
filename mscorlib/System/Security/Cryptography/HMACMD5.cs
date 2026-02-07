using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200048F RID: 1167
	[ComVisible(true)]
	public class HMACMD5 : HMAC
	{
		// Token: 0x06002EF9 RID: 12025 RVA: 0x000A7C64 File Offset: 0x000A5E64
		public HMACMD5() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x000A7C73 File Offset: 0x000A5E73
		public HMACMD5(byte[] key)
		{
			this.m_hashName = "MD5";
			this.m_hash1 = new MD5CryptoServiceProvider();
			this.m_hash2 = new MD5CryptoServiceProvider();
			this.HashSizeValue = 128;
			base.InitializeKey(key);
		}
	}
}

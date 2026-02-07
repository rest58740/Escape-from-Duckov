using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000491 RID: 1169
	[ComVisible(true)]
	public class HMACSHA1 : HMAC
	{
		// Token: 0x06002EFD RID: 12029 RVA: 0x000A7CF8 File Offset: 0x000A5EF8
		public HMACSHA1() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000A7D07 File Offset: 0x000A5F07
		public HMACSHA1(byte[] key) : this(key, false)
		{
		}

		// Token: 0x06002EFF RID: 12031 RVA: 0x000A7D14 File Offset: 0x000A5F14
		public HMACSHA1(byte[] key, bool useManagedSha1)
		{
			this.m_hashName = "SHA1";
			if (useManagedSha1)
			{
				this.m_hash1 = new SHA1Managed();
				this.m_hash2 = new SHA1Managed();
			}
			else
			{
				this.m_hash1 = new SHA1CryptoServiceProvider();
				this.m_hash2 = new SHA1CryptoServiceProvider();
			}
			this.HashSizeValue = 160;
			base.InitializeKey(key);
		}
	}
}

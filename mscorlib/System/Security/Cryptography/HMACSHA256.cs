using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000492 RID: 1170
	[ComVisible(true)]
	public class HMACSHA256 : HMAC
	{
		// Token: 0x06002F00 RID: 12032 RVA: 0x000A7D75 File Offset: 0x000A5F75
		public HMACSHA256() : this(Utils.GenerateRandom(64))
		{
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000A7D84 File Offset: 0x000A5F84
		public HMACSHA256(byte[] key)
		{
			this.m_hashName = "SHA256";
			this.m_hash1 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA256Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA256CryptoServiceProvider"));
			this.m_hash2 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA256Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA256CryptoServiceProvider"));
			this.HashSizeValue = 256;
			base.InitializeKey(key);
		}
	}
}

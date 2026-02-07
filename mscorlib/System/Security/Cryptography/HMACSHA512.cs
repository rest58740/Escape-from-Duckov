using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000496 RID: 1174
	[ComVisible(true)]
	public class HMACSHA512 : HMAC
	{
		// Token: 0x06002F13 RID: 12051 RVA: 0x000A7FAB File Offset: 0x000A61AB
		public HMACSHA512() : this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000A7FC0 File Offset: 0x000A61C0
		[SecuritySafeCritical]
		public HMACSHA512(byte[] key)
		{
			this.m_hashName = "SHA512";
			this.m_hash1 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA512Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA512CryptoServiceProvider"));
			this.m_hash2 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA512Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA512CryptoServiceProvider"));
			this.HashSizeValue = 512;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002F15 RID: 12053 RVA: 0x000A8099 File Offset: 0x000A6299
		private int BlockSize
		{
			get
			{
				if (!this.m_useLegacyBlockSize)
				{
					return 128;
				}
				return 64;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002F16 RID: 12054 RVA: 0x000A80AB File Offset: 0x000A62AB
		// (set) Token: 0x06002F17 RID: 12055 RVA: 0x000A80B3 File Offset: 0x000A62B3
		public bool ProduceLegacyHmacValues
		{
			get
			{
				return this.m_useLegacyBlockSize;
			}
			set
			{
				this.m_useLegacyBlockSize = value;
				base.BlockSizeValue = this.BlockSize;
				base.InitializeKey(this.KeyValue);
			}
		}

		// Token: 0x04002177 RID: 8567
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000494 RID: 1172
	[ComVisible(true)]
	public class HMACSHA384 : HMAC
	{
		// Token: 0x06002F08 RID: 12040 RVA: 0x000A7E65 File Offset: 0x000A6065
		public HMACSHA384() : this(Utils.GenerateRandom(128))
		{
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000A7E78 File Offset: 0x000A6078
		[SecuritySafeCritical]
		public HMACSHA384(byte[] key)
		{
			this.m_hashName = "SHA384";
			this.m_hash1 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
			this.m_hash2 = HMAC.GetHashAlgorithmWithFipsFallback(() => new SHA384Managed(), () => HashAlgorithm.Create("System.Security.Cryptography.SHA384CryptoServiceProvider"));
			this.HashSizeValue = 384;
			base.BlockSizeValue = this.BlockSize;
			base.InitializeKey(key);
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x06002F0A RID: 12042 RVA: 0x000A7F51 File Offset: 0x000A6151
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

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x000A7F63 File Offset: 0x000A6163
		// (set) Token: 0x06002F0C RID: 12044 RVA: 0x000A7F6B File Offset: 0x000A616B
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

		// Token: 0x04002171 RID: 8561
		private bool m_useLegacyBlockSize = Utils._ProduceLegacyHmacValues();
	}
}

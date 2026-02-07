using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x020004A2 RID: 1186
	[ComVisible(true)]
	public sealed class RC2CryptoServiceProvider : RC2
	{
		// Token: 0x06002F76 RID: 12150 RVA: 0x000A902C File Offset: 0x000A722C
		[SecuritySafeCritical]
		public RC2CryptoServiceProvider()
		{
			if (CryptoConfig.AllowOnlyFipsAlgorithms)
			{
				throw new InvalidOperationException(Environment.GetResourceString("This implementation is not part of the Windows Platform FIPS validated cryptographic algorithms."));
			}
			if (!Utils.HasAlgorithm(26114, 0))
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptographic service provider (CSP) could not be found for this algorithm."));
			}
			this.LegalKeySizesValue = RC2CryptoServiceProvider.s_legalKeySizes;
			this.FeedbackSizeValue = 8;
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002F77 RID: 12151 RVA: 0x000A8FB2 File Offset: 0x000A71B2
		// (set) Token: 0x06002F78 RID: 12152 RVA: 0x000A9085 File Offset: 0x000A7285
		public override int EffectiveKeySize
		{
			get
			{
				return this.KeySizeValue;
			}
			set
			{
				if (value != this.KeySizeValue)
				{
					throw new CryptographicUnexpectedOperationException(Environment.GetResourceString("EffectiveKeySize must be the same as KeySize in this implementation."));
				}
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002F79 RID: 12153 RVA: 0x000A90A0 File Offset: 0x000A72A0
		// (set) Token: 0x06002F7A RID: 12154 RVA: 0x000A90A8 File Offset: 0x000A72A8
		[ComVisible(false)]
		public bool UseSalt
		{
			get
			{
				return this.m_use40bitSalt;
			}
			set
			{
				this.m_use40bitSalt = value;
			}
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000A90B1 File Offset: 0x000A72B1
		[SecuritySafeCritical]
		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			if (this.m_use40bitSalt)
			{
				throw new NotImplementedException("UseSalt=true is not implemented on Mono yet");
			}
			return new RC2Transform(this, true, rgbKey, rgbIV);
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000A90CF File Offset: 0x000A72CF
		[SecuritySafeCritical]
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			if (this.m_use40bitSalt)
			{
				throw new NotImplementedException("UseSalt=true is not implemented on Mono yet");
			}
			return new RC2Transform(this, false, rgbKey, rgbIV);
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x000A90ED File Offset: 0x000A72ED
		public override void GenerateKey()
		{
			this.KeyValue = new byte[this.KeySizeValue / 8];
			Utils.StaticRandomNumberGenerator.GetBytes(this.KeyValue);
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000A6F1D File Offset: 0x000A511D
		public override void GenerateIV()
		{
			this.IVValue = new byte[8];
			Utils.StaticRandomNumberGenerator.GetBytes(this.IVValue);
		}

		// Token: 0x04002195 RID: 8597
		private bool m_use40bitSalt;

		// Token: 0x04002196 RID: 8598
		private static KeySizes[] s_legalKeySizes = new KeySizes[]
		{
			new KeySizes(40, 128, 8)
		};
	}
}

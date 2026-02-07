using System;
using System.Security.Cryptography;

namespace ICSharpCode.SharpZipLib.Encryption
{
	// Token: 0x0200006D RID: 109
	public sealed class PkzipClassicManaged : PkzipClassic
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000191BC File Offset: 0x000173BC
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x000191C0 File Offset: 0x000173C0
		public override int BlockSize
		{
			get
			{
				return 8;
			}
			set
			{
				if (value != 8)
				{
					throw new CryptographicException("Block size is invalid");
				}
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x000191D4 File Offset: 0x000173D4
		public override KeySizes[] LegalKeySizes
		{
			get
			{
				return new KeySizes[]
				{
					new KeySizes(96, 96, 0)
				};
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x000191F8 File Offset: 0x000173F8
		public override void GenerateIV()
		{
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x000191FC File Offset: 0x000173FC
		public override KeySizes[] LegalBlockSizes
		{
			get
			{
				return new KeySizes[]
				{
					new KeySizes(8, 8, 0)
				};
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001921C File Offset: 0x0001741C
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00019240 File Offset: 0x00017440
		public override byte[] Key
		{
			get
			{
				if (this.key_ == null)
				{
					this.GenerateKey();
				}
				return (byte[])this.key_.Clone();
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length != 12)
				{
					throw new CryptographicException("Key size is illegal");
				}
				this.key_ = (byte[])value.Clone();
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001927C File Offset: 0x0001747C
		public override void GenerateKey()
		{
			this.key_ = new byte[12];
			Random random = new Random();
			random.NextBytes(this.key_);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x000192A8 File Offset: 0x000174A8
		public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
		{
			this.key_ = rgbKey;
			return new PkzipClassicEncryptCryptoTransform(this.Key);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x000192BC File Offset: 0x000174BC
		public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
		{
			this.key_ = rgbKey;
			return new PkzipClassicDecryptCryptoTransform(this.Key);
		}

		// Token: 0x040002F1 RID: 753
		private byte[] key_;
	}
}

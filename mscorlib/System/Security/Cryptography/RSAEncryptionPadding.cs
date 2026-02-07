using System;
using Unity;

namespace System.Security.Cryptography
{
	// Token: 0x02000473 RID: 1139
	public sealed class RSAEncryptionPadding : IEquatable<RSAEncryptionPadding>
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002E27 RID: 11815 RVA: 0x000A5E94 File Offset: 0x000A4094
		public static RSAEncryptionPadding Pkcs1
		{
			get
			{
				return RSAEncryptionPadding.s_pkcs1;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002E28 RID: 11816 RVA: 0x000A5E9B File Offset: 0x000A409B
		public static RSAEncryptionPadding OaepSHA1
		{
			get
			{
				return RSAEncryptionPadding.s_oaepSHA1;
			}
		}

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06002E29 RID: 11817 RVA: 0x000A5EA2 File Offset: 0x000A40A2
		public static RSAEncryptionPadding OaepSHA256
		{
			get
			{
				return RSAEncryptionPadding.s_oaepSHA256;
			}
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x000A5EA9 File Offset: 0x000A40A9
		public static RSAEncryptionPadding OaepSHA384
		{
			get
			{
				return RSAEncryptionPadding.s_oaepSHA384;
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06002E2B RID: 11819 RVA: 0x000A5EB0 File Offset: 0x000A40B0
		public static RSAEncryptionPadding OaepSHA512
		{
			get
			{
				return RSAEncryptionPadding.s_oaepSHA512;
			}
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x000A5EB7 File Offset: 0x000A40B7
		private RSAEncryptionPadding(RSAEncryptionPaddingMode mode, HashAlgorithmName oaepHashAlgorithm)
		{
			this._mode = mode;
			this._oaepHashAlgorithm = oaepHashAlgorithm;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x000A5ECD File Offset: 0x000A40CD
		public static RSAEncryptionPadding CreateOaep(HashAlgorithmName hashAlgorithm)
		{
			if (string.IsNullOrEmpty(hashAlgorithm.Name))
			{
				throw new ArgumentException(Environment.GetResourceString("The hash algorithm name cannot be null or empty."), "hashAlgorithm");
			}
			return new RSAEncryptionPadding(RSAEncryptionPaddingMode.Oaep, hashAlgorithm);
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000A5EF9 File Offset: 0x000A40F9
		public RSAEncryptionPaddingMode Mode
		{
			get
			{
				return this._mode;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002E2F RID: 11823 RVA: 0x000A5F01 File Offset: 0x000A4101
		public HashAlgorithmName OaepHashAlgorithm
		{
			get
			{
				return this._oaepHashAlgorithm;
			}
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x000A5F09 File Offset: 0x000A4109
		public override int GetHashCode()
		{
			return RSAEncryptionPadding.CombineHashCodes(this._mode.GetHashCode(), this._oaepHashAlgorithm.GetHashCode());
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00033E4F File Offset: 0x0003204F
		private static int CombineHashCodes(int h1, int h2)
		{
			return (h1 << 5) + h1 ^ h2;
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x000A5F32 File Offset: 0x000A4132
		public override bool Equals(object obj)
		{
			return this.Equals(obj as RSAEncryptionPadding);
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x000A5F40 File Offset: 0x000A4140
		public bool Equals(RSAEncryptionPadding other)
		{
			return other != null && this._mode == other._mode && this._oaepHashAlgorithm == other._oaepHashAlgorithm;
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x000A5F6C File Offset: 0x000A416C
		public static bool operator ==(RSAEncryptionPadding left, RSAEncryptionPadding right)
		{
			if (left == null)
			{
				return right == null;
			}
			return left.Equals(right);
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x000A5F7D File Offset: 0x000A417D
		public static bool operator !=(RSAEncryptionPadding left, RSAEncryptionPadding right)
		{
			return !(left == right);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000A5F89 File Offset: 0x000A4189
		public override string ToString()
		{
			return this._mode.ToString() + this._oaepHashAlgorithm.Name;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000173AD File Offset: 0x000155AD
		internal RSAEncryptionPadding()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002119 RID: 8473
		private static readonly RSAEncryptionPadding s_pkcs1 = new RSAEncryptionPadding(RSAEncryptionPaddingMode.Pkcs1, default(HashAlgorithmName));

		// Token: 0x0400211A RID: 8474
		private static readonly RSAEncryptionPadding s_oaepSHA1 = RSAEncryptionPadding.CreateOaep(HashAlgorithmName.SHA1);

		// Token: 0x0400211B RID: 8475
		private static readonly RSAEncryptionPadding s_oaepSHA256 = RSAEncryptionPadding.CreateOaep(HashAlgorithmName.SHA256);

		// Token: 0x0400211C RID: 8476
		private static readonly RSAEncryptionPadding s_oaepSHA384 = RSAEncryptionPadding.CreateOaep(HashAlgorithmName.SHA384);

		// Token: 0x0400211D RID: 8477
		private static readonly RSAEncryptionPadding s_oaepSHA512 = RSAEncryptionPadding.CreateOaep(HashAlgorithmName.SHA512);

		// Token: 0x0400211E RID: 8478
		private RSAEncryptionPaddingMode _mode;

		// Token: 0x0400211F RID: 8479
		private HashAlgorithmName _oaepHashAlgorithm;
	}
}

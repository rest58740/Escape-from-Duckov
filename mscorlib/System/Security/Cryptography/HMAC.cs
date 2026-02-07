using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200048E RID: 1166
	[ComVisible(true)]
	public abstract class HMAC : KeyedHashAlgorithm
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x000A78BF File Offset: 0x000A5ABF
		// (set) Token: 0x06002EEA RID: 12010 RVA: 0x000A78C7 File Offset: 0x000A5AC7
		protected int BlockSizeValue
		{
			get
			{
				return this.blockSizeValue;
			}
			set
			{
				this.blockSizeValue = value;
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000A78D0 File Offset: 0x000A5AD0
		private void UpdateIOPadBuffers()
		{
			if (this.m_inner == null)
			{
				this.m_inner = new byte[this.BlockSizeValue];
			}
			if (this.m_outer == null)
			{
				this.m_outer = new byte[this.BlockSizeValue];
			}
			for (int i = 0; i < this.BlockSizeValue; i++)
			{
				this.m_inner[i] = 54;
				this.m_outer[i] = 92;
			}
			for (int i = 0; i < this.KeyValue.Length; i++)
			{
				byte[] inner = this.m_inner;
				int num = i;
				inner[num] ^= this.KeyValue[i];
				byte[] outer = this.m_outer;
				int num2 = i;
				outer[num2] ^= this.KeyValue[i];
			}
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000A797C File Offset: 0x000A5B7C
		internal void InitializeKey(byte[] key)
		{
			this.m_inner = null;
			this.m_outer = null;
			if (key.Length > this.BlockSizeValue)
			{
				this.KeyValue = this.m_hash1.ComputeHash(key);
			}
			else
			{
				this.KeyValue = (byte[])key.Clone();
			}
			this.UpdateIOPadBuffers();
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06002EED RID: 12013 RVA: 0x000A79CD File Offset: 0x000A5BCD
		// (set) Token: 0x06002EEE RID: 12014 RVA: 0x000A79DF File Offset: 0x000A5BDF
		public override byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Hash key cannot be changed after the first write to the stream."));
				}
				this.InitializeKey(value);
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000A7A00 File Offset: 0x000A5C00
		// (set) Token: 0x06002EF0 RID: 12016 RVA: 0x000A7A08 File Offset: 0x000A5C08
		public string HashName
		{
			get
			{
				return this.m_hashName;
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Hash name cannot be changed after the first write to the stream."));
				}
				this.m_hashName = value;
				this.m_hash1 = HashAlgorithm.Create(this.m_hashName);
				this.m_hash2 = HashAlgorithm.Create(this.m_hashName);
			}
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000A7A56 File Offset: 0x000A5C56
		public new static HMAC Create()
		{
			return HMAC.Create("System.Security.Cryptography.HMAC");
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000A7A62 File Offset: 0x000A5C62
		public new static HMAC Create(string algorithmName)
		{
			return (HMAC)CryptoConfig.CreateFromName(algorithmName);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000A7A6F File Offset: 0x000A5C6F
		public override void Initialize()
		{
			this.m_hash1.Initialize();
			this.m_hash2.Initialize();
			this.m_hashing = false;
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000A7A90 File Offset: 0x000A5C90
		protected override void HashCore(byte[] rgb, int ib, int cb)
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformBlock(rgb, ib, cb, rgb, ib);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000A7AE0 File Offset: 0x000A5CE0
		protected override byte[] HashFinal()
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformFinalBlock(EmptyArray<byte>.Value, 0, 0);
			byte[] hashValue = this.m_hash1.HashValue;
			this.m_hash2.TransformBlock(this.m_outer, 0, this.m_outer.Length, this.m_outer, 0);
			this.m_hash2.TransformBlock(hashValue, 0, hashValue.Length, hashValue, 0);
			this.m_hashing = false;
			this.m_hash2.TransformFinalBlock(EmptyArray<byte>.Value, 0, 0);
			return this.m_hash2.HashValue;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000A7B98 File Offset: 0x000A5D98
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_hash1 != null)
				{
					((IDisposable)this.m_hash1).Dispose();
				}
				if (this.m_hash2 != null)
				{
					((IDisposable)this.m_hash2).Dispose();
				}
				if (this.m_inner != null)
				{
					Array.Clear(this.m_inner, 0, this.m_inner.Length);
				}
				if (this.m_outer != null)
				{
					Array.Clear(this.m_outer, 0, this.m_outer.Length);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000A7C10 File Offset: 0x000A5E10
		internal static HashAlgorithm GetHashAlgorithmWithFipsFallback(Func<HashAlgorithm> createStandardHashAlgorithmCallback, Func<HashAlgorithm> createFipsHashAlgorithmCallback)
		{
			if (CryptoConfig.AllowOnlyFipsAlgorithms)
			{
				try
				{
					return createFipsHashAlgorithmCallback();
				}
				catch (PlatformNotSupportedException ex)
				{
					throw new InvalidOperationException(ex.Message, ex);
				}
			}
			return createStandardHashAlgorithmCallback();
		}

		// Token: 0x04002165 RID: 8549
		private int blockSizeValue = 64;

		// Token: 0x04002166 RID: 8550
		internal string m_hashName;

		// Token: 0x04002167 RID: 8551
		internal HashAlgorithm m_hash1;

		// Token: 0x04002168 RID: 8552
		internal HashAlgorithm m_hash2;

		// Token: 0x04002169 RID: 8553
		private byte[] m_inner;

		// Token: 0x0400216A RID: 8554
		private byte[] m_outer;

		// Token: 0x0400216B RID: 8555
		private bool m_hashing;
	}
}

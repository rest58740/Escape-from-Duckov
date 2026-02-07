using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200049A RID: 1178
	[ComVisible(true)]
	public class MACTripleDES : KeyedHashAlgorithm
	{
		// Token: 0x06002F2A RID: 12074 RVA: 0x000A8170 File Offset: 0x000A6370
		public MACTripleDES()
		{
			this.KeyValue = new byte[24];
			Utils.StaticRandomNumberGenerator.GetBytes(this.KeyValue);
			this.des = TripleDES.Create();
			this.HashSizeValue = this.des.BlockSize;
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x000A81F8 File Offset: 0x000A63F8
		public MACTripleDES(byte[] rgbKey) : this("System.Security.Cryptography.TripleDES", rgbKey)
		{
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x000A8208 File Offset: 0x000A6408
		public MACTripleDES(string strTripleDES, byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (strTripleDES == null)
			{
				this.des = TripleDES.Create();
			}
			else
			{
				this.des = TripleDES.Create(strTripleDES);
			}
			this.HashSizeValue = this.des.BlockSize;
			this.KeyValue = (byte[])rgbKey.Clone();
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x000A82A3 File Offset: 0x000A64A3
		public override void Initialize()
		{
			this.m_encryptor = null;
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002F2E RID: 12078 RVA: 0x000A82AC File Offset: 0x000A64AC
		// (set) Token: 0x06002F2F RID: 12079 RVA: 0x000A82B9 File Offset: 0x000A64B9
		[ComVisible(false)]
		public PaddingMode Padding
		{
			get
			{
				return this.des.Padding;
			}
			set
			{
				if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Specified padding mode is not valid for this algorithm."));
				}
				this.des.Padding = value;
			}
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x000A82E0 File Offset: 0x000A64E0
		protected override void HashCore(byte[] rgbData, int ibStart, int cbSize)
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.Write(rgbData, ibStart, cbSize);
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000A8358 File Offset: 0x000A6558
		protected override byte[] HashFinal()
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.FlushFinalBlock();
			return this._ts.Buffer;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000A83D8 File Offset: 0x000A65D8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.des != null)
				{
					this.des.Clear();
				}
				if (this.m_encryptor != null)
				{
					this.m_encryptor.Dispose();
				}
				if (this._cs != null)
				{
					this._cs.Clear();
				}
				if (this._ts != null)
				{
					this._ts.Clear();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x0400217E RID: 8574
		private ICryptoTransform m_encryptor;

		// Token: 0x0400217F RID: 8575
		private CryptoStream _cs;

		// Token: 0x04002180 RID: 8576
		private TailStream _ts;

		// Token: 0x04002181 RID: 8577
		private const int m_bitsPerByte = 8;

		// Token: 0x04002182 RID: 8578
		private int m_bytesPerBlock;

		// Token: 0x04002183 RID: 8579
		private TripleDES des;
	}
}

using System;
using System.IO;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Encryption;

namespace ICSharpCode.SharpZipLib.Zip.Compression.Streams
{
	// Token: 0x02000004 RID: 4
	public class DeflaterOutputStream : Stream
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002804 File Offset: 0x00000A04
		public DeflaterOutputStream(Stream baseOutputStream) : this(baseOutputStream, new Deflater(), 512)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002818 File Offset: 0x00000A18
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater) : this(baseOutputStream, deflater, 512)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002828 File Offset: 0x00000A28
		public DeflaterOutputStream(Stream baseOutputStream, Deflater deflater, int bufferSize)
		{
			if (baseOutputStream == null)
			{
				throw new ArgumentNullException("baseOutputStream");
			}
			if (!baseOutputStream.CanWrite)
			{
				throw new ArgumentException("Must support writing", "baseOutputStream");
			}
			if (deflater == null)
			{
				throw new ArgumentNullException("deflater");
			}
			if (bufferSize < 512)
			{
				throw new ArgumentOutOfRangeException("bufferSize");
			}
			this.baseOutputStream_ = baseOutputStream;
			this.buffer_ = new byte[bufferSize];
			this.deflater_ = deflater;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000028B0 File Offset: 0x00000AB0
		public virtual void Finish()
		{
			this.deflater_.Finish();
			while (!this.deflater_.IsFinished)
			{
				int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (this.cryptoTransform_ != null)
				{
					this.EncryptBlock(this.buffer_, 0, num);
				}
				this.baseOutputStream_.Write(this.buffer_, 0, num);
			}
			if (!this.deflater_.IsFinished)
			{
				throw new SharpZipBaseException("Can't deflate all input?");
			}
			this.baseOutputStream_.Flush();
			if (this.cryptoTransform_ != null)
			{
				this.cryptoTransform_.Dispose();
				this.cryptoTransform_ = null;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002974 File Offset: 0x00000B74
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000297C File Offset: 0x00000B7C
		public bool IsStreamOwner
		{
			get
			{
				return this.isStreamOwner_;
			}
			set
			{
				this.isStreamOwner_ = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002988 File Offset: 0x00000B88
		public bool CanPatchEntries
		{
			get
			{
				return this.baseOutputStream_.CanSeek;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002998 File Offset: 0x00000B98
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000029A0 File Offset: 0x00000BA0
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				if (value != null && value.Length == 0)
				{
					this.password = null;
				}
				else
				{
					this.password = value;
				}
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000029D4 File Offset: 0x00000BD4
		protected void EncryptBlock(byte[] buffer, int offset, int length)
		{
			this.cryptoTransform_.TransformBlock(buffer, 0, length, buffer, 0);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000029E8 File Offset: 0x00000BE8
		protected void InitializePassword(string password)
		{
			PkzipClassicManaged pkzipClassicManaged = new PkzipClassicManaged();
			byte[] rgbKey = PkzipClassic.GenerateKeys(ZipConstants.ConvertToArray(password));
			this.cryptoTransform_ = pkzipClassicManaged.CreateEncryptor(rgbKey, null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A18 File Offset: 0x00000C18
		protected void Deflate()
		{
			while (!this.deflater_.IsNeedingInput)
			{
				int num = this.deflater_.Deflate(this.buffer_, 0, this.buffer_.Length);
				if (num <= 0)
				{
					break;
				}
				if (this.cryptoTransform_ != null)
				{
					this.EncryptBlock(this.buffer_, 0, num);
				}
				this.baseOutputStream_.Write(this.buffer_, 0, num);
			}
			if (!this.deflater_.IsNeedingInput)
			{
				throw new SharpZipBaseException("DeflaterOutputStream can't deflate all input?");
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002AA8 File Offset: 0x00000CA8
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002AAC File Offset: 0x00000CAC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public override bool CanWrite
		{
			get
			{
				return this.baseOutputStream_.CanWrite;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002AC0 File Offset: 0x00000CC0
		public override long Length
		{
			get
			{
				return this.baseOutputStream_.Length;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002AD0 File Offset: 0x00000CD0
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002AE0 File Offset: 0x00000CE0
		public override long Position
		{
			get
			{
				return this.baseOutputStream_.Position;
			}
			set
			{
				throw new NotSupportedException("Position property not supported");
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002AEC File Offset: 0x00000CEC
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException("DeflaterOutputStream Seek not supported");
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public override void SetLength(long value)
		{
			throw new NotSupportedException("DeflaterOutputStream SetLength not supported");
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B04 File Offset: 0x00000D04
		public override int ReadByte()
		{
			throw new NotSupportedException("DeflaterOutputStream ReadByte not supported");
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B10 File Offset: 0x00000D10
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("DeflaterOutputStream Read not supported");
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002B1C File Offset: 0x00000D1C
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("DeflaterOutputStream BeginRead not currently supported");
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002B28 File Offset: 0x00000D28
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			throw new NotSupportedException("BeginWrite is not supported");
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002B34 File Offset: 0x00000D34
		public override void Flush()
		{
			this.deflater_.Flush();
			this.Deflate();
			this.baseOutputStream_.Flush();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002B54 File Offset: 0x00000D54
		public override void Close()
		{
			if (!this.isClosed_)
			{
				this.isClosed_ = true;
				try
				{
					this.Finish();
					if (this.cryptoTransform_ != null)
					{
						this.GetAuthCodeIfAES();
						this.cryptoTransform_.Dispose();
						this.cryptoTransform_ = null;
					}
				}
				finally
				{
					if (this.isStreamOwner_)
					{
						this.baseOutputStream_.Close();
					}
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002BD4 File Offset: 0x00000DD4
		private void GetAuthCodeIfAES()
		{
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002BD8 File Offset: 0x00000DD8
		public override void WriteByte(byte value)
		{
			this.Write(new byte[]
			{
				value
			}, 0, 1);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002BFC File Offset: 0x00000DFC
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.deflater_.SetInput(buffer, offset, count);
			this.Deflate();
		}

		// Token: 0x0400000F RID: 15
		private string password;

		// Token: 0x04000010 RID: 16
		private ICryptoTransform cryptoTransform_;

		// Token: 0x04000011 RID: 17
		protected byte[] AESAuthCode;

		// Token: 0x04000012 RID: 18
		private byte[] buffer_;

		// Token: 0x04000013 RID: 19
		protected Deflater deflater_;

		// Token: 0x04000014 RID: 20
		protected Stream baseOutputStream_;

		// Token: 0x04000015 RID: 21
		private bool isClosed_;

		// Token: 0x04000016 RID: 22
		private bool isStreamOwner_ = true;
	}
}

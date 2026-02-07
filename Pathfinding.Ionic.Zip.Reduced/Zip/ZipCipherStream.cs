using System;
using System.IO;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000029 RID: 41
	internal class ZipCipherStream : Stream
	{
		// Token: 0x060000D0 RID: 208 RVA: 0x00004D90 File Offset: 0x00002F90
		public ZipCipherStream(Stream s, ZipCrypto cipher, CryptoMode mode)
		{
			this._cipher = cipher;
			this._s = s;
			this._mode = mode;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Encrypt)
			{
				throw new NotSupportedException("This stream does not encrypt via Read()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			byte[] array = new byte[count];
			int num = this._s.Read(array, 0, count);
			byte[] array2 = this._cipher.DecryptMessage(array, num);
			for (int i = 0; i < num; i++)
			{
				buffer[offset + i] = array2[i];
			}
			return num;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004E24 File Offset: 0x00003024
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._mode == CryptoMode.Decrypt)
			{
				throw new NotSupportedException("This stream does not Decrypt via Write()");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count == 0)
			{
				return;
			}
			byte[] array;
			if (offset != 0)
			{
				array = new byte[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = buffer[offset + i];
				}
			}
			else
			{
				array = buffer;
			}
			byte[] array2 = this._cipher.EncryptMessage(array, count);
			this._s.Write(array2, 0, array2.Length);
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004EB0 File Offset: 0x000030B0
		public override bool CanRead
		{
			get
			{
				return this._mode == CryptoMode.Decrypt;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00004EBC File Offset: 0x000030BC
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00004EC0 File Offset: 0x000030C0
		public override bool CanWrite
		{
			get
			{
				return this._mode == CryptoMode.Encrypt;
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004ECC File Offset: 0x000030CC
		public override void Flush()
		{
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00004ED0 File Offset: 0x000030D0
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00004ED8 File Offset: 0x000030D8
		// (set) Token: 0x060000D9 RID: 217 RVA: 0x00004EE0 File Offset: 0x000030E0
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004EE8 File Offset: 0x000030E8
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004EF0 File Offset: 0x000030F0
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04000075 RID: 117
		private ZipCrypto _cipher;

		// Token: 0x04000076 RID: 118
		private Stream _s;

		// Token: 0x04000077 RID: 119
		private CryptoMode _mode;
	}
}

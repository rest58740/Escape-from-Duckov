using System;
using System.IO;

namespace Pathfinding.Ionic.Crc
{
	// Token: 0x0200006B RID: 107
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0001F914 File Offset: 0x0001DB14
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001F924 File Offset: 0x0001DB24
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001F934 File Offset: 0x0001DB34
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001F954 File Offset: 0x0001DB54
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001F974 File Offset: 0x0001DB74
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001F994 File Offset: 0x0001DB94
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			this._innerStream = stream;
			this._Crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001F9E8 File Offset: 0x0001DBE8
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0001F9F0 File Offset: 0x0001DBF0
		public long TotalBytesSlurped
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001FA00 File Offset: 0x0001DC00
		public int Crc
		{
			get
			{
				return this._Crc32.Crc32Result;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001FA10 File Offset: 0x0001DC10
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0001FA18 File Offset: 0x0001DC18
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001FA24 File Offset: 0x0001DC24
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._Crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num2 = this._lengthLimit - this._Crc32.TotalBytesRead;
				if (num2 < (long)count)
				{
					num = (int)num2;
				}
			}
			int num3 = this._innerStream.Read(buffer, offset, num);
			if (num3 > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, num3);
			}
			return num3;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001FAA0 File Offset: 0x0001DCA0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._Crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001FAD8 File Offset: 0x0001DCD8
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001FADC File Offset: 0x0001DCDC
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001FAEC File Offset: 0x0001DCEC
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001FAFC File Offset: 0x0001DCFC
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004A8 RID: 1192 RVA: 0x0001FB2C File Offset: 0x0001DD2C
		// (set) Token: 0x060004A9 RID: 1193 RVA: 0x0001FB3C File Offset: 0x0001DD3C
		public override long Position
		{
			get
			{
				return this._Crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001FB44 File Offset: 0x0001DD44
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001FB4C File Offset: 0x0001DD4C
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001FB54 File Offset: 0x0001DD54
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x0400039E RID: 926
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x0400039F RID: 927
		internal Stream _innerStream;

		// Token: 0x040003A0 RID: 928
		private CRC32 _Crc32;

		// Token: 0x040003A1 RID: 929
		private long _lengthLimit = -99L;

		// Token: 0x040003A2 RID: 930
		private bool _leaveOpen;
	}
}

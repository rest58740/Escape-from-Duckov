using System;
using System.Runtime.InteropServices;

namespace Pathfinding.Ionic.Zlib
{
	// Token: 0x02000067 RID: 103
	[ClassInterface(1)]
	[ComVisible(true)]
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	public sealed class ZlibCodec
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x0001EA30 File Offset: 0x0001CC30
		public ZlibCodec()
		{
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001EA48 File Offset: 0x0001CC48
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				int num = this.InitializeDeflate();
				if (num != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				int num2 = this.InitializeInflate();
				if (num2 != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001EABC File Offset: 0x0001CCBC
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001EAC4 File Offset: 0x0001CCC4
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001EAE4 File Offset: 0x0001CCE4
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001EAF8 File Offset: 0x0001CCF8
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001EB3C File Offset: 0x0001CD3C
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001EB6C File Offset: 0x0001CD6C
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001EBD4 File Offset: 0x0001CDD4
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001EBE4 File Offset: 0x0001CDE4
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001EC0C File Offset: 0x0001CE0C
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001EC24 File Offset: 0x0001CE24
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001EC7C File Offset: 0x0001CE7C
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001ECAC File Offset: 0x0001CEAC
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0001ECCC File Offset: 0x0001CECC
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0001ED18 File Offset: 0x0001CF18
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0001ED60 File Offset: 0x0001CF60
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001EEC0 File Offset: 0x0001D0C0
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x0400037D RID: 893
		public byte[] InputBuffer;

		// Token: 0x0400037E RID: 894
		public int NextIn;

		// Token: 0x0400037F RID: 895
		public int AvailableBytesIn;

		// Token: 0x04000380 RID: 896
		public long TotalBytesIn;

		// Token: 0x04000381 RID: 897
		public byte[] OutputBuffer;

		// Token: 0x04000382 RID: 898
		public int NextOut;

		// Token: 0x04000383 RID: 899
		public int AvailableBytesOut;

		// Token: 0x04000384 RID: 900
		public long TotalBytesOut;

		// Token: 0x04000385 RID: 901
		public string Message;

		// Token: 0x04000386 RID: 902
		internal DeflateManager dstate;

		// Token: 0x04000387 RID: 903
		internal InflateManager istate;

		// Token: 0x04000388 RID: 904
		internal uint _Adler32;

		// Token: 0x04000389 RID: 905
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x0400038A RID: 906
		public int WindowBits = 15;

		// Token: 0x0400038B RID: 907
		public CompressionStrategy Strategy;
	}
}

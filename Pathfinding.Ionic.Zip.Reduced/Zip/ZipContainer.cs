using System;
using System.IO;
using System.Text;
using Pathfinding.Ionic.Zlib;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000039 RID: 57
	internal class ZipContainer
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x00010010 File Offset: 0x0000E210
		public ZipContainer(object o)
		{
			this._zf = (o as ZipFile);
			this._zos = (o as ZipOutputStream);
			this._zis = (o as ZipInputStream);
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00010048 File Offset: 0x0000E248
		public ZipFile ZipFile
		{
			get
			{
				return this._zf;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x00010050 File Offset: 0x0000E250
		public ZipOutputStream ZipOutputStream
		{
			get
			{
				return this._zos;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00010058 File Offset: 0x0000E258
		public string Name
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Name;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos.Name;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00010090 File Offset: 0x0000E290
		public string Password
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._Password;
				}
				if (this._zis != null)
				{
					return this._zis._Password;
				}
				return this._zos._password;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002BC RID: 700 RVA: 0x000100CC File Offset: 0x0000E2CC
		public Zip64Option Zip64
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf._zip64;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return this._zos._zip64;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00010104 File Offset: 0x0000E304
		public int BufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.BufferSize;
				}
				if (this._zis != null)
				{
					throw new NotSupportedException();
				}
				return 0;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00010130 File Offset: 0x0000E330
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00010164 File Offset: 0x0000E364
		public ParallelDeflateOutputStream ParallelDeflater
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflater;
				}
				if (this._zis != null)
				{
					return null;
				}
				return this._zos.ParallelDeflater;
			}
			set
			{
				if (this._zf != null)
				{
					this._zf.ParallelDeflater = value;
				}
				else if (this._zos != null)
				{
					this._zos.ParallelDeflater = value;
				}
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0001019C File Offset: 0x0000E39C
		public long ParallelDeflateThreshold
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateThreshold;
				}
				return this._zos.ParallelDeflateThreshold;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x000101CC File Offset: 0x0000E3CC
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ParallelDeflateMaxBufferPairs;
				}
				return this._zos.ParallelDeflateMaxBufferPairs;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000101FC File Offset: 0x0000E3FC
		public int CodecBufferSize
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.CodecBufferSize;
				}
				if (this._zis != null)
				{
					return this._zis.CodecBufferSize;
				}
				return this._zos.CodecBufferSize;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00010244 File Offset: 0x0000E444
		public CompressionStrategy Strategy
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.Strategy;
				}
				return this._zos.Strategy;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00010274 File Offset: 0x0000E474
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.UseZip64WhenSaving;
				}
				return this._zos.EnableZip64;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x000102A4 File Offset: 0x0000E4A4
		public Encoding AlternateEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncoding;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncoding;
				}
				return null;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x000102D8 File Offset: 0x0000E4D8
		public Encoding DefaultEncoding
		{
			get
			{
				if (this._zf != null)
				{
					return ZipFile.DefaultEncoding;
				}
				if (this._zos != null)
				{
					return ZipOutputStream.DefaultEncoding;
				}
				return null;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00010300 File Offset: 0x0000E500
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.AlternateEncodingUsage;
				}
				if (this._zos != null)
				{
					return this._zos.AlternateEncodingUsage;
				}
				return ZipOption.Default;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00010334 File Offset: 0x0000E534
		public Stream ReadStream
		{
			get
			{
				if (this._zf != null)
				{
					return this._zf.ReadStream;
				}
				return this._zis.ReadStream;
			}
		}

		// Token: 0x0400015C RID: 348
		private ZipFile _zf;

		// Token: 0x0400015D RID: 349
		private ZipOutputStream _zos;

		// Token: 0x0400015E RID: 350
		private ZipInputStream _zis;
	}
}

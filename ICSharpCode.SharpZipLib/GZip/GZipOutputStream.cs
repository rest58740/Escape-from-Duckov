using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.GZip
{
	// Token: 0x02000056 RID: 86
	public class GZipOutputStream : DeflaterOutputStream
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x00016B84 File Offset: 0x00014D84
		public GZipOutputStream(Stream baseOutputStream) : this(baseOutputStream, 4096)
		{
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00016B94 File Offset: 0x00014D94
		public GZipOutputStream(Stream baseOutputStream, int size) : base(baseOutputStream, new Deflater(-1, true), size)
		{
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00016BB0 File Offset: 0x00014DB0
		public void SetLevel(int level)
		{
			if (level < 1)
			{
				throw new ArgumentOutOfRangeException("level");
			}
			this.deflater_.SetLevel(level);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00016BD0 File Offset: 0x00014DD0
		public int GetLevel()
		{
			return this.deflater_.GetLevel();
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00016BE0 File Offset: 0x00014DE0
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.state_ == GZipOutputStream.OutputState.Header)
			{
				this.WriteHeader();
			}
			if (this.state_ != GZipOutputStream.OutputState.Footer)
			{
				throw new InvalidOperationException("Write not permitted in current state");
			}
			this.crc.Update(buffer, offset, count);
			base.Write(buffer, offset, count);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00016C2C File Offset: 0x00014E2C
		public override void Close()
		{
			try
			{
				this.Finish();
			}
			finally
			{
				if (this.state_ != GZipOutputStream.OutputState.Closed)
				{
					this.state_ = GZipOutputStream.OutputState.Closed;
					if (base.IsStreamOwner)
					{
						this.baseOutputStream_.Close();
					}
				}
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00016C8C File Offset: 0x00014E8C
		public override void Finish()
		{
			if (this.state_ == GZipOutputStream.OutputState.Header)
			{
				this.WriteHeader();
			}
			if (this.state_ == GZipOutputStream.OutputState.Footer)
			{
				this.state_ = GZipOutputStream.OutputState.Finished;
				base.Finish();
				uint num = (uint)(this.deflater_.TotalIn & (long)((ulong)-1));
				uint num2 = (uint)(this.crc.Value & (long)((ulong)-1));
				byte[] array = new byte[]
				{
					(byte)num2,
					(byte)(num2 >> 8),
					(byte)(num2 >> 16),
					(byte)(num2 >> 24),
					(byte)num,
					(byte)(num >> 8),
					(byte)(num >> 16),
					(byte)(num >> 24)
				};
				this.baseOutputStream_.Write(array, 0, array.Length);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00016D34 File Offset: 0x00014F34
		private void WriteHeader()
		{
			if (this.state_ == GZipOutputStream.OutputState.Header)
			{
				this.state_ = GZipOutputStream.OutputState.Footer;
				long ticks = DateTime.Now.Ticks;
				DateTime dateTime = new DateTime(1970, 1, 1);
				int num = (int)((ticks - dateTime.Ticks) / 10000000L);
				byte[] array = new byte[]
				{
					31,
					139,
					8,
					0,
					0,
					0,
					0,
					0,
					0,
					byte.MaxValue
				};
				array[4] = (byte)num;
				array[5] = (byte)(num >> 8);
				array[6] = (byte)(num >> 16);
				array[7] = (byte)(num >> 24);
				byte[] array2 = array;
				this.baseOutputStream_.Write(array2, 0, array2.Length);
			}
		}

		// Token: 0x040002BD RID: 701
		protected Crc32 crc = new Crc32();

		// Token: 0x040002BE RID: 702
		private GZipOutputStream.OutputState state_;

		// Token: 0x02000057 RID: 87
		private enum OutputState
		{
			// Token: 0x040002C0 RID: 704
			Header,
			// Token: 0x040002C1 RID: 705
			Footer,
			// Token: 0x040002C2 RID: 706
			Finished,
			// Token: 0x040002C3 RID: 707
			Closed
		}
	}
}

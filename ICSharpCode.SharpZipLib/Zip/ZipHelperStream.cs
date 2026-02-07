using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200005D RID: 93
	internal class ZipHelperStream : Stream
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x000171F0 File Offset: 0x000153F0
		public ZipHelperStream(string name)
		{
			this.stream_ = new FileStream(name, FileMode.Open, FileAccess.ReadWrite);
			this.isOwner_ = true;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00017210 File Offset: 0x00015410
		public ZipHelperStream(Stream stream)
		{
			this.stream_ = stream;
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x00017220 File Offset: 0x00015420
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00017228 File Offset: 0x00015428
		public bool IsStreamOwner
		{
			get
			{
				return this.isOwner_;
			}
			set
			{
				this.isOwner_ = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00017234 File Offset: 0x00015434
		public override bool CanRead
		{
			get
			{
				return this.stream_.CanRead;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00017244 File Offset: 0x00015444
		public override bool CanSeek
		{
			get
			{
				return this.stream_.CanSeek;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00017254 File Offset: 0x00015454
		public override long Length
		{
			get
			{
				return this.stream_.Length;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00017264 File Offset: 0x00015464
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00017274 File Offset: 0x00015474
		public override long Position
		{
			get
			{
				return this.stream_.Position;
			}
			set
			{
				this.stream_.Position = value;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00017284 File Offset: 0x00015484
		public override bool CanWrite
		{
			get
			{
				return this.stream_.CanWrite;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00017294 File Offset: 0x00015494
		public override void Flush()
		{
			this.stream_.Flush();
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000172A4 File Offset: 0x000154A4
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream_.Seek(offset, origin);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000172B4 File Offset: 0x000154B4
		public override void SetLength(long value)
		{
			this.stream_.SetLength(value);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000172C4 File Offset: 0x000154C4
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.stream_.Read(buffer, offset, count);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x000172D4 File Offset: 0x000154D4
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.stream_.Write(buffer, offset, count);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000172E4 File Offset: 0x000154E4
		public override void Close()
		{
			Stream stream = this.stream_;
			this.stream_ = null;
			if (this.isOwner_ && stream != null)
			{
				this.isOwner_ = false;
				stream.Close();
			}
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00017320 File Offset: 0x00015520
		private void WriteLocalHeader(ZipEntry entry, EntryPatchData patchData)
		{
			CompressionMethod compressionMethod = entry.CompressionMethod;
			bool flag = true;
			bool flag2 = false;
			this.WriteLEInt(67324752);
			this.WriteLEShort(entry.Version);
			this.WriteLEShort(entry.Flags);
			this.WriteLEShort((int)((byte)compressionMethod));
			this.WriteLEInt((int)entry.DosTime);
			if (flag)
			{
				this.WriteLEInt((int)entry.Crc);
				if (entry.LocalHeaderRequiresZip64)
				{
					this.WriteLEInt(-1);
					this.WriteLEInt(-1);
				}
				else
				{
					this.WriteLEInt((!entry.IsCrypted) ? ((int)entry.CompressedSize) : ((int)entry.CompressedSize + 12));
					this.WriteLEInt((int)entry.Size);
				}
			}
			else
			{
				if (patchData != null)
				{
					patchData.CrcPatchOffset = this.stream_.Position;
				}
				this.WriteLEInt(0);
				if (patchData != null)
				{
					patchData.SizePatchOffset = this.stream_.Position;
				}
				if (entry.LocalHeaderRequiresZip64 && flag2)
				{
					this.WriteLEInt(-1);
					this.WriteLEInt(-1);
				}
				else
				{
					this.WriteLEInt(0);
					this.WriteLEInt(0);
				}
			}
			byte[] array = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long.");
			}
			ZipExtraData zipExtraData = new ZipExtraData(entry.ExtraData);
			if (entry.LocalHeaderRequiresZip64 && (flag || flag2))
			{
				zipExtraData.StartNewEntry();
				if (flag)
				{
					zipExtraData.AddLeLong(entry.Size);
					zipExtraData.AddLeLong(entry.CompressedSize);
				}
				else
				{
					zipExtraData.AddLeLong(-1L);
					zipExtraData.AddLeLong(-1L);
				}
				zipExtraData.AddNewEntry(1);
				if (!zipExtraData.Find(1))
				{
					throw new ZipException("Internal error cant find extra data");
				}
				if (patchData != null)
				{
					patchData.SizePatchOffset = (long)zipExtraData.CurrentReadIndex;
				}
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			this.WriteLEShort(array.Length);
			this.WriteLEShort(entryData.Length);
			if (array.Length > 0)
			{
				this.stream_.Write(array, 0, array.Length);
			}
			if (entry.LocalHeaderRequiresZip64 && flag2)
			{
				patchData.SizePatchOffset += this.stream_.Position;
			}
			if (entryData.Length > 0)
			{
				this.stream_.Write(entryData, 0, entryData.Length);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0001758C File Offset: 0x0001578C
		public long LocateBlockWithSignature(int signature, long endLocation, int minimumBlockSize, int maximumVariableData)
		{
			long num = endLocation - (long)minimumBlockSize;
			if (num < 0L)
			{
				return -1L;
			}
			long num2 = Math.Max(num - (long)maximumVariableData, 0L);
			while (num >= num2)
			{
				long num3 = num;
				num = num3 - 1L;
				this.Seek(num3, SeekOrigin.Begin);
				if (this.ReadLEInt() == signature)
				{
					return this.Position;
				}
			}
			return -1L;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x000175E0 File Offset: 0x000157E0
		public void WriteZip64EndOfCentralDirectory(long noOfEntries, long sizeEntries, long centralDirOffset)
		{
			long position = this.stream_.Position;
			this.WriteLEInt(101075792);
			this.WriteLELong(44L);
			this.WriteLEShort(51);
			this.WriteLEShort(45);
			this.WriteLEInt(0);
			this.WriteLEInt(0);
			this.WriteLELong(noOfEntries);
			this.WriteLELong(noOfEntries);
			this.WriteLELong(sizeEntries);
			this.WriteLELong(centralDirOffset);
			this.WriteLEInt(117853008);
			this.WriteLEInt(0);
			this.WriteLELong(position);
			this.WriteLEInt(1);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017668 File Offset: 0x00015868
		public void WriteEndOfCentralDirectory(long noOfEntries, long sizeEntries, long startOfCentralDirectory, byte[] comment)
		{
			if (noOfEntries >= 65535L || startOfCentralDirectory >= (long)((ulong)-1) || sizeEntries >= (long)((ulong)-1))
			{
				this.WriteZip64EndOfCentralDirectory(noOfEntries, sizeEntries, startOfCentralDirectory);
			}
			this.WriteLEInt(101010256);
			this.WriteLEShort(0);
			this.WriteLEShort(0);
			if (noOfEntries >= 65535L)
			{
				this.WriteLEUshort(ushort.MaxValue);
				this.WriteLEUshort(ushort.MaxValue);
			}
			else
			{
				this.WriteLEShort((int)((short)noOfEntries));
				this.WriteLEShort((int)((short)noOfEntries));
			}
			if (sizeEntries >= (long)((ulong)-1))
			{
				this.WriteLEUint(uint.MaxValue);
			}
			else
			{
				this.WriteLEInt((int)sizeEntries);
			}
			if (startOfCentralDirectory >= (long)((ulong)-1))
			{
				this.WriteLEUint(uint.MaxValue);
			}
			else
			{
				this.WriteLEInt((int)startOfCentralDirectory);
			}
			int num = (comment == null) ? 0 : comment.Length;
			if (num > 65535)
			{
				throw new ZipException(string.Format("Comment length({0}) is too long can only be 64K", num));
			}
			this.WriteLEShort(num);
			if (num > 0)
			{
				this.Write(comment, 0, comment.Length);
			}
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00017770 File Offset: 0x00015970
		public int ReadLEShort()
		{
			int num = this.stream_.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			int num2 = this.stream_.ReadByte();
			if (num2 < 0)
			{
				throw new EndOfStreamException();
			}
			return num | num2 << 8;
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000177B4 File Offset: 0x000159B4
		public int ReadLEInt()
		{
			return this.ReadLEShort() | this.ReadLEShort() << 16;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000177C8 File Offset: 0x000159C8
		public long ReadLELong()
		{
			return (long)((ulong)this.ReadLEInt() | (ulong)((ulong)((long)this.ReadLEInt()) << 32));
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000177DC File Offset: 0x000159DC
		public void WriteLEShort(int value)
		{
			this.stream_.WriteByte((byte)(value & 255));
			this.stream_.WriteByte((byte)(value >> 8 & 255));
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00017814 File Offset: 0x00015A14
		public void WriteLEUshort(ushort value)
		{
			this.stream_.WriteByte((byte)(value & 255));
			this.stream_.WriteByte((byte)(value >> 8));
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00017844 File Offset: 0x00015A44
		public void WriteLEInt(int value)
		{
			this.WriteLEShort(value);
			this.WriteLEShort(value >> 16);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00017858 File Offset: 0x00015A58
		public void WriteLEUint(uint value)
		{
			this.WriteLEUshort((ushort)(value & 65535U));
			this.WriteLEUshort((ushort)(value >> 16));
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00017874 File Offset: 0x00015A74
		public void WriteLELong(long value)
		{
			this.WriteLEInt((int)value);
			this.WriteLEInt((int)(value >> 32));
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0001788C File Offset: 0x00015A8C
		public void WriteLEUlong(ulong value)
		{
			this.WriteLEUint((uint)(value & (ulong)-1));
			this.WriteLEUint((uint)(value >> 32));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x000178A4 File Offset: 0x00015AA4
		public int WriteDataDescriptor(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			int num = 0;
			if ((entry.Flags & 8) != 0)
			{
				this.WriteLEInt(134695760);
				this.WriteLEInt((int)entry.Crc);
				num += 8;
				if (entry.LocalHeaderRequiresZip64)
				{
					this.WriteLELong(entry.CompressedSize);
					this.WriteLELong(entry.Size);
					num += 16;
				}
				else
				{
					this.WriteLEInt((int)entry.CompressedSize);
					this.WriteLEInt((int)entry.Size);
					num += 8;
				}
			}
			return num;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0001793C File Offset: 0x00015B3C
		public void ReadDataDescriptor(bool zip64, DescriptorData data)
		{
			int num = this.ReadLEInt();
			if (num != 134695760)
			{
				throw new ZipException("Data descriptor signature not found");
			}
			data.Crc = (long)this.ReadLEInt();
			if (zip64)
			{
				data.CompressedSize = this.ReadLELong();
				data.Size = this.ReadLELong();
			}
			else
			{
				data.CompressedSize = (long)this.ReadLEInt();
				data.Size = (long)this.ReadLEInt();
			}
		}

		// Token: 0x040002CE RID: 718
		private bool isOwner_;

		// Token: 0x040002CF RID: 719
		private Stream stream_;
	}
}

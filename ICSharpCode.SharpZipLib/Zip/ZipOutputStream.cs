using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000019 RID: 25
	public class ZipOutputStream : DeflaterOutputStream
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00007F24 File Offset: 0x00006124
		public ZipOutputStream(Stream baseOutputStream) : base(baseOutputStream, new Deflater(-1, true))
		{
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00007F88 File Offset: 0x00006188
		public ZipOutputStream(Stream baseOutputStream, int bufferSize) : base(baseOutputStream, new Deflater(-1, true), bufferSize)
		{
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00007FEC File Offset: 0x000061EC
		public bool IsFinished
		{
			get
			{
				return this.entries == null;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00007FF8 File Offset: 0x000061F8
		public void SetComment(string comment)
		{
			byte[] array = ZipConstants.ConvertToArray(comment);
			if (array.Length > 65535)
			{
				throw new ArgumentOutOfRangeException("comment");
			}
			this.zipComment = array;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000802C File Offset: 0x0000622C
		public void SetLevel(int level)
		{
			this.deflater_.SetLevel(level);
			this.defaultCompressionLevel = level;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00008044 File Offset: 0x00006244
		public int GetLevel()
		{
			return this.deflater_.GetLevel();
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00008054 File Offset: 0x00006254
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x0000805C File Offset: 0x0000625C
		public UseZip64 UseZip64
		{
			get
			{
				return this.useZip64_;
			}
			set
			{
				this.useZip64_ = value;
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00008068 File Offset: 0x00006268
		private void WriteLeShort(int value)
		{
			this.baseOutputStream_.WriteByte((byte)(value & 255));
			this.baseOutputStream_.WriteByte((byte)(value >> 8 & 255));
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000080A0 File Offset: 0x000062A0
		private void WriteLeInt(int value)
		{
			this.WriteLeShort(value);
			this.WriteLeShort(value >> 16);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000080B4 File Offset: 0x000062B4
		private void WriteLeLong(long value)
		{
			this.WriteLeInt((int)value);
			this.WriteLeInt((int)(value >> 32));
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000080CC File Offset: 0x000062CC
		public void PutNextEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (this.entries == null)
			{
				throw new InvalidOperationException("ZipOutputStream was finished");
			}
			if (this.curEntry != null)
			{
				this.CloseEntry();
			}
			if (this.entries.Count == 2147483647)
			{
				throw new ZipException("Too many entries for Zip file");
			}
			CompressionMethod compressionMethod = entry.CompressionMethod;
			int level = this.defaultCompressionLevel;
			entry.Flags &= 2048;
			this.patchEntryHeader = false;
			bool flag;
			if (entry.Size == 0L)
			{
				entry.CompressedSize = entry.Size;
				entry.Crc = 0L;
				compressionMethod = CompressionMethod.Stored;
				flag = true;
			}
			else
			{
				flag = (entry.Size >= 0L && entry.HasCrc && entry.CompressedSize >= 0L);
				if (compressionMethod == CompressionMethod.Stored)
				{
					if (!flag)
					{
						if (!base.CanPatchEntries)
						{
							compressionMethod = CompressionMethod.Deflated;
							level = 0;
						}
					}
					else
					{
						entry.CompressedSize = entry.Size;
						flag = entry.HasCrc;
					}
				}
			}
			if (!flag)
			{
				if (!base.CanPatchEntries)
				{
					entry.Flags |= 8;
				}
				else
				{
					this.patchEntryHeader = true;
				}
			}
			if (base.Password != null)
			{
				entry.IsCrypted = true;
				if (entry.Crc < 0L)
				{
					entry.Flags |= 8;
				}
			}
			entry.Offset = this.offset;
			entry.CompressionMethod = compressionMethod;
			this.curMethod = compressionMethod;
			this.sizePatchPos = -1L;
			if (this.useZip64_ == UseZip64.On || (entry.Size < 0L && this.useZip64_ == UseZip64.Dynamic))
			{
				entry.ForceZip64();
			}
			this.WriteLeInt(67324752);
			this.WriteLeShort(entry.Version);
			this.WriteLeShort(entry.Flags);
			this.WriteLeShort((int)((byte)entry.CompressionMethodForHeader));
			this.WriteLeInt((int)entry.DosTime);
			if (flag)
			{
				this.WriteLeInt((int)entry.Crc);
				if (entry.LocalHeaderRequiresZip64)
				{
					this.WriteLeInt(-1);
					this.WriteLeInt(-1);
				}
				else
				{
					this.WriteLeInt((!entry.IsCrypted) ? ((int)entry.CompressedSize) : ((int)entry.CompressedSize + 12));
					this.WriteLeInt((int)entry.Size);
				}
			}
			else
			{
				if (this.patchEntryHeader)
				{
					this.crcPatchPos = this.baseOutputStream_.Position;
				}
				this.WriteLeInt(0);
				if (this.patchEntryHeader)
				{
					this.sizePatchPos = this.baseOutputStream_.Position;
				}
				if (entry.LocalHeaderRequiresZip64 || this.patchEntryHeader)
				{
					this.WriteLeInt(-1);
					this.WriteLeInt(-1);
				}
				else
				{
					this.WriteLeInt(0);
					this.WriteLeInt(0);
				}
			}
			byte[] array = ZipConstants.ConvertToArray(entry.Flags, entry.Name);
			if (array.Length > 65535)
			{
				throw new ZipException("Entry name too long.");
			}
			ZipExtraData zipExtraData = new ZipExtraData(entry.ExtraData);
			if (entry.LocalHeaderRequiresZip64)
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
				if (this.patchEntryHeader)
				{
					this.sizePatchPos = (long)zipExtraData.CurrentReadIndex;
				}
			}
			else
			{
				zipExtraData.Delete(1);
			}
			byte[] entryData = zipExtraData.GetEntryData();
			this.WriteLeShort(array.Length);
			this.WriteLeShort(entryData.Length);
			if (array.Length > 0)
			{
				this.baseOutputStream_.Write(array, 0, array.Length);
			}
			if (entry.LocalHeaderRequiresZip64 && this.patchEntryHeader)
			{
				this.sizePatchPos += this.baseOutputStream_.Position;
			}
			if (entryData.Length > 0)
			{
				this.baseOutputStream_.Write(entryData, 0, entryData.Length);
			}
			this.offset += (long)(30 + array.Length + entryData.Length);
			if (entry.AESKeySize > 0)
			{
				this.offset += (long)entry.AESOverheadSize;
			}
			this.curEntry = entry;
			this.crc.Reset();
			if (compressionMethod == CompressionMethod.Deflated)
			{
				this.deflater_.Reset();
				this.deflater_.SetLevel(level);
			}
			this.size = 0L;
			if (entry.IsCrypted)
			{
				if (entry.Crc < 0L)
				{
					this.WriteEncryptionHeader(entry.DosTime << 16);
				}
				else
				{
					this.WriteEncryptionHeader(entry.Crc);
				}
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008590 File Offset: 0x00006790
		public void CloseEntry()
		{
			if (this.curEntry == null)
			{
				throw new InvalidOperationException("No open entry");
			}
			long totalOut = this.size;
			if (this.curMethod == CompressionMethod.Deflated)
			{
				if (this.size >= 0L)
				{
					base.Finish();
					totalOut = this.deflater_.TotalOut;
				}
				else
				{
					this.deflater_.Reset();
				}
			}
			if (this.curEntry.AESKeySize > 0)
			{
				this.baseOutputStream_.Write(this.AESAuthCode, 0, 10);
			}
			if (this.curEntry.Size < 0L)
			{
				this.curEntry.Size = this.size;
			}
			else if (this.curEntry.Size != this.size)
			{
				throw new ZipException(string.Concat(new object[]
				{
					"size was ",
					this.size,
					", but I expected ",
					this.curEntry.Size
				}));
			}
			if (this.curEntry.CompressedSize < 0L)
			{
				this.curEntry.CompressedSize = totalOut;
			}
			else if (this.curEntry.CompressedSize != totalOut)
			{
				throw new ZipException(string.Concat(new object[]
				{
					"compressed size was ",
					totalOut,
					", but I expected ",
					this.curEntry.CompressedSize
				}));
			}
			if (this.curEntry.Crc < 0L)
			{
				this.curEntry.Crc = this.crc.Value;
			}
			else if (this.curEntry.Crc != this.crc.Value)
			{
				throw new ZipException(string.Concat(new object[]
				{
					"crc was ",
					this.crc.Value,
					", but I expected ",
					this.curEntry.Crc
				}));
			}
			this.offset += totalOut;
			if (this.curEntry.IsCrypted)
			{
				if (this.curEntry.AESKeySize > 0)
				{
					this.curEntry.CompressedSize += (long)this.curEntry.AESOverheadSize;
				}
				else
				{
					this.curEntry.CompressedSize += 12L;
				}
			}
			if (this.patchEntryHeader)
			{
				this.patchEntryHeader = false;
				long position = this.baseOutputStream_.Position;
				this.baseOutputStream_.Seek(this.crcPatchPos, SeekOrigin.Begin);
				this.WriteLeInt((int)this.curEntry.Crc);
				if (this.curEntry.LocalHeaderRequiresZip64)
				{
					if (this.sizePatchPos == -1L)
					{
						throw new ZipException("Entry requires zip64 but this has been turned off");
					}
					this.baseOutputStream_.Seek(this.sizePatchPos, SeekOrigin.Begin);
					this.WriteLeLong(this.curEntry.Size);
					this.WriteLeLong(this.curEntry.CompressedSize);
				}
				else
				{
					this.WriteLeInt((int)this.curEntry.CompressedSize);
					this.WriteLeInt((int)this.curEntry.Size);
				}
				this.baseOutputStream_.Seek(position, SeekOrigin.Begin);
			}
			if ((this.curEntry.Flags & 8) != 0)
			{
				this.WriteLeInt(134695760);
				this.WriteLeInt((int)this.curEntry.Crc);
				if (this.curEntry.LocalHeaderRequiresZip64)
				{
					this.WriteLeLong(this.curEntry.CompressedSize);
					this.WriteLeLong(this.curEntry.Size);
					this.offset += 24L;
				}
				else
				{
					this.WriteLeInt((int)this.curEntry.CompressedSize);
					this.WriteLeInt((int)this.curEntry.Size);
					this.offset += 16L;
				}
			}
			this.entries.Add(this.curEntry);
			this.curEntry = null;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000899C File Offset: 0x00006B9C
		private void WriteEncryptionHeader(long crcValue)
		{
			this.offset += 12L;
			base.InitializePassword(base.Password);
			byte[] array = new byte[12];
			Random random = new Random();
			random.NextBytes(array);
			array[11] = (byte)(crcValue >> 24);
			base.EncryptBlock(array, 0, array.Length);
			this.baseOutputStream_.Write(array, 0, array.Length);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00008A00 File Offset: 0x00006C00
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.curEntry == null)
			{
				throw new InvalidOperationException("No open entry.");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be negative");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Cannot be negative");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Invalid offset/count combination");
			}
			this.crc.Update(buffer, offset, count);
			this.size += (long)count;
			CompressionMethod compressionMethod = this.curMethod;
			if (compressionMethod != CompressionMethod.Stored)
			{
				if (compressionMethod == CompressionMethod.Deflated)
				{
					base.Write(buffer, offset, count);
				}
			}
			else if (base.Password != null)
			{
				this.CopyAndEncrypt(buffer, offset, count);
			}
			else
			{
				this.baseOutputStream_.Write(buffer, offset, count);
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00008AE8 File Offset: 0x00006CE8
		private void CopyAndEncrypt(byte[] buffer, int offset, int count)
		{
			byte[] array = new byte[4096];
			while (count > 0)
			{
				int num = (count >= 4096) ? 4096 : count;
				Array.Copy(buffer, offset, array, 0, num);
				base.EncryptBlock(array, 0, num);
				this.baseOutputStream_.Write(array, 0, num);
				count -= num;
				offset += num;
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00008B50 File Offset: 0x00006D50
		public override void Finish()
		{
			if (this.entries == null)
			{
				return;
			}
			if (this.curEntry != null)
			{
				this.CloseEntry();
			}
			long noOfEntries = (long)this.entries.Count;
			long num = 0L;
			foreach (object obj in this.entries)
			{
				ZipEntry zipEntry = (ZipEntry)obj;
				this.WriteLeInt(33639248);
				this.WriteLeShort(51);
				this.WriteLeShort(zipEntry.Version);
				this.WriteLeShort(zipEntry.Flags);
				this.WriteLeShort((int)((short)zipEntry.CompressionMethodForHeader));
				this.WriteLeInt((int)zipEntry.DosTime);
				this.WriteLeInt((int)zipEntry.Crc);
				if (zipEntry.IsZip64Forced() || zipEntry.CompressedSize >= (long)((ulong)-1))
				{
					this.WriteLeInt(-1);
				}
				else
				{
					this.WriteLeInt((int)zipEntry.CompressedSize);
				}
				if (zipEntry.IsZip64Forced() || zipEntry.Size >= (long)((ulong)-1))
				{
					this.WriteLeInt(-1);
				}
				else
				{
					this.WriteLeInt((int)zipEntry.Size);
				}
				byte[] array = ZipConstants.ConvertToArray(zipEntry.Flags, zipEntry.Name);
				if (array.Length > 65535)
				{
					throw new ZipException("Name too long.");
				}
				ZipExtraData zipExtraData = new ZipExtraData(zipEntry.ExtraData);
				if (zipEntry.CentralHeaderRequiresZip64)
				{
					zipExtraData.StartNewEntry();
					if (zipEntry.IsZip64Forced() || zipEntry.Size >= (long)((ulong)-1))
					{
						zipExtraData.AddLeLong(zipEntry.Size);
					}
					if (zipEntry.IsZip64Forced() || zipEntry.CompressedSize >= (long)((ulong)-1))
					{
						zipExtraData.AddLeLong(zipEntry.CompressedSize);
					}
					if (zipEntry.Offset >= (long)((ulong)-1))
					{
						zipExtraData.AddLeLong(zipEntry.Offset);
					}
					zipExtraData.AddNewEntry(1);
				}
				else
				{
					zipExtraData.Delete(1);
				}
				byte[] entryData = zipExtraData.GetEntryData();
				byte[] array2 = (zipEntry.Comment == null) ? new byte[0] : ZipConstants.ConvertToArray(zipEntry.Flags, zipEntry.Comment);
				if (array2.Length > 65535)
				{
					throw new ZipException("Comment too long.");
				}
				this.WriteLeShort(array.Length);
				this.WriteLeShort(entryData.Length);
				this.WriteLeShort(array2.Length);
				this.WriteLeShort(0);
				this.WriteLeShort(0);
				if (zipEntry.ExternalFileAttributes != -1)
				{
					this.WriteLeInt(zipEntry.ExternalFileAttributes);
				}
				else if (zipEntry.IsDirectory)
				{
					this.WriteLeInt(16);
				}
				else
				{
					this.WriteLeInt(0);
				}
				if (zipEntry.Offset >= (long)((ulong)-1))
				{
					this.WriteLeInt(-1);
				}
				else
				{
					this.WriteLeInt((int)zipEntry.Offset);
				}
				if (array.Length > 0)
				{
					this.baseOutputStream_.Write(array, 0, array.Length);
				}
				if (entryData.Length > 0)
				{
					this.baseOutputStream_.Write(entryData, 0, entryData.Length);
				}
				if (array2.Length > 0)
				{
					this.baseOutputStream_.Write(array2, 0, array2.Length);
				}
				num += (long)(46 + array.Length + entryData.Length + array2.Length);
			}
			using (ZipHelperStream zipHelperStream = new ZipHelperStream(this.baseOutputStream_))
			{
				zipHelperStream.WriteEndOfCentralDirectory(noOfEntries, num, this.offset, this.zipComment);
			}
			this.entries = null;
		}

		// Token: 0x04000120 RID: 288
		private ArrayList entries = new ArrayList();

		// Token: 0x04000121 RID: 289
		private Crc32 crc = new Crc32();

		// Token: 0x04000122 RID: 290
		private ZipEntry curEntry;

		// Token: 0x04000123 RID: 291
		private int defaultCompressionLevel = -1;

		// Token: 0x04000124 RID: 292
		private CompressionMethod curMethod = CompressionMethod.Deflated;

		// Token: 0x04000125 RID: 293
		private long size;

		// Token: 0x04000126 RID: 294
		private long offset;

		// Token: 0x04000127 RID: 295
		private byte[] zipComment = new byte[0];

		// Token: 0x04000128 RID: 296
		private bool patchEntryHeader;

		// Token: 0x04000129 RID: 297
		private long crcPatchPos = -1L;

		// Token: 0x0400012A RID: 298
		private long sizePatchPos = -1L;

		// Token: 0x0400012B RID: 299
		private UseZip64 useZip64_ = UseZip64.Dynamic;
	}
}

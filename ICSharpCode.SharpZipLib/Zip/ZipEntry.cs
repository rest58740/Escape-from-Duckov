using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000035 RID: 53
	public class ZipEntry : ICloneable
	{
		// Token: 0x06000200 RID: 512 RVA: 0x0000DBF4 File Offset: 0x0000BDF4
		public ZipEntry(string name) : this(name, 0, 51, CompressionMethod.Deflated)
		{
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000DC04 File Offset: 0x0000BE04
		internal ZipEntry(string name, int versionRequiredToExtract) : this(name, versionRequiredToExtract, 51, CompressionMethod.Deflated)
		{
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000DC14 File Offset: 0x0000BE14
		internal ZipEntry(string name, int versionRequiredToExtract, int madeByInfo, CompressionMethod method)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			base..ctor();
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length > 65535)
			{
				throw new ArgumentException("Name is too long", "name");
			}
			if (versionRequiredToExtract != 0 && versionRequiredToExtract < 10)
			{
				throw new ArgumentOutOfRangeException("versionRequiredToExtract");
			}
			this.DateTime = DateTime.Now;
			this.name = ZipEntry.CleanName(name);
			this.versionMadeBy = (ushort)madeByInfo;
			this.versionToExtract = (ushort)versionRequiredToExtract;
			this.method = method;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		[Obsolete("Use Clone instead")]
		public ZipEntry(ZipEntry entry)
		{
			this.externalFileAttributes = -1;
			this.method = CompressionMethod.Deflated;
			this.zipFileIndex = -1L;
			base..ctor();
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.known = entry.known;
			this.name = entry.name;
			this.size = entry.size;
			this.compressedSize = entry.compressedSize;
			this.crc = entry.crc;
			this.dosTime = entry.dosTime;
			this.method = entry.method;
			this.comment = entry.comment;
			this.versionToExtract = entry.versionToExtract;
			this.versionMadeBy = entry.versionMadeBy;
			this.externalFileAttributes = entry.externalFileAttributes;
			this.flags = entry.flags;
			this.zipFileIndex = entry.zipFileIndex;
			this.offset = entry.offset;
			this.forceZip64_ = entry.forceZip64_;
			if (entry.extra != null)
			{
				this.extra = new byte[entry.extra.Length];
				Array.Copy(entry.extra, 0, this.extra, 0, entry.extra.Length);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		public bool HasCrc
		{
			get
			{
				return (byte)(this.known & ZipEntry.Known.Crc) != 0;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0000DE04 File Offset: 0x0000C004
		public bool IsCrypted
		{
			get
			{
				return (this.flags & 1) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 1;
				}
				else
				{
					this.flags &= -2;
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000DE3C File Offset: 0x0000C03C
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0000DE50 File Offset: 0x0000C050
		public bool IsUnicodeText
		{
			get
			{
				return (this.flags & 2048) != 0;
			}
			set
			{
				if (value)
				{
					this.flags |= 2048;
				}
				else
				{
					this.flags &= -2049;
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000DE84 File Offset: 0x0000C084
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0000DE8C File Offset: 0x0000C08C
		internal byte CryptoCheckValue
		{
			get
			{
				return this.cryptoCheckValue_;
			}
			set
			{
				this.cryptoCheckValue_ = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000DE98 File Offset: 0x0000C098
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0000DEA0 File Offset: 0x0000C0A0
		public int Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0000DEB4 File Offset: 0x0000C0B4
		public long ZipFileIndex
		{
			get
			{
				return this.zipFileIndex;
			}
			set
			{
				this.zipFileIndex = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
		public long Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				this.offset = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000DED4 File Offset: 0x0000C0D4
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000DEF0 File Offset: 0x0000C0F0
		public int ExternalFileAttributes
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) == 0)
				{
					return -1;
				}
				return this.externalFileAttributes;
			}
			set
			{
				this.externalFileAttributes = value;
				this.known |= ZipEntry.Known.ExternalAttributes;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public int VersionMadeBy
		{
			get
			{
				return (int)(this.versionMadeBy & 255);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000DF1C File Offset: 0x0000C11C
		public bool IsDOSEntry
		{
			get
			{
				return this.HostSystem == 0 || this.HostSystem == 10;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000DF38 File Offset: 0x0000C138
		private bool HasDosAttributes(int attributes)
		{
			bool result = false;
			if ((byte)(this.known & ZipEntry.Known.ExternalAttributes) != 0 && (this.HostSystem == 0 || this.HostSystem == 10) && (this.ExternalFileAttributes & attributes) == attributes)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000DF80 File Offset: 0x0000C180
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000DF90 File Offset: 0x0000C190
		public int HostSystem
		{
			get
			{
				return this.versionMadeBy >> 8 & 255;
			}
			set
			{
				this.versionMadeBy &= 255;
				this.versionMadeBy |= (ushort)((value & 255) << 8);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public int Version
		{
			get
			{
				if (this.versionToExtract != 0)
				{
					return (int)(this.versionToExtract & 255);
				}
				int result = 10;
				if (this.AESKeySize > 0)
				{
					result = 51;
				}
				else if (this.CentralHeaderRequiresZip64)
				{
					result = 45;
				}
				else if (this.method == CompressionMethod.Deflated)
				{
					result = 20;
				}
				else if (this.IsDirectory)
				{
					result = 20;
				}
				else if (this.IsCrypted)
				{
					result = 20;
				}
				else if (this.HasDosAttributes(8))
				{
					result = 11;
				}
				return result;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000E05C File Offset: 0x0000C25C
		public bool CanDecompress
		{
			get
			{
				return this.Version <= 51 && (this.Version == 10 || this.Version == 11 || this.Version == 20 || this.Version == 45 || this.Version == 51) && this.IsCompressionMethodSupported();
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		public void ForceZip64()
		{
			this.forceZip64_ = true;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E0CC File Offset: 0x0000C2CC
		public bool IsZip64Forced()
		{
			return this.forceZip64_;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		public bool LocalHeaderRequiresZip64
		{
			get
			{
				bool flag = this.forceZip64_;
				if (!flag)
				{
					ulong num = this.compressedSize;
					if (this.versionToExtract == 0 && this.IsCrypted)
					{
						num += 12UL;
					}
					flag = ((this.size >= (ulong)-1 || num >= (ulong)-1) && (this.versionToExtract == 0 || this.versionToExtract >= 45));
				}
				return flag;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000E148 File Offset: 0x0000C348
		public bool CentralHeaderRequiresZip64
		{
			get
			{
				return this.LocalHeaderRequiresZip64 || this.offset >= (long)((ulong)-1);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000E168 File Offset: 0x0000C368
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000E184 File Offset: 0x0000C384
		public long DosTime
		{
			get
			{
				if ((byte)(this.known & ZipEntry.Known.Time) == 0)
				{
					return 0L;
				}
				return (long)((ulong)this.dosTime);
			}
			set
			{
				this.dosTime = (uint)value;
				this.known |= ZipEntry.Known.Time;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000E244 File Offset: 0x0000C444
		public DateTime DateTime
		{
			get
			{
				uint second = Math.Min(59U, 2U * (this.dosTime & 31U));
				uint minute = Math.Min(59U, this.dosTime >> 5 & 63U);
				uint hour = Math.Min(23U, this.dosTime >> 11 & 31U);
				uint month = Math.Max(1U, Math.Min(12U, this.dosTime >> 21 & 15U));
				uint year = (this.dosTime >> 25 & 127U) + 1980U;
				int day = Math.Max(1, Math.Min(DateTime.DaysInMonth((int)year, (int)month), (int)(this.dosTime >> 16 & 31U)));
				return new DateTime((int)year, (int)month, day, (int)hour, (int)minute, (int)second);
			}
			set
			{
				uint num = (uint)value.Year;
				uint num2 = (uint)value.Month;
				uint num3 = (uint)value.Day;
				uint num4 = (uint)value.Hour;
				uint num5 = (uint)value.Minute;
				uint num6 = (uint)value.Second;
				if (num < 1980U)
				{
					num = 1980U;
					num2 = 1U;
					num3 = 1U;
					num4 = 0U;
					num5 = 0U;
					num6 = 0U;
				}
				else if (num > 2107U)
				{
					num = 2107U;
					num2 = 12U;
					num3 = 31U;
					num4 = 23U;
					num5 = 59U;
					num6 = 59U;
				}
				this.DosTime = (long)((ulong)((num - 1980U & 127U) << 25 | num2 << 21 | num3 << 16 | num4 << 11 | num5 << 5 | num6 >> 1));
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000E31C File Offset: 0x0000C51C
		public long Size
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.Size) == 0) ? ulong.MaxValue : this.size);
			}
			set
			{
				this.size = (ulong)value;
				this.known |= ZipEntry.Known.Size;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000E334 File Offset: 0x0000C534
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000E354 File Offset: 0x0000C554
		public long CompressedSize
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.CompressedSize) == 0) ? ulong.MaxValue : this.compressedSize);
			}
			set
			{
				this.compressedSize = (ulong)value;
				this.known |= ZipEntry.Known.CompressedSize;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000E36C File Offset: 0x0000C56C
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000E390 File Offset: 0x0000C590
		public long Crc
		{
			get
			{
				return (long)(((byte)(this.known & ZipEntry.Known.Crc) == 0) ? ulong.MaxValue : ((ulong)this.crc & (ulong)-1));
			}
			set
			{
				if (((ulong)this.crc & 18446744069414584320UL) != 0UL)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.crc = (uint)value;
				this.known |= ZipEntry.Known.Crc;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000E3D8 File Offset: 0x0000C5D8
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000E3E0 File Offset: 0x0000C5E0
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this.method;
			}
			set
			{
				if (!ZipEntry.IsCompressionMethodSupported(value))
				{
					throw new NotSupportedException("Compression method not supported");
				}
				this.method = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000E400 File Offset: 0x0000C600
		internal CompressionMethod CompressionMethodForHeader
		{
			get
			{
				return (this.AESKeySize <= 0) ? this.method : CompressionMethod.WinZipAES;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000E41C File Offset: 0x0000C61C
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000E424 File Offset: 0x0000C624
		public byte[] ExtraData
		{
			get
			{
				return this.extra;
			}
			set
			{
				if (value == null)
				{
					this.extra = null;
				}
				else
				{
					if (value.Length > 65535)
					{
						throw new ArgumentOutOfRangeException("value");
					}
					this.extra = new byte[value.Length];
					Array.Copy(value, 0, this.extra, 0, value.Length);
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000E47C File Offset: 0x0000C67C
		internal int AESSaltLen
		{
			get
			{
				return this.AESKeySize / 16;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000E488 File Offset: 0x0000C688
		internal int AESOverheadSize
		{
			get
			{
				return 12 + this.AESSaltLen;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000E494 File Offset: 0x0000C694
		internal void ProcessExtraData(bool localHeader)
		{
			ZipExtraData zipExtraData = new ZipExtraData(this.extra);
			if (zipExtraData.Find(1))
			{
				this.forceZip64_ = true;
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("Extra data extended Zip64 information length is invalid");
				}
				if (localHeader || this.size == (ulong)-1)
				{
					this.size = (ulong)zipExtraData.ReadLong();
				}
				if (localHeader || this.compressedSize == (ulong)-1)
				{
					this.compressedSize = (ulong)zipExtraData.ReadLong();
				}
				if (!localHeader && this.offset == (long)((ulong)-1))
				{
					this.offset = zipExtraData.ReadLong();
				}
			}
			else if ((this.versionToExtract & 255) >= 45 && (this.size == (ulong)-1 || this.compressedSize == (ulong)-1))
			{
				throw new ZipException("Zip64 Extended information required but is missing.");
			}
			if (zipExtraData.Find(10))
			{
				if (zipExtraData.ValueLength < 4)
				{
					throw new ZipException("NTFS Extra data invalid");
				}
				zipExtraData.ReadInt();
				while (zipExtraData.UnreadCount >= 4)
				{
					int num = zipExtraData.ReadShort();
					int num2 = zipExtraData.ReadShort();
					if (num == 1)
					{
						if (num2 >= 24)
						{
							long fileTime = zipExtraData.ReadLong();
							long num3 = zipExtraData.ReadLong();
							long num4 = zipExtraData.ReadLong();
							this.DateTime = DateTime.FromFileTime(fileTime);
						}
						break;
					}
					zipExtraData.Skip(num2);
				}
			}
			else if (zipExtraData.Find(21589))
			{
				int valueLength = zipExtraData.ValueLength;
				int num5 = zipExtraData.ReadByte();
				if ((num5 & 1) != 0 && valueLength >= 5)
				{
					int seconds = zipExtraData.ReadInt();
					DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
					this.DateTime = (dateTime.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
				}
			}
			if (this.method == CompressionMethod.WinZipAES)
			{
				this.ProcessAESExtraData(zipExtraData);
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E684 File Offset: 0x0000C884
		private void ProcessAESExtraData(ZipExtraData extraData)
		{
			throw new ZipException("AES unsupported");
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000E690 File Offset: 0x0000C890
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000E698 File Offset: 0x0000C898
		public string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				if (value != null && value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException("value", "cannot exceed 65535");
				}
				this.comment = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000E6C8 File Offset: 0x0000C8C8
		public bool IsDirectory
		{
			get
			{
				int length = this.name.Length;
				return (length > 0 && (this.name[length - 1] == '/' || this.name[length - 1] == '\\')) || this.HasDosAttributes(16);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000E720 File Offset: 0x0000C920
		public bool IsFile
		{
			get
			{
				return !this.IsDirectory && !this.HasDosAttributes(8);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000E73C File Offset: 0x0000C93C
		public bool IsCompressionMethodSupported()
		{
			return ZipEntry.IsCompressionMethodSupported(this.CompressionMethod);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000E74C File Offset: 0x0000C94C
		public object Clone()
		{
			ZipEntry zipEntry = (ZipEntry)base.MemberwiseClone();
			if (this.extra != null)
			{
				zipEntry.extra = new byte[this.extra.Length];
				Array.Copy(this.extra, 0, zipEntry.extra, 0, this.extra.Length);
			}
			return zipEntry;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		public static bool IsCompressionMethodSupported(CompressionMethod method)
		{
			return method == CompressionMethod.Deflated || method == CompressionMethod.Stored;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000E7B8 File Offset: 0x0000C9B8
		public static string CleanName(string name)
		{
			if (name == null)
			{
				return string.Empty;
			}
			if (Path.IsPathRooted(name))
			{
				name = name.Substring(Path.GetPathRoot(name).Length);
			}
			name = name.Replace("\\", "/");
			while (name.Length > 0 && name[0] == '/')
			{
				name = name.Remove(0, 1);
			}
			return name;
		}

		// Token: 0x040001AC RID: 428
		internal int AESKeySize;

		// Token: 0x040001AD RID: 429
		private ZipEntry.Known known;

		// Token: 0x040001AE RID: 430
		private int externalFileAttributes;

		// Token: 0x040001AF RID: 431
		private ushort versionMadeBy;

		// Token: 0x040001B0 RID: 432
		private string name;

		// Token: 0x040001B1 RID: 433
		private ulong size;

		// Token: 0x040001B2 RID: 434
		private ulong compressedSize;

		// Token: 0x040001B3 RID: 435
		private ushort versionToExtract;

		// Token: 0x040001B4 RID: 436
		private uint crc;

		// Token: 0x040001B5 RID: 437
		private uint dosTime;

		// Token: 0x040001B6 RID: 438
		private CompressionMethod method;

		// Token: 0x040001B7 RID: 439
		private byte[] extra;

		// Token: 0x040001B8 RID: 440
		private string comment;

		// Token: 0x040001B9 RID: 441
		private int flags;

		// Token: 0x040001BA RID: 442
		private long zipFileIndex;

		// Token: 0x040001BB RID: 443
		private long offset;

		// Token: 0x040001BC RID: 444
		private bool forceZip64_;

		// Token: 0x040001BD RID: 445
		private byte cryptoCheckValue_;

		// Token: 0x02000036 RID: 54
		[Flags]
		private enum Known : byte
		{
			// Token: 0x040001BF RID: 447
			None = 0,
			// Token: 0x040001C0 RID: 448
			Size = 1,
			// Token: 0x040001C1 RID: 449
			CompressedSize = 2,
			// Token: 0x040001C2 RID: 450
			Crc = 4,
			// Token: 0x040001C3 RID: 451
			Time = 8,
			// Token: 0x040001C4 RID: 452
			ExternalAttributes = 16
		}
	}
}

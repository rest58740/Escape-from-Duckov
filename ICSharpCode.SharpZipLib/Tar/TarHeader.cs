using System;
using System.Text;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000041 RID: 65
	public class TarHeader : ICloneable
	{
		// Token: 0x060002BB RID: 699 RVA: 0x000128F8 File Offset: 0x00010AF8
		public TarHeader()
		{
			this.Magic = "ustar ";
			this.Version = " ";
			this.Name = string.Empty;
			this.LinkName = string.Empty;
			this.UserId = TarHeader.defaultUserId;
			this.GroupId = TarHeader.defaultGroupId;
			this.UserName = TarHeader.defaultUser;
			this.GroupName = TarHeader.defaultGroupName;
			this.Size = 0L;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00012998 File Offset: 0x00010B98
		// (set) Token: 0x060002BE RID: 702 RVA: 0x000129A0 File Offset: 0x00010BA0
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.name = value;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000129BC File Offset: 0x00010BBC
		[Obsolete("Use the Name property instead", true)]
		public string GetName()
		{
			return this.name;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x000129C4 File Offset: 0x00010BC4
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x000129CC File Offset: 0x00010BCC
		public int Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x000129D8 File Offset: 0x00010BD8
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x000129E0 File Offset: 0x00010BE0
		public int UserId
		{
			get
			{
				return this.userId;
			}
			set
			{
				this.userId = value;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x000129EC File Offset: 0x00010BEC
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x000129F4 File Offset: 0x00010BF4
		public int GroupId
		{
			get
			{
				return this.groupId;
			}
			set
			{
				this.groupId = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00012A00 File Offset: 0x00010C00
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00012A08 File Offset: 0x00010C08
		public long Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Cannot be less than zero");
				}
				this.size = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00012A2C File Offset: 0x00010C2C
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00012A34 File Offset: 0x00010C34
		public DateTime ModTime
		{
			get
			{
				return this.modTime;
			}
			set
			{
				if (value < TarHeader.dateTime1970)
				{
					throw new ArgumentOutOfRangeException("value", "ModTime cannot be before Jan 1st 1970");
				}
				this.modTime = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00012A98 File Offset: 0x00010C98
		public int Checksum
		{
			get
			{
				return this.checksum;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00012AA0 File Offset: 0x00010CA0
		public bool IsChecksumValid
		{
			get
			{
				return this.isChecksumValid;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00012AA8 File Offset: 0x00010CA8
		// (set) Token: 0x060002CD RID: 717 RVA: 0x00012AB0 File Offset: 0x00010CB0
		public byte TypeFlag
		{
			get
			{
				return this.typeFlag;
			}
			set
			{
				this.typeFlag = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00012ABC File Offset: 0x00010CBC
		// (set) Token: 0x060002CF RID: 719 RVA: 0x00012AC4 File Offset: 0x00010CC4
		public string LinkName
		{
			get
			{
				return this.linkName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.linkName = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00012AE0 File Offset: 0x00010CE0
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00012AE8 File Offset: 0x00010CE8
		public string Magic
		{
			get
			{
				return this.magic;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.magic = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00012B04 File Offset: 0x00010D04
		// (set) Token: 0x060002D3 RID: 723 RVA: 0x00012B0C File Offset: 0x00010D0C
		public string Version
		{
			get
			{
				return this.version;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.version = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x00012B28 File Offset: 0x00010D28
		// (set) Token: 0x060002D5 RID: 725 RVA: 0x00012B30 File Offset: 0x00010D30
		public string UserName
		{
			get
			{
				return this.userName;
			}
			set
			{
				if (value != null)
				{
					this.userName = value.Substring(0, Math.Min(32, value.Length));
				}
				else
				{
					string text = Environment.UserName;
					if (text.Length > 32)
					{
						text = text.Substring(0, 32);
					}
					this.userName = text;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00012B88 File Offset: 0x00010D88
		// (set) Token: 0x060002D7 RID: 727 RVA: 0x00012B90 File Offset: 0x00010D90
		public string GroupName
		{
			get
			{
				return this.groupName;
			}
			set
			{
				if (value == null)
				{
					this.groupName = "None";
				}
				else
				{
					this.groupName = value;
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00012BB0 File Offset: 0x00010DB0
		// (set) Token: 0x060002D9 RID: 729 RVA: 0x00012BB8 File Offset: 0x00010DB8
		public int DevMajor
		{
			get
			{
				return this.devMajor;
			}
			set
			{
				this.devMajor = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00012BC4 File Offset: 0x00010DC4
		// (set) Token: 0x060002DB RID: 731 RVA: 0x00012BCC File Offset: 0x00010DCC
		public int DevMinor
		{
			get
			{
				return this.devMinor;
			}
			set
			{
				this.devMinor = value;
			}
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00012BD8 File Offset: 0x00010DD8
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00012BE0 File Offset: 0x00010DE0
		public void ParseBuffer(byte[] header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			int num = 0;
			this.name = TarHeader.ParseName(header, num, 100).ToString();
			num += 100;
			this.mode = (int)TarHeader.ParseOctal(header, num, 8);
			num += 8;
			this.UserId = (int)TarHeader.ParseOctal(header, num, 8);
			num += 8;
			this.GroupId = (int)TarHeader.ParseOctal(header, num, 8);
			num += 8;
			this.Size = TarHeader.ParseBinaryOrOctal(header, num, 12);
			num += 12;
			this.ModTime = TarHeader.GetDateTimeFromCTime(TarHeader.ParseOctal(header, num, 12));
			num += 12;
			this.checksum = (int)TarHeader.ParseOctal(header, num, 8);
			num += 8;
			this.TypeFlag = header[num++];
			this.LinkName = TarHeader.ParseName(header, num, 100).ToString();
			num += 100;
			this.Magic = TarHeader.ParseName(header, num, 6).ToString();
			num += 6;
			this.Version = TarHeader.ParseName(header, num, 2).ToString();
			num += 2;
			this.UserName = TarHeader.ParseName(header, num, 32).ToString();
			num += 32;
			this.GroupName = TarHeader.ParseName(header, num, 32).ToString();
			num += 32;
			this.DevMajor = (int)TarHeader.ParseOctal(header, num, 8);
			num += 8;
			this.DevMinor = (int)TarHeader.ParseOctal(header, num, 8);
			this.isChecksumValid = (this.Checksum == TarHeader.MakeCheckSum(header));
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00012D50 File Offset: 0x00010F50
		public void WriteHeader(byte[] outBuffer)
		{
			if (outBuffer == null)
			{
				throw new ArgumentNullException("outBuffer");
			}
			int i = 0;
			i = TarHeader.GetNameBytes(this.Name, outBuffer, i, 100);
			i = TarHeader.GetOctalBytes((long)this.mode, outBuffer, i, 8);
			i = TarHeader.GetOctalBytes((long)this.UserId, outBuffer, i, 8);
			i = TarHeader.GetOctalBytes((long)this.GroupId, outBuffer, i, 8);
			i = TarHeader.GetBinaryOrOctalBytes(this.Size, outBuffer, i, 12);
			i = TarHeader.GetOctalBytes((long)TarHeader.GetCTime(this.ModTime), outBuffer, i, 12);
			int offset = i;
			for (int j = 0; j < 8; j++)
			{
				outBuffer[i++] = 32;
			}
			outBuffer[i++] = this.TypeFlag;
			i = TarHeader.GetNameBytes(this.LinkName, outBuffer, i, 100);
			i = TarHeader.GetAsciiBytes(this.Magic, 0, outBuffer, i, 6);
			i = TarHeader.GetNameBytes(this.Version, outBuffer, i, 2);
			i = TarHeader.GetNameBytes(this.UserName, outBuffer, i, 32);
			i = TarHeader.GetNameBytes(this.GroupName, outBuffer, i, 32);
			if (this.TypeFlag == 51 || this.TypeFlag == 52)
			{
				i = TarHeader.GetOctalBytes((long)this.DevMajor, outBuffer, i, 8);
				i = TarHeader.GetOctalBytes((long)this.DevMinor, outBuffer, i, 8);
			}
			while (i < outBuffer.Length)
			{
				outBuffer[i++] = 0;
			}
			this.checksum = TarHeader.ComputeCheckSum(outBuffer);
			TarHeader.GetCheckSumOctalBytes((long)this.checksum, outBuffer, offset, 8);
			this.isChecksumValid = true;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00012EC4 File Offset: 0x000110C4
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00012ED4 File Offset: 0x000110D4
		public override bool Equals(object obj)
		{
			TarHeader tarHeader = obj as TarHeader;
			return tarHeader != null && (this.name == tarHeader.name && this.mode == tarHeader.mode && this.UserId == tarHeader.UserId && this.GroupId == tarHeader.GroupId && this.Size == tarHeader.Size && this.ModTime == tarHeader.ModTime && this.Checksum == tarHeader.Checksum && this.TypeFlag == tarHeader.TypeFlag && this.LinkName == tarHeader.LinkName && this.Magic == tarHeader.Magic && this.Version == tarHeader.Version && this.UserName == tarHeader.UserName && this.GroupName == tarHeader.GroupName && this.DevMajor == tarHeader.DevMajor) && this.DevMinor == tarHeader.DevMinor;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0001301C File Offset: 0x0001121C
		internal static void SetValueDefaults(int userId, string userName, int groupId, string groupName)
		{
			TarHeader.userIdAsSet = userId;
			TarHeader.defaultUserId = userId;
			TarHeader.userNameAsSet = userName;
			TarHeader.defaultUser = userName;
			TarHeader.groupIdAsSet = groupId;
			TarHeader.defaultGroupId = groupId;
			TarHeader.groupNameAsSet = groupName;
			TarHeader.defaultGroupName = groupName;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001305C File Offset: 0x0001125C
		internal static void RestoreSetValues()
		{
			TarHeader.defaultUserId = TarHeader.userIdAsSet;
			TarHeader.defaultUser = TarHeader.userNameAsSet;
			TarHeader.defaultGroupId = TarHeader.groupIdAsSet;
			TarHeader.defaultGroupName = TarHeader.groupNameAsSet;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00013094 File Offset: 0x00011294
		private static long ParseBinaryOrOctal(byte[] header, int offset, int length)
		{
			if (header[offset] >= 128)
			{
				long num = 0L;
				for (int i = length - 8; i < length; i++)
				{
					num = (num << 8 | (long)header[offset + i]);
				}
				return num;
			}
			return TarHeader.ParseOctal(header, offset, length);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000130DC File Offset: 0x000112DC
		public static long ParseOctal(byte[] header, int offset, int length)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			long num = 0L;
			bool flag = true;
			int num2 = offset + length;
			int i = offset;
			while (i < num2)
			{
				if (header[i] == 0)
				{
					break;
				}
				if (header[i] != 32 && header[i] != 48)
				{
					goto IL_5C;
				}
				if (!flag)
				{
					if (header[i] == 32)
					{
						break;
					}
					goto IL_5C;
				}
				IL_6A:
				i++;
				continue;
				IL_5C:
				flag = false;
				num = (num << 3) + (long)(header[i] - 48);
				goto IL_6A;
			}
			return num;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00013160 File Offset: 0x00011360
		public static StringBuilder ParseName(byte[] header, int offset, int length)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Cannot be less than zero");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length", "Cannot be less than zero");
			}
			if (offset + length > header.Length)
			{
				throw new ArgumentException("Exceeds header size", "length");
			}
			StringBuilder stringBuilder = new StringBuilder(length);
			for (int i = offset; i < offset + length; i++)
			{
				if (header[i] == 0)
				{
					break;
				}
				stringBuilder.Append((char)header[i]);
			}
			return stringBuilder;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000131FC File Offset: 0x000113FC
		public static int GetNameBytes(StringBuilder name, int nameOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return TarHeader.GetNameBytes(name.ToString(), nameOffset, buffer, bufferOffset, length);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001323C File Offset: 0x0001143C
		public static int GetNameBytes(string name, int nameOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int i = 0;
			while (i < length - 1 && nameOffset + i < name.Length)
			{
				buffer[bufferOffset + i] = (byte)name[nameOffset + i];
				i++;
			}
			while (i < length)
			{
				buffer[bufferOffset + i] = 0;
				i++;
			}
			return bufferOffset + length;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000132B8 File Offset: 0x000114B8
		public static int GetNameBytes(StringBuilder name, byte[] buffer, int offset, int length)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return TarHeader.GetNameBytes(name.ToString(), 0, buffer, offset, length);
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000132EC File Offset: 0x000114EC
		public static int GetNameBytes(string name, byte[] buffer, int offset, int length)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			return TarHeader.GetNameBytes(name, 0, buffer, offset, length);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x00013328 File Offset: 0x00011528
		public static int GetAsciiBytes(string toAdd, int nameOffset, byte[] buffer, int bufferOffset, int length)
		{
			if (toAdd == null)
			{
				throw new ArgumentNullException("toAdd");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = 0;
			while (num < length && nameOffset + num < toAdd.Length)
			{
				buffer[bufferOffset + num] = (byte)toAdd[nameOffset + num];
				num++;
			}
			return bufferOffset + length;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001338C File Offset: 0x0001158C
		public static int GetOctalBytes(long value, byte[] buffer, int offset, int length)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int i = length - 1;
			buffer[offset + i] = 0;
			i--;
			if (value > 0L)
			{
				long num = value;
				while (i >= 0 && num > 0L)
				{
					buffer[offset + i] = 48 + (byte)(num & 7L);
					num >>= 3;
					i--;
				}
			}
			while (i >= 0)
			{
				buffer[offset + i] = 48;
				i--;
			}
			return offset + length;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00013408 File Offset: 0x00011608
		private static int GetBinaryOrOctalBytes(long value, byte[] buffer, int offset, int length)
		{
			if (value > 8589934591L)
			{
				for (int i = length - 1; i > 0; i--)
				{
					buffer[offset + i] = (byte)value;
					value >>= 8;
				}
				buffer[offset] = 128;
				return offset + length;
			}
			return TarHeader.GetOctalBytes(value, buffer, offset, length);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001345C File Offset: 0x0001165C
		private static void GetCheckSumOctalBytes(long value, byte[] buffer, int offset, int length)
		{
			TarHeader.GetOctalBytes(value, buffer, offset, length - 1);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0001346C File Offset: 0x0001166C
		private static int ComputeCheckSum(byte[] buffer)
		{
			int num = 0;
			for (int i = 0; i < buffer.Length; i++)
			{
				num += (int)buffer[i];
			}
			return num;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00013498 File Offset: 0x00011698
		private static int MakeCheckSum(byte[] buffer)
		{
			int num = 0;
			for (int i = 0; i < 148; i++)
			{
				num += (int)buffer[i];
			}
			for (int j = 0; j < 8; j++)
			{
				num += 32;
			}
			for (int k = 156; k < buffer.Length; k++)
			{
				num += (int)buffer[k];
			}
			return num;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000134FC File Offset: 0x000116FC
		private static int GetCTime(DateTime dateTime)
		{
			return (int)((dateTime.Ticks - TarHeader.dateTime1970.Ticks) / 10000000L);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00013528 File Offset: 0x00011728
		private static DateTime GetDateTimeFromCTime(long ticks)
		{
			DateTime result;
			try
			{
				result = new DateTime(TarHeader.dateTime1970.Ticks + ticks * 10000000L);
			}
			catch (ArgumentOutOfRangeException)
			{
				result = TarHeader.dateTime1970;
			}
			return result;
		}

		// Token: 0x04000231 RID: 561
		public const int NAMELEN = 100;

		// Token: 0x04000232 RID: 562
		public const int MODELEN = 8;

		// Token: 0x04000233 RID: 563
		public const int UIDLEN = 8;

		// Token: 0x04000234 RID: 564
		public const int GIDLEN = 8;

		// Token: 0x04000235 RID: 565
		public const int CHKSUMLEN = 8;

		// Token: 0x04000236 RID: 566
		public const int CHKSUMOFS = 148;

		// Token: 0x04000237 RID: 567
		public const int SIZELEN = 12;

		// Token: 0x04000238 RID: 568
		public const int MAGICLEN = 6;

		// Token: 0x04000239 RID: 569
		public const int VERSIONLEN = 2;

		// Token: 0x0400023A RID: 570
		public const int MODTIMELEN = 12;

		// Token: 0x0400023B RID: 571
		public const int UNAMELEN = 32;

		// Token: 0x0400023C RID: 572
		public const int GNAMELEN = 32;

		// Token: 0x0400023D RID: 573
		public const int DEVLEN = 8;

		// Token: 0x0400023E RID: 574
		public const byte LF_OLDNORM = 0;

		// Token: 0x0400023F RID: 575
		public const byte LF_NORMAL = 48;

		// Token: 0x04000240 RID: 576
		public const byte LF_LINK = 49;

		// Token: 0x04000241 RID: 577
		public const byte LF_SYMLINK = 50;

		// Token: 0x04000242 RID: 578
		public const byte LF_CHR = 51;

		// Token: 0x04000243 RID: 579
		public const byte LF_BLK = 52;

		// Token: 0x04000244 RID: 580
		public const byte LF_DIR = 53;

		// Token: 0x04000245 RID: 581
		public const byte LF_FIFO = 54;

		// Token: 0x04000246 RID: 582
		public const byte LF_CONTIG = 55;

		// Token: 0x04000247 RID: 583
		public const byte LF_GHDR = 103;

		// Token: 0x04000248 RID: 584
		public const byte LF_XHDR = 120;

		// Token: 0x04000249 RID: 585
		public const byte LF_ACL = 65;

		// Token: 0x0400024A RID: 586
		public const byte LF_GNU_DUMPDIR = 68;

		// Token: 0x0400024B RID: 587
		public const byte LF_EXTATTR = 69;

		// Token: 0x0400024C RID: 588
		public const byte LF_META = 73;

		// Token: 0x0400024D RID: 589
		public const byte LF_GNU_LONGLINK = 75;

		// Token: 0x0400024E RID: 590
		public const byte LF_GNU_LONGNAME = 76;

		// Token: 0x0400024F RID: 591
		public const byte LF_GNU_MULTIVOL = 77;

		// Token: 0x04000250 RID: 592
		public const byte LF_GNU_NAMES = 78;

		// Token: 0x04000251 RID: 593
		public const byte LF_GNU_SPARSE = 83;

		// Token: 0x04000252 RID: 594
		public const byte LF_GNU_VOLHDR = 86;

		// Token: 0x04000253 RID: 595
		public const string TMAGIC = "ustar ";

		// Token: 0x04000254 RID: 596
		public const string GNU_TMAGIC = "ustar  ";

		// Token: 0x04000255 RID: 597
		private const long timeConversionFactor = 10000000L;

		// Token: 0x04000256 RID: 598
		private static readonly DateTime dateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x04000257 RID: 599
		private string name;

		// Token: 0x04000258 RID: 600
		private int mode;

		// Token: 0x04000259 RID: 601
		private int userId;

		// Token: 0x0400025A RID: 602
		private int groupId;

		// Token: 0x0400025B RID: 603
		private long size;

		// Token: 0x0400025C RID: 604
		private DateTime modTime;

		// Token: 0x0400025D RID: 605
		private int checksum;

		// Token: 0x0400025E RID: 606
		private bool isChecksumValid;

		// Token: 0x0400025F RID: 607
		private byte typeFlag;

		// Token: 0x04000260 RID: 608
		private string linkName;

		// Token: 0x04000261 RID: 609
		private string magic;

		// Token: 0x04000262 RID: 610
		private string version;

		// Token: 0x04000263 RID: 611
		private string userName;

		// Token: 0x04000264 RID: 612
		private string groupName;

		// Token: 0x04000265 RID: 613
		private int devMajor;

		// Token: 0x04000266 RID: 614
		private int devMinor;

		// Token: 0x04000267 RID: 615
		internal static int userIdAsSet;

		// Token: 0x04000268 RID: 616
		internal static int groupIdAsSet;

		// Token: 0x04000269 RID: 617
		internal static string userNameAsSet;

		// Token: 0x0400026A RID: 618
		internal static string groupNameAsSet = "None";

		// Token: 0x0400026B RID: 619
		internal static int defaultUserId;

		// Token: 0x0400026C RID: 620
		internal static int defaultGroupId;

		// Token: 0x0400026D RID: 621
		internal static string defaultGroupName = "None";

		// Token: 0x0400026E RID: 622
		internal static string defaultUser;
	}
}

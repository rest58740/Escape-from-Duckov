using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000060 RID: 96
	public class ExtendedUnixData : ITaggedData
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00017A78 File Offset: 0x00015C78
		public short TagID
		{
			get
			{
				return 21589;
			}
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00017A80 File Offset: 0x00015C80
		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					this._flags = (ExtendedUnixData.Flags)zipHelperStream.ReadByte();
					if ((byte)(this._flags & ExtendedUnixData.Flags.ModificationTime) != 0 && count >= 5)
					{
						int seconds = zipHelperStream.ReadLEInt();
						DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
						this._modificationTime = (dateTime.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds, 0)).ToLocalTime();
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						int seconds2 = zipHelperStream.ReadLEInt();
						DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0);
						this._lastAccessTime = (dateTime2.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds2, 0)).ToLocalTime();
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						int seconds3 = zipHelperStream.ReadLEInt();
						DateTime dateTime3 = new DateTime(1970, 1, 1, 0, 0, 0);
						this._createTime = (dateTime3.ToUniversalTime() + new TimeSpan(0, 0, 0, seconds3, 0)).ToLocalTime();
					}
				}
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00017BF0 File Offset: 0x00015DF0
		public byte[] GetData()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteByte((byte)this._flags);
					if ((byte)(this._flags & ExtendedUnixData.Flags.ModificationTime) != 0)
					{
						DateTime d = this._modificationTime.ToUniversalTime();
						DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0);
						int value = (int)(d - dateTime.ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value);
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.AccessTime) != 0)
					{
						DateTime d2 = this._lastAccessTime.ToUniversalTime();
						DateTime dateTime2 = new DateTime(1970, 1, 1, 0, 0, 0);
						int value2 = (int)(d2 - dateTime2.ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value2);
					}
					if ((byte)(this._flags & ExtendedUnixData.Flags.CreateTime) != 0)
					{
						DateTime d3 = this._createTime.ToUniversalTime();
						DateTime dateTime3 = new DateTime(1970, 1, 1, 0, 0, 0);
						int value3 = (int)(d3 - dateTime3.ToUniversalTime()).TotalSeconds;
						zipHelperStream.WriteLEInt(value3);
					}
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00017D60 File Offset: 0x00015F60
		public static bool IsValidValue(DateTime value)
		{
			return value >= new DateTime(1901, 12, 13, 20, 45, 52) || value <= new DateTime(2038, 1, 19, 3, 14, 7);
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x00017DA8 File Offset: 0x00015FA8
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x00017DB0 File Offset: 0x00015FB0
		public DateTime ModificationTime
		{
			get
			{
				return this._modificationTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.ModificationTime;
				this._modificationTime = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x00017DEC File Offset: 0x00015FEC
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x00017DF4 File Offset: 0x00015FF4
		public DateTime AccessTime
		{
			get
			{
				return this._lastAccessTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.AccessTime;
				this._lastAccessTime = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x00017E30 File Offset: 0x00016030
		// (set) Token: 0x06000433 RID: 1075 RVA: 0x00017E38 File Offset: 0x00016038
		public DateTime CreateTime
		{
			get
			{
				return this._createTime;
			}
			set
			{
				if (!ExtendedUnixData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._flags |= ExtendedUnixData.Flags.CreateTime;
				this._createTime = value;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x00017E74 File Offset: 0x00016074
		// (set) Token: 0x06000435 RID: 1077 RVA: 0x00017E7C File Offset: 0x0001607C
		private ExtendedUnixData.Flags Include
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x040002D2 RID: 722
		private ExtendedUnixData.Flags _flags;

		// Token: 0x040002D3 RID: 723
		private DateTime _modificationTime = new DateTime(1970, 1, 1);

		// Token: 0x040002D4 RID: 724
		private DateTime _lastAccessTime = new DateTime(1970, 1, 1);

		// Token: 0x040002D5 RID: 725
		private DateTime _createTime = new DateTime(1970, 1, 1);

		// Token: 0x02000061 RID: 97
		[Flags]
		public enum Flags : byte
		{
			// Token: 0x040002D7 RID: 727
			ModificationTime = 1,
			// Token: 0x040002D8 RID: 728
			AccessTime = 2,
			// Token: 0x040002D9 RID: 729
			CreateTime = 4
		}
	}
}

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000062 RID: 98
	public class NTTaggedData : ITaggedData
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00017EB8 File Offset: 0x000160B8
		public short TagID
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00017EBC File Offset: 0x000160BC
		public void SetData(byte[] data, int index, int count)
		{
			using (MemoryStream memoryStream = new MemoryStream(data, index, count, false))
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.ReadLEInt();
					while (zipHelperStream.Position < zipHelperStream.Length)
					{
						int num = zipHelperStream.ReadLEShort();
						int num2 = zipHelperStream.ReadLEShort();
						if (num == 1)
						{
							if (num2 >= 24)
							{
								long fileTime = zipHelperStream.ReadLELong();
								this._lastModificationTime = DateTime.FromFileTime(fileTime);
								long fileTime2 = zipHelperStream.ReadLELong();
								this._lastAccessTime = DateTime.FromFileTime(fileTime2);
								long fileTime3 = zipHelperStream.ReadLELong();
								this._createTime = DateTime.FromFileTime(fileTime3);
							}
							break;
						}
						zipHelperStream.Seek((long)num2, SeekOrigin.Current);
					}
				}
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017FBC File Offset: 0x000161BC
		public byte[] GetData()
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZipHelperStream zipHelperStream = new ZipHelperStream(memoryStream))
				{
					zipHelperStream.IsStreamOwner = false;
					zipHelperStream.WriteLEInt(0);
					zipHelperStream.WriteLEShort(1);
					zipHelperStream.WriteLEShort(24);
					zipHelperStream.WriteLELong(this._lastModificationTime.ToFileTime());
					zipHelperStream.WriteLELong(this._lastAccessTime.ToFileTime());
					zipHelperStream.WriteLELong(this._createTime.ToFileTime());
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0001808C File Offset: 0x0001628C
		public static bool IsValidValue(DateTime value)
		{
			bool result = true;
			try
			{
				value.ToFileTimeUtc();
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x000180D0 File Offset: 0x000162D0
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x000180D8 File Offset: 0x000162D8
		public DateTime LastModificationTime
		{
			get
			{
				return this._lastModificationTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lastModificationTime = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x000180F8 File Offset: 0x000162F8
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x00018100 File Offset: 0x00016300
		public DateTime CreateTime
		{
			get
			{
				return this._createTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._createTime = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00018120 File Offset: 0x00016320
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x00018128 File Offset: 0x00016328
		public DateTime LastAccessTime
		{
			get
			{
				return this._lastAccessTime;
			}
			set
			{
				if (!NTTaggedData.IsValidValue(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._lastAccessTime = value;
			}
		}

		// Token: 0x040002DA RID: 730
		private DateTime _lastAccessTime = DateTime.FromFileTime(0L);

		// Token: 0x040002DB RID: 731
		private DateTime _lastModificationTime = DateTime.FromFileTime(0L);

		// Token: 0x040002DC RID: 732
		private DateTime _createTime = DateTime.FromFileTime(0L);
	}
}

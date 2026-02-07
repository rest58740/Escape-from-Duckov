using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000065 RID: 101
	public class ZipEntryFactory : IEntryFactory
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x00018874 File Offset: 0x00016A74
		public ZipEntryFactory()
		{
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001889C File Offset: 0x00016A9C
		public ZipEntryFactory(ZipEntryFactory.TimeSetting timeSetting)
		{
			this.timeSetting_ = timeSetting;
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000188D4 File Offset: 0x00016AD4
		public ZipEntryFactory(DateTime time)
		{
			this.timeSetting_ = ZipEntryFactory.TimeSetting.Fixed;
			this.FixedDateTime = time;
			this.nameTransform_ = new ZipNameTransform();
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00018908 File Offset: 0x00016B08
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00018910 File Offset: 0x00016B10
		public INameTransform NameTransform
		{
			get
			{
				return this.nameTransform_;
			}
			set
			{
				if (value == null)
				{
					this.nameTransform_ = new ZipNameTransform();
				}
				else
				{
					this.nameTransform_ = value;
				}
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x00018930 File Offset: 0x00016B30
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00018938 File Offset: 0x00016B38
		public ZipEntryFactory.TimeSetting Setting
		{
			get
			{
				return this.timeSetting_;
			}
			set
			{
				this.timeSetting_ = value;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00018944 File Offset: 0x00016B44
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001894C File Offset: 0x00016B4C
		public DateTime FixedDateTime
		{
			get
			{
				return this.fixedDateTime_;
			}
			set
			{
				if (value.Year < 1970)
				{
					throw new ArgumentException("Value is too old to be valid", "value");
				}
				this.fixedDateTime_ = value;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00018984 File Offset: 0x00016B84
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0001898C File Offset: 0x00016B8C
		public int GetAttributes
		{
			get
			{
				return this.getAttributes_;
			}
			set
			{
				this.getAttributes_ = value;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00018998 File Offset: 0x00016B98
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x000189A0 File Offset: 0x00016BA0
		public int SetAttributes
		{
			get
			{
				return this.setAttributes_;
			}
			set
			{
				this.setAttributes_ = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000189AC File Offset: 0x00016BAC
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x000189B4 File Offset: 0x00016BB4
		public bool IsUnicodeText
		{
			get
			{
				return this.isUnicodeText_;
			}
			set
			{
				this.isUnicodeText_ = value;
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000189C0 File Offset: 0x00016BC0
		public ZipEntry MakeFileEntry(string fileName)
		{
			return this.MakeFileEntry(fileName, null, true);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000189CC File Offset: 0x00016BCC
		public ZipEntry MakeFileEntry(string fileName, bool useFileSystem)
		{
			return this.MakeFileEntry(fileName, null, useFileSystem);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000189D8 File Offset: 0x00016BD8
		public ZipEntry MakeFileEntry(string fileName, string entryName, bool useFileSystem)
		{
			ZipEntry zipEntry = new ZipEntry(this.nameTransform_.TransformFile((entryName == null || entryName.Length <= 0) ? fileName : entryName));
			zipEntry.IsUnicodeText = this.isUnicodeText_;
			int num = 0;
			bool flag = this.setAttributes_ != 0;
			FileInfo fileInfo = null;
			if (useFileSystem)
			{
				fileInfo = new FileInfo(fileName);
			}
			if (fileInfo != null && fileInfo.Exists)
			{
				switch (this.timeSetting_)
				{
				case ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = fileInfo.LastWriteTime;
					break;
				case ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = fileInfo.LastWriteTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = fileInfo.CreationTime;
					break;
				case ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = fileInfo.CreationTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = fileInfo.LastAccessTime;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = fileInfo.LastAccessTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = this.fixedDateTime_;
					break;
				default:
					throw new ZipException("Unhandled time setting in MakeFileEntry");
				}
				zipEntry.Size = fileInfo.Length;
				flag = true;
				num = (int)(fileInfo.Attributes & (FileAttributes)this.getAttributes_);
			}
			else if (this.timeSetting_ == ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = this.fixedDateTime_;
			}
			if (flag)
			{
				num |= this.setAttributes_;
				zipEntry.ExternalFileAttributes = num;
			}
			return zipEntry;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00018B4C File Offset: 0x00016D4C
		public ZipEntry MakeDirectoryEntry(string directoryName)
		{
			return this.MakeDirectoryEntry(directoryName, true);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00018B58 File Offset: 0x00016D58
		public ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem)
		{
			ZipEntry zipEntry = new ZipEntry(this.nameTransform_.TransformDirectory(directoryName));
			zipEntry.IsUnicodeText = this.isUnicodeText_;
			zipEntry.Size = 0L;
			int num = 0;
			DirectoryInfo directoryInfo = null;
			if (useFileSystem)
			{
				directoryInfo = new DirectoryInfo(directoryName);
			}
			if (directoryInfo != null && directoryInfo.Exists)
			{
				switch (this.timeSetting_)
				{
				case ZipEntryFactory.TimeSetting.LastWriteTime:
					zipEntry.DateTime = directoryInfo.LastWriteTime;
					break;
				case ZipEntryFactory.TimeSetting.LastWriteTimeUtc:
					zipEntry.DateTime = directoryInfo.LastWriteTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.CreateTime:
					zipEntry.DateTime = directoryInfo.CreationTime;
					break;
				case ZipEntryFactory.TimeSetting.CreateTimeUtc:
					zipEntry.DateTime = directoryInfo.CreationTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTime:
					zipEntry.DateTime = directoryInfo.LastAccessTime;
					break;
				case ZipEntryFactory.TimeSetting.LastAccessTimeUtc:
					zipEntry.DateTime = directoryInfo.LastAccessTimeUtc;
					break;
				case ZipEntryFactory.TimeSetting.Fixed:
					zipEntry.DateTime = this.fixedDateTime_;
					break;
				default:
					throw new ZipException("Unhandled time setting in MakeDirectoryEntry");
				}
				num = (int)(directoryInfo.Attributes & (FileAttributes)this.getAttributes_);
			}
			else if (this.timeSetting_ == ZipEntryFactory.TimeSetting.Fixed)
			{
				zipEntry.DateTime = this.fixedDateTime_;
			}
			num |= (this.setAttributes_ | 16);
			zipEntry.ExternalFileAttributes = num;
			return zipEntry;
		}

		// Token: 0x040002E2 RID: 738
		private INameTransform nameTransform_;

		// Token: 0x040002E3 RID: 739
		private DateTime fixedDateTime_ = DateTime.Now;

		// Token: 0x040002E4 RID: 740
		private ZipEntryFactory.TimeSetting timeSetting_;

		// Token: 0x040002E5 RID: 741
		private bool isUnicodeText_;

		// Token: 0x040002E6 RID: 742
		private int getAttributes_ = -1;

		// Token: 0x040002E7 RID: 743
		private int setAttributes_;

		// Token: 0x02000066 RID: 102
		public enum TimeSetting
		{
			// Token: 0x040002E9 RID: 745
			LastWriteTime,
			// Token: 0x040002EA RID: 746
			LastWriteTimeUtc,
			// Token: 0x040002EB RID: 747
			CreateTime,
			// Token: 0x040002EC RID: 748
			CreateTimeUtc,
			// Token: 0x040002ED RID: 749
			LastAccessTime,
			// Token: 0x040002EE RID: 750
			LastAccessTimeUtc,
			// Token: 0x040002EF RID: 751
			Fixed
		}
	}
}

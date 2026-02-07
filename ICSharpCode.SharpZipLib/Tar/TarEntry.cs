using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000049 RID: 73
	public class TarEntry : ICloneable
	{
		// Token: 0x06000374 RID: 884 RVA: 0x00015494 File Offset: 0x00013694
		private TarEntry()
		{
			this.header = new TarHeader();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000154A8 File Offset: 0x000136A8
		public TarEntry(byte[] headerBuffer)
		{
			this.header = new TarHeader();
			this.header.ParseBuffer(headerBuffer);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000154C8 File Offset: 0x000136C8
		public TarEntry(TarHeader header)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			this.header = (TarHeader)header.Clone();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00015500 File Offset: 0x00013700
		public object Clone()
		{
			return new TarEntry
			{
				file = this.file,
				header = (TarHeader)this.header.Clone(),
				Name = this.Name
			};
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00015544 File Offset: 0x00013744
		public static TarEntry CreateTarEntry(string name)
		{
			TarEntry tarEntry = new TarEntry();
			TarEntry.NameTarHeader(tarEntry.header, name);
			return tarEntry;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00015564 File Offset: 0x00013764
		public static TarEntry CreateEntryFromFile(string fileName)
		{
			TarEntry tarEntry = new TarEntry();
			tarEntry.GetFileTarHeader(tarEntry.header, fileName);
			return tarEntry;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00015588 File Offset: 0x00013788
		public override bool Equals(object obj)
		{
			TarEntry tarEntry = obj as TarEntry;
			return tarEntry != null && this.Name.Equals(tarEntry.Name);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000155B8 File Offset: 0x000137B8
		public override int GetHashCode()
		{
			return this.Name.GetHashCode();
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000155C8 File Offset: 0x000137C8
		public bool IsDescendent(TarEntry toTest)
		{
			if (toTest == null)
			{
				throw new ArgumentNullException("toTest");
			}
			return toTest.Name.StartsWith(this.Name);
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600037D RID: 893 RVA: 0x000155F8 File Offset: 0x000137F8
		public TarHeader TarHeader
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00015600 File Offset: 0x00013800
		// (set) Token: 0x0600037F RID: 895 RVA: 0x00015610 File Offset: 0x00013810
		public string Name
		{
			get
			{
				return this.header.Name;
			}
			set
			{
				this.header.Name = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000380 RID: 896 RVA: 0x00015620 File Offset: 0x00013820
		// (set) Token: 0x06000381 RID: 897 RVA: 0x00015630 File Offset: 0x00013830
		public int UserId
		{
			get
			{
				return this.header.UserId;
			}
			set
			{
				this.header.UserId = value;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00015640 File Offset: 0x00013840
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00015650 File Offset: 0x00013850
		public int GroupId
		{
			get
			{
				return this.header.GroupId;
			}
			set
			{
				this.header.GroupId = value;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00015660 File Offset: 0x00013860
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00015670 File Offset: 0x00013870
		public string UserName
		{
			get
			{
				return this.header.UserName;
			}
			set
			{
				this.header.UserName = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00015680 File Offset: 0x00013880
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00015690 File Offset: 0x00013890
		public string GroupName
		{
			get
			{
				return this.header.GroupName;
			}
			set
			{
				this.header.GroupName = value;
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000156A0 File Offset: 0x000138A0
		public void SetIds(int userId, int groupId)
		{
			this.UserId = userId;
			this.GroupId = groupId;
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000156B0 File Offset: 0x000138B0
		public void SetNames(string userName, string groupName)
		{
			this.UserName = userName;
			this.GroupName = groupName;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600038A RID: 906 RVA: 0x000156C0 File Offset: 0x000138C0
		// (set) Token: 0x0600038B RID: 907 RVA: 0x000156D0 File Offset: 0x000138D0
		public DateTime ModTime
		{
			get
			{
				return this.header.ModTime;
			}
			set
			{
				this.header.ModTime = value;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600038C RID: 908 RVA: 0x000156E0 File Offset: 0x000138E0
		public string File
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600038D RID: 909 RVA: 0x000156E8 File Offset: 0x000138E8
		// (set) Token: 0x0600038E RID: 910 RVA: 0x000156F8 File Offset: 0x000138F8
		public long Size
		{
			get
			{
				return this.header.Size;
			}
			set
			{
				this.header.Size = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00015708 File Offset: 0x00013908
		public bool IsDirectory
		{
			get
			{
				if (this.file != null)
				{
					return Directory.Exists(this.file);
				}
				return this.header != null && (this.header.TypeFlag == 53 || this.Name.EndsWith("/"));
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00015764 File Offset: 0x00013964
		public void GetFileTarHeader(TarHeader header, string file)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			this.file = file;
			string text = file;
			if (text.IndexOf(Environment.CurrentDirectory) == 0)
			{
				text = text.Substring(Environment.CurrentDirectory.Length);
			}
			text = text.Replace(Path.DirectorySeparatorChar, '/');
			while (text.StartsWith("/"))
			{
				text = text.Substring(1);
			}
			header.LinkName = string.Empty;
			header.Name = text;
			if (Directory.Exists(file))
			{
				header.Mode = 1003;
				header.TypeFlag = 53;
				if (header.Name.Length == 0 || header.Name[header.Name.Length - 1] != '/')
				{
					header.Name += "/";
				}
				header.Size = 0L;
			}
			else
			{
				header.Mode = 33216;
				header.TypeFlag = 48;
				header.Size = new FileInfo(file.Replace('/', Path.DirectorySeparatorChar)).Length;
			}
			header.ModTime = System.IO.File.GetLastWriteTime(file.Replace('/', Path.DirectorySeparatorChar)).ToUniversalTime();
			header.DevMajor = 0;
			header.DevMinor = 0;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000158C8 File Offset: 0x00013AC8
		public TarEntry[] GetDirectoryEntries()
		{
			if (this.file == null || !Directory.Exists(this.file))
			{
				return new TarEntry[0];
			}
			string[] fileSystemEntries = Directory.GetFileSystemEntries(this.file);
			TarEntry[] array = new TarEntry[fileSystemEntries.Length];
			for (int i = 0; i < fileSystemEntries.Length; i++)
			{
				array[i] = TarEntry.CreateEntryFromFile(fileSystemEntries[i]);
			}
			return array;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001592C File Offset: 0x00013B2C
		public void WriteEntryHeader(byte[] outBuffer)
		{
			this.header.WriteHeader(outBuffer);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0001593C File Offset: 0x00013B3C
		public static void AdjustEntryName(byte[] buffer, string newName)
		{
			TarHeader.GetNameBytes(newName, buffer, 0, 100);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001594C File Offset: 0x00013B4C
		public static void NameTarHeader(TarHeader header, string name)
		{
			if (header == null)
			{
				throw new ArgumentNullException("header");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			bool flag = name.EndsWith("/");
			header.Name = name;
			header.Mode = ((!flag) ? 33216 : 1003);
			header.UserId = 0;
			header.GroupId = 0;
			header.Size = 0L;
			header.ModTime = DateTime.UtcNow;
			header.TypeFlag = ((!flag) ? 48 : 53);
			header.LinkName = string.Empty;
			header.UserName = string.Empty;
			header.GroupName = string.Empty;
			header.DevMajor = 0;
			header.DevMinor = 0;
		}

		// Token: 0x04000297 RID: 663
		private string file;

		// Token: 0x04000298 RID: 664
		private TarHeader header;
	}
}

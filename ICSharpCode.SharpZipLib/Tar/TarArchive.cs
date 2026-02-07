using System;
using System.IO;
using System.Text;

namespace ICSharpCode.SharpZipLib.Tar
{
	// Token: 0x02000048 RID: 72
	public class TarArchive : IDisposable
	{
		// Token: 0x0600034D RID: 845 RVA: 0x00014880 File Offset: 0x00012A80
		protected TarArchive()
		{
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000148A0 File Offset: 0x00012AA0
		protected TarArchive(TarInputStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.tarIn = stream;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000148E4 File Offset: 0x00012AE4
		protected TarArchive(TarOutputStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this.tarOut = stream;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000350 RID: 848 RVA: 0x00014928 File Offset: 0x00012B28
		// (remove) Token: 0x06000351 RID: 849 RVA: 0x00014944 File Offset: 0x00012B44
		public event ProgressMessageHandler ProgressMessageEvent;

		// Token: 0x06000352 RID: 850 RVA: 0x00014960 File Offset: 0x00012B60
		protected virtual void OnProgressMessageEvent(TarEntry entry, string message)
		{
			ProgressMessageHandler progressMessageEvent = this.ProgressMessageEvent;
			if (progressMessageEvent != null)
			{
				progressMessageEvent(this, entry, message);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00014984 File Offset: 0x00012B84
		public static TarArchive CreateInputTarArchive(Stream inputStream)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			TarInputStream tarInputStream = inputStream as TarInputStream;
			TarArchive result;
			if (tarInputStream != null)
			{
				result = new TarArchive(tarInputStream);
			}
			else
			{
				result = TarArchive.CreateInputTarArchive(inputStream, 20);
			}
			return result;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000149C8 File Offset: 0x00012BC8
		public static TarArchive CreateInputTarArchive(Stream inputStream, int blockFactor)
		{
			if (inputStream == null)
			{
				throw new ArgumentNullException("inputStream");
			}
			if (inputStream is TarInputStream)
			{
				throw new ArgumentException("TarInputStream not valid");
			}
			return new TarArchive(new TarInputStream(inputStream, blockFactor));
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00014A00 File Offset: 0x00012C00
		public static TarArchive CreateOutputTarArchive(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			TarOutputStream tarOutputStream = outputStream as TarOutputStream;
			TarArchive result;
			if (tarOutputStream != null)
			{
				result = new TarArchive(tarOutputStream);
			}
			else
			{
				result = TarArchive.CreateOutputTarArchive(outputStream, 20);
			}
			return result;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00014A44 File Offset: 0x00012C44
		public static TarArchive CreateOutputTarArchive(Stream outputStream, int blockFactor)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (outputStream is TarOutputStream)
			{
				throw new ArgumentException("TarOutputStream is not valid");
			}
			return new TarArchive(new TarOutputStream(outputStream, blockFactor));
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00014A7C File Offset: 0x00012C7C
		public void SetKeepOldFiles(bool keepExistingFiles)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			this.keepOldFiles = keepExistingFiles;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00014A9C File Offset: 0x00012C9C
		// (set) Token: 0x06000359 RID: 857 RVA: 0x00014ABC File Offset: 0x00012CBC
		public bool AsciiTranslate
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.asciiTranslate;
			}
			set
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				this.asciiTranslate = value;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00014ADC File Offset: 0x00012CDC
		[Obsolete("Use the AsciiTranslate property")]
		public void SetAsciiTranslation(bool translateAsciiFiles)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			this.asciiTranslate = translateAsciiFiles;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00014AFC File Offset: 0x00012CFC
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00014B1C File Offset: 0x00012D1C
		public string PathPrefix
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.pathPrefix;
			}
			set
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				this.pathPrefix = value;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00014B3C File Offset: 0x00012D3C
		// (set) Token: 0x0600035E RID: 862 RVA: 0x00014B5C File Offset: 0x00012D5C
		public string RootPath
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.rootPath;
			}
			set
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				this.rootPath = value.Replace('\\', '/').TrimEnd(new char[]
				{
					'/'
				});
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00014BA0 File Offset: 0x00012DA0
		public void SetUserInfo(int userId, string userName, int groupId, string groupName)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			this.userId = userId;
			this.userName = userName;
			this.groupId = groupId;
			this.groupName = groupName;
			this.applyUserInfoOverrides = true;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00014BE8 File Offset: 0x00012DE8
		// (set) Token: 0x06000361 RID: 865 RVA: 0x00014C08 File Offset: 0x00012E08
		public bool ApplyUserInfoOverrides
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.applyUserInfoOverrides;
			}
			set
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				this.applyUserInfoOverrides = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000362 RID: 866 RVA: 0x00014C28 File Offset: 0x00012E28
		public int UserId
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.userId;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00014C48 File Offset: 0x00012E48
		public string UserName
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.userName;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000364 RID: 868 RVA: 0x00014C68 File Offset: 0x00012E68
		public int GroupId
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.groupId;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00014C88 File Offset: 0x00012E88
		public string GroupName
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				return this.groupName;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00014CA8 File Offset: 0x00012EA8
		public int RecordSize
		{
			get
			{
				if (this.isDisposed)
				{
					throw new ObjectDisposedException("TarArchive");
				}
				if (this.tarIn != null)
				{
					return this.tarIn.RecordSize;
				}
				if (this.tarOut != null)
				{
					return this.tarOut.RecordSize;
				}
				return 10240;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (set) Token: 0x06000367 RID: 871 RVA: 0x00014D00 File Offset: 0x00012F00
		public bool IsStreamOwner
		{
			set
			{
				if (this.tarIn != null)
				{
					this.tarIn.IsStreamOwner = value;
				}
				else
				{
					this.tarOut.IsStreamOwner = value;
				}
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00014D38 File Offset: 0x00012F38
		[Obsolete("Use Close instead")]
		public void CloseArchive()
		{
			this.Close();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00014D40 File Offset: 0x00012F40
		public void ListContents()
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			for (;;)
			{
				TarEntry nextEntry = this.tarIn.GetNextEntry();
				if (nextEntry == null)
				{
					break;
				}
				this.OnProgressMessageEvent(nextEntry, null);
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00014D88 File Offset: 0x00012F88
		public void ExtractContents(string destinationDirectory)
		{
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			for (;;)
			{
				TarEntry nextEntry = this.tarIn.GetNextEntry();
				if (nextEntry == null)
				{
					break;
				}
				this.ExtractEntry(destinationDirectory, nextEntry);
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00014DD0 File Offset: 0x00012FD0
		private void ExtractEntry(string destDir, TarEntry entry)
		{
			this.OnProgressMessageEvent(entry, null);
			string text = entry.Name;
			if (Path.IsPathRooted(text))
			{
				text = text.Substring(Path.GetPathRoot(text).Length);
			}
			text = text.Replace('/', Path.DirectorySeparatorChar);
			string text2 = Path.Combine(destDir, text);
			if (entry.IsDirectory)
			{
				TarArchive.EnsureDirectoryExists(text2);
			}
			else
			{
				string directoryName = Path.GetDirectoryName(text2);
				TarArchive.EnsureDirectoryExists(directoryName);
				bool flag = true;
				FileInfo fileInfo = new FileInfo(text2);
				if (fileInfo.Exists)
				{
					if (this.keepOldFiles)
					{
						this.OnProgressMessageEvent(entry, "Destination file already exists");
						flag = false;
					}
					else if ((fileInfo.Attributes & FileAttributes.ReadOnly) != (FileAttributes)0)
					{
						this.OnProgressMessageEvent(entry, "Destination file already exists, and is read-only");
						flag = false;
					}
				}
				if (flag)
				{
					bool flag2 = false;
					Stream stream = File.Create(text2);
					if (this.asciiTranslate)
					{
						flag2 = !TarArchive.IsBinary(text2);
					}
					StreamWriter streamWriter = null;
					if (flag2)
					{
						streamWriter = new StreamWriter(stream);
					}
					byte[] array = new byte[32768];
					for (;;)
					{
						int num = this.tarIn.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						if (flag2)
						{
							int num2 = 0;
							for (int i = 0; i < num; i++)
							{
								if (array[i] == 10)
								{
									string @string = Encoding.ASCII.GetString(array, num2, i - num2);
									streamWriter.WriteLine(@string);
									num2 = i + 1;
								}
							}
						}
						else
						{
							stream.Write(array, 0, num);
						}
					}
					if (flag2)
					{
						streamWriter.Close();
					}
					else
					{
						stream.Close();
					}
				}
			}
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00014F7C File Offset: 0x0001317C
		public void WriteEntry(TarEntry sourceEntry, bool recurse)
		{
			if (sourceEntry == null)
			{
				throw new ArgumentNullException("sourceEntry");
			}
			if (this.isDisposed)
			{
				throw new ObjectDisposedException("TarArchive");
			}
			try
			{
				if (recurse)
				{
					TarHeader.SetValueDefaults(sourceEntry.UserId, sourceEntry.UserName, sourceEntry.GroupId, sourceEntry.GroupName);
				}
				this.WriteEntryCore(sourceEntry, recurse);
			}
			finally
			{
				if (recurse)
				{
					TarHeader.RestoreSetValues();
				}
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00015008 File Offset: 0x00013208
		private void WriteEntryCore(TarEntry sourceEntry, bool recurse)
		{
			string text = null;
			string text2 = sourceEntry.File;
			TarEntry tarEntry = (TarEntry)sourceEntry.Clone();
			if (this.applyUserInfoOverrides)
			{
				tarEntry.GroupId = this.groupId;
				tarEntry.GroupName = this.groupName;
				tarEntry.UserId = this.userId;
				tarEntry.UserName = this.userName;
			}
			this.OnProgressMessageEvent(tarEntry, null);
			if (this.asciiTranslate && !tarEntry.IsDirectory && !TarArchive.IsBinary(text2))
			{
				text = Path.GetTempFileName();
				using (StreamReader streamReader = File.OpenText(text2))
				{
					using (Stream stream = File.Create(text))
					{
						for (;;)
						{
							string text3 = streamReader.ReadLine();
							if (text3 == null)
							{
								break;
							}
							byte[] bytes = Encoding.ASCII.GetBytes(text3);
							stream.Write(bytes, 0, bytes.Length);
							stream.WriteByte(10);
						}
						stream.Flush();
					}
				}
				tarEntry.Size = new FileInfo(text).Length;
				text2 = text;
			}
			string text4 = null;
			if (this.rootPath != null && tarEntry.Name.StartsWith(this.rootPath, StringComparison.OrdinalIgnoreCase))
			{
				text4 = tarEntry.Name.Substring(this.rootPath.Length + 1);
			}
			if (this.pathPrefix != null)
			{
				text4 = ((text4 != null) ? (this.pathPrefix + "/" + text4) : (this.pathPrefix + "/" + tarEntry.Name));
			}
			if (text4 != null)
			{
				tarEntry.Name = text4;
			}
			this.tarOut.PutNextEntry(tarEntry);
			if (tarEntry.IsDirectory)
			{
				if (recurse)
				{
					TarEntry[] directoryEntries = tarEntry.GetDirectoryEntries();
					for (int i = 0; i < directoryEntries.Length; i++)
					{
						this.WriteEntryCore(directoryEntries[i], recurse);
					}
				}
			}
			else
			{
				using (Stream stream2 = File.OpenRead(text2))
				{
					byte[] array = new byte[32768];
					for (;;)
					{
						int num = stream2.Read(array, 0, array.Length);
						if (num <= 0)
						{
							break;
						}
						this.tarOut.Write(array, 0, num);
					}
				}
				if (text != null && text.Length > 0)
				{
					File.Delete(text);
				}
				this.tarOut.CloseEntry();
			}
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000152C8 File Offset: 0x000134C8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000152D8 File Offset: 0x000134D8
		protected virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				this.isDisposed = true;
				if (disposing)
				{
					if (this.tarOut != null)
					{
						this.tarOut.Flush();
						this.tarOut.Close();
					}
					if (this.tarIn != null)
					{
						this.tarIn.Close();
					}
				}
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00015334 File Offset: 0x00013534
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00015340 File Offset: 0x00013540
		~TarArchive()
		{
			this.Dispose(false);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001537C File Offset: 0x0001357C
		private static void EnsureDirectoryExists(string directoryName)
		{
			if (!Directory.Exists(directoryName))
			{
				try
				{
					Directory.CreateDirectory(directoryName);
				}
				catch (Exception ex)
				{
					throw new TarException("Exception creating directory '" + directoryName + "', " + ex.Message, ex);
				}
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000153E0 File Offset: 0x000135E0
		private static bool IsBinary(string filename)
		{
			using (FileStream fileStream = File.OpenRead(filename))
			{
				int num = Math.Min(4096, (int)fileStream.Length);
				byte[] array = new byte[num];
				int num2 = fileStream.Read(array, 0, num);
				for (int i = 0; i < num2; i++)
				{
					byte b = array[i];
					if (b < 8 || (b > 13 && b < 32) || b == 255)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0400028A RID: 650
		private bool keepOldFiles;

		// Token: 0x0400028B RID: 651
		private bool asciiTranslate;

		// Token: 0x0400028C RID: 652
		private int userId;

		// Token: 0x0400028D RID: 653
		private string userName = string.Empty;

		// Token: 0x0400028E RID: 654
		private int groupId;

		// Token: 0x0400028F RID: 655
		private string groupName = string.Empty;

		// Token: 0x04000290 RID: 656
		private string rootPath;

		// Token: 0x04000291 RID: 657
		private string pathPrefix;

		// Token: 0x04000292 RID: 658
		private bool applyUserInfoOverrides;

		// Token: 0x04000293 RID: 659
		private TarInputStream tarIn;

		// Token: 0x04000294 RID: 660
		private TarOutputStream tarOut;

		// Token: 0x04000295 RID: 661
		private bool isDisposed;
	}
}

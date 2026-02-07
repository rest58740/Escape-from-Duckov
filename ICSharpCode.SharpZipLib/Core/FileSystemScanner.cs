using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Core
{
	// Token: 0x0200004E RID: 78
	public class FileSystemScanner
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x00015B38 File Offset: 0x00013D38
		public FileSystemScanner(string filter)
		{
			this.fileFilter_ = new PathFilter(filter);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00015B4C File Offset: 0x00013D4C
		public FileSystemScanner(string fileFilter, string directoryFilter)
		{
			this.fileFilter_ = new PathFilter(fileFilter);
			this.directoryFilter_ = new PathFilter(directoryFilter);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00015B6C File Offset: 0x00013D6C
		public FileSystemScanner(IScanFilter fileFilter)
		{
			this.fileFilter_ = fileFilter;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00015B7C File Offset: 0x00013D7C
		public FileSystemScanner(IScanFilter fileFilter, IScanFilter directoryFilter)
		{
			this.fileFilter_ = fileFilter;
			this.directoryFilter_ = directoryFilter;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00015B94 File Offset: 0x00013D94
		private bool OnDirectoryFailure(string directory, Exception e)
		{
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			bool flag = directoryFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00015BD4 File Offset: 0x00013DD4
		private bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				this.FileFailure(this, scanFailureEventArgs);
				this.alive_ = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00015C18 File Offset: 0x00013E18
		private void OnProcessFile(string file)
		{
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				processFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00015C50 File Offset: 0x00013E50
		private void OnCompleteFile(string file)
		{
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				this.alive_ = scanEventArgs.ContinueRunning;
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00015C88 File Offset: 0x00013E88
		private void OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				this.alive_ = directoryEventArgs.ContinueRunning;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00015CC0 File Offset: 0x00013EC0
		public void Scan(string directory, bool recurse)
		{
			this.alive_ = true;
			this.ScanDir(directory, recurse);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00015CD4 File Offset: 0x00013ED4
		private void ScanDir(string directory, bool recurse)
		{
			try
			{
				string[] files = Directory.GetFiles(directory);
				bool flag = false;
				for (int i = 0; i < files.Length; i++)
				{
					if (!this.fileFilter_.IsMatch(files[i]))
					{
						files[i] = null;
					}
					else
					{
						flag = true;
					}
				}
				this.OnProcessDirectory(directory, flag);
				if (this.alive_ && flag)
				{
					foreach (string text in files)
					{
						try
						{
							if (text != null)
							{
								this.OnProcessFile(text);
								if (!this.alive_)
								{
									break;
								}
							}
						}
						catch (Exception e)
						{
							if (!this.OnFileFailure(text, e))
							{
								throw;
							}
						}
					}
				}
			}
			catch (Exception e2)
			{
				if (!this.OnDirectoryFailure(directory, e2))
				{
					throw;
				}
			}
			if (this.alive_ && recurse)
			{
				try
				{
					string[] directories = Directory.GetDirectories(directory);
					foreach (string text2 in directories)
					{
						if (this.directoryFilter_ == null || this.directoryFilter_.IsMatch(text2))
						{
							this.ScanDir(text2, true);
							if (!this.alive_)
							{
								break;
							}
						}
					}
				}
				catch (Exception e3)
				{
					if (!this.OnDirectoryFailure(directory, e3))
					{
						throw;
					}
				}
			}
		}

		// Token: 0x040002A3 RID: 675
		public ProcessDirectoryHandler ProcessDirectory;

		// Token: 0x040002A4 RID: 676
		public ProcessFileHandler ProcessFile;

		// Token: 0x040002A5 RID: 677
		public CompletedFileHandler CompletedFile;

		// Token: 0x040002A6 RID: 678
		public DirectoryFailureHandler DirectoryFailure;

		// Token: 0x040002A7 RID: 679
		public FileFailureHandler FileFailure;

		// Token: 0x040002A8 RID: 680
		private IScanFilter fileFilter_;

		// Token: 0x040002A9 RID: 681
		private IScanFilter directoryFilter_;

		// Token: 0x040002AA RID: 682
		private bool alive_;
	}
}

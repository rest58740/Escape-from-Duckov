using System;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200001A RID: 26
	public class FastZipEvents
	{
		// Token: 0x06000104 RID: 260 RVA: 0x00008F14 File Offset: 0x00007114
		public bool OnDirectoryFailure(string directory, Exception e)
		{
			bool result = false;
			DirectoryFailureHandler directoryFailure = this.DirectoryFailure;
			if (directoryFailure != null)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(directory, e);
				directoryFailure(this, scanFailureEventArgs);
				result = scanFailureEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00008F48 File Offset: 0x00007148
		public bool OnFileFailure(string file, Exception e)
		{
			FileFailureHandler fileFailure = this.FileFailure;
			bool flag = fileFailure != null;
			if (flag)
			{
				ScanFailureEventArgs scanFailureEventArgs = new ScanFailureEventArgs(file, e);
				fileFailure(this, scanFailureEventArgs);
				flag = scanFailureEventArgs.ContinueRunning;
			}
			return flag;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00008F84 File Offset: 0x00007184
		public bool OnProcessFile(string file)
		{
			bool result = true;
			ProcessFileHandler processFile = this.ProcessFile;
			if (processFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				processFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00008FB8 File Offset: 0x000071B8
		public bool OnCompletedFile(string file)
		{
			bool result = true;
			CompletedFileHandler completedFile = this.CompletedFile;
			if (completedFile != null)
			{
				ScanEventArgs scanEventArgs = new ScanEventArgs(file);
				completedFile(this, scanEventArgs);
				result = scanEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00008FEC File Offset: 0x000071EC
		public bool OnProcessDirectory(string directory, bool hasMatchingFiles)
		{
			bool result = true;
			ProcessDirectoryHandler processDirectory = this.ProcessDirectory;
			if (processDirectory != null)
			{
				DirectoryEventArgs directoryEventArgs = new DirectoryEventArgs(directory, hasMatchingFiles);
				processDirectory(this, directoryEventArgs);
				result = directoryEventArgs.ContinueRunning;
			}
			return result;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00009020 File Offset: 0x00007220
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00009028 File Offset: 0x00007228
		public TimeSpan ProgressInterval
		{
			get
			{
				return this.progressInterval_;
			}
			set
			{
				this.progressInterval_ = value;
			}
		}

		// Token: 0x0400012C RID: 300
		public ProcessDirectoryHandler ProcessDirectory;

		// Token: 0x0400012D RID: 301
		public ProcessFileHandler ProcessFile;

		// Token: 0x0400012E RID: 302
		public ProgressHandler Progress;

		// Token: 0x0400012F RID: 303
		public CompletedFileHandler CompletedFile;

		// Token: 0x04000130 RID: 304
		public DirectoryFailureHandler DirectoryFailure;

		// Token: 0x04000131 RID: 305
		public FileFailureHandler FileFailure;

		// Token: 0x04000132 RID: 306
		private TimeSpan progressInterval_ = TimeSpan.FromSeconds(3.0);
	}
}

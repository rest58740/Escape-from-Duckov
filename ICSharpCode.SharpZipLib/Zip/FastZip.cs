using System;
using System.Collections;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200001B RID: 27
	public class FastZip
	{
		// Token: 0x0600010B RID: 267 RVA: 0x00009034 File Offset: 0x00007234
		public FastZip()
		{
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00009050 File Offset: 0x00007250
		public FastZip(FastZipEvents events)
		{
			this.events_ = events;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600010D RID: 269 RVA: 0x00009074 File Offset: 0x00007274
		// (set) Token: 0x0600010E RID: 270 RVA: 0x0000907C File Offset: 0x0000727C
		public bool CreateEmptyDirectories
		{
			get
			{
				return this.createEmptyDirectories_;
			}
			set
			{
				this.createEmptyDirectories_ = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00009088 File Offset: 0x00007288
		// (set) Token: 0x06000110 RID: 272 RVA: 0x00009090 File Offset: 0x00007290
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000909C File Offset: 0x0000729C
		// (set) Token: 0x06000112 RID: 274 RVA: 0x000090AC File Offset: 0x000072AC
		public INameTransform NameTransform
		{
			get
			{
				return this.entryFactory_.NameTransform;
			}
			set
			{
				this.entryFactory_.NameTransform = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000113 RID: 275 RVA: 0x000090BC File Offset: 0x000072BC
		// (set) Token: 0x06000114 RID: 276 RVA: 0x000090C4 File Offset: 0x000072C4
		public IEntryFactory EntryFactory
		{
			get
			{
				return this.entryFactory_;
			}
			set
			{
				if (value == null)
				{
					this.entryFactory_ = new ZipEntryFactory();
				}
				else
				{
					this.entryFactory_ = value;
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000090E4 File Offset: 0x000072E4
		// (set) Token: 0x06000116 RID: 278 RVA: 0x000090EC File Offset: 0x000072EC
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000090F8 File Offset: 0x000072F8
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00009100 File Offset: 0x00007300
		public bool RestoreDateTimeOnExtract
		{
			get
			{
				return this.restoreDateTimeOnExtract_;
			}
			set
			{
				this.restoreDateTimeOnExtract_ = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000910C File Offset: 0x0000730C
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00009114 File Offset: 0x00007314
		public bool RestoreAttributesOnExtract
		{
			get
			{
				return this.restoreAttributesOnExtract_;
			}
			set
			{
				this.restoreAttributesOnExtract_ = value;
			}
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00009120 File Offset: 0x00007320
		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, directoryFilter);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00009134 File Offset: 0x00007334
		public void CreateZip(string zipFileName, string sourceDirectory, bool recurse, string fileFilter)
		{
			this.CreateZip(File.Create(zipFileName), sourceDirectory, recurse, fileFilter, null);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00009148 File Offset: 0x00007348
		public void CreateZip(Stream outputStream, string sourceDirectory, bool recurse, string fileFilter, string directoryFilter)
		{
			this.NameTransform = new ZipNameTransform(sourceDirectory);
			this.sourceDirectory_ = sourceDirectory;
			using (this.outputStream_ = new ZipOutputStream(outputStream))
			{
				if (this.password_ != null)
				{
					this.outputStream_.Password = this.password_;
				}
				this.outputStream_.UseZip64 = this.UseZip64;
				FileSystemScanner fileSystemScanner = new FileSystemScanner(fileFilter, directoryFilter);
				FileSystemScanner fileSystemScanner2 = fileSystemScanner;
				fileSystemScanner2.ProcessFile = (ProcessFileHandler)Delegate.Combine(fileSystemScanner2.ProcessFile, new ProcessFileHandler(this.ProcessFile));
				if (this.CreateEmptyDirectories)
				{
					FileSystemScanner fileSystemScanner3 = fileSystemScanner;
					fileSystemScanner3.ProcessDirectory = (ProcessDirectoryHandler)Delegate.Combine(fileSystemScanner3.ProcessDirectory, new ProcessDirectoryHandler(this.ProcessDirectory));
				}
				if (this.events_ != null)
				{
					if (this.events_.FileFailure != null)
					{
						FileSystemScanner fileSystemScanner4 = fileSystemScanner;
						fileSystemScanner4.FileFailure = (FileFailureHandler)Delegate.Combine(fileSystemScanner4.FileFailure, this.events_.FileFailure);
					}
					if (this.events_.DirectoryFailure != null)
					{
						FileSystemScanner fileSystemScanner5 = fileSystemScanner;
						fileSystemScanner5.DirectoryFailure = (DirectoryFailureHandler)Delegate.Combine(fileSystemScanner5.DirectoryFailure, this.events_.DirectoryFailure);
					}
				}
				fileSystemScanner.Scan(sourceDirectory, recurse);
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000092A4 File Offset: 0x000074A4
		public void ExtractZip(string zipFileName, string targetDirectory, string fileFilter)
		{
			this.ExtractZip(zipFileName, targetDirectory, FastZip.Overwrite.Always, null, fileFilter, null, this.restoreDateTimeOnExtract_);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000092C4 File Offset: 0x000074C4
		public void ExtractZip(string zipFileName, string targetDirectory, FastZip.Overwrite overwrite, FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime)
		{
			Stream inputStream = File.Open(zipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this.ExtractZip(inputStream, targetDirectory, overwrite, confirmDelegate, fileFilter, directoryFilter, restoreDateTime, true);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000092F0 File Offset: 0x000074F0
		public void ExtractZip(Stream inputStream, string targetDirectory, FastZip.Overwrite overwrite, FastZip.ConfirmOverwriteDelegate confirmDelegate, string fileFilter, string directoryFilter, bool restoreDateTime, bool isStreamOwner)
		{
			if (overwrite == FastZip.Overwrite.Prompt && confirmDelegate == null)
			{
				throw new ArgumentNullException("confirmDelegate");
			}
			this.continueRunning_ = true;
			this.overwrite_ = overwrite;
			this.confirmDelegate_ = confirmDelegate;
			this.extractNameTransform_ = new WindowsNameTransform(targetDirectory);
			this.fileFilter_ = new NameFilter(fileFilter);
			this.directoryFilter_ = new NameFilter(directoryFilter);
			this.restoreDateTimeOnExtract_ = restoreDateTime;
			using (this.zipFile_ = new ZipFile(inputStream))
			{
				if (this.password_ != null)
				{
					this.zipFile_.Password = this.password_;
				}
				this.zipFile_.IsStreamOwner = isStreamOwner;
				IEnumerator enumerator = this.zipFile_.GetEnumerator();
				while (this.continueRunning_ && enumerator.MoveNext())
				{
					ZipEntry zipEntry = (ZipEntry)enumerator.Current;
					if (zipEntry.IsFile)
					{
						if (this.directoryFilter_.IsMatch(Path.GetDirectoryName(zipEntry.Name)) && this.fileFilter_.IsMatch(zipEntry.Name))
						{
							this.ExtractEntry(zipEntry);
						}
					}
					else if (zipEntry.IsDirectory)
					{
						if (this.directoryFilter_.IsMatch(zipEntry.Name) && this.CreateEmptyDirectories)
						{
							this.ExtractEntry(zipEntry);
						}
					}
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009474 File Offset: 0x00007674
		private void ProcessDirectory(object sender, DirectoryEventArgs e)
		{
			if (!e.HasMatchingFiles && this.CreateEmptyDirectories)
			{
				if (this.events_ != null)
				{
					this.events_.OnProcessDirectory(e.Name, e.HasMatchingFiles);
				}
				if (e.ContinueRunning && e.Name != this.sourceDirectory_)
				{
					ZipEntry entry = this.entryFactory_.MakeDirectoryEntry(e.Name);
					this.outputStream_.PutNextEntry(entry);
				}
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000094FC File Offset: 0x000076FC
		private void ProcessFile(object sender, ScanEventArgs e)
		{
			if (this.events_ != null && this.events_.ProcessFile != null)
			{
				this.events_.ProcessFile(sender, e);
			}
			if (e.ContinueRunning)
			{
				try
				{
					using (FileStream fileStream = File.Open(e.Name, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						ZipEntry entry = this.entryFactory_.MakeFileEntry(e.Name);
						this.outputStream_.PutNextEntry(entry);
						this.AddFileContents(e.Name, fileStream);
					}
				}
				catch (Exception e2)
				{
					if (this.events_ == null)
					{
						this.continueRunning_ = false;
						throw;
					}
					this.continueRunning_ = this.events_.OnFileFailure(e.Name, e2);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00009600 File Offset: 0x00007800
		private void AddFileContents(string name, Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.buffer_ == null)
			{
				this.buffer_ = new byte[4096];
			}
			if (this.events_ != null && this.events_.Progress != null)
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, name);
			}
			else
			{
				StreamUtils.Copy(stream, this.outputStream_, this.buffer_);
			}
			if (this.events_ != null)
			{
				this.continueRunning_ = this.events_.OnCompletedFile(name);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000096B4 File Offset: 0x000078B4
		private void ExtractFileEntry(ZipEntry entry, string targetName)
		{
			bool flag = true;
			if (this.overwrite_ != FastZip.Overwrite.Always && File.Exists(targetName))
			{
				flag = (this.overwrite_ == FastZip.Overwrite.Prompt && this.confirmDelegate_ != null && this.confirmDelegate_(targetName));
			}
			if (flag)
			{
				if (this.events_ != null)
				{
					this.continueRunning_ = this.events_.OnProcessFile(entry.Name);
				}
				if (this.continueRunning_)
				{
					try
					{
						using (FileStream fileStream = File.Create(targetName))
						{
							if (this.buffer_ == null)
							{
								this.buffer_ = new byte[4096];
							}
							if (this.events_ != null && this.events_.Progress != null)
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_, this.events_.Progress, this.events_.ProgressInterval, this, entry.Name, entry.Size);
							}
							else
							{
								StreamUtils.Copy(this.zipFile_.GetInputStream(entry), fileStream, this.buffer_);
							}
							if (this.events_ != null)
							{
								this.continueRunning_ = this.events_.OnCompletedFile(entry.Name);
							}
						}
						if (this.restoreDateTimeOnExtract_)
						{
							File.SetLastWriteTime(targetName, entry.DateTime);
						}
						if (this.RestoreAttributesOnExtract && entry.IsDOSEntry && entry.ExternalFileAttributes != -1)
						{
							FileAttributes fileAttributes = (FileAttributes)entry.ExternalFileAttributes;
							fileAttributes &= (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.Archive | FileAttributes.Normal);
							File.SetAttributes(targetName, fileAttributes);
						}
					}
					catch (Exception e)
					{
						if (this.events_ == null)
						{
							this.continueRunning_ = false;
							throw;
						}
						this.continueRunning_ = this.events_.OnFileFailure(targetName, e);
					}
				}
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000098BC File Offset: 0x00007ABC
		private void ExtractEntry(ZipEntry entry)
		{
			bool flag = entry.IsCompressionMethodSupported();
			string text = entry.Name;
			if (flag)
			{
				if (entry.IsFile)
				{
					text = this.extractNameTransform_.TransformFile(text);
				}
				else if (entry.IsDirectory)
				{
					text = this.extractNameTransform_.TransformDirectory(text);
				}
				flag = (text != null && text.Length != 0);
			}
			string path = null;
			if (flag)
			{
				if (entry.IsDirectory)
				{
					path = text;
				}
				else
				{
					path = Path.GetDirectoryName(Path.GetFullPath(text));
				}
			}
			if (flag && !Directory.Exists(path))
			{
				if (entry.IsDirectory)
				{
					if (!this.CreateEmptyDirectories)
					{
						goto IL_10F;
					}
				}
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception e)
				{
					flag = false;
					if (this.events_ == null)
					{
						this.continueRunning_ = false;
						throw;
					}
					if (entry.IsDirectory)
					{
						this.continueRunning_ = this.events_.OnDirectoryFailure(text, e);
					}
					else
					{
						this.continueRunning_ = this.events_.OnFileFailure(text, e);
					}
				}
			}
			IL_10F:
			if (flag && entry.IsFile)
			{
				this.ExtractFileEntry(entry, text);
			}
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00009A10 File Offset: 0x00007C10
		private static int MakeExternalAttributes(FileInfo info)
		{
			return (int)info.Attributes;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00009A18 File Offset: 0x00007C18
		private static bool NameIsValid(string name)
		{
			return name != null && name.Length > 0 && name.IndexOfAny(Path.InvalidPathChars) < 0;
		}

		// Token: 0x04000133 RID: 307
		private bool continueRunning_;

		// Token: 0x04000134 RID: 308
		private byte[] buffer_;

		// Token: 0x04000135 RID: 309
		private ZipOutputStream outputStream_;

		// Token: 0x04000136 RID: 310
		private ZipFile zipFile_;

		// Token: 0x04000137 RID: 311
		private string sourceDirectory_;

		// Token: 0x04000138 RID: 312
		private NameFilter fileFilter_;

		// Token: 0x04000139 RID: 313
		private NameFilter directoryFilter_;

		// Token: 0x0400013A RID: 314
		private FastZip.Overwrite overwrite_;

		// Token: 0x0400013B RID: 315
		private FastZip.ConfirmOverwriteDelegate confirmDelegate_;

		// Token: 0x0400013C RID: 316
		private bool restoreDateTimeOnExtract_;

		// Token: 0x0400013D RID: 317
		private bool restoreAttributesOnExtract_;

		// Token: 0x0400013E RID: 318
		private bool createEmptyDirectories_;

		// Token: 0x0400013F RID: 319
		private FastZipEvents events_;

		// Token: 0x04000140 RID: 320
		private IEntryFactory entryFactory_ = new ZipEntryFactory();

		// Token: 0x04000141 RID: 321
		private INameTransform extractNameTransform_;

		// Token: 0x04000142 RID: 322
		private UseZip64 useZip64_ = UseZip64.Dynamic;

		// Token: 0x04000143 RID: 323
		private string password_;

		// Token: 0x0200001C RID: 28
		public enum Overwrite
		{
			// Token: 0x04000145 RID: 325
			Prompt,
			// Token: 0x04000146 RID: 326
			Never,
			// Token: 0x04000147 RID: 327
			Always
		}

		// Token: 0x02000071 RID: 113
		// (Invoke) Token: 0x060004AC RID: 1196
		public delegate bool ConfirmOverwriteDelegate(string fileName);
	}
}

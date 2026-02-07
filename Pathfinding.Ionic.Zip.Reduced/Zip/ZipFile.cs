using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Pathfinding.Ionic.Zlib;

namespace Pathfinding.Ionic.Zip
{
	// Token: 0x02000031 RID: 49
	[Guid("ebc25cf6-9120-4283-b972-0e5520d00005")]
	[ClassInterface(1)]
	[ComVisible(true)]
	public class ZipFile : IEnumerable<ZipEntry>, IDisposable, IEnumerable
	{
		// Token: 0x06000182 RID: 386 RVA: 0x0000AAF8 File Offset: 0x00008CF8
		public ZipFile(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("Could not read {0} as a zip file", fileName), innerException);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public ZipFile(string fileName, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000AC50 File Offset: 0x00008E50
		public ZipFile()
		{
			this._InitInstance(null, null);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000ACBC File Offset: 0x00008EBC
		public ZipFile(Encoding encoding)
		{
			this.AlternateEncoding = encoding;
			this.AlternateEncodingUsage = ZipOption.Always;
			this._InitInstance(null, null);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000AD34 File Offset: 0x00008F34
		public ZipFile(string fileName, TextWriter statusMessageWriter)
		{
			try
			{
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000ADD8 File Offset: 0x00008FD8
		public ZipFile(string fileName, TextWriter statusMessageWriter, Encoding encoding)
		{
			try
			{
				this.AlternateEncoding = encoding;
				this.AlternateEncodingUsage = ZipOption.Always;
				this._InitInstance(fileName, statusMessageWriter);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000189 RID: 393 RVA: 0x0000AEA4 File Offset: 0x000090A4
		// (remove) Token: 0x0600018A RID: 394 RVA: 0x0000AEE0 File Offset: 0x000090E0
		public event EventHandler<SaveProgressEventArgs> SaveProgress
		{
			add
			{
				EventHandler<SaveProgressEventArgs> eventHandler = this.SaveProgress;
				EventHandler<SaveProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<SaveProgressEventArgs>>(ref this.SaveProgress, (EventHandler<SaveProgressEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<SaveProgressEventArgs> eventHandler = this.SaveProgress;
				EventHandler<SaveProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<SaveProgressEventArgs>>(ref this.SaveProgress, (EventHandler<SaveProgressEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600018B RID: 395 RVA: 0x0000AF1C File Offset: 0x0000911C
		// (remove) Token: 0x0600018C RID: 396 RVA: 0x0000AF58 File Offset: 0x00009158
		public event EventHandler<ReadProgressEventArgs> ReadProgress
		{
			add
			{
				EventHandler<ReadProgressEventArgs> eventHandler = this.ReadProgress;
				EventHandler<ReadProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ReadProgressEventArgs>>(ref this.ReadProgress, (EventHandler<ReadProgressEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ReadProgressEventArgs> eventHandler = this.ReadProgress;
				EventHandler<ReadProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ReadProgressEventArgs>>(ref this.ReadProgress, (EventHandler<ReadProgressEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600018D RID: 397 RVA: 0x0000AF94 File Offset: 0x00009194
		// (remove) Token: 0x0600018E RID: 398 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public event EventHandler<ExtractProgressEventArgs> ExtractProgress
		{
			add
			{
				EventHandler<ExtractProgressEventArgs> eventHandler = this.ExtractProgress;
				EventHandler<ExtractProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ExtractProgressEventArgs>>(ref this.ExtractProgress, (EventHandler<ExtractProgressEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ExtractProgressEventArgs> eventHandler = this.ExtractProgress;
				EventHandler<ExtractProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ExtractProgressEventArgs>>(ref this.ExtractProgress, (EventHandler<ExtractProgressEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600018F RID: 399 RVA: 0x0000B00C File Offset: 0x0000920C
		// (remove) Token: 0x06000190 RID: 400 RVA: 0x0000B048 File Offset: 0x00009248
		public event EventHandler<AddProgressEventArgs> AddProgress
		{
			add
			{
				EventHandler<AddProgressEventArgs> eventHandler = this.AddProgress;
				EventHandler<AddProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<AddProgressEventArgs>>(ref this.AddProgress, (EventHandler<AddProgressEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<AddProgressEventArgs> eventHandler = this.AddProgress;
				EventHandler<AddProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<AddProgressEventArgs>>(ref this.AddProgress, (EventHandler<AddProgressEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000191 RID: 401 RVA: 0x0000B084 File Offset: 0x00009284
		// (remove) Token: 0x06000192 RID: 402 RVA: 0x0000B0C0 File Offset: 0x000092C0
		public event EventHandler<ZipErrorEventArgs> ZipError
		{
			add
			{
				EventHandler<ZipErrorEventArgs> eventHandler = this.ZipError;
				EventHandler<ZipErrorEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ZipErrorEventArgs>>(ref this.ZipError, (EventHandler<ZipErrorEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ZipErrorEventArgs> eventHandler = this.ZipError;
				EventHandler<ZipErrorEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ZipErrorEventArgs>>(ref this.ZipError, (EventHandler<ZipErrorEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000B0FC File Offset: 0x000092FC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000B104 File Offset: 0x00009304
		public ZipEntry AddItem(string fileOrDirectoryName)
		{
			return this.AddItem(fileOrDirectoryName, null);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0000B110 File Offset: 0x00009310
		public ZipEntry AddItem(string fileOrDirectoryName, string directoryPathInArchive)
		{
			if (File.Exists(fileOrDirectoryName))
			{
				return this.AddFile(fileOrDirectoryName, directoryPathInArchive);
			}
			if (Directory.Exists(fileOrDirectoryName))
			{
				return this.AddDirectory(fileOrDirectoryName, directoryPathInArchive);
			}
			throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", fileOrDirectoryName));
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0000B158 File Offset: 0x00009358
		public ZipEntry AddFile(string fileName)
		{
			return this.AddFile(fileName, null);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000B164 File Offset: 0x00009364
		public ZipEntry AddFile(string fileName, string directoryPathInArchive)
		{
			string nameInArchive = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			ZipEntry ze = ZipEntry.CreateFromFile(fileName, nameInArchive);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", fileName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000B1A4 File Offset: 0x000093A4
		public void RemoveEntries(ICollection<ZipEntry> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (ZipEntry entry in entriesToRemove)
			{
				this.RemoveEntry(entry);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000B218 File Offset: 0x00009418
		public void RemoveEntries(ICollection<string> entriesToRemove)
		{
			if (entriesToRemove == null)
			{
				throw new ArgumentNullException("entriesToRemove");
			}
			foreach (string fileName in entriesToRemove)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000B28C File Offset: 0x0000948C
		public void AddFiles(IEnumerable<string> fileNames)
		{
			this.AddFiles(fileNames, null);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000B298 File Offset: 0x00009498
		public void UpdateFiles(IEnumerable<string> fileNames)
		{
			this.UpdateFiles(fileNames, null);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000B2A4 File Offset: 0x000094A4
		public void AddFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			this.AddFiles(fileNames, false, directoryPathInArchive);
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000B2B0 File Offset: 0x000094B0
		public void AddFiles(IEnumerable<string> fileNames, bool preserveDirHierarchy, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this._addOperationCanceled = false;
			this.OnAddStarted();
			if (preserveDirHierarchy)
			{
				foreach (string text in fileNames)
				{
					if (this._addOperationCanceled)
					{
						break;
					}
					if (directoryPathInArchive != null)
					{
						string fullPath = Path.GetFullPath(Path.Combine(directoryPathInArchive, Path.GetDirectoryName(text)));
						this.AddFile(text, fullPath);
					}
					else
					{
						this.AddFile(text, null);
					}
				}
			}
			else
			{
				foreach (string fileName in fileNames)
				{
					if (this._addOperationCanceled)
					{
						break;
					}
					this.AddFile(fileName, directoryPathInArchive);
				}
			}
			if (!this._addOperationCanceled)
			{
				this.OnAddCompleted();
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000B3E8 File Offset: 0x000095E8
		public void UpdateFiles(IEnumerable<string> fileNames, string directoryPathInArchive)
		{
			if (fileNames == null)
			{
				throw new ArgumentNullException("fileNames");
			}
			this.OnAddStarted();
			foreach (string fileName in fileNames)
			{
				this.UpdateFile(fileName, directoryPathInArchive);
			}
			this.OnAddCompleted();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000B468 File Offset: 0x00009668
		public ZipEntry UpdateFile(string fileName)
		{
			return this.UpdateFile(fileName, null);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000B474 File Offset: 0x00009674
		public ZipEntry UpdateFile(string fileName, string directoryPathInArchive)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, directoryPathInArchive);
			if (this[fileName2] != null)
			{
				this.RemoveEntry(fileName2);
			}
			return this.AddFile(fileName, directoryPathInArchive);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000B4A4 File Offset: 0x000096A4
		public ZipEntry UpdateDirectory(string directoryName)
		{
			return this.UpdateDirectory(directoryName, null);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000B4B0 File Offset: 0x000096B0
		public ZipEntry UpdateDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOrUpdate);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000B4BC File Offset: 0x000096BC
		public void UpdateItem(string itemName)
		{
			this.UpdateItem(itemName, null);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public void UpdateItem(string itemName, string directoryPathInArchive)
		{
			if (File.Exists(itemName))
			{
				this.UpdateFile(itemName, directoryPathInArchive);
			}
			else
			{
				if (!Directory.Exists(itemName))
				{
					throw new FileNotFoundException(string.Format("That file or directory ({0}) does not exist!", itemName));
				}
				this.UpdateDirectory(itemName, directoryPathInArchive);
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B518 File Offset: 0x00009718
		public ZipEntry AddEntry(string entryName, string content)
		{
			return this.AddEntry(entryName, content, Encoding.UTF8);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B528 File Offset: 0x00009728
		public ZipEntry AddEntry(string entryName, string content, Encoding encoding)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream, encoding);
			streamWriter.Write(content);
			streamWriter.Flush();
			memoryStream.Seek(0L, 0);
			return this.AddEntry(entryName, memoryStream);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B564 File Offset: 0x00009764
		public ZipEntry AddEntry(string entryName, Stream stream)
		{
			ZipEntry zipEntry = ZipEntry.CreateForStream(entryName, stream);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B5B4 File Offset: 0x000097B4
		public ZipEntry AddEntry(string entryName, WriteDelegate writer)
		{
			ZipEntry ze = ZipEntry.CreateForWriter(entryName, writer);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(ze);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000B5EC File Offset: 0x000097EC
		public ZipEntry AddEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			ZipEntry zipEntry = ZipEntry.CreateForJitStreamProvider(entryName, opener, closer);
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding {0}...", entryName);
			}
			return this._InternalAddEntry(zipEntry);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000B63C File Offset: 0x0000983C
		private ZipEntry _InternalAddEntry(ZipEntry ze)
		{
			ze._container = new ZipContainer(this);
			ze.CompressionMethod = this.CompressionMethod;
			ze.CompressionLevel = this.CompressionLevel;
			ze.ExtractExistingFile = this.ExtractExistingFile;
			ze.ZipErrorAction = this.ZipErrorAction;
			ze.SetCompression = this.SetCompression;
			ze.AlternateEncoding = this.AlternateEncoding;
			ze.AlternateEncodingUsage = this.AlternateEncodingUsage;
			ze.Password = this._Password;
			ze.Encryption = this.Encryption;
			ze.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			ze.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			this.InternalAddEntry(ze.FileName, ze);
			this.AfterAddEntry(ze);
			return ze;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000B6F0 File Offset: 0x000098F0
		public ZipEntry UpdateEntry(string entryName, string content)
		{
			return this.UpdateEntry(entryName, content, Encoding.UTF8);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000B700 File Offset: 0x00009900
		public ZipEntry UpdateEntry(string entryName, string content, Encoding encoding)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, content, encoding);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000B714 File Offset: 0x00009914
		public ZipEntry UpdateEntry(string entryName, WriteDelegate writer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, writer);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000B728 File Offset: 0x00009928
		public ZipEntry UpdateEntry(string entryName, OpenDelegate opener, CloseDelegate closer)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, opener, closer);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B73C File Offset: 0x0000993C
		public ZipEntry UpdateEntry(string entryName, Stream stream)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B750 File Offset: 0x00009950
		private void RemoveEntryForUpdate(string entryName)
		{
			if (string.IsNullOrEmpty(entryName))
			{
				throw new ArgumentNullException("entryName");
			}
			string directoryPathInArchive = null;
			if (entryName.IndexOf('\\') != -1)
			{
				directoryPathInArchive = Path.GetDirectoryName(entryName);
				entryName = Path.GetFileName(entryName);
			}
			string fileName = ZipEntry.NameInArchive(entryName, directoryPathInArchive);
			if (this[fileName] != null)
			{
				this.RemoveEntry(fileName);
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000B7B0 File Offset: 0x000099B0
		public ZipEntry AddEntry(string entryName, byte[] byteContent)
		{
			if (byteContent == null)
			{
				throw new ArgumentException("bad argument", "byteContent");
			}
			MemoryStream stream = new MemoryStream(byteContent);
			return this.AddEntry(entryName, stream);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B7E4 File Offset: 0x000099E4
		public ZipEntry UpdateEntry(string entryName, byte[] byteContent)
		{
			this.RemoveEntryForUpdate(entryName);
			return this.AddEntry(entryName, byteContent);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B7F8 File Offset: 0x000099F8
		public ZipEntry AddDirectory(string directoryName)
		{
			return this.AddDirectory(directoryName, null);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B804 File Offset: 0x00009A04
		public ZipEntry AddDirectory(string directoryName, string directoryPathInArchive)
		{
			return this.AddOrUpdateDirectoryImpl(directoryName, directoryPathInArchive, AddOrUpdateAction.AddOnly);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000B810 File Offset: 0x00009A10
		public ZipEntry AddDirectoryByName(string directoryNameInArchive)
		{
			ZipEntry zipEntry = ZipEntry.CreateFromNothing(directoryNameInArchive);
			zipEntry._container = new ZipContainer(this);
			zipEntry.MarkAsDirectory();
			zipEntry.AlternateEncoding = this.AlternateEncoding;
			zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
			zipEntry.SetEntryTimes(DateTime.Now, DateTime.Now, DateTime.Now);
			zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
			zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
			zipEntry._Source = ZipEntrySource.Stream;
			this.InternalAddEntry(zipEntry.FileName, zipEntry);
			this.AfterAddEntry(zipEntry);
			return zipEntry;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000B898 File Offset: 0x00009A98
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action)
		{
			if (rootDirectoryPathInArchive == null)
			{
				rootDirectoryPathInArchive = string.Empty;
			}
			return this.AddOrUpdateDirectoryImpl(directoryName, rootDirectoryPathInArchive, action, true, 0);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B8B4 File Offset: 0x00009AB4
		internal void InternalAddEntry(string name, ZipEntry entry)
		{
			this._entries.Add(name, entry);
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		private ZipEntry AddOrUpdateDirectoryImpl(string directoryName, string rootDirectoryPathInArchive, AddOrUpdateAction action, bool recurse, int level)
		{
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("{0} {1}...", (action != AddOrUpdateAction.AddOnly) ? "Adding or updating" : "adding", directoryName);
			}
			if (level == 0)
			{
				this._addOperationCanceled = false;
				this.OnAddStarted();
			}
			if (this._addOperationCanceled)
			{
				return null;
			}
			string text = rootDirectoryPathInArchive;
			ZipEntry zipEntry = null;
			if (level > 0)
			{
				int num = directoryName.Length;
				for (int i = level; i > 0; i--)
				{
					num = directoryName.LastIndexOfAny("/\\".ToCharArray(), num - 1, num - 1);
				}
				text = directoryName.Substring(num + 1);
				text = Path.Combine(rootDirectoryPathInArchive, text);
			}
			if (level > 0 || rootDirectoryPathInArchive != string.Empty)
			{
				zipEntry = ZipEntry.CreateFromFile(directoryName, text);
				zipEntry._container = new ZipContainer(this);
				zipEntry.AlternateEncoding = this.AlternateEncoding;
				zipEntry.AlternateEncodingUsage = this.AlternateEncodingUsage;
				zipEntry.MarkAsDirectory();
				zipEntry.EmitTimesInWindowsFormatWhenSaving = this._emitNtfsTimes;
				zipEntry.EmitTimesInUnixFormatWhenSaving = this._emitUnixTimes;
				if (!this._entries.ContainsKey(zipEntry.FileName))
				{
					this.InternalAddEntry(zipEntry.FileName, zipEntry);
					this.AfterAddEntry(zipEntry);
				}
				text = zipEntry.FileName;
			}
			if (!this._addOperationCanceled)
			{
				string[] files = Directory.GetFiles(directoryName);
				if (recurse)
				{
					foreach (string fileName in files)
					{
						if (this._addOperationCanceled)
						{
							break;
						}
						if (action == AddOrUpdateAction.AddOnly)
						{
							this.AddFile(fileName, text);
						}
						else
						{
							this.UpdateFile(fileName, text);
						}
					}
					if (!this._addOperationCanceled)
					{
						string[] directories = Directory.GetDirectories(directoryName);
						foreach (string directoryName2 in directories)
						{
							if (this.AddDirectoryWillTraverseReparsePoints)
							{
								this.AddOrUpdateDirectoryImpl(directoryName2, rootDirectoryPathInArchive, action, recurse, level + 1);
							}
						}
					}
				}
			}
			if (level == 0)
			{
				this.OnAddCompleted();
			}
			return zipEntry;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000BAE4 File Offset: 0x00009CE4
		public static bool CheckZip(string zipFileName)
		{
			return ZipFile.CheckZip(zipFileName, false, null);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000BAF0 File Offset: 0x00009CF0
		public static bool CheckZip(string zipFileName, bool fixIfNecessary, TextWriter writer)
		{
			ZipFile zipFile = null;
			ZipFile zipFile2 = null;
			bool flag = true;
			try
			{
				zipFile = new ZipFile();
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile2 = ZipFile.Read(zipFileName);
				foreach (ZipEntry zipEntry in zipFile)
				{
					foreach (ZipEntry zipEntry2 in zipFile2)
					{
						if (zipEntry.FileName == zipEntry2.FileName)
						{
							if (zipEntry._RelativeOffsetOfLocalHeader != zipEntry2._RelativeOffsetOfLocalHeader)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in RelativeOffsetOfLocalHeader  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._RelativeOffsetOfLocalHeader, zipEntry2._RelativeOffsetOfLocalHeader);
								}
							}
							if (zipEntry._CompressedSize != zipEntry2._CompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._CompressedSize, zipEntry2._CompressedSize);
								}
							}
							if (zipEntry._UncompressedSize != zipEntry2._UncompressedSize)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in UncompressedSize  (0x{1:X16} != 0x{2:X16})", zipEntry.FileName, zipEntry._UncompressedSize, zipEntry2._UncompressedSize);
								}
							}
							if (zipEntry.CompressionMethod != zipEntry2.CompressionMethod)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in CompressionMethod  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.CompressionMethod, zipEntry2.CompressionMethod);
								}
							}
							if (zipEntry.Crc != zipEntry2.Crc)
							{
								flag = false;
								if (writer != null)
								{
									writer.WriteLine("{0}: mismatch in Crc32  (0x{1:X4} != 0x{2:X4})", zipEntry.FileName, zipEntry.Crc, zipEntry2.Crc);
								}
							}
							break;
						}
					}
				}
				zipFile2.Dispose();
				zipFile2 = null;
				if (!flag && fixIfNecessary)
				{
					string text = Path.GetFileNameWithoutExtension(zipFileName);
					text = string.Format("{0}_fixed.zip", text);
					zipFile.Save(text);
				}
			}
			finally
			{
				if (zipFile != null)
				{
					zipFile.Dispose();
				}
				if (zipFile2 != null)
				{
					zipFile2.Dispose();
				}
			}
			return flag;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000BD98 File Offset: 0x00009F98
		public static void FixZipDirectory(string zipFileName)
		{
			using (ZipFile zipFile = new ZipFile())
			{
				zipFile.FullScan = true;
				zipFile.Initialize(zipFileName);
				zipFile.Save(zipFileName);
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000BDF0 File Offset: 0x00009FF0
		public static bool CheckZipPassword(string zipFileName, string password)
		{
			bool result = false;
			try
			{
				using (ZipFile zipFile = ZipFile.Read(zipFileName))
				{
					foreach (ZipEntry zipEntry in zipFile)
					{
						if (!zipEntry.IsDirectory && zipEntry.UsesEncryption)
						{
							zipEntry.ExtractWithPassword(Stream.Null, password);
						}
					}
				}
				result = true;
			}
			catch (BadPasswordException)
			{
			}
			return result;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0000BEC4 File Offset: 0x0000A0C4
		public string Info
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(string.Format("          ZipFile: {0}\n", this.Name));
				if (!string.IsNullOrEmpty(this._Comment))
				{
					stringBuilder.Append(string.Format("          Comment: {0}\n", this._Comment));
				}
				if (this._versionMadeBy != 0)
				{
					stringBuilder.Append(string.Format("  version made by: 0x{0:X4}\n", this._versionMadeBy));
				}
				if (this._versionNeededToExtract != 0)
				{
					stringBuilder.Append(string.Format("needed to extract: 0x{0:X4}\n", this._versionNeededToExtract));
				}
				stringBuilder.Append(string.Format("       uses ZIP64: {0}\n", this.InputUsesZip64));
				stringBuilder.Append(string.Format("     disk with CD: {0}\n", this._diskNumberWithCd));
				if (this._OffsetOfCentralDirectory == 4294967295U)
				{
					stringBuilder.Append(string.Format("      CD64 offset: 0x{0:X16}\n", this._OffsetOfCentralDirectory64));
				}
				else
				{
					stringBuilder.Append(string.Format("        CD offset: 0x{0:X8}\n", this._OffsetOfCentralDirectory));
				}
				stringBuilder.Append("\n");
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					stringBuilder.Append(zipEntry.Info);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000C05C File Offset: 0x0000A25C
		private string ArchiveNameForEvent
		{
			get
			{
				return (this._name == null) ? "(stream)" : this._name;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000C07C File Offset: 0x0000A27C
		internal bool OnSaveBlock(ZipEntry entry, long bytesXferred, long totalBytesToXfer)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesXferred, totalBytesToXfer);
				saveProgress.Invoke(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
		private void OnSaveEntry(int current, ZipEntry entry, bool before)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, entry);
				saveProgress.Invoke(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000C118 File Offset: 0x0000A318
		private void OnSaveEvent(ZipProgressEventType eventFlavor)
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = new SaveProgressEventArgs(this.ArchiveNameForEvent, eventFlavor);
				saveProgress.Invoke(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000C15C File Offset: 0x0000A35C
		private void OnSaveStarted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Started(this.ArchiveNameForEvent);
				saveProgress.Invoke(this, saveProgressEventArgs);
				if (saveProgressEventArgs.Cancel)
				{
					this._saveOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000C19C File Offset: 0x0000A39C
		private void OnSaveCompleted()
		{
			EventHandler<SaveProgressEventArgs> saveProgress = this.SaveProgress;
			if (saveProgress != null)
			{
				SaveProgressEventArgs saveProgressEventArgs = SaveProgressEventArgs.Completed(this.ArchiveNameForEvent);
				saveProgress.Invoke(this, saveProgressEventArgs);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		private void OnReadStarted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.Started(this.ArchiveNameForEvent);
				readProgress.Invoke(this, readProgressEventArgs);
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		private void OnReadCompleted()
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.Completed(this.ArchiveNameForEvent);
				readProgress.Invoke(this, readProgressEventArgs);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000C22C File Offset: 0x0000A42C
		internal void OnReadBytes(ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = ReadProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, this.ReadStream.Position, this.LengthOfReadStream);
				readProgress.Invoke(this, readProgressEventArgs);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000C26C File Offset: 0x0000A46C
		internal void OnReadEntry(bool before, ZipEntry entry)
		{
			EventHandler<ReadProgressEventArgs> readProgress = this.ReadProgress;
			if (readProgress != null)
			{
				ReadProgressEventArgs readProgressEventArgs = (!before) ? ReadProgressEventArgs.After(this.ArchiveNameForEvent, entry, this._entries.Count) : ReadProgressEventArgs.Before(this.ArchiveNameForEvent, this._entries.Count);
				readProgress.Invoke(this, readProgressEventArgs);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000C2C8 File Offset: 0x0000A4C8
		private long LengthOfReadStream
		{
			get
			{
				if (this._lengthOfReadStream == -99L)
				{
					this._lengthOfReadStream = ((!this._ReadStreamIsOurs) ? -1L : SharedUtilities.GetFileLength(this._name));
				}
				return this._lengthOfReadStream;
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000C304 File Offset: 0x0000A504
		private void OnExtractEntry(int current, bool before, ZipEntry currentEntry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = new ExtractProgressEventArgs(this.ArchiveNameForEvent, before, this._entries.Count, current, currentEntry, path);
				extractProgress.Invoke(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C354 File Offset: 0x0000A554
		internal bool OnExtractBlock(ZipEntry entry, long bytesWritten, long totalBytesToWrite)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ByteUpdate(this.ArchiveNameForEvent, entry, bytesWritten, totalBytesToWrite);
				extractProgress.Invoke(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C3A0 File Offset: 0x0000A5A0
		internal bool OnSingleEntryExtract(ZipEntry entry, string path, bool before)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = (!before) ? ExtractProgressEventArgs.AfterExtractEntry(this.ArchiveNameForEvent, entry, path) : ExtractProgressEventArgs.BeforeExtractEntry(this.ArchiveNameForEvent, entry, path);
				extractProgress.Invoke(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000C400 File Offset: 0x0000A600
		internal bool OnExtractExisting(ZipEntry entry, string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractExisting(this.ArchiveNameForEvent, entry, path);
				extractProgress.Invoke(this, extractProgressEventArgs);
				if (extractProgressEventArgs.Cancel)
				{
					this._extractOperationCanceled = true;
				}
			}
			return this._extractOperationCanceled;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000C448 File Offset: 0x0000A648
		private void OnExtractAllCompleted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractAllCompleted(this.ArchiveNameForEvent, path);
				extractProgress.Invoke(this, extractProgressEventArgs);
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000C478 File Offset: 0x0000A678
		private void OnExtractAllStarted(string path)
		{
			EventHandler<ExtractProgressEventArgs> extractProgress = this.ExtractProgress;
			if (extractProgress != null)
			{
				ExtractProgressEventArgs extractProgressEventArgs = ExtractProgressEventArgs.ExtractAllStarted(this.ArchiveNameForEvent, path);
				extractProgress.Invoke(this, extractProgressEventArgs);
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
		private void OnAddStarted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Started(this.ArchiveNameForEvent);
				addProgress.Invoke(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000C4E8 File Offset: 0x0000A6E8
		private void OnAddCompleted()
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.Completed(this.ArchiveNameForEvent);
				addProgress.Invoke(this, addProgressEventArgs);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C518 File Offset: 0x0000A718
		internal void AfterAddEntry(ZipEntry entry)
		{
			EventHandler<AddProgressEventArgs> addProgress = this.AddProgress;
			if (addProgress != null)
			{
				AddProgressEventArgs addProgressEventArgs = AddProgressEventArgs.AfterEntry(this.ArchiveNameForEvent, entry, this._entries.Count);
				addProgress.Invoke(this, addProgressEventArgs);
				if (addProgressEventArgs.Cancel)
				{
					this._addOperationCanceled = true;
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C564 File Offset: 0x0000A764
		internal bool OnZipErrorSaving(ZipEntry entry, Exception exc)
		{
			if (this.ZipError != null)
			{
				object @lock = this.LOCK;
				lock (@lock)
				{
					ZipErrorEventArgs zipErrorEventArgs = ZipErrorEventArgs.Saving(this.Name, entry, exc);
					this.ZipError.Invoke(this, zipErrorEventArgs);
					if (zipErrorEventArgs.Cancel)
					{
						this._saveOperationCanceled = true;
					}
				}
			}
			return this._saveOperationCanceled;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
		public void ExtractAll(string path)
		{
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000C5F0 File Offset: 0x0000A7F0
		public void ExtractAll(string path, ExtractExistingFileAction extractExistingFile)
		{
			this.ExtractExistingFile = extractExistingFile;
			this._InternalExtractAll(path, true);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C604 File Offset: 0x0000A804
		private void _InternalExtractAll(string path, bool overrideExtractExistingProperty)
		{
			bool flag = this.Verbose;
			this._inExtractAll = true;
			try
			{
				this.OnExtractAllStarted(path);
				int num = 0;
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (flag)
					{
						this.StatusMessageTextWriter.WriteLine("\n{1,-22} {2,-8} {3,4}   {4,-8}  {0}", new object[]
						{
							"Name",
							"Modified",
							"Size",
							"Ratio",
							"Packed"
						});
						this.StatusMessageTextWriter.WriteLine(new string('-', 72));
						flag = false;
					}
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("{1,-22} {2,-8} {3,4:F0}%   {4,-8} {0}", new object[]
						{
							zipEntry.FileName,
							zipEntry.LastModified.ToString("yyyy-MM-dd HH:mm:ss"),
							zipEntry.UncompressedSize,
							zipEntry.CompressionRatio,
							zipEntry.CompressedSize
						});
						if (!string.IsNullOrEmpty(zipEntry.Comment))
						{
							this.StatusMessageTextWriter.WriteLine("  Comment: {0}", zipEntry.Comment);
						}
					}
					zipEntry.Password = this._Password;
					this.OnExtractEntry(num, true, zipEntry, path);
					if (overrideExtractExistingProperty)
					{
						zipEntry.ExtractExistingFile = this.ExtractExistingFile;
					}
					zipEntry.Extract(path);
					num++;
					this.OnExtractEntry(num, false, zipEntry, path);
					if (this._extractOperationCanceled)
					{
						break;
					}
				}
				if (!this._extractOperationCanceled)
				{
					foreach (ZipEntry zipEntry2 in this._entries.Values)
					{
						if (zipEntry2.IsDirectory || zipEntry2.FileName.EndsWith("/"))
						{
							string fileOrDirectory = (!zipEntry2.FileName.StartsWith("/")) ? Path.Combine(path, zipEntry2.FileName) : Path.Combine(path, zipEntry2.FileName.Substring(1));
							zipEntry2._SetTimes(fileOrDirectory, false);
						}
					}
					this.OnExtractAllCompleted(path);
				}
			}
			finally
			{
				this._inExtractAll = false;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
		public static ZipFile Read(string fileName)
		{
			return ZipFile.Read(fileName, null, null, null);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000C8BC File Offset: 0x0000AABC
		public static ZipFile Read(string fileName, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(fileName, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		private static ZipFile Read(string fileName, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			ZipFile zipFile = new ZipFile();
			zipFile.AlternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile.AlternateEncodingUsage = ZipOption.Always;
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._name = fileName;
			if (readProgress != null)
			{
				zipFile.ReadProgress = readProgress;
			}
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from {0}...", fileName);
			}
			ZipFile.ReadIntoInstance(zipFile);
			zipFile._fileAlreadyExists = true;
			return zipFile;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C968 File Offset: 0x0000AB68
		public static ZipFile Read(Stream zipStream)
		{
			return ZipFile.Read(zipStream, null, null, null);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C974 File Offset: 0x0000AB74
		public static ZipFile Read(Stream zipStream, ReadOptions options)
		{
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			return ZipFile.Read(zipStream, options.StatusMessageWriter, options.Encoding, options.ReadProgress);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C9AC File Offset: 0x0000ABAC
		private static ZipFile Read(Stream zipStream, TextWriter statusMessageWriter, Encoding encoding, EventHandler<ReadProgressEventArgs> readProgress)
		{
			if (zipStream == null)
			{
				throw new ArgumentNullException("zipStream");
			}
			ZipFile zipFile = new ZipFile();
			zipFile._StatusMessageTextWriter = statusMessageWriter;
			zipFile._alternateEncoding = (encoding ?? ZipFile.DefaultEncoding);
			zipFile._alternateEncodingUsage = ZipOption.Always;
			if (readProgress != null)
			{
				zipFile.ReadProgress += readProgress;
			}
			zipFile._readstream = ((zipStream.Position != 0L) ? new OffsetStream(zipStream) : zipStream);
			zipFile._ReadStreamIsOurs = false;
			if (zipFile.Verbose)
			{
				zipFile._StatusMessageTextWriter.WriteLine("reading from stream...");
			}
			ZipFile.ReadIntoInstance(zipFile);
			return zipFile;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000CA48 File Offset: 0x0000AC48
		private static void ReadIntoInstance(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			try
			{
				zf._readName = zf._name;
				if (!readStream.CanSeek)
				{
					ZipFile.ReadIntoInstance_Orig(zf);
					return;
				}
				zf.OnReadStarted();
				uint num = ZipFile.ReadFirstFourBytes(readStream);
				if (num == 101010256U)
				{
					return;
				}
				int num2 = 0;
				bool flag = false;
				long num3 = readStream.Length - 64L;
				long num4 = Math.Max(readStream.Length - 16384L, 10L);
				do
				{
					if (num3 < 0L)
					{
						num3 = 0L;
					}
					readStream.Seek(num3, 0);
					long num5 = SharedUtilities.FindSignature(readStream, 101010256);
					if (num5 != -1L)
					{
						flag = true;
					}
					else
					{
						if (num3 == 0L)
						{
							break;
						}
						num2++;
						num3 -= (long)(32 * (num2 + 1) * num2);
					}
				}
				while (!flag && num3 > num4);
				if (flag)
				{
					zf._locEndOfCDS = readStream.Position - 4L;
					byte[] array = new byte[16];
					readStream.Read(array, 0, array.Length);
					zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
					if (zf._diskNumberWithCd == 65535U)
					{
						throw new ZipException("Spanned archives with more than 65534 segments are not supported at this time.");
					}
					zf._diskNumberWithCd += 1U;
					int num6 = 12;
					uint num7 = BitConverter.ToUInt32(array, num6);
					if (num7 == 4294967295U)
					{
						ZipFile.Zip64SeekToCentralDirectory(zf);
					}
					else
					{
						zf._OffsetOfCentralDirectory = num7;
						readStream.Seek((long)((ulong)num7), 0);
					}
					ZipFile.ReadCentralDirectory(zf);
				}
				else
				{
					readStream.Seek(0L, 0);
					ZipFile.ReadIntoInstance_Orig(zf);
				}
			}
			catch (Exception innerException)
			{
				if (zf._ReadStreamIsOurs && zf._readstream != null)
				{
					try
					{
						zf._readstream.Dispose();
						zf._readstream = null;
					}
					finally
					{
					}
				}
				throw new ZipException("Cannot read that as a ZipFile", innerException);
			}
			zf._contentsChanged = false;
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		private static void Zip64SeekToCentralDirectory(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			byte[] array = new byte[16];
			readStream.Seek(-40L, 1);
			readStream.Read(array, 0, 16);
			long num = BitConverter.ToInt64(array, 8);
			zf._OffsetOfCentralDirectory = uint.MaxValue;
			zf._OffsetOfCentralDirectory64 = num;
			readStream.Seek(num, 0);
			uint num2 = (uint)SharedUtilities.ReadInt(readStream);
			if (num2 != 101075792U)
			{
				throw new BadReadException(string.Format("  Bad signature (0x{0:X8}) looking for ZIP64 EoCD Record at position 0x{1:X8}", num2, readStream.Position));
			}
			readStream.Read(array, 0, 8);
			long num3 = BitConverter.ToInt64(array, 0);
			array = new byte[num3];
			readStream.Read(array, 0, array.Length);
			num = BitConverter.ToInt64(array, 36);
			readStream.Seek(num, 0);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000CD1C File Offset: 0x0000AF1C
		private static uint ReadFirstFourBytes(Stream s)
		{
			return (uint)SharedUtilities.ReadInt(s);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000CD34 File Offset: 0x0000AF34
		private static void ReadCentralDirectory(ZipFile zf)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
			{
				zipEntry.ResetDirEntry();
				zf.OnReadEntry(true, null);
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("entry {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				if (zipEntry._InputUsesZip64)
				{
					flag = true;
				}
				dictionary.Add(zipEntry.FileName, null);
			}
			if (flag)
			{
				zf.UseZip64WhenSaving = Zip64Option.Always;
			}
			if (zf._locEndOfCDS > 0L)
			{
				zf.ReadStream.Seek(zf._locEndOfCDS, 0);
			}
			ZipFile.ReadCentralDirectoryFooter(zf);
			if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
			{
				zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
			}
			if (zf.Verbose)
			{
				zf.StatusMessageTextWriter.WriteLine("read in {0} entries.", zf._entries.Count);
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000CE4C File Offset: 0x0000B04C
		private static void ReadIntoInstance_Orig(ZipFile zf)
		{
			zf.OnReadStarted();
			zf._entries = new Dictionary<string, ZipEntry>();
			if (zf.Verbose)
			{
				if (zf.Name == null)
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip from stream...");
				}
				else
				{
					zf.StatusMessageTextWriter.WriteLine("Reading zip {0}...", zf.Name);
				}
			}
			bool first = true;
			ZipContainer zc = new ZipContainer(zf);
			ZipEntry zipEntry;
			while ((zipEntry = ZipEntry.ReadEntry(zc, first)) != null)
			{
				if (zf.Verbose)
				{
					zf.StatusMessageTextWriter.WriteLine("  {0}", zipEntry.FileName);
				}
				zf._entries.Add(zipEntry.FileName, zipEntry);
				first = false;
			}
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				ZipEntry zipEntry2;
				while ((zipEntry2 = ZipEntry.ReadDirEntry(zf, dictionary)) != null)
				{
					ZipEntry zipEntry3 = zf._entries[zipEntry2.FileName];
					if (zipEntry3 != null)
					{
						zipEntry3._Comment = zipEntry2.Comment;
						if (zipEntry2.IsDirectory)
						{
							zipEntry3.MarkAsDirectory();
						}
					}
					dictionary.Add(zipEntry2.FileName, null);
				}
				if (zf._locEndOfCDS > 0L)
				{
					zf.ReadStream.Seek(zf._locEndOfCDS, 0);
				}
				ZipFile.ReadCentralDirectoryFooter(zf);
				if (zf.Verbose && !string.IsNullOrEmpty(zf.Comment))
				{
					zf.StatusMessageTextWriter.WriteLine("Zip file Comment: {0}", zf.Comment);
				}
			}
			catch (ZipException)
			{
			}
			catch (IOException)
			{
			}
			zf.OnReadCompleted();
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000D000 File Offset: 0x0000B200
		private static void ReadCentralDirectoryFooter(ZipFile zf)
		{
			Stream readStream = zf.ReadStream;
			int num = SharedUtilities.ReadSignature(readStream);
			int num2 = 0;
			byte[] array;
			if ((long)num == 101075792L)
			{
				array = new byte[52];
				readStream.Read(array, 0, array.Length);
				long num3 = BitConverter.ToInt64(array, 0);
				if (num3 < 44L)
				{
					throw new ZipException("Bad size in the ZIP64 Central Directory.");
				}
				zf._versionMadeBy = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._versionNeededToExtract = BitConverter.ToUInt16(array, num2);
				num2 += 2;
				zf._diskNumberWithCd = BitConverter.ToUInt32(array, num2);
				num2 += 2;
				array = new byte[num3 - 44L];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
				if ((long)num != 117853008L)
				{
					throw new ZipException("Inconsistent metadata in the ZIP64 Central Directory.");
				}
				array = new byte[16];
				readStream.Read(array, 0, array.Length);
				num = SharedUtilities.ReadSignature(readStream);
			}
			if ((long)num != 101010256L)
			{
				readStream.Seek(-4L, 1);
				throw new BadReadException(string.Format("Bad signature ({0:X8}) at position 0x{1:X8}", num, readStream.Position));
			}
			array = new byte[16];
			zf.ReadStream.Read(array, 0, array.Length);
			if (zf._diskNumberWithCd == 0U)
			{
				zf._diskNumberWithCd = (uint)BitConverter.ToUInt16(array, 2);
			}
			ZipFile.ReadZipFileComment(zf);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000D154 File Offset: 0x0000B354
		private static void ReadZipFileComment(ZipFile zf)
		{
			byte[] array = new byte[2];
			zf.ReadStream.Read(array, 0, array.Length);
			short num = (short)((int)array[0] + (int)array[1] * 256);
			if (num > 0)
			{
				array = new byte[(int)num];
				zf.ReadStream.Read(array, 0, array.Length);
				string @string = zf.AlternateEncoding.GetString(array, 0, array.Length);
				zf.Comment = @string;
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		public static bool IsZipFile(string fileName)
		{
			return ZipFile.IsZipFile(fileName, false);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000D1CC File Offset: 0x0000B3CC
		public static bool IsZipFile(string fileName, bool testExtract)
		{
			bool result = false;
			try
			{
				if (!File.Exists(fileName))
				{
					return false;
				}
				using (FileStream fileStream = File.Open(fileName, 3, 1, 3))
				{
					result = ZipFile.IsZipFile(fileStream, testExtract);
				}
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000D274 File Offset: 0x0000B474
		public static bool IsZipFile(Stream stream, bool testExtract)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			bool result = false;
			try
			{
				if (!stream.CanRead)
				{
					return false;
				}
				Stream @null = Stream.Null;
				using (ZipFile zipFile = ZipFile.Read(stream, null, null, null))
				{
					if (testExtract)
					{
						foreach (ZipEntry zipEntry in zipFile)
						{
							if (!zipEntry.IsDirectory)
							{
								zipEntry.Extract(@null);
							}
						}
					}
				}
				result = true;
			}
			catch (IOException)
			{
			}
			catch (ZipException)
			{
			}
			return result;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000D394 File Offset: 0x0000B594
		private void DeleteFileWithRetry(string filename)
		{
			bool flag = false;
			int num = 3;
			int num2 = 0;
			while (num2 < num && !flag)
			{
				try
				{
					File.Delete(filename);
					flag = true;
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("************************************************** Retry delete.");
					Thread.Sleep(200 + num2 * 200);
				}
				num2++;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000D408 File Offset: 0x0000B608
		public void Save()
		{
			try
			{
				bool flag = false;
				this._saveOperationCanceled = false;
				this._numberOfSegmentsForMostRecentSave = 0U;
				this.OnSaveStarted();
				if (this.WriteStream == null)
				{
					throw new BadStateException("You haven't specified where to save the zip.");
				}
				if (this._name != null && this._name.EndsWith(".exe") && !this._SavingSfx)
				{
					throw new BadStateException("You specified an EXE for a plain zip file.");
				}
				if (!this._contentsChanged)
				{
					this.OnSaveCompleted();
					if (this.Verbose)
					{
						this.StatusMessageTextWriter.WriteLine("No save is necessary....");
					}
					return;
				}
				this.Reset(true);
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("saving....");
				}
				if (this._entries.Count >= 65535 && this._zip64 == Zip64Option.Default)
				{
					throw new ZipException("The number of entries is 65535 or greater. Consider setting the UseZip64WhenSaving property on the ZipFile instance.");
				}
				int num = 0;
				ICollection<ZipEntry> collection;
				if (this.SortEntriesBeforeSaving)
				{
					ICollection<ZipEntry> entriesSorted = this.EntriesSorted;
					collection = entriesSorted;
				}
				else
				{
					collection = this.Entries;
				}
				ICollection<ZipEntry> collection2 = collection;
				foreach (ZipEntry zipEntry in collection2)
				{
					this.OnSaveEntry(num, zipEntry, true);
					zipEntry.Write(this.WriteStream);
					if (this._saveOperationCanceled)
					{
						break;
					}
					num++;
					this.OnSaveEntry(num, zipEntry, false);
					if (this._saveOperationCanceled)
					{
						break;
					}
					if (zipEntry.IncludedInMostRecentSave)
					{
						flag |= zipEntry.OutputUsedZip64.Value;
					}
				}
				if (this._saveOperationCanceled)
				{
					return;
				}
				ZipSegmentedStream zipSegmentedStream = this.WriteStream as ZipSegmentedStream;
				this._numberOfSegmentsForMostRecentSave = ((zipSegmentedStream == null) ? 1U : zipSegmentedStream.CurrentSegment);
				bool flag2 = ZipOutput.WriteCentralDirectoryStructure(this.WriteStream, collection2, this._numberOfSegmentsForMostRecentSave, this._zip64, this.Comment, new ZipContainer(this));
				this.OnSaveEvent(ZipProgressEventType.Saving_AfterSaveTempArchive);
				this._hasBeenSaved = true;
				this._contentsChanged = false;
				flag = (flag || flag2);
				this._OutputUsesZip64 = new bool?(flag);
				if (this._name != null && (this._temporaryFileName != null || zipSegmentedStream != null))
				{
					this.WriteStream.Dispose();
					if (this._saveOperationCanceled)
					{
						return;
					}
					if (this._fileAlreadyExists && this._readstream != null)
					{
						this._readstream.Close();
						this._readstream = null;
						foreach (ZipEntry zipEntry2 in collection2)
						{
							ZipSegmentedStream zipSegmentedStream2 = zipEntry2._archiveStream as ZipSegmentedStream;
							if (zipSegmentedStream2 != null)
							{
								zipSegmentedStream2.Dispose();
							}
							zipEntry2._archiveStream = null;
						}
					}
					string text = null;
					if (File.Exists(this._name))
					{
						text = this._name + "." + SharedUtilities.GenerateRandomStringImpl(8, 0) + ".tmp";
						if (File.Exists(text))
						{
							this.DeleteFileWithRetry(text);
						}
						File.Move(this._name, text);
					}
					this.OnSaveEvent(ZipProgressEventType.Saving_BeforeRenameTempArchive);
					File.Move((zipSegmentedStream == null) ? this._temporaryFileName : zipSegmentedStream.CurrentTempName, this._name);
					this.OnSaveEvent(ZipProgressEventType.Saving_AfterRenameTempArchive);
					if (text != null)
					{
						try
						{
							if (File.Exists(text))
							{
								File.Delete(text);
							}
						}
						catch
						{
						}
					}
					this._fileAlreadyExists = true;
				}
				ZipFile.NotifyEntriesSaveComplete(collection2);
				this.OnSaveCompleted();
				this._JustSaved = true;
			}
			finally
			{
				this.CleanupAfterSaveOperation();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000D824 File Offset: 0x0000BA24
		private static void NotifyEntriesSaveComplete(ICollection<ZipEntry> c)
		{
			foreach (ZipEntry zipEntry in c)
			{
				zipEntry.NotifySaveComplete();
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D884 File Offset: 0x0000BA84
		private void RemoveTempFile()
		{
			try
			{
				if (File.Exists(this._temporaryFileName))
				{
					File.Delete(this._temporaryFileName);
				}
			}
			catch (IOException ex)
			{
				if (this.Verbose)
				{
					this.StatusMessageTextWriter.WriteLine("ZipFile::Save: could not delete temp file: {0}.", ex.Message);
				}
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
		private void CleanupAfterSaveOperation()
		{
			if (this._name != null)
			{
				if (this._writestream != null)
				{
					try
					{
						this._writestream.Dispose();
					}
					catch (IOException)
					{
					}
				}
				this._writestream = null;
				if (this._temporaryFileName != null)
				{
					this.RemoveTempFile();
					this._temporaryFileName = null;
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000D968 File Offset: 0x0000BB68
		public void Save(string fileName)
		{
			if (this._name == null)
			{
				this._writestream = null;
			}
			else
			{
				this._readName = this._name;
			}
			this._name = fileName;
			if (Directory.Exists(this._name))
			{
				throw new ZipException("Bad Directory", new ArgumentException("That name specifies an existing directory. Please specify a filename.", "fileName"));
			}
			this._contentsChanged = true;
			this._fileAlreadyExists = File.Exists(this._name);
			this.Save();
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
		public void Save(Stream outputStream)
		{
			if (outputStream == null)
			{
				throw new ArgumentNullException("outputStream");
			}
			if (!outputStream.CanWrite)
			{
				throw new ArgumentException("Must be a writable stream.", "outputStream");
			}
			this._name = null;
			this._writestream = new CountingStream(outputStream);
			this._contentsChanged = true;
			this._fileAlreadyExists = false;
			this.Save();
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000DA48 File Offset: 0x0000BC48
		public void AddSelectedFiles(string selectionCriteria)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, false);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000DA58 File Offset: 0x0000BC58
		public void AddSelectedFiles(string selectionCriteria, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, ".", null, recurseDirectories);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, false);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000DA74 File Offset: 0x0000BC74
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, bool recurseDirectories)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, null, recurseDirectories);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000DA80 File Offset: 0x0000BC80
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive)
		{
			this.AddSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, false);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public void AddSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, false);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000DA9C File Offset: 0x0000BC9C
		public void UpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories)
		{
			this._AddOrUpdateSelectedFiles(selectionCriteria, directoryOnDisk, directoryPathInArchive, recurseDirectories, true);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000DAAC File Offset: 0x0000BCAC
		private string EnsureendInSlash(string s)
		{
			if (s.EndsWith("\\"))
			{
				return s;
			}
			return s + "\\";
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000DACC File Offset: 0x0000BCCC
		private void _AddOrUpdateSelectedFiles(string selectionCriteria, string directoryOnDisk, string directoryPathInArchive, bool recurseDirectories, bool wantUpdate)
		{
			if (directoryOnDisk == null && Directory.Exists(selectionCriteria))
			{
				directoryOnDisk = selectionCriteria;
				selectionCriteria = "*.*";
			}
			else if (string.IsNullOrEmpty(directoryOnDisk))
			{
				directoryOnDisk = ".";
			}
			while (directoryOnDisk.EndsWith("\\"))
			{
				directoryOnDisk = directoryOnDisk.Substring(0, directoryOnDisk.Length - 1);
			}
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("adding selection '{0}' from dir '{1}'...", selectionCriteria, directoryOnDisk);
			}
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			ReadOnlyCollection<string> readOnlyCollection = fileSelector.SelectFiles(directoryOnDisk, recurseDirectories);
			if (this.Verbose)
			{
				this.StatusMessageTextWriter.WriteLine("found {0} files...", readOnlyCollection.Count);
			}
			this.OnAddStarted();
			AddOrUpdateAction action = (!wantUpdate) ? AddOrUpdateAction.AddOnly : AddOrUpdateAction.AddOrUpdate;
			foreach (string text in readOnlyCollection)
			{
				string text2 = (directoryPathInArchive != null) ? ZipFile.ReplaceLeadingDirectory(Path.GetDirectoryName(text), directoryOnDisk, directoryPathInArchive) : null;
				if (File.Exists(text))
				{
					if (wantUpdate)
					{
						this.UpdateFile(text, text2);
					}
					else
					{
						this.AddFile(text, text2);
					}
				}
				else
				{
					this.AddOrUpdateDirectoryImpl(text, text2, action, false, 0);
				}
			}
			this.OnAddCompleted();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000DC50 File Offset: 0x0000BE50
		private static string ReplaceLeadingDirectory(string original, string pattern, string replacement)
		{
			string text = original.ToUpper();
			string text2 = pattern.ToUpper();
			int num = text.IndexOf(text2);
			if (num != 0)
			{
				return original;
			}
			return replacement + original.Substring(text2.Length);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000DC90 File Offset: 0x0000BE90
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000DCB4 File Offset: 0x0000BEB4
		public ICollection<ZipEntry> SelectEntries(string selectionCriteria, string directoryPathInArchive)
		{
			FileSelector fileSelector = new FileSelector(selectionCriteria, this.AddDirectoryWillTraverseReparsePoints);
			return fileSelector.SelectEntries(this, directoryPathInArchive);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000DCD8 File Offset: 0x0000BED8
		public int RemoveSelectedEntries(string selectionCriteria)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000DCFC File Offset: 0x0000BEFC
		public int RemoveSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			ICollection<ZipEntry> collection = this.SelectEntries(selectionCriteria, directoryPathInArchive);
			this.RemoveEntries(collection);
			return collection.Count;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public void ExtractSelectedEntries(string selectionCriteria)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000DD94 File Offset: 0x0000BF94
		public void ExtractSelectedEntries(string selectionCriteria, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractExistingFile);
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000DE08 File Offset: 0x0000C008
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract();
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000DE7C File Offset: 0x0000C07C
		public void ExtractSelectedEntries(string selectionCriteria, string directoryInArchive, string extractDirectory)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory);
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000DEF0 File Offset: 0x0000C0F0
		public void ExtractSelectedEntries(string selectionCriteria, string directoryPathInArchive, string extractDirectory, ExtractExistingFileAction extractExistingFile)
		{
			foreach (ZipEntry zipEntry in this.SelectEntries(selectionCriteria, directoryPathInArchive))
			{
				zipEntry.Password = this._Password;
				zipEntry.Extract(extractDirectory, extractExistingFile);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000DF68 File Offset: 0x0000C168
		// (set) Token: 0x06000201 RID: 513 RVA: 0x0000DF70 File Offset: 0x0000C170
		public bool FullScan { get; set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000DF7C File Offset: 0x0000C17C
		// (set) Token: 0x06000203 RID: 515 RVA: 0x0000DF84 File Offset: 0x0000C184
		public bool SortEntriesBeforeSaving { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000DF90 File Offset: 0x0000C190
		// (set) Token: 0x06000205 RID: 517 RVA: 0x0000DF98 File Offset: 0x0000C198
		public bool AddDirectoryWillTraverseReparsePoints { get; set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000DFA4 File Offset: 0x0000C1A4
		// (set) Token: 0x06000207 RID: 519 RVA: 0x0000DFAC File Offset: 0x0000C1AC
		public int BufferSize
		{
			get
			{
				return this._BufferSize;
			}
			set
			{
				this._BufferSize = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000DFB8 File Offset: 0x0000C1B8
		// (set) Token: 0x06000209 RID: 521 RVA: 0x0000DFC0 File Offset: 0x0000C1C0
		public int CodecBufferSize { get; set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		// (set) Token: 0x0600020B RID: 523 RVA: 0x0000DFD4 File Offset: 0x0000C1D4
		public bool FlattenFoldersOnExtract { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
		// (set) Token: 0x0600020D RID: 525 RVA: 0x0000DFE8 File Offset: 0x0000C1E8
		public CompressionStrategy Strategy
		{
			get
			{
				return this._Strategy;
			}
			set
			{
				this._Strategy = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000DFF4 File Offset: 0x0000C1F4
		// (set) Token: 0x0600020F RID: 527 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000E008 File Offset: 0x0000C208
		// (set) Token: 0x06000211 RID: 529 RVA: 0x0000E010 File Offset: 0x0000C210
		public CompressionLevel CompressionLevel { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000E01C File Offset: 0x0000C21C
		// (set) Token: 0x06000213 RID: 531 RVA: 0x0000E024 File Offset: 0x0000C224
		public CompressionMethod CompressionMethod
		{
			get
			{
				return this._compressionMethod;
			}
			set
			{
				this._compressionMethod = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000E030 File Offset: 0x0000C230
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000E038 File Offset: 0x0000C238
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				this._Comment = value;
				this._contentsChanged = true;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000E048 File Offset: 0x0000C248
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0000E050 File Offset: 0x0000C250
		public bool EmitTimesInWindowsFormatWhenSaving
		{
			get
			{
				return this._emitNtfsTimes;
			}
			set
			{
				this._emitNtfsTimes = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000E05C File Offset: 0x0000C25C
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0000E064 File Offset: 0x0000C264
		public bool EmitTimesInUnixFormatWhenSaving
		{
			get
			{
				return this._emitUnixTimes;
			}
			set
			{
				this._emitUnixTimes = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000E070 File Offset: 0x0000C270
		internal bool Verbose
		{
			get
			{
				return this._StatusMessageTextWriter != null;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000E080 File Offset: 0x0000C280
		public bool ContainsEntry(string name)
		{
			return this._entries.ContainsKey(SharedUtilities.NormalizePathForUseInZipFile(name));
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000E094 File Offset: 0x0000C294
		// (set) Token: 0x0600021D RID: 541 RVA: 0x0000E09C File Offset: 0x0000C29C
		public bool CaseSensitiveRetrieval
		{
			get
			{
				return this._CaseSensitiveRetrieval;
			}
			set
			{
				if (value != this._CaseSensitiveRetrieval)
				{
					this._CaseSensitiveRetrieval = value;
					this._initEntriesDictionary();
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		// (set) Token: 0x0600021F RID: 543 RVA: 0x0000E0DC File Offset: 0x0000C2DC
		[Obsolete("Beginning with v1.9.1.6 of DotNetZip, this property is obsolete.  It will be removed in a future version of the library. Your applications should  use AlternateEncoding and AlternateEncodingUsage instead.")]
		public bool UseUnicodeAsNecessary
		{
			get
			{
				return this._alternateEncoding == Encoding.GetEncoding("UTF-8") && this._alternateEncodingUsage == ZipOption.AsNecessary;
			}
			set
			{
				if (value)
				{
					this._alternateEncoding = Encoding.GetEncoding("UTF-8");
					this._alternateEncodingUsage = ZipOption.AsNecessary;
				}
				else
				{
					this._alternateEncoding = ZipFile.DefaultEncoding;
					this._alternateEncodingUsage = ZipOption.Default;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000E120 File Offset: 0x0000C320
		// (set) Token: 0x06000221 RID: 545 RVA: 0x0000E128 File Offset: 0x0000C328
		public Zip64Option UseZip64WhenSaving
		{
			get
			{
				return this._zip64;
			}
			set
			{
				this._zip64 = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000E134 File Offset: 0x0000C334
		public bool? RequiresZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				if (!this._hasBeenSaved || this._contentsChanged)
				{
					return default(bool?);
				}
				foreach (ZipEntry zipEntry in this._entries.Values)
				{
					if (zipEntry.RequiresZip64.Value)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000E200 File Offset: 0x0000C400
		public bool? OutputUsedZip64
		{
			get
			{
				return this._OutputUsesZip64;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000E208 File Offset: 0x0000C408
		public bool? InputUsesZip64
		{
			get
			{
				if (this._entries.Count > 65534)
				{
					return new bool?(true);
				}
				foreach (ZipEntry zipEntry in this)
				{
					if (zipEntry.Source != ZipEntrySource.ZipFile)
					{
						return default(bool?);
					}
					if (zipEntry._InputUsesZip64)
					{
						return new bool?(true);
					}
				}
				return new bool?(false);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000E2B8 File Offset: 0x0000C4B8
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
		[Obsolete("use AlternateEncoding instead.")]
		public Encoding ProvisionalAlternateEncoding
		{
			get
			{
				if (this._alternateEncodingUsage == ZipOption.AsNecessary)
				{
					return this._alternateEncoding;
				}
				return null;
			}
			set
			{
				this._alternateEncoding = value;
				this._alternateEncodingUsage = ZipOption.AsNecessary;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000E2E0 File Offset: 0x0000C4E0
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000E2E8 File Offset: 0x0000C4E8
		public Encoding AlternateEncoding
		{
			get
			{
				return this._alternateEncoding;
			}
			set
			{
				this._alternateEncoding = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000E2FC File Offset: 0x0000C4FC
		public ZipOption AlternateEncodingUsage
		{
			get
			{
				return this._alternateEncodingUsage;
			}
			set
			{
				this._alternateEncodingUsage = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000E308 File Offset: 0x0000C508
		public static Encoding DefaultEncoding
		{
			get
			{
				return ZipFile._defaultEncoding;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000E310 File Offset: 0x0000C510
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000E318 File Offset: 0x0000C518
		public TextWriter StatusMessageTextWriter
		{
			get
			{
				return this._StatusMessageTextWriter;
			}
			set
			{
				this._StatusMessageTextWriter = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000E324 File Offset: 0x0000C524
		// (set) Token: 0x0600022F RID: 559 RVA: 0x0000E32C File Offset: 0x0000C52C
		public string TempFileFolder
		{
			get
			{
				return this._TempFileFolder;
			}
			set
			{
				this._TempFileFolder = value;
				if (value == null)
				{
					return;
				}
				if (!Directory.Exists(value))
				{
					throw new FileNotFoundException(string.Format("That directory ({0}) does not exist.", value));
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000E364 File Offset: 0x0000C564
		public string Password
		{
			private get
			{
				return this._Password;
			}
			set
			{
				this._Password = value;
				if (this._Password == null)
				{
					this.Encryption = EncryptionAlgorithm.None;
				}
				else if (this.Encryption == EncryptionAlgorithm.None)
				{
					this.Encryption = EncryptionAlgorithm.PkzipWeak;
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000232 RID: 562 RVA: 0x0000E3AC File Offset: 0x0000C5AC
		// (set) Token: 0x06000233 RID: 563 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public ExtractExistingFileAction ExtractExistingFile { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000E3C0 File Offset: 0x0000C5C0
		// (set) Token: 0x06000235 RID: 565 RVA: 0x0000E3DC File Offset: 0x0000C5DC
		public ZipErrorAction ZipErrorAction
		{
			get
			{
				if (this.ZipError != null)
				{
					this._zipErrorAction = ZipErrorAction.InvokeErrorEvent;
				}
				return this._zipErrorAction;
			}
			set
			{
				this._zipErrorAction = value;
				if (this._zipErrorAction != ZipErrorAction.InvokeErrorEvent && this.ZipError != null)
				{
					this.ZipError = null;
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000E404 File Offset: 0x0000C604
		// (set) Token: 0x06000237 RID: 567 RVA: 0x0000E40C File Offset: 0x0000C60C
		public EncryptionAlgorithm Encryption
		{
			get
			{
				return this._Encryption;
			}
			set
			{
				if (value == EncryptionAlgorithm.Unsupported)
				{
					throw new InvalidOperationException("You may not set Encryption to that value.");
				}
				this._Encryption = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000E428 File Offset: 0x0000C628
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000E430 File Offset: 0x0000C630
		public SetCompressionCallback SetCompression { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0000E43C File Offset: 0x0000C63C
		// (set) Token: 0x0600023B RID: 571 RVA: 0x0000E444 File Offset: 0x0000C644
		public int MaxOutputSegmentSize
		{
			get
			{
				return this._maxOutputSegmentSize;
			}
			set
			{
				if (value < 65536 && value != 0)
				{
					throw new ZipException("The minimum acceptable segment size is 65536.");
				}
				this._maxOutputSegmentSize = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000E46C File Offset: 0x0000C66C
		public int NumberOfSegmentsForMostRecentSave
		{
			get
			{
				return (int)(this._numberOfSegmentsForMostRecentSave + 1U);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000E478 File Offset: 0x0000C678
		public long ParallelDeflateThreshold
		{
			get
			{
				return this._ParallelDeflateThreshold;
			}
			set
			{
				if (value != 0L && value != -1L && value < 65536L)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateThreshold should be -1, 0, or > 65536");
				}
				this._ParallelDeflateThreshold = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		// (set) Token: 0x06000240 RID: 576 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		public int ParallelDeflateMaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentOutOfRangeException("ParallelDeflateMaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
		public override string ToString()
		{
			return string.Format("ZipFile::{0}", this.Name);
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000E4F8 File Offset: 0x0000C6F8
		public static Version LibraryVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000E50C File Offset: 0x0000C70C
		internal void NotifyEntryChanged()
		{
			this._contentsChanged = true;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000E518 File Offset: 0x0000C718
		internal Stream StreamForDiskNumber(uint diskNumber)
		{
			if (diskNumber + 1U == this._diskNumberWithCd || (diskNumber == 0U && this._diskNumberWithCd == 0U))
			{
				return this.ReadStream;
			}
			return ZipSegmentedStream.ForReading(this._readName ?? this._name, diskNumber, this._diskNumberWithCd);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000E56C File Offset: 0x0000C76C
		internal void Reset(bool whileSaving)
		{
			if (this._JustSaved)
			{
				using (ZipFile zipFile = new ZipFile())
				{
					zipFile._readName = (zipFile._name = ((!whileSaving) ? this._name : (this._readName ?? this._name)));
					zipFile.AlternateEncoding = this.AlternateEncoding;
					zipFile.AlternateEncodingUsage = this.AlternateEncodingUsage;
					ZipFile.ReadIntoInstance(zipFile);
					foreach (ZipEntry zipEntry in zipFile)
					{
						foreach (ZipEntry zipEntry2 in this)
						{
							if (zipEntry.FileName == zipEntry2.FileName)
							{
								zipEntry2.CopyMetaData(zipEntry);
								break;
							}
						}
					}
				}
				this._JustSaved = false;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		public void Initialize(string fileName)
		{
			try
			{
				this._InitInstance(fileName, null);
			}
			catch (Exception innerException)
			{
				throw new ZipException(string.Format("{0} is not a valid zip file", fileName), innerException);
			}
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000E71C File Offset: 0x0000C91C
		private void _initEntriesDictionary()
		{
			StringComparer stringComparer = (!this.CaseSensitiveRetrieval) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
			this._entries = ((this._entries != null) ? new Dictionary<string, ZipEntry>(this._entries, stringComparer) : new Dictionary<string, ZipEntry>(stringComparer));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000E76C File Offset: 0x0000C96C
		private void _InitInstance(string zipFileName, TextWriter statusMessageWriter)
		{
			this._name = zipFileName;
			this._StatusMessageTextWriter = statusMessageWriter;
			this._contentsChanged = true;
			this.AddDirectoryWillTraverseReparsePoints = true;
			this.CompressionLevel = CompressionLevel.Default;
			this.ParallelDeflateThreshold = 524288L;
			this._initEntriesDictionary();
			if (zipFileName != null && File.Exists(this._name))
			{
				if (this.FullScan)
				{
					ZipFile.ReadIntoInstance_Orig(this);
				}
				else
				{
					ZipFile.ReadIntoInstance(this);
				}
				this._fileAlreadyExists = true;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000E7E8 File Offset: 0x0000C9E8
		private List<ZipEntry> ZipEntriesAsList
		{
			get
			{
				if (this._zipEntriesAsList == null)
				{
					this._zipEntriesAsList = new List<ZipEntry>(this._entries.Values);
				}
				return this._zipEntriesAsList;
			}
		}

		// Token: 0x1700007D RID: 125
		public ZipEntry this[int ix]
		{
			get
			{
				return this.ZipEntriesAsList[ix];
			}
		}

		// Token: 0x1700007E RID: 126
		public ZipEntry this[string fileName]
		{
			get
			{
				string text = SharedUtilities.NormalizePathForUseInZipFile(fileName);
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				text = text.Replace("/", "\\");
				if (this._entries.ContainsKey(text))
				{
					return this._entries[text];
				}
				return null;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000E888 File Offset: 0x0000CA88
		public ICollection<string> EntryFileNames
		{
			get
			{
				return this._entries.Keys;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000E898 File Offset: 0x0000CA98
		public ICollection<ZipEntry> Entries
		{
			get
			{
				return this._entries.Values;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
		public ICollection<ZipEntry> EntriesSorted
		{
			get
			{
				List<ZipEntry> list = new List<ZipEntry>();
				foreach (ZipEntry zipEntry in this.Entries)
				{
					list.Add(zipEntry);
				}
				int num = (!this.CaseSensitiveRetrieval) ? 5 : 4;
				return list.AsReadOnly();
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000E92C File Offset: 0x0000CB2C
		public int Count
		{
			get
			{
				return this._entries.Count;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E93C File Offset: 0x0000CB3C
		public void RemoveEntry(ZipEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this._entries.Remove(SharedUtilities.NormalizePathForUseInZipFile(entry.FileName));
			this._zipEntriesAsList = null;
			this._contentsChanged = true;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000E980 File Offset: 0x0000CB80
		public void RemoveEntry(string fileName)
		{
			string fileName2 = ZipEntry.NameInArchive(fileName, null);
			ZipEntry zipEntry = this[fileName2];
			if (zipEntry == null)
			{
				throw new ArgumentException("The entry you specified was not found in the zip archive.");
			}
			this.RemoveEntry(zipEntry);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000E9B8 File Offset: 0x0000CBB8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000E9C8 File Offset: 0x0000CBC8
		protected virtual void Dispose(bool disposeManagedResources)
		{
			if (!this._disposed)
			{
				if (disposeManagedResources)
				{
					if (this._ReadStreamIsOurs && this._readstream != null)
					{
						this._readstream.Dispose();
						this._readstream = null;
					}
					if (this._temporaryFileName != null && this._name != null && this._writestream != null)
					{
						this._writestream.Dispose();
						this._writestream = null;
					}
					if (this.ParallelDeflater != null)
					{
						this.ParallelDeflater.Dispose();
						this.ParallelDeflater = null;
					}
				}
				this._disposed = true;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000EA68 File Offset: 0x0000CC68
		internal Stream ReadStream
		{
			get
			{
				if (this._readstream == null && (this._readName != null || this._name != null))
				{
					this._readstream = File.Open(this._readName ?? this._name, 3, 1, 3);
					this._ReadStreamIsOurs = true;
				}
				return this._readstream;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
		// (set) Token: 0x06000256 RID: 598 RVA: 0x0000EB50 File Offset: 0x0000CD50
		private Stream WriteStream
		{
			get
			{
				if (this._writestream != null)
				{
					return this._writestream;
				}
				if (this._name == null)
				{
					return this._writestream;
				}
				if (this._maxOutputSegmentSize != 0)
				{
					this._writestream = ZipSegmentedStream.ForWriting(this._name, this._maxOutputSegmentSize);
					return this._writestream;
				}
				SharedUtilities.CreateAndOpenUniqueTempFile(this.TempFileFolder ?? Path.GetDirectoryName(this._name), out this._writestream, out this._temporaryFileName);
				return this._writestream;
			}
			set
			{
				if (value != null)
				{
					throw new ZipException("Cannot set the stream to a non-null value.");
				}
				this._writestream = null;
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000EB6C File Offset: 0x0000CD6C
		public IEnumerator<ZipEntry> GetEnumerator()
		{
			foreach (ZipEntry e in this._entries.Values)
			{
				yield return e;
			}
			yield break;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000EB88 File Offset: 0x0000CD88
		[DispId(-4)]
		public IEnumerator GetNewEnum()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040000E0 RID: 224
		private long _lengthOfReadStream = -99L;

		// Token: 0x040000E1 RID: 225
		private TextWriter _StatusMessageTextWriter;

		// Token: 0x040000E2 RID: 226
		private bool _CaseSensitiveRetrieval;

		// Token: 0x040000E3 RID: 227
		private Stream _readstream;

		// Token: 0x040000E4 RID: 228
		private Stream _writestream;

		// Token: 0x040000E5 RID: 229
		private ushort _versionMadeBy;

		// Token: 0x040000E6 RID: 230
		private ushort _versionNeededToExtract;

		// Token: 0x040000E7 RID: 231
		private uint _diskNumberWithCd;

		// Token: 0x040000E8 RID: 232
		private int _maxOutputSegmentSize;

		// Token: 0x040000E9 RID: 233
		private uint _numberOfSegmentsForMostRecentSave;

		// Token: 0x040000EA RID: 234
		private ZipErrorAction _zipErrorAction;

		// Token: 0x040000EB RID: 235
		private bool _disposed;

		// Token: 0x040000EC RID: 236
		private Dictionary<string, ZipEntry> _entries;

		// Token: 0x040000ED RID: 237
		private List<ZipEntry> _zipEntriesAsList;

		// Token: 0x040000EE RID: 238
		private string _name;

		// Token: 0x040000EF RID: 239
		private string _readName;

		// Token: 0x040000F0 RID: 240
		private string _Comment;

		// Token: 0x040000F1 RID: 241
		internal string _Password;

		// Token: 0x040000F2 RID: 242
		private bool _emitNtfsTimes = true;

		// Token: 0x040000F3 RID: 243
		private bool _emitUnixTimes;

		// Token: 0x040000F4 RID: 244
		private CompressionStrategy _Strategy;

		// Token: 0x040000F5 RID: 245
		private CompressionMethod _compressionMethod = CompressionMethod.Deflate;

		// Token: 0x040000F6 RID: 246
		private bool _fileAlreadyExists;

		// Token: 0x040000F7 RID: 247
		private string _temporaryFileName;

		// Token: 0x040000F8 RID: 248
		private bool _contentsChanged;

		// Token: 0x040000F9 RID: 249
		private bool _hasBeenSaved;

		// Token: 0x040000FA RID: 250
		private string _TempFileFolder;

		// Token: 0x040000FB RID: 251
		private bool _ReadStreamIsOurs = true;

		// Token: 0x040000FC RID: 252
		private object LOCK = new object();

		// Token: 0x040000FD RID: 253
		private bool _saveOperationCanceled;

		// Token: 0x040000FE RID: 254
		private bool _extractOperationCanceled;

		// Token: 0x040000FF RID: 255
		private bool _addOperationCanceled;

		// Token: 0x04000100 RID: 256
		private EncryptionAlgorithm _Encryption;

		// Token: 0x04000101 RID: 257
		private bool _JustSaved;

		// Token: 0x04000102 RID: 258
		private long _locEndOfCDS = -1L;

		// Token: 0x04000103 RID: 259
		private uint _OffsetOfCentralDirectory;

		// Token: 0x04000104 RID: 260
		private long _OffsetOfCentralDirectory64;

		// Token: 0x04000105 RID: 261
		private bool? _OutputUsesZip64;

		// Token: 0x04000106 RID: 262
		internal bool _inExtractAll;

		// Token: 0x04000107 RID: 263
		private static Encoding _defaultEncoding = Encoding.UTF8;

		// Token: 0x04000108 RID: 264
		private Encoding _alternateEncoding = Encoding.UTF8;

		// Token: 0x04000109 RID: 265
		private ZipOption _alternateEncodingUsage;

		// Token: 0x0400010A RID: 266
		private int _BufferSize = ZipFile.BufferSizeDefault;

		// Token: 0x0400010B RID: 267
		internal ParallelDeflateOutputStream ParallelDeflater;

		// Token: 0x0400010C RID: 268
		private long _ParallelDeflateThreshold;

		// Token: 0x0400010D RID: 269
		private int _maxBufferPairs = 16;

		// Token: 0x0400010E RID: 270
		internal Zip64Option _zip64;

		// Token: 0x0400010F RID: 271
		private bool _SavingSfx;

		// Token: 0x04000110 RID: 272
		public static readonly int BufferSizeDefault = 32768;
	}
}

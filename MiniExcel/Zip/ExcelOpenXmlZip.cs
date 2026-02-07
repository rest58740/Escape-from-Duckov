using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace MiniExcelLibs.Zip
{
	// Token: 0x0200001C RID: 28
	internal class ExcelOpenXmlZip : IDisposable
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00003EF0 File Offset: 0x000020F0
		public ExcelOpenXmlZip(Stream fileStream, ZipArchiveMode mode = ZipArchiveMode.Read, bool leaveOpen = false, Encoding entryNameEncoding = null)
		{
			this.zipFile = new MiniExcelZipArchive(fileStream, mode, leaveOpen, entryNameEncoding);
			this._entries = new Dictionary<string, ZipArchiveEntry>(StringComparer.OrdinalIgnoreCase);
			try
			{
				this.entries = this.zipFile.Entries;
			}
			catch (InvalidDataException ex)
			{
				throw new InvalidDataException("It's not legal excel zip, please check or issue for me. " + ex.Message);
			}
			foreach (ZipArchiveEntry zipArchiveEntry in this.zipFile.Entries)
			{
				this._entries.Add(zipArchiveEntry.FullName.Replace('\\', '/'), zipArchiveEntry);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003FB4 File Offset: 0x000021B4
		public ZipArchiveEntry GetEntry(string path)
		{
			ZipArchiveEntry result;
			if (this._entries.TryGetValue(path, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00003FD4 File Offset: 0x000021D4
		public XmlReader GetXmlReader(string path)
		{
			ZipArchiveEntry entry = this.GetEntry(path);
			if (entry != null)
			{
				return XmlReader.Create(entry.Open(), ExcelOpenXmlZip.XmlSettings);
			}
			return null;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004000 File Offset: 0x00002200
		~ExcelOpenXmlZip()
		{
			this.Dispose(false);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004030 File Offset: 0x00002230
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000403F File Offset: 0x0000223F
		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing && this.zipFile != null)
				{
					this.zipFile.Dispose();
					this.zipFile = null;
				}
				this._disposed = true;
			}
		}

		// Token: 0x0400002A RID: 42
		private readonly Dictionary<string, ZipArchiveEntry> _entries;

		// Token: 0x0400002B RID: 43
		private bool _disposed;

		// Token: 0x0400002C RID: 44
		internal MiniExcelZipArchive zipFile;

		// Token: 0x0400002D RID: 45
		public ReadOnlyCollection<ZipArchiveEntry> entries;

		// Token: 0x0400002E RID: 46
		private static readonly XmlReaderSettings XmlSettings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			XmlResolver = null
		};
	}
}

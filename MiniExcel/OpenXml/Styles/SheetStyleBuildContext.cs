using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.Zip;

namespace MiniExcelLibs.OpenXml.Styles
{
	// Token: 0x02000056 RID: 86
	internal class SheetStyleBuildContext : IDisposable
	{
		// Token: 0x060002AB RID: 683 RVA: 0x00010A71 File Offset: 0x0000EC71
		public SheetStyleBuildContext(Dictionary<string, ZipPackageInfo> zipDictionary, MiniExcelZipArchive archive, Encoding encoding, ICollection<ExcelColumnAttribute> columns)
		{
			this._zipDictionary = zipDictionary;
			this._archive = archive;
			this._encoding = encoding;
			this._columns = columns;
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00010A96 File Offset: 0x0000EC96
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00010A9E File Offset: 0x0000EC9E
		public XmlReader OldXmlReader { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00010AA7 File Offset: 0x0000ECA7
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00010AAF File Offset: 0x0000ECAF
		public XmlWriter NewXmlWriter { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00010AB8 File Offset: 0x0000ECB8
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00010AC0 File Offset: 0x0000ECC0
		public SheetStyleElementInfos OldElementInfos { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00010AC9 File Offset: 0x0000ECC9
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x00010AD1 File Offset: 0x0000ECD1
		public SheetStyleElementInfos GenerateElementInfos { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00010ADA File Offset: 0x0000ECDA
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00010AE2 File Offset: 0x0000ECE2
		public IEnumerable<ExcelColumnAttribute> ColumnsToApply { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00010AEB File Offset: 0x0000ECEB
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00010AF3 File Offset: 0x0000ECF3
		public int CustomFormatCount { get; private set; }

		// Token: 0x060002B8 RID: 696 RVA: 0x00010AFC File Offset: 0x0000ECFC
		public void Initialize(SheetStyleElementInfos generateElementInfos)
		{
			if (this.initialized)
			{
				throw new InvalidOperationException("The context has been initialized.");
			}
			ZipArchiveEntry zipArchiveEntry;
			if (this._archive.Mode != ZipArchiveMode.Update)
			{
				zipArchiveEntry = null;
			}
			else
			{
				zipArchiveEntry = this._archive.Entries.SingleOrDefault((ZipArchiveEntry s) => s.FullName == "xl/styles.xml");
			}
			this.oldStyleXmlZipEntry = zipArchiveEntry;
			if (this.oldStyleXmlZipEntry != null)
			{
				using (Stream stream = this.oldStyleXmlZipEntry.Open())
				{
					this.OldElementInfos = SheetStyleBuildContext.ReadSheetStyleElementInfos(XmlReader.Create(stream, new XmlReaderSettings
					{
						IgnoreWhitespace = true
					}));
				}
				this.oldXmlReaderStream = this.oldStyleXmlZipEntry.Open();
				this.OldXmlReader = XmlReader.Create(this.oldXmlReaderStream, new XmlReaderSettings
				{
					IgnoreWhitespace = true
				});
				this.newStyleXmlZipEntry = this._archive.CreateEntry("xl/styles.xml.temp", CompressionLevel.Fastest);
			}
			else
			{
				this.OldElementInfos = new SheetStyleElementInfos();
				this.emptyStylesXmlStringReader = new StringReader(SheetStyleBuildContext._emptyStylesXml);
				this.OldXmlReader = XmlReader.Create(this.emptyStylesXmlStringReader, new XmlReaderSettings
				{
					IgnoreWhitespace = true
				});
				this.newStyleXmlZipEntry = this._archive.CreateEntry("xl/styles.xml", CompressionLevel.Fastest);
			}
			this.newXmlWriterStream = this.newStyleXmlZipEntry.Open();
			this.NewXmlWriter = XmlWriter.Create(this.newXmlWriterStream, new XmlWriterSettings
			{
				Indent = true,
				Encoding = this._encoding
			});
			this.GenerateElementInfos = generateElementInfos;
			this.ColumnsToApply = SheetStyleBuilderHelper.GenerateStyleIds(this.OldElementInfos.CellXfCount + generateElementInfos.CellXfCount, this._columns).ToArray<ExcelColumnAttribute>();
			this.CustomFormatCount = this.ColumnsToApply.Count<ExcelColumnAttribute>();
			this.initialized = true;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00010CC8 File Offset: 0x0000EEC8
		public Task InitializeAsync(SheetStyleElementInfos generateElementInfos)
		{
			SheetStyleBuildContext.<InitializeAsync>d__39 <InitializeAsync>d__;
			<InitializeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InitializeAsync>d__.<>4__this = this;
			<InitializeAsync>d__.generateElementInfos = generateElementInfos;
			<InitializeAsync>d__.<>1__state = -1;
			<InitializeAsync>d__.<>t__builder.Start<SheetStyleBuildContext.<InitializeAsync>d__39>(ref <InitializeAsync>d__);
			return <InitializeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00010D14 File Offset: 0x0000EF14
		public void FinalizeAndUpdateZipDictionary()
		{
			if (!this.initialized)
			{
				throw new InvalidOperationException("The context has not been initialized.");
			}
			if (this.disposed)
			{
				throw new ObjectDisposedException("SheetStyleBuildContext");
			}
			if (this.finalized)
			{
				throw new InvalidOperationException("The context has been finalized.");
			}
			try
			{
				this.OldXmlReader.Dispose();
				this.OldXmlReader = null;
				Stream stream = this.oldXmlReaderStream;
				if (stream != null)
				{
					stream.Dispose();
				}
				this.oldXmlReaderStream = null;
				StringReader stringReader = this.emptyStylesXmlStringReader;
				if (stringReader != null)
				{
					stringReader.Dispose();
				}
				this.emptyStylesXmlStringReader = null;
				this.NewXmlWriter.Flush();
				this.NewXmlWriter.Close();
				this.NewXmlWriter.Dispose();
				this.NewXmlWriter = null;
				this.newXmlWriterStream.Dispose();
				this.newXmlWriterStream = null;
				if (this.oldStyleXmlZipEntry == null)
				{
					this._zipDictionary.Add("xl/styles.xml", new ZipPackageInfo(this.newStyleXmlZipEntry, "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"));
				}
				else
				{
					ZipArchiveEntry zipArchiveEntry = this.oldStyleXmlZipEntry;
					if (zipArchiveEntry != null)
					{
						zipArchiveEntry.Delete();
					}
					this.oldStyleXmlZipEntry = null;
					ZipArchiveEntry zipArchiveEntry2 = this._archive.CreateEntry("xl/styles.xml", CompressionLevel.Fastest);
					using (Stream stream2 = this.newStyleXmlZipEntry.Open())
					{
						using (Stream stream3 = zipArchiveEntry2.Open())
						{
							stream2.CopyTo(stream3);
						}
					}
					this._zipDictionary["xl/styles.xml"] = new ZipPackageInfo(zipArchiveEntry2, "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
					this.newStyleXmlZipEntry.Delete();
					this.newStyleXmlZipEntry = null;
				}
				this.finalized = true;
			}
			catch (Exception innerException)
			{
				throw new Exception("Failed to finalize and replace styles.", innerException);
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00010EF0 File Offset: 0x0000F0F0
		public Task FinalizeAndUpdateZipDictionaryAsync()
		{
			SheetStyleBuildContext.<FinalizeAndUpdateZipDictionaryAsync>d__41 <FinalizeAndUpdateZipDictionaryAsync>d__;
			<FinalizeAndUpdateZipDictionaryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FinalizeAndUpdateZipDictionaryAsync>d__.<>4__this = this;
			<FinalizeAndUpdateZipDictionaryAsync>d__.<>1__state = -1;
			<FinalizeAndUpdateZipDictionaryAsync>d__.<>t__builder.Start<SheetStyleBuildContext.<FinalizeAndUpdateZipDictionaryAsync>d__41>(ref <FinalizeAndUpdateZipDictionaryAsync>d__);
			return <FinalizeAndUpdateZipDictionaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x00010F34 File Offset: 0x0000F134
		private static SheetStyleElementInfos ReadSheetStyleElementInfos(XmlReader reader)
		{
			SheetStyleElementInfos sheetStyleElementInfos = new SheetStyleElementInfos();
			while (reader.Read())
			{
				SheetStyleBuildContext.SetElementInfos(reader, sheetStyleElementInfos);
			}
			return sheetStyleElementInfos;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x00010F5C File Offset: 0x0000F15C
		private static Task<SheetStyleElementInfos> ReadSheetStyleElementInfosAsync(XmlReader reader)
		{
			SheetStyleBuildContext.<ReadSheetStyleElementInfosAsync>d__43 <ReadSheetStyleElementInfosAsync>d__;
			<ReadSheetStyleElementInfosAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SheetStyleElementInfos>.Create();
			<ReadSheetStyleElementInfosAsync>d__.reader = reader;
			<ReadSheetStyleElementInfosAsync>d__.<>1__state = -1;
			<ReadSheetStyleElementInfosAsync>d__.<>t__builder.Start<SheetStyleBuildContext.<ReadSheetStyleElementInfosAsync>d__43>(ref <ReadSheetStyleElementInfosAsync>d__);
			return <ReadSheetStyleElementInfosAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00010FA0 File Offset: 0x0000F1A0
		private static void SetElementInfos(XmlReader reader, SheetStyleElementInfos elementInfos)
		{
			SheetStyleBuildContext.<>c__DisplayClass44_0 CS$<>8__locals1;
			CS$<>8__locals1.reader = reader;
			if (CS$<>8__locals1.reader.NodeType == XmlNodeType.Element)
			{
				string localName = CS$<>8__locals1.reader.LocalName;
				if (localName == "numFmts")
				{
					elementInfos.ExistsNumFmts = true;
					elementInfos.NumFmtCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
					return;
				}
				if (localName == "fonts")
				{
					elementInfos.ExistsFonts = true;
					elementInfos.FontCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
					return;
				}
				if (localName == "fills")
				{
					elementInfos.ExistsFills = true;
					elementInfos.FillCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
					return;
				}
				if (localName == "borders")
				{
					elementInfos.ExistsBorders = true;
					elementInfos.BorderCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
					return;
				}
				if (localName == "cellStyleXfs")
				{
					elementInfos.ExistsCellStyleXfs = true;
					elementInfos.CellStyleXfCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
					return;
				}
				if (!(localName == "cellXfs"))
				{
					return;
				}
				elementInfos.ExistsCellXfs = true;
				elementInfos.CellXfCount = SheetStyleBuildContext.<SetElementInfos>g__GetCount|44_0(ref CS$<>8__locals1);
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0001109E File Offset: 0x0000F29E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000110B0 File Offset: 0x0000F2B0
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					XmlReader oldXmlReader = this.OldXmlReader;
					if (oldXmlReader != null)
					{
						oldXmlReader.Dispose();
					}
					Stream stream = this.oldXmlReaderStream;
					if (stream != null)
					{
						stream.Dispose();
					}
					StringReader stringReader = this.emptyStylesXmlStringReader;
					if (stringReader != null)
					{
						stringReader.Dispose();
					}
					XmlWriter newXmlWriter = this.NewXmlWriter;
					if (newXmlWriter != null)
					{
						newXmlWriter.Dispose();
					}
					Stream stream2 = this.newXmlWriterStream;
					if (stream2 != null)
					{
						stream2.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00011138 File Offset: 0x0000F338
		[CompilerGenerated]
		internal static int <SetElementInfos>g__GetCount|44_0(ref SheetStyleBuildContext.<>c__DisplayClass44_0 A_0)
		{
			string attribute = A_0.reader.GetAttribute("count");
			int result;
			if (!string.IsNullOrEmpty(attribute) && int.TryParse(attribute, out result))
			{
				return result;
			}
			return 0;
		}

		// Token: 0x040000F5 RID: 245
		private static readonly string _emptyStylesXml = ExcelOpenXmlUtils.MinifyXml("\r\n            <?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n            <x:styleSheet xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">                \r\n            </x:styleSheet>");

		// Token: 0x040000F6 RID: 246
		private readonly Dictionary<string, ZipPackageInfo> _zipDictionary;

		// Token: 0x040000F7 RID: 247
		private readonly MiniExcelZipArchive _archive;

		// Token: 0x040000F8 RID: 248
		private readonly Encoding _encoding;

		// Token: 0x040000F9 RID: 249
		private readonly ICollection<ExcelColumnAttribute> _columns;

		// Token: 0x040000FA RID: 250
		private StringReader emptyStylesXmlStringReader;

		// Token: 0x040000FB RID: 251
		private ZipArchiveEntry oldStyleXmlZipEntry;

		// Token: 0x040000FC RID: 252
		private ZipArchiveEntry newStyleXmlZipEntry;

		// Token: 0x040000FD RID: 253
		private Stream oldXmlReaderStream;

		// Token: 0x040000FE RID: 254
		private Stream newXmlWriterStream;

		// Token: 0x040000FF RID: 255
		private bool initialized;

		// Token: 0x04000100 RID: 256
		private bool finalized;

		// Token: 0x04000101 RID: 257
		private bool disposed;
	}
}

using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace MiniExcelLibs.Zip
{
	// Token: 0x0200001D RID: 29
	public class MiniExcelZipArchive : ZipArchive
	{
		// Token: 0x060000CF RID: 207 RVA: 0x0000408E File Offset: 0x0000228E
		public MiniExcelZipArchive(Stream stream, ZipArchiveMode mode, bool leaveOpen, Encoding entryNameEncoding) : base(stream, mode, leaveOpen, entryNameEncoding)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000409B File Offset: 0x0000229B
		public new void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}

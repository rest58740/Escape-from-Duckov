using System;
using System.IO.Compression;

namespace MiniExcelLibs.Zip
{
	// Token: 0x0200001E RID: 30
	internal class ZipPackageInfo
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000040AA File Offset: 0x000022AA
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000040B2 File Offset: 0x000022B2
		public ZipArchiveEntry ZipArchiveEntry { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000040BB File Offset: 0x000022BB
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000040C3 File Offset: 0x000022C3
		public string ContentType { get; set; }

		// Token: 0x060000D5 RID: 213 RVA: 0x000040CC File Offset: 0x000022CC
		public ZipPackageInfo(ZipArchiveEntry zipArchiveEntry, string contentType)
		{
			this.ZipArchiveEntry = zipArchiveEntry;
			this.ContentType = contentType;
		}
	}
}

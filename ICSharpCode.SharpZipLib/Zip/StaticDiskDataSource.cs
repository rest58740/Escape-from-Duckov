using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200002E RID: 46
	public class StaticDiskDataSource : IStaticDataSource
	{
		// Token: 0x060001DF RID: 479 RVA: 0x0000D7B0 File Offset: 0x0000B9B0
		public StaticDiskDataSource(string fileName)
		{
			this.fileName_ = fileName;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000D7C0 File Offset: 0x0000B9C0
		public Stream GetSource()
		{
			return File.Open(this.fileName_, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		// Token: 0x04000190 RID: 400
		private string fileName_;
	}
}

using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200002D RID: 45
	public interface IDynamicDataSource
	{
		// Token: 0x060001DE RID: 478
		Stream GetSource(ZipEntry entry, string name);
	}
}

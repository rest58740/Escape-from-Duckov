using System;
using System.IO;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x0200002F RID: 47
	public class DynamicDiskDataSource : IDynamicDataSource
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
		public Stream GetSource(ZipEntry entry, string name)
		{
			Stream result = null;
			if (name != null)
			{
				result = File.Open(name, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			return result;
		}
	}
}

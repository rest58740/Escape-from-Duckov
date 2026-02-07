using System;
using System.IO;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000036 RID: 54
	internal static class FileHelper
	{
		// Token: 0x06000172 RID: 370 RVA: 0x000062F8 File Offset: 0x000044F8
		public static FileStream OpenSharedRead(string path)
		{
			return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		}
	}
}

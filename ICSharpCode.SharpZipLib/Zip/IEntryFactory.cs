using System;
using ICSharpCode.SharpZipLib.Core;

namespace ICSharpCode.SharpZipLib.Zip
{
	// Token: 0x02000067 RID: 103
	public interface IEntryFactory
	{
		// Token: 0x06000475 RID: 1141
		ZipEntry MakeFileEntry(string fileName);

		// Token: 0x06000476 RID: 1142
		ZipEntry MakeFileEntry(string fileName, bool useFileSystem);

		// Token: 0x06000477 RID: 1143
		ZipEntry MakeFileEntry(string fileName, string entryName, bool useFileSystem);

		// Token: 0x06000478 RID: 1144
		ZipEntry MakeDirectoryEntry(string directoryName);

		// Token: 0x06000479 RID: 1145
		ZipEntry MakeDirectoryEntry(string directoryName, bool useFileSystem);

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600047A RID: 1146
		// (set) Token: 0x0600047B RID: 1147
		INameTransform NameTransform { get; set; }
	}
}

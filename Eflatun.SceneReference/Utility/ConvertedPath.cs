using System;
using System.IO;

namespace Eflatun.SceneReference.Utility
{
	// Token: 0x0200000F RID: 15
	internal class ConvertedPath
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002731 File Offset: 0x00000931
		public string GivenPath { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002739 File Offset: 0x00000939
		public string WindowsPath { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002741 File Offset: 0x00000941
		public string UnixPath { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002749 File Offset: 0x00000949
		public string PlatformPath { get; }

		// Token: 0x06000038 RID: 56 RVA: 0x00002754 File Offset: 0x00000954
		public ConvertedPath(string path)
		{
			this.GivenPath = path;
			string[] value = path.Split(new char[]
			{
				'\\',
				'/'
			});
			this.WindowsPath = string.Join("\\", value);
			this.UnixPath = string.Join("/", value);
			this.PlatformPath = string.Join(Path.DirectorySeparatorChar.ToString(), value);
		}
	}
}

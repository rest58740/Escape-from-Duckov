using System;

namespace ICSharpCode.SharpZipLib.GZip
{
	// Token: 0x02000055 RID: 85
	public sealed class GZipConstants
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x00016B7C File Offset: 0x00014D7C
		private GZipConstants()
		{
		}

		// Token: 0x040002B7 RID: 695
		public const int GZIP_MAGIC = 8075;

		// Token: 0x040002B8 RID: 696
		public const int FTEXT = 1;

		// Token: 0x040002B9 RID: 697
		public const int FHCRC = 2;

		// Token: 0x040002BA RID: 698
		public const int FEXTRA = 4;

		// Token: 0x040002BB RID: 699
		public const int FNAME = 8;

		// Token: 0x040002BC RID: 700
		public const int FCOMMENT = 16;
	}
}

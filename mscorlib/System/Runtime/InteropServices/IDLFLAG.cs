using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000726 RID: 1830
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.IDLFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04002B4C RID: 11084
		IDLFLAG_NONE = 0,
		// Token: 0x04002B4D RID: 11085
		IDLFLAG_FIN = 1,
		// Token: 0x04002B4E RID: 11086
		IDLFLAG_FOUT = 2,
		// Token: 0x04002B4F RID: 11087
		IDLFLAG_FLCID = 4,
		// Token: 0x04002B50 RID: 11088
		IDLFLAG_FRETVAL = 8
	}
}

using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007B8 RID: 1976
	[Flags]
	[Serializable]
	public enum IDLFLAG : short
	{
		// Token: 0x04002C9F RID: 11423
		IDLFLAG_NONE = 0,
		// Token: 0x04002CA0 RID: 11424
		IDLFLAG_FIN = 1,
		// Token: 0x04002CA1 RID: 11425
		IDLFLAG_FOUT = 2,
		// Token: 0x04002CA2 RID: 11426
		IDLFLAG_FLCID = 4,
		// Token: 0x04002CA3 RID: 11427
		IDLFLAG_FRETVAL = 8
	}
}

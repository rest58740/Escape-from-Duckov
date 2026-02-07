using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000728 RID: 1832
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.PARAMFLAG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Flags]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04002B54 RID: 11092
		PARAMFLAG_NONE = 0,
		// Token: 0x04002B55 RID: 11093
		PARAMFLAG_FIN = 1,
		// Token: 0x04002B56 RID: 11094
		PARAMFLAG_FOUT = 2,
		// Token: 0x04002B57 RID: 11095
		PARAMFLAG_FLCID = 4,
		// Token: 0x04002B58 RID: 11096
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04002B59 RID: 11097
		PARAMFLAG_FOPT = 16,
		// Token: 0x04002B5A RID: 11098
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04002B5B RID: 11099
		PARAMFLAG_FHASCUSTDATA = 64
	}
}

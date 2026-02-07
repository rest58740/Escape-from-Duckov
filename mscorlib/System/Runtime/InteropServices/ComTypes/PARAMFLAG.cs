using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007BA RID: 1978
	[Flags]
	[Serializable]
	public enum PARAMFLAG : short
	{
		// Token: 0x04002CA7 RID: 11431
		PARAMFLAG_NONE = 0,
		// Token: 0x04002CA8 RID: 11432
		PARAMFLAG_FIN = 1,
		// Token: 0x04002CA9 RID: 11433
		PARAMFLAG_FOUT = 2,
		// Token: 0x04002CAA RID: 11434
		PARAMFLAG_FLCID = 4,
		// Token: 0x04002CAB RID: 11435
		PARAMFLAG_FRETVAL = 8,
		// Token: 0x04002CAC RID: 11436
		PARAMFLAG_FOPT = 16,
		// Token: 0x04002CAD RID: 11437
		PARAMFLAG_FHASDEFAULT = 32,
		// Token: 0x04002CAE RID: 11438
		PARAMFLAG_FHASCUSTDATA = 64
	}
}

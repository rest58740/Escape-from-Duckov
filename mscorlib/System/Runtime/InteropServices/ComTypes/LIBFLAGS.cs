using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007CC RID: 1996
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04002D09 RID: 11529
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04002D0A RID: 11530
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04002D0B RID: 11531
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04002D0C RID: 11532
		LIBFLAG_FHASDISKIMAGE = 8
	}
}

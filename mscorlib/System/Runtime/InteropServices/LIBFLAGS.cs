using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000746 RID: 1862
	[Obsolete]
	[Flags]
	[Serializable]
	public enum LIBFLAGS : short
	{
		// Token: 0x04002BC7 RID: 11207
		LIBFLAG_FRESTRICTED = 1,
		// Token: 0x04002BC8 RID: 11208
		LIBFLAG_FCONTROL = 2,
		// Token: 0x04002BC9 RID: 11209
		LIBFLAG_FHIDDEN = 4,
		// Token: 0x04002BCA RID: 11210
		LIBFLAG_FHASDISKIMAGE = 8
	}
}

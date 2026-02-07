using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000674 RID: 1652
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum StreamingContextStates
	{
		// Token: 0x04002798 RID: 10136
		CrossProcess = 1,
		// Token: 0x04002799 RID: 10137
		CrossMachine = 2,
		// Token: 0x0400279A RID: 10138
		File = 4,
		// Token: 0x0400279B RID: 10139
		Persistence = 8,
		// Token: 0x0400279C RID: 10140
		Remoting = 16,
		// Token: 0x0400279D RID: 10141
		Other = 32,
		// Token: 0x0400279E RID: 10142
		Clone = 64,
		// Token: 0x0400279F RID: 10143
		CrossAppDomain = 128,
		// Token: 0x040027A0 RID: 10144
		All = 255
	}
}

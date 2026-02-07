using System;

namespace System.Reflection
{
	// Token: 0x020008BB RID: 2235
	[Flags]
	public enum PropertyAttributes
	{
		// Token: 0x04002F1E RID: 12062
		None = 0,
		// Token: 0x04002F1F RID: 12063
		SpecialName = 512,
		// Token: 0x04002F20 RID: 12064
		RTSpecialName = 1024,
		// Token: 0x04002F21 RID: 12065
		HasDefault = 4096,
		// Token: 0x04002F22 RID: 12066
		Reserved2 = 8192,
		// Token: 0x04002F23 RID: 12067
		Reserved3 = 16384,
		// Token: 0x04002F24 RID: 12068
		Reserved4 = 32768,
		// Token: 0x04002F25 RID: 12069
		ReservedMask = 62464
	}
}

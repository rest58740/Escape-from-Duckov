using System;

namespace System.Reflection
{
	// Token: 0x020008B5 RID: 2229
	[Flags]
	public enum ParameterAttributes
	{
		// Token: 0x04002EFA RID: 12026
		None = 0,
		// Token: 0x04002EFB RID: 12027
		In = 1,
		// Token: 0x04002EFC RID: 12028
		Out = 2,
		// Token: 0x04002EFD RID: 12029
		Lcid = 4,
		// Token: 0x04002EFE RID: 12030
		Retval = 8,
		// Token: 0x04002EFF RID: 12031
		Optional = 16,
		// Token: 0x04002F00 RID: 12032
		HasDefault = 4096,
		// Token: 0x04002F01 RID: 12033
		HasFieldMarshal = 8192,
		// Token: 0x04002F02 RID: 12034
		Reserved3 = 16384,
		// Token: 0x04002F03 RID: 12035
		Reserved4 = 32768,
		// Token: 0x04002F04 RID: 12036
		ReservedMask = 61440
	}
}

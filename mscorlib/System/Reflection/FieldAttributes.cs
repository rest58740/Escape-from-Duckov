using System;

namespace System.Reflection
{
	// Token: 0x0200089E RID: 2206
	[Flags]
	public enum FieldAttributes
	{
		// Token: 0x04002E92 RID: 11922
		FieldAccessMask = 7,
		// Token: 0x04002E93 RID: 11923
		PrivateScope = 0,
		// Token: 0x04002E94 RID: 11924
		Private = 1,
		// Token: 0x04002E95 RID: 11925
		FamANDAssem = 2,
		// Token: 0x04002E96 RID: 11926
		Assembly = 3,
		// Token: 0x04002E97 RID: 11927
		Family = 4,
		// Token: 0x04002E98 RID: 11928
		FamORAssem = 5,
		// Token: 0x04002E99 RID: 11929
		Public = 6,
		// Token: 0x04002E9A RID: 11930
		Static = 16,
		// Token: 0x04002E9B RID: 11931
		InitOnly = 32,
		// Token: 0x04002E9C RID: 11932
		Literal = 64,
		// Token: 0x04002E9D RID: 11933
		NotSerialized = 128,
		// Token: 0x04002E9E RID: 11934
		SpecialName = 512,
		// Token: 0x04002E9F RID: 11935
		PinvokeImpl = 8192,
		// Token: 0x04002EA0 RID: 11936
		RTSpecialName = 1024,
		// Token: 0x04002EA1 RID: 11937
		HasFieldMarshal = 4096,
		// Token: 0x04002EA2 RID: 11938
		HasDefault = 32768,
		// Token: 0x04002EA3 RID: 11939
		HasFieldRVA = 256,
		// Token: 0x04002EA4 RID: 11940
		ReservedMask = 38144
	}
}

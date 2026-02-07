using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x020007C5 RID: 1989
	[Flags]
	[Serializable]
	public enum INVOKEKIND
	{
		// Token: 0x04002CD8 RID: 11480
		INVOKE_FUNC = 1,
		// Token: 0x04002CD9 RID: 11481
		INVOKE_PROPERTYGET = 2,
		// Token: 0x04002CDA RID: 11482
		INVOKE_PROPERTYPUT = 4,
		// Token: 0x04002CDB RID: 11483
		INVOKE_PROPERTYPUTREF = 8
	}
}

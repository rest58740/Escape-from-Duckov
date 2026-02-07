using System;

namespace System.Reflection
{
	// Token: 0x02000894 RID: 2196
	[Flags]
	public enum CallingConventions
	{
		// Token: 0x04002E7E RID: 11902
		Standard = 1,
		// Token: 0x04002E7F RID: 11903
		VarArgs = 2,
		// Token: 0x04002E80 RID: 11904
		Any = 3,
		// Token: 0x04002E81 RID: 11905
		HasThis = 32,
		// Token: 0x04002E82 RID: 11906
		ExplicitThis = 64
	}
}

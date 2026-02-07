using System;

namespace System.Reflection
{
	// Token: 0x0200089D RID: 2205
	[Flags]
	public enum ExceptionHandlingClauseOptions
	{
		// Token: 0x04002E8D RID: 11917
		Clause = 0,
		// Token: 0x04002E8E RID: 11918
		Filter = 1,
		// Token: 0x04002E8F RID: 11919
		Finally = 2,
		// Token: 0x04002E90 RID: 11920
		Fault = 4
	}
}

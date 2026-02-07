using System;

namespace System.Reflection
{
	// Token: 0x020008AB RID: 2219
	[Flags]
	public enum MemberTypes
	{
		// Token: 0x04002EBB RID: 11963
		Constructor = 1,
		// Token: 0x04002EBC RID: 11964
		Event = 2,
		// Token: 0x04002EBD RID: 11965
		Field = 4,
		// Token: 0x04002EBE RID: 11966
		Method = 8,
		// Token: 0x04002EBF RID: 11967
		Property = 16,
		// Token: 0x04002EC0 RID: 11968
		TypeInfo = 32,
		// Token: 0x04002EC1 RID: 11969
		Custom = 64,
		// Token: 0x04002EC2 RID: 11970
		NestedType = 128,
		// Token: 0x04002EC3 RID: 11971
		All = 191
	}
}

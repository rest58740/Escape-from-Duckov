using System;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020007F8 RID: 2040
	public interface ITuple
	{
		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600460A RID: 17930
		int Length { get; }

		// Token: 0x17000AC4 RID: 2756
		object this[int index]
		{
			get;
		}
	}
}

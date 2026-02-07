using System;

namespace System
{
	// Token: 0x02000143 RID: 323
	public interface IProgress<in T>
	{
		// Token: 0x06000C05 RID: 3077
		void Report(T value);
	}
}

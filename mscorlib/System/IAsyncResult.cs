using System;
using System.Threading;

namespace System
{
	// Token: 0x02000137 RID: 311
	public interface IAsyncResult
	{
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000BE4 RID: 3044
		bool IsCompleted { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000BE5 RID: 3045
		WaitHandle AsyncWaitHandle { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000BE6 RID: 3046
		object AsyncState { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000BE7 RID: 3047
		bool CompletedSynchronously { get; }
	}
}

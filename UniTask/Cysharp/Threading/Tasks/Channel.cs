using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000010 RID: 16
	public static class Channel
	{
		// Token: 0x06000050 RID: 80 RVA: 0x00002A90 File Offset: 0x00000C90
		public static Channel<T> CreateSingleConsumerUnbounded<T>()
		{
			return new SingleConsumerUnboundedChannel<T>();
		}
	}
}

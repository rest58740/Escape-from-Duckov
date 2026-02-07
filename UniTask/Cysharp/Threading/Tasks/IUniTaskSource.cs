using System;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000023 RID: 35
	public interface IUniTaskSource : IValueTaskSource
	{
		// Token: 0x06000084 RID: 132
		UniTaskStatus GetStatus(short token);

		// Token: 0x06000085 RID: 133
		void OnCompleted(Action<object> continuation, object state, short token);

		// Token: 0x06000086 RID: 134
		void GetResult(short token);

		// Token: 0x06000087 RID: 135
		UniTaskStatus UnsafeGetStatus();

		// Token: 0x06000088 RID: 136 RVA: 0x00002F1C File Offset: 0x0000111C
		ValueTaskSourceStatus GetStatus(short token)
		{
			return this.GetStatus(token);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00002F25 File Offset: 0x00001125
		void GetResult(short token)
		{
			this.GetResult(token);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00002F2E File Offset: 0x0000112E
		void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
		{
			this.OnCompleted(continuation, state, token);
		}
	}
}

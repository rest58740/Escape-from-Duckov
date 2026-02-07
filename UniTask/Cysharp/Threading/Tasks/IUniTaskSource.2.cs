using System;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000024 RID: 36
	public interface IUniTaskSource<out T> : IUniTaskSource, IValueTaskSource, IValueTaskSource<T>
	{
		// Token: 0x0600008B RID: 139
		T GetResult(short token);

		// Token: 0x0600008C RID: 140 RVA: 0x00002F39 File Offset: 0x00001139
		UniTaskStatus GetStatus(short token)
		{
			return this.GetStatus(token);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00002F42 File Offset: 0x00001142
		void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.OnCompleted(continuation, state, token);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00002F4D File Offset: 0x0000114D
		ValueTaskSourceStatus GetStatus(short token)
		{
			return this.GetStatus(token);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00002F56 File Offset: 0x00001156
		T GetResult(short token)
		{
			return this.GetResult(token);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00002F5F File Offset: 0x0000115F
		void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
		{
			this.OnCompleted(continuation, state, token);
		}
	}
}

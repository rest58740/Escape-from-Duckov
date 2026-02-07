using System;
using System.Threading.Tasks.Sources;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000026 RID: 38
	public abstract class MoveNextSource : IUniTaskSource<bool>, IUniTaskSource, IValueTaskSource, IValueTaskSource<bool>
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00002F82 File Offset: 0x00001182
		public bool GetResult(short token)
		{
			return this.completionSource.GetResult(token);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002F90 File Offset: 0x00001190
		public UniTaskStatus GetStatus(short token)
		{
			return this.completionSource.GetStatus(token);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002F9E File Offset: 0x0000119E
		public void OnCompleted(Action<object> continuation, object state, short token)
		{
			this.completionSource.OnCompleted(continuation, state, token);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002FAE File Offset: 0x000011AE
		public UniTaskStatus UnsafeGetStatus()
		{
			return this.completionSource.UnsafeGetStatus();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002FBB File Offset: 0x000011BB
		void IUniTaskSource.GetResult(short token)
		{
			this.completionSource.GetResult(token);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002FCC File Offset: 0x000011CC
		protected bool TryGetResult<T>(UniTask<T>.Awaiter awaiter, out T result)
		{
			bool result2;
			try
			{
				result = awaiter.GetResult();
				result2 = true;
			}
			catch (Exception error)
			{
				this.completionSource.TrySetException(error);
				result = default(T);
				result2 = false;
			}
			return result2;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003014 File Offset: 0x00001214
		protected bool TryGetResult(UniTask.Awaiter awaiter)
		{
			bool result;
			try
			{
				awaiter.GetResult();
				result = true;
			}
			catch (Exception error)
			{
				this.completionSource.TrySetException(error);
				result = false;
			}
			return result;
		}

		// Token: 0x0400002D RID: 45
		protected UniTaskCompletionSourceCore<bool> completionSource;
	}
}

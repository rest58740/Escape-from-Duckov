using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000005 RID: 5
	public class AsyncLazy<T>
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000230B File Offset: 0x0000050B
		public AsyncLazy(Func<UniTask<T>> taskFactory)
		{
			this.taskFactory = taskFactory;
			this.completionSource = new UniTaskCompletionSource<T>();
			this.syncLock = new object();
			this.initialized = false;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002338 File Offset: 0x00000538
		internal AsyncLazy(UniTask<T> task)
		{
			this.taskFactory = null;
			this.completionSource = new UniTaskCompletionSource<T>();
			this.syncLock = null;
			this.initialized = true;
			UniTask<T>.Awaiter awaiter = task.GetAwaiter();
			if (awaiter.IsCompleted)
			{
				this.SetCompletionSource(awaiter);
				return;
			}
			this.awaiter = awaiter;
			awaiter.SourceOnCompleted(AsyncLazy<T>.continuation, this);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002399 File Offset: 0x00000599
		public UniTask<T> Task
		{
			get
			{
				this.EnsureInitialized();
				return this.completionSource.Task;
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023AC File Offset: 0x000005AC
		public UniTask<T>.Awaiter GetAwaiter()
		{
			return this.Task.GetAwaiter();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023C7 File Offset: 0x000005C7
		private void EnsureInitialized()
		{
			if (Volatile.Read(ref this.initialized))
			{
				return;
			}
			this.EnsureInitializedCore();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023E0 File Offset: 0x000005E0
		private void EnsureInitializedCore()
		{
			object obj = this.syncLock;
			lock (obj)
			{
				if (!Volatile.Read(ref this.initialized))
				{
					Func<UniTask<T>> func = Interlocked.Exchange<Func<UniTask<T>>>(ref this.taskFactory, null);
					if (func != null)
					{
						UniTask<T>.Awaiter awaiter = func().GetAwaiter();
						if (awaiter.IsCompleted)
						{
							this.SetCompletionSource(awaiter);
						}
						else
						{
							this.awaiter = awaiter;
							awaiter.SourceOnCompleted(AsyncLazy<T>.continuation, this);
						}
						Volatile.Write(ref this.initialized, true);
					}
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000247C File Offset: 0x0000067C
		private void SetCompletionSource(in UniTask<T>.Awaiter awaiter)
		{
			try
			{
				T result = awaiter.GetResult();
				this.completionSource.TrySetResult(result);
			}
			catch (Exception exception)
			{
				this.completionSource.TrySetException(exception);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024C0 File Offset: 0x000006C0
		private static void SetCompletionSource(object state)
		{
			AsyncLazy<T> asyncLazy = (AsyncLazy<T>)state;
			try
			{
				T result = asyncLazy.awaiter.GetResult();
				asyncLazy.completionSource.TrySetResult(result);
			}
			catch (Exception exception)
			{
				asyncLazy.completionSource.TrySetException(exception);
			}
			finally
			{
				asyncLazy.awaiter = default(UniTask<T>.Awaiter);
			}
		}

		// Token: 0x04000008 RID: 8
		private static Action<object> continuation = new Action<object>(AsyncLazy<T>.SetCompletionSource);

		// Token: 0x04000009 RID: 9
		private Func<UniTask<T>> taskFactory;

		// Token: 0x0400000A RID: 10
		private UniTaskCompletionSource<T> completionSource;

		// Token: 0x0400000B RID: 11
		private UniTask<T>.Awaiter awaiter;

		// Token: 0x0400000C RID: 12
		private object syncLock;

		// Token: 0x0400000D RID: 13
		private bool initialized;
	}
}

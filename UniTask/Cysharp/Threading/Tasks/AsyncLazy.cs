using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000004 RID: 4
	public class AsyncLazy
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020DA File Offset: 0x000002DA
		public AsyncLazy(Func<UniTask> taskFactory)
		{
			this.taskFactory = taskFactory;
			this.completionSource = new UniTaskCompletionSource();
			this.syncLock = new object();
			this.initialized = false;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002108 File Offset: 0x00000308
		internal AsyncLazy(UniTask task)
		{
			this.taskFactory = null;
			this.completionSource = new UniTaskCompletionSource();
			this.syncLock = null;
			this.initialized = true;
			UniTask.Awaiter awaiter = task.GetAwaiter();
			if (awaiter.IsCompleted)
			{
				this.SetCompletionSource(awaiter);
				return;
			}
			this.awaiter = awaiter;
			awaiter.SourceOnCompleted(AsyncLazy.continuation, this);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002169 File Offset: 0x00000369
		public UniTask Task
		{
			get
			{
				this.EnsureInitialized();
				return this.completionSource.Task;
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000217C File Offset: 0x0000037C
		public UniTask.Awaiter GetAwaiter()
		{
			return this.Task.GetAwaiter();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002197 File Offset: 0x00000397
		private void EnsureInitialized()
		{
			if (Volatile.Read(ref this.initialized))
			{
				return;
			}
			this.EnsureInitializedCore();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021B0 File Offset: 0x000003B0
		private void EnsureInitializedCore()
		{
			object obj = this.syncLock;
			lock (obj)
			{
				if (!Volatile.Read(ref this.initialized))
				{
					Func<UniTask> func = Interlocked.Exchange<Func<UniTask>>(ref this.taskFactory, null);
					if (func != null)
					{
						UniTask.Awaiter awaiter = func().GetAwaiter();
						if (awaiter.IsCompleted)
						{
							this.SetCompletionSource(awaiter);
						}
						else
						{
							this.awaiter = awaiter;
							awaiter.SourceOnCompleted(AsyncLazy.continuation, this);
						}
						Volatile.Write(ref this.initialized, true);
					}
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000224C File Offset: 0x0000044C
		private void SetCompletionSource(in UniTask.Awaiter awaiter)
		{
			try
			{
				awaiter.GetResult();
				this.completionSource.TrySetResult();
			}
			catch (Exception exception)
			{
				this.completionSource.TrySetException(exception);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002290 File Offset: 0x00000490
		private static void SetCompletionSource(object state)
		{
			AsyncLazy asyncLazy = (AsyncLazy)state;
			try
			{
				asyncLazy.awaiter.GetResult();
				asyncLazy.completionSource.TrySetResult();
			}
			catch (Exception exception)
			{
				asyncLazy.completionSource.TrySetException(exception);
			}
			finally
			{
				asyncLazy.awaiter = default(UniTask.Awaiter);
			}
		}

		// Token: 0x04000002 RID: 2
		private static Action<object> continuation = new Action<object>(AsyncLazy.SetCompletionSource);

		// Token: 0x04000003 RID: 3
		private Func<UniTask> taskFactory;

		// Token: 0x04000004 RID: 4
		private UniTaskCompletionSource completionSource;

		// Token: 0x04000005 RID: 5
		private UniTask.Awaiter awaiter;

		// Token: 0x04000006 RID: 6
		private object syncLock;

		// Token: 0x04000007 RID: 7
		private bool initialized;
	}
}

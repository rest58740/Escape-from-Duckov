using System;
using System.Threading;
using System.Threading.Tasks.Sources;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000020 RID: 32
	internal sealed class Create<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00008FF7 File Offset: 0x000071F7
		public Create(Func<IAsyncWriter<T>, CancellationToken, UniTask> create)
		{
			this.create = create;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00009006 File Offset: 0x00007206
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Create<T>._Create(this.create, cancellationToken);
		}

		// Token: 0x040000A0 RID: 160
		private readonly Func<IAsyncWriter<T>, CancellationToken, UniTask> create;

		// Token: 0x020000F3 RID: 243
		private sealed class _Create : MoveNextSource, IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600057C RID: 1404 RVA: 0x00025876 File Offset: 0x00023A76
			public _Create(Func<IAsyncWriter<T>, CancellationToken, UniTask> create, CancellationToken cancellationToken)
			{
				this.create = create;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x0600057D RID: 1405 RVA: 0x00025893 File Offset: 0x00023A93
			// (set) Token: 0x0600057E RID: 1406 RVA: 0x0002589B File Offset: 0x00023A9B
			public T Current { get; private set; }

			// Token: 0x0600057F RID: 1407 RVA: 0x000258A4 File Offset: 0x00023AA4
			public UniTask DisposeAsync()
			{
				this.writer.Dispose();
				return default(UniTask);
			}

			// Token: 0x06000580 RID: 1408 RVA: 0x000258C8 File Offset: 0x00023AC8
			public UniTask<bool> MoveNextAsync()
			{
				if (this.state == -2)
				{
					return default(UniTask<bool>);
				}
				this.completionSource.Reset();
				this.MoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x0002590C File Offset: 0x00023B0C
			private void MoveNext()
			{
				try
				{
					int num = this.state;
					if (num != -1)
					{
						if (num == 0)
						{
							this.writer.SignalWriter();
							return;
						}
					}
					else
					{
						this.writer = new Create<T>.AsyncWriter(this);
						this.RunWriterTask(this.create(this.writer, this.cancellationToken)).Forget();
						if (Volatile.Read(ref this.state) == -2)
						{
							return;
						}
						this.state = 0;
						return;
					}
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000582 RID: 1410 RVA: 0x000259C4 File Offset: 0x00023BC4
			private UniTaskVoid RunWriterTask(UniTask task)
			{
				Create<T>._Create.<RunWriterTask>d__12 <RunWriterTask>d__;
				<RunWriterTask>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunWriterTask>d__.<>4__this = this;
				<RunWriterTask>d__.task = task;
				<RunWriterTask>d__.<>1__state = -1;
				<RunWriterTask>d__.<>t__builder.Start<Create<T>._Create.<RunWriterTask>d__12>(ref <RunWriterTask>d__);
				return <RunWriterTask>d__.<>t__builder.Task;
			}

			// Token: 0x06000583 RID: 1411 RVA: 0x00025A0F File Offset: 0x00023C0F
			public void SetResult(T value)
			{
				this.Current = value;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x04000882 RID: 2178
			private readonly Func<IAsyncWriter<T>, CancellationToken, UniTask> create;

			// Token: 0x04000883 RID: 2179
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000884 RID: 2180
			private int state = -1;

			// Token: 0x04000885 RID: 2181
			private Create<T>.AsyncWriter writer;
		}

		// Token: 0x020000F4 RID: 244
		private sealed class AsyncWriter : IUniTaskSource, IValueTaskSource, IAsyncWriter<T>, IDisposable
		{
			// Token: 0x06000584 RID: 1412 RVA: 0x00025A25 File Offset: 0x00023C25
			public AsyncWriter(Create<T>._Create enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x00025A34 File Offset: 0x00023C34
			public void Dispose()
			{
				if (this.core.GetStatus(this.core.Version) == null)
				{
					this.core.TrySetCanceled(default(CancellationToken));
				}
			}

			// Token: 0x06000586 RID: 1414 RVA: 0x00025A6E File Offset: 0x00023C6E
			public void GetResult(short token)
			{
				this.core.GetResult(token);
			}

			// Token: 0x06000587 RID: 1415 RVA: 0x00025A7D File Offset: 0x00023C7D
			public UniTaskStatus GetStatus(short token)
			{
				return this.core.GetStatus(token);
			}

			// Token: 0x06000588 RID: 1416 RVA: 0x00025A8B File Offset: 0x00023C8B
			public UniTaskStatus UnsafeGetStatus()
			{
				return this.core.UnsafeGetStatus();
			}

			// Token: 0x06000589 RID: 1417 RVA: 0x00025A98 File Offset: 0x00023C98
			public void OnCompleted(Action<object> continuation, object state, short token)
			{
				this.core.OnCompleted(continuation, state, token);
			}

			// Token: 0x0600058A RID: 1418 RVA: 0x00025AA8 File Offset: 0x00023CA8
			public UniTask YieldAsync(T value)
			{
				this.core.Reset();
				this.enumerator.SetResult(value);
				return new UniTask(this, this.core.Version);
			}

			// Token: 0x0600058B RID: 1419 RVA: 0x00025AD2 File Offset: 0x00023CD2
			public void SignalWriter()
			{
				this.core.TrySetResult(AsyncUnit.Default);
			}

			// Token: 0x04000887 RID: 2183
			private readonly Create<T>._Create enumerator;

			// Token: 0x04000888 RID: 2184
			private UniTaskCompletionSourceCore<AsyncUnit> core;
		}
	}
}

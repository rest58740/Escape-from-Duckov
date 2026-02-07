using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000007 RID: 7
	internal sealed class AppendPrepend<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x060001EE RID: 494 RVA: 0x00007257 File Offset: 0x00005457
		public AppendPrepend(IUniTaskAsyncEnumerable<TSource> source, TSource element, bool append)
		{
			this.source = source;
			this.element = element;
			this.append = append;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007274 File Offset: 0x00005474
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new AppendPrepend<TSource>._AppendPrepend(this.source, this.element, this.append, cancellationToken);
		}

		// Token: 0x04000001 RID: 1
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000002 RID: 2
		private readonly TSource element;

		// Token: 0x04000003 RID: 3
		private readonly bool append;

		// Token: 0x020000B3 RID: 179
		private sealed class _AppendPrepend : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000435 RID: 1077 RVA: 0x00010CC6 File Offset: 0x0000EEC6
			public _AppendPrepend(IUniTaskAsyncEnumerable<TSource> source, TSource element, bool append, CancellationToken cancellationToken)
			{
				this.source = source;
				this.element = element;
				this.state = (append ? AppendPrepend<TSource>._AppendPrepend.State.RequireAppend : AppendPrepend<TSource>._AppendPrepend.State.RequirePrepend);
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000436 RID: 1078 RVA: 0x00010CF1 File Offset: 0x0000EEF1
			// (set) Token: 0x06000437 RID: 1079 RVA: 0x00010CF9 File Offset: 0x0000EEF9
			public TSource Current { get; private set; }

			// Token: 0x06000438 RID: 1080 RVA: 0x00010D04 File Offset: 0x0000EF04
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.enumerator == null)
				{
					if (this.state == AppendPrepend<TSource>._AppendPrepend.State.RequirePrepend)
					{
						this.Current = this.element;
						this.state = AppendPrepend<TSource>._AppendPrepend.State.None;
						return CompletedTasks.True;
					}
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
				}
				if (this.state == AppendPrepend<TSource>._AppendPrepend.State.Completed)
				{
					return CompletedTasks.False;
				}
				this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
				if (this.awaiter.IsCompleted)
				{
					AppendPrepend<TSource>._AppendPrepend.MoveNextCoreDelegate(this);
				}
				else
				{
					this.awaiter.SourceOnCompleted(AppendPrepend<TSource>._AppendPrepend.MoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000439 RID: 1081 RVA: 0x00010DCC File Offset: 0x0000EFCC
			private static void MoveNextCore(object state)
			{
				AppendPrepend<TSource>._AppendPrepend appendPrepend = (AppendPrepend<TSource>._AppendPrepend)state;
				bool flag;
				if (appendPrepend.TryGetResult<bool>(appendPrepend.awaiter, ref flag))
				{
					if (flag)
					{
						appendPrepend.Current = appendPrepend.enumerator.Current;
						appendPrepend.completionSource.TrySetResult(true);
						return;
					}
					if (appendPrepend.state == AppendPrepend<TSource>._AppendPrepend.State.RequireAppend)
					{
						appendPrepend.state = AppendPrepend<TSource>._AppendPrepend.State.Completed;
						appendPrepend.Current = appendPrepend.element;
						appendPrepend.completionSource.TrySetResult(true);
						return;
					}
					appendPrepend.state = AppendPrepend<TSource>._AppendPrepend.State.Completed;
					appendPrepend.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x0600043A RID: 1082 RVA: 0x00010E50 File Offset: 0x0000F050
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x0400029C RID: 668
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(AppendPrepend<TSource>._AppendPrepend.MoveNextCore);

			// Token: 0x0400029D RID: 669
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400029E RID: 670
			private readonly TSource element;

			// Token: 0x0400029F RID: 671
			private CancellationToken cancellationToken;

			// Token: 0x040002A0 RID: 672
			private AppendPrepend<TSource>._AppendPrepend.State state;

			// Token: 0x040002A1 RID: 673
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040002A2 RID: 674
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x02000204 RID: 516
			private enum State : byte
			{
				// Token: 0x04001378 RID: 4984
				None,
				// Token: 0x04001379 RID: 4985
				RequirePrepend,
				// Token: 0x0400137A RID: 4986
				RequireAppend,
				// Token: 0x0400137B RID: 4987
				Completed
			}
		}
	}
}

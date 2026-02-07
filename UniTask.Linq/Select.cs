using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000053 RID: 83
	internal sealed class Select<TSource, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000330 RID: 816 RVA: 0x0000C04D File Offset: 0x0000A24D
		public Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
		{
			this.source = source;
			this.selector = selector;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000C063 File Offset: 0x0000A263
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Select<TSource, TResult>._Select(this.source, this.selector, cancellationToken);
		}

		// Token: 0x04000131 RID: 305
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000132 RID: 306
		private readonly Func<TSource, TResult> selector;

		// Token: 0x0200018B RID: 395
		private sealed class _Select : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600075F RID: 1887 RVA: 0x0003EEC6 File Offset: 0x0003D0C6
			public _Select(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TResult> selector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector = selector;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x17000038 RID: 56
			// (get) Token: 0x06000760 RID: 1888 RVA: 0x0003EEFC File Offset: 0x0003D0FC
			// (set) Token: 0x06000761 RID: 1889 RVA: 0x0003EF04 File Offset: 0x0003D104
			public TResult Current { get; private set; }

			// Token: 0x06000762 RID: 1890 RVA: 0x0003EF10 File Offset: 0x0003D110
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

			// Token: 0x06000763 RID: 1891 RVA: 0x0003EF54 File Offset: 0x0003D154
			private void MoveNext()
			{
				try
				{
					switch (this.state)
					{
					case -1:
						this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
						break;
					case 0:
						break;
					case 1:
						goto IL_77;
					default:
						goto IL_A2;
					}
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (!this.awaiter.IsCompleted)
					{
						this.state = 1;
						this.awaiter.UnsafeOnCompleted(this.moveNextAction);
						return;
					}
					IL_77:
					if (this.awaiter.GetResult())
					{
						this.Current = this.selector(this.enumerator.Current);
						goto IL_D2;
					}
					IL_A2:;
				}
				catch (Exception ex)
				{
					this.state = -2;
					this.completionSource.TrySetException(ex);
					return;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_D2:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x06000764 RID: 1892 RVA: 0x0003F058 File Offset: 0x0003D258
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000EC1 RID: 3777
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EC2 RID: 3778
			private readonly Func<TSource, TResult> selector;

			// Token: 0x04000EC3 RID: 3779
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000EC4 RID: 3780
			private int state = -1;

			// Token: 0x04000EC5 RID: 3781
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000EC6 RID: 3782
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000EC7 RID: 3783
			private Action moveNextAction;
		}
	}
}

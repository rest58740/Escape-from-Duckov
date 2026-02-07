using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000027 RID: 39
	internal sealed class DistinctUntilChanged<TSource, TKey> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600026D RID: 621 RVA: 0x00009137 File Offset: 0x00007337
		public DistinctUntilChanged(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.comparer = comparer;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009154 File Offset: 0x00007354
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctUntilChanged<TSource, TKey>._DistinctUntilChanged(this.source, this.keySelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000B0 RID: 176
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000B1 RID: 177
		private readonly Func<TSource, TKey> keySelector;

		// Token: 0x040000B2 RID: 178
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x020000FB RID: 251
		private sealed class _DistinctUntilChanged : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005A3 RID: 1443 RVA: 0x00026015 File Offset: 0x00024215
			public _DistinctUntilChanged(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700001B RID: 27
			// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00026053 File Offset: 0x00024253
			// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0002605B File Offset: 0x0002425B
			public TSource Current { get; private set; }

			// Token: 0x060005A6 RID: 1446 RVA: 0x00026064 File Offset: 0x00024264
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

			// Token: 0x060005A7 RID: 1447 RVA: 0x000260A8 File Offset: 0x000242A8
			private void MoveNext()
			{
				for (;;)
				{
					try
					{
						switch (this.state)
						{
						case -3:
							break;
						case -2:
							goto IL_146;
						case -1:
							this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
							this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
							if (!this.awaiter.IsCompleted)
							{
								this.state = -3;
								this.awaiter.UnsafeOnCompleted(this.moveNextAction);
								return;
							}
							break;
						case 0:
							this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
							if (!this.awaiter.IsCompleted)
							{
								this.state = 1;
								this.awaiter.UnsafeOnCompleted(this.moveNextAction);
								return;
							}
							goto IL_F0;
						case 1:
							goto IL_F0;
						default:
							goto IL_146;
						}
						if (this.awaiter.GetResult())
						{
							this.Current = this.enumerator.Current;
							goto IL_178;
						}
						break;
						IL_F0:
						if (this.awaiter.GetResult())
						{
							TSource tsource = this.enumerator.Current;
							TKey y = this.keySelector(tsource);
							if (!this.comparer.Equals(this.prev, y))
							{
								this.prev = y;
								this.Current = tsource;
								goto IL_178;
							}
							this.state = 0;
							continue;
						}
						IL_146:;
					}
					catch (Exception ex)
					{
						this.state = -2;
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				this.state = -2;
				this.completionSource.TrySetResult(false);
				return;
				IL_178:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060005A8 RID: 1448 RVA: 0x00026260 File Offset: 0x00024460
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x040008A0 RID: 2208
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040008A1 RID: 2209
			private readonly Func<TSource, TKey> keySelector;

			// Token: 0x040008A2 RID: 2210
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x040008A3 RID: 2211
			private readonly CancellationToken cancellationToken;

			// Token: 0x040008A4 RID: 2212
			private int state = -1;

			// Token: 0x040008A5 RID: 2213
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040008A6 RID: 2214
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040008A7 RID: 2215
			private Action moveNextAction;

			// Token: 0x040008A8 RID: 2216
			private TKey prev;
		}
	}
}

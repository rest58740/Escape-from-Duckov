using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000026 RID: 38
	internal sealed class DistinctUntilChanged<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0000910D File Offset: 0x0000730D
		public DistinctUntilChanged(IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
		{
			this.source = source;
			this.comparer = comparer;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00009123 File Offset: 0x00007323
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DistinctUntilChanged<TSource>._DistinctUntilChanged(this.source, this.comparer, cancellationToken);
		}

		// Token: 0x040000AE RID: 174
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000AF RID: 175
		private readonly IEqualityComparer<TSource> comparer;

		// Token: 0x020000FA RID: 250
		private sealed class _DistinctUntilChanged : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600059D RID: 1437 RVA: 0x00025DDD File Offset: 0x00023FDD
			public _DistinctUntilChanged(IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
				this.moveNextAction = new Action(this.MoveNext);
			}

			// Token: 0x1700001A RID: 26
			// (get) Token: 0x0600059E RID: 1438 RVA: 0x00025E13 File Offset: 0x00024013
			// (set) Token: 0x0600059F RID: 1439 RVA: 0x00025E1B File Offset: 0x0002401B
			public TSource Current { get; private set; }

			// Token: 0x060005A0 RID: 1440 RVA: 0x00025E24 File Offset: 0x00024024
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

			// Token: 0x060005A1 RID: 1441 RVA: 0x00025E68 File Offset: 0x00024068
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
							goto IL_132;
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
							goto IL_132;
						}
						if (this.awaiter.GetResult())
						{
							this.Current = this.enumerator.Current;
							goto IL_162;
						}
						break;
						IL_F0:
						if (this.awaiter.GetResult())
						{
							TSource tsource = this.enumerator.Current;
							if (!this.comparer.Equals(this.Current, tsource))
							{
								this.Current = tsource;
								goto IL_162;
							}
							this.state = 0;
							continue;
						}
						IL_132:;
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
				IL_162:
				this.state = 0;
				this.completionSource.TrySetResult(true);
			}

			// Token: 0x060005A2 RID: 1442 RVA: 0x00026008 File Offset: 0x00024208
			public UniTask DisposeAsync()
			{
				return this.enumerator.DisposeAsync();
			}

			// Token: 0x04000898 RID: 2200
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000899 RID: 2201
			private readonly IEqualityComparer<TSource> comparer;

			// Token: 0x0400089A RID: 2202
			private readonly CancellationToken cancellationToken;

			// Token: 0x0400089B RID: 2203
			private int state = -1;

			// Token: 0x0400089C RID: 2204
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x0400089D RID: 2205
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x0400089E RID: 2206
			private Action moveNextAction;
		}
	}
}

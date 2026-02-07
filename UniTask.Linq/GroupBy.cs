using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000030 RID: 48
	internal sealed class GroupBy<TSource, TKey, TElement> : IUniTaskAsyncEnumerable<IGrouping<TKey, TElement>>
	{
		// Token: 0x06000285 RID: 645 RVA: 0x00009633 File Offset: 0x00007833
		public GroupBy(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00009658 File Offset: 0x00007858
		public IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupBy<TSource, TKey, TElement>._GroupBy(this.source, this.keySelector, this.elementSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000C1 RID: 193
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000C2 RID: 194
		private readonly Func<TSource, TKey> keySelector;

		// Token: 0x040000C3 RID: 195
		private readonly Func<TSource, TElement> elementSelector;

		// Token: 0x040000C4 RID: 196
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x0200010C RID: 268
		private sealed class _GroupBy : MoveNextSource, IUniTaskAsyncEnumerator<IGrouping<TKey, TElement>>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005DE RID: 1502 RVA: 0x0002859A File Offset: 0x0002679A
			public _GroupBy(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x060005DF RID: 1503 RVA: 0x000285C7 File Offset: 0x000267C7
			// (set) Token: 0x060005E0 RID: 1504 RVA: 0x000285CF File Offset: 0x000267CF
			public IGrouping<TKey, TElement> Current { get; private set; }

			// Token: 0x060005E1 RID: 1505 RVA: 0x000285D8 File Offset: 0x000267D8
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.groupEnumerator == null)
				{
					this.CreateLookup().Forget();
				}
				else
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060005E2 RID: 1506 RVA: 0x0002862C File Offset: 0x0002682C
			private UniTaskVoid CreateLookup()
			{
				GroupBy<TSource, TKey, TElement>._GroupBy.<CreateLookup>d__12 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupBy<TSource, TKey, TElement>._GroupBy.<CreateLookup>d__12>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x060005E3 RID: 1507 RVA: 0x00028670 File Offset: 0x00026870
			private void SourceMoveNext()
			{
				try
				{
					if (this.groupEnumerator.MoveNext())
					{
						this.Current = this.groupEnumerator.Current;
						this.completionSource.TrySetResult(true);
					}
					else
					{
						this.completionSource.TrySetResult(false);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x060005E4 RID: 1508 RVA: 0x000286DC File Offset: 0x000268DC
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x0400094E RID: 2382
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400094F RID: 2383
			private readonly Func<TSource, TKey> keySelector;

			// Token: 0x04000950 RID: 2384
			private readonly Func<TSource, TElement> elementSelector;

			// Token: 0x04000951 RID: 2385
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x04000952 RID: 2386
			private CancellationToken cancellationToken;

			// Token: 0x04000953 RID: 2387
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;
		}
	}
}

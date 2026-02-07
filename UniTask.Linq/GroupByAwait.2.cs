using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000033 RID: 51
	internal sealed class GroupByAwait<TSource, TKey, TElement, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00009710 File Offset: 0x00007910
		public GroupByAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.source = source;
			this.keySelector = keySelector;
			this.elementSelector = elementSelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000973D File Offset: 0x0000793D
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait(this.source, this.keySelector, this.elementSelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000CE RID: 206
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000CF RID: 207
		private readonly Func<TSource, UniTask<TKey>> keySelector;

		// Token: 0x040000D0 RID: 208
		private readonly Func<TSource, UniTask<TElement>> elementSelector;

		// Token: 0x040000D1 RID: 209
		private readonly Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector;

		// Token: 0x040000D2 RID: 210
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x0200010F RID: 271
		private sealed class _GroupByAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060005F3 RID: 1523 RVA: 0x000289F5 File Offset: 0x00026BF5
			public _GroupByAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.source = source;
				this.keySelector = keySelector;
				this.elementSelector = elementSelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x060005F4 RID: 1524 RVA: 0x00028A2A File Offset: 0x00026C2A
			// (set) Token: 0x060005F5 RID: 1525 RVA: 0x00028A32 File Offset: 0x00026C32
			public TResult Current { get; private set; }

			// Token: 0x060005F6 RID: 1526 RVA: 0x00028A3C File Offset: 0x00026C3C
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

			// Token: 0x060005F7 RID: 1527 RVA: 0x00028A90 File Offset: 0x00026C90
			private UniTaskVoid CreateLookup()
			{
				GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait.<CreateLookup>d__15 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait.<CreateLookup>d__15>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x060005F8 RID: 1528 RVA: 0x00028AD4 File Offset: 0x00026CD4
			private void SourceMoveNext()
			{
				try
				{
					if (this.groupEnumerator.MoveNext())
					{
						IGrouping<TKey, TElement> grouping = this.groupEnumerator.Current;
						this.awaiter = this.resultSelector(grouping.Key, grouping).GetAwaiter();
						if (this.awaiter.IsCompleted)
						{
							GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait.ResultSelectCore(this);
						}
						else
						{
							this.awaiter.SourceOnCompleted(GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait.ResultSelectCoreDelegate, this);
						}
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

			// Token: 0x060005F9 RID: 1529 RVA: 0x00028B74 File Offset: 0x00026D74
			private static void ResultSelectCore(object state)
			{
				GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait groupByAwait = (GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait)state;
				TResult value;
				if (groupByAwait.TryGetResult<TResult>(groupByAwait.awaiter, ref value))
				{
					groupByAwait.Current = value;
					groupByAwait.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x060005FA RID: 1530 RVA: 0x00028BAC File Offset: 0x00026DAC
			public UniTask DisposeAsync()
			{
				if (this.groupEnumerator != null)
				{
					this.groupEnumerator.Dispose();
				}
				return default(UniTask);
			}

			// Token: 0x04000964 RID: 2404
			private static readonly Action<object> ResultSelectCoreDelegate = new Action<object>(GroupByAwait<TSource, TKey, TElement, TResult>._GroupByAwait.ResultSelectCore);

			// Token: 0x04000965 RID: 2405
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000966 RID: 2406
			private readonly Func<TSource, UniTask<TKey>> keySelector;

			// Token: 0x04000967 RID: 2407
			private readonly Func<TSource, UniTask<TElement>> elementSelector;

			// Token: 0x04000968 RID: 2408
			private readonly Func<TKey, IEnumerable<TElement>, UniTask<TResult>> resultSelector;

			// Token: 0x04000969 RID: 2409
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x0400096A RID: 2410
			private CancellationToken cancellationToken;

			// Token: 0x0400096B RID: 2411
			private IEnumerator<IGrouping<TKey, TElement>> groupEnumerator;

			// Token: 0x0400096C RID: 2412
			private UniTask<TResult>.Awaiter awaiter;
		}
	}
}

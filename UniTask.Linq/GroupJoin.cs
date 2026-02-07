using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000036 RID: 54
	internal sealed class GroupJoin<TOuter, TInner, TKey, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000291 RID: 657 RVA: 0x000097FB File Offset: 0x000079FB
		public GroupJoin(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
		{
			this.outer = outer;
			this.inner = inner;
			this.outerKeySelector = outerKeySelector;
			this.innerKeySelector = innerKeySelector;
			this.resultSelector = resultSelector;
			this.comparer = comparer;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00009830 File Offset: 0x00007A30
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin(this.outer, this.inner, this.outerKeySelector, this.innerKeySelector, this.resultSelector, this.comparer, cancellationToken);
		}

		// Token: 0x040000DC RID: 220
		private readonly IUniTaskAsyncEnumerable<TOuter> outer;

		// Token: 0x040000DD RID: 221
		private readonly IUniTaskAsyncEnumerable<TInner> inner;

		// Token: 0x040000DE RID: 222
		private readonly Func<TOuter, TKey> outerKeySelector;

		// Token: 0x040000DF RID: 223
		private readonly Func<TInner, TKey> innerKeySelector;

		// Token: 0x040000E0 RID: 224
		private readonly Func<TOuter, IEnumerable<TInner>, TResult> resultSelector;

		// Token: 0x040000E1 RID: 225
		private readonly IEqualityComparer<TKey> comparer;

		// Token: 0x02000112 RID: 274
		private sealed class _GroupJoin : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600060C RID: 1548 RVA: 0x00028F4C File Offset: 0x0002714C
			public _GroupJoin(IUniTaskAsyncEnumerable<TOuter> outer, IUniTaskAsyncEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
			{
				this.outer = outer;
				this.inner = inner;
				this.outerKeySelector = outerKeySelector;
				this.innerKeySelector = innerKeySelector;
				this.resultSelector = resultSelector;
				this.comparer = comparer;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x0600060D RID: 1549 RVA: 0x00028F89 File Offset: 0x00027189
			// (set) Token: 0x0600060E RID: 1550 RVA: 0x00028F91 File Offset: 0x00027191
			public TResult Current { get; private set; }

			// Token: 0x0600060F RID: 1551 RVA: 0x00028F9C File Offset: 0x0002719C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.lookup == null)
				{
					this.CreateLookup().Forget();
				}
				else
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000610 RID: 1552 RVA: 0x00028FF0 File Offset: 0x000271F0
			private UniTaskVoid CreateLookup()
			{
				GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin.<CreateLookup>d__17 <CreateLookup>d__;
				<CreateLookup>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<CreateLookup>d__.<>4__this = this;
				<CreateLookup>d__.<>1__state = -1;
				<CreateLookup>d__.<>t__builder.Start<GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin.<CreateLookup>d__17>(ref <CreateLookup>d__);
				return <CreateLookup>d__.<>t__builder.Task;
			}

			// Token: 0x06000611 RID: 1553 RVA: 0x00029034 File Offset: 0x00027234
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000612 RID: 1554 RVA: 0x000290A4 File Offset: 0x000272A4
			private static void MoveNextCore(object state)
			{
				GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin groupJoin = (GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin)state;
				bool flag;
				if (groupJoin.TryGetResult<bool>(groupJoin.awaiter, ref flag))
				{
					if (flag)
					{
						TOuter touter = groupJoin.enumerator.Current;
						TKey key = groupJoin.outerKeySelector(touter);
						IEnumerable<TInner> arg = groupJoin.lookup[key];
						groupJoin.Current = groupJoin.resultSelector(touter, arg);
						groupJoin.completionSource.TrySetResult(true);
						return;
					}
					groupJoin.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000613 RID: 1555 RVA: 0x00029124 File Offset: 0x00027324
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x0400097F RID: 2431
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(GroupJoin<TOuter, TInner, TKey, TResult>._GroupJoin.MoveNextCore);

			// Token: 0x04000980 RID: 2432
			private readonly IUniTaskAsyncEnumerable<TOuter> outer;

			// Token: 0x04000981 RID: 2433
			private readonly IUniTaskAsyncEnumerable<TInner> inner;

			// Token: 0x04000982 RID: 2434
			private readonly Func<TOuter, TKey> outerKeySelector;

			// Token: 0x04000983 RID: 2435
			private readonly Func<TInner, TKey> innerKeySelector;

			// Token: 0x04000984 RID: 2436
			private readonly Func<TOuter, IEnumerable<TInner>, TResult> resultSelector;

			// Token: 0x04000985 RID: 2437
			private readonly IEqualityComparer<TKey> comparer;

			// Token: 0x04000986 RID: 2438
			private CancellationToken cancellationToken;

			// Token: 0x04000987 RID: 2439
			private ILookup<TKey, TInner> lookup;

			// Token: 0x04000988 RID: 2440
			private IUniTaskAsyncEnumerator<TOuter> enumerator;

			// Token: 0x04000989 RID: 2441
			private UniTask<bool>.Awaiter awaiter;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200002D RID: 45
	internal sealed class Except<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000279 RID: 633 RVA: 0x0000929A File Offset: 0x0000749A
		public Except(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			this.first = first;
			this.second = second;
			this.comparer = comparer;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000092B7 File Offset: 0x000074B7
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Except<TSource>._Except(this.first, this.second, this.comparer, cancellationToken);
		}

		// Token: 0x040000BE RID: 190
		private readonly IUniTaskAsyncEnumerable<TSource> first;

		// Token: 0x040000BF RID: 191
		private readonly IUniTaskAsyncEnumerable<TSource> second;

		// Token: 0x040000C0 RID: 192
		private readonly IEqualityComparer<TSource> comparer;

		// Token: 0x02000101 RID: 257
		private class _Except : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x060005C5 RID: 1477 RVA: 0x00026D0A File Offset: 0x00024F0A
			public _Except(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken) : base(first, cancellationToken)
			{
				this.second = second;
				this.comparer = comparer;
			}

			// Token: 0x060005C6 RID: 1478 RVA: 0x00026D24 File Offset: 0x00024F24
			protected override bool OnFirstIteration()
			{
				if (this.set != null)
				{
					return false;
				}
				this.awaiter = this.second.ToHashSetAsync(this.cancellationToken).GetAwaiter();
				if (this.awaiter.IsCompleted)
				{
					this.set = this.awaiter.GetResult();
					base.SourceMoveNext();
				}
				else
				{
					this.awaiter.SourceOnCompleted(Except<TSource>._Except.HashSetAsyncCoreDelegate, this);
				}
				return true;
			}

			// Token: 0x060005C7 RID: 1479 RVA: 0x00026D94 File Offset: 0x00024F94
			private static void HashSetAsyncCore(object state)
			{
				Except<TSource>._Except except = (Except<TSource>._Except)state;
				HashSet<TSource> hashSet;
				if (except.TryGetResult<HashSet<TSource>>(except.awaiter, ref hashSet))
				{
					except.set = hashSet;
					except.SourceMoveNext();
				}
			}

			// Token: 0x060005C8 RID: 1480 RVA: 0x00026DC8 File Offset: 0x00024FC8
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				TSource sourceCurrent = base.SourceCurrent;
				if (this.set.Add(sourceCurrent))
				{
					base.Current = sourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x040008D9 RID: 2265
			private static Action<object> HashSetAsyncCoreDelegate = new Action<object>(Except<TSource>._Except.HashSetAsyncCore);

			// Token: 0x040008DA RID: 2266
			private readonly IEqualityComparer<TSource> comparer;

			// Token: 0x040008DB RID: 2267
			private readonly IUniTaskAsyncEnumerable<TSource> second;

			// Token: 0x040008DC RID: 2268
			private HashSet<TSource> set;

			// Token: 0x040008DD RID: 2269
			private UniTask<HashSet<TSource>>.Awaiter awaiter;
		}
	}
}

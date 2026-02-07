using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000039 RID: 57
	internal sealed class Intersect<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000297 RID: 663 RVA: 0x0000991E File Offset: 0x00007B1E
		public Intersect(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			this.first = first;
			this.second = second;
			this.comparer = comparer;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000993B File Offset: 0x00007B3B
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Intersect<TSource>._Intersect(this.first, this.second, this.comparer, cancellationToken);
		}

		// Token: 0x040000EE RID: 238
		private readonly IUniTaskAsyncEnumerable<TSource> first;

		// Token: 0x040000EF RID: 239
		private readonly IUniTaskAsyncEnumerable<TSource> second;

		// Token: 0x040000F0 RID: 240
		private readonly IEqualityComparer<TSource> comparer;

		// Token: 0x02000115 RID: 277
		private class _Intersect : AsyncEnumeratorBase<TSource, TSource>
		{
			// Token: 0x0600062B RID: 1579 RVA: 0x000297DF File Offset: 0x000279DF
			public _Intersect(IUniTaskAsyncEnumerable<TSource> first, IUniTaskAsyncEnumerable<TSource> second, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken) : base(first, cancellationToken)
			{
				this.second = second;
				this.comparer = comparer;
			}

			// Token: 0x0600062C RID: 1580 RVA: 0x000297F8 File Offset: 0x000279F8
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
					this.awaiter.SourceOnCompleted(Intersect<TSource>._Intersect.HashSetAsyncCoreDelegate, this);
				}
				return true;
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x00029868 File Offset: 0x00027A68
			private static void HashSetAsyncCore(object state)
			{
				Intersect<TSource>._Intersect intersect = (Intersect<TSource>._Intersect)state;
				HashSet<TSource> hashSet;
				if (intersect.TryGetResult<HashSet<TSource>>(intersect.awaiter, ref hashSet))
				{
					intersect.set = hashSet;
					intersect.SourceMoveNext();
				}
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x0002989C File Offset: 0x00027A9C
			protected override bool TryMoveNextCore(bool sourceHasCurrent, out bool result)
			{
				if (!sourceHasCurrent)
				{
					result = false;
					return true;
				}
				TSource sourceCurrent = base.SourceCurrent;
				if (this.set.Remove(sourceCurrent))
				{
					base.Current = sourceCurrent;
					result = true;
					return true;
				}
				result = false;
				return false;
			}

			// Token: 0x040009AD RID: 2477
			private static Action<object> HashSetAsyncCoreDelegate = new Action<object>(Intersect<TSource>._Intersect.HashSetAsyncCore);

			// Token: 0x040009AE RID: 2478
			private readonly IEqualityComparer<TSource> comparer;

			// Token: 0x040009AF RID: 2479
			private readonly IUniTaskAsyncEnumerable<TSource> second;

			// Token: 0x040009B0 RID: 2480
			private HashSet<TSource> set;

			// Token: 0x040009B1 RID: 2481
			private UniTask<HashSet<TSource>>.Awaiter awaiter;
		}
	}
}

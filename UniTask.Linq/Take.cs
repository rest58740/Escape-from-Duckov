using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200006B RID: 107
	internal sealed class Take<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000390 RID: 912 RVA: 0x0000D593 File Offset: 0x0000B793
		public Take(IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			this.source = source;
			this.count = count;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000D5A9 File Offset: 0x0000B7A9
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Take<TSource>._Take(this.source, this.count, cancellationToken);
		}

		// Token: 0x04000161 RID: 353
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000162 RID: 354
		private readonly int count;

		// Token: 0x020001D2 RID: 466
		private sealed class _Take : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000840 RID: 2112 RVA: 0x000490B6 File Offset: 0x000472B6
			public _Take(IUniTaskAsyncEnumerable<TSource> source, int count, CancellationToken cancellationToken)
			{
				this.source = source;
				this.count = count;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000044 RID: 68
			// (get) Token: 0x06000841 RID: 2113 RVA: 0x000490D3 File Offset: 0x000472D3
			// (set) Token: 0x06000842 RID: 2114 RVA: 0x000490DB File Offset: 0x000472DB
			public TSource Current { get; private set; }

			// Token: 0x06000843 RID: 2115 RVA: 0x000490E4 File Offset: 0x000472E4
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
				}
				if (this.index >= this.count)
				{
					return CompletedTasks.False;
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000844 RID: 2116 RVA: 0x00049154 File Offset: 0x00047354
			private void SourceMoveNext()
			{
				try
				{
					this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
					if (this.awaiter.IsCompleted)
					{
						Take<TSource>._Take.MoveNextCore(this);
					}
					else
					{
						this.awaiter.SourceOnCompleted(Take<TSource>._Take.MoveNextCoreDelegate, this);
					}
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x06000845 RID: 2117 RVA: 0x000491C4 File Offset: 0x000473C4
			private static void MoveNextCore(object state)
			{
				Take<TSource>._Take take = (Take<TSource>._Take)state;
				bool flag;
				if (take.TryGetResult<bool>(take.awaiter, ref flag))
				{
					if (flag)
					{
						take.index++;
						take.Current = take.enumerator.Current;
						take.completionSource.TrySetResult(true);
						return;
					}
					take.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000846 RID: 2118 RVA: 0x00049228 File Offset: 0x00047428
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040011C1 RID: 4545
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Take<TSource>._Take.MoveNextCore);

			// Token: 0x040011C2 RID: 4546
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040011C3 RID: 4547
			private readonly int count;

			// Token: 0x040011C4 RID: 4548
			private CancellationToken cancellationToken;

			// Token: 0x040011C5 RID: 4549
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040011C6 RID: 4550
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040011C7 RID: 4551
			private int index;
		}
	}
}

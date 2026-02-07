using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000021 RID: 33
	internal sealed class DefaultIfEmpty<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000261 RID: 609 RVA: 0x00009014 File Offset: 0x00007214
		public DefaultIfEmpty(IUniTaskAsyncEnumerable<TSource> source, TSource defaultValue)
		{
			this.source = source;
			this.defaultValue = defaultValue;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000902A File Offset: 0x0000722A
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new DefaultIfEmpty<TSource>._DefaultIfEmpty(this.source, this.defaultValue, cancellationToken);
		}

		// Token: 0x040000A1 RID: 161
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x040000A2 RID: 162
		private readonly TSource defaultValue;

		// Token: 0x020000F5 RID: 245
		private sealed class _DefaultIfEmpty : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600058C RID: 1420 RVA: 0x00025AE5 File Offset: 0x00023CE5
			public _DefaultIfEmpty(IUniTaskAsyncEnumerable<TSource> source, TSource defaultValue, CancellationToken cancellationToken)
			{
				this.source = source;
				this.defaultValue = defaultValue;
				this.cancellationToken = cancellationToken;
				this.iteratingState = DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState.Empty;
			}

			// Token: 0x17000019 RID: 25
			// (get) Token: 0x0600058D RID: 1421 RVA: 0x00025B09 File Offset: 0x00023D09
			// (set) Token: 0x0600058E RID: 1422 RVA: 0x00025B11 File Offset: 0x00023D11
			public TSource Current { get; private set; }

			// Token: 0x0600058F RID: 1423 RVA: 0x00025B1C File Offset: 0x00023D1C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				if (this.iteratingState == DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState.Completed)
				{
					return CompletedTasks.False;
				}
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
				}
				this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
				if (this.awaiter.IsCompleted)
				{
					DefaultIfEmpty<TSource>._DefaultIfEmpty.MoveNextCore(this);
				}
				else
				{
					this.awaiter.SourceOnCompleted(DefaultIfEmpty<TSource>._DefaultIfEmpty.MoveNextCoreDelegate, this);
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000590 RID: 1424 RVA: 0x00025BC0 File Offset: 0x00023DC0
			private static void MoveNextCore(object state)
			{
				DefaultIfEmpty<TSource>._DefaultIfEmpty defaultIfEmpty = (DefaultIfEmpty<TSource>._DefaultIfEmpty)state;
				bool flag;
				if (defaultIfEmpty.TryGetResult<bool>(defaultIfEmpty.awaiter, ref flag))
				{
					if (flag)
					{
						defaultIfEmpty.iteratingState = DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState.Iterating;
						defaultIfEmpty.Current = defaultIfEmpty.enumerator.Current;
						defaultIfEmpty.completionSource.TrySetResult(true);
						return;
					}
					if (defaultIfEmpty.iteratingState == DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState.Empty)
					{
						defaultIfEmpty.iteratingState = DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState.Completed;
						defaultIfEmpty.Current = defaultIfEmpty.defaultValue;
						defaultIfEmpty.completionSource.TrySetResult(true);
						return;
					}
					defaultIfEmpty.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000591 RID: 1425 RVA: 0x00025C44 File Offset: 0x00023E44
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000889 RID: 2185
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(DefaultIfEmpty<TSource>._DefaultIfEmpty.MoveNextCore);

			// Token: 0x0400088A RID: 2186
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400088B RID: 2187
			private readonly TSource defaultValue;

			// Token: 0x0400088C RID: 2188
			private CancellationToken cancellationToken;

			// Token: 0x0400088D RID: 2189
			private DefaultIfEmpty<TSource>._DefaultIfEmpty.IteratingState iteratingState;

			// Token: 0x0400088E RID: 2190
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x0400088F RID: 2191
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x02000216 RID: 534
			private enum IteratingState : byte
			{
				// Token: 0x040013C2 RID: 5058
				Empty,
				// Token: 0x040013C3 RID: 5059
				Iterating,
				// Token: 0x040013C4 RID: 5060
				Completed
			}
		}
	}
}

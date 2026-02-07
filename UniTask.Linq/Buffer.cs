using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000B RID: 11
	internal sealed class Buffer<TSource> : IUniTaskAsyncEnumerable<IList<TSource>>
	{
		// Token: 0x06000235 RID: 565 RVA: 0x000084D7 File Offset: 0x000066D7
		public Buffer(IUniTaskAsyncEnumerable<TSource> source, int count)
		{
			this.source = source;
			this.count = count;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x000084ED File Offset: 0x000066ED
		public IUniTaskAsyncEnumerator<IList<TSource>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Buffer<TSource>._Buffer(this.source, this.count, cancellationToken);
		}

		// Token: 0x04000013 RID: 19
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000014 RID: 20
		private readonly int count;

		// Token: 0x020000DC RID: 220
		private sealed class _Buffer : MoveNextSource, IUniTaskAsyncEnumerator<IList<TSource>>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600048C RID: 1164 RVA: 0x000176A6 File Offset: 0x000158A6
			public _Buffer(IUniTaskAsyncEnumerable<TSource> source, int count, CancellationToken cancellationToken)
			{
				this.source = source;
				this.count = count;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600048D RID: 1165 RVA: 0x000176C3 File Offset: 0x000158C3
			// (set) Token: 0x0600048E RID: 1166 RVA: 0x000176CB File Offset: 0x000158CB
			public IList<TSource> Current { get; private set; }

			// Token: 0x0600048F RID: 1167 RVA: 0x000176D4 File Offset: 0x000158D4
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					this.buffer = new List<TSource>(this.count);
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000490 RID: 1168 RVA: 0x00017740 File Offset: 0x00015940
			private void SourceMoveNext()
			{
				if (!this.completed)
				{
					try
					{
						for (;;)
						{
							this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
							if (!this.awaiter.IsCompleted)
							{
								break;
							}
							this.continueNext = true;
							Buffer<TSource>._Buffer.MoveNextCore(this);
							if (!this.continueNext)
							{
								goto IL_A5;
							}
							this.continueNext = false;
						}
						this.awaiter.SourceOnCompleted(Buffer<TSource>._Buffer.MoveNextCoreDelegate, this);
						IL_A5:;
					}
					catch (Exception ex)
					{
						this.completionSource.TrySetException(ex);
					}
					return;
				}
				if (this.buffer != null && this.buffer.Count > 0)
				{
					List<TSource> value = this.buffer;
					this.buffer = null;
					this.Current = value;
					this.completionSource.TrySetResult(true);
					return;
				}
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000491 RID: 1169 RVA: 0x00017814 File Offset: 0x00015A14
			private static void MoveNextCore(object state)
			{
				Buffer<TSource>._Buffer buffer = (Buffer<TSource>._Buffer)state;
				bool flag;
				if (buffer.TryGetResult<bool>(buffer.awaiter, ref flag))
				{
					if (!flag)
					{
						buffer.continueNext = false;
						buffer.completed = true;
						buffer.SourceMoveNext();
						return;
					}
					buffer.buffer.Add(buffer.enumerator.Current);
					if (buffer.buffer.Count == buffer.count)
					{
						buffer.Current = buffer.buffer;
						buffer.buffer = new List<TSource>(buffer.count);
						buffer.continueNext = false;
						buffer.completionSource.TrySetResult(true);
						return;
					}
					if (!buffer.continueNext)
					{
						buffer.SourceMoveNext();
						return;
					}
				}
				else
				{
					buffer.continueNext = false;
				}
			}

			// Token: 0x06000492 RID: 1170 RVA: 0x000178C4 File Offset: 0x00015AC4
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000498 RID: 1176
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(Buffer<TSource>._Buffer.MoveNextCore);

			// Token: 0x04000499 RID: 1177
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x0400049A RID: 1178
			private readonly int count;

			// Token: 0x0400049B RID: 1179
			private CancellationToken cancellationToken;

			// Token: 0x0400049C RID: 1180
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x0400049D RID: 1181
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x0400049E RID: 1182
			private bool continueNext;

			// Token: 0x0400049F RID: 1183
			private bool completed;

			// Token: 0x040004A0 RID: 1184
			private List<TSource> buffer;
		}
	}
}

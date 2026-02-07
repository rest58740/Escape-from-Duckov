using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000C RID: 12
	internal sealed class BufferSkip<TSource> : IUniTaskAsyncEnumerable<IList<TSource>>
	{
		// Token: 0x06000237 RID: 567 RVA: 0x00008501 File Offset: 0x00006701
		public BufferSkip(IUniTaskAsyncEnumerable<TSource> source, int count, int skip)
		{
			this.source = source;
			this.count = count;
			this.skip = skip;
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000851E File Offset: 0x0000671E
		public IUniTaskAsyncEnumerator<IList<TSource>> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new BufferSkip<TSource>._BufferSkip(this.source, this.count, this.skip, cancellationToken);
		}

		// Token: 0x04000015 RID: 21
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000016 RID: 22
		private readonly int count;

		// Token: 0x04000017 RID: 23
		private readonly int skip;

		// Token: 0x020000DD RID: 221
		private sealed class _BufferSkip : MoveNextSource, IUniTaskAsyncEnumerator<IList<TSource>>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000494 RID: 1172 RVA: 0x00017901 File Offset: 0x00015B01
			public _BufferSkip(IUniTaskAsyncEnumerable<TSource> source, int count, int skip, CancellationToken cancellationToken)
			{
				this.source = source;
				this.count = count;
				this.skip = skip;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000495 RID: 1173 RVA: 0x00017926 File Offset: 0x00015B26
			// (set) Token: 0x06000496 RID: 1174 RVA: 0x0001792E File Offset: 0x00015B2E
			public IList<TSource> Current { get; private set; }

			// Token: 0x06000497 RID: 1175 RVA: 0x00017938 File Offset: 0x00015B38
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					this.buffers = new Queue<List<TSource>>();
				}
				this.completionSource.Reset();
				this.SourceMoveNext();
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000498 RID: 1176 RVA: 0x0001799C File Offset: 0x00015B9C
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
							BufferSkip<TSource>._BufferSkip.MoveNextCore(this);
							if (!this.continueNext)
							{
								goto IL_99;
							}
							this.continueNext = false;
						}
						this.awaiter.SourceOnCompleted(BufferSkip<TSource>._BufferSkip.MoveNextCoreDelegate, this);
						IL_99:;
					}
					catch (Exception ex)
					{
						this.completionSource.TrySetException(ex);
					}
					return;
				}
				if (this.buffers.Count > 0)
				{
					this.Current = this.buffers.Dequeue();
					this.completionSource.TrySetResult(true);
					return;
				}
				this.completionSource.TrySetResult(false);
			}

			// Token: 0x06000499 RID: 1177 RVA: 0x00017A64 File Offset: 0x00015C64
			private static void MoveNextCore(object state)
			{
				BufferSkip<TSource>._BufferSkip bufferSkip = (BufferSkip<TSource>._BufferSkip)state;
				bool flag;
				if (bufferSkip.TryGetResult<bool>(bufferSkip.awaiter, ref flag))
				{
					if (!flag)
					{
						bufferSkip.continueNext = false;
						bufferSkip.completed = true;
						bufferSkip.SourceMoveNext();
						return;
					}
					BufferSkip<TSource>._BufferSkip bufferSkip2 = bufferSkip;
					int num = bufferSkip2.index;
					bufferSkip2.index = num + 1;
					if (num % bufferSkip.skip == 0)
					{
						bufferSkip.buffers.Enqueue(new List<TSource>(bufferSkip.count));
					}
					TSource item = bufferSkip.enumerator.Current;
					foreach (List<TSource> list in bufferSkip.buffers)
					{
						list.Add(item);
					}
					if (bufferSkip.buffers.Count > 0 && bufferSkip.buffers.Peek().Count == bufferSkip.count)
					{
						bufferSkip.Current = bufferSkip.buffers.Dequeue();
						bufferSkip.continueNext = false;
						bufferSkip.completionSource.TrySetResult(true);
						return;
					}
					if (!bufferSkip.continueNext)
					{
						bufferSkip.SourceMoveNext();
						return;
					}
				}
				else
				{
					bufferSkip.continueNext = false;
				}
			}

			// Token: 0x0600049A RID: 1178 RVA: 0x00017B8C File Offset: 0x00015D8C
			public UniTask DisposeAsync()
			{
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x040004A2 RID: 1186
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(BufferSkip<TSource>._BufferSkip.MoveNextCore);

			// Token: 0x040004A3 RID: 1187
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x040004A4 RID: 1188
			private readonly int count;

			// Token: 0x040004A5 RID: 1189
			private readonly int skip;

			// Token: 0x040004A6 RID: 1190
			private CancellationToken cancellationToken;

			// Token: 0x040004A7 RID: 1191
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x040004A8 RID: 1192
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x040004A9 RID: 1193
			private bool continueNext;

			// Token: 0x040004AA RID: 1194
			private bool completed;

			// Token: 0x040004AB RID: 1195
			private Queue<List<TSource>> buffers;

			// Token: 0x040004AC RID: 1196
			private int index;
		}
	}
}

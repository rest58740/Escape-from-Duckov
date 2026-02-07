using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000040 RID: 64
	internal sealed class Merge<T> : IUniTaskAsyncEnumerable<T>
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x0000AB3F File Offset: 0x00008D3F
		public Merge(IUniTaskAsyncEnumerable<T>[] sources)
		{
			if (sources.Length == 0)
			{
				Error.ThrowArgumentException("No source async enumerable to merge");
			}
			this.sources = sources;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000AB5C File Offset: 0x00008D5C
		public IUniTaskAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new Merge<T>._Merge(this.sources, cancellationToken);
		}

		// Token: 0x04000103 RID: 259
		private readonly IUniTaskAsyncEnumerable<T>[] sources;

		// Token: 0x0200014D RID: 333
		private enum MergeSourceState
		{
			// Token: 0x04000C56 RID: 3158
			Pending,
			// Token: 0x04000C57 RID: 3159
			Running,
			// Token: 0x04000C58 RID: 3160
			Completed
		}

		// Token: 0x0200014E RID: 334
		private sealed class _Merge : MoveNextSource, IUniTaskAsyncEnumerator<T>, IUniTaskAsyncDisposable
		{
			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0003481A File Offset: 0x00032A1A
			// (set) Token: 0x060006B8 RID: 1720 RVA: 0x00034822 File Offset: 0x00032A22
			public T Current { get; private set; }

			// Token: 0x060006B9 RID: 1721 RVA: 0x0003482C File Offset: 0x00032A2C
			public _Merge(IUniTaskAsyncEnumerable<T>[] sources, CancellationToken cancellationToken)
			{
				this.cancellationToken = cancellationToken;
				this.length = sources.Length;
				this.states = ArrayPool<Merge<T>.MergeSourceState>.Shared.Rent(this.length);
				this.enumerators = ArrayPool<IUniTaskAsyncEnumerator<T>>.Shared.Rent(this.length);
				for (int i = 0; i < this.length; i++)
				{
					this.enumerators[i] = sources[i].GetAsyncEnumerator(cancellationToken);
					this.states[i] = Merge<T>.MergeSourceState.Pending;
				}
			}

			// Token: 0x060006BA RID: 1722 RVA: 0x000348B4 File Offset: 0x00032AB4
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				this.completionSource.Reset();
				Interlocked.Exchange(ref this.moveNextCompleted, 0);
				if (this.HasQueuedResult() && Interlocked.CompareExchange(ref this.moveNextCompleted, 1, 0) == 0)
				{
					Merge<T>.MergeSourceState[] obj = this.states;
					ValueTuple<T, Exception, bool> valueTuple;
					lock (obj)
					{
						valueTuple = this.queuedResult.Dequeue();
					}
					T item = valueTuple.Item1;
					Exception item2 = valueTuple.Item2;
					bool item3 = valueTuple.Item3;
					if (item2 != null)
					{
						this.completionSource.TrySetException(item2);
					}
					else
					{
						this.Current = item;
						this.completionSource.TrySetResult(item3);
					}
					return new UniTask<bool>(this, this.completionSource.Version);
				}
				int i = 0;
				while (i < this.length)
				{
					Merge<T>.MergeSourceState[] obj = this.states;
					lock (obj)
					{
						if (this.states[i] != Merge<T>.MergeSourceState.Pending)
						{
							goto IL_142;
						}
						this.states[i] = Merge<T>.MergeSourceState.Running;
					}
					goto IL_FE;
					IL_142:
					i++;
					continue;
					IL_FE:
					UniTask<bool>.Awaiter awaiter = this.enumerators[i].MoveNextAsync().GetAwaiter();
					if (awaiter.IsCompleted)
					{
						this.GetResultAt(i, awaiter);
						goto IL_142;
					}
					awaiter.SourceOnCompleted(Merge<T>._Merge.GetResultAtAction, StateTuple.Create<Merge<T>._Merge, int, UniTask<bool>.Awaiter>(this, i, awaiter));
					goto IL_142;
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060006BB RID: 1723 RVA: 0x00034A44 File Offset: 0x00032C44
			public UniTask DisposeAsync()
			{
				Merge<T>._Merge.<DisposeAsync>d__13 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<Merge<T>._Merge.<DisposeAsync>d__13>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x060006BC RID: 1724 RVA: 0x00034A88 File Offset: 0x00032C88
			private static void GetResultAt(object state)
			{
				using (StateTuple<Merge<T>._Merge, int, UniTask<bool>.Awaiter> stateTuple = (StateTuple<Merge<T>._Merge, int, UniTask<bool>.Awaiter>)state)
				{
					stateTuple.Item1.GetResultAt(stateTuple.Item2, stateTuple.Item3);
				}
			}

			// Token: 0x060006BD RID: 1725 RVA: 0x00034AD0 File Offset: 0x00032CD0
			private void GetResultAt(int index, UniTask<bool>.Awaiter awaiter)
			{
				bool result;
				Merge<T>.MergeSourceState[] obj;
				try
				{
					result = awaiter.GetResult();
				}
				catch (Exception ex)
				{
					if (Interlocked.CompareExchange(ref this.moveNextCompleted, 1, 0) == 0)
					{
						this.completionSource.TrySetException(ex);
					}
					else
					{
						obj = this.states;
						lock (obj)
						{
							this.queuedResult.Enqueue(new ValueTuple<T, Exception, bool>(default(T), ex, false));
						}
					}
					return;
				}
				obj = this.states;
				bool flag2;
				lock (obj)
				{
					this.states[index] = (result ? Merge<T>.MergeSourceState.Pending : Merge<T>.MergeSourceState.Completed);
					flag2 = (!result && this.IsCompletedAll());
				}
				if (result || flag2)
				{
					if (Interlocked.CompareExchange(ref this.moveNextCompleted, 1, 0) == 0)
					{
						this.Current = this.enumerators[index].Current;
						this.completionSource.TrySetResult(!flag2);
						return;
					}
					obj = this.states;
					lock (obj)
					{
						this.queuedResult.Enqueue(new ValueTuple<T, Exception, bool>(this.enumerators[index].Current, null, !flag2));
					}
				}
			}

			// Token: 0x060006BE RID: 1726 RVA: 0x00034C30 File Offset: 0x00032E30
			private bool HasQueuedResult()
			{
				Merge<T>.MergeSourceState[] obj = this.states;
				bool result;
				lock (obj)
				{
					result = (this.queuedResult.Count > 0);
				}
				return result;
			}

			// Token: 0x060006BF RID: 1727 RVA: 0x00034C7C File Offset: 0x00032E7C
			private bool IsCompletedAll()
			{
				Merge<T>.MergeSourceState[] obj = this.states;
				lock (obj)
				{
					for (int i = 0; i < this.length; i++)
					{
						if (this.states[i] != Merge<T>.MergeSourceState.Completed)
						{
							return false;
						}
					}
				}
				return true;
			}

			// Token: 0x04000C59 RID: 3161
			private static readonly Action<object> GetResultAtAction = new Action<object>(Merge<T>._Merge.GetResultAt);

			// Token: 0x04000C5A RID: 3162
			private readonly int length;

			// Token: 0x04000C5B RID: 3163
			private readonly IUniTaskAsyncEnumerator<T>[] enumerators;

			// Token: 0x04000C5C RID: 3164
			private readonly Merge<T>.MergeSourceState[] states;

			// Token: 0x04000C5D RID: 3165
			private readonly Queue<ValueTuple<T, Exception, bool>> queuedResult = new Queue<ValueTuple<T, Exception, bool>>();

			// Token: 0x04000C5E RID: 3166
			private readonly CancellationToken cancellationToken;

			// Token: 0x04000C5F RID: 3167
			private int moveNextCompleted;
		}
	}
}

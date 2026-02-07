using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000E RID: 14
	internal class CombineLatest<T1, T2, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600023B RID: 571 RVA: 0x00008555 File Offset: 0x00006755
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, Func<T1, T2, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008572 File Offset: 0x00006772
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, TResult>._CombineLatest(this.source1, this.source2, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000019 RID: 25
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400001A RID: 26
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400001B RID: 27
		private readonly Func<T1, T2, TResult> resultSelector;

		// Token: 0x020000DF RID: 223
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600049E RID: 1182 RVA: 0x00017BF2 File Offset: 0x00015DF2
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, Func<T1, T2, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x0600049F RID: 1183 RVA: 0x00017C17 File Offset: 0x00015E17
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x00017C20 File Offset: 0x00015E20
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 2)
				{
					return CompletedTasks.False;
				}
				if (this.enumerator1 == null)
				{
					this.enumerator1 = this.source1.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator2 = this.source2.GetAsyncEnumerator(this.cancellationToken);
				}
				this.completionSource.Reset();
				do
				{
					this.syncRunning = true;
					if (!this.running1)
					{
						this.running1 = true;
						this.awaiter1 = this.enumerator1.MoveNextAsync().GetAwaiter();
						if (this.awaiter1.IsCompleted)
						{
							CombineLatest<T1, T2, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x00017D5C File Offset: 0x00015F5C
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, TResult>._CombineLatest)state;
				combineLatest.running1 = false;
				try
				{
					if (combineLatest.awaiter1.GetResult())
					{
						combineLatest.hasCurrent1 = true;
						combineLatest.current1 = combineLatest.enumerator1.Current;
					}
					else
					{
						combineLatest.running1 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 2)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 2;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running1 = true;
					try
					{
						combineLatest.awaiter1 = combineLatest.enumerator1.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 2;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x00017E60 File Offset: 0x00016060
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, TResult>._CombineLatest)state;
				combineLatest.running2 = false;
				try
				{
					if (combineLatest.awaiter2.GetResult())
					{
						combineLatest.hasCurrent2 = true;
						combineLatest.current2 = combineLatest.enumerator2.Current;
					}
					else
					{
						combineLatest.running2 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 2)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 2;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running2 = true;
					try
					{
						combineLatest.awaiter2 = combineLatest.enumerator2.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 2;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x00017F64 File Offset: 0x00016164
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2)
				{
					this.result = this.resultSelector(this.current1, this.current2);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004A4 RID: 1188 RVA: 0x00017FA4 File Offset: 0x000161A4
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, TResult>._CombineLatest.<DisposeAsync>d__27 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, TResult>._CombineLatest.<DisposeAsync>d__27>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040004AE RID: 1198
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, TResult>._CombineLatest.Completed1);

			// Token: 0x040004AF RID: 1199
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, TResult>._CombineLatest.Completed2);

			// Token: 0x040004B0 RID: 1200
			private const int CompleteCount = 2;

			// Token: 0x040004B1 RID: 1201
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x040004B2 RID: 1202
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x040004B3 RID: 1203
			private readonly Func<T1, T2, TResult> resultSelector;

			// Token: 0x040004B4 RID: 1204
			private CancellationToken cancellationToken;

			// Token: 0x040004B5 RID: 1205
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x040004B6 RID: 1206
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x040004B7 RID: 1207
			private bool hasCurrent1;

			// Token: 0x040004B8 RID: 1208
			private bool running1;

			// Token: 0x040004B9 RID: 1209
			private T1 current1;

			// Token: 0x040004BA RID: 1210
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x040004BB RID: 1211
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x040004BC RID: 1212
			private bool hasCurrent2;

			// Token: 0x040004BD RID: 1213
			private bool running2;

			// Token: 0x040004BE RID: 1214
			private T2 current2;

			// Token: 0x040004BF RID: 1215
			private int completedCount;

			// Token: 0x040004C0 RID: 1216
			private bool syncRunning;

			// Token: 0x040004C1 RID: 1217
			private TResult result;
		}
	}
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200000F RID: 15
	internal class CombineLatest<T1, T2, T3, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600023D RID: 573 RVA: 0x0000858C File Offset: 0x0000678C
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, Func<T1, T2, T3, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000085B1 File Offset: 0x000067B1
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400001C RID: 28
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400001D RID: 29
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400001E RID: 30
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x0400001F RID: 31
		private readonly Func<T1, T2, T3, TResult> resultSelector;

		// Token: 0x020000E0 RID: 224
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004A6 RID: 1190 RVA: 0x0001800B File Offset: 0x0001620B
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, Func<T1, T2, T3, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00018038 File Offset: 0x00016238
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004A8 RID: 1192 RVA: 0x00018040 File Offset: 0x00016240
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 3)
				{
					return CompletedTasks.False;
				}
				if (this.enumerator1 == null)
				{
					this.enumerator1 = this.source1.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator2 = this.source2.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator3 = this.source3.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004A9 RID: 1193 RVA: 0x000181EC File Offset: 0x000163EC
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 3)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 3;
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
						combineLatest.completedCount = 3;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004AA RID: 1194 RVA: 0x000182F0 File Offset: 0x000164F0
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 3)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 3;
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
						combineLatest.completedCount = 3;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004AB RID: 1195 RVA: 0x000183F4 File Offset: 0x000165F4
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, TResult>._CombineLatest)state;
				combineLatest.running3 = false;
				try
				{
					if (combineLatest.awaiter3.GetResult())
					{
						combineLatest.hasCurrent3 = true;
						combineLatest.current3 = combineLatest.enumerator3.Current;
					}
					else
					{
						combineLatest.running3 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 3)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 3;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running3 = true;
					try
					{
						combineLatest.awaiter3 = combineLatest.enumerator3.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 3;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004AC RID: 1196 RVA: 0x000184F8 File Offset: 0x000166F8
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004AD RID: 1197 RVA: 0x00018550 File Offset: 0x00016750
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, TResult>._CombineLatest.<DisposeAsync>d__35 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, TResult>._CombineLatest.<DisposeAsync>d__35>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040004C2 RID: 1218
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed1);

			// Token: 0x040004C3 RID: 1219
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed2);

			// Token: 0x040004C4 RID: 1220
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, TResult>._CombineLatest.Completed3);

			// Token: 0x040004C5 RID: 1221
			private const int CompleteCount = 3;

			// Token: 0x040004C6 RID: 1222
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x040004C7 RID: 1223
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x040004C8 RID: 1224
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x040004C9 RID: 1225
			private readonly Func<T1, T2, T3, TResult> resultSelector;

			// Token: 0x040004CA RID: 1226
			private CancellationToken cancellationToken;

			// Token: 0x040004CB RID: 1227
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x040004CC RID: 1228
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x040004CD RID: 1229
			private bool hasCurrent1;

			// Token: 0x040004CE RID: 1230
			private bool running1;

			// Token: 0x040004CF RID: 1231
			private T1 current1;

			// Token: 0x040004D0 RID: 1232
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x040004D1 RID: 1233
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x040004D2 RID: 1234
			private bool hasCurrent2;

			// Token: 0x040004D3 RID: 1235
			private bool running2;

			// Token: 0x040004D4 RID: 1236
			private T2 current2;

			// Token: 0x040004D5 RID: 1237
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x040004D6 RID: 1238
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x040004D7 RID: 1239
			private bool hasCurrent3;

			// Token: 0x040004D8 RID: 1240
			private bool running3;

			// Token: 0x040004D9 RID: 1241
			private T3 current3;

			// Token: 0x040004DA RID: 1242
			private int completedCount;

			// Token: 0x040004DB RID: 1243
			private bool syncRunning;

			// Token: 0x040004DC RID: 1244
			private TResult result;
		}
	}
}

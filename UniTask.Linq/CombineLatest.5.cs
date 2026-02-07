using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000012 RID: 18
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000243 RID: 579 RVA: 0x00008685 File Offset: 0x00006885
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000086C2 File Offset: 0x000068C2
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400002B RID: 43
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400002C RID: 44
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400002D RID: 45
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x0400002E RID: 46
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x0400002F RID: 47
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000030 RID: 48
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000031 RID: 49
		private readonly Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector;

		// Token: 0x020000E3 RID: 227
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004C4 RID: 1220 RVA: 0x00019634 File Offset: 0x00017834
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.source5 = source5;
				this.source6 = source6;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000D RID: 13
			// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00019684 File Offset: 0x00017884
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004C6 RID: 1222 RVA: 0x0001968C File Offset: 0x0001788C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 6)
				{
					return CompletedTasks.False;
				}
				if (this.enumerator1 == null)
				{
					this.enumerator1 = this.source1.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator2 = this.source2.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator3 = this.source3.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator4 = this.source4.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator5 = this.source5.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator6 = this.source6.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004C7 RID: 1223 RVA: 0x00019988 File Offset: 0x00017B88
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 6;
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
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x00019A8C File Offset: 0x00017C8C
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 6;
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
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x00019B90 File Offset: 0x00017D90
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 6;
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
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x00019C94 File Offset: 0x00017E94
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
				combineLatest.running4 = false;
				try
				{
					if (combineLatest.awaiter4.GetResult())
					{
						combineLatest.hasCurrent4 = true;
						combineLatest.current4 = combineLatest.enumerator4.Current;
					}
					else
					{
						combineLatest.running4 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 6;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running4 = true;
					try
					{
						combineLatest.awaiter4 = combineLatest.enumerator4.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004CB RID: 1227 RVA: 0x00019D98 File Offset: 0x00017F98
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
				combineLatest.running5 = false;
				try
				{
					if (combineLatest.awaiter5.GetResult())
					{
						combineLatest.hasCurrent5 = true;
						combineLatest.current5 = combineLatest.enumerator5.Current;
					}
					else
					{
						combineLatest.running5 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 6;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running5 = true;
					try
					{
						combineLatest.awaiter5 = combineLatest.enumerator5.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004CC RID: 1228 RVA: 0x00019E9C File Offset: 0x0001809C
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest)state;
				combineLatest.running6 = false;
				try
				{
					if (combineLatest.awaiter6.GetResult())
					{
						combineLatest.hasCurrent6 = true;
						combineLatest.current6 = combineLatest.enumerator6.Current;
					}
					else
					{
						combineLatest.running6 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 6)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 6;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running6 = true;
					try
					{
						combineLatest.awaiter6 = combineLatest.enumerator6.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 6;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004CD RID: 1229 RVA: 0x00019FA0 File Offset: 0x000181A0
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x0001A024 File Offset: 0x00018224
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.<DisposeAsync>d__59 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.<DisposeAsync>d__59>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000528 RID: 1320
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed1);

			// Token: 0x04000529 RID: 1321
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed2);

			// Token: 0x0400052A RID: 1322
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed3);

			// Token: 0x0400052B RID: 1323
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed4);

			// Token: 0x0400052C RID: 1324
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed5);

			// Token: 0x0400052D RID: 1325
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, TResult>._CombineLatest.Completed6);

			// Token: 0x0400052E RID: 1326
			private const int CompleteCount = 6;

			// Token: 0x0400052F RID: 1327
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x04000530 RID: 1328
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x04000531 RID: 1329
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x04000532 RID: 1330
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x04000533 RID: 1331
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x04000534 RID: 1332
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x04000535 RID: 1333
			private readonly Func<T1, T2, T3, T4, T5, T6, TResult> resultSelector;

			// Token: 0x04000536 RID: 1334
			private CancellationToken cancellationToken;

			// Token: 0x04000537 RID: 1335
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x04000538 RID: 1336
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x04000539 RID: 1337
			private bool hasCurrent1;

			// Token: 0x0400053A RID: 1338
			private bool running1;

			// Token: 0x0400053B RID: 1339
			private T1 current1;

			// Token: 0x0400053C RID: 1340
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x0400053D RID: 1341
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x0400053E RID: 1342
			private bool hasCurrent2;

			// Token: 0x0400053F RID: 1343
			private bool running2;

			// Token: 0x04000540 RID: 1344
			private T2 current2;

			// Token: 0x04000541 RID: 1345
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000542 RID: 1346
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000543 RID: 1347
			private bool hasCurrent3;

			// Token: 0x04000544 RID: 1348
			private bool running3;

			// Token: 0x04000545 RID: 1349
			private T3 current3;

			// Token: 0x04000546 RID: 1350
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000547 RID: 1351
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x04000548 RID: 1352
			private bool hasCurrent4;

			// Token: 0x04000549 RID: 1353
			private bool running4;

			// Token: 0x0400054A RID: 1354
			private T4 current4;

			// Token: 0x0400054B RID: 1355
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400054C RID: 1356
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400054D RID: 1357
			private bool hasCurrent5;

			// Token: 0x0400054E RID: 1358
			private bool running5;

			// Token: 0x0400054F RID: 1359
			private T5 current5;

			// Token: 0x04000550 RID: 1360
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x04000551 RID: 1361
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000552 RID: 1362
			private bool hasCurrent6;

			// Token: 0x04000553 RID: 1363
			private bool running6;

			// Token: 0x04000554 RID: 1364
			private T6 current6;

			// Token: 0x04000555 RID: 1365
			private int completedCount;

			// Token: 0x04000556 RID: 1366
			private bool syncRunning;

			// Token: 0x04000557 RID: 1367
			private TResult result;
		}
	}
}

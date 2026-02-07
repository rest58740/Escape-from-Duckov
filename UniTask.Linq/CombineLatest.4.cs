using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000011 RID: 17
	internal class CombineLatest<T1, T2, T3, T4, T5, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000241 RID: 577 RVA: 0x00008624 File Offset: 0x00006824
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008659 File Offset: 0x00006859
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000025 RID: 37
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000026 RID: 38
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000027 RID: 39
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000028 RID: 40
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000029 RID: 41
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x0400002A RID: 42
		private readonly Func<T1, T2, T3, T4, T5, TResult> resultSelector;

		// Token: 0x020000E2 RID: 226
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004B9 RID: 1209 RVA: 0x00018D31 File Offset: 0x00016F31
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, Func<T1, T2, T3, T4, T5, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.source5 = source5;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000C RID: 12
			// (get) Token: 0x060004BA RID: 1210 RVA: 0x00018D6E File Offset: 0x00016F6E
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004BB RID: 1211 RVA: 0x00018D78 File Offset: 0x00016F78
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 5)
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
							CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004BC RID: 1212 RVA: 0x00019004 File Offset: 0x00017204
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 5)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 5;
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
						combineLatest.completedCount = 5;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x00019108 File Offset: 0x00017308
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 5)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 5;
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
						combineLatest.completedCount = 5;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x0001920C File Offset: 0x0001740C
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 5)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 5;
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
						combineLatest.completedCount = 5;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x00019310 File Offset: 0x00017510
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 5)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 5;
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
						combineLatest.completedCount = 5;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x00019414 File Offset: 0x00017614
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 5)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 5;
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
						combineLatest.completedCount = 5;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004C1 RID: 1217 RVA: 0x00019518 File Offset: 0x00017718
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x0001958C File Offset: 0x0001778C
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.<DisposeAsync>d__51 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.<DisposeAsync>d__51>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040004FF RID: 1279
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed1);

			// Token: 0x04000500 RID: 1280
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed2);

			// Token: 0x04000501 RID: 1281
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed3);

			// Token: 0x04000502 RID: 1282
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed4);

			// Token: 0x04000503 RID: 1283
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, TResult>._CombineLatest.Completed5);

			// Token: 0x04000504 RID: 1284
			private const int CompleteCount = 5;

			// Token: 0x04000505 RID: 1285
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x04000506 RID: 1286
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x04000507 RID: 1287
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x04000508 RID: 1288
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x04000509 RID: 1289
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x0400050A RID: 1290
			private readonly Func<T1, T2, T3, T4, T5, TResult> resultSelector;

			// Token: 0x0400050B RID: 1291
			private CancellationToken cancellationToken;

			// Token: 0x0400050C RID: 1292
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x0400050D RID: 1293
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x0400050E RID: 1294
			private bool hasCurrent1;

			// Token: 0x0400050F RID: 1295
			private bool running1;

			// Token: 0x04000510 RID: 1296
			private T1 current1;

			// Token: 0x04000511 RID: 1297
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x04000512 RID: 1298
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04000513 RID: 1299
			private bool hasCurrent2;

			// Token: 0x04000514 RID: 1300
			private bool running2;

			// Token: 0x04000515 RID: 1301
			private T2 current2;

			// Token: 0x04000516 RID: 1302
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000517 RID: 1303
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000518 RID: 1304
			private bool hasCurrent3;

			// Token: 0x04000519 RID: 1305
			private bool running3;

			// Token: 0x0400051A RID: 1306
			private T3 current3;

			// Token: 0x0400051B RID: 1307
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x0400051C RID: 1308
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x0400051D RID: 1309
			private bool hasCurrent4;

			// Token: 0x0400051E RID: 1310
			private bool running4;

			// Token: 0x0400051F RID: 1311
			private T4 current4;

			// Token: 0x04000520 RID: 1312
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x04000521 RID: 1313
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x04000522 RID: 1314
			private bool hasCurrent5;

			// Token: 0x04000523 RID: 1315
			private bool running5;

			// Token: 0x04000524 RID: 1316
			private T5 current5;

			// Token: 0x04000525 RID: 1317
			private int completedCount;

			// Token: 0x04000526 RID: 1318
			private bool syncRunning;

			// Token: 0x04000527 RID: 1319
			private TResult result;
		}
	}
}

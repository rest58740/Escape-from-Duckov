using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000013 RID: 19
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000245 RID: 581 RVA: 0x000086F4 File Offset: 0x000068F4
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.source7 = source7;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008744 File Offset: 0x00006944
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000032 RID: 50
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000033 RID: 51
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000034 RID: 52
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000035 RID: 53
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000036 RID: 54
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000037 RID: 55
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000038 RID: 56
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000039 RID: 57
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector;

		// Token: 0x020000E4 RID: 228
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004D0 RID: 1232 RVA: 0x0001A0DC File Offset: 0x000182DC
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.source5 = source5;
				this.source6 = source6;
				this.source7 = source7;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000E RID: 14
			// (get) Token: 0x060004D1 RID: 1233 RVA: 0x0001A134 File Offset: 0x00018334
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004D2 RID: 1234 RVA: 0x0001A13C File Offset: 0x0001833C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 7)
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
					this.enumerator7 = this.source7.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004D3 RID: 1235 RVA: 0x0001A4A8 File Offset: 0x000186A8
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D4 RID: 1236 RVA: 0x0001A5AC File Offset: 0x000187AC
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D5 RID: 1237 RVA: 0x0001A6B0 File Offset: 0x000188B0
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D6 RID: 1238 RVA: 0x0001A7B4 File Offset: 0x000189B4
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D7 RID: 1239 RVA: 0x0001A8B8 File Offset: 0x00018AB8
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D8 RID: 1240 RVA: 0x0001A9BC File Offset: 0x00018BBC
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 7;
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
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x0001AAC0 File Offset: 0x00018CC0
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest)state;
				combineLatest.running7 = false;
				try
				{
					if (combineLatest.awaiter7.GetResult())
					{
						combineLatest.hasCurrent7 = true;
						combineLatest.current7 = combineLatest.enumerator7.Current;
					}
					else
					{
						combineLatest.running7 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 7)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 7;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running7 = true;
					try
					{
						combineLatest.awaiter7 = combineLatest.enumerator7.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 7;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x0001ABC4 File Offset: 0x00018DC4
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004DB RID: 1243 RVA: 0x0001AC54 File Offset: 0x00018E54
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.<DisposeAsync>d__67 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.<DisposeAsync>d__67>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000558 RID: 1368
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed1);

			// Token: 0x04000559 RID: 1369
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed2);

			// Token: 0x0400055A RID: 1370
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed3);

			// Token: 0x0400055B RID: 1371
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed4);

			// Token: 0x0400055C RID: 1372
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed5);

			// Token: 0x0400055D RID: 1373
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed6);

			// Token: 0x0400055E RID: 1374
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>._CombineLatest.Completed7);

			// Token: 0x0400055F RID: 1375
			private const int CompleteCount = 7;

			// Token: 0x04000560 RID: 1376
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x04000561 RID: 1377
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x04000562 RID: 1378
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x04000563 RID: 1379
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x04000564 RID: 1380
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x04000565 RID: 1381
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x04000566 RID: 1382
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x04000567 RID: 1383
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> resultSelector;

			// Token: 0x04000568 RID: 1384
			private CancellationToken cancellationToken;

			// Token: 0x04000569 RID: 1385
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x0400056A RID: 1386
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x0400056B RID: 1387
			private bool hasCurrent1;

			// Token: 0x0400056C RID: 1388
			private bool running1;

			// Token: 0x0400056D RID: 1389
			private T1 current1;

			// Token: 0x0400056E RID: 1390
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x0400056F RID: 1391
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04000570 RID: 1392
			private bool hasCurrent2;

			// Token: 0x04000571 RID: 1393
			private bool running2;

			// Token: 0x04000572 RID: 1394
			private T2 current2;

			// Token: 0x04000573 RID: 1395
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000574 RID: 1396
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000575 RID: 1397
			private bool hasCurrent3;

			// Token: 0x04000576 RID: 1398
			private bool running3;

			// Token: 0x04000577 RID: 1399
			private T3 current3;

			// Token: 0x04000578 RID: 1400
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000579 RID: 1401
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x0400057A RID: 1402
			private bool hasCurrent4;

			// Token: 0x0400057B RID: 1403
			private bool running4;

			// Token: 0x0400057C RID: 1404
			private T4 current4;

			// Token: 0x0400057D RID: 1405
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400057E RID: 1406
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400057F RID: 1407
			private bool hasCurrent5;

			// Token: 0x04000580 RID: 1408
			private bool running5;

			// Token: 0x04000581 RID: 1409
			private T5 current5;

			// Token: 0x04000582 RID: 1410
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x04000583 RID: 1411
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000584 RID: 1412
			private bool hasCurrent6;

			// Token: 0x04000585 RID: 1413
			private bool running6;

			// Token: 0x04000586 RID: 1414
			private T6 current6;

			// Token: 0x04000587 RID: 1415
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x04000588 RID: 1416
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x04000589 RID: 1417
			private bool hasCurrent7;

			// Token: 0x0400058A RID: 1418
			private bool running7;

			// Token: 0x0400058B RID: 1419
			private T7 current7;

			// Token: 0x0400058C RID: 1420
			private int completedCount;

			// Token: 0x0400058D RID: 1421
			private bool syncRunning;

			// Token: 0x0400058E RID: 1422
			private TResult result;
		}
	}
}

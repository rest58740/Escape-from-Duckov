using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000014 RID: 20
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000247 RID: 583 RVA: 0x00008788 File Offset: 0x00006988
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.source7 = source7;
			this.source8 = source8;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x000087E0 File Offset: 0x000069E0
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400003A RID: 58
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400003B RID: 59
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400003C RID: 60
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x0400003D RID: 61
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x0400003E RID: 62
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x0400003F RID: 63
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000040 RID: 64
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000041 RID: 65
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000042 RID: 66
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector;

		// Token: 0x020000E5 RID: 229
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004DD RID: 1245 RVA: 0x0001AD1C File Offset: 0x00018F1C
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.source5 = source5;
				this.source6 = source6;
				this.source7 = source7;
				this.source8 = source8;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001AD7C File Offset: 0x00018F7C
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0001AD84 File Offset: 0x00018F84
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 8)
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
					this.enumerator8 = this.source8.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004E0 RID: 1248 RVA: 0x0001B160 File Offset: 0x00019360
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E1 RID: 1249 RVA: 0x0001B264 File Offset: 0x00019464
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E2 RID: 1250 RVA: 0x0001B368 File Offset: 0x00019568
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E3 RID: 1251 RVA: 0x0001B46C File Offset: 0x0001966C
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E4 RID: 1252 RVA: 0x0001B570 File Offset: 0x00019770
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E5 RID: 1253 RVA: 0x0001B674 File Offset: 0x00019874
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E6 RID: 1254 RVA: 0x0001B778 File Offset: 0x00019978
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 8;
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
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E7 RID: 1255 RVA: 0x0001B87C File Offset: 0x00019A7C
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest)state;
				combineLatest.running8 = false;
				try
				{
					if (combineLatest.awaiter8.GetResult())
					{
						combineLatest.hasCurrent8 = true;
						combineLatest.current8 = combineLatest.enumerator8.Current;
					}
					else
					{
						combineLatest.running8 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 8)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 8;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running8 = true;
					try
					{
						combineLatest.awaiter8 = combineLatest.enumerator8.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 8;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004E8 RID: 1256 RVA: 0x0001B980 File Offset: 0x00019B80
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004E9 RID: 1257 RVA: 0x0001BA24 File Offset: 0x00019C24
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.<DisposeAsync>d__75 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.<DisposeAsync>d__75>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400058F RID: 1423
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed1);

			// Token: 0x04000590 RID: 1424
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed2);

			// Token: 0x04000591 RID: 1425
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed3);

			// Token: 0x04000592 RID: 1426
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed4);

			// Token: 0x04000593 RID: 1427
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed5);

			// Token: 0x04000594 RID: 1428
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed6);

			// Token: 0x04000595 RID: 1429
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed7);

			// Token: 0x04000596 RID: 1430
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, TResult>._CombineLatest.Completed8);

			// Token: 0x04000597 RID: 1431
			private const int CompleteCount = 8;

			// Token: 0x04000598 RID: 1432
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x04000599 RID: 1433
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x0400059A RID: 1434
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x0400059B RID: 1435
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x0400059C RID: 1436
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x0400059D RID: 1437
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x0400059E RID: 1438
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x0400059F RID: 1439
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x040005A0 RID: 1440
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> resultSelector;

			// Token: 0x040005A1 RID: 1441
			private CancellationToken cancellationToken;

			// Token: 0x040005A2 RID: 1442
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x040005A3 RID: 1443
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x040005A4 RID: 1444
			private bool hasCurrent1;

			// Token: 0x040005A5 RID: 1445
			private bool running1;

			// Token: 0x040005A6 RID: 1446
			private T1 current1;

			// Token: 0x040005A7 RID: 1447
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x040005A8 RID: 1448
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x040005A9 RID: 1449
			private bool hasCurrent2;

			// Token: 0x040005AA RID: 1450
			private bool running2;

			// Token: 0x040005AB RID: 1451
			private T2 current2;

			// Token: 0x040005AC RID: 1452
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x040005AD RID: 1453
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x040005AE RID: 1454
			private bool hasCurrent3;

			// Token: 0x040005AF RID: 1455
			private bool running3;

			// Token: 0x040005B0 RID: 1456
			private T3 current3;

			// Token: 0x040005B1 RID: 1457
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x040005B2 RID: 1458
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x040005B3 RID: 1459
			private bool hasCurrent4;

			// Token: 0x040005B4 RID: 1460
			private bool running4;

			// Token: 0x040005B5 RID: 1461
			private T4 current4;

			// Token: 0x040005B6 RID: 1462
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x040005B7 RID: 1463
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x040005B8 RID: 1464
			private bool hasCurrent5;

			// Token: 0x040005B9 RID: 1465
			private bool running5;

			// Token: 0x040005BA RID: 1466
			private T5 current5;

			// Token: 0x040005BB RID: 1467
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x040005BC RID: 1468
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x040005BD RID: 1469
			private bool hasCurrent6;

			// Token: 0x040005BE RID: 1470
			private bool running6;

			// Token: 0x040005BF RID: 1471
			private T6 current6;

			// Token: 0x040005C0 RID: 1472
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x040005C1 RID: 1473
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x040005C2 RID: 1474
			private bool hasCurrent7;

			// Token: 0x040005C3 RID: 1475
			private bool running7;

			// Token: 0x040005C4 RID: 1476
			private T7 current7;

			// Token: 0x040005C5 RID: 1477
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x040005C6 RID: 1478
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x040005C7 RID: 1479
			private bool hasCurrent8;

			// Token: 0x040005C8 RID: 1480
			private bool running8;

			// Token: 0x040005C9 RID: 1481
			private T8 current8;

			// Token: 0x040005CA RID: 1482
			private int completedCount;

			// Token: 0x040005CB RID: 1483
			private bool syncRunning;

			// Token: 0x040005CC RID: 1484
			private TResult result;
		}
	}
}

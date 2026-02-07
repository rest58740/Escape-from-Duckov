using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000016 RID: 22
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600024B RID: 587 RVA: 0x000088DC File Offset: 0x00006ADC
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.source5 = source5;
			this.source6 = source6;
			this.source7 = source7;
			this.source8 = source8;
			this.source9 = source9;
			this.source10 = source10;
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008944 File Offset: 0x00006B44
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.source9, this.source10, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400004D RID: 77
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400004E RID: 78
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400004F RID: 79
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000050 RID: 80
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000051 RID: 81
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000052 RID: 82
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000053 RID: 83
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000054 RID: 84
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000055 RID: 85
		private readonly IUniTaskAsyncEnumerable<T9> source9;

		// Token: 0x04000056 RID: 86
		private readonly IUniTaskAsyncEnumerable<T10> source10;

		// Token: 0x04000057 RID: 87
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector;

		// Token: 0x020000E7 RID: 231
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004FA RID: 1274 RVA: 0x0001CAAC File Offset: 0x0001ACAC
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.source5 = source5;
				this.source6 = source6;
				this.source7 = source7;
				this.source8 = source8;
				this.source9 = source9;
				this.source10 = source10;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000011 RID: 17
			// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001CB1C File Offset: 0x0001AD1C
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004FC RID: 1276 RVA: 0x0001CB24 File Offset: 0x0001AD24
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 10)
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
					this.enumerator9 = this.source9.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator10 = this.source10.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
					if (!this.running9)
					{
						this.running9 = true;
						this.awaiter9 = this.enumerator9.MoveNextAsync().GetAwaiter();
						if (this.awaiter9.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed9(this);
						}
						else
						{
							this.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed9Delegate, this);
						}
					}
					if (!this.running10)
					{
						this.running10 = true;
						this.awaiter10 = this.enumerator10.MoveNextAsync().GetAwaiter();
						if (this.awaiter10.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed10(this);
						}
						else
						{
							this.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed10Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8 || !this.running9 || !this.running10);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004FD RID: 1277 RVA: 0x0001CFE4 File Offset: 0x0001B1E4
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004FE RID: 1278 RVA: 0x0001D0EC File Offset: 0x0001B2EC
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004FF RID: 1279 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000500 RID: 1280 RVA: 0x0001D2FC File Offset: 0x0001B4FC
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x0001D404 File Offset: 0x0001B604
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000502 RID: 1282 RVA: 0x0001D50C File Offset: 0x0001B70C
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000503 RID: 1283 RVA: 0x0001D614 File Offset: 0x0001B814
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x0001D71C File Offset: 0x0001B91C
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 10;
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
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x0001D824 File Offset: 0x0001BA24
			private static void Completed9(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
				combineLatest.running9 = false;
				try
				{
					if (combineLatest.awaiter9.GetResult())
					{
						combineLatest.hasCurrent9 = true;
						combineLatest.current9 = combineLatest.enumerator9.Current;
					}
					else
					{
						combineLatest.running9 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running9 = true;
					combineLatest.completedCount = 10;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running9 = true;
					try
					{
						combineLatest.awaiter9 = combineLatest.enumerator9.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed9Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000506 RID: 1286 RVA: 0x0001D92C File Offset: 0x0001BB2C
			private static void Completed10(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest)state;
				combineLatest.running10 = false;
				try
				{
					if (combineLatest.awaiter10.GetResult())
					{
						combineLatest.hasCurrent10 = true;
						combineLatest.current10 = combineLatest.enumerator10.Current;
					}
					else
					{
						combineLatest.running10 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 10)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running10 = true;
					combineLatest.completedCount = 10;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running10 = true;
					try
					{
						combineLatest.awaiter10 = combineLatest.enumerator10.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 10;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed10Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000507 RID: 1287 RVA: 0x0001DA34 File Offset: 0x0001BC34
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8 && this.hasCurrent9 && this.hasCurrent10)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8, this.current9, this.current10);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x06000508 RID: 1288 RVA: 0x0001DB00 File Offset: 0x0001BD00
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.<DisposeAsync>d__91 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.<DisposeAsync>d__91>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000612 RID: 1554
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed1);

			// Token: 0x04000613 RID: 1555
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed2);

			// Token: 0x04000614 RID: 1556
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed3);

			// Token: 0x04000615 RID: 1557
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed4);

			// Token: 0x04000616 RID: 1558
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed5);

			// Token: 0x04000617 RID: 1559
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed6);

			// Token: 0x04000618 RID: 1560
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed7);

			// Token: 0x04000619 RID: 1561
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed8);

			// Token: 0x0400061A RID: 1562
			private static readonly Action<object> Completed9Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed9);

			// Token: 0x0400061B RID: 1563
			private static readonly Action<object> Completed10Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>._CombineLatest.Completed10);

			// Token: 0x0400061C RID: 1564
			private const int CompleteCount = 10;

			// Token: 0x0400061D RID: 1565
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x0400061E RID: 1566
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x0400061F RID: 1567
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x04000620 RID: 1568
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x04000621 RID: 1569
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x04000622 RID: 1570
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x04000623 RID: 1571
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x04000624 RID: 1572
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x04000625 RID: 1573
			private readonly IUniTaskAsyncEnumerable<T9> source9;

			// Token: 0x04000626 RID: 1574
			private readonly IUniTaskAsyncEnumerable<T10> source10;

			// Token: 0x04000627 RID: 1575
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> resultSelector;

			// Token: 0x04000628 RID: 1576
			private CancellationToken cancellationToken;

			// Token: 0x04000629 RID: 1577
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x0400062A RID: 1578
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x0400062B RID: 1579
			private bool hasCurrent1;

			// Token: 0x0400062C RID: 1580
			private bool running1;

			// Token: 0x0400062D RID: 1581
			private T1 current1;

			// Token: 0x0400062E RID: 1582
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x0400062F RID: 1583
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04000630 RID: 1584
			private bool hasCurrent2;

			// Token: 0x04000631 RID: 1585
			private bool running2;

			// Token: 0x04000632 RID: 1586
			private T2 current2;

			// Token: 0x04000633 RID: 1587
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000634 RID: 1588
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000635 RID: 1589
			private bool hasCurrent3;

			// Token: 0x04000636 RID: 1590
			private bool running3;

			// Token: 0x04000637 RID: 1591
			private T3 current3;

			// Token: 0x04000638 RID: 1592
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000639 RID: 1593
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x0400063A RID: 1594
			private bool hasCurrent4;

			// Token: 0x0400063B RID: 1595
			private bool running4;

			// Token: 0x0400063C RID: 1596
			private T4 current4;

			// Token: 0x0400063D RID: 1597
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400063E RID: 1598
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400063F RID: 1599
			private bool hasCurrent5;

			// Token: 0x04000640 RID: 1600
			private bool running5;

			// Token: 0x04000641 RID: 1601
			private T5 current5;

			// Token: 0x04000642 RID: 1602
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x04000643 RID: 1603
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000644 RID: 1604
			private bool hasCurrent6;

			// Token: 0x04000645 RID: 1605
			private bool running6;

			// Token: 0x04000646 RID: 1606
			private T6 current6;

			// Token: 0x04000647 RID: 1607
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x04000648 RID: 1608
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x04000649 RID: 1609
			private bool hasCurrent7;

			// Token: 0x0400064A RID: 1610
			private bool running7;

			// Token: 0x0400064B RID: 1611
			private T7 current7;

			// Token: 0x0400064C RID: 1612
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x0400064D RID: 1613
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x0400064E RID: 1614
			private bool hasCurrent8;

			// Token: 0x0400064F RID: 1615
			private bool running8;

			// Token: 0x04000650 RID: 1616
			private T8 current8;

			// Token: 0x04000651 RID: 1617
			private IUniTaskAsyncEnumerator<T9> enumerator9;

			// Token: 0x04000652 RID: 1618
			private UniTask<bool>.Awaiter awaiter9;

			// Token: 0x04000653 RID: 1619
			private bool hasCurrent9;

			// Token: 0x04000654 RID: 1620
			private bool running9;

			// Token: 0x04000655 RID: 1621
			private T9 current9;

			// Token: 0x04000656 RID: 1622
			private IUniTaskAsyncEnumerator<T10> enumerator10;

			// Token: 0x04000657 RID: 1623
			private UniTask<bool>.Awaiter awaiter10;

			// Token: 0x04000658 RID: 1624
			private bool hasCurrent10;

			// Token: 0x04000659 RID: 1625
			private bool running10;

			// Token: 0x0400065A RID: 1626
			private T10 current10;

			// Token: 0x0400065B RID: 1627
			private int completedCount;

			// Token: 0x0400065C RID: 1628
			private bool syncRunning;

			// Token: 0x0400065D RID: 1629
			private TResult result;
		}
	}
}

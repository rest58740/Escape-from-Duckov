using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200001B RID: 27
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000255 RID: 597 RVA: 0x00008D24 File Offset: 0x00006F24
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, IUniTaskAsyncEnumerable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector)
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
			this.source11 = source11;
			this.source12 = source12;
			this.source13 = source13;
			this.source14 = source14;
			this.source15 = source15;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00008DB4 File Offset: 0x00006FB4
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.source9, this.source10, this.source11, this.source12, this.source13, this.source14, this.source15, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400008E RID: 142
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x0400008F RID: 143
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000090 RID: 144
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000091 RID: 145
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000092 RID: 146
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000093 RID: 147
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000094 RID: 148
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000095 RID: 149
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000096 RID: 150
		private readonly IUniTaskAsyncEnumerable<T9> source9;

		// Token: 0x04000097 RID: 151
		private readonly IUniTaskAsyncEnumerable<T10> source10;

		// Token: 0x04000098 RID: 152
		private readonly IUniTaskAsyncEnumerable<T11> source11;

		// Token: 0x04000099 RID: 153
		private readonly IUniTaskAsyncEnumerable<T12> source12;

		// Token: 0x0400009A RID: 154
		private readonly IUniTaskAsyncEnumerable<T13> source13;

		// Token: 0x0400009B RID: 155
		private readonly IUniTaskAsyncEnumerable<T14> source14;

		// Token: 0x0400009C RID: 156
		private readonly IUniTaskAsyncEnumerable<T15> source15;

		// Token: 0x0400009D RID: 157
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector;

		// Token: 0x020000EC RID: 236
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000554 RID: 1364 RVA: 0x000231A8 File Offset: 0x000213A8
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, IUniTaskAsyncEnumerable<T15> source15, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector, CancellationToken cancellationToken)
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
				this.source11 = source11;
				this.source12 = source12;
				this.source13 = source13;
				this.source14 = source14;
				this.source15 = source15;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x06000555 RID: 1365 RVA: 0x00023240 File Offset: 0x00021440
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x00023248 File Offset: 0x00021448
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 15)
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
					this.enumerator11 = this.source11.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator12 = this.source12.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator13 = this.source13.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator14 = this.source14.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator15 = this.source15.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
					if (!this.running9)
					{
						this.running9 = true;
						this.awaiter9 = this.enumerator9.MoveNextAsync().GetAwaiter();
						if (this.awaiter9.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed9(this);
						}
						else
						{
							this.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed9Delegate, this);
						}
					}
					if (!this.running10)
					{
						this.running10 = true;
						this.awaiter10 = this.enumerator10.MoveNextAsync().GetAwaiter();
						if (this.awaiter10.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed10(this);
						}
						else
						{
							this.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed10Delegate, this);
						}
					}
					if (!this.running11)
					{
						this.running11 = true;
						this.awaiter11 = this.enumerator11.MoveNextAsync().GetAwaiter();
						if (this.awaiter11.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed11(this);
						}
						else
						{
							this.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed11Delegate, this);
						}
					}
					if (!this.running12)
					{
						this.running12 = true;
						this.awaiter12 = this.enumerator12.MoveNextAsync().GetAwaiter();
						if (this.awaiter12.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed12(this);
						}
						else
						{
							this.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed12Delegate, this);
						}
					}
					if (!this.running13)
					{
						this.running13 = true;
						this.awaiter13 = this.enumerator13.MoveNextAsync().GetAwaiter();
						if (this.awaiter13.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed13(this);
						}
						else
						{
							this.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed13Delegate, this);
						}
					}
					if (!this.running14)
					{
						this.running14 = true;
						this.awaiter14 = this.enumerator14.MoveNextAsync().GetAwaiter();
						if (this.awaiter14.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed14(this);
						}
						else
						{
							this.awaiter14.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed14Delegate, this);
						}
					}
					if (!this.running15)
					{
						this.running15 = true;
						this.awaiter15 = this.enumerator15.MoveNextAsync().GetAwaiter();
						if (this.awaiter15.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed15(this);
						}
						else
						{
							this.awaiter15.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed15Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8 || !this.running9 || !this.running10 || !this.running11 || !this.running12 || !this.running13 || !this.running14 || !this.running15);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x00023938 File Offset: 0x00021B38
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x00023A40 File Offset: 0x00021C40
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x00023B48 File Offset: 0x00021D48
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x00023C50 File Offset: 0x00021E50
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055B RID: 1371 RVA: 0x00023D58 File Offset: 0x00021F58
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x00023E60 File Offset: 0x00022060
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055D RID: 1373 RVA: 0x00023F68 File Offset: 0x00022168
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x00024070 File Offset: 0x00022270
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x00024178 File Offset: 0x00022378
			private static void Completed9(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running9 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed9Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x00024280 File Offset: 0x00022480
			private static void Completed10(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running10 = true;
					combineLatest.completedCount = 15;
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
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed10Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x00024388 File Offset: 0x00022588
			private static void Completed11(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
				combineLatest.running11 = false;
				try
				{
					if (combineLatest.awaiter11.GetResult())
					{
						combineLatest.hasCurrent11 = true;
						combineLatest.current11 = combineLatest.enumerator11.Current;
					}
					else
					{
						combineLatest.running11 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running11 = true;
					combineLatest.completedCount = 15;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running11 = true;
					try
					{
						combineLatest.awaiter11 = combineLatest.enumerator11.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed11Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x00024490 File Offset: 0x00022690
			private static void Completed12(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
				combineLatest.running12 = false;
				try
				{
					if (combineLatest.awaiter12.GetResult())
					{
						combineLatest.hasCurrent12 = true;
						combineLatest.current12 = combineLatest.enumerator12.Current;
					}
					else
					{
						combineLatest.running12 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running12 = true;
					combineLatest.completedCount = 15;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running12 = true;
					try
					{
						combineLatest.awaiter12 = combineLatest.enumerator12.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed12Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x00024598 File Offset: 0x00022798
			private static void Completed13(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
				combineLatest.running13 = false;
				try
				{
					if (combineLatest.awaiter13.GetResult())
					{
						combineLatest.hasCurrent13 = true;
						combineLatest.current13 = combineLatest.enumerator13.Current;
					}
					else
					{
						combineLatest.running13 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running13 = true;
					combineLatest.completedCount = 15;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running13 = true;
					try
					{
						combineLatest.awaiter13 = combineLatest.enumerator13.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed13Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x000246A0 File Offset: 0x000228A0
			private static void Completed14(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
				combineLatest.running14 = false;
				try
				{
					if (combineLatest.awaiter14.GetResult())
					{
						combineLatest.hasCurrent14 = true;
						combineLatest.current14 = combineLatest.enumerator14.Current;
					}
					else
					{
						combineLatest.running14 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running14 = true;
					combineLatest.completedCount = 15;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running14 = true;
					try
					{
						combineLatest.awaiter14 = combineLatest.enumerator14.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter14.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed14Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x000247A8 File Offset: 0x000229A8
			private static void Completed15(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest)state;
				combineLatest.running15 = false;
				try
				{
					if (combineLatest.awaiter15.GetResult())
					{
						combineLatest.hasCurrent15 = true;
						combineLatest.current15 = combineLatest.enumerator15.Current;
					}
					else
					{
						combineLatest.running15 = true;
						if (Interlocked.Increment(ref combineLatest.completedCount) == 15)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running15 = true;
					combineLatest.completedCount = 15;
					combineLatest.completionSource.TrySetException(ex);
					return;
				}
				if (!combineLatest.TrySetResult())
				{
					if (combineLatest.syncRunning)
					{
						return;
					}
					combineLatest.running15 = true;
					try
					{
						combineLatest.awaiter15 = combineLatest.enumerator15.MoveNextAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						combineLatest.completedCount = 15;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter15.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed15Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x000248B0 File Offset: 0x00022AB0
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8 && this.hasCurrent9 && this.hasCurrent10 && this.hasCurrent11 && this.hasCurrent12 && this.hasCurrent13 && this.hasCurrent14 && this.hasCurrent15)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8, this.current9, this.current10, this.current11, this.current12, this.current13, this.current14, this.current15);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x000249DC File Offset: 0x00022BDC
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.<DisposeAsync>d__131 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.<DisposeAsync>d__131>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040007D4 RID: 2004
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed1);

			// Token: 0x040007D5 RID: 2005
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed2);

			// Token: 0x040007D6 RID: 2006
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed3);

			// Token: 0x040007D7 RID: 2007
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed4);

			// Token: 0x040007D8 RID: 2008
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed5);

			// Token: 0x040007D9 RID: 2009
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed6);

			// Token: 0x040007DA RID: 2010
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed7);

			// Token: 0x040007DB RID: 2011
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed8);

			// Token: 0x040007DC RID: 2012
			private static readonly Action<object> Completed9Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed9);

			// Token: 0x040007DD RID: 2013
			private static readonly Action<object> Completed10Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed10);

			// Token: 0x040007DE RID: 2014
			private static readonly Action<object> Completed11Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed11);

			// Token: 0x040007DF RID: 2015
			private static readonly Action<object> Completed12Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed12);

			// Token: 0x040007E0 RID: 2016
			private static readonly Action<object> Completed13Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed13);

			// Token: 0x040007E1 RID: 2017
			private static readonly Action<object> Completed14Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed14);

			// Token: 0x040007E2 RID: 2018
			private static readonly Action<object> Completed15Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>._CombineLatest.Completed15);

			// Token: 0x040007E3 RID: 2019
			private const int CompleteCount = 15;

			// Token: 0x040007E4 RID: 2020
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x040007E5 RID: 2021
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x040007E6 RID: 2022
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x040007E7 RID: 2023
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x040007E8 RID: 2024
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x040007E9 RID: 2025
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x040007EA RID: 2026
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x040007EB RID: 2027
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x040007EC RID: 2028
			private readonly IUniTaskAsyncEnumerable<T9> source9;

			// Token: 0x040007ED RID: 2029
			private readonly IUniTaskAsyncEnumerable<T10> source10;

			// Token: 0x040007EE RID: 2030
			private readonly IUniTaskAsyncEnumerable<T11> source11;

			// Token: 0x040007EF RID: 2031
			private readonly IUniTaskAsyncEnumerable<T12> source12;

			// Token: 0x040007F0 RID: 2032
			private readonly IUniTaskAsyncEnumerable<T13> source13;

			// Token: 0x040007F1 RID: 2033
			private readonly IUniTaskAsyncEnumerable<T14> source14;

			// Token: 0x040007F2 RID: 2034
			private readonly IUniTaskAsyncEnumerable<T15> source15;

			// Token: 0x040007F3 RID: 2035
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> resultSelector;

			// Token: 0x040007F4 RID: 2036
			private CancellationToken cancellationToken;

			// Token: 0x040007F5 RID: 2037
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x040007F6 RID: 2038
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x040007F7 RID: 2039
			private bool hasCurrent1;

			// Token: 0x040007F8 RID: 2040
			private bool running1;

			// Token: 0x040007F9 RID: 2041
			private T1 current1;

			// Token: 0x040007FA RID: 2042
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x040007FB RID: 2043
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x040007FC RID: 2044
			private bool hasCurrent2;

			// Token: 0x040007FD RID: 2045
			private bool running2;

			// Token: 0x040007FE RID: 2046
			private T2 current2;

			// Token: 0x040007FF RID: 2047
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000800 RID: 2048
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000801 RID: 2049
			private bool hasCurrent3;

			// Token: 0x04000802 RID: 2050
			private bool running3;

			// Token: 0x04000803 RID: 2051
			private T3 current3;

			// Token: 0x04000804 RID: 2052
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000805 RID: 2053
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x04000806 RID: 2054
			private bool hasCurrent4;

			// Token: 0x04000807 RID: 2055
			private bool running4;

			// Token: 0x04000808 RID: 2056
			private T4 current4;

			// Token: 0x04000809 RID: 2057
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400080A RID: 2058
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400080B RID: 2059
			private bool hasCurrent5;

			// Token: 0x0400080C RID: 2060
			private bool running5;

			// Token: 0x0400080D RID: 2061
			private T5 current5;

			// Token: 0x0400080E RID: 2062
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x0400080F RID: 2063
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000810 RID: 2064
			private bool hasCurrent6;

			// Token: 0x04000811 RID: 2065
			private bool running6;

			// Token: 0x04000812 RID: 2066
			private T6 current6;

			// Token: 0x04000813 RID: 2067
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x04000814 RID: 2068
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x04000815 RID: 2069
			private bool hasCurrent7;

			// Token: 0x04000816 RID: 2070
			private bool running7;

			// Token: 0x04000817 RID: 2071
			private T7 current7;

			// Token: 0x04000818 RID: 2072
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x04000819 RID: 2073
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x0400081A RID: 2074
			private bool hasCurrent8;

			// Token: 0x0400081B RID: 2075
			private bool running8;

			// Token: 0x0400081C RID: 2076
			private T8 current8;

			// Token: 0x0400081D RID: 2077
			private IUniTaskAsyncEnumerator<T9> enumerator9;

			// Token: 0x0400081E RID: 2078
			private UniTask<bool>.Awaiter awaiter9;

			// Token: 0x0400081F RID: 2079
			private bool hasCurrent9;

			// Token: 0x04000820 RID: 2080
			private bool running9;

			// Token: 0x04000821 RID: 2081
			private T9 current9;

			// Token: 0x04000822 RID: 2082
			private IUniTaskAsyncEnumerator<T10> enumerator10;

			// Token: 0x04000823 RID: 2083
			private UniTask<bool>.Awaiter awaiter10;

			// Token: 0x04000824 RID: 2084
			private bool hasCurrent10;

			// Token: 0x04000825 RID: 2085
			private bool running10;

			// Token: 0x04000826 RID: 2086
			private T10 current10;

			// Token: 0x04000827 RID: 2087
			private IUniTaskAsyncEnumerator<T11> enumerator11;

			// Token: 0x04000828 RID: 2088
			private UniTask<bool>.Awaiter awaiter11;

			// Token: 0x04000829 RID: 2089
			private bool hasCurrent11;

			// Token: 0x0400082A RID: 2090
			private bool running11;

			// Token: 0x0400082B RID: 2091
			private T11 current11;

			// Token: 0x0400082C RID: 2092
			private IUniTaskAsyncEnumerator<T12> enumerator12;

			// Token: 0x0400082D RID: 2093
			private UniTask<bool>.Awaiter awaiter12;

			// Token: 0x0400082E RID: 2094
			private bool hasCurrent12;

			// Token: 0x0400082F RID: 2095
			private bool running12;

			// Token: 0x04000830 RID: 2096
			private T12 current12;

			// Token: 0x04000831 RID: 2097
			private IUniTaskAsyncEnumerator<T13> enumerator13;

			// Token: 0x04000832 RID: 2098
			private UniTask<bool>.Awaiter awaiter13;

			// Token: 0x04000833 RID: 2099
			private bool hasCurrent13;

			// Token: 0x04000834 RID: 2100
			private bool running13;

			// Token: 0x04000835 RID: 2101
			private T13 current13;

			// Token: 0x04000836 RID: 2102
			private IUniTaskAsyncEnumerator<T14> enumerator14;

			// Token: 0x04000837 RID: 2103
			private UniTask<bool>.Awaiter awaiter14;

			// Token: 0x04000838 RID: 2104
			private bool hasCurrent14;

			// Token: 0x04000839 RID: 2105
			private bool running14;

			// Token: 0x0400083A RID: 2106
			private T14 current14;

			// Token: 0x0400083B RID: 2107
			private IUniTaskAsyncEnumerator<T15> enumerator15;

			// Token: 0x0400083C RID: 2108
			private UniTask<bool>.Awaiter awaiter15;

			// Token: 0x0400083D RID: 2109
			private bool hasCurrent15;

			// Token: 0x0400083E RID: 2110
			private bool running15;

			// Token: 0x0400083F RID: 2111
			private T15 current15;

			// Token: 0x04000840 RID: 2112
			private int completedCount;

			// Token: 0x04000841 RID: 2113
			private bool syncRunning;

			// Token: 0x04000842 RID: 2114
			private TResult result;
		}
	}
}

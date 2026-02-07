using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000019 RID: 25
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000251 RID: 593 RVA: 0x00008B44 File Offset: 0x00006D44
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector)
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
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008BC4 File Offset: 0x00006DC4
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.source9, this.source10, this.source11, this.source12, this.source13, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000071 RID: 113
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000072 RID: 114
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000073 RID: 115
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000074 RID: 116
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000075 RID: 117
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000076 RID: 118
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000077 RID: 119
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000078 RID: 120
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000079 RID: 121
		private readonly IUniTaskAsyncEnumerable<T9> source9;

		// Token: 0x0400007A RID: 122
		private readonly IUniTaskAsyncEnumerable<T10> source10;

		// Token: 0x0400007B RID: 123
		private readonly IUniTaskAsyncEnumerable<T11> source11;

		// Token: 0x0400007C RID: 124
		private readonly IUniTaskAsyncEnumerable<T12> source12;

		// Token: 0x0400007D RID: 125
		private readonly IUniTaskAsyncEnumerable<T13> source13;

		// Token: 0x0400007E RID: 126
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector;

		// Token: 0x020000EA RID: 234
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600052D RID: 1325 RVA: 0x0002038C File Offset: 0x0001E58C
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector, CancellationToken cancellationToken)
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
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000014 RID: 20
			// (get) Token: 0x0600052E RID: 1326 RVA: 0x00020414 File Offset: 0x0001E614
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x0600052F RID: 1327 RVA: 0x0002041C File Offset: 0x0001E61C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 13)
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
					if (!this.running9)
					{
						this.running9 = true;
						this.awaiter9 = this.enumerator9.MoveNextAsync().GetAwaiter();
						if (this.awaiter9.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed9(this);
						}
						else
						{
							this.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed9Delegate, this);
						}
					}
					if (!this.running10)
					{
						this.running10 = true;
						this.awaiter10 = this.enumerator10.MoveNextAsync().GetAwaiter();
						if (this.awaiter10.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed10(this);
						}
						else
						{
							this.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed10Delegate, this);
						}
					}
					if (!this.running11)
					{
						this.running11 = true;
						this.awaiter11 = this.enumerator11.MoveNextAsync().GetAwaiter();
						if (this.awaiter11.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed11(this);
						}
						else
						{
							this.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed11Delegate, this);
						}
					}
					if (!this.running12)
					{
						this.running12 = true;
						this.awaiter12 = this.enumerator12.MoveNextAsync().GetAwaiter();
						if (this.awaiter12.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed12(this);
						}
						else
						{
							this.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed12Delegate, this);
						}
					}
					if (!this.running13)
					{
						this.running13 = true;
						this.awaiter13 = this.enumerator13.MoveNextAsync().GetAwaiter();
						if (this.awaiter13.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed13(this);
						}
						else
						{
							this.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed13Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8 || !this.running9 || !this.running10 || !this.running11 || !this.running12 || !this.running13);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000530 RID: 1328 RVA: 0x00020A2C File Offset: 0x0001EC2C
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x00020B34 File Offset: 0x0001ED34
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x00020C3C File Offset: 0x0001EE3C
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000533 RID: 1331 RVA: 0x00020D44 File Offset: 0x0001EF44
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000534 RID: 1332 RVA: 0x00020E4C File Offset: 0x0001F04C
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000535 RID: 1333 RVA: 0x00020F54 File Offset: 0x0001F154
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000536 RID: 1334 RVA: 0x0002105C File Offset: 0x0001F25C
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000537 RID: 1335 RVA: 0x00021164 File Offset: 0x0001F364
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000538 RID: 1336 RVA: 0x0002126C File Offset: 0x0001F46C
			private static void Completed9(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running9 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed9Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000539 RID: 1337 RVA: 0x00021374 File Offset: 0x0001F574
			private static void Completed10(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running10 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed10Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600053A RID: 1338 RVA: 0x0002147C File Offset: 0x0001F67C
			private static void Completed11(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running11 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed11Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600053B RID: 1339 RVA: 0x00021584 File Offset: 0x0001F784
			private static void Completed12(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running12 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed12Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600053C RID: 1340 RVA: 0x0002168C File Offset: 0x0001F88C
			private static void Completed13(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 13)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running13 = true;
					combineLatest.completedCount = 13;
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
						combineLatest.completedCount = 13;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed13Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600053D RID: 1341 RVA: 0x00021794 File Offset: 0x0001F994
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8 && this.hasCurrent9 && this.hasCurrent10 && this.hasCurrent11 && this.hasCurrent12 && this.hasCurrent13)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8, this.current9, this.current10, this.current11, this.current12, this.current13);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x0600053E RID: 1342 RVA: 0x00021898 File Offset: 0x0001FA98
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.<DisposeAsync>d__115 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.<DisposeAsync>d__115>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400070B RID: 1803
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed1);

			// Token: 0x0400070C RID: 1804
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed2);

			// Token: 0x0400070D RID: 1805
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed3);

			// Token: 0x0400070E RID: 1806
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed4);

			// Token: 0x0400070F RID: 1807
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed5);

			// Token: 0x04000710 RID: 1808
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed6);

			// Token: 0x04000711 RID: 1809
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed7);

			// Token: 0x04000712 RID: 1810
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed8);

			// Token: 0x04000713 RID: 1811
			private static readonly Action<object> Completed9Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed9);

			// Token: 0x04000714 RID: 1812
			private static readonly Action<object> Completed10Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed10);

			// Token: 0x04000715 RID: 1813
			private static readonly Action<object> Completed11Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed11);

			// Token: 0x04000716 RID: 1814
			private static readonly Action<object> Completed12Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed12);

			// Token: 0x04000717 RID: 1815
			private static readonly Action<object> Completed13Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>._CombineLatest.Completed13);

			// Token: 0x04000718 RID: 1816
			private const int CompleteCount = 13;

			// Token: 0x04000719 RID: 1817
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x0400071A RID: 1818
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x0400071B RID: 1819
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x0400071C RID: 1820
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x0400071D RID: 1821
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x0400071E RID: 1822
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x0400071F RID: 1823
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x04000720 RID: 1824
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x04000721 RID: 1825
			private readonly IUniTaskAsyncEnumerable<T9> source9;

			// Token: 0x04000722 RID: 1826
			private readonly IUniTaskAsyncEnumerable<T10> source10;

			// Token: 0x04000723 RID: 1827
			private readonly IUniTaskAsyncEnumerable<T11> source11;

			// Token: 0x04000724 RID: 1828
			private readonly IUniTaskAsyncEnumerable<T12> source12;

			// Token: 0x04000725 RID: 1829
			private readonly IUniTaskAsyncEnumerable<T13> source13;

			// Token: 0x04000726 RID: 1830
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> resultSelector;

			// Token: 0x04000727 RID: 1831
			private CancellationToken cancellationToken;

			// Token: 0x04000728 RID: 1832
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x04000729 RID: 1833
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x0400072A RID: 1834
			private bool hasCurrent1;

			// Token: 0x0400072B RID: 1835
			private bool running1;

			// Token: 0x0400072C RID: 1836
			private T1 current1;

			// Token: 0x0400072D RID: 1837
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x0400072E RID: 1838
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x0400072F RID: 1839
			private bool hasCurrent2;

			// Token: 0x04000730 RID: 1840
			private bool running2;

			// Token: 0x04000731 RID: 1841
			private T2 current2;

			// Token: 0x04000732 RID: 1842
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000733 RID: 1843
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000734 RID: 1844
			private bool hasCurrent3;

			// Token: 0x04000735 RID: 1845
			private bool running3;

			// Token: 0x04000736 RID: 1846
			private T3 current3;

			// Token: 0x04000737 RID: 1847
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000738 RID: 1848
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x04000739 RID: 1849
			private bool hasCurrent4;

			// Token: 0x0400073A RID: 1850
			private bool running4;

			// Token: 0x0400073B RID: 1851
			private T4 current4;

			// Token: 0x0400073C RID: 1852
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400073D RID: 1853
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400073E RID: 1854
			private bool hasCurrent5;

			// Token: 0x0400073F RID: 1855
			private bool running5;

			// Token: 0x04000740 RID: 1856
			private T5 current5;

			// Token: 0x04000741 RID: 1857
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x04000742 RID: 1858
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000743 RID: 1859
			private bool hasCurrent6;

			// Token: 0x04000744 RID: 1860
			private bool running6;

			// Token: 0x04000745 RID: 1861
			private T6 current6;

			// Token: 0x04000746 RID: 1862
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x04000747 RID: 1863
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x04000748 RID: 1864
			private bool hasCurrent7;

			// Token: 0x04000749 RID: 1865
			private bool running7;

			// Token: 0x0400074A RID: 1866
			private T7 current7;

			// Token: 0x0400074B RID: 1867
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x0400074C RID: 1868
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x0400074D RID: 1869
			private bool hasCurrent8;

			// Token: 0x0400074E RID: 1870
			private bool running8;

			// Token: 0x0400074F RID: 1871
			private T8 current8;

			// Token: 0x04000750 RID: 1872
			private IUniTaskAsyncEnumerator<T9> enumerator9;

			// Token: 0x04000751 RID: 1873
			private UniTask<bool>.Awaiter awaiter9;

			// Token: 0x04000752 RID: 1874
			private bool hasCurrent9;

			// Token: 0x04000753 RID: 1875
			private bool running9;

			// Token: 0x04000754 RID: 1876
			private T9 current9;

			// Token: 0x04000755 RID: 1877
			private IUniTaskAsyncEnumerator<T10> enumerator10;

			// Token: 0x04000756 RID: 1878
			private UniTask<bool>.Awaiter awaiter10;

			// Token: 0x04000757 RID: 1879
			private bool hasCurrent10;

			// Token: 0x04000758 RID: 1880
			private bool running10;

			// Token: 0x04000759 RID: 1881
			private T10 current10;

			// Token: 0x0400075A RID: 1882
			private IUniTaskAsyncEnumerator<T11> enumerator11;

			// Token: 0x0400075B RID: 1883
			private UniTask<bool>.Awaiter awaiter11;

			// Token: 0x0400075C RID: 1884
			private bool hasCurrent11;

			// Token: 0x0400075D RID: 1885
			private bool running11;

			// Token: 0x0400075E RID: 1886
			private T11 current11;

			// Token: 0x0400075F RID: 1887
			private IUniTaskAsyncEnumerator<T12> enumerator12;

			// Token: 0x04000760 RID: 1888
			private UniTask<bool>.Awaiter awaiter12;

			// Token: 0x04000761 RID: 1889
			private bool hasCurrent12;

			// Token: 0x04000762 RID: 1890
			private bool running12;

			// Token: 0x04000763 RID: 1891
			private T12 current12;

			// Token: 0x04000764 RID: 1892
			private IUniTaskAsyncEnumerator<T13> enumerator13;

			// Token: 0x04000765 RID: 1893
			private UniTask<bool>.Awaiter awaiter13;

			// Token: 0x04000766 RID: 1894
			private bool hasCurrent13;

			// Token: 0x04000767 RID: 1895
			private bool running13;

			// Token: 0x04000768 RID: 1896
			private T13 current13;

			// Token: 0x04000769 RID: 1897
			private int completedCount;

			// Token: 0x0400076A RID: 1898
			private bool syncRunning;

			// Token: 0x0400076B RID: 1899
			private TResult result;
		}
	}
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200001A RID: 26
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000253 RID: 595 RVA: 0x00008C2C File Offset: 0x00006E2C
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector)
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
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008CB4 File Offset: 0x00006EB4
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.source9, this.source10, this.source11, this.source12, this.source13, this.source14, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400007F RID: 127
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000080 RID: 128
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000081 RID: 129
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000082 RID: 130
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000083 RID: 131
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x04000084 RID: 132
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x04000085 RID: 133
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x04000086 RID: 134
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000087 RID: 135
		private readonly IUniTaskAsyncEnumerable<T9> source9;

		// Token: 0x04000088 RID: 136
		private readonly IUniTaskAsyncEnumerable<T10> source10;

		// Token: 0x04000089 RID: 137
		private readonly IUniTaskAsyncEnumerable<T11> source11;

		// Token: 0x0400008A RID: 138
		private readonly IUniTaskAsyncEnumerable<T12> source12;

		// Token: 0x0400008B RID: 139
		private readonly IUniTaskAsyncEnumerable<T13> source13;

		// Token: 0x0400008C RID: 140
		private readonly IUniTaskAsyncEnumerable<T14> source14;

		// Token: 0x0400008D RID: 141
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector;

		// Token: 0x020000EB RID: 235
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000540 RID: 1344 RVA: 0x000219C8 File Offset: 0x0001FBC8
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, IUniTaskAsyncEnumerable<T12> source12, IUniTaskAsyncEnumerable<T13> source13, IUniTaskAsyncEnumerable<T14> source14, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector, CancellationToken cancellationToken)
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
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000015 RID: 21
			// (get) Token: 0x06000541 RID: 1345 RVA: 0x00021A58 File Offset: 0x0001FC58
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x00021A60 File Offset: 0x0001FC60
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 14)
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
					if (!this.running9)
					{
						this.running9 = true;
						this.awaiter9 = this.enumerator9.MoveNextAsync().GetAwaiter();
						if (this.awaiter9.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed9(this);
						}
						else
						{
							this.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed9Delegate, this);
						}
					}
					if (!this.running10)
					{
						this.running10 = true;
						this.awaiter10 = this.enumerator10.MoveNextAsync().GetAwaiter();
						if (this.awaiter10.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed10(this);
						}
						else
						{
							this.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed10Delegate, this);
						}
					}
					if (!this.running11)
					{
						this.running11 = true;
						this.awaiter11 = this.enumerator11.MoveNextAsync().GetAwaiter();
						if (this.awaiter11.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed11(this);
						}
						else
						{
							this.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed11Delegate, this);
						}
					}
					if (!this.running12)
					{
						this.running12 = true;
						this.awaiter12 = this.enumerator12.MoveNextAsync().GetAwaiter();
						if (this.awaiter12.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed12(this);
						}
						else
						{
							this.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed12Delegate, this);
						}
					}
					if (!this.running13)
					{
						this.running13 = true;
						this.awaiter13 = this.enumerator13.MoveNextAsync().GetAwaiter();
						if (this.awaiter13.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed13(this);
						}
						else
						{
							this.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed13Delegate, this);
						}
					}
					if (!this.running14)
					{
						this.running14 = true;
						this.awaiter14 = this.enumerator14.MoveNextAsync().GetAwaiter();
						if (this.awaiter14.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed14(this);
						}
						else
						{
							this.awaiter14.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed14Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8 || !this.running9 || !this.running10 || !this.running11 || !this.running12 || !this.running13 || !this.running14);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x000220E0 File Offset: 0x000202E0
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000544 RID: 1348 RVA: 0x000221E8 File Offset: 0x000203E8
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000545 RID: 1349 RVA: 0x000222F0 File Offset: 0x000204F0
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000546 RID: 1350 RVA: 0x000223F8 File Offset: 0x000205F8
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000547 RID: 1351 RVA: 0x00022500 File Offset: 0x00020700
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000548 RID: 1352 RVA: 0x00022608 File Offset: 0x00020808
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000549 RID: 1353 RVA: 0x00022710 File Offset: 0x00020910
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x00022818 File Offset: 0x00020A18
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054B RID: 1355 RVA: 0x00022920 File Offset: 0x00020B20
			private static void Completed9(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running9 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed9Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054C RID: 1356 RVA: 0x00022A28 File Offset: 0x00020C28
			private static void Completed10(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running10 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed10Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054D RID: 1357 RVA: 0x00022B30 File Offset: 0x00020D30
			private static void Completed11(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running11 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed11Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x00022C38 File Offset: 0x00020E38
			private static void Completed12(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running12 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter12.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed12Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x00022D40 File Offset: 0x00020F40
			private static void Completed13(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running13 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter13.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed13Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x00022E48 File Offset: 0x00021048
			private static void Completed14(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 14)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running14 = true;
					combineLatest.completedCount = 14;
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
						combineLatest.completedCount = 14;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter14.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed14Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x00022F50 File Offset: 0x00021150
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8 && this.hasCurrent9 && this.hasCurrent10 && this.hasCurrent11 && this.hasCurrent12 && this.hasCurrent13 && this.hasCurrent14)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8, this.current9, this.current10, this.current11, this.current12, this.current13, this.current14);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x00023068 File Offset: 0x00021268
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.<DisposeAsync>d__123 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.<DisposeAsync>d__123>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400076C RID: 1900
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed1);

			// Token: 0x0400076D RID: 1901
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed2);

			// Token: 0x0400076E RID: 1902
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed3);

			// Token: 0x0400076F RID: 1903
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed4);

			// Token: 0x04000770 RID: 1904
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed5);

			// Token: 0x04000771 RID: 1905
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed6);

			// Token: 0x04000772 RID: 1906
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed7);

			// Token: 0x04000773 RID: 1907
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed8);

			// Token: 0x04000774 RID: 1908
			private static readonly Action<object> Completed9Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed9);

			// Token: 0x04000775 RID: 1909
			private static readonly Action<object> Completed10Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed10);

			// Token: 0x04000776 RID: 1910
			private static readonly Action<object> Completed11Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed11);

			// Token: 0x04000777 RID: 1911
			private static readonly Action<object> Completed12Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed12);

			// Token: 0x04000778 RID: 1912
			private static readonly Action<object> Completed13Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed13);

			// Token: 0x04000779 RID: 1913
			private static readonly Action<object> Completed14Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>._CombineLatest.Completed14);

			// Token: 0x0400077A RID: 1914
			private const int CompleteCount = 14;

			// Token: 0x0400077B RID: 1915
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x0400077C RID: 1916
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x0400077D RID: 1917
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x0400077E RID: 1918
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x0400077F RID: 1919
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x04000780 RID: 1920
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x04000781 RID: 1921
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x04000782 RID: 1922
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x04000783 RID: 1923
			private readonly IUniTaskAsyncEnumerable<T9> source9;

			// Token: 0x04000784 RID: 1924
			private readonly IUniTaskAsyncEnumerable<T10> source10;

			// Token: 0x04000785 RID: 1925
			private readonly IUniTaskAsyncEnumerable<T11> source11;

			// Token: 0x04000786 RID: 1926
			private readonly IUniTaskAsyncEnumerable<T12> source12;

			// Token: 0x04000787 RID: 1927
			private readonly IUniTaskAsyncEnumerable<T13> source13;

			// Token: 0x04000788 RID: 1928
			private readonly IUniTaskAsyncEnumerable<T14> source14;

			// Token: 0x04000789 RID: 1929
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> resultSelector;

			// Token: 0x0400078A RID: 1930
			private CancellationToken cancellationToken;

			// Token: 0x0400078B RID: 1931
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x0400078C RID: 1932
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x0400078D RID: 1933
			private bool hasCurrent1;

			// Token: 0x0400078E RID: 1934
			private bool running1;

			// Token: 0x0400078F RID: 1935
			private T1 current1;

			// Token: 0x04000790 RID: 1936
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x04000791 RID: 1937
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x04000792 RID: 1938
			private bool hasCurrent2;

			// Token: 0x04000793 RID: 1939
			private bool running2;

			// Token: 0x04000794 RID: 1940
			private T2 current2;

			// Token: 0x04000795 RID: 1941
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000796 RID: 1942
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000797 RID: 1943
			private bool hasCurrent3;

			// Token: 0x04000798 RID: 1944
			private bool running3;

			// Token: 0x04000799 RID: 1945
			private T3 current3;

			// Token: 0x0400079A RID: 1946
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x0400079B RID: 1947
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x0400079C RID: 1948
			private bool hasCurrent4;

			// Token: 0x0400079D RID: 1949
			private bool running4;

			// Token: 0x0400079E RID: 1950
			private T4 current4;

			// Token: 0x0400079F RID: 1951
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x040007A0 RID: 1952
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x040007A1 RID: 1953
			private bool hasCurrent5;

			// Token: 0x040007A2 RID: 1954
			private bool running5;

			// Token: 0x040007A3 RID: 1955
			private T5 current5;

			// Token: 0x040007A4 RID: 1956
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x040007A5 RID: 1957
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x040007A6 RID: 1958
			private bool hasCurrent6;

			// Token: 0x040007A7 RID: 1959
			private bool running6;

			// Token: 0x040007A8 RID: 1960
			private T6 current6;

			// Token: 0x040007A9 RID: 1961
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x040007AA RID: 1962
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x040007AB RID: 1963
			private bool hasCurrent7;

			// Token: 0x040007AC RID: 1964
			private bool running7;

			// Token: 0x040007AD RID: 1965
			private T7 current7;

			// Token: 0x040007AE RID: 1966
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x040007AF RID: 1967
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x040007B0 RID: 1968
			private bool hasCurrent8;

			// Token: 0x040007B1 RID: 1969
			private bool running8;

			// Token: 0x040007B2 RID: 1970
			private T8 current8;

			// Token: 0x040007B3 RID: 1971
			private IUniTaskAsyncEnumerator<T9> enumerator9;

			// Token: 0x040007B4 RID: 1972
			private UniTask<bool>.Awaiter awaiter9;

			// Token: 0x040007B5 RID: 1973
			private bool hasCurrent9;

			// Token: 0x040007B6 RID: 1974
			private bool running9;

			// Token: 0x040007B7 RID: 1975
			private T9 current9;

			// Token: 0x040007B8 RID: 1976
			private IUniTaskAsyncEnumerator<T10> enumerator10;

			// Token: 0x040007B9 RID: 1977
			private UniTask<bool>.Awaiter awaiter10;

			// Token: 0x040007BA RID: 1978
			private bool hasCurrent10;

			// Token: 0x040007BB RID: 1979
			private bool running10;

			// Token: 0x040007BC RID: 1980
			private T10 current10;

			// Token: 0x040007BD RID: 1981
			private IUniTaskAsyncEnumerator<T11> enumerator11;

			// Token: 0x040007BE RID: 1982
			private UniTask<bool>.Awaiter awaiter11;

			// Token: 0x040007BF RID: 1983
			private bool hasCurrent11;

			// Token: 0x040007C0 RID: 1984
			private bool running11;

			// Token: 0x040007C1 RID: 1985
			private T11 current11;

			// Token: 0x040007C2 RID: 1986
			private IUniTaskAsyncEnumerator<T12> enumerator12;

			// Token: 0x040007C3 RID: 1987
			private UniTask<bool>.Awaiter awaiter12;

			// Token: 0x040007C4 RID: 1988
			private bool hasCurrent12;

			// Token: 0x040007C5 RID: 1989
			private bool running12;

			// Token: 0x040007C6 RID: 1990
			private T12 current12;

			// Token: 0x040007C7 RID: 1991
			private IUniTaskAsyncEnumerator<T13> enumerator13;

			// Token: 0x040007C8 RID: 1992
			private UniTask<bool>.Awaiter awaiter13;

			// Token: 0x040007C9 RID: 1993
			private bool hasCurrent13;

			// Token: 0x040007CA RID: 1994
			private bool running13;

			// Token: 0x040007CB RID: 1995
			private T13 current13;

			// Token: 0x040007CC RID: 1996
			private IUniTaskAsyncEnumerator<T14> enumerator14;

			// Token: 0x040007CD RID: 1997
			private UniTask<bool>.Awaiter awaiter14;

			// Token: 0x040007CE RID: 1998
			private bool hasCurrent14;

			// Token: 0x040007CF RID: 1999
			private bool running14;

			// Token: 0x040007D0 RID: 2000
			private T14 current14;

			// Token: 0x040007D1 RID: 2001
			private int completedCount;

			// Token: 0x040007D2 RID: 2002
			private bool syncRunning;

			// Token: 0x040007D3 RID: 2003
			private TResult result;
		}
	}
}

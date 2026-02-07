using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000017 RID: 23
	internal class CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600024D RID: 589 RVA: 0x0000899C File Offset: 0x00006B9C
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector)
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
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008A0C File Offset: 0x00006C0C
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.source5, this.source6, this.source7, this.source8, this.source9, this.source10, this.source11, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000058 RID: 88
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000059 RID: 89
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x0400005A RID: 90
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x0400005B RID: 91
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x0400005C RID: 92
		private readonly IUniTaskAsyncEnumerable<T5> source5;

		// Token: 0x0400005D RID: 93
		private readonly IUniTaskAsyncEnumerable<T6> source6;

		// Token: 0x0400005E RID: 94
		private readonly IUniTaskAsyncEnumerable<T7> source7;

		// Token: 0x0400005F RID: 95
		private readonly IUniTaskAsyncEnumerable<T8> source8;

		// Token: 0x04000060 RID: 96
		private readonly IUniTaskAsyncEnumerable<T9> source9;

		// Token: 0x04000061 RID: 97
		private readonly IUniTaskAsyncEnumerable<T10> source10;

		// Token: 0x04000062 RID: 98
		private readonly IUniTaskAsyncEnumerable<T11> source11;

		// Token: 0x04000063 RID: 99
		private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector;

		// Token: 0x020000E8 RID: 232
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600050A RID: 1290 RVA: 0x0001DBFC File Offset: 0x0001BDFC
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, IUniTaskAsyncEnumerable<T5> source5, IUniTaskAsyncEnumerable<T6> source6, IUniTaskAsyncEnumerable<T7> source7, IUniTaskAsyncEnumerable<T8> source8, IUniTaskAsyncEnumerable<T9> source9, IUniTaskAsyncEnumerable<T10> source10, IUniTaskAsyncEnumerable<T11> source11, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector, CancellationToken cancellationToken)
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
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x0600050B RID: 1291 RVA: 0x0001DC74 File Offset: 0x0001BE74
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x0600050C RID: 1292 RVA: 0x0001DC7C File Offset: 0x0001BE7C
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 11)
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
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
					if (!this.running5)
					{
						this.running5 = true;
						this.awaiter5 = this.enumerator5.MoveNextAsync().GetAwaiter();
						if (this.awaiter5.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed5(this);
						}
						else
						{
							this.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed5Delegate, this);
						}
					}
					if (!this.running6)
					{
						this.running6 = true;
						this.awaiter6 = this.enumerator6.MoveNextAsync().GetAwaiter();
						if (this.awaiter6.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed6(this);
						}
						else
						{
							this.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed6Delegate, this);
						}
					}
					if (!this.running7)
					{
						this.running7 = true;
						this.awaiter7 = this.enumerator7.MoveNextAsync().GetAwaiter();
						if (this.awaiter7.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed7(this);
						}
						else
						{
							this.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed7Delegate, this);
						}
					}
					if (!this.running8)
					{
						this.running8 = true;
						this.awaiter8 = this.enumerator8.MoveNextAsync().GetAwaiter();
						if (this.awaiter8.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed8(this);
						}
						else
						{
							this.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed8Delegate, this);
						}
					}
					if (!this.running9)
					{
						this.running9 = true;
						this.awaiter9 = this.enumerator9.MoveNextAsync().GetAwaiter();
						if (this.awaiter9.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed9(this);
						}
						else
						{
							this.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed9Delegate, this);
						}
					}
					if (!this.running10)
					{
						this.running10 = true;
						this.awaiter10 = this.enumerator10.MoveNextAsync().GetAwaiter();
						if (this.awaiter10.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed10(this);
						}
						else
						{
							this.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed10Delegate, this);
						}
					}
					if (!this.running11)
					{
						this.running11 = true;
						this.awaiter11 = this.enumerator11.MoveNextAsync().GetAwaiter();
						if (this.awaiter11.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed11(this);
						}
						else
						{
							this.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed11Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4 || !this.running5 || !this.running6 || !this.running7 || !this.running8 || !this.running9 || !this.running10 || !this.running11);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x0600050D RID: 1293 RVA: 0x0001E1AC File Offset: 0x0001C3AC
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600050E RID: 1294 RVA: 0x0001E2B4 File Offset: 0x0001C4B4
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x0600050F RID: 1295 RVA: 0x0001E3BC File Offset: 0x0001C5BC
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000510 RID: 1296 RVA: 0x0001E4C4 File Offset: 0x0001C6C4
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000511 RID: 1297 RVA: 0x0001E5CC File Offset: 0x0001C7CC
			private static void Completed5(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running5 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter5.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed5Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000512 RID: 1298 RVA: 0x0001E6D4 File Offset: 0x0001C8D4
			private static void Completed6(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running6 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter6.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed6Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000513 RID: 1299 RVA: 0x0001E7DC File Offset: 0x0001C9DC
			private static void Completed7(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running7 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter7.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed7Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x0001E8E4 File Offset: 0x0001CAE4
			private static void Completed8(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running8 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter8.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed8Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000515 RID: 1301 RVA: 0x0001E9EC File Offset: 0x0001CBEC
			private static void Completed9(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running9 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter9.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed9Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000516 RID: 1302 RVA: 0x0001EAF4 File Offset: 0x0001CCF4
			private static void Completed10(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running10 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter10.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed10Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000517 RID: 1303 RVA: 0x0001EBFC File Offset: 0x0001CDFC
			private static void Completed11(object state)
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 11)
						{
							goto IL_D1;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running11 = true;
					combineLatest.completedCount = 11;
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
						combineLatest.completedCount = 11;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter11.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed11Delegate, combineLatest);
				}
				return;
				IL_D1:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x06000518 RID: 1304 RVA: 0x0001ED04 File Offset: 0x0001CF04
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4 && this.hasCurrent5 && this.hasCurrent6 && this.hasCurrent7 && this.hasCurrent8 && this.hasCurrent9 && this.hasCurrent10 && this.hasCurrent11)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4, this.current5, this.current6, this.current7, this.current8, this.current9, this.current10, this.current11);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x06000519 RID: 1305 RVA: 0x0001EDE4 File Offset: 0x0001CFE4
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.<DisposeAsync>d__99 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.<DisposeAsync>d__99>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x0400065E RID: 1630
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed1);

			// Token: 0x0400065F RID: 1631
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed2);

			// Token: 0x04000660 RID: 1632
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed3);

			// Token: 0x04000661 RID: 1633
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed4);

			// Token: 0x04000662 RID: 1634
			private static readonly Action<object> Completed5Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed5);

			// Token: 0x04000663 RID: 1635
			private static readonly Action<object> Completed6Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed6);

			// Token: 0x04000664 RID: 1636
			private static readonly Action<object> Completed7Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed7);

			// Token: 0x04000665 RID: 1637
			private static readonly Action<object> Completed8Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed8);

			// Token: 0x04000666 RID: 1638
			private static readonly Action<object> Completed9Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed9);

			// Token: 0x04000667 RID: 1639
			private static readonly Action<object> Completed10Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed10);

			// Token: 0x04000668 RID: 1640
			private static readonly Action<object> Completed11Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>._CombineLatest.Completed11);

			// Token: 0x04000669 RID: 1641
			private const int CompleteCount = 11;

			// Token: 0x0400066A RID: 1642
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x0400066B RID: 1643
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x0400066C RID: 1644
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x0400066D RID: 1645
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x0400066E RID: 1646
			private readonly IUniTaskAsyncEnumerable<T5> source5;

			// Token: 0x0400066F RID: 1647
			private readonly IUniTaskAsyncEnumerable<T6> source6;

			// Token: 0x04000670 RID: 1648
			private readonly IUniTaskAsyncEnumerable<T7> source7;

			// Token: 0x04000671 RID: 1649
			private readonly IUniTaskAsyncEnumerable<T8> source8;

			// Token: 0x04000672 RID: 1650
			private readonly IUniTaskAsyncEnumerable<T9> source9;

			// Token: 0x04000673 RID: 1651
			private readonly IUniTaskAsyncEnumerable<T10> source10;

			// Token: 0x04000674 RID: 1652
			private readonly IUniTaskAsyncEnumerable<T11> source11;

			// Token: 0x04000675 RID: 1653
			private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> resultSelector;

			// Token: 0x04000676 RID: 1654
			private CancellationToken cancellationToken;

			// Token: 0x04000677 RID: 1655
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x04000678 RID: 1656
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x04000679 RID: 1657
			private bool hasCurrent1;

			// Token: 0x0400067A RID: 1658
			private bool running1;

			// Token: 0x0400067B RID: 1659
			private T1 current1;

			// Token: 0x0400067C RID: 1660
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x0400067D RID: 1661
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x0400067E RID: 1662
			private bool hasCurrent2;

			// Token: 0x0400067F RID: 1663
			private bool running2;

			// Token: 0x04000680 RID: 1664
			private T2 current2;

			// Token: 0x04000681 RID: 1665
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x04000682 RID: 1666
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x04000683 RID: 1667
			private bool hasCurrent3;

			// Token: 0x04000684 RID: 1668
			private bool running3;

			// Token: 0x04000685 RID: 1669
			private T3 current3;

			// Token: 0x04000686 RID: 1670
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x04000687 RID: 1671
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x04000688 RID: 1672
			private bool hasCurrent4;

			// Token: 0x04000689 RID: 1673
			private bool running4;

			// Token: 0x0400068A RID: 1674
			private T4 current4;

			// Token: 0x0400068B RID: 1675
			private IUniTaskAsyncEnumerator<T5> enumerator5;

			// Token: 0x0400068C RID: 1676
			private UniTask<bool>.Awaiter awaiter5;

			// Token: 0x0400068D RID: 1677
			private bool hasCurrent5;

			// Token: 0x0400068E RID: 1678
			private bool running5;

			// Token: 0x0400068F RID: 1679
			private T5 current5;

			// Token: 0x04000690 RID: 1680
			private IUniTaskAsyncEnumerator<T6> enumerator6;

			// Token: 0x04000691 RID: 1681
			private UniTask<bool>.Awaiter awaiter6;

			// Token: 0x04000692 RID: 1682
			private bool hasCurrent6;

			// Token: 0x04000693 RID: 1683
			private bool running6;

			// Token: 0x04000694 RID: 1684
			private T6 current6;

			// Token: 0x04000695 RID: 1685
			private IUniTaskAsyncEnumerator<T7> enumerator7;

			// Token: 0x04000696 RID: 1686
			private UniTask<bool>.Awaiter awaiter7;

			// Token: 0x04000697 RID: 1687
			private bool hasCurrent7;

			// Token: 0x04000698 RID: 1688
			private bool running7;

			// Token: 0x04000699 RID: 1689
			private T7 current7;

			// Token: 0x0400069A RID: 1690
			private IUniTaskAsyncEnumerator<T8> enumerator8;

			// Token: 0x0400069B RID: 1691
			private UniTask<bool>.Awaiter awaiter8;

			// Token: 0x0400069C RID: 1692
			private bool hasCurrent8;

			// Token: 0x0400069D RID: 1693
			private bool running8;

			// Token: 0x0400069E RID: 1694
			private T8 current8;

			// Token: 0x0400069F RID: 1695
			private IUniTaskAsyncEnumerator<T9> enumerator9;

			// Token: 0x040006A0 RID: 1696
			private UniTask<bool>.Awaiter awaiter9;

			// Token: 0x040006A1 RID: 1697
			private bool hasCurrent9;

			// Token: 0x040006A2 RID: 1698
			private bool running9;

			// Token: 0x040006A3 RID: 1699
			private T9 current9;

			// Token: 0x040006A4 RID: 1700
			private IUniTaskAsyncEnumerator<T10> enumerator10;

			// Token: 0x040006A5 RID: 1701
			private UniTask<bool>.Awaiter awaiter10;

			// Token: 0x040006A6 RID: 1702
			private bool hasCurrent10;

			// Token: 0x040006A7 RID: 1703
			private bool running10;

			// Token: 0x040006A8 RID: 1704
			private T10 current10;

			// Token: 0x040006A9 RID: 1705
			private IUniTaskAsyncEnumerator<T11> enumerator11;

			// Token: 0x040006AA RID: 1706
			private UniTask<bool>.Awaiter awaiter11;

			// Token: 0x040006AB RID: 1707
			private bool hasCurrent11;

			// Token: 0x040006AC RID: 1708
			private bool running11;

			// Token: 0x040006AD RID: 1709
			private T11 current11;

			// Token: 0x040006AE RID: 1710
			private int completedCount;

			// Token: 0x040006AF RID: 1711
			private bool syncRunning;

			// Token: 0x040006B0 RID: 1712
			private TResult result;
		}
	}
}

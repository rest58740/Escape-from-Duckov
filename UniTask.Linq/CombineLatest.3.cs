using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000010 RID: 16
	internal class CombineLatest<T1, T2, T3, T4, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600023F RID: 575 RVA: 0x000085D1 File Offset: 0x000067D1
		public CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector)
		{
			this.source1 = source1;
			this.source2 = source2;
			this.source3 = source3;
			this.source4 = source4;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000085FE File Offset: 0x000067FE
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest(this.source1, this.source2, this.source3, this.source4, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000020 RID: 32
		private readonly IUniTaskAsyncEnumerable<T1> source1;

		// Token: 0x04000021 RID: 33
		private readonly IUniTaskAsyncEnumerable<T2> source2;

		// Token: 0x04000022 RID: 34
		private readonly IUniTaskAsyncEnumerable<T3> source3;

		// Token: 0x04000023 RID: 35
		private readonly IUniTaskAsyncEnumerable<T4> source4;

		// Token: 0x04000024 RID: 36
		private readonly Func<T1, T2, T3, T4, TResult> resultSelector;

		// Token: 0x020000E1 RID: 225
		private class _CombineLatest : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x060004AF RID: 1199 RVA: 0x000185C8 File Offset: 0x000167C8
			public _CombineLatest(IUniTaskAsyncEnumerable<T1> source1, IUniTaskAsyncEnumerable<T2> source2, IUniTaskAsyncEnumerable<T3> source3, IUniTaskAsyncEnumerable<T4> source4, Func<T1, T2, T3, T4, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source1 = source1;
				this.source2 = source2;
				this.source3 = source3;
				this.source4 = source4;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700000B RID: 11
			// (get) Token: 0x060004B0 RID: 1200 RVA: 0x000185FD File Offset: 0x000167FD
			public TResult Current
			{
				get
				{
					return this.result;
				}
			}

			// Token: 0x060004B1 RID: 1201 RVA: 0x00018608 File Offset: 0x00016808
			public UniTask<bool> MoveNextAsync()
			{
				this.cancellationToken.ThrowIfCancellationRequested();
				if (this.completedCount == 4)
				{
					return CompletedTasks.False;
				}
				if (this.enumerator1 == null)
				{
					this.enumerator1 = this.source1.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator2 = this.source2.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator3 = this.source3.GetAsyncEnumerator(this.cancellationToken);
					this.enumerator4 = this.source4.GetAsyncEnumerator(this.cancellationToken);
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
							CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed1(this);
						}
						else
						{
							this.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed1Delegate, this);
						}
					}
					if (!this.running2)
					{
						this.running2 = true;
						this.awaiter2 = this.enumerator2.MoveNextAsync().GetAwaiter();
						if (this.awaiter2.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed2(this);
						}
						else
						{
							this.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed2Delegate, this);
						}
					}
					if (!this.running3)
					{
						this.running3 = true;
						this.awaiter3 = this.enumerator3.MoveNextAsync().GetAwaiter();
						if (this.awaiter3.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed3(this);
						}
						else
						{
							this.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed3Delegate, this);
						}
					}
					if (!this.running4)
					{
						this.running4 = true;
						this.awaiter4 = this.enumerator4.MoveNextAsync().GetAwaiter();
						if (this.awaiter4.IsCompleted)
						{
							CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed4(this);
						}
						else
						{
							this.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed4Delegate, this);
						}
					}
				}
				while (!this.running1 || !this.running2 || !this.running3 || !this.running4);
				this.syncRunning = false;
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060004B2 RID: 1202 RVA: 0x00018824 File Offset: 0x00016A24
			private static void Completed1(object state)
			{
				CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 4)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running1 = true;
					combineLatest.completedCount = 4;
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
						combineLatest.completedCount = 4;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter1.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed1Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004B3 RID: 1203 RVA: 0x00018928 File Offset: 0x00016B28
			private static void Completed2(object state)
			{
				CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 4)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running2 = true;
					combineLatest.completedCount = 4;
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
						combineLatest.completedCount = 4;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter2.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed2Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004B4 RID: 1204 RVA: 0x00018A2C File Offset: 0x00016C2C
			private static void Completed3(object state)
			{
				CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 4)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running3 = true;
					combineLatest.completedCount = 4;
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
						combineLatest.completedCount = 4;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter3.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed3Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004B5 RID: 1205 RVA: 0x00018B30 File Offset: 0x00016D30
			private static void Completed4(object state)
			{
				CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest combineLatest = (CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest)state;
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
						if (Interlocked.Increment(ref combineLatest.completedCount) == 4)
						{
							goto IL_CB;
						}
						return;
					}
				}
				catch (Exception ex)
				{
					combineLatest.running4 = true;
					combineLatest.completedCount = 4;
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
						combineLatest.completedCount = 4;
						combineLatest.completionSource.TrySetException(ex2);
						return;
					}
					combineLatest.awaiter4.SourceOnCompleted(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed4Delegate, combineLatest);
				}
				return;
				IL_CB:
				combineLatest.completionSource.TrySetResult(false);
			}

			// Token: 0x060004B6 RID: 1206 RVA: 0x00018C34 File Offset: 0x00016E34
			private bool TrySetResult()
			{
				if (this.hasCurrent1 && this.hasCurrent2 && this.hasCurrent3 && this.hasCurrent4)
				{
					this.result = this.resultSelector(this.current1, this.current2, this.current3, this.current4);
					this.completionSource.TrySetResult(true);
					return true;
				}
				return false;
			}

			// Token: 0x060004B7 RID: 1207 RVA: 0x00018C9C File Offset: 0x00016E9C
			public UniTask DisposeAsync()
			{
				CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.<DisposeAsync>d__43 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.<DisposeAsync>d__43>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x040004DD RID: 1245
			private static readonly Action<object> Completed1Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed1);

			// Token: 0x040004DE RID: 1246
			private static readonly Action<object> Completed2Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed2);

			// Token: 0x040004DF RID: 1247
			private static readonly Action<object> Completed3Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed3);

			// Token: 0x040004E0 RID: 1248
			private static readonly Action<object> Completed4Delegate = new Action<object>(CombineLatest<T1, T2, T3, T4, TResult>._CombineLatest.Completed4);

			// Token: 0x040004E1 RID: 1249
			private const int CompleteCount = 4;

			// Token: 0x040004E2 RID: 1250
			private readonly IUniTaskAsyncEnumerable<T1> source1;

			// Token: 0x040004E3 RID: 1251
			private readonly IUniTaskAsyncEnumerable<T2> source2;

			// Token: 0x040004E4 RID: 1252
			private readonly IUniTaskAsyncEnumerable<T3> source3;

			// Token: 0x040004E5 RID: 1253
			private readonly IUniTaskAsyncEnumerable<T4> source4;

			// Token: 0x040004E6 RID: 1254
			private readonly Func<T1, T2, T3, T4, TResult> resultSelector;

			// Token: 0x040004E7 RID: 1255
			private CancellationToken cancellationToken;

			// Token: 0x040004E8 RID: 1256
			private IUniTaskAsyncEnumerator<T1> enumerator1;

			// Token: 0x040004E9 RID: 1257
			private UniTask<bool>.Awaiter awaiter1;

			// Token: 0x040004EA RID: 1258
			private bool hasCurrent1;

			// Token: 0x040004EB RID: 1259
			private bool running1;

			// Token: 0x040004EC RID: 1260
			private T1 current1;

			// Token: 0x040004ED RID: 1261
			private IUniTaskAsyncEnumerator<T2> enumerator2;

			// Token: 0x040004EE RID: 1262
			private UniTask<bool>.Awaiter awaiter2;

			// Token: 0x040004EF RID: 1263
			private bool hasCurrent2;

			// Token: 0x040004F0 RID: 1264
			private bool running2;

			// Token: 0x040004F1 RID: 1265
			private T2 current2;

			// Token: 0x040004F2 RID: 1266
			private IUniTaskAsyncEnumerator<T3> enumerator3;

			// Token: 0x040004F3 RID: 1267
			private UniTask<bool>.Awaiter awaiter3;

			// Token: 0x040004F4 RID: 1268
			private bool hasCurrent3;

			// Token: 0x040004F5 RID: 1269
			private bool running3;

			// Token: 0x040004F6 RID: 1270
			private T3 current3;

			// Token: 0x040004F7 RID: 1271
			private IUniTaskAsyncEnumerator<T4> enumerator4;

			// Token: 0x040004F8 RID: 1272
			private UniTask<bool>.Awaiter awaiter4;

			// Token: 0x040004F9 RID: 1273
			private bool hasCurrent4;

			// Token: 0x040004FA RID: 1274
			private bool running4;

			// Token: 0x040004FB RID: 1275
			private T4 current4;

			// Token: 0x040004FC RID: 1276
			private int completedCount;

			// Token: 0x040004FD RID: 1277
			private bool syncRunning;

			// Token: 0x040004FE RID: 1278
			private TResult result;
		}
	}
}

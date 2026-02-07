using System;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000061 RID: 97
	internal sealed class SkipUntilCanceled<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0000C4ED File Offset: 0x0000A6ED
		public SkipUntilCanceled(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			this.source = source;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0000C503 File Offset: 0x0000A703
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SkipUntilCanceled<TSource>._SkipUntilCanceled(this.source, this.cancellationToken, cancellationToken);
		}

		// Token: 0x04000150 RID: 336
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000151 RID: 337
		private readonly CancellationToken cancellationToken;

		// Token: 0x0200019C RID: 412
		private sealed class _SkipUntilCanceled : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060007C6 RID: 1990 RVA: 0x00041BC4 File Offset: 0x0003FDC4
			public _SkipUntilCanceled(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken1, CancellationToken cancellationToken2)
			{
				this.source = source;
				this.cancellationToken1 = cancellationToken1;
				this.cancellationToken2 = cancellationToken2;
				if (cancellationToken1.CanBeCanceled)
				{
					this.cancellationTokenRegistration1 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken1, SkipUntilCanceled<TSource>._SkipUntilCanceled.CancelDelegate1, this);
				}
				if (cancellationToken1 != cancellationToken2 && cancellationToken2.CanBeCanceled)
				{
					this.cancellationTokenRegistration2 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken2, SkipUntilCanceled<TSource>._SkipUntilCanceled.CancelDelegate2, this);
				}
			}

			// Token: 0x17000043 RID: 67
			// (get) Token: 0x060007C7 RID: 1991 RVA: 0x00041C2B File Offset: 0x0003FE2B
			// (set) Token: 0x060007C8 RID: 1992 RVA: 0x00041C33 File Offset: 0x0003FE33
			public TSource Current { get; private set; }

			// Token: 0x060007C9 RID: 1993 RVA: 0x00041C3C File Offset: 0x0003FE3C
			public UniTask<bool> MoveNextAsync()
			{
				if (this.enumerator == null)
				{
					if (this.cancellationToken1.IsCancellationRequested)
					{
						this.isCanceled = 1;
					}
					if (this.cancellationToken2.IsCancellationRequested)
					{
						this.isCanceled = 1;
					}
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken2);
				}
				this.completionSource.Reset();
				if (this.isCanceled != 0)
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060007CA RID: 1994 RVA: 0x00041CBC File Offset: 0x0003FEBC
			private void SourceMoveNext()
			{
				try
				{
					for (;;)
					{
						this.awaiter = this.enumerator.MoveNextAsync().GetAwaiter();
						if (!this.awaiter.IsCompleted)
						{
							break;
						}
						this.continueNext = true;
						SkipUntilCanceled<TSource>._SkipUntilCanceled.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_55;
						}
						this.continueNext = false;
					}
					this.awaiter.SourceOnCompleted(SkipUntilCanceled<TSource>._SkipUntilCanceled.MoveNextCoreDelegate, this);
					IL_55:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x060007CB RID: 1995 RVA: 0x00041D40 File Offset: 0x0003FF40
			private static void MoveNextCore(object state)
			{
				SkipUntilCanceled<TSource>._SkipUntilCanceled skipUntilCanceled = (SkipUntilCanceled<TSource>._SkipUntilCanceled)state;
				bool flag;
				if (skipUntilCanceled.TryGetResult<bool>(skipUntilCanceled.awaiter, ref flag))
				{
					if (flag)
					{
						skipUntilCanceled.Current = skipUntilCanceled.enumerator.Current;
						skipUntilCanceled.completionSource.TrySetResult(true);
						if (skipUntilCanceled.continueNext)
						{
							skipUntilCanceled.SourceMoveNext();
							return;
						}
					}
					else
					{
						skipUntilCanceled.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x060007CC RID: 1996 RVA: 0x00041DA4 File Offset: 0x0003FFA4
			private static void OnCanceled1(object state)
			{
				SkipUntilCanceled<TSource>._SkipUntilCanceled skipUntilCanceled = (SkipUntilCanceled<TSource>._SkipUntilCanceled)state;
				if (skipUntilCanceled.isCanceled == 0 && Interlocked.Increment(ref skipUntilCanceled.isCanceled) == 1)
				{
					skipUntilCanceled.cancellationTokenRegistration2.Dispose();
					skipUntilCanceled.SourceMoveNext();
				}
			}

			// Token: 0x060007CD RID: 1997 RVA: 0x00041DE0 File Offset: 0x0003FFE0
			private static void OnCanceled2(object state)
			{
				SkipUntilCanceled<TSource>._SkipUntilCanceled skipUntilCanceled = (SkipUntilCanceled<TSource>._SkipUntilCanceled)state;
				if (skipUntilCanceled.isCanceled == 0 && Interlocked.Increment(ref skipUntilCanceled.isCanceled) == 1)
				{
					skipUntilCanceled.cancellationTokenRegistration2.Dispose();
					skipUntilCanceled.SourceMoveNext();
				}
			}

			// Token: 0x060007CE RID: 1998 RVA: 0x00041E1C File Offset: 0x0004001C
			public UniTask DisposeAsync()
			{
				this.cancellationTokenRegistration1.Dispose();
				this.cancellationTokenRegistration2.Dispose();
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000F8E RID: 3982
			private static readonly Action<object> CancelDelegate1 = new Action<object>(SkipUntilCanceled<TSource>._SkipUntilCanceled.OnCanceled1);

			// Token: 0x04000F8F RID: 3983
			private static readonly Action<object> CancelDelegate2 = new Action<object>(SkipUntilCanceled<TSource>._SkipUntilCanceled.OnCanceled2);

			// Token: 0x04000F90 RID: 3984
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(SkipUntilCanceled<TSource>._SkipUntilCanceled.MoveNextCore);

			// Token: 0x04000F91 RID: 3985
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000F92 RID: 3986
			private CancellationToken cancellationToken1;

			// Token: 0x04000F93 RID: 3987
			private CancellationToken cancellationToken2;

			// Token: 0x04000F94 RID: 3988
			private CancellationTokenRegistration cancellationTokenRegistration1;

			// Token: 0x04000F95 RID: 3989
			private CancellationTokenRegistration cancellationTokenRegistration2;

			// Token: 0x04000F96 RID: 3990
			private int isCanceled;

			// Token: 0x04000F97 RID: 3991
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000F98 RID: 3992
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000F99 RID: 3993
			private bool continueNext;
		}
	}
}

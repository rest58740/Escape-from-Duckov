using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000060 RID: 96
	internal sealed class SkipUntil<TSource> : IUniTaskAsyncEnumerable<TSource>
	{
		// Token: 0x0600034E RID: 846 RVA: 0x0000C49B File Offset: 0x0000A69B
		public SkipUntil(IUniTaskAsyncEnumerable<TSource> source, UniTask other, Func<CancellationToken, UniTask> other2)
		{
			this.source = source;
			this.other = other;
			this.other2 = other2;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000C4B8 File Offset: 0x0000A6B8
		public IUniTaskAsyncEnumerator<TSource> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			if (this.other2 != null)
			{
				return new SkipUntil<TSource>._SkipUntil(this.source, this.other2(cancellationToken), cancellationToken);
			}
			return new SkipUntil<TSource>._SkipUntil(this.source, this.other, cancellationToken);
		}

		// Token: 0x0400014D RID: 333
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400014E RID: 334
		private readonly UniTask other;

		// Token: 0x0400014F RID: 335
		private readonly Func<CancellationToken, UniTask> other2;

		// Token: 0x0200019B RID: 411
		private sealed class _SkipUntil : MoveNextSource, IUniTaskAsyncEnumerator<TSource>, IUniTaskAsyncDisposable
		{
			// Token: 0x060007BC RID: 1980 RVA: 0x0004192C File Offset: 0x0003FB2C
			public _SkipUntil(IUniTaskAsyncEnumerable<TSource> source, UniTask other, CancellationToken cancellationToken1)
			{
				this.source = source;
				this.cancellationToken1 = cancellationToken1;
				if (cancellationToken1.CanBeCanceled)
				{
					this.cancellationTokenRegistration1 = CancellationTokenExtensions.RegisterWithoutCaptureExecutionContext(cancellationToken1, SkipUntil<TSource>._SkipUntil.CancelDelegate1, this);
				}
				this.RunOther(other).Forget();
			}

			// Token: 0x17000042 RID: 66
			// (get) Token: 0x060007BD RID: 1981 RVA: 0x00041977 File Offset: 0x0003FB77
			// (set) Token: 0x060007BE RID: 1982 RVA: 0x0004197F File Offset: 0x0003FB7F
			public TSource Current { get; private set; }

			// Token: 0x060007BF RID: 1983 RVA: 0x00041988 File Offset: 0x0003FB88
			public UniTask<bool> MoveNextAsync()
			{
				if (this.exception != null)
				{
					return UniTask.FromException<bool>(this.exception);
				}
				if (this.cancellationToken1.IsCancellationRequested)
				{
					return UniTask.FromCanceled<bool>(this.cancellationToken1);
				}
				if (this.enumerator == null)
				{
					this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken1);
				}
				this.completionSource.Reset();
				if (this.completed)
				{
					this.SourceMoveNext();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x00041A0C File Offset: 0x0003FC0C
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
						SkipUntil<TSource>._SkipUntil.MoveNextCore(this);
						if (!this.continueNext)
						{
							goto IL_55;
						}
						this.continueNext = false;
					}
					this.awaiter.SourceOnCompleted(SkipUntil<TSource>._SkipUntil.MoveNextCoreDelegate, this);
					IL_55:;
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
				}
			}

			// Token: 0x060007C1 RID: 1985 RVA: 0x00041A90 File Offset: 0x0003FC90
			private static void MoveNextCore(object state)
			{
				SkipUntil<TSource>._SkipUntil skipUntil = (SkipUntil<TSource>._SkipUntil)state;
				bool flag;
				if (skipUntil.TryGetResult<bool>(skipUntil.awaiter, ref flag))
				{
					if (flag)
					{
						skipUntil.Current = skipUntil.enumerator.Current;
						skipUntil.completionSource.TrySetResult(true);
						if (skipUntil.continueNext)
						{
							skipUntil.SourceMoveNext();
							return;
						}
					}
					else
					{
						skipUntil.completionSource.TrySetResult(false);
					}
				}
			}

			// Token: 0x060007C2 RID: 1986 RVA: 0x00041AF4 File Offset: 0x0003FCF4
			private UniTaskVoid RunOther(UniTask other)
			{
				SkipUntil<TSource>._SkipUntil.<RunOther>d__18 <RunOther>d__;
				<RunOther>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
				<RunOther>d__.<>4__this = this;
				<RunOther>d__.other = other;
				<RunOther>d__.<>1__state = -1;
				<RunOther>d__.<>t__builder.Start<SkipUntil<TSource>._SkipUntil.<RunOther>d__18>(ref <RunOther>d__);
				return <RunOther>d__.<>t__builder.Task;
			}

			// Token: 0x060007C3 RID: 1987 RVA: 0x00041B40 File Offset: 0x0003FD40
			private static void OnCanceled1(object state)
			{
				SkipUntil<TSource>._SkipUntil skipUntil = (SkipUntil<TSource>._SkipUntil)state;
				skipUntil.completionSource.TrySetCanceled(skipUntil.cancellationToken1);
			}

			// Token: 0x060007C4 RID: 1988 RVA: 0x00041B68 File Offset: 0x0003FD68
			public UniTask DisposeAsync()
			{
				this.cancellationTokenRegistration1.Dispose();
				if (this.enumerator != null)
				{
					return this.enumerator.DisposeAsync();
				}
				return default(UniTask);
			}

			// Token: 0x04000F83 RID: 3971
			private static readonly Action<object> CancelDelegate1 = new Action<object>(SkipUntil<TSource>._SkipUntil.OnCanceled1);

			// Token: 0x04000F84 RID: 3972
			private static readonly Action<object> MoveNextCoreDelegate = new Action<object>(SkipUntil<TSource>._SkipUntil.MoveNextCore);

			// Token: 0x04000F85 RID: 3973
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000F86 RID: 3974
			private CancellationToken cancellationToken1;

			// Token: 0x04000F87 RID: 3975
			private bool completed;

			// Token: 0x04000F88 RID: 3976
			private CancellationTokenRegistration cancellationTokenRegistration1;

			// Token: 0x04000F89 RID: 3977
			private IUniTaskAsyncEnumerator<TSource> enumerator;

			// Token: 0x04000F8A RID: 3978
			private UniTask<bool>.Awaiter awaiter;

			// Token: 0x04000F8B RID: 3979
			private bool continueNext;

			// Token: 0x04000F8C RID: 3980
			private Exception exception;
		}
	}
}

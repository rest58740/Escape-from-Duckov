using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000059 RID: 89
	internal sealed class SelectMany<TSource, TCollection, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000C149 File Offset: 0x0000A349
		public SelectMany(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, IUniTaskAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
		{
			this.source = source;
			this.selector1 = selector;
			this.selector2 = null;
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000C16D File Offset: 0x0000A36D
		public SelectMany(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> selector, Func<TSource, TCollection, TResult> resultSelector)
		{
			this.source = source;
			this.selector1 = null;
			this.selector2 = selector;
			this.resultSelector = resultSelector;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000C191 File Offset: 0x0000A391
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectMany<TSource, TCollection, TResult>._SelectMany(this.source, this.selector1, this.selector2, this.resultSelector, cancellationToken);
		}

		// Token: 0x0400013D RID: 317
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400013E RID: 318
		private readonly Func<TSource, IUniTaskAsyncEnumerable<TCollection>> selector1;

		// Token: 0x0400013F RID: 319
		private readonly Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> selector2;

		// Token: 0x04000140 RID: 320
		private readonly Func<TSource, TCollection, TResult> resultSelector;

		// Token: 0x02000191 RID: 401
		private sealed class _SelectMany : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x06000783 RID: 1923 RVA: 0x0003F9ED File Offset: 0x0003DBED
			public _SelectMany(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, IUniTaskAsyncEnumerable<TCollection>> selector1, Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> selector2, Func<TSource, TCollection, TResult> resultSelector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector1 = selector1;
				this.selector2 = selector2;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700003E RID: 62
			// (get) Token: 0x06000784 RID: 1924 RVA: 0x0003FA1A File Offset: 0x0003DC1A
			// (set) Token: 0x06000785 RID: 1925 RVA: 0x0003FA22 File Offset: 0x0003DC22
			public TResult Current { get; private set; }

			// Token: 0x06000786 RID: 1926 RVA: 0x0003FA2C File Offset: 0x0003DC2C
			public UniTask<bool> MoveNextAsync()
			{
				this.completionSource.Reset();
				if (this.selectedEnumerator != null)
				{
					this.MoveNextSelected();
				}
				else
				{
					if (this.sourceEnumerator == null)
					{
						this.sourceEnumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
					}
					this.MoveNextSource();
				}
				return new UniTask<bool>(this, this.completionSource.Version);
			}

			// Token: 0x06000787 RID: 1927 RVA: 0x0003FA8C File Offset: 0x0003DC8C
			private void MoveNextSource()
			{
				try
				{
					this.sourceAwaiter = this.sourceEnumerator.MoveNextAsync().GetAwaiter();
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
					return;
				}
				if (this.sourceAwaiter.IsCompleted)
				{
					SelectMany<TSource, TCollection, TResult>._SelectMany.SourceMoveNextCore(this);
					return;
				}
				this.sourceAwaiter.SourceOnCompleted(SelectMany<TSource, TCollection, TResult>._SelectMany.sourceMoveNextCoreDelegate, this);
			}

			// Token: 0x06000788 RID: 1928 RVA: 0x0003FAFC File Offset: 0x0003DCFC
			private void MoveNextSelected()
			{
				try
				{
					this.selectedAwaiter = this.selectedEnumerator.MoveNextAsync().GetAwaiter();
				}
				catch (Exception ex)
				{
					this.completionSource.TrySetException(ex);
					return;
				}
				if (this.selectedAwaiter.IsCompleted)
				{
					SelectMany<TSource, TCollection, TResult>._SelectMany.SeletedSourceMoveNextCore(this);
					return;
				}
				this.selectedAwaiter.SourceOnCompleted(SelectMany<TSource, TCollection, TResult>._SelectMany.selectedSourceMoveNextCoreDelegate, this);
			}

			// Token: 0x06000789 RID: 1929 RVA: 0x0003FB6C File Offset: 0x0003DD6C
			private static void SourceMoveNextCore(object state)
			{
				SelectMany<TSource, TCollection, TResult>._SelectMany selectMany = (SelectMany<TSource, TCollection, TResult>._SelectMany)state;
				bool flag;
				if (selectMany.TryGetResult<bool>(selectMany.sourceAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectMany.sourceCurrent = selectMany.sourceEnumerator.Current;
							if (selectMany.selector1 != null)
							{
								selectMany.selectedEnumerator = selectMany.selector1(selectMany.sourceCurrent).GetAsyncEnumerator(selectMany.cancellationToken);
							}
							else
							{
								SelectMany<TSource, TCollection, TResult>._SelectMany selectMany2 = selectMany;
								Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> func = selectMany.selector2;
								TSource arg = selectMany.sourceCurrent;
								SelectMany<TSource, TCollection, TResult>._SelectMany selectMany3 = selectMany;
								int num = selectMany3.sourceIndex;
								selectMany3.sourceIndex = checked(num + 1);
								selectMany2.selectedEnumerator = func(arg, num).GetAsyncEnumerator(selectMany.cancellationToken);
							}
						}
						catch (Exception ex)
						{
							selectMany.completionSource.TrySetException(ex);
							return;
						}
						selectMany.MoveNextSelected();
						return;
					}
					selectMany.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x0600078A RID: 1930 RVA: 0x0003FC40 File Offset: 0x0003DE40
			private static void SeletedSourceMoveNextCore(object state)
			{
				SelectMany<TSource, TCollection, TResult>._SelectMany selectMany = (SelectMany<TSource, TCollection, TResult>._SelectMany)state;
				bool flag;
				if (selectMany.TryGetResult<bool>(selectMany.selectedAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectMany.Current = selectMany.resultSelector(selectMany.sourceCurrent, selectMany.selectedEnumerator.Current);
						}
						catch (Exception ex)
						{
							selectMany.completionSource.TrySetException(ex);
							return;
						}
						selectMany.completionSource.TrySetResult(true);
						return;
					}
					try
					{
						selectMany.selectedDisposeAsyncAwaiter = selectMany.selectedEnumerator.DisposeAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						selectMany.completionSource.TrySetException(ex2);
						return;
					}
					if (selectMany.selectedDisposeAsyncAwaiter.IsCompleted)
					{
						SelectMany<TSource, TCollection, TResult>._SelectMany.SelectedEnumeratorDisposeAsyncCore(selectMany);
						return;
					}
					selectMany.selectedDisposeAsyncAwaiter.SourceOnCompleted(SelectMany<TSource, TCollection, TResult>._SelectMany.selectedEnumeratorDisposeAsyncCoreDelegate, selectMany);
				}
			}

			// Token: 0x0600078B RID: 1931 RVA: 0x0003FD1C File Offset: 0x0003DF1C
			private static void SelectedEnumeratorDisposeAsyncCore(object state)
			{
				SelectMany<TSource, TCollection, TResult>._SelectMany selectMany = (SelectMany<TSource, TCollection, TResult>._SelectMany)state;
				if (selectMany.TryGetResult(selectMany.selectedDisposeAsyncAwaiter))
				{
					selectMany.selectedEnumerator = null;
					selectMany.selectedAwaiter = default(UniTask<bool>.Awaiter);
					selectMany.MoveNextSource();
				}
			}

			// Token: 0x0600078C RID: 1932 RVA: 0x0003FD58 File Offset: 0x0003DF58
			public UniTask DisposeAsync()
			{
				SelectMany<TSource, TCollection, TResult>._SelectMany.<DisposeAsync>d__26 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<SelectMany<TSource, TCollection, TResult>._SelectMany.<DisposeAsync>d__26>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000EF8 RID: 3832
			private static readonly Action<object> sourceMoveNextCoreDelegate = new Action<object>(SelectMany<TSource, TCollection, TResult>._SelectMany.SourceMoveNextCore);

			// Token: 0x04000EF9 RID: 3833
			private static readonly Action<object> selectedSourceMoveNextCoreDelegate = new Action<object>(SelectMany<TSource, TCollection, TResult>._SelectMany.SeletedSourceMoveNextCore);

			// Token: 0x04000EFA RID: 3834
			private static readonly Action<object> selectedEnumeratorDisposeAsyncCoreDelegate = new Action<object>(SelectMany<TSource, TCollection, TResult>._SelectMany.SelectedEnumeratorDisposeAsyncCore);

			// Token: 0x04000EFB RID: 3835
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000EFC RID: 3836
			private readonly Func<TSource, IUniTaskAsyncEnumerable<TCollection>> selector1;

			// Token: 0x04000EFD RID: 3837
			private readonly Func<TSource, int, IUniTaskAsyncEnumerable<TCollection>> selector2;

			// Token: 0x04000EFE RID: 3838
			private readonly Func<TSource, TCollection, TResult> resultSelector;

			// Token: 0x04000EFF RID: 3839
			private CancellationToken cancellationToken;

			// Token: 0x04000F00 RID: 3840
			private TSource sourceCurrent;

			// Token: 0x04000F01 RID: 3841
			private int sourceIndex;

			// Token: 0x04000F02 RID: 3842
			private IUniTaskAsyncEnumerator<TSource> sourceEnumerator;

			// Token: 0x04000F03 RID: 3843
			private IUniTaskAsyncEnumerator<TCollection> selectedEnumerator;

			// Token: 0x04000F04 RID: 3844
			private UniTask<bool>.Awaiter sourceAwaiter;

			// Token: 0x04000F05 RID: 3845
			private UniTask<bool>.Awaiter selectedAwaiter;

			// Token: 0x04000F06 RID: 3846
			private UniTask.Awaiter selectedDisposeAsyncAwaiter;
		}
	}
}

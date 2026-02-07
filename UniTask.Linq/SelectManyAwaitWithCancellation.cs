using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200005B RID: 91
	internal sealed class SelectManyAwaitWithCancellation<TSource, TCollection, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000C219 File Offset: 0x0000A419
		public SelectManyAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector)
		{
			this.source = source;
			this.selector1 = selector;
			this.selector2 = null;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000C23D File Offset: 0x0000A43D
		public SelectManyAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector)
		{
			this.source = source;
			this.selector1 = null;
			this.selector2 = selector;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000C261 File Offset: 0x0000A461
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation(this.source, this.selector1, this.selector2, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000145 RID: 325
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000146 RID: 326
		private readonly Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1;

		// Token: 0x04000147 RID: 327
		private readonly Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2;

		// Token: 0x04000148 RID: 328
		private readonly Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector;

		// Token: 0x02000193 RID: 403
		private sealed class _SelectManyAwaitWithCancellation : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600079B RID: 1947 RVA: 0x00040292 File Offset: 0x0003E492
			public _SelectManyAwaitWithCancellation(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1, Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2, Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector1 = selector1;
				this.selector2 = selector2;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x17000040 RID: 64
			// (get) Token: 0x0600079C RID: 1948 RVA: 0x000402BF File Offset: 0x0003E4BF
			// (set) Token: 0x0600079D RID: 1949 RVA: 0x000402C7 File Offset: 0x0003E4C7
			public TResult Current { get; private set; }

			// Token: 0x0600079E RID: 1950 RVA: 0x000402D0 File Offset: 0x0003E4D0
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

			// Token: 0x0600079F RID: 1951 RVA: 0x00040330 File Offset: 0x0003E530
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
					SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SourceMoveNextCore(this);
					return;
				}
				this.sourceAwaiter.SourceOnCompleted(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.sourceMoveNextCoreDelegate, this);
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x000403A0 File Offset: 0x0003E5A0
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
					SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SeletedSourceMoveNextCore(this);
					return;
				}
				this.selectedAwaiter.SourceOnCompleted(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.selectedSourceMoveNextCoreDelegate, this);
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x00040410 File Offset: 0x0003E610
			private static void SourceMoveNextCore(object state)
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation = (SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation)state;
				bool flag;
				if (selectManyAwaitWithCancellation.TryGetResult<bool>(selectManyAwaitWithCancellation.sourceAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectManyAwaitWithCancellation.sourceCurrent = selectManyAwaitWithCancellation.sourceEnumerator.Current;
							if (selectManyAwaitWithCancellation.selector1 != null)
							{
								selectManyAwaitWithCancellation.collectionSelectorAwaiter = selectManyAwaitWithCancellation.selector1(selectManyAwaitWithCancellation.sourceCurrent, selectManyAwaitWithCancellation.cancellationToken).GetAwaiter();
							}
							else
							{
								SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation2 = selectManyAwaitWithCancellation;
								Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> func = selectManyAwaitWithCancellation.selector2;
								TSource arg = selectManyAwaitWithCancellation.sourceCurrent;
								SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation3 = selectManyAwaitWithCancellation;
								int num = selectManyAwaitWithCancellation3.sourceIndex;
								selectManyAwaitWithCancellation3.sourceIndex = checked(num + 1);
								selectManyAwaitWithCancellation2.collectionSelectorAwaiter = func(arg, num, selectManyAwaitWithCancellation.cancellationToken).GetAwaiter();
							}
							if (selectManyAwaitWithCancellation.collectionSelectorAwaiter.IsCompleted)
							{
								SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SelectorAwaitCore(selectManyAwaitWithCancellation);
							}
							else
							{
								selectManyAwaitWithCancellation.collectionSelectorAwaiter.SourceOnCompleted(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.selectorAwaitCoreDelegate, selectManyAwaitWithCancellation);
							}
							return;
						}
						catch (Exception ex)
						{
							selectManyAwaitWithCancellation.completionSource.TrySetException(ex);
							return;
						}
					}
					selectManyAwaitWithCancellation.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x0004050C File Offset: 0x0003E70C
			private static void SeletedSourceMoveNextCore(object state)
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation = (SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation)state;
				bool flag;
				if (selectManyAwaitWithCancellation.TryGetResult<bool>(selectManyAwaitWithCancellation.selectedAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectManyAwaitWithCancellation.resultSelectorAwaiter = selectManyAwaitWithCancellation.resultSelector(selectManyAwaitWithCancellation.sourceCurrent, selectManyAwaitWithCancellation.selectedEnumerator.Current, selectManyAwaitWithCancellation.cancellationToken).GetAwaiter();
							if (selectManyAwaitWithCancellation.resultSelectorAwaiter.IsCompleted)
							{
								SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.ResultSelectorAwaitCore(selectManyAwaitWithCancellation);
							}
							else
							{
								selectManyAwaitWithCancellation.resultSelectorAwaiter.SourceOnCompleted(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.resultSelectorAwaitCoreDelegate, selectManyAwaitWithCancellation);
							}
							return;
						}
						catch (Exception ex)
						{
							selectManyAwaitWithCancellation.completionSource.TrySetException(ex);
							return;
						}
					}
					try
					{
						selectManyAwaitWithCancellation.selectedDisposeAsyncAwaiter = selectManyAwaitWithCancellation.selectedEnumerator.DisposeAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						selectManyAwaitWithCancellation.completionSource.TrySetException(ex2);
						return;
					}
					if (selectManyAwaitWithCancellation.selectedDisposeAsyncAwaiter.IsCompleted)
					{
						SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SelectedEnumeratorDisposeAsyncCore(selectManyAwaitWithCancellation);
						return;
					}
					selectManyAwaitWithCancellation.selectedDisposeAsyncAwaiter.SourceOnCompleted(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.selectedEnumeratorDisposeAsyncCoreDelegate, selectManyAwaitWithCancellation);
				}
			}

			// Token: 0x060007A3 RID: 1955 RVA: 0x00040610 File Offset: 0x0003E810
			private static void SelectedEnumeratorDisposeAsyncCore(object state)
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation = (SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation)state;
				if (selectManyAwaitWithCancellation.TryGetResult(selectManyAwaitWithCancellation.selectedDisposeAsyncAwaiter))
				{
					selectManyAwaitWithCancellation.selectedEnumerator = null;
					selectManyAwaitWithCancellation.selectedAwaiter = default(UniTask<bool>.Awaiter);
					selectManyAwaitWithCancellation.MoveNextSource();
				}
			}

			// Token: 0x060007A4 RID: 1956 RVA: 0x0004064C File Offset: 0x0003E84C
			private static void SelectorAwaitCore(object state)
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation = (SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation)state;
				IUniTaskAsyncEnumerable<TCollection> uniTaskAsyncEnumerable;
				if (selectManyAwaitWithCancellation.TryGetResult<IUniTaskAsyncEnumerable<TCollection>>(selectManyAwaitWithCancellation.collectionSelectorAwaiter, ref uniTaskAsyncEnumerable))
				{
					selectManyAwaitWithCancellation.selectedEnumerator = uniTaskAsyncEnumerable.GetAsyncEnumerator(selectManyAwaitWithCancellation.cancellationToken);
					selectManyAwaitWithCancellation.MoveNextSelected();
				}
			}

			// Token: 0x060007A5 RID: 1957 RVA: 0x00040688 File Offset: 0x0003E888
			private static void ResultSelectorAwaitCore(object state)
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation selectManyAwaitWithCancellation = (SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation)state;
				TResult value;
				if (selectManyAwaitWithCancellation.TryGetResult<TResult>(selectManyAwaitWithCancellation.resultSelectorAwaiter, ref value))
				{
					selectManyAwaitWithCancellation.Current = value;
					selectManyAwaitWithCancellation.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x000406C0 File Offset: 0x0003E8C0
			public UniTask DisposeAsync()
			{
				SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.<DisposeAsync>d__32 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.<DisposeAsync>d__32>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000F1C RID: 3868
			private static readonly Action<object> sourceMoveNextCoreDelegate = new Action<object>(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SourceMoveNextCore);

			// Token: 0x04000F1D RID: 3869
			private static readonly Action<object> selectedSourceMoveNextCoreDelegate = new Action<object>(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SeletedSourceMoveNextCore);

			// Token: 0x04000F1E RID: 3870
			private static readonly Action<object> selectedEnumeratorDisposeAsyncCoreDelegate = new Action<object>(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SelectedEnumeratorDisposeAsyncCore);

			// Token: 0x04000F1F RID: 3871
			private static readonly Action<object> selectorAwaitCoreDelegate = new Action<object>(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.SelectorAwaitCore);

			// Token: 0x04000F20 RID: 3872
			private static readonly Action<object> resultSelectorAwaitCoreDelegate = new Action<object>(SelectManyAwaitWithCancellation<TSource, TCollection, TResult>._SelectManyAwaitWithCancellation.ResultSelectorAwaitCore);

			// Token: 0x04000F21 RID: 3873
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000F22 RID: 3874
			private readonly Func<TSource, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1;

			// Token: 0x04000F23 RID: 3875
			private readonly Func<TSource, int, CancellationToken, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2;

			// Token: 0x04000F24 RID: 3876
			private readonly Func<TSource, TCollection, CancellationToken, UniTask<TResult>> resultSelector;

			// Token: 0x04000F25 RID: 3877
			private CancellationToken cancellationToken;

			// Token: 0x04000F26 RID: 3878
			private TSource sourceCurrent;

			// Token: 0x04000F27 RID: 3879
			private int sourceIndex;

			// Token: 0x04000F28 RID: 3880
			private IUniTaskAsyncEnumerator<TSource> sourceEnumerator;

			// Token: 0x04000F29 RID: 3881
			private IUniTaskAsyncEnumerator<TCollection> selectedEnumerator;

			// Token: 0x04000F2A RID: 3882
			private UniTask<bool>.Awaiter sourceAwaiter;

			// Token: 0x04000F2B RID: 3883
			private UniTask<bool>.Awaiter selectedAwaiter;

			// Token: 0x04000F2C RID: 3884
			private UniTask.Awaiter selectedDisposeAsyncAwaiter;

			// Token: 0x04000F2D RID: 3885
			private UniTask<IUniTaskAsyncEnumerable<TCollection>>.Awaiter collectionSelectorAwaiter;

			// Token: 0x04000F2E RID: 3886
			private UniTask<TResult>.Awaiter resultSelectorAwaiter;
		}
	}
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200005A RID: 90
	internal sealed class SelectManyAwait<TSource, TCollection, TResult> : IUniTaskAsyncEnumerable<TResult>
	{
		// Token: 0x0600033F RID: 831 RVA: 0x0000C1B1 File Offset: 0x0000A3B1
		public SelectManyAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, UniTask<TResult>> resultSelector)
		{
			this.source = source;
			this.selector1 = selector;
			this.selector2 = null;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000C1D5 File Offset: 0x0000A3D5
		public SelectManyAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector, Func<TSource, TCollection, UniTask<TResult>> resultSelector)
		{
			this.source = source;
			this.selector1 = null;
			this.selector2 = selector;
			this.resultSelector = resultSelector;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000C1F9 File Offset: 0x0000A3F9
		public IUniTaskAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default(CancellationToken))
		{
			return new SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait(this.source, this.selector1, this.selector2, this.resultSelector, cancellationToken);
		}

		// Token: 0x04000141 RID: 321
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x04000142 RID: 322
		private readonly Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1;

		// Token: 0x04000143 RID: 323
		private readonly Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2;

		// Token: 0x04000144 RID: 324
		private readonly Func<TSource, TCollection, UniTask<TResult>> resultSelector;

		// Token: 0x02000192 RID: 402
		private sealed class _SelectManyAwait : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
		{
			// Token: 0x0600078E RID: 1934 RVA: 0x0003FDD0 File Offset: 0x0003DFD0
			public _SelectManyAwait(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1, Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2, Func<TSource, TCollection, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
			{
				this.source = source;
				this.selector1 = selector1;
				this.selector2 = selector2;
				this.resultSelector = resultSelector;
				this.cancellationToken = cancellationToken;
			}

			// Token: 0x1700003F RID: 63
			// (get) Token: 0x0600078F RID: 1935 RVA: 0x0003FDFD File Offset: 0x0003DFFD
			// (set) Token: 0x06000790 RID: 1936 RVA: 0x0003FE05 File Offset: 0x0003E005
			public TResult Current { get; private set; }

			// Token: 0x06000791 RID: 1937 RVA: 0x0003FE10 File Offset: 0x0003E010
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

			// Token: 0x06000792 RID: 1938 RVA: 0x0003FE70 File Offset: 0x0003E070
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
					SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SourceMoveNextCore(this);
					return;
				}
				this.sourceAwaiter.SourceOnCompleted(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.sourceMoveNextCoreDelegate, this);
			}

			// Token: 0x06000793 RID: 1939 RVA: 0x0003FEE0 File Offset: 0x0003E0E0
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
					SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SeletedSourceMoveNextCore(this);
					return;
				}
				this.selectedAwaiter.SourceOnCompleted(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.selectedSourceMoveNextCoreDelegate, this);
			}

			// Token: 0x06000794 RID: 1940 RVA: 0x0003FF50 File Offset: 0x0003E150
			private static void SourceMoveNextCore(object state)
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait = (SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait)state;
				bool flag;
				if (selectManyAwait.TryGetResult<bool>(selectManyAwait.sourceAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectManyAwait.sourceCurrent = selectManyAwait.sourceEnumerator.Current;
							if (selectManyAwait.selector1 != null)
							{
								selectManyAwait.collectionSelectorAwaiter = selectManyAwait.selector1(selectManyAwait.sourceCurrent).GetAwaiter();
							}
							else
							{
								SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait2 = selectManyAwait;
								Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> func = selectManyAwait.selector2;
								TSource arg = selectManyAwait.sourceCurrent;
								SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait3 = selectManyAwait;
								int num = selectManyAwait3.sourceIndex;
								selectManyAwait3.sourceIndex = checked(num + 1);
								selectManyAwait2.collectionSelectorAwaiter = func(arg, num).GetAwaiter();
							}
							if (selectManyAwait.collectionSelectorAwaiter.IsCompleted)
							{
								SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SelectorAwaitCore(selectManyAwait);
							}
							else
							{
								selectManyAwait.collectionSelectorAwaiter.SourceOnCompleted(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.selectorAwaitCoreDelegate, selectManyAwait);
							}
							return;
						}
						catch (Exception ex)
						{
							selectManyAwait.completionSource.TrySetException(ex);
							return;
						}
					}
					selectManyAwait.completionSource.TrySetResult(false);
				}
			}

			// Token: 0x06000795 RID: 1941 RVA: 0x00040040 File Offset: 0x0003E240
			private static void SeletedSourceMoveNextCore(object state)
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait = (SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait)state;
				bool flag;
				if (selectManyAwait.TryGetResult<bool>(selectManyAwait.selectedAwaiter, ref flag))
				{
					if (flag)
					{
						try
						{
							selectManyAwait.resultSelectorAwaiter = selectManyAwait.resultSelector(selectManyAwait.sourceCurrent, selectManyAwait.selectedEnumerator.Current).GetAwaiter();
							if (selectManyAwait.resultSelectorAwaiter.IsCompleted)
							{
								SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.ResultSelectorAwaitCore(selectManyAwait);
							}
							else
							{
								selectManyAwait.resultSelectorAwaiter.SourceOnCompleted(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.resultSelectorAwaitCoreDelegate, selectManyAwait);
							}
							return;
						}
						catch (Exception ex)
						{
							selectManyAwait.completionSource.TrySetException(ex);
							return;
						}
					}
					try
					{
						selectManyAwait.selectedDisposeAsyncAwaiter = selectManyAwait.selectedEnumerator.DisposeAsync().GetAwaiter();
					}
					catch (Exception ex2)
					{
						selectManyAwait.completionSource.TrySetException(ex2);
						return;
					}
					if (selectManyAwait.selectedDisposeAsyncAwaiter.IsCompleted)
					{
						SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SelectedEnumeratorDisposeAsyncCore(selectManyAwait);
						return;
					}
					selectManyAwait.selectedDisposeAsyncAwaiter.SourceOnCompleted(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.selectedEnumeratorDisposeAsyncCoreDelegate, selectManyAwait);
				}
			}

			// Token: 0x06000796 RID: 1942 RVA: 0x0004013C File Offset: 0x0003E33C
			private static void SelectedEnumeratorDisposeAsyncCore(object state)
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait = (SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait)state;
				if (selectManyAwait.TryGetResult(selectManyAwait.selectedDisposeAsyncAwaiter))
				{
					selectManyAwait.selectedEnumerator = null;
					selectManyAwait.selectedAwaiter = default(UniTask<bool>.Awaiter);
					selectManyAwait.MoveNextSource();
				}
			}

			// Token: 0x06000797 RID: 1943 RVA: 0x00040178 File Offset: 0x0003E378
			private static void SelectorAwaitCore(object state)
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait = (SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait)state;
				IUniTaskAsyncEnumerable<TCollection> uniTaskAsyncEnumerable;
				if (selectManyAwait.TryGetResult<IUniTaskAsyncEnumerable<TCollection>>(selectManyAwait.collectionSelectorAwaiter, ref uniTaskAsyncEnumerable))
				{
					selectManyAwait.selectedEnumerator = uniTaskAsyncEnumerable.GetAsyncEnumerator(selectManyAwait.cancellationToken);
					selectManyAwait.MoveNextSelected();
				}
			}

			// Token: 0x06000798 RID: 1944 RVA: 0x000401B4 File Offset: 0x0003E3B4
			private static void ResultSelectorAwaitCore(object state)
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait selectManyAwait = (SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait)state;
				TResult value;
				if (selectManyAwait.TryGetResult<TResult>(selectManyAwait.resultSelectorAwaiter, ref value))
				{
					selectManyAwait.Current = value;
					selectManyAwait.completionSource.TrySetResult(true);
				}
			}

			// Token: 0x06000799 RID: 1945 RVA: 0x000401EC File Offset: 0x0003E3EC
			public UniTask DisposeAsync()
			{
				SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.<DisposeAsync>d__32 <DisposeAsync>d__;
				<DisposeAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
				<DisposeAsync>d__.<>4__this = this;
				<DisposeAsync>d__.<>1__state = -1;
				<DisposeAsync>d__.<>t__builder.Start<SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.<DisposeAsync>d__32>(ref <DisposeAsync>d__);
				return <DisposeAsync>d__.<>t__builder.Task;
			}

			// Token: 0x04000F08 RID: 3848
			private static readonly Action<object> sourceMoveNextCoreDelegate = new Action<object>(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SourceMoveNextCore);

			// Token: 0x04000F09 RID: 3849
			private static readonly Action<object> selectedSourceMoveNextCoreDelegate = new Action<object>(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SeletedSourceMoveNextCore);

			// Token: 0x04000F0A RID: 3850
			private static readonly Action<object> selectedEnumeratorDisposeAsyncCoreDelegate = new Action<object>(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SelectedEnumeratorDisposeAsyncCore);

			// Token: 0x04000F0B RID: 3851
			private static readonly Action<object> selectorAwaitCoreDelegate = new Action<object>(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.SelectorAwaitCore);

			// Token: 0x04000F0C RID: 3852
			private static readonly Action<object> resultSelectorAwaitCoreDelegate = new Action<object>(SelectManyAwait<TSource, TCollection, TResult>._SelectManyAwait.ResultSelectorAwaitCore);

			// Token: 0x04000F0D RID: 3853
			private readonly IUniTaskAsyncEnumerable<TSource> source;

			// Token: 0x04000F0E RID: 3854
			private readonly Func<TSource, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector1;

			// Token: 0x04000F0F RID: 3855
			private readonly Func<TSource, int, UniTask<IUniTaskAsyncEnumerable<TCollection>>> selector2;

			// Token: 0x04000F10 RID: 3856
			private readonly Func<TSource, TCollection, UniTask<TResult>> resultSelector;

			// Token: 0x04000F11 RID: 3857
			private CancellationToken cancellationToken;

			// Token: 0x04000F12 RID: 3858
			private TSource sourceCurrent;

			// Token: 0x04000F13 RID: 3859
			private int sourceIndex;

			// Token: 0x04000F14 RID: 3860
			private IUniTaskAsyncEnumerator<TSource> sourceEnumerator;

			// Token: 0x04000F15 RID: 3861
			private IUniTaskAsyncEnumerator<TCollection> selectedEnumerator;

			// Token: 0x04000F16 RID: 3862
			private UniTask<bool>.Awaiter sourceAwaiter;

			// Token: 0x04000F17 RID: 3863
			private UniTask<bool>.Awaiter selectedAwaiter;

			// Token: 0x04000F18 RID: 3864
			private UniTask.Awaiter selectedDisposeAsyncAwaiter;

			// Token: 0x04000F19 RID: 3865
			private UniTask<IUniTaskAsyncEnumerable<TCollection>>.Awaiter collectionSelectorAwaiter;

			// Token: 0x04000F1A RID: 3866
			private UniTask<TResult>.Awaiter resultSelectorAwaiter;
		}
	}
}

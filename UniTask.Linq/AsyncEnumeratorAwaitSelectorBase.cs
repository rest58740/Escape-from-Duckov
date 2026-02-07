using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000009 RID: 9
	internal abstract class AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait> : MoveNextSource, IUniTaskAsyncEnumerator<TResult>, IUniTaskAsyncDisposable
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000748D File Offset: 0x0000568D
		public AsyncEnumeratorAwaitSelectorBase(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			this.source = source;
			this.cancellationToken = cancellationToken;
		}

		// Token: 0x060001FC RID: 508
		protected abstract UniTask<TAwait> TransformAsync(TSource sourceCurrent);

		// Token: 0x060001FD RID: 509
		protected abstract bool TrySetCurrentCore(TAwait awaitResult, out bool terminateIteration);

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x060001FE RID: 510 RVA: 0x000074A3 File Offset: 0x000056A3
		// (set) Token: 0x060001FF RID: 511 RVA: 0x000074AB File Offset: 0x000056AB
		private protected TSource SourceCurrent { protected get; private set; }

		// Token: 0x06000200 RID: 512 RVA: 0x000074B4 File Offset: 0x000056B4
		[return: TupleElementNames(new string[]
		{
			"waitCallback",
			"requireNextIteration"
		})]
		protected ValueTuple<bool, bool> ActionCompleted(bool trySetCurrentResult, out bool moveNextResult)
		{
			if (trySetCurrentResult)
			{
				moveNextResult = true;
				return new ValueTuple<bool, bool>(false, false);
			}
			moveNextResult = false;
			return new ValueTuple<bool, bool>(false, true);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x000074CE File Offset: 0x000056CE
		[return: TupleElementNames(new string[]
		{
			"waitCallback",
			"requireNextIteration"
		})]
		protected ValueTuple<bool, bool> WaitAwaitCallback(out bool moveNextResult)
		{
			moveNextResult = false;
			return new ValueTuple<bool, bool>(true, false);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000074DA File Offset: 0x000056DA
		[return: TupleElementNames(new string[]
		{
			"waitCallback",
			"requireNextIteration"
		})]
		protected ValueTuple<bool, bool> IterateFinished(out bool moveNextResult)
		{
			moveNextResult = false;
			return new ValueTuple<bool, bool>(false, false);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000203 RID: 515 RVA: 0x000074E6 File Offset: 0x000056E6
		// (set) Token: 0x06000204 RID: 516 RVA: 0x000074EE File Offset: 0x000056EE
		public TResult Current { get; protected set; }

		// Token: 0x06000205 RID: 517 RVA: 0x000074F8 File Offset: 0x000056F8
		public UniTask<bool> MoveNextAsync()
		{
			if (this.enumerator == null)
			{
				this.enumerator = this.source.GetAsyncEnumerator(this.cancellationToken);
			}
			this.completionSource.Reset();
			this.SourceMoveNext();
			return new UniTask<bool>(this, this.completionSource.Version);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007548 File Offset: 0x00005748
		protected void SourceMoveNext()
		{
			for (;;)
			{
				this.sourceMoveNext = this.enumerator.MoveNextAsync().GetAwaiter();
				if (this.sourceMoveNext.IsCompleted)
				{
					bool flag = false;
					try
					{
						ValueTuple<bool, bool> valueTuple = this.TryMoveNextCore(this.sourceMoveNext.GetResult(), out flag);
						bool item = valueTuple.Item1;
						bool item2 = valueTuple.Item2;
						if (item)
						{
							return;
						}
						if (item2)
						{
							continue;
						}
						this.completionSource.TrySetResult(flag);
						return;
					}
					catch (Exception ex)
					{
						this.completionSource.TrySetException(ex);
						return;
					}
					break;
				}
				break;
			}
			this.sourceMoveNext.SourceOnCompleted(AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>.moveNextCallbackDelegate, this);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x000075EC File Offset: 0x000057EC
		[return: TupleElementNames(new string[]
		{
			"waitCallback",
			"requireNextIteration"
		})]
		private ValueTuple<bool, bool> TryMoveNextCore(bool sourceHasCurrent, out bool result)
		{
			if (!sourceHasCurrent)
			{
				return this.IterateFinished(out result);
			}
			this.SourceCurrent = this.enumerator.Current;
			UniTask<TAwait> taskResult = this.TransformAsync(this.SourceCurrent);
			TAwait awaitResult;
			if (!this.UnwarapTask(taskResult, out awaitResult))
			{
				return this.WaitAwaitCallback(out result);
			}
			bool flag;
			bool trySetCurrentResult = this.TrySetCurrentCore(awaitResult, out flag);
			if (flag)
			{
				return this.IterateFinished(out result);
			}
			return this.ActionCompleted(trySetCurrentResult, out result);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00007654 File Offset: 0x00005854
		protected bool UnwarapTask(UniTask<TAwait> taskResult, out TAwait result)
		{
			this.resultAwaiter = taskResult.GetAwaiter();
			if (this.resultAwaiter.IsCompleted)
			{
				result = this.resultAwaiter.GetResult();
				return true;
			}
			this.resultAwaiter.SourceOnCompleted(AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>.setCurrentCallbackDelegate, this);
			result = default(TAwait);
			return false;
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000076A8 File Offset: 0x000058A8
		private static void MoveNextCallBack(object state)
		{
			AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait> asyncEnumeratorAwaitSelectorBase = (AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>)state;
			bool flag = false;
			try
			{
				ValueTuple<bool, bool> valueTuple = asyncEnumeratorAwaitSelectorBase.TryMoveNextCore(asyncEnumeratorAwaitSelectorBase.sourceMoveNext.GetResult(), out flag);
				bool item = valueTuple.Item1;
				bool item2 = valueTuple.Item2;
				if (!item)
				{
					if (item2)
					{
						asyncEnumeratorAwaitSelectorBase.SourceMoveNext();
					}
					else
					{
						asyncEnumeratorAwaitSelectorBase.completionSource.TrySetResult(flag);
					}
				}
			}
			catch (Exception ex)
			{
				asyncEnumeratorAwaitSelectorBase.completionSource.TrySetException(ex);
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00007720 File Offset: 0x00005920
		private static void SetCurrentCallBack(object state)
		{
			AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait> asyncEnumeratorAwaitSelectorBase = (AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>)state;
			bool flag2;
			bool flag;
			try
			{
				TAwait result = asyncEnumeratorAwaitSelectorBase.resultAwaiter.GetResult();
				flag = asyncEnumeratorAwaitSelectorBase.TrySetCurrentCore(result, out flag2);
			}
			catch (Exception ex)
			{
				asyncEnumeratorAwaitSelectorBase.completionSource.TrySetException(ex);
				return;
			}
			if (asyncEnumeratorAwaitSelectorBase.cancellationToken.IsCancellationRequested)
			{
				asyncEnumeratorAwaitSelectorBase.completionSource.TrySetCanceled(asyncEnumeratorAwaitSelectorBase.cancellationToken);
				return;
			}
			if (flag)
			{
				asyncEnumeratorAwaitSelectorBase.completionSource.TrySetResult(true);
				return;
			}
			if (flag2)
			{
				asyncEnumeratorAwaitSelectorBase.completionSource.TrySetResult(false);
				return;
			}
			asyncEnumeratorAwaitSelectorBase.SourceMoveNext();
		}

		// Token: 0x0600020B RID: 523 RVA: 0x000077B8 File Offset: 0x000059B8
		public virtual UniTask DisposeAsync()
		{
			if (this.enumerator != null)
			{
				return this.enumerator.DisposeAsync();
			}
			return default(UniTask);
		}

		// Token: 0x0400000A RID: 10
		private static readonly Action<object> moveNextCallbackDelegate = new Action<object>(AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>.MoveNextCallBack);

		// Token: 0x0400000B RID: 11
		private static readonly Action<object> setCurrentCallbackDelegate = new Action<object>(AsyncEnumeratorAwaitSelectorBase<TSource, TResult, TAwait>.SetCurrentCallBack);

		// Token: 0x0400000C RID: 12
		private readonly IUniTaskAsyncEnumerable<TSource> source;

		// Token: 0x0400000D RID: 13
		protected CancellationToken cancellationToken;

		// Token: 0x0400000E RID: 14
		private IUniTaskAsyncEnumerator<TSource> enumerator;

		// Token: 0x0400000F RID: 15
		private UniTask<bool>.Awaiter sourceMoveNext;

		// Token: 0x04000010 RID: 16
		private UniTask<TAwait>.Awaiter resultAwaiter;
	}
}

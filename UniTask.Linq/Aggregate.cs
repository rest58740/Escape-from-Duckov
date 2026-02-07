using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000004 RID: 4
	internal static class Aggregate
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00006CD8 File Offset: 0x00004ED8
		internal static UniTask<TSource> AggregateAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, TSource> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAsync>d__0<TSource> <AggregateAsync>d__;
			<AggregateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<AggregateAsync>d__.source = source;
			<AggregateAsync>d__.accumulator = accumulator;
			<AggregateAsync>d__.cancellationToken = cancellationToken;
			<AggregateAsync>d__.<>1__state = -1;
			<AggregateAsync>d__.<>t__builder.Start<Aggregate.<AggregateAsync>d__0<TSource>>(ref <AggregateAsync>d__);
			return <AggregateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006D2C File Offset: 0x00004F2C
		internal static UniTask<TAccumulate> AggregateAsync<TSource, TAccumulate>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAsync>d__1<TSource, TAccumulate> <AggregateAsync>d__;
			<AggregateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TAccumulate>.Create();
			<AggregateAsync>d__.source = source;
			<AggregateAsync>d__.seed = seed;
			<AggregateAsync>d__.accumulator = accumulator;
			<AggregateAsync>d__.cancellationToken = cancellationToken;
			<AggregateAsync>d__.<>1__state = -1;
			<AggregateAsync>d__.<>t__builder.Start<Aggregate.<AggregateAsync>d__1<TSource, TAccumulate>>(ref <AggregateAsync>d__);
			return <AggregateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00006D88 File Offset: 0x00004F88
		internal static UniTask<TResult> AggregateAsync<TSource, TAccumulate, TResult>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TResult> resultSelector, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAsync>d__2<TSource, TAccumulate, TResult> <AggregateAsync>d__;
			<AggregateAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<AggregateAsync>d__.source = source;
			<AggregateAsync>d__.seed = seed;
			<AggregateAsync>d__.accumulator = accumulator;
			<AggregateAsync>d__.resultSelector = resultSelector;
			<AggregateAsync>d__.cancellationToken = cancellationToken;
			<AggregateAsync>d__.<>1__state = -1;
			<AggregateAsync>d__.<>t__builder.Start<Aggregate.<AggregateAsync>d__2<TSource, TAccumulate, TResult>>(ref <AggregateAsync>d__);
			return <AggregateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006DEC File Offset: 0x00004FEC
		internal static UniTask<TSource> AggregateAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, UniTask<TSource>> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitAsync>d__3<TSource> <AggregateAwaitAsync>d__;
			<AggregateAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<AggregateAwaitAsync>d__.source = source;
			<AggregateAwaitAsync>d__.accumulator = accumulator;
			<AggregateAwaitAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitAsync>d__.<>1__state = -1;
			<AggregateAwaitAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitAsync>d__3<TSource>>(ref <AggregateAwaitAsync>d__);
			return <AggregateAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00006E40 File Offset: 0x00005040
		internal static UniTask<TAccumulate> AggregateAwaitAsync<TSource, TAccumulate>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitAsync>d__4<TSource, TAccumulate> <AggregateAwaitAsync>d__;
			<AggregateAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TAccumulate>.Create();
			<AggregateAwaitAsync>d__.source = source;
			<AggregateAwaitAsync>d__.seed = seed;
			<AggregateAwaitAsync>d__.accumulator = accumulator;
			<AggregateAwaitAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitAsync>d__.<>1__state = -1;
			<AggregateAwaitAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitAsync>d__4<TSource, TAccumulate>>(ref <AggregateAwaitAsync>d__);
			return <AggregateAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00006E9C File Offset: 0x0000509C
		internal static UniTask<TResult> AggregateAwaitAsync<TSource, TAccumulate, TResult>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> accumulator, Func<TAccumulate, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitAsync>d__5<TSource, TAccumulate, TResult> <AggregateAwaitAsync>d__;
			<AggregateAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<AggregateAwaitAsync>d__.source = source;
			<AggregateAwaitAsync>d__.seed = seed;
			<AggregateAwaitAsync>d__.accumulator = accumulator;
			<AggregateAwaitAsync>d__.resultSelector = resultSelector;
			<AggregateAwaitAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitAsync>d__.<>1__state = -1;
			<AggregateAwaitAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitAsync>d__5<TSource, TAccumulate, TResult>>(ref <AggregateAwaitAsync>d__);
			return <AggregateAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00006F00 File Offset: 0x00005100
		internal static UniTask<TSource> AggregateAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TSource, CancellationToken, UniTask<TSource>> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitWithCancellationAsync>d__6<TSource> <AggregateAwaitWithCancellationAsync>d__;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<AggregateAwaitWithCancellationAsync>d__.source = source;
			<AggregateAwaitWithCancellationAsync>d__.accumulator = accumulator;
			<AggregateAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitWithCancellationAsync>d__6<TSource>>(ref <AggregateAwaitWithCancellationAsync>d__);
			return <AggregateAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006F54 File Offset: 0x00005154
		internal static UniTask<TAccumulate> AggregateAwaitWithCancellationAsync<TSource, TAccumulate>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>> accumulator, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitWithCancellationAsync>d__7<TSource, TAccumulate> <AggregateAwaitWithCancellationAsync>d__;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TAccumulate>.Create();
			<AggregateAwaitWithCancellationAsync>d__.source = source;
			<AggregateAwaitWithCancellationAsync>d__.seed = seed;
			<AggregateAwaitWithCancellationAsync>d__.accumulator = accumulator;
			<AggregateAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitWithCancellationAsync>d__7<TSource, TAccumulate>>(ref <AggregateAwaitWithCancellationAsync>d__);
			return <AggregateAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00006FB0 File Offset: 0x000051B0
		internal static UniTask<TResult> AggregateAwaitWithCancellationAsync<TSource, TAccumulate, TResult>(IUniTaskAsyncEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, CancellationToken, UniTask<TAccumulate>> accumulator, Func<TAccumulate, CancellationToken, UniTask<TResult>> resultSelector, CancellationToken cancellationToken)
		{
			Aggregate.<AggregateAwaitWithCancellationAsync>d__8<TSource, TAccumulate, TResult> <AggregateAwaitWithCancellationAsync>d__;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TResult>.Create();
			<AggregateAwaitWithCancellationAsync>d__.source = source;
			<AggregateAwaitWithCancellationAsync>d__.seed = seed;
			<AggregateAwaitWithCancellationAsync>d__.accumulator = accumulator;
			<AggregateAwaitWithCancellationAsync>d__.resultSelector = resultSelector;
			<AggregateAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AggregateAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AggregateAwaitWithCancellationAsync>d__.<>t__builder.Start<Aggregate.<AggregateAwaitWithCancellationAsync>d__8<TSource, TAccumulate, TResult>>(ref <AggregateAwaitWithCancellationAsync>d__);
			return <AggregateAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

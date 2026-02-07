using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200003E RID: 62
	internal static class LongCount
	{
		// Token: 0x060002A3 RID: 675 RVA: 0x00009BE0 File Offset: 0x00007DE0
		internal static UniTask<long> LongCountAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			LongCount.<LongCountAsync>d__0<TSource> <LongCountAsync>d__;
			<LongCountAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<LongCountAsync>d__.source = source;
			<LongCountAsync>d__.cancellationToken = cancellationToken;
			<LongCountAsync>d__.<>1__state = -1;
			<LongCountAsync>d__.<>t__builder.Start<LongCount.<LongCountAsync>d__0<TSource>>(ref <LongCountAsync>d__);
			return <LongCountAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009C2C File Offset: 0x00007E2C
		internal static UniTask<long> LongCountAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
		{
			LongCount.<LongCountAsync>d__1<TSource> <LongCountAsync>d__;
			<LongCountAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<LongCountAsync>d__.source = source;
			<LongCountAsync>d__.predicate = predicate;
			<LongCountAsync>d__.cancellationToken = cancellationToken;
			<LongCountAsync>d__.<>1__state = -1;
			<LongCountAsync>d__.<>t__builder.Start<LongCount.<LongCountAsync>d__1<TSource>>(ref <LongCountAsync>d__);
			return <LongCountAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00009C80 File Offset: 0x00007E80
		internal static UniTask<long> LongCountAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			LongCount.<LongCountAwaitAsync>d__2<TSource> <LongCountAwaitAsync>d__;
			<LongCountAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<LongCountAwaitAsync>d__.source = source;
			<LongCountAwaitAsync>d__.predicate = predicate;
			<LongCountAwaitAsync>d__.cancellationToken = cancellationToken;
			<LongCountAwaitAsync>d__.<>1__state = -1;
			<LongCountAwaitAsync>d__.<>t__builder.Start<LongCount.<LongCountAwaitAsync>d__2<TSource>>(ref <LongCountAwaitAsync>d__);
			return <LongCountAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009CD4 File Offset: 0x00007ED4
		internal static UniTask<long> LongCountAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			LongCount.<LongCountAwaitWithCancellationAsync>d__3<TSource> <LongCountAwaitWithCancellationAsync>d__;
			<LongCountAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<long>.Create();
			<LongCountAwaitWithCancellationAsync>d__.source = source;
			<LongCountAwaitWithCancellationAsync>d__.predicate = predicate;
			<LongCountAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<LongCountAwaitWithCancellationAsync>d__.<>1__state = -1;
			<LongCountAwaitWithCancellationAsync>d__.<>t__builder.Start<LongCount.<LongCountAwaitWithCancellationAsync>d__3<TSource>>(ref <LongCountAwaitWithCancellationAsync>d__);
			return <LongCountAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

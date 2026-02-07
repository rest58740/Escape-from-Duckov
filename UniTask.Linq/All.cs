using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000005 RID: 5
	internal static class All
	{
		// Token: 0x060001E7 RID: 487 RVA: 0x00007014 File Offset: 0x00005214
		internal static UniTask<bool> AllAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
		{
			All.<AllAsync>d__0<TSource> <AllAsync>d__;
			<AllAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AllAsync>d__.source = source;
			<AllAsync>d__.predicate = predicate;
			<AllAsync>d__.cancellationToken = cancellationToken;
			<AllAsync>d__.<>1__state = -1;
			<AllAsync>d__.<>t__builder.Start<All.<AllAsync>d__0<TSource>>(ref <AllAsync>d__);
			return <AllAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00007068 File Offset: 0x00005268
		internal static UniTask<bool> AllAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			All.<AllAwaitAsync>d__1<TSource> <AllAwaitAsync>d__;
			<AllAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AllAwaitAsync>d__.source = source;
			<AllAwaitAsync>d__.predicate = predicate;
			<AllAwaitAsync>d__.cancellationToken = cancellationToken;
			<AllAwaitAsync>d__.<>1__state = -1;
			<AllAwaitAsync>d__.<>t__builder.Start<All.<AllAwaitAsync>d__1<TSource>>(ref <AllAwaitAsync>d__);
			return <AllAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000070BC File Offset: 0x000052BC
		internal static UniTask<bool> AllAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			All.<AllAwaitWithCancellationAsync>d__2<TSource> <AllAwaitWithCancellationAsync>d__;
			<AllAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AllAwaitWithCancellationAsync>d__.source = source;
			<AllAwaitWithCancellationAsync>d__.predicate = predicate;
			<AllAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AllAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AllAwaitWithCancellationAsync>d__.<>t__builder.Start<All.<AllAwaitWithCancellationAsync>d__2<TSource>>(ref <AllAwaitWithCancellationAsync>d__);
			return <AllAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

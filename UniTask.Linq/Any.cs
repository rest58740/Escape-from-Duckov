using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000006 RID: 6
	internal static class Any
	{
		// Token: 0x060001EA RID: 490 RVA: 0x00007110 File Offset: 0x00005310
		internal static UniTask<bool> AnyAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			Any.<AnyAsync>d__0<TSource> <AnyAsync>d__;
			<AnyAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AnyAsync>d__.source = source;
			<AnyAsync>d__.cancellationToken = cancellationToken;
			<AnyAsync>d__.<>1__state = -1;
			<AnyAsync>d__.<>t__builder.Start<Any.<AnyAsync>d__0<TSource>>(ref <AnyAsync>d__);
			return <AnyAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000715C File Offset: 0x0000535C
		internal static UniTask<bool> AnyAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, bool> predicate, CancellationToken cancellationToken)
		{
			Any.<AnyAsync>d__1<TSource> <AnyAsync>d__;
			<AnyAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AnyAsync>d__.source = source;
			<AnyAsync>d__.predicate = predicate;
			<AnyAsync>d__.cancellationToken = cancellationToken;
			<AnyAsync>d__.<>1__state = -1;
			<AnyAsync>d__.<>t__builder.Start<Any.<AnyAsync>d__1<TSource>>(ref <AnyAsync>d__);
			return <AnyAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000071B0 File Offset: 0x000053B0
		internal static UniTask<bool> AnyAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			Any.<AnyAwaitAsync>d__2<TSource> <AnyAwaitAsync>d__;
			<AnyAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AnyAwaitAsync>d__.source = source;
			<AnyAwaitAsync>d__.predicate = predicate;
			<AnyAwaitAsync>d__.cancellationToken = cancellationToken;
			<AnyAwaitAsync>d__.<>1__state = -1;
			<AnyAwaitAsync>d__.<>t__builder.Start<Any.<AnyAwaitAsync>d__2<TSource>>(ref <AnyAwaitAsync>d__);
			return <AnyAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007204 File Offset: 0x00005404
		internal static UniTask<bool> AnyAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<bool>> predicate, CancellationToken cancellationToken)
		{
			Any.<AnyAwaitWithCancellationAsync>d__3<TSource> <AnyAwaitWithCancellationAsync>d__;
			<AnyAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<AnyAwaitWithCancellationAsync>d__.source = source;
			<AnyAwaitWithCancellationAsync>d__.predicate = predicate;
			<AnyAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<AnyAwaitWithCancellationAsync>d__.<>1__state = -1;
			<AnyAwaitWithCancellationAsync>d__.<>t__builder.Start<Any.<AnyAwaitWithCancellationAsync>d__3<TSource>>(ref <AnyAwaitWithCancellationAsync>d__);
			return <AnyAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

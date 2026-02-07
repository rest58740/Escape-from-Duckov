using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200002F RID: 47
	internal static class ForEach
	{
		// Token: 0x0600027F RID: 639 RVA: 0x0000943C File Offset: 0x0000763C
		public static UniTask ForEachAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Action<TSource> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAsync>d__0<TSource> <ForEachAsync>d__;
			<ForEachAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAsync>d__.source = source;
			<ForEachAsync>d__.action = action;
			<ForEachAsync>d__.cancellationToken = cancellationToken;
			<ForEachAsync>d__.<>1__state = -1;
			<ForEachAsync>d__.<>t__builder.Start<ForEach.<ForEachAsync>d__0<TSource>>(ref <ForEachAsync>d__);
			return <ForEachAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00009490 File Offset: 0x00007690
		public static UniTask ForEachAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Action<TSource, int> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAsync>d__1<TSource> <ForEachAsync>d__;
			<ForEachAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAsync>d__.source = source;
			<ForEachAsync>d__.action = action;
			<ForEachAsync>d__.cancellationToken = cancellationToken;
			<ForEachAsync>d__.<>1__state = -1;
			<ForEachAsync>d__.<>t__builder.Start<ForEach.<ForEachAsync>d__1<TSource>>(ref <ForEachAsync>d__);
			return <ForEachAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x000094E4 File Offset: 0x000076E4
		public static UniTask ForEachAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAwaitAsync>d__2<TSource> <ForEachAwaitAsync>d__;
			<ForEachAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAwaitAsync>d__.source = source;
			<ForEachAwaitAsync>d__.action = action;
			<ForEachAwaitAsync>d__.cancellationToken = cancellationToken;
			<ForEachAwaitAsync>d__.<>1__state = -1;
			<ForEachAwaitAsync>d__.<>t__builder.Start<ForEach.<ForEachAwaitAsync>d__2<TSource>>(ref <ForEachAwaitAsync>d__);
			return <ForEachAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00009538 File Offset: 0x00007738
		public static UniTask ForEachAwaitAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, UniTask> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAwaitAsync>d__3<TSource> <ForEachAwaitAsync>d__;
			<ForEachAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAwaitAsync>d__.source = source;
			<ForEachAwaitAsync>d__.action = action;
			<ForEachAwaitAsync>d__.cancellationToken = cancellationToken;
			<ForEachAwaitAsync>d__.<>1__state = -1;
			<ForEachAwaitAsync>d__.<>t__builder.Start<ForEach.<ForEachAwaitAsync>d__3<TSource>>(ref <ForEachAwaitAsync>d__);
			return <ForEachAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000958C File Offset: 0x0000778C
		public static UniTask ForEachAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAwaitWithCancellationAsync>d__4<TSource> <ForEachAwaitWithCancellationAsync>d__;
			<ForEachAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAwaitWithCancellationAsync>d__.source = source;
			<ForEachAwaitWithCancellationAsync>d__.action = action;
			<ForEachAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ForEachAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ForEachAwaitWithCancellationAsync>d__.<>t__builder.Start<ForEach.<ForEachAwaitWithCancellationAsync>d__4<TSource>>(ref <ForEachAwaitWithCancellationAsync>d__);
			return <ForEachAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x000095E0 File Offset: 0x000077E0
		public static UniTask ForEachAwaitWithCancellationAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, int, CancellationToken, UniTask> action, CancellationToken cancellationToken)
		{
			ForEach.<ForEachAwaitWithCancellationAsync>d__5<TSource> <ForEachAwaitWithCancellationAsync>d__;
			<ForEachAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ForEachAwaitWithCancellationAsync>d__.source = source;
			<ForEachAwaitWithCancellationAsync>d__.action = action;
			<ForEachAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ForEachAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ForEachAwaitWithCancellationAsync>d__.<>t__builder.Start<ForEach.<ForEachAwaitWithCancellationAsync>d__5<TSource>>(ref <ForEachAwaitWithCancellationAsync>d__);
			return <ForEachAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

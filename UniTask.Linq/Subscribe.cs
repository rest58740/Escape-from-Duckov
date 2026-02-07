using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000069 RID: 105
	internal static class Subscribe
	{
		// Token: 0x06000361 RID: 865 RVA: 0x0000C650 File Offset: 0x0000A850
		public static UniTaskVoid SubscribeCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeCore>d__2<TSource> <SubscribeCore>d__;
			<SubscribeCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeCore>d__.source = source;
			<SubscribeCore>d__.onNext = onNext;
			<SubscribeCore>d__.onError = onError;
			<SubscribeCore>d__.onCompleted = onCompleted;
			<SubscribeCore>d__.cancellationToken = cancellationToken;
			<SubscribeCore>d__.<>1__state = -1;
			<SubscribeCore>d__.<>t__builder.Start<Subscribe.<SubscribeCore>d__2<TSource>>(ref <SubscribeCore>d__);
			return <SubscribeCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public static UniTaskVoid SubscribeCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTaskVoid> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeCore>d__3<TSource> <SubscribeCore>d__;
			<SubscribeCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeCore>d__.source = source;
			<SubscribeCore>d__.onNext = onNext;
			<SubscribeCore>d__.onError = onError;
			<SubscribeCore>d__.onCompleted = onCompleted;
			<SubscribeCore>d__.cancellationToken = cancellationToken;
			<SubscribeCore>d__.<>1__state = -1;
			<SubscribeCore>d__.<>t__builder.Start<Subscribe.<SubscribeCore>d__3<TSource>>(ref <SubscribeCore>d__);
			return <SubscribeCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000C718 File Offset: 0x0000A918
		public static UniTaskVoid SubscribeCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTaskVoid> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeCore>d__4<TSource> <SubscribeCore>d__;
			<SubscribeCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeCore>d__.source = source;
			<SubscribeCore>d__.onNext = onNext;
			<SubscribeCore>d__.onError = onError;
			<SubscribeCore>d__.onCompleted = onCompleted;
			<SubscribeCore>d__.cancellationToken = cancellationToken;
			<SubscribeCore>d__.<>1__state = -1;
			<SubscribeCore>d__.<>t__builder.Start<Subscribe.<SubscribeCore>d__4<TSource>>(ref <SubscribeCore>d__);
			return <SubscribeCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000C77C File Offset: 0x0000A97C
		public static UniTaskVoid SubscribeCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, IObserver<TSource> observer, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeCore>d__5<TSource> <SubscribeCore>d__;
			<SubscribeCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeCore>d__.source = source;
			<SubscribeCore>d__.observer = observer;
			<SubscribeCore>d__.cancellationToken = cancellationToken;
			<SubscribeCore>d__.<>1__state = -1;
			<SubscribeCore>d__.<>t__builder.Start<Subscribe.<SubscribeCore>d__5<TSource>>(ref <SubscribeCore>d__);
			return <SubscribeCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		public static UniTaskVoid SubscribeAwaitCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeAwaitCore>d__6<TSource> <SubscribeAwaitCore>d__;
			<SubscribeAwaitCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeAwaitCore>d__.source = source;
			<SubscribeAwaitCore>d__.onNext = onNext;
			<SubscribeAwaitCore>d__.onError = onError;
			<SubscribeAwaitCore>d__.onCompleted = onCompleted;
			<SubscribeAwaitCore>d__.cancellationToken = cancellationToken;
			<SubscribeAwaitCore>d__.<>1__state = -1;
			<SubscribeAwaitCore>d__.<>t__builder.Start<Subscribe.<SubscribeAwaitCore>d__6<TSource>>(ref <SubscribeAwaitCore>d__);
			return <SubscribeAwaitCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0000C834 File Offset: 0x0000AA34
		public static UniTaskVoid SubscribeAwaitCore<TSource>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask> onNext, Action<Exception> onError, Action onCompleted, CancellationToken cancellationToken)
		{
			Subscribe.<SubscribeAwaitCore>d__7<TSource> <SubscribeAwaitCore>d__;
			<SubscribeAwaitCore>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
			<SubscribeAwaitCore>d__.source = source;
			<SubscribeAwaitCore>d__.onNext = onNext;
			<SubscribeAwaitCore>d__.onError = onError;
			<SubscribeAwaitCore>d__.onCompleted = onCompleted;
			<SubscribeAwaitCore>d__.cancellationToken = cancellationToken;
			<SubscribeAwaitCore>d__.<>1__state = -1;
			<SubscribeAwaitCore>d__.<>t__builder.Start<Subscribe.<SubscribeAwaitCore>d__7<TSource>>(ref <SubscribeAwaitCore>d__);
			return <SubscribeAwaitCore>d__.<>t__builder.Task;
		}

		// Token: 0x0400015F RID: 351
		public static readonly Action<Exception> NopError = delegate(Exception _)
		{
		};

		// Token: 0x04000160 RID: 352
		public static readonly Action NopCompleted = delegate()
		{
		};
	}
}

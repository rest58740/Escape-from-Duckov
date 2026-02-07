using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000077 RID: 119
	internal static class ToDictionary
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0000D7C8 File Offset: 0x0000B9C8
		internal static UniTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAsync>d__0<TSource, TKey> <ToDictionaryAsync>d__;
			<ToDictionaryAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TSource>>.Create();
			<ToDictionaryAsync>d__.source = source;
			<ToDictionaryAsync>d__.keySelector = keySelector;
			<ToDictionaryAsync>d__.comparer = comparer;
			<ToDictionaryAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAsync>d__.<>1__state = -1;
			<ToDictionaryAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAsync>d__0<TSource, TKey>>(ref <ToDictionaryAsync>d__);
			return <ToDictionaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D824 File Offset: 0x0000BA24
		internal static UniTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAsync>d__1<TSource, TKey, TElement> <ToDictionaryAsync>d__;
			<ToDictionaryAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TElement>>.Create();
			<ToDictionaryAsync>d__.source = source;
			<ToDictionaryAsync>d__.keySelector = keySelector;
			<ToDictionaryAsync>d__.elementSelector = elementSelector;
			<ToDictionaryAsync>d__.comparer = comparer;
			<ToDictionaryAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAsync>d__.<>1__state = -1;
			<ToDictionaryAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAsync>d__1<TSource, TKey, TElement>>(ref <ToDictionaryAsync>d__);
			return <ToDictionaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D888 File Offset: 0x0000BA88
		internal static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAwaitAsync>d__2<TSource, TKey> <ToDictionaryAwaitAsync>d__;
			<ToDictionaryAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TSource>>.Create();
			<ToDictionaryAwaitAsync>d__.source = source;
			<ToDictionaryAwaitAsync>d__.keySelector = keySelector;
			<ToDictionaryAwaitAsync>d__.comparer = comparer;
			<ToDictionaryAwaitAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAwaitAsync>d__.<>1__state = -1;
			<ToDictionaryAwaitAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAwaitAsync>d__2<TSource, TKey>>(ref <ToDictionaryAwaitAsync>d__);
			return <ToDictionaryAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
		internal static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, UniTask<TKey>> keySelector, Func<TSource, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAwaitAsync>d__3<TSource, TKey, TElement> <ToDictionaryAwaitAsync>d__;
			<ToDictionaryAwaitAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TElement>>.Create();
			<ToDictionaryAwaitAsync>d__.source = source;
			<ToDictionaryAwaitAsync>d__.keySelector = keySelector;
			<ToDictionaryAwaitAsync>d__.elementSelector = elementSelector;
			<ToDictionaryAwaitAsync>d__.comparer = comparer;
			<ToDictionaryAwaitAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAwaitAsync>d__.<>1__state = -1;
			<ToDictionaryAwaitAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAwaitAsync>d__3<TSource, TKey, TElement>>(ref <ToDictionaryAwaitAsync>d__);
			return <ToDictionaryAwaitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D948 File Offset: 0x0000BB48
		internal static UniTask<Dictionary<TKey, TSource>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAwaitWithCancellationAsync>d__4<TSource, TKey> <ToDictionaryAwaitWithCancellationAsync>d__;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TSource>>.Create();
			<ToDictionaryAwaitWithCancellationAsync>d__.source = source;
			<ToDictionaryAwaitWithCancellationAsync>d__.keySelector = keySelector;
			<ToDictionaryAwaitWithCancellationAsync>d__.comparer = comparer;
			<ToDictionaryAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAwaitWithCancellationAsync>d__4<TSource, TKey>>(ref <ToDictionaryAwaitWithCancellationAsync>d__);
			return <ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
		internal static UniTask<Dictionary<TKey, TElement>> ToDictionaryAwaitWithCancellationAsync<TSource, TKey, TElement>(IUniTaskAsyncEnumerable<TSource> source, Func<TSource, CancellationToken, UniTask<TKey>> keySelector, Func<TSource, CancellationToken, UniTask<TElement>> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken)
		{
			ToDictionary.<ToDictionaryAwaitWithCancellationAsync>d__5<TSource, TKey, TElement> <ToDictionaryAwaitWithCancellationAsync>d__;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Dictionary<TKey, TElement>>.Create();
			<ToDictionaryAwaitWithCancellationAsync>d__.source = source;
			<ToDictionaryAwaitWithCancellationAsync>d__.keySelector = keySelector;
			<ToDictionaryAwaitWithCancellationAsync>d__.elementSelector = elementSelector;
			<ToDictionaryAwaitWithCancellationAsync>d__.comparer = comparer;
			<ToDictionaryAwaitWithCancellationAsync>d__.cancellationToken = cancellationToken;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>1__state = -1;
			<ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder.Start<ToDictionary.<ToDictionaryAwaitWithCancellationAsync>d__5<TSource, TKey, TElement>>(ref <ToDictionaryAwaitWithCancellationAsync>d__);
			return <ToDictionaryAwaitWithCancellationAsync>d__.<>t__builder.Task;
		}
	}
}

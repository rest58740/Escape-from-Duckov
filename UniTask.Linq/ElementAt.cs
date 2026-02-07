using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200002B RID: 43
	internal static class ElementAt
	{
		// Token: 0x06000275 RID: 629 RVA: 0x00009224 File Offset: 0x00007424
		public static UniTask<TSource> ElementAtAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, int index, CancellationToken cancellationToken, bool defaultIfEmpty)
		{
			ElementAt.<ElementAtAsync>d__0<TSource> <ElementAtAsync>d__;
			<ElementAtAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource>.Create();
			<ElementAtAsync>d__.source = source;
			<ElementAtAsync>d__.index = index;
			<ElementAtAsync>d__.cancellationToken = cancellationToken;
			<ElementAtAsync>d__.defaultIfEmpty = defaultIfEmpty;
			<ElementAtAsync>d__.<>1__state = -1;
			<ElementAtAsync>d__.<>t__builder.Start<ElementAt.<ElementAtAsync>d__0<TSource>>(ref <ElementAtAsync>d__);
			return <ElementAtAsync>d__.<>t__builder.Task;
		}
	}
}

using System;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000076 RID: 118
	internal static class ToArray
	{
		// Token: 0x060003A6 RID: 934 RVA: 0x0000D77C File Offset: 0x0000B97C
		internal static UniTask<TSource[]> ToArrayAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			ToArray.<ToArrayAsync>d__0<TSource> <ToArrayAsync>d__;
			<ToArrayAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<TSource[]>.Create();
			<ToArrayAsync>d__.source = source;
			<ToArrayAsync>d__.cancellationToken = cancellationToken;
			<ToArrayAsync>d__.<>1__state = -1;
			<ToArrayAsync>d__.<>t__builder.Start<ToArray.<ToArrayAsync>d__0<TSource>>(ref <ToArrayAsync>d__);
			return <ToArrayAsync>d__.<>t__builder.Task;
		}
	}
}

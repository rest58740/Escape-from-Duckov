using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x0200001D RID: 29
	internal static class Contains
	{
		// Token: 0x06000259 RID: 601 RVA: 0x00008E54 File Offset: 0x00007054
		internal static UniTask<bool> ContainsAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, TSource value, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			Contains.<ContainsAsync>d__0<TSource> <ContainsAsync>d__;
			<ContainsAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<ContainsAsync>d__.source = source;
			<ContainsAsync>d__.value = value;
			<ContainsAsync>d__.comparer = comparer;
			<ContainsAsync>d__.cancellationToken = cancellationToken;
			<ContainsAsync>d__.<>1__state = -1;
			<ContainsAsync>d__.<>t__builder.Start<Contains.<ContainsAsync>d__0<TSource>>(ref <ContainsAsync>d__);
			return <ContainsAsync>d__.<>t__builder.Task;
		}
	}
}

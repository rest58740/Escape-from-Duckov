using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000078 RID: 120
	internal static class ToHashSet
	{
		// Token: 0x060003AD RID: 941 RVA: 0x0000DA08 File Offset: 0x0000BC08
		internal static UniTask<HashSet<TSource>> ToHashSetAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, IEqualityComparer<TSource> comparer, CancellationToken cancellationToken)
		{
			ToHashSet.<ToHashSetAsync>d__0<TSource> <ToHashSetAsync>d__;
			<ToHashSetAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<HashSet<TSource>>.Create();
			<ToHashSetAsync>d__.source = source;
			<ToHashSetAsync>d__.comparer = comparer;
			<ToHashSetAsync>d__.cancellationToken = cancellationToken;
			<ToHashSetAsync>d__.<>1__state = -1;
			<ToHashSetAsync>d__.<>t__builder.Start<ToHashSet.<ToHashSetAsync>d__0<TSource>>(ref <ToHashSetAsync>d__);
			return <ToHashSetAsync>d__.<>t__builder.Task;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks.CompilerServices;

namespace Cysharp.Threading.Tasks.Linq
{
	// Token: 0x02000079 RID: 121
	internal static class ToList
	{
		// Token: 0x060003AE RID: 942 RVA: 0x0000DA5C File Offset: 0x0000BC5C
		internal static UniTask<List<TSource>> ToListAsync<TSource>(IUniTaskAsyncEnumerable<TSource> source, CancellationToken cancellationToken)
		{
			ToList.<ToListAsync>d__0<TSource> <ToListAsync>d__;
			<ToListAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<List<TSource>>.Create();
			<ToListAsync>d__.source = source;
			<ToListAsync>d__.cancellationToken = cancellationToken;
			<ToListAsync>d__.<>1__state = -1;
			<ToListAsync>d__.<>t__builder.Start<ToList.<ToListAsync>d__0<TSource>>(ref <ToListAsync>d__);
			return <ToListAsync>d__.<>t__builder.Task;
		}
	}
}

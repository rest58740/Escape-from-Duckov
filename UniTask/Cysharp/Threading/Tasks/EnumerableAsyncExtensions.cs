using System;
using System.Collections.Generic;
using System.Linq;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000017 RID: 23
	public static class EnumerableAsyncExtensions
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00002BD5 File Offset: 0x00000DD5
		public static IEnumerable<UniTask> Select<T>(this IEnumerable<T> source, Func<T, UniTask> selector)
		{
			return source.Select(selector);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002BDE File Offset: 0x00000DDE
		public static IEnumerable<UniTask<TR>> Select<T, TR>(this IEnumerable<T> source, Func<T, UniTask<TR>> selector)
		{
			return source.Select(selector);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002BE7 File Offset: 0x00000DE7
		public static IEnumerable<UniTask> Select<T>(this IEnumerable<T> source, Func<T, int, UniTask> selector)
		{
			return source.Select(selector);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public static IEnumerable<UniTask<TR>> Select<T, TR>(this IEnumerable<T> source, Func<T, int, UniTask<TR>> selector)
		{
			return source.Select(selector);
		}
	}
}

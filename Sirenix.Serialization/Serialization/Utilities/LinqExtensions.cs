using System;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000B8 RID: 184
	internal static class LinqExtensions
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00023A48 File Offset: 0x00021C48
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T t in source)
			{
				action.Invoke(t);
			}
			return source;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00023A94 File Offset: 0x00021C94
		public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
		{
			int num = 0;
			foreach (T t in source)
			{
				action.Invoke(t, num++);
			}
			return source;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00023AE4 File Offset: 0x00021CE4
		public static IEnumerable<T> Append<T>(this IEnumerable<T> source, IEnumerable<T> append)
		{
			foreach (T t in source)
			{
				yield return t;
			}
			IEnumerator<T> enumerator = null;
			foreach (T t2 in append)
			{
				yield return t2;
			}
			enumerator = null;
			yield break;
			yield break;
		}
	}
}

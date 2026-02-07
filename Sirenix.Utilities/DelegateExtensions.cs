using System;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000003 RID: 3
	public static class DelegateExtensions
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000024FC File Offset: 0x000006FC
		public static Func<TResult> Memoize<TResult>(this Func<TResult> getValue)
		{
			TResult value = default(TResult);
			bool hasValue = false;
			return delegate()
			{
				if (!hasValue)
				{
					hasValue = true;
					value = getValue.Invoke();
				}
				return value;
			};
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002538 File Offset: 0x00000738
		public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
		{
			Dictionary<T, TResult> dic = new Dictionary<T, TResult>();
			return delegate(T n)
			{
				TResult tresult;
				if (!dic.TryGetValue(n, ref tresult))
				{
					tresult = func.Invoke(n);
					dic.Add(n, tresult);
				}
				return tresult;
			};
		}
	}
}

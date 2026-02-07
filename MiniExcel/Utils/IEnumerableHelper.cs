using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniExcelLibs.Utils
{
	// Token: 0x02000038 RID: 56
	public static class IEnumerableHelper
	{
		// Token: 0x06000175 RID: 373 RVA: 0x000063EC File Offset: 0x000045EC
		public static bool StartsWith<T>(this IList<T> span, IList<T> value) where T : IEquatable<T>
		{
			if (value.Count<T>() == 0)
			{
				return true;
			}
			IEnumerable<T> source = span.Take(value.Count<T>());
			if (source.Count<T>() != value.Count<T>())
			{
				return false;
			}
			for (int i = 0; i < source.Count<T>(); i++)
			{
				T t = span[i];
				if (!t.Equals(value[i]))
				{
					return false;
				}
			}
			return true;
		}
	}
}

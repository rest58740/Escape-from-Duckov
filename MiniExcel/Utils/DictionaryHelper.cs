using System;
using System.Collections.Generic;

namespace MiniExcelLibs.Utils
{
	// Token: 0x0200002C RID: 44
	internal static class DictionaryHelper
	{
		// Token: 0x06000134 RID: 308 RVA: 0x00005408 File Offset: 0x00003608
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
		{
			TValue result;
			if (!dictionary.TryGetValue(key, out result))
			{
				return defaultValue;
			}
			return result;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005424 File Offset: 0x00003624
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
		{
			TValue result;
			if (!dictionary.TryGetValue(key, out result))
			{
				return defaultValueProvider();
			}
			return result;
		}
	}
}

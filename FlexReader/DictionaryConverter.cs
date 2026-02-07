using System;
using System.Collections.Generic;

namespace FlexFramework.Excel
{
	// Token: 0x02000007 RID: 7
	public sealed class DictionaryConverter<TKey, TValue> : CustomConverter<Dictionary<TKey, TValue>>
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000024C0 File Offset: 0x000006C0
		public override Dictionary<TKey, TValue> Convert(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return null;
			}
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (string input2 in base.SplitGroup(input, '[', ']'))
			{
				string[] array = base.Split(input2, new char[]
				{
					','
				});
				TKey tkey = ValueConverter.Convert<TKey>(array[0]);
				TValue value = ValueConverter.Convert<TValue>(array[1]);
				if (dictionary.ContainsKey(tkey))
				{
					string str = "Dictionary key is duplicate: ";
					TKey tkey2 = tkey;
					throw new FormatException(str + ((tkey2 != null) ? tkey2.ToString() : null));
				}
				dictionary.Add(tkey, value);
			}
			return dictionary;
		}
	}
}

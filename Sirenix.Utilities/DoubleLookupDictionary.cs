using System;
using System.Collections.Generic;

namespace Sirenix.Utilities
{
	// Token: 0x02000014 RID: 20
	[Serializable]
	public class DoubleLookupDictionary<TFirstKey, TSecondKey, TValue> : Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>
	{
		// Token: 0x06000107 RID: 263 RVA: 0x00008A2B File Offset: 0x00006C2B
		public DoubleLookupDictionary()
		{
			this.secondKeyComparer = EqualityComparer<TSecondKey>.Default;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00008A3E File Offset: 0x00006C3E
		public DoubleLookupDictionary(IEqualityComparer<TFirstKey> firstKeyComparer, IEqualityComparer<TSecondKey> secondKeyComparer) : base(firstKeyComparer)
		{
			this.secondKeyComparer = secondKeyComparer;
		}

		// Token: 0x17000001 RID: 1
		public Dictionary<TSecondKey, TValue> this[TFirstKey firstKey]
		{
			get
			{
				Dictionary<TSecondKey, TValue> dictionary;
				if (!base.TryGetValue(firstKey, ref dictionary))
				{
					dictionary = new Dictionary<TSecondKey, TValue>(this.secondKeyComparer);
					base.Add(firstKey, dictionary);
				}
				return dictionary;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00008A80 File Offset: 0x00006C80
		public int InnerCount(TFirstKey firstKey)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			if (base.TryGetValue(firstKey, ref dictionary))
			{
				return dictionary.Count;
			}
			return 0;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00008AA0 File Offset: 0x00006CA0
		public int TotalInnerCount()
		{
			int num = 0;
			if (base.Count > 0)
			{
				foreach (Dictionary<TSecondKey, TValue> dictionary in base.Values)
				{
					num += dictionary.Count;
				}
			}
			return num;
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00008B04 File Offset: 0x00006D04
		public bool ContainsKeys(TFirstKey firstKey, TSecondKey secondKey)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			return base.TryGetValue(firstKey, ref dictionary) && dictionary.ContainsKey(secondKey);
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00008B28 File Offset: 0x00006D28
		public bool TryGetInnerValue(TFirstKey firstKey, TSecondKey secondKey, out TValue value)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			if (base.TryGetValue(firstKey, ref dictionary) && dictionary.TryGetValue(secondKey, ref value))
			{
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00008B54 File Offset: 0x00006D54
		public TValue AddInner(TFirstKey firstKey, TSecondKey secondKey, TValue value)
		{
			if (this.ContainsKeys(firstKey, secondKey))
			{
				throw new ArgumentException("An element with the same keys already exists in the " + base.GetType().GetNiceName() + ".");
			}
			this[firstKey][secondKey] = value;
			return value;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00008B9C File Offset: 0x00006D9C
		public bool RemoveInner(TFirstKey firstKey, TSecondKey secondKey)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			if (base.TryGetValue(firstKey, ref dictionary))
			{
				bool result = dictionary.Remove(secondKey);
				if (dictionary.Count == 0)
				{
					base.Remove(firstKey);
				}
				return result;
			}
			return false;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00008BD0 File Offset: 0x00006DD0
		public void RemoveWhere(Func<TValue, bool> predicate)
		{
			List<TFirstKey> list = new List<TFirstKey>();
			List<TSecondKey> list2 = new List<TSecondKey>();
			foreach (KeyValuePair<TFirstKey, Dictionary<TSecondKey, TValue>> keyValuePair in this.GFIterator<TFirstKey, Dictionary<TSecondKey, TValue>>())
			{
				foreach (KeyValuePair<TSecondKey, TValue> keyValuePair2 in keyValuePair.Value.GFIterator<TSecondKey, TValue>())
				{
					if (predicate.Invoke(keyValuePair2.Value))
					{
						list.Add(keyValuePair.Key);
						list2.Add(keyValuePair2.Key);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.RemoveInner(list[i], list2[i]);
			}
		}

		// Token: 0x0400003B RID: 59
		private readonly IEqualityComparer<TSecondKey> secondKeyComparer;
	}
}

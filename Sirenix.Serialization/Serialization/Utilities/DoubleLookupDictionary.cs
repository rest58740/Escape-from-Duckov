using System;
using System.Collections.Generic;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000C3 RID: 195
	[Serializable]
	internal class DoubleLookupDictionary<TFirstKey, TSecondKey, TValue> : Dictionary<TFirstKey, Dictionary<TSecondKey, TValue>>
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x00027305 File Offset: 0x00025505
		public DoubleLookupDictionary()
		{
			this.secondKeyComparer = EqualityComparer<TSecondKey>.Default;
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x00027318 File Offset: 0x00025518
		public DoubleLookupDictionary(IEqualityComparer<TFirstKey> firstKeyComparer, IEqualityComparer<TSecondKey> secondKeyComparer) : base(firstKeyComparer)
		{
			this.secondKeyComparer = secondKeyComparer;
		}

		// Token: 0x17000061 RID: 97
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

		// Token: 0x06000582 RID: 1410 RVA: 0x00027358 File Offset: 0x00025558
		public int InnerCount(TFirstKey firstKey)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			if (base.TryGetValue(firstKey, ref dictionary))
			{
				return dictionary.Count;
			}
			return 0;
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00027378 File Offset: 0x00025578
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

		// Token: 0x06000584 RID: 1412 RVA: 0x000273DC File Offset: 0x000255DC
		public bool ContainsKeys(TFirstKey firstKey, TSecondKey secondKey)
		{
			Dictionary<TSecondKey, TValue> dictionary;
			return base.TryGetValue(firstKey, ref dictionary) && dictionary.ContainsKey(secondKey);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00027400 File Offset: 0x00025600
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

		// Token: 0x06000586 RID: 1414 RVA: 0x0002742C File Offset: 0x0002562C
		public TValue AddInner(TFirstKey firstKey, TSecondKey secondKey, TValue value)
		{
			if (this.ContainsKeys(firstKey, secondKey))
			{
				throw new ArgumentException("An element with the same keys already exists in the " + base.GetType().GetNiceName() + ".");
			}
			this[firstKey][secondKey] = value;
			return value;
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00027474 File Offset: 0x00025674
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

		// Token: 0x06000588 RID: 1416 RVA: 0x000274A8 File Offset: 0x000256A8
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

		// Token: 0x0400020D RID: 525
		private readonly IEqualityComparer<TSecondKey> secondKeyComparer;
	}
}

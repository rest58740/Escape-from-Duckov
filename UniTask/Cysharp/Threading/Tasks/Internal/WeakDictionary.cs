using System;
using System.Collections.Generic;
using System.Threading;

namespace Cysharp.Threading.Tasks.Internal
{
	// Token: 0x0200011A RID: 282
	internal class WeakDictionary<TKey, TValue> where TKey : class
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x0000EE04 File Offset: 0x0000D004
		public WeakDictionary(int capacity = 4, float loadFactor = 0.75f, IEqualityComparer<TKey> keyComparer = null)
		{
			int num = WeakDictionary<TKey, TValue>.CalculateCapacity(capacity, loadFactor);
			this.buckets = new WeakDictionary<TKey, TValue>.Entry[num];
			this.loadFactor = loadFactor;
			this.gate = new SpinLock(false);
			this.keyEqualityComparer = (keyComparer ?? EqualityComparer<TKey>.Default);
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0000EE50 File Offset: 0x0000D050
		public bool TryAdd(TKey key, TValue value)
		{
			bool flag = false;
			bool result;
			try
			{
				this.gate.Enter(ref flag);
				result = this.TryAddInternal(key, value);
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
			return result;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000EE98 File Offset: 0x0000D098
		public bool TryGetValue(TKey key, out TValue value)
		{
			bool flag = false;
			bool result;
			try
			{
				this.gate.Enter(ref flag);
				int num;
				WeakDictionary<TKey, TValue>.Entry entry;
				if (this.TryGetEntry(key, out num, out entry))
				{
					value = entry.Value;
					result = true;
				}
				else
				{
					value = default(TValue);
					result = false;
				}
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
			return result;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0000EF00 File Offset: 0x0000D100
		public bool TryRemove(TKey key)
		{
			bool flag = false;
			bool result;
			try
			{
				this.gate.Enter(ref flag);
				int hashIndex;
				WeakDictionary<TKey, TValue>.Entry entry;
				if (this.TryGetEntry(key, out hashIndex, out entry))
				{
					this.Remove(hashIndex, entry);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
			return result;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0000EF5C File Offset: 0x0000D15C
		private bool TryAddInternal(TKey key, TValue value)
		{
			int num = WeakDictionary<TKey, TValue>.CalculateCapacity(this.size + 1, this.loadFactor);
			while (this.buckets.Length < num)
			{
				WeakDictionary<TKey, TValue>.Entry[] targetBuckets = new WeakDictionary<TKey, TValue>.Entry[num];
				for (int i = 0; i < this.buckets.Length; i++)
				{
					for (WeakDictionary<TKey, TValue>.Entry entry = this.buckets[i]; entry != null; entry = entry.Next)
					{
						this.AddToBuckets(targetBuckets, key, entry.Value, entry.Hash);
					}
				}
				this.buckets = targetBuckets;
			}
			bool flag = this.AddToBuckets(this.buckets, key, value, this.keyEqualityComparer.GetHashCode(key));
			if (flag)
			{
				this.size++;
			}
			return flag;
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0000F000 File Offset: 0x0000D200
		private bool AddToBuckets(WeakDictionary<TKey, TValue>.Entry[] targetBuckets, TKey newKey, TValue value, int keyHash)
		{
			int num = keyHash & targetBuckets.Length - 1;
			IL_0B:
			while (targetBuckets[num] != null)
			{
				WeakDictionary<TKey, TValue>.Entry entry = targetBuckets[num];
				while (entry != null)
				{
					TKey y;
					if (entry.Key.TryGetTarget(out y))
					{
						if (this.keyEqualityComparer.Equals(newKey, y))
						{
							return false;
						}
					}
					else
					{
						this.Remove(num, entry);
						if (targetBuckets[num] == null)
						{
							goto IL_0B;
						}
					}
					if (entry.Next != null)
					{
						entry = entry.Next;
					}
					else
					{
						entry.Next = new WeakDictionary<TKey, TValue>.Entry
						{
							Key = new WeakReference<TKey>(newKey, false),
							Value = value,
							Hash = keyHash
						};
						entry.Next.Prev = entry;
					}
				}
				return false;
			}
			targetBuckets[num] = new WeakDictionary<TKey, TValue>.Entry
			{
				Key = new WeakReference<TKey>(newKey, false),
				Value = value,
				Hash = keyHash
			};
			return true;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0000F0BC File Offset: 0x0000D2BC
		private bool TryGetEntry(TKey key, out int hashIndex, out WeakDictionary<TKey, TValue>.Entry entry)
		{
			WeakDictionary<TKey, TValue>.Entry[] array = this.buckets;
			int hashCode = this.keyEqualityComparer.GetHashCode(key);
			hashIndex = (hashCode & array.Length - 1);
			for (entry = array[hashIndex]; entry != null; entry = entry.Next)
			{
				TKey y;
				if (entry.Key.TryGetTarget(out y))
				{
					if (this.keyEqualityComparer.Equals(key, y))
					{
						return true;
					}
				}
				else
				{
					this.Remove(hashIndex, entry);
				}
			}
			return false;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0000F128 File Offset: 0x0000D328
		private void Remove(int hashIndex, WeakDictionary<TKey, TValue>.Entry entry)
		{
			if (entry.Prev == null && entry.Next == null)
			{
				this.buckets[hashIndex] = null;
			}
			else
			{
				if (entry.Prev == null)
				{
					this.buckets[hashIndex] = entry.Next;
				}
				if (entry.Prev != null)
				{
					entry.Prev.Next = entry.Next;
				}
				if (entry.Next != null)
				{
					entry.Next.Prev = entry.Prev;
				}
			}
			this.size--;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public List<KeyValuePair<TKey, TValue>> ToList()
		{
			List<KeyValuePair<TKey, TValue>> result = new List<KeyValuePair<TKey, TValue>>(this.size);
			this.ToList(ref result, false);
			return result;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		public int ToList(ref List<KeyValuePair<TKey, TValue>> list, bool clear = true)
		{
			if (clear)
			{
				list.Clear();
			}
			int num = 0;
			bool flag = false;
			try
			{
				for (int i = 0; i < this.buckets.Length; i++)
				{
					for (WeakDictionary<TKey, TValue>.Entry entry = this.buckets[i]; entry != null; entry = entry.Next)
					{
						TKey key;
						if (entry.Key.TryGetTarget(out key))
						{
							KeyValuePair<TKey, TValue> keyValuePair = new KeyValuePair<TKey, TValue>(key, entry.Value);
							if (num < list.Count)
							{
								list[num++] = keyValuePair;
							}
							else
							{
								list.Add(keyValuePair);
								num++;
							}
						}
						else
						{
							this.Remove(i, entry);
						}
					}
				}
			}
			finally
			{
				if (flag)
				{
					this.gate.Exit(false);
				}
			}
			return num;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0000F284 File Offset: 0x0000D484
		private static int CalculateCapacity(int collectionSize, float loadFactor)
		{
			int num = (int)((float)collectionSize / loadFactor);
			num--;
			num |= num >> 1;
			num |= num >> 2;
			num |= num >> 4;
			num |= num >> 8;
			num |= num >> 16;
			num++;
			if (num < 8)
			{
				num = 8;
			}
			return num;
		}

		// Token: 0x04000145 RID: 325
		private WeakDictionary<TKey, TValue>.Entry[] buckets;

		// Token: 0x04000146 RID: 326
		private int size;

		// Token: 0x04000147 RID: 327
		private SpinLock gate;

		// Token: 0x04000148 RID: 328
		private readonly float loadFactor;

		// Token: 0x04000149 RID: 329
		private readonly IEqualityComparer<TKey> keyEqualityComparer;

		// Token: 0x0200021C RID: 540
		private class Entry
		{
			// Token: 0x06000BFD RID: 3069 RVA: 0x0002B1F0 File Offset: 0x000293F0
			public override string ToString()
			{
				TKey tkey;
				if (this.Key.TryGetTarget(out tkey))
				{
					TKey tkey2 = tkey;
					return ((tkey2 != null) ? tkey2.ToString() : null) + "(" + this.Count().ToString() + ")";
				}
				return "(Dead)";
			}

			// Token: 0x06000BFE RID: 3070 RVA: 0x0002B244 File Offset: 0x00029444
			private int Count()
			{
				int num = 1;
				WeakDictionary<TKey, TValue>.Entry entry = this;
				while (entry.Next != null)
				{
					num++;
					entry = entry.Next;
				}
				return num;
			}

			// Token: 0x04000550 RID: 1360
			public WeakReference<TKey> Key;

			// Token: 0x04000551 RID: 1361
			public TValue Value;

			// Token: 0x04000552 RID: 1362
			public int Hash;

			// Token: 0x04000553 RID: 1363
			public WeakDictionary<TKey, TValue>.Entry Prev;

			// Token: 0x04000554 RID: 1364
			public WeakDictionary<TKey, TValue>.Entry Next;
		}
	}
}

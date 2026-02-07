using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemStatsSystem
{
	// Token: 0x02000023 RID: 35
	public class StatCollection : ItemComponent, ICollection<Stat>, IEnumerable<Stat>, IEnumerable
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001EA RID: 490 RVA: 0x00007E0D File Offset: 0x0000600D
		private Dictionary<int, Stat> statsDictionary
		{
			get
			{
				if (this._cachedStatsDictionary == null)
				{
					this.BuildDictionary();
				}
				return this._cachedStatsDictionary;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001EB RID: 491 RVA: 0x00007E23 File Offset: 0x00006023
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001EC RID: 492 RVA: 0x00007E30 File Offset: 0x00006030
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00007E34 File Offset: 0x00006034
		public Stat GetStat(int hash)
		{
			Stat result;
			if (this.statsDictionary.TryGetValue(hash, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00007E54 File Offset: 0x00006054
		public Stat GetStat(string key)
		{
			int hashCode = key.GetHashCode();
			Stat stat = this.GetStat(hashCode);
			if (stat == null)
			{
				stat = this.list.Find((Stat e) => e.Key == key);
			}
			return stat;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00007EA0 File Offset: 0x000060A0
		private void BuildDictionary()
		{
			if (this._cachedStatsDictionary == null)
			{
				this._cachedStatsDictionary = new Dictionary<int, Stat>();
			}
			this._cachedStatsDictionary.Clear();
			foreach (Stat stat in this.list)
			{
				int hashCode = stat.Key.GetHashCode();
				this._cachedStatsDictionary[hashCode] = stat;
			}
		}

		// Token: 0x17000089 RID: 137
		public Stat this[int hash]
		{
			get
			{
				return this.GetStat(hash);
			}
		}

		// Token: 0x1700008A RID: 138
		public Stat this[string key]
		{
			get
			{
				return this.GetStat(key);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00007F38 File Offset: 0x00006138
		internal override void OnInitialize()
		{
			foreach (Stat stat in this.list)
			{
				stat.Initialize(this);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00007F8C File Offset: 0x0000618C
		public IEnumerator<Stat> GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00007F9E File Offset: 0x0000619E
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00007FB0 File Offset: 0x000061B0
		public void Add(Stat item)
		{
			this.list.Add(item);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00007FBE File Offset: 0x000061BE
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00007FCB File Offset: 0x000061CB
		public bool Contains(Stat item)
		{
			return this.list.Contains(item);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00007FD9 File Offset: 0x000061D9
		public void CopyTo(Stat[] array, int arrayIndex)
		{
			this.list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00007FE8 File Offset: 0x000061E8
		public bool Remove(Stat item)
		{
			return this.list.Remove(item);
		}

		// Token: 0x040000AC RID: 172
		[SerializeField]
		private List<Stat> list = new List<Stat>();

		// Token: 0x040000AD RID: 173
		private Dictionary<int, Stat> _cachedStatsDictionary;
	}
}

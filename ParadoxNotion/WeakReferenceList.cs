using System;
using System.Collections.Generic;

namespace ParadoxNotion
{
	// Token: 0x0200007E RID: 126
	public class WeakReferenceList<T> where T : class
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0000DBA6 File Offset: 0x0000BDA6
		public int Count
		{
			get
			{
				return this.list.Count;
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000DBB3 File Offset: 0x0000BDB3
		public WeakReferenceList()
		{
			this.list = new List<WeakReference<T>>();
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0000DBC6 File Offset: 0x0000BDC6
		public WeakReferenceList(int capacity)
		{
			this.list = new List<WeakReference<T>>(capacity);
		}

		// Token: 0x17000111 RID: 273
		public T this[int i]
		{
			get
			{
				T result;
				this.list[i].TryGetTarget(ref result);
				return result;
			}
			set
			{
				this.list[i].SetTarget(value);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000DC12 File Offset: 0x0000BE12
		public void Add(T item)
		{
			this.list.Add(new WeakReference<T>(item));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000DC28 File Offset: 0x0000BE28
		public void Remove(T item)
		{
			int count = this.list.Count;
			while (count-- > 0)
			{
				WeakReference<T> weakReference = this.list[count];
				T t;
				if (weakReference.TryGetTarget(ref t) && t == item)
				{
					this.list.Remove(weakReference);
				}
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000DC80 File Offset: 0x0000BE80
		public bool Contains(T item, out int index)
		{
			for (int i = 0; i < this.list.Count; i++)
			{
				T t;
				if (this.list[i].TryGetTarget(ref t) && t == item)
				{
					index = i;
					return true;
				}
			}
			index = -1;
			return false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000DCCF File Offset: 0x0000BECF
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000DCDC File Offset: 0x0000BEDC
		public List<T> ToReferenceList()
		{
			List<T> list = new List<T>();
			for (int i = 0; i < this.list.Count; i++)
			{
				T t;
				if (this.list[i].TryGetTarget(ref t))
				{
					list.Add(t);
				}
			}
			return list;
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public static implicit operator WeakReferenceList<T>(List<T> value)
		{
			WeakReferenceList<T> weakReferenceList = new WeakReferenceList<T>(value.Count);
			for (int i = 0; i < value.Count; i++)
			{
				weakReferenceList.Add(value[i]);
			}
			return weakReferenceList;
		}

		// Token: 0x0400017E RID: 382
		private List<WeakReference<T>> list;
	}
}

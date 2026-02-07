using System;
using System.Collections.Generic;

namespace Shapes
{
	// Token: 0x02000055 RID: 85
	public class ExpandoList<T>
	{
		// Token: 0x170001D5 RID: 469
		public T this[int i]
		{
			get
			{
				if (i < 0)
				{
					throw new IndexOutOfRangeException();
				}
				if (i >= this.list.Count)
				{
					return default(T);
				}
				return this.list[i];
			}
			set
			{
				if (i < 0)
				{
					throw new IndexOutOfRangeException();
				}
				int count = this.list.Count;
				if (i < count)
				{
					this.list[i] = value;
					return;
				}
				int num = i - count;
				if (num > 0)
				{
					for (int j = 0; j < num; j++)
					{
						this.list.Add(default(T));
					}
				}
				this.list.Add(value);
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x000190B0 File Offset: 0x000172B0
		public void SetCountToAtLeast(int minCount)
		{
			int count = this.list.Count;
			if (count < minCount)
			{
				int num = minCount - count;
				for (int i = 0; i < num; i++)
				{
					this.list.Add(default(T));
				}
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x000190F1 File Offset: 0x000172F1
		public void Add(T item)
		{
			this.list.Add(item);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000190FF File Offset: 0x000172FF
		public void Clear()
		{
			this.list.Clear();
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0001910C File Offset: 0x0001730C
		public void ClearAndSetMinCapacity(int minCapacity)
		{
			this.list.Clear();
			if (this.list.Capacity < minCapacity)
			{
				this.list.Capacity = minCapacity;
			}
		}

		// Token: 0x040001E7 RID: 487
		public List<T> list = new List<T>();
	}
}

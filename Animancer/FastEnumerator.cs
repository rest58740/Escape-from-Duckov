using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Animancer
{
	// Token: 0x02000008 RID: 8
	public struct FastEnumerator<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00002F6F File Offset: 0x0000116F
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00002F77 File Offset: 0x00001177
		public int Count
		{
			get
			{
				return this._Count;
			}
			set
			{
				this._Count = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00002F80 File Offset: 0x00001180
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00002F88 File Offset: 0x00001188
		public int Index
		{
			get
			{
				return this._Index;
			}
			set
			{
				this._Index = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002F91 File Offset: 0x00001191
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002FA4 File Offset: 0x000011A4
		public T Current
		{
			get
			{
				return this.List[this._Index];
			}
			set
			{
				this.List[this._Index] = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00002FB8 File Offset: 0x000011B8
		object IEnumerator.Current
		{
			get
			{
				return this.Current;
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00002FC5 File Offset: 0x000011C5
		public FastEnumerator(IList<T> list)
		{
			this = new FastEnumerator<T>(list, list.Count);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00002FD4 File Offset: 0x000011D4
		public FastEnumerator(IList<T> list, int count)
		{
			this.List = list;
			this._Count = count;
			this._Index = -1;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002FEB File Offset: 0x000011EB
		public bool MoveNext()
		{
			this._Index++;
			if (this._Index < this._Count)
			{
				return true;
			}
			this._Index = int.MinValue;
			return false;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003017 File Offset: 0x00001217
		public bool MovePrevious()
		{
			if (this._Index > 0)
			{
				this._Index--;
				return true;
			}
			this._Index = -1;
			return false;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x0000303A File Offset: 0x0000123A
		public void Reset()
		{
			this._Index = -1;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003043 File Offset: 0x00001243
		void IDisposable.Dispose()
		{
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00003045 File Offset: 0x00001245
		public FastEnumerator<T> GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000304D File Offset: 0x0000124D
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000305A File Offset: 0x0000125A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00003067 File Offset: 0x00001267
		public int IndexOf(T item)
		{
			return this.List.IndexOf(item);
		}

		// Token: 0x17000034 RID: 52
		public T this[int index]
		{
			get
			{
				return this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00003092 File Offset: 0x00001292
		public void Insert(int index, T item)
		{
			this.List.Insert(index, item);
			if (this._Index >= index)
			{
				this._Index++;
			}
			this._Count++;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000030C6 File Offset: 0x000012C6
		public void RemoveAt(int index)
		{
			this.List.RemoveAt(index);
			if (this._Index >= index)
			{
				this._Index--;
			}
			this._Count--;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000030F9 File Offset: 0x000012F9
		public bool IsReadOnly
		{
			get
			{
				return this.List.IsReadOnly;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00003106 File Offset: 0x00001306
		public bool Contains(T item)
		{
			return this.List.Contains(item);
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00003114 File Offset: 0x00001314
		public void Add(T item)
		{
			this.List.Add(item);
			this._Count++;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00003130 File Offset: 0x00001330
		public bool Remove(T item)
		{
			int num = this.List.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003158 File Offset: 0x00001358
		public void Clear()
		{
			this.List.Clear();
			this._Index = -1;
			this._Count = 0;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00003174 File Offset: 0x00001374
		public void CopyTo(T[] array, int arrayIndex)
		{
			for (int i = 0; i < this._Count; i++)
			{
				array[arrayIndex + i] = this.List[i];
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000031A7 File Offset: 0x000013A7
		[Conditional("UNITY_ASSERTIONS")]
		private void AssertIndex(int index)
		{
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000031A9 File Offset: 0x000013A9
		[Conditional("UNITY_ASSERTIONS")]
		private void AssertCount(int count)
		{
		}

		// Token: 0x04000008 RID: 8
		private readonly IList<T> List;

		// Token: 0x04000009 RID: 9
		private int _Count;

		// Token: 0x0400000A RID: 10
		private int _Index;
	}
}

using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200002E RID: 46
	[Serializable]
	public class SortedFastList<T> : FastList<T>
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00009308 File Offset: 0x00007508
		public new void RemoveAt(int index)
		{
			if (index >= this._count)
			{
				Debug.LogError("Index " + index.ToString() + " is out of range " + this._count.ToString());
			}
			this._count--;
			if (index < this._count)
			{
				Array.Copy(this.items, index + 1, this.items, index, this._count - index);
			}
			this.items[this._count] = default(T);
			this.Count = this._count;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000093A0 File Offset: 0x000075A0
		public new void RemoveRange(int index, int endIndex)
		{
			int num = endIndex - index + 1;
			if (index < 0)
			{
				Debug.LogError("Index needs to be bigger than 0 -> " + index.ToString());
				return;
			}
			if (num < 0)
			{
				Debug.LogError("Length needs to be bigger than 0 -> " + num.ToString());
				return;
			}
			if (this._count - index < num)
			{
				return;
			}
			this._count -= num;
			if (index < this._count)
			{
				Array.Copy(this.items, index + num, this.items, index, this._count - index);
			}
			Array.Clear(this.items, this._count, num);
		}
	}
}

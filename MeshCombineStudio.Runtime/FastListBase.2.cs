using System;

namespace MeshCombineStudio
{
	// Token: 0x0200002C RID: 44
	public class FastListBase<T> : FastListBase
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00008714 File Offset: 0x00006914
		protected void DoubleCapacity()
		{
			this.arraySize *= 2;
			T[] destinationArray = new T[this.arraySize];
			Array.Copy(this.items, destinationArray, this._count);
			this.items = destinationArray;
		}

		// Token: 0x0400011B RID: 283
		public T[] items;
	}
}

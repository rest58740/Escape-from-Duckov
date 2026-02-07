using System;

namespace MeshCombineStudio
{
	// Token: 0x02000021 RID: 33
	public class ObjectHolder<T> : FastIndex
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00008158 File Offset: 0x00006358
		public ObjectHolder()
		{
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00008160 File Offset: 0x00006360
		public ObjectHolder(T item)
		{
			this.item = item;
		}

		// Token: 0x0400010E RID: 270
		public T item;
	}
}

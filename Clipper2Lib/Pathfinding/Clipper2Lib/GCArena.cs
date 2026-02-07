using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pathfinding.Clipper2Lib
{
	// Token: 0x02000032 RID: 50
	[NullableContext(1)]
	[Nullable(0)]
	internal class GCArena<T> where T : class, IDisposable, new()
	{
		// Token: 0x060001D5 RID: 469 RVA: 0x0000E244 File Offset: 0x0000C444
		public void Reclaim()
		{
			this.index = 0;
			foreach (T t in this.arena)
			{
				t.Dispose();
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		public T Get()
		{
			if (this.index < this.arena.Count)
			{
				List<T> list = this.arena;
				int num = this.index;
				this.index = num + 1;
				return list[num];
			}
			T t = Activator.CreateInstance<T>();
			this.arena.Add(t);
			this.index++;
			return t;
		}

		// Token: 0x040000B9 RID: 185
		private List<T> arena = new List<T>();

		// Token: 0x040000BA RID: 186
		private int index;
	}
}

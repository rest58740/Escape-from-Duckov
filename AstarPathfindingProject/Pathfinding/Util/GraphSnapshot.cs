using System;
using System.Collections.Generic;

namespace Pathfinding.Util
{
	// Token: 0x02000280 RID: 640
	public struct GraphSnapshot : IGraphSnapshot, IDisposable
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x0005D9D4 File Offset: 0x0005BBD4
		internal GraphSnapshot(List<IGraphSnapshot> inner)
		{
			this.inner = inner;
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x0005D9E0 File Offset: 0x0005BBE0
		public void Restore(IGraphUpdateContext ctx)
		{
			for (int i = 0; i < this.inner.Count; i++)
			{
				this.inner[i].Restore(ctx);
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x0005DA18 File Offset: 0x0005BC18
		public void Dispose()
		{
			if (this.inner != null)
			{
				for (int i = 0; i < this.inner.Count; i++)
				{
					this.inner[i].Dispose();
				}
				this.inner = null;
			}
		}

		// Token: 0x04000B52 RID: 2898
		private List<IGraphSnapshot> inner;
	}
}

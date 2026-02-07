using System;

namespace Pathfinding
{
	// Token: 0x0200008E RID: 142
	public struct AstarWorkItem
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x000172D0 File Offset: 0x000154D0
		public AstarWorkItem(Func<bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = null;
			this.update = update;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x000172EE File Offset: 0x000154EE
		public AstarWorkItem(Func<IWorkItemContext, bool, bool> update)
		{
			this.init = null;
			this.initWithContext = null;
			this.updateWithContext = update;
			this.update = null;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001730C File Offset: 0x0001550C
		public AstarWorkItem(Action init, Func<bool, bool> update = null)
		{
			this.init = init;
			this.initWithContext = null;
			this.update = update;
			this.updateWithContext = null;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001732A File Offset: 0x0001552A
		public AstarWorkItem(Action<IWorkItemContext> init, Func<IWorkItemContext, bool, bool> update = null)
		{
			this.init = null;
			this.initWithContext = init;
			this.update = null;
			this.updateWithContext = update;
		}

		// Token: 0x04000303 RID: 771
		public Action init;

		// Token: 0x04000304 RID: 772
		public Action<IWorkItemContext> initWithContext;

		// Token: 0x04000305 RID: 773
		public Func<bool, bool> update;

		// Token: 0x04000306 RID: 774
		public Func<IWorkItemContext, bool, bool> updateWithContext;
	}
}

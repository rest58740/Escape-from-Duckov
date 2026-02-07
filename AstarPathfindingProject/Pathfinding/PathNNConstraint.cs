using System;

namespace Pathfinding
{
	// Token: 0x02000029 RID: 41
	public class PathNNConstraint : NNConstraint
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00009E2E File Offset: 0x0000802E
		public new static PathNNConstraint Walkable
		{
			get
			{
				return new PathNNConstraint
				{
					constrainArea = true
				};
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009E3C File Offset: 0x0000803C
		public virtual void SetStart(GraphNode node)
		{
			if (node != null)
			{
				this.area = (int)node.Area;
				return;
			}
			this.constrainArea = false;
		}
	}
}

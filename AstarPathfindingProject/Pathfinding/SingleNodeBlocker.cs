using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013F RID: 319
	[HelpURL("https://arongranberg.com/astar/documentation/stable/singlenodeblocker.html")]
	public class SingleNodeBlocker : VersionedMonoBehaviour
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00034BC6 File Offset: 0x00032DC6
		// (set) Token: 0x060009A8 RID: 2472 RVA: 0x00034BCE File Offset: 0x00032DCE
		public GraphNode lastBlocked { get; private set; }

		// Token: 0x060009A9 RID: 2473 RVA: 0x00034BD7 File Offset: 0x00032DD7
		public void BlockAtCurrentPosition()
		{
			this.BlockAt(base.transform.position);
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x00034BEC File Offset: 0x00032DEC
		public void BlockAt(Vector3 position)
		{
			this.Unblock();
			GraphNode node = AstarPath.active.GetNearest(position, NNConstraint.None).node;
			if (node != null)
			{
				this.Block(node);
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00034C1F File Offset: 0x00032E1F
		public void Block(GraphNode node)
		{
			this.Unblock();
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this.manager.InternalBlock(node, this);
			this.lastBlocked = node;
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00034C49 File Offset: 0x00032E49
		public void Unblock()
		{
			if (this.lastBlocked == null || this.lastBlocked.Destroyed)
			{
				this.lastBlocked = null;
				return;
			}
			this.manager.InternalUnblock(this.lastBlocked, this);
			this.lastBlocked = null;
		}

		// Token: 0x0400069A RID: 1690
		public BlockManager manager;
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020002CD RID: 717
	[HelpURL("https://arongranberg.com/astar/documentation/stable/turnbasedai.html")]
	public class TurnBasedAI : VersionedMonoBehaviour
	{
		// Token: 0x06001113 RID: 4371 RVA: 0x0006ADFE File Offset: 0x00068FFE
		private void Start()
		{
			this.blocker.BlockAtCurrentPosition();
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x0006AE0B File Offset: 0x0006900B
		protected override void Awake()
		{
			base.Awake();
			this.traversalProvider = new BlockManager.TraversalProvider(this.blockManager, BlockManager.BlockMode.AllExceptSelector, new List<SingleNodeBlocker>
			{
				this.blocker
			});
		}

		// Token: 0x04000CDC RID: 3292
		public int movementPoints = 2;

		// Token: 0x04000CDD RID: 3293
		public BlockManager blockManager;

		// Token: 0x04000CDE RID: 3294
		public SingleNodeBlocker blocker;

		// Token: 0x04000CDF RID: 3295
		public GraphNode targetNode;

		// Token: 0x04000CE0 RID: 3296
		public BlockManager.TraversalProvider traversalProvider;
	}
}

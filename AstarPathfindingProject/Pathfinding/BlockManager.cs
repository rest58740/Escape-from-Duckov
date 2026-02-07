using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200013A RID: 314
	[HelpURL("https://arongranberg.com/astar/documentation/stable/blockmanager.html")]
	public class BlockManager : VersionedMonoBehaviour
	{
		// Token: 0x06000996 RID: 2454 RVA: 0x000348D2 File Offset: 0x00032AD2
		private void Start()
		{
			if (!AstarPath.active)
			{
				throw new Exception("No AstarPath object in the scene");
			}
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000348EC File Offset: 0x00032AEC
		public bool NodeContainsAnyOf(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00034944 File Offset: 0x00032B44
		public bool NodeContainsAnyExcept(GraphNode node, List<SingleNodeBlocker> selector)
		{
			List<SingleNodeBlocker> list;
			if (!this.blocked.TryGetValue(node, out list))
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				SingleNodeBlocker singleNodeBlocker = list[i];
				bool flag = false;
				for (int j = 0; j < selector.Count; j++)
				{
					if (singleNodeBlocker == selector[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000349A8 File Offset: 0x00032BA8
		public void InternalBlock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate()
			{
				List<SingleNodeBlocker> list;
				if (!this.blocked.TryGetValue(node, out list))
				{
					list = (this.blocked[node] = ListPool<SingleNodeBlocker>.Claim());
				}
				list.Add(blocker);
			}, null));
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000349EC File Offset: 0x00032BEC
		public void InternalUnblock(GraphNode node, SingleNodeBlocker blocker)
		{
			AstarPath.active.AddWorkItem(new AstarWorkItem(delegate()
			{
				List<SingleNodeBlocker> list;
				if (this.blocked.TryGetValue(node, out list))
				{
					list.Remove(blocker);
					if (list.Count == 0)
					{
						this.blocked.Remove(node);
						ListPool<SingleNodeBlocker>.Release(ref list);
					}
				}
			}, null));
		}

		// Token: 0x0400068C RID: 1676
		private Dictionary<GraphNode, List<SingleNodeBlocker>> blocked = new Dictionary<GraphNode, List<SingleNodeBlocker>>();

		// Token: 0x0200013B RID: 315
		public enum BlockMode
		{
			// Token: 0x0400068E RID: 1678
			AllExceptSelector,
			// Token: 0x0400068F RID: 1679
			OnlySelector
		}

		// Token: 0x0200013C RID: 316
		public class TraversalProvider : ITraversalProvider
		{
			// Token: 0x17000187 RID: 391
			// (get) Token: 0x0600099C RID: 2460 RVA: 0x00034A43 File Offset: 0x00032C43
			// (set) Token: 0x0600099D RID: 2461 RVA: 0x00034A4B File Offset: 0x00032C4B
			public BlockManager.BlockMode mode { get; private set; }

			// Token: 0x0600099E RID: 2462 RVA: 0x00034A54 File Offset: 0x00032C54
			public TraversalProvider(BlockManager blockManager, BlockManager.BlockMode mode, List<SingleNodeBlocker> selector)
			{
				if (blockManager == null)
				{
					throw new ArgumentNullException("blockManager");
				}
				if (selector == null)
				{
					throw new ArgumentNullException("selector");
				}
				this.blockManager = blockManager;
				this.mode = mode;
				this.selector = selector;
			}

			// Token: 0x0600099F RID: 2463 RVA: 0x00034A94 File Offset: 0x00032C94
			public bool CanTraverse(Path path, GraphNode node)
			{
				if (!node.Walkable || (path != null && (path.enabledTags >> (int)node.Tag & 1) == 0))
				{
					return false;
				}
				if (this.mode == BlockManager.BlockMode.OnlySelector)
				{
					return !this.blockManager.NodeContainsAnyOf(node, this.selector);
				}
				return !this.blockManager.NodeContainsAnyExcept(node, this.selector);
			}

			// Token: 0x060009A0 RID: 2464 RVA: 0x00034AF6 File Offset: 0x00032CF6
			public bool CanTraverse(Path path, GraphNode from, GraphNode to)
			{
				return this.CanTraverse(path, to);
			}

			// Token: 0x060009A1 RID: 2465 RVA: 0x00034B00 File Offset: 0x00032D00
			public uint GetTraversalCost(Path path, GraphNode node)
			{
				return path.GetTagPenalty((int)node.Tag) + node.Penalty;
			}

			// Token: 0x17000188 RID: 392
			// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0001797A File Offset: 0x00015B7A
			public bool filterDiagonalGridConnections
			{
				get
				{
					return true;
				}
			}

			// Token: 0x04000690 RID: 1680
			private readonly BlockManager blockManager;

			// Token: 0x04000692 RID: 1682
			private readonly List<SingleNodeBlocker> selector;
		}
	}
}

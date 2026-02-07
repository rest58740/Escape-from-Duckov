using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000112 RID: 274
	[AddComponentMenu("Pathfinding/Modifiers/Funnel Modifier")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/funnelmodifier.html")]
	[Serializable]
	public class FunnelModifier : MonoModifier
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x060008CC RID: 2252 RVA: 0x0002E5AD File Offset: 0x0002C7AD
		public override int Order
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002E74C File Offset: 0x0002C94C
		public override void Apply(Path p)
		{
			if (p.path == null || p.path.Count == 0 || p.vectorPath == null || p.vectorPath.Count == 0)
			{
				return;
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			List<Funnel.PathPart> list2 = Funnel.SplitIntoParts(p);
			if (list2.Count == 0)
			{
				return;
			}
			if (this.quality == FunnelModifier.FunnelQuality.High)
			{
				Func<GraphNode, bool> filter = new Func<GraphNode, bool>(p.CanTraverse);
				Funnel.Simplify(list2, ref p.path, filter);
			}
			for (int i = 0; i < list2.Count; i++)
			{
				Funnel.PathPart pathPart = list2[i];
				if (pathPart.type == Funnel.PartType.NodeSequence)
				{
					GridGraph gridGraph = p.path[pathPart.startIndex].Graph as GridGraph;
					if (gridGraph != null && gridGraph.neighbours != NumNeighbours.Six)
					{
						Func<GraphNode, uint> traversalCost = null;
						if (this.accountForGridPenalties)
						{
							traversalCost = new Func<GraphNode, uint>(p.GetTraversalCost);
						}
						Func<GraphNode, bool> filter2 = new Func<GraphNode, bool>(p.CanTraverse);
						List<Vector3> collection = GridStringPulling.Calculate(p.path, pathPart.startIndex, pathPart.endIndex, pathPart.startPoint, pathPart.endPoint, traversalCost, filter2, int.MaxValue);
						list.AddRange(collection);
						ListPool<Vector3>.Release(ref collection);
					}
					else
					{
						Funnel.FunnelPortals funnel = Funnel.ConstructFunnelPortals(p.path, pathPart);
						List<Vector3> collection2 = Funnel.Calculate(funnel, this.splitAtEveryPortal);
						list.AddRange(collection2);
						ListPool<Vector3>.Release(ref funnel.left);
						ListPool<Vector3>.Release(ref funnel.right);
						ListPool<Vector3>.Release(ref collection2);
					}
				}
				else
				{
					if (i == 0 || list2[i - 1].type == Funnel.PartType.OffMeshLink)
					{
						list.Add(pathPart.startPoint);
					}
					if (i == list2.Count - 1 || list2[i + 1].type == Funnel.PartType.OffMeshLink)
					{
						list.Add(pathPart.endPoint);
					}
				}
			}
			ListPool<Funnel.PathPart>.Release(ref list2);
			ListPool<Vector3>.Release(ref p.vectorPath);
			p.vectorPath = list;
		}

		// Token: 0x040005C6 RID: 1478
		public FunnelModifier.FunnelQuality quality;

		// Token: 0x040005C7 RID: 1479
		public bool splitAtEveryPortal;

		// Token: 0x040005C8 RID: 1480
		public bool accountForGridPenalties;

		// Token: 0x02000113 RID: 275
		public enum FunnelQuality
		{
			// Token: 0x040005CA RID: 1482
			Medium,
			// Token: 0x040005CB RID: 1483
			High
		}
	}
}

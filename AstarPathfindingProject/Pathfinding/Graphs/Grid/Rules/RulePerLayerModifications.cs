using System;
using Pathfinding.Jobs;
using Pathfinding.Util;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Rules
{
	// Token: 0x02000209 RID: 521
	[Preserve]
	public class RulePerLayerModifications : GridGraphRule
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x00050C0C File Offset: 0x0004EE0C
		public override void Register(GridGraphRules rules)
		{
			int[] layerToTag = new int[32];
			bool[] layerToUnwalkable = new bool[32];
			for (int i = 0; i < this.layerRules.Length; i++)
			{
				RulePerLayerModifications.PerLayerRule perLayerRule = this.layerRules[i];
				if (perLayerRule.action == RulePerLayerModifications.RuleAction.SetTag)
				{
					layerToTag[perLayerRule.layer] = (1073741824 | perLayerRule.tag);
				}
				else
				{
					layerToUnwalkable[perLayerRule.layer] = true;
				}
			}
			rules.AddMainThreadPass(GridGraphRule.Pass.BeforeConnections, delegate(GridGraphRules.Context context)
			{
				if (!context.data.heightHits.IsCreated)
				{
					Debug.LogError("RulePerLayerModifications requires height testing to be enabled on the grid graph", context.graph.active);
					return;
				}
				NativeArray<RaycastHit> heightHits = context.data.heightHits;
				NativeArray<bool> walkable = context.data.nodes.walkable;
				NativeArray<int> tags = context.data.nodes.tags;
				Slice3D slice3D = new Slice3D(context.data.nodes.bounds, context.data.heightHitsBounds);
				int3 size = slice3D.slice.size;
				for (int j = 0; j < size.y; j++)
				{
					for (int k = 0; k < size.z; k++)
					{
						int num = j * size.x * size.z + k * size.x;
						for (int l = 0; l < size.x; l++)
						{
							int index = num + l;
							int index2 = slice3D.InnerCoordinateToOuterIndex(l, j, k);
							Collider collider = heightHits[index].collider;
							if (collider != null)
							{
								int layer = collider.gameObject.layer;
								if (layerToUnwalkable[layer])
								{
									walkable[index2] = false;
								}
								int num2 = layerToTag[layer];
								if ((num2 & 1073741824) != 0)
								{
									tags[index2] = (num2 & 255);
								}
							}
						}
					}
				}
			});
		}

		// Token: 0x0400097F RID: 2431
		public RulePerLayerModifications.PerLayerRule[] layerRules = new RulePerLayerModifications.PerLayerRule[0];

		// Token: 0x04000980 RID: 2432
		private const int SetTagBit = 1073741824;

		// Token: 0x0200020A RID: 522
		public struct PerLayerRule
		{
			// Token: 0x04000981 RID: 2433
			public int layer;

			// Token: 0x04000982 RID: 2434
			public RulePerLayerModifications.RuleAction action;

			// Token: 0x04000983 RID: 2435
			public int tag;
		}

		// Token: 0x0200020B RID: 523
		public enum RuleAction
		{
			// Token: 0x04000985 RID: 2437
			SetTag,
			// Token: 0x04000986 RID: 2438
			MakeUnwalkable
		}
	}
}

using System;
using System.Collections.Generic;
using NodeCanvas.Framework;

namespace Duckov.PerkTrees
{
	// Token: 0x02000263 RID: 611
	public class PerkTreeRelationGraphOwner : GraphOwner<PerkRelationGraph>
	{
		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x0004944C File Offset: 0x0004764C
		public PerkRelationGraph RelationGraph
		{
			get
			{
				if (this._relationGraph == null)
				{
					this._relationGraph = (this.graph as PerkRelationGraph);
				}
				return this._relationGraph;
			}
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00049474 File Offset: 0x00047674
		public List<Perk> GetRequiredNodes(Perk node)
		{
			PerkRelationNode relatedNode = this.RelationGraph.GetRelatedNode(node);
			if (relatedNode == null)
			{
				return null;
			}
			List<PerkRelationNode> incomingNodes = this.RelationGraph.GetIncomingNodes(relatedNode);
			if (incomingNodes == null)
			{
				return null;
			}
			if (incomingNodes.Count < 1)
			{
				return null;
			}
			List<Perk> list = new List<Perk>();
			foreach (PerkRelationNode perkRelationNode in incomingNodes)
			{
				Perk relatedNode2 = perkRelationNode.relatedNode;
				if (!(relatedNode2 == null))
				{
					list.Add(relatedNode2);
				}
			}
			return list;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0004950C File Offset: 0x0004770C
		internal PerkRelationNode GetRelatedNode(Perk perk)
		{
			if (this.RelationGraph == null)
			{
				return null;
			}
			return this.RelationGraph.GetRelatedNode(perk);
		}

		// Token: 0x04000EA2 RID: 3746
		private PerkRelationGraph _relationGraph;
	}
}

using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;

namespace Duckov.PerkTrees
{
	// Token: 0x02000260 RID: 608
	public class PerkRelationGraph : Graph
	{
		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600133B RID: 4923 RVA: 0x00049221 File Offset: 0x00047421
		public override Type baseNodeType
		{
			get
			{
				return typeof(PerkRelationNodeBase);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600133C RID: 4924 RVA: 0x0004922D File Offset: 0x0004742D
		public override bool requiresAgent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600133D RID: 4925 RVA: 0x00049230 File Offset: 0x00047430
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600133E RID: 4926 RVA: 0x00049233 File Offset: 0x00047433
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00049236 File Offset: 0x00047436
		public override PlanarDirection flowDirection
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00049239 File Offset: 0x00047439
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06001341 RID: 4929 RVA: 0x0004923C File Offset: 0x0004743C
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00049240 File Offset: 0x00047440
		public PerkRelationNode GetRelatedNode(Perk perk)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				if (node == null)
				{
					return false;
				}
				PerkRelationNode perkRelationNode = node as PerkRelationNode;
				return perkRelationNode != null && perkRelationNode.relatedNode == perk;
			}) as PerkRelationNode;
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x00049278 File Offset: 0x00047478
		public List<PerkRelationNode> GetIncomingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.inConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.sourceNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x000492E4 File Offset: 0x000474E4
		public List<PerkRelationNode> GetOutgoingNodes(PerkRelationNode skillTreeNode)
		{
			List<PerkRelationNode> list = new List<PerkRelationNode>();
			foreach (Connection connection in skillTreeNode.outConnections)
			{
				if (connection != null)
				{
					PerkRelationNode perkRelationNode = connection.targetNode as PerkRelationNode;
					if (perkRelationNode != null)
					{
						list.Add(perkRelationNode);
					}
				}
			}
			return list;
		}
	}
}

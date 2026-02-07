using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using UnityEngine;

namespace Duckov.PerkTrees
{
	// Token: 0x02000261 RID: 609
	public class PerkRelationNode : PerkRelationNodeBase
	{
		// Token: 0x06001346 RID: 4934 RVA: 0x00049358 File Offset: 0x00047558
		internal void SetDirty()
		{
			this.dirty = true;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x00049364 File Offset: 0x00047564
		public override void OnDestroy()
		{
			if (this.relatedNode == null)
			{
				return;
			}
			IEnumerable<Node> enumerable = from e in base.graph.allNodes
			where (e as PerkRelationNode).relatedNode == this.relatedNode
			select e;
			if (enumerable.Count<Node>() <= 2)
			{
				foreach (Node node in enumerable)
				{
					PerkRelationNode perkRelationNode = node as PerkRelationNode;
					if (perkRelationNode != null)
					{
						perkRelationNode.isDuplicate = false;
						perkRelationNode.SetDirty();
					}
				}
			}
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x000493F0 File Offset: 0x000475F0
		internal void NotifyIncomingStateChanged()
		{
			this.relatedNode.NotifyParentStateChanged();
		}

		// Token: 0x04000E9D RID: 3741
		public Perk relatedNode;

		// Token: 0x04000E9E RID: 3742
		public Vector2 cachedPosition;

		// Token: 0x04000E9F RID: 3743
		private bool dirty = true;

		// Token: 0x04000EA0 RID: 3744
		internal bool isDuplicate;

		// Token: 0x04000EA1 RID: 3745
		internal bool isInvalid;
	}
}

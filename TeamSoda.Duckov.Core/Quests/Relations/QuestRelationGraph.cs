using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using UnityEngine;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000376 RID: 886
	[CreateAssetMenu(menuName = "Quests/Relations")]
	public class QuestRelationGraph : Graph
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0006E892 File Offset: 0x0006CA92
		public override Type baseNodeType
		{
			get
			{
				return typeof(QuestRelationNodeBase);
			}
		}

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0006E89E File Offset: 0x0006CA9E
		public override bool requiresAgent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x0006E8A1 File Offset: 0x0006CAA1
		public override bool requiresPrimeNode
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0006E8A4 File Offset: 0x0006CAA4
		public override bool isTree
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0006E8A7 File Offset: 0x0006CAA7
		public override PlanarDirection flowDirection
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x0006E8AA File Offset: 0x0006CAAA
		public override bool allowBlackboardOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x0006E8AD File Offset: 0x0006CAAD
		public override bool canAcceptVariableDrops
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0006E8B0 File Offset: 0x0006CAB0
		public QuestRelationNode GetNode(int questID)
		{
			return base.allNodes.Find(delegate(Node node)
			{
				QuestRelationNode questRelationNode = node as QuestRelationNode;
				return questRelationNode != null && questRelationNode.questID == questID;
			}) as QuestRelationNode;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0006E8E8 File Offset: 0x0006CAE8
		public List<int> GetRequiredIDs(int targetID)
		{
			List<int> list = new List<int>();
			QuestRelationNode node = this.GetNode(targetID);
			if (node == null)
			{
				return list;
			}
			foreach (Connection connection in node.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					int questID = questRelationNode.questID;
					list.Add(questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						int questID2 = questRelationProxyNode.questID;
						list.Add(questID2);
					}
				}
			}
			return list;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x0006E990 File Offset: 0x0006CB90
		protected override void OnGraphValidate()
		{
			this.CheckDuplicates();
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x0006E998 File Offset: 0x0006CB98
		internal void CheckDuplicates()
		{
		}

		// Token: 0x0400154F RID: 5455
		public static int selectedQuestID = -1;
	}
}

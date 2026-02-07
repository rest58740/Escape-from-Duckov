using System;
using System.Collections.Generic;
using Duckov.Utilities;
using NodeCanvas.Framework;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000378 RID: 888
	public class QuestRelationNode : QuestRelationNodeBase
	{
		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x0006E9D2 File Offset: 0x0006CBD2
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationNode._questCollection == null)
				{
					QuestRelationNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationNode._questCollection;
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x0006E9F0 File Offset: 0x0006CBF0
		private void SelectQuest()
		{
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x0006E9F4 File Offset: 0x0006CBF4
		public List<int> GetParents()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.inConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
				else
				{
					QuestRelationProxyNode questRelationProxyNode = connection.sourceNode as QuestRelationProxyNode;
					if (questRelationProxyNode != null)
					{
						list.Add(questRelationProxyNode.questID);
					}
				}
			}
			return list;
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x0006EA84 File Offset: 0x0006CC84
		public List<int> GetChildren()
		{
			List<int> list = new List<int>();
			foreach (Connection connection in base.outConnections)
			{
				QuestRelationNode questRelationNode = connection.sourceNode as QuestRelationNode;
				if (questRelationNode != null)
				{
					list.Add(questRelationNode.questID);
				}
			}
			return list;
		}

		// Token: 0x04001550 RID: 5456
		public int questID;

		// Token: 0x04001551 RID: 5457
		private static QuestCollection _questCollection;

		// Token: 0x04001552 RID: 5458
		internal bool isDuplicate;
	}
}

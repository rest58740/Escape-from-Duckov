using System;
using Duckov.Utilities;

namespace Duckov.Quests.Relations
{
	// Token: 0x02000379 RID: 889
	public class QuestRelationProxyNode : QuestRelationNodeBase
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x0006EAF8 File Offset: 0x0006CCF8
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0006EAFB File Offset: 0x0006CCFB
		private static QuestCollection QuestCollection
		{
			get
			{
				if (QuestRelationProxyNode._questCollection == null)
				{
					QuestRelationProxyNode._questCollection = GameplayDataSettings.QuestCollection;
				}
				return QuestRelationProxyNode._questCollection;
			}
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x0006EB19 File Offset: 0x0006CD19
		private void SelectQuest()
		{
		}

		// Token: 0x04001553 RID: 5459
		private static QuestCollection _questCollection;

		// Token: 0x04001554 RID: 5460
		public int questID;
	}
}

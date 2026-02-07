using System;
using Duckov.Quests;
using UnityEngine;

namespace Duckov.UI.RedDots
{
	// Token: 0x02000404 RID: 1028
	public class QuestsButtonRedDot : MonoBehaviour
	{
		// Token: 0x06002547 RID: 9543 RVA: 0x000824D7 File Offset: 0x000806D7
		private void Awake()
		{
			Quest.onQuestNeedInspectionChanged += this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000824EA File Offset: 0x000806EA
		private void OnDestroy()
		{
			Quest.onQuestNeedInspectionChanged -= this.OnQuestNeedInspectionChanged;
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000824FD File Offset: 0x000806FD
		private void OnQuestNeedInspectionChanged(Quest quest)
		{
			this.Refresh();
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x00082505 File Offset: 0x00080705
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x0008250D File Offset: 0x0008070D
		private void Refresh()
		{
			this.dot.SetActive(QuestManager.AnyQuestNeedsInspection);
		}

		// Token: 0x04001954 RID: 6484
		public GameObject dot;
	}
}

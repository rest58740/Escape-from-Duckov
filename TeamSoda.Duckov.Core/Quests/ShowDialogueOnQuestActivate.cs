using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000355 RID: 853
	public class ShowDialogueOnQuestActivate : MonoBehaviour
	{
		// Token: 0x06001D83 RID: 7555 RVA: 0x0006AE48 File Offset: 0x00069048
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x0006AE7B File Offset: 0x0006907B
		private void OnQuestActivated(Quest quest)
		{
			this.ShowDIalogue().Forget();
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x0006AE88 File Offset: 0x00069088
		private UniTask ShowDIalogue()
		{
			ShowDialogueOnQuestActivate.<ShowDIalogue>d__6 <ShowDIalogue>d__;
			<ShowDIalogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDIalogue>d__.<>4__this = this;
			<ShowDIalogue>d__.<>1__state = -1;
			<ShowDIalogue>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDIalogue>d__6>(ref <ShowDIalogue>d__);
			return <ShowDIalogue>d__.<>t__builder.Task;
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x0006AECC File Offset: 0x000690CC
		private UniTask ShowDialogueEntry(ShowDialogueOnQuestActivate.DialogueEntry cur)
		{
			ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7 <ShowDialogueEntry>d__;
			<ShowDialogueEntry>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<ShowDialogueEntry>d__.<>4__this = this;
			<ShowDialogueEntry>d__.cur = cur;
			<ShowDialogueEntry>d__.<>1__state = -1;
			<ShowDialogueEntry>d__.<>t__builder.Start<ShowDialogueOnQuestActivate.<ShowDialogueEntry>d__7>(ref <ShowDialogueEntry>d__);
			return <ShowDialogueEntry>d__.<>t__builder.Task;
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x0006AF18 File Offset: 0x00069118
		private Transform GetQuestGiverTransform(Quest quest)
		{
			QuestGiverID id = quest.QuestGiverID;
			QuestGiver questGiver = Object.FindObjectsByType<QuestGiver>(0).FirstOrDefault((QuestGiver e) => e != null && e.ID == id);
			if (questGiver == null)
			{
				return null;
			}
			return questGiver.transform;
		}

		// Token: 0x0400148F RID: 5263
		[SerializeField]
		private Quest quest;

		// Token: 0x04001490 RID: 5264
		[SerializeField]
		private List<ShowDialogueOnQuestActivate.DialogueEntry> dialogueEntries;

		// Token: 0x04001491 RID: 5265
		private Transform cachedQuestGiverTransform;

		// Token: 0x0200061F RID: 1567
		[Serializable]
		public class DialogueEntry
		{
			// Token: 0x0400221E RID: 8734
			[TextArea]
			public string content;
		}
	}
}

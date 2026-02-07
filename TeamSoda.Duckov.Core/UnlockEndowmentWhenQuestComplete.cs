using System;
using Duckov.Endowment;
using Duckov.Quests;
using UnityEngine;

// Token: 0x02000129 RID: 297
public class UnlockEndowmentWhenQuestComplete : MonoBehaviour
{
	// Token: 0x060009E2 RID: 2530 RVA: 0x0002B7AC File Offset: 0x000299AC
	private void Awake()
	{
		if (this.quest == null)
		{
			this.quest = base.GetComponent<Quest>();
		}
		if (this.quest != null)
		{
			this.quest.onCompleted += this.OnQuestCompleted;
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0002B7F8 File Offset: 0x000299F8
	private void Start()
	{
		if (this.quest.Complete && !EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0002B820 File Offset: 0x00029A20
	private void OnDestroy()
	{
		if (this.quest != null)
		{
			this.quest.onCompleted -= this.OnQuestCompleted;
		}
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x0002B847 File Offset: 0x00029A47
	private void OnQuestCompleted(Quest quest)
	{
		if (!EndowmentManager.GetEndowmentUnlocked(this.endowmentToUnlock))
		{
			EndowmentManager.UnlockEndowment(this.endowmentToUnlock);
		}
	}

	// Token: 0x040008CD RID: 2253
	[SerializeField]
	private Quest quest;

	// Token: 0x040008CE RID: 2254
	[SerializeField]
	private EndowmentIndex endowmentToUnlock;
}

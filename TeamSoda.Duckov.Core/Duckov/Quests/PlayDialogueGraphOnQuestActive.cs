using System;
using Duckov.UI;
using NodeCanvas.DialogueTrees;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000354 RID: 852
	public class PlayDialogueGraphOnQuestActive : MonoBehaviour
	{
		// Token: 0x06001D7E RID: 7550 RVA: 0x0006AD22 File Offset: 0x00068F22
		private void Awake()
		{
			if (this.quest == null)
			{
				this.quest = base.GetComponent<Quest>();
			}
			this.quest.onActivated += this.OnQuestActivated;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x0006AD55 File Offset: 0x00068F55
		private void OnQuestActivated(Quest quest)
		{
			if (View.ActiveView != null)
			{
				View.ActiveView.Close();
			}
			this.SetupActors();
			this.PlayDialogue();
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x0006AD7A File Offset: 0x00068F7A
		private void PlayDialogue()
		{
			this.dialogueTreeController.StartDialogue();
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x0006AD88 File Offset: 0x00068F88
		private void SetupActors()
		{
			if (this.dialogueTreeController.behaviour == null)
			{
				Debug.LogError("Dialoguetree没有配置", this.dialogueTreeController);
				return;
			}
			foreach (DialogueTree.ActorParameter actorParameter in this.dialogueTreeController.behaviour.actorParameters)
			{
				string name = actorParameter.name;
				if (!string.IsNullOrEmpty(name))
				{
					DuckovDialogueActor duckovDialogueActor = DuckovDialogueActor.Get(name);
					if (duckovDialogueActor == null)
					{
						Debug.LogError("未找到actor ID:" + name);
					}
					else
					{
						this.dialogueTreeController.SetActorReference(name, duckovDialogueActor);
					}
				}
			}
		}

		// Token: 0x0400148D RID: 5261
		[SerializeField]
		private Quest quest;

		// Token: 0x0400148E RID: 5262
		[SerializeField]
		private DialogueTreeController dialogueTreeController;
	}
}

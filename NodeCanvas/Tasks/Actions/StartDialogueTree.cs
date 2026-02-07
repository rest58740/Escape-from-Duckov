using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008D RID: 141
	[Category("Dialogue")]
	[Description("Starts the Dialogue Tree assigned on a Dialogue Tree Controller object with specified agent used for 'Instigator'.")]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	public class StartDialogueTree : ActionTask<IDialogueActor>
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000271 RID: 625 RVA: 0x00009C17 File Offset: 0x00007E17
		protected override string info
		{
			get
			{
				return string.Format("Start Dialogue {0}", this.dialogueTreeController);
			}
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009C2C File Offset: 0x00007E2C
		protected override void OnExecute()
		{
			this.instance = (this.isPrefab ? Object.Instantiate<DialogueTreeController>(this.dialogueTreeController.value) : this.dialogueTreeController.value);
			if (this.waitActionFinish)
			{
				this.instance.StartDialogue(base.agent, delegate(bool success)
				{
					if (this.isPrefab)
					{
						Object.Destroy(this.instance.gameObject);
					}
					base.EndAction(success);
				});
				return;
			}
			this.instance.StartDialogue(base.agent, delegate(bool success)
			{
				if (this.isPrefab)
				{
					Object.Destroy(this.instance.gameObject);
				}
			});
			base.EndAction();
		}

		// Token: 0x04000195 RID: 405
		[RequiredField]
		public BBParameter<DialogueTreeController> dialogueTreeController;

		// Token: 0x04000196 RID: 406
		public bool waitActionFinish = true;

		// Token: 0x04000197 RID: 407
		public bool isPrefab;

		// Token: 0x04000198 RID: 408
		private DialogueTreeController instance;
	}
}

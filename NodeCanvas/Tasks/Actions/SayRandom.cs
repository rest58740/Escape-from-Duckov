using System;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008C RID: 140
	[Category("Dialogue")]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	[Description("A random statement will be chosen each time for the actor to say")]
	public class SayRandom : ActionTask<IDialogueActor>
	{
		// Token: 0x0600026F RID: 623 RVA: 0x00009BB0 File Offset: 0x00007DB0
		protected override void OnExecute()
		{
			int num = Random.Range(0, this.statements.Count);
			IStatement statement = this.statements[num].BlackboardReplace(base.blackboard);
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.agent, statement, new Action(base.EndAction)));
		}

		// Token: 0x04000194 RID: 404
		public List<Statement> statements = new List<Statement>();
	}
}

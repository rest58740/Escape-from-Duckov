using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace NodeCanvas.Tasks.Actions
{
	// Token: 0x0200008B RID: 139
	[Category("Dialogue")]
	[Description("You can use a variable inline with the text by using brackets likeso: [myVarName] or [Global/myVarName].\nThe bracket will be replaced with the variable value ToString")]
	[Icon("Dialogue", false, "")]
	public class Say : ActionTask<IDialogueActor>
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009B3C File Offset: 0x00007D3C
		protected override string info
		{
			get
			{
				return string.Format("<i>' {0} '</i>", this.statement.text.CapLength(30));
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009B5C File Offset: 0x00007D5C
		protected override void OnExecute()
		{
			IStatement statement = this.statement.BlackboardReplace(base.blackboard);
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.agent, statement, new Action(base.EndAction)));
		}

		// Token: 0x04000193 RID: 403
		public Statement statement = new Statement("This is a dialogue text...");
	}
}

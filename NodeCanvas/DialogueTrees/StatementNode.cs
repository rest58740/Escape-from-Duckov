using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000105 RID: 261
	[Name("Say", 0)]
	[Description("Make the selected Dialogue Actor talk. You can make the text more dynamic by using variable names in square brackets\ne.g. [myVarName] or [Global/myVarName]")]
	public class StatementNode : DTNode
	{
		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x000118F7 File Offset: 0x0000FAF7
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000118FC File Offset: 0x0000FAFC
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			IStatement statement = this.statement.BlackboardReplace(bb);
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, new Action(this.OnStatementFinish)));
			return Status.Running;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00011934 File Offset: 0x0000FB34
		private void OnStatementFinish()
		{
			base.status = Status.Success;
			base.DLGTree.Continue(0);
		}

		// Token: 0x040002EC RID: 748
		[SerializeField]
		public Statement statement = new Statement("This is a dialogue text");
	}
}

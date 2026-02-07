using System;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F3 RID: 243
	public class SubtitlesRequestInfo
	{
		// Token: 0x060004C2 RID: 1218 RVA: 0x0001079D File Offset: 0x0000E99D
		public SubtitlesRequestInfo(IDialogueActor actor, IStatement statement, Action callback)
		{
			this.actor = actor;
			this.statement = statement;
			this.Continue = callback;
		}

		// Token: 0x040002C5 RID: 709
		public IDialogueActor actor;

		// Token: 0x040002C6 RID: 710
		public IStatement statement;

		// Token: 0x040002C7 RID: 711
		public Action Continue;
	}
}

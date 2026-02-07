using System;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000F4 RID: 244
	public class MultipleChoiceRequestInfo
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x000107BA File Offset: 0x0000E9BA
		public MultipleChoiceRequestInfo(IDialogueActor actor, Dictionary<IStatement, int> options, float availableTime, bool showLastStatement, Action<int> callback)
		{
			this.actor = actor;
			this.options = options;
			this.availableTime = availableTime;
			this.showLastStatement = showLastStatement;
			this.SelectOption = callback;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000107E7 File Offset: 0x0000E9E7
		public MultipleChoiceRequestInfo(IDialogueActor actor, Dictionary<IStatement, int> options, float availableTime, Action<int> callback)
		{
			this.actor = actor;
			this.options = options;
			this.availableTime = availableTime;
			this.SelectOption = callback;
		}

		// Token: 0x040002C8 RID: 712
		public IDialogueActor actor;

		// Token: 0x040002C9 RID: 713
		public Dictionary<IStatement, int> options;

		// Token: 0x040002CA RID: 714
		public float availableTime;

		// Token: 0x040002CB RID: 715
		public bool showLastStatement;

		// Token: 0x040002CC RID: 716
		public Action<int> SelectOption;
	}
}

using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x02000227 RID: 551
	public class LocalizedStatementNode : DTNode
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00041C21 File Offset: 0x0003FE21
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x00041C24 File Offset: 0x0003FE24
		private string Key
		{
			get
			{
				if (this.useSequence.value)
				{
					return string.Format("{0}_{1}", this.key.value, this.sequenceIndex.value);
				}
				return this.key.value;
			}
		}

		// Token: 0x060010CA RID: 4298 RVA: 0x00041C64 File Offset: 0x0003FE64
		private LocalizedStatement CreateStatement()
		{
			return new LocalizedStatement(this.Key);
		}

		// Token: 0x060010CB RID: 4299 RVA: 0x00041C74 File Offset: 0x0003FE74
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			LocalizedStatement localizedStatement = this.CreateStatement();
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, localizedStatement, new Action(this.OnStatementFinish)));
			return 2;
		}

		// Token: 0x060010CC RID: 4300 RVA: 0x00041CA6 File Offset: 0x0003FEA6
		private void OnStatementFinish()
		{
			base.status = 1;
			base.DLGTree.Continue(0);
		}

		// Token: 0x04000D75 RID: 3445
		public BBParameter<string> key;

		// Token: 0x04000D76 RID: 3446
		public BBParameter<bool> useSequence;

		// Token: 0x04000D77 RID: 3447
		public BBParameter<int> sequenceIndex;
	}
}

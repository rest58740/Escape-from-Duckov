using System;
using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Dialogues
{
	// Token: 0x02000228 RID: 552
	public class LocalizedStatementSequence : DTNode
	{
		// Token: 0x17000305 RID: 773
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x00041CC3 File Offset: 0x0003FEC3
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x00041CC6 File Offset: 0x0003FEC6
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			this.Begin();
			return 2;
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00041CCF File Offset: 0x0003FECF
		private void Begin()
		{
			this.index = this.beginIndex.value - 1;
			this.Next();
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00041CEC File Offset: 0x0003FEEC
		private void Next()
		{
			this.index++;
			if (this.index > this.endIndex.value)
			{
				base.status = 1;
				base.DLGTree.Continue(0);
				return;
			}
			LocalizedStatement localizedStatement = new LocalizedStatement(StringExtensions.Format(this.format.value, new
			{
				keyPrefix = this.keyPrefix.value,
				index = this.index
			}));
			DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, localizedStatement, new Action(this.OnStatementFinish)));
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x00041D77 File Offset: 0x0003FF77
		private void OnStatementFinish()
		{
			this.Next();
		}

		// Token: 0x04000D78 RID: 3448
		public BBParameter<string> keyPrefix;

		// Token: 0x04000D79 RID: 3449
		public BBParameter<int> beginIndex;

		// Token: 0x04000D7A RID: 3450
		public BBParameter<int> endIndex;

		// Token: 0x04000D7B RID: 3451
		public BBParameter<string> format = new BBParameter<string>("{keyPrefix}_{index}");

		// Token: 0x04000D7C RID: 3452
		private int index;
	}
}

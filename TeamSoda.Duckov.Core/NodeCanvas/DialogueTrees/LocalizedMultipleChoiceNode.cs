using System;
using System.Collections.Generic;
using Dialogues;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x0200041B RID: 1051
	[Icon("List", false, "")]
	[Name("Multiple Choice Localized", 0)]
	[Category("Branch")]
	[Description("Prompt a Dialogue Multiple Choice. A choice will be available if the choice condition(s) are true or there is no choice conditions. The Actor selected is used for the condition checks and will also Say the selection if the option is checked.")]
	[Color("b3ff7f")]
	public class LocalizedMultipleChoiceNode : DTNode
	{
		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002652 RID: 9810 RVA: 0x00084D56 File Offset: 0x00082F56
		public override int maxOutConnections
		{
			get
			{
				return this.availableChoices.Count;
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06002653 RID: 9811 RVA: 0x00084D63 File Offset: 0x00082F63
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x00084D68 File Offset: 0x00082F68
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (base.outConnections.Count == 0)
			{
				return base.Error("There are no connections to the Multiple Choice Node!");
			}
			Dictionary<IStatement, int> dictionary = new Dictionary<IStatement, int>();
			for (int i = 0; i < this.availableChoices.Count; i++)
			{
				ConditionTask condition = this.availableChoices[i].condition;
				if (condition == null || condition.CheckOnce(base.finalActor.transform, bb))
				{
					LocalizedStatement statement = this.availableChoices[i].statement;
					dictionary[statement] = i;
				}
			}
			if (dictionary.Count == 0)
			{
				base.DLGTree.Stop(false);
				return 0;
			}
			DialogueTree.RequestMultipleChoices(new MultipleChoiceRequestInfo(base.finalActor, dictionary, this.availableTime, new Action<int>(this.OnOptionSelected))
			{
				showLastStatement = true
			});
			return 2;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x00084E30 File Offset: 0x00083030
		private void OnOptionSelected(int index)
		{
			base.status = 1;
			Action action = delegate()
			{
				this.DLGTree.Continue(index);
			};
			if (this.saySelection)
			{
				LocalizedStatement statement = this.availableChoices[index].statement;
				DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, action));
				return;
			}
			action();
		}

		// Token: 0x04001A21 RID: 6689
		[SliderField(0f, 10f)]
		public float availableTime;

		// Token: 0x04001A22 RID: 6690
		public bool saySelection;

		// Token: 0x04001A23 RID: 6691
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<LocalizedMultipleChoiceNode.Choice> availableChoices = new List<LocalizedMultipleChoiceNode.Choice>();

		// Token: 0x02000698 RID: 1688
		[Serializable]
		public class Choice
		{
			// Token: 0x06002BF0 RID: 11248 RVA: 0x000A6D14 File Offset: 0x000A4F14
			public Choice()
			{
			}

			// Token: 0x06002BF1 RID: 11249 RVA: 0x000A6D23 File Offset: 0x000A4F23
			public Choice(LocalizedStatement statement)
			{
				this.statement = statement;
			}

			// Token: 0x04002433 RID: 9267
			public bool isUnfolded = true;

			// Token: 0x04002434 RID: 9268
			public LocalizedStatement statement;

			// Token: 0x04002435 RID: 9269
			public ConditionTask condition;
		}
	}
}

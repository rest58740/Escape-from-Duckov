using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000102 RID: 258
	[ParadoxNotion.Design.Icon("List", false, "")]
	[Name("Multiple Choice", 0)]
	[Category("Branch")]
	[Description("Prompt a Dialogue Multiple Choice. A choice will be available if the choice condition(s) are true or there is no choice conditions. The Actor selected is used for the condition checks and will also Say the selection if the option is checked.")]
	[Color("b3ff7f")]
	public class MultipleChoiceNode : DTNode
	{
		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00011520 File Offset: 0x0000F720
		public override int maxOutConnections
		{
			get
			{
				return this.availableChoices.Count;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001152D File Offset: 0x0000F72D
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00011530 File Offset: 0x0000F730
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
					IStatement statement = this.availableChoices[i].statement.BlackboardReplace(bb);
					dictionary[statement] = i;
				}
			}
			if (dictionary.Count == 0)
			{
				base.DLGTree.Stop(false);
				return Status.Failure;
			}
			DialogueTree.RequestMultipleChoices(new MultipleChoiceRequestInfo(base.finalActor, dictionary, this.availableTime, new Action<int>(this.OnOptionSelected))
			{
				showLastStatement = true
			});
			return Status.Running;
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000115FC File Offset: 0x0000F7FC
		private void OnOptionSelected(int index)
		{
			base.status = Status.Success;
			Action action = delegate()
			{
				this.DLGTree.Continue(index);
			};
			if (this.saySelection)
			{
				IStatement statement = this.availableChoices[index].statement.BlackboardReplace(base.graphBlackboard);
				DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(base.finalActor, statement, action));
				return;
			}
			action.Invoke();
		}

		// Token: 0x040002E6 RID: 742
		[SliderField(0f, 10f)]
		public float availableTime;

		// Token: 0x040002E7 RID: 743
		public bool saySelection;

		// Token: 0x040002E8 RID: 744
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<MultipleChoiceNode.Choice> availableChoices = new List<MultipleChoiceNode.Choice>();

		// Token: 0x0200015B RID: 347
		[Serializable]
		public class Choice
		{
			// Token: 0x060006BF RID: 1727 RVA: 0x000149ED File Offset: 0x00012BED
			public Choice()
			{
			}

			// Token: 0x060006C0 RID: 1728 RVA: 0x000149FC File Offset: 0x00012BFC
			public Choice(Statement statement)
			{
				this.statement = statement;
			}

			// Token: 0x040003E1 RID: 993
			public bool isUnfolded = true;

			// Token: 0x040003E2 RID: 994
			public Statement statement;

			// Token: 0x040003E3 RID: 995
			public ConditionTask condition;
		}
	}
}

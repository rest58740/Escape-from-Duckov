using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000129 RID: 297
	[Name("Condition", 0)]
	[Description("Checks a condition and returns Success or Failure.")]
	[ParadoxNotion.Design.Icon("Condition", false, "")]
	public class ConditionNode : BTNode, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00013D2A File Offset: 0x00011F2A
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x00013D32 File Offset: 0x00011F32
		public Task task
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = (ConditionTask)value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00013D40 File Offset: 0x00011F40
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00013D48 File Offset: 0x00011F48
		public ConditionTask condition
		{
			get
			{
				return this._condition;
			}
			set
			{
				this._condition = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x00013D51 File Offset: 0x00011F51
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00013D5E File Offset: 0x00011F5E
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.condition == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				this.condition.Enable(agent, blackboard);
			}
			if (!this.condition.Check(agent, blackboard))
			{
				return Status.Failure;
			}
			return Status.Success;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00013D92 File Offset: 0x00011F92
		protected override void OnReset()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
		}

		// Token: 0x04000353 RID: 851
		[SerializeField]
		private ConditionTask _condition;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000127 RID: 295
	[Category("Decorators")]
	[Description("Returns Running until the assigned condition becomes true, after which the decorated child is executed.")]
	[ParadoxNotion.Design.Icon("Halt", false, "")]
	public class WaitUntil : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00013B76 File Offset: 0x00011D76
		// (set) Token: 0x06000630 RID: 1584 RVA: 0x00013B7E File Offset: 0x00011D7E
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00013B8C File Offset: 0x00011D8C
		// (set) Token: 0x06000632 RID: 1586 RVA: 0x00013B94 File Offset: 0x00011D94
		private ConditionTask condition
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

		// Token: 0x06000633 RID: 1587 RVA: 0x00013BA0 File Offset: 0x00011DA0
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
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
					return Status.Running;
				}
				return Status.Success;
			}
			else
			{
				if (this.condition == null)
				{
					return base.decoratedConnection.Execute(agent, blackboard);
				}
				if (base.status == Status.Resting)
				{
					this.condition.Enable(agent, blackboard);
				}
				if (this.accessed)
				{
					return base.decoratedConnection.Execute(agent, blackboard);
				}
				if (this.condition.Check(agent, blackboard))
				{
					this.accessed = true;
				}
				if (!this.accessed)
				{
					return Status.Running;
				}
				return base.decoratedConnection.Execute(agent, blackboard);
			}
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x00013C57 File Offset: 0x00011E57
		protected override void OnReset()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
			this.accessed = false;
		}

		// Token: 0x04000350 RID: 848
		[SerializeField]
		private ConditionTask _condition;

		// Token: 0x04000351 RID: 849
		private bool accessed;
	}
}

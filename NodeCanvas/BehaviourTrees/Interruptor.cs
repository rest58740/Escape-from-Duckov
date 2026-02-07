using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011D RID: 285
	[Name("Interrupt", 0)]
	[Category("Decorators")]
	[Description("Executes and returns the child status. If the condition is or becomes true, the child is interrupted and returns Failure.")]
	[ParadoxNotion.Design.Icon("Interruptor", false, "")]
	public class Interruptor : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x000135A4 File Offset: 0x000117A4
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x000135AC File Offset: 0x000117AC
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x000135B5 File Offset: 0x000117B5
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x000135BD File Offset: 0x000117BD
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

		// Token: 0x06000611 RID: 1553 RVA: 0x000135CC File Offset: 0x000117CC
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			if (this.condition == null)
			{
				return base.decoratedConnection.Execute(agent, blackboard);
			}
			if (base.status == Status.Resting)
			{
				this.condition.Enable(agent, blackboard);
			}
			if (!this.condition.Check(agent, blackboard))
			{
				return base.decoratedConnection.Execute(agent, blackboard);
			}
			if (base.decoratedConnection.status == Status.Running)
			{
				base.decoratedConnection.Reset(true);
			}
			return Status.Failure;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00013647 File Offset: 0x00011847
		protected override void OnReset()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
		}

		// Token: 0x0400033B RID: 827
		[SerializeField]
		private ConditionTask _condition;
	}
}

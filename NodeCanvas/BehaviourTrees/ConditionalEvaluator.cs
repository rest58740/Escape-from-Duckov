using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011A RID: 282
	[Name("Conditional", 0)]
	[Category("Decorators")]
	[Description("Executes and returns the child status only if the condition is true. Returns Failure if the condition is false.")]
	[ParadoxNotion.Design.Icon("Accessor", false, "")]
	public class ConditionalEvaluator : BTDecorator, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00013176 File Offset: 0x00011376
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0001317E File Offset: 0x0001137E
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0001318C File Offset: 0x0001138C
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x00013194 File Offset: 0x00011394
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

		// Token: 0x060005FE RID: 1534 RVA: 0x000131A0 File Offset: 0x000113A0
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
			if (this.isDynamic)
			{
				if (this.condition.Check(agent, blackboard))
				{
					return base.decoratedConnection.Execute(agent, blackboard);
				}
				base.decoratedConnection.Reset(true);
				return (Status)this.conditionFailReturn;
			}
			else
			{
				if (base.status != Status.Running)
				{
					this.accessed = this.condition.Check(agent, blackboard);
				}
				if (!this.accessed)
				{
					return (Status)this.conditionFailReturn;
				}
				return base.decoratedConnection.Execute(agent, blackboard);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00013253 File Offset: 0x00011453
		protected override void OnReset()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
			this.accessed = false;
		}

		// Token: 0x0400032C RID: 812
		[Name("Dynamic", 0)]
		[Tooltip("If enabled, the condition is re-evaluated per frame and the child is aborted if the condition becomes false.")]
		public bool isDynamic;

		// Token: 0x0400032D RID: 813
		[Tooltip("The status that will be returned if the assigned condition is or becomes false.")]
		public CompactStatus conditionFailReturn;

		// Token: 0x0400032E RID: 814
		[SerializeField]
		private ConditionTask _condition;

		// Token: 0x0400032F RID: 815
		private bool accessed;
	}
}

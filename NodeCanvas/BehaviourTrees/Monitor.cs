using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000121 RID: 289
	[Category("Decorators")]
	[ParadoxNotion.Design.Icon("Eye", false, "")]
	[Description("Monitors the decorated child for a returned Status and executes an Action when that is the case.\nThe final Status returned to the parent can either be the original decorated child Status, or the new decorator Action Status.")]
	public class Monitor : BTDecorator, ITaskAssignable<ActionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x0001383C File Offset: 0x00011A3C
		// (set) Token: 0x0600061E RID: 1566 RVA: 0x00013844 File Offset: 0x00011A44
		public ActionTask action
		{
			get
			{
				return this._action;
			}
			set
			{
				this._action = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001384D File Offset: 0x00011A4D
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x00013855 File Offset: 0x00011A55
		public Task task
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = (ActionTask)value;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00013864 File Offset: 0x00011A64
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			Status status = base.decoratedConnection.Execute(agent, blackboard);
			if (this.action == null)
			{
				return status;
			}
			if (base.status != status && (false | (status == Status.Success && this.monitorMode == Monitor.MonitorMode.Success) | (status == Status.Failure && this.monitorMode == Monitor.MonitorMode.Failure) | (this.monitorMode == Monitor.MonitorMode.AnyStatus && status != Status.Running)))
			{
				this.decoratorActionStatus = this.action.Execute(agent, blackboard);
				if (this.decoratorActionStatus == Status.Running)
				{
					return Status.Running;
				}
			}
			if (this.returnMode != Monitor.ReturnStatusMode.NewDecoratorActionStatus || this.decoratorActionStatus == Status.Resting)
			{
				return status;
			}
			return this.decoratorActionStatus;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00013910 File Offset: 0x00011B10
		protected override void OnReset()
		{
			if (this.action != null)
			{
				this.action.EndAction(default(bool?));
				this.decoratorActionStatus = Status.Resting;
			}
		}

		// Token: 0x04000343 RID: 835
		[Name("Monitor", 0)]
		[Tooltip("The Status to monitor for.")]
		public Monitor.MonitorMode monitorMode;

		// Token: 0x04000344 RID: 836
		[Name("Return", 0)]
		[Tooltip("The Status to return after (and if) the Action is executed.")]
		public Monitor.ReturnStatusMode returnMode;

		// Token: 0x04000345 RID: 837
		private Status decoratorActionStatus;

		// Token: 0x04000346 RID: 838
		[SerializeField]
		private ActionTask _action;

		// Token: 0x02000171 RID: 369
		public enum MonitorMode
		{
			// Token: 0x0400042C RID: 1068
			Failure,
			// Token: 0x0400042D RID: 1069
			Success,
			// Token: 0x0400042E RID: 1070
			AnyStatus = 10
		}

		// Token: 0x02000172 RID: 370
		public enum ReturnStatusMode
		{
			// Token: 0x04000430 RID: 1072
			OriginalDecoratedChildStatus,
			// Token: 0x04000431 RID: 1073
			NewDecoratorActionStatus
		}
	}
}

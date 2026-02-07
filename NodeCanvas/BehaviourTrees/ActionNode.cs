using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000128 RID: 296
	[Name("Action", 0)]
	[Description("Executes an action and returns Success or Failure when the action is finished.\nReturns Running until the action is finished.")]
	[ParadoxNotion.Design.Icon("Action", false, "")]
	public class ActionNode : BTNode, ITaskAssignable<ActionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x00013C7B File Offset: 0x00011E7B
		// (set) Token: 0x06000637 RID: 1591 RVA: 0x00013C83 File Offset: 0x00011E83
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x00013C91 File Offset: 0x00011E91
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00013C99 File Offset: 0x00011E99
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

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x00013CA2 File Offset: 0x00011EA2
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00013CAF File Offset: 0x00011EAF
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.action == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting || base.status == Status.Running)
			{
				return this.action.Execute(agent, blackboard);
			}
			return base.status;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00013CE4 File Offset: 0x00011EE4
		protected override void OnReset()
		{
			if (this.action != null)
			{
				this.action.EndAction(default(bool?));
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00013D0D File Offset: 0x00011F0D
		public override void OnGraphPaused()
		{
			if (this.action != null)
			{
				this.action.Pause();
			}
		}

		// Token: 0x04000352 RID: 850
		[SerializeField]
		private ActionTask _action;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E3 RID: 227
	[Name("Action State", 100)]
	[Description("Execute a number of Action Tasks OnEnter. All actions will be stoped OnExit. This state is Finished when all Actions are finished as well")]
	public class ActionState : FSMState, ITaskAssignable, IGraphElement
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000F736 File Offset: 0x0000D936
		// (set) Token: 0x06000434 RID: 1076 RVA: 0x0000F73E File Offset: 0x0000D93E
		public Task task
		{
			get
			{
				return this.actionList;
			}
			set
			{
				this.actionList = (ActionList)value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0000F74C File Offset: 0x0000D94C
		// (set) Token: 0x06000436 RID: 1078 RVA: 0x0000F754 File Offset: 0x0000D954
		public ActionList actionList
		{
			get
			{
				return this._actionList;
			}
			set
			{
				this._actionList = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0000F75D File Offset: 0x0000D95D
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0000F765 File Offset: 0x0000D965
		public bool repeatStateActions
		{
			get
			{
				return this._repeatStateActions;
			}
			set
			{
				this._repeatStateActions = value;
			}
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x0000F76E File Offset: 0x0000D96E
		public override void OnValidate(Graph assignedGraph)
		{
			if (this.actionList == null)
			{
				this.actionList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this.actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000F79F File Offset: 0x0000D99F
		protected override void OnEnter()
		{
			this.OnUpdate();
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
		protected override void OnUpdate()
		{
			Status status = this.actionList.Execute(base.graphAgent, base.graphBlackboard);
			if (!this.repeatStateActions && status != Status.Running)
			{
				base.Finish(status);
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		protected override void OnExit()
		{
			this.actionList.EndAction(default(bool?));
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000F801 File Offset: 0x0000DA01
		protected override void OnPause()
		{
			this.actionList.Pause();
		}

		// Token: 0x0400029C RID: 668
		[SerializeField]
		private ActionList _actionList;

		// Token: 0x0400029D RID: 669
		[SerializeField]
		private bool _repeatStateActions;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000EE RID: 238
	[Name("Action State (Super)", 99)]
	[Description("The Super Action State provides finer control on when to execute actions. This state is never Finished by it's own if there is any Actions in the OnUpdate list and thus OnFinish transitions will never be called in that case. OnExit Actions are only called for 1 frame when the state exits.")]
	public class SuperActionState : FSMState
	{
		// Token: 0x06000491 RID: 1169 RVA: 0x000101AC File Offset: 0x0000E3AC
		public override void OnValidate(Graph assignedGraph)
		{
			if (this._onEnterList == null)
			{
				this._onEnterList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this._onEnterList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
			if (this._onUpdateList == null)
			{
				this._onUpdateList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this._onUpdateList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
			if (this._onExitList == null)
			{
				this._onExitList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this._onExitList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00010246 File Offset: 0x0000E446
		protected override void OnEnter()
		{
			this.enterListFinished = false;
			this.OnUpdate();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00010258 File Offset: 0x0000E458
		protected override void OnUpdate()
		{
			if (!this.enterListFinished)
			{
				Status status = this._onEnterList.Execute(base.graphAgent, base.graphBlackboard);
				if (status != Status.Running)
				{
					this.enterListFinished = true;
					if (this._onUpdateList.actions.Count == 0)
					{
						base.Finish(status);
					}
				}
			}
			this._onUpdateList.Execute(base.graphAgent, base.graphBlackboard);
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x000102C4 File Offset: 0x0000E4C4
		protected override void OnExit()
		{
			this._onEnterList.EndAction(default(bool?));
			this._onUpdateList.EndAction(default(bool?));
			this._onExitList.Execute(base.graphAgent, base.graphBlackboard);
			this._onExitList.EndAction(default(bool?));
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00010325 File Offset: 0x0000E525
		protected override void OnPause()
		{
			this._onEnterList.Pause();
			this._onUpdateList.Pause();
		}

		// Token: 0x040002B1 RID: 689
		[SerializeField]
		private ActionList _onEnterList;

		// Token: 0x040002B2 RID: 690
		[SerializeField]
		private ActionList _onUpdateList;

		// Token: 0x040002B3 RID: 691
		[SerializeField]
		private ActionList _onExitList;

		// Token: 0x040002B4 RID: 692
		private bool enterListFinished;
	}
}

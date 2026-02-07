using System;
using System.Collections;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FE RID: 254
	[Name("Task Action", 0)]
	[Description("Execute an Action Task for the Dialogue Actor selected.")]
	public class ActionNode : DTNode, ITaskAssignable<ActionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x000112E9 File Offset: 0x0000F4E9
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x000112F1 File Offset: 0x0000F4F1
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

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x000112FA File Offset: 0x0000F4FA
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00011302 File Offset: 0x0000F502
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

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00011310 File Offset: 0x0000F510
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x00011313 File Offset: 0x0000F513
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (this.action == null)
			{
				return base.Error("Action is null on Dialogue Action Node");
			}
			base.status = Status.Running;
			base.StartCoroutine(this.UpdateAction(base.finalActor.transform));
			return base.status;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001134E File Offset: 0x0000F54E
		private IEnumerator UpdateAction(Component actionAgent)
		{
			while (base.status == Status.Running)
			{
				Status status = this.action.Execute(actionAgent, base.graphBlackboard);
				if (status != Status.Running)
				{
					this.OnActionEnd(status == Status.Success);
					yield break;
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00011364 File Offset: 0x0000F564
		private void OnActionEnd(bool success)
		{
			if (success)
			{
				base.status = Status.Success;
				base.DLGTree.Continue(0);
				return;
			}
			base.status = Status.Failure;
			base.DLGTree.Stop(false);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00011390 File Offset: 0x0000F590
		protected override void OnReset()
		{
			if (this.action != null)
			{
				this.action.EndAction(default(bool?));
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000113B9 File Offset: 0x0000F5B9
		public override void OnGraphPaused()
		{
			if (this.action != null)
			{
				this.action.Pause();
			}
		}

		// Token: 0x040002E2 RID: 738
		[SerializeField]
		private ActionTask _action;
	}
}

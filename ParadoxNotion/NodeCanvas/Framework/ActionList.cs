using System;
using System.Collections.Generic;
using ParadoxNotion.Design;

namespace NodeCanvas.Framework
{
	// Token: 0x0200001F RID: 31
	[DoNotList]
	public class ActionList : ActionTask
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00005C38 File Offset: 0x00003E38
		protected override string info
		{
			get
			{
				if (this.actions.Count == 0)
				{
					return "No Actions";
				}
				string text = (this.actions.Count > 1) ? string.Format("<b>({0})</b>\n", (this.executionMode == ActionList.ActionsExecutionMode.ActionsRunInSequence) ? "In Sequence" : "In Parallel") : string.Empty;
				for (int i = 0; i < this.actions.Count; i++)
				{
					ActionTask actionTask = this.actions[i];
					if (actionTask != null && actionTask.isUserEnabled)
					{
						string text2 = actionTask.isPaused ? "<b>||</b> " : (actionTask.isRunning ? "► " : "▪");
						text = text + text2 + actionTask.summaryInfo + ((i == this.actions.Count - 1) ? "" : "\n");
					}
				}
				return text;
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00005D0C File Offset: 0x00003F0C
		public override Task Duplicate(ITaskSystem newOwnerSystem)
		{
			ActionList actionList = (ActionList)base.Duplicate(newOwnerSystem);
			actionList.actions.Clear();
			foreach (ActionTask actionTask in this.actions)
			{
				actionList.AddAction((ActionTask)actionTask.Duplicate(newOwnerSystem));
			}
			return actionList;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00005D84 File Offset: 0x00003F84
		protected override string OnInit()
		{
			this.finishedIndeces = new bool[this.actions.Count];
			return null;
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00005DA0 File Offset: 0x00003FA0
		protected override void OnExecute()
		{
			this.currentActionIndex = 0;
			for (int i = 0; i < this.actions.Count; i++)
			{
				this.finishedIndeces[i] = false;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00005DD4 File Offset: 0x00003FD4
		protected override void OnUpdate()
		{
			if (this.actions.Count == 0)
			{
				base.EndAction();
				return;
			}
			ActionList.ActionsExecutionMode actionsExecutionMode = this.executionMode;
			if (actionsExecutionMode != ActionList.ActionsExecutionMode.ActionsRunInSequence)
			{
				if (actionsExecutionMode == ActionList.ActionsExecutionMode.ActionsRunInParallel)
				{
					for (int i = 0; i < this.actions.Count; i++)
					{
						if (!this.finishedIndeces[i])
						{
							if (!this.actions[i].isUserEnabled)
							{
								this.finishedIndeces[i] = true;
							}
							else
							{
								Status status = this.actions[i].Execute(base.agent, base.blackboard);
								if (status == Status.Failure)
								{
									base.EndAction(false);
									return;
								}
								if (status == Status.Success)
								{
									this.finishedIndeces[i] = true;
								}
							}
						}
					}
					bool flag = true;
					for (int j = 0; j < this.actions.Count; j++)
					{
						flag &= this.finishedIndeces[j];
					}
					if (flag)
					{
						base.EndAction(true);
						return;
					}
				}
			}
			else
			{
				for (int k = this.currentActionIndex; k < this.actions.Count; k++)
				{
					if (this.actions[k].isUserEnabled)
					{
						Status status2 = this.actions[k].Execute(base.agent, base.blackboard);
						if (status2 == Status.Failure)
						{
							base.EndAction(false);
							return;
						}
						if (status2 == Status.Running)
						{
							this.currentActionIndex = k;
							return;
						}
					}
				}
				base.EndAction(true);
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00005F24 File Offset: 0x00004124
		protected override void OnStop()
		{
			for (int i = 0; i < this.actions.Count; i++)
			{
				if (this.actions[i].isUserEnabled)
				{
					this.actions[i].EndAction(default(bool?));
				}
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00005F74 File Offset: 0x00004174
		protected override void OnPause()
		{
			for (int i = 0; i < this.actions.Count; i++)
			{
				if (this.actions[i].isUserEnabled)
				{
					this.actions[i].Pause();
				}
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00005FBC File Offset: 0x000041BC
		public override void OnDrawGizmosSelected()
		{
			for (int i = 0; i < this.actions.Count; i++)
			{
				if (this.actions[i].isUserEnabled)
				{
					this.actions[i].OnDrawGizmosSelected();
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00006004 File Offset: 0x00004204
		public void AddAction(ActionTask action)
		{
			if (action is ActionList)
			{
				foreach (ActionTask action2 in (action as ActionList).actions)
				{
					this.AddAction(action2);
				}
				return;
			}
			this.actions.Add(action);
			action.SetOwnerSystem(base.ownerSystem);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00006080 File Offset: 0x00004280
		internal override string GetWarningOrError()
		{
			for (int i = 0; i < this.actions.Count; i++)
			{
				string warningOrError = this.actions[i].GetWarningOrError();
				if (warningOrError != null)
				{
					return warningOrError;
				}
			}
			return null;
		}

		// Token: 0x04000063 RID: 99
		public ActionList.ActionsExecutionMode executionMode;

		// Token: 0x04000064 RID: 100
		public List<ActionTask> actions = new List<ActionTask>();

		// Token: 0x04000065 RID: 101
		private int currentActionIndex;

		// Token: 0x04000066 RID: 102
		private bool[] finishedIndeces;

		// Token: 0x02000105 RID: 261
		public enum ActionsExecutionMode
		{
			// Token: 0x04000294 RID: 660
			ActionsRunInSequence,
			// Token: 0x04000295 RID: 661
			ActionsRunInParallel
		}
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000ED RID: 237
	[Description("Execute a number of Actions repeatedly and in parallel to any other FSM state while the FSM is running. Conditions are optional. This is not a state.")]
	[Color("ff64cb")]
	[ParadoxNotion.Design.Icon("Repeat", false, "")]
	[Name("On FSM Update", 0)]
	public class OnFSMUpdate : FSMNode, IUpdatable, IGraphElement
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0001007D File Offset: 0x0000E27D
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001008A File Offset: 0x0000E28A
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001008D File Offset: 0x0000E28D
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600048B RID: 1163 RVA: 0x00010090 File Offset: 0x0000E290
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00010094 File Offset: 0x0000E294
		public override void OnValidate(Graph assignedGraph)
		{
			if (this._conditionList == null)
			{
				this._conditionList = (ConditionList)Task.Create(typeof(ConditionList), assignedGraph);
				this._conditionList.checkMode = ConditionList.ConditionsCheckMode.AllTrueRequired;
			}
			if (this._actionList == null)
			{
				this._actionList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this._actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000100FF File Offset: 0x0000E2FF
		public override void OnGraphStarted()
		{
			this._conditionList.Enable(base.graphAgent, base.graphBlackboard);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00010118 File Offset: 0x0000E318
		public override void OnGraphStoped()
		{
			this._conditionList.Disable();
			this._actionList.EndAction(default(bool?));
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00010144 File Offset: 0x0000E344
		void IUpdatable.Update()
		{
			if (this._conditionList.Check(base.graphAgent, base.graphBlackboard))
			{
				base.status = this._actionList.Execute(base.graphAgent, base.graphBlackboard);
				return;
			}
			this._actionList.EndAction(default(bool?));
			base.status = Status.Failure;
		}

		// Token: 0x040002AF RID: 687
		[SerializeField]
		private ConditionList _conditionList;

		// Token: 0x040002B0 RID: 688
		[SerializeField]
		private ActionList _actionList;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000EB RID: 235
	[Description("Execute a number of Actions when the FSM starts/enters, if Conditions are met. This is not a state.")]
	[Color("ff64cb")]
	[ParadoxNotion.Design.Icon("MacroIn", false, "")]
	[Name("On FSM Enter", 0)]
	public class OnFSMEnter : FSMNode, IUpdatable, IGraphElement
	{
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0000FE39 File Offset: 0x0000E039
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0000FE46 File Offset: 0x0000E046
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0000FE49 File Offset: 0x0000E049
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0000FE4C File Offset: 0x0000E04C
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000FE50 File Offset: 0x0000E050
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

		// Token: 0x0600047C RID: 1148 RVA: 0x0000FEBC File Offset: 0x0000E0BC
		public override void OnGraphStarted()
		{
			this._conditionList.Enable(base.graphAgent, base.graphBlackboard);
			if (this._conditionList.Check(base.graphAgent, base.graphBlackboard))
			{
				base.status = this._actionList.Execute(base.graphAgent, base.graphBlackboard);
				return;
			}
			base.status = Status.Failure;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000FF20 File Offset: 0x0000E120
		public override void OnGraphStoped()
		{
			this._conditionList.Disable();
			this._actionList.EndAction(default(bool?));
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000FF4C File Offset: 0x0000E14C
		void IUpdatable.Update()
		{
			if (base.status == Status.Running)
			{
				base.status = this._actionList.Execute(base.graphAgent, base.graphBlackboard);
			}
		}

		// Token: 0x040002AB RID: 683
		[SerializeField]
		private ConditionList _conditionList;

		// Token: 0x040002AC RID: 684
		[SerializeField]
		private ActionList _actionList;
	}
}

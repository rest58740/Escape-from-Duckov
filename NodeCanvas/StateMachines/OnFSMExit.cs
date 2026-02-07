using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000EC RID: 236
	[Description("Execute a number of Actions when the FSM stops/exits, if Conditions are met. Note that the actions will only execute for 1 frame. This is not a state.")]
	[Color("ff64cb")]
	[ParadoxNotion.Design.Icon("MacroOut", false, "")]
	[Name("On FSM Exit", 0)]
	public class OnFSMExit : FSMNode
	{
		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x0000FF89 File Offset: 0x0000E189
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0000FF8C File Offset: 0x0000E18C
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x0000FF8F File Offset: 0x0000E18F
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0000FF94 File Offset: 0x0000E194
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

		// Token: 0x06000485 RID: 1157 RVA: 0x0000FFFF File Offset: 0x0000E1FF
		public override void OnGraphStarted()
		{
			this._conditionList.Enable(base.graphAgent, base.graphBlackboard);
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00010018 File Offset: 0x0000E218
		public override void OnGraphStoped()
		{
			if (this._conditionList.Check(base.graphAgent, base.graphBlackboard))
			{
				this._actionList.Execute(base.graphAgent, base.graphBlackboard);
			}
			this._actionList.EndAction(default(bool?));
			this._conditionList.Disable();
		}

		// Token: 0x040002AD RID: 685
		[SerializeField]
		private ConditionList _conditionList;

		// Token: 0x040002AE RID: 686
		[SerializeField]
		private ActionList _actionList;
	}
}

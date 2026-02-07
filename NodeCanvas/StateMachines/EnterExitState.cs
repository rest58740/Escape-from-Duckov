using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000F0 RID: 240
	[Name("Enter | Exit", 0)]
	[Description("Execute a number of Actions when the FSM enters/starts and when it exits/stops. This is not a state.")]
	[Color("ff64cb")]
	[ParadoxNotion.Design.Icon("MacroIn", false, "")]
	[Obsolete("Use On FSM Enter and On FSM Exit nodes")]
	public class EnterExitState : FSMNode, IUpdatable, IGraphElement
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x000104EE File Offset: 0x0000E6EE
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x000104F6 File Offset: 0x0000E6F6
		public ActionList actionListEnter
		{
			get
			{
				return this._actionListEnter;
			}
			set
			{
				this._actionListEnter = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x000104FF File Offset: 0x0000E6FF
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x00010507 File Offset: 0x0000E707
		public ActionList actionListExit
		{
			get
			{
				return this._actionListExit;
			}
			set
			{
				this._actionListExit = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x00010510 File Offset: 0x0000E710
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0001051D File Offset: 0x0000E71D
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00010520 File Offset: 0x0000E720
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00010523 File Offset: 0x0000E723
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00010528 File Offset: 0x0000E728
		public override void OnValidate(Graph assignedGraph)
		{
			if (this.actionListEnter == null)
			{
				this.actionListEnter = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this.actionListEnter.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
			if (this.actionListExit == null)
			{
				this.actionListExit = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this.actionListExit.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00010593 File Offset: 0x0000E793
		public override void OnGraphStarted()
		{
			base.status = this.actionListEnter.Execute(base.graphAgent, base.graphBlackboard);
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x000105B4 File Offset: 0x0000E7B4
		public override void OnGraphStoped()
		{
			this.actionListExit.Execute(base.graphAgent, base.graphBlackboard);
			this.actionListExit.EndAction(default(bool?));
			this.actionListEnter.EndAction(default(bool?));
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00010601 File Offset: 0x0000E801
		void IUpdatable.Update()
		{
			if (base.status == Status.Running)
			{
				base.status = this.actionListEnter.Execute(base.graphAgent, base.graphBlackboard);
			}
		}

		// Token: 0x040002B9 RID: 697
		[SerializeField]
		private ActionList _actionListEnter;

		// Token: 0x040002BA RID: 698
		[SerializeField]
		private ActionList _actionListExit;
	}
}

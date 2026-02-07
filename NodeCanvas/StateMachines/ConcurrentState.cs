using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000EF RID: 239
	[Name("Parallel", 0)]
	[Description("Execute a number of Actions with optional conditional requirement and in parallel to any other state, as soon as the FSM is started. All actions will prematurely be stoped as soon as the FSM stops as well. This is not a state.")]
	[Color("ff64cb")]
	[ParadoxNotion.Design.Icon("Repeat", false, "")]
	[Obsolete("Use On FSM Update node")]
	public class ConcurrentState : FSMNode, IUpdatable, IGraphElement
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x00010345 File Offset: 0x0000E545
		// (set) Token: 0x06000498 RID: 1176 RVA: 0x0001034D File Offset: 0x0000E54D
		public ConditionList conditionList
		{
			get
			{
				return this._conditionList;
			}
			set
			{
				this._conditionList = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00010356 File Offset: 0x0000E556
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001035E File Offset: 0x0000E55E
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00010367 File Offset: 0x0000E567
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0001036F File Offset: 0x0000E56F
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00010378 File Offset: 0x0000E578
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00010385 File Offset: 0x0000E585
		public override int maxInConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00010388 File Offset: 0x0000E588
		public override int maxOutConnections
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0001038B File Offset: 0x0000E58B
		public override bool allowAsPrime
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00010390 File Offset: 0x0000E590
		public override void OnValidate(Graph assignedGraph)
		{
			if (this.conditionList == null)
			{
				this.conditionList = (ConditionList)Task.Create(typeof(ConditionList), assignedGraph);
				this.conditionList.checkMode = ConditionList.ConditionsCheckMode.AllTrueRequired;
			}
			if (this.actionList == null)
			{
				this.actionList = (ActionList)Task.Create(typeof(ActionList), assignedGraph);
				this.actionList.executionMode = ActionList.ActionsExecutionMode.ActionsRunInParallel;
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x000103FB File Offset: 0x0000E5FB
		public override void OnGraphStarted()
		{
			this.conditionList.Enable(base.graphAgent, base.graphBlackboard);
			this.done = false;
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001041C File Offset: 0x0000E61C
		public override void OnGraphStoped()
		{
			this.conditionList.Disable();
			this.actionList.EndAction(default(bool?));
			this.done = false;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001044F File Offset: 0x0000E64F
		public override void OnGraphPaused()
		{
			this.actionList.Pause();
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001045C File Offset: 0x0000E65C
		void IUpdatable.Update()
		{
			if (this.done && !this.repeatStateActions)
			{
				return;
			}
			base.status = Status.Running;
			if (this.conditionList.Check(base.graphAgent, base.graphBlackboard))
			{
				if (this.actionList.Execute(base.graphAgent, base.graphBlackboard) != Status.Running)
				{
					if (!this.repeatStateActions)
					{
						base.status = Status.Success;
					}
					this.done = true;
					return;
				}
			}
			else
			{
				this.actionList.EndAction(default(bool?));
				base.status = Status.Failure;
			}
		}

		// Token: 0x040002B5 RID: 693
		[SerializeField]
		private ConditionList _conditionList;

		// Token: 0x040002B6 RID: 694
		[SerializeField]
		private ActionList _actionList;

		// Token: 0x040002B7 RID: 695
		[SerializeField]
		private bool _repeatStateActions;

		// Token: 0x040002B8 RID: 696
		private bool done;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000EA RID: 234
	[Name("Sub FSM", 0)]
	[Description("Execute a sub FSM OnEnter, and Stop that FSM OnExit. This state is Finished only when and if the sub FSM is finished as well.")]
	[DropReferenceType(typeof(FSM))]
	[ParadoxNotion.Design.Icon("FSM", false, "")]
	public class NestedFSMState : FSMStateNested<FSM>
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000FD7F File Offset: 0x0000DF7F
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		public override FSM subGraph
		{
			get
			{
				return this._nestedFSM.value;
			}
			set
			{
				this._nestedFSM.value = value;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0000FD9A File Offset: 0x0000DF9A
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._nestedFSM;
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		protected override void OnEnter()
		{
			if (this.subGraph == null)
			{
				base.Finish(false);
				return;
			}
			this.TryStartSubGraph(base.graphAgent, new Action<bool>(this.Finish));
			this.OnUpdate();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000FDDA File Offset: 0x0000DFDA
		protected override void OnUpdate()
		{
			base.currentInstance.UpdateGraph(base.graph.deltaTime);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000FDF2 File Offset: 0x0000DFF2
		protected override void OnExit()
		{
			if (base.currentInstance != null)
			{
				if (base.status == Status.Running)
				{
					this.TryReadAndUnbindMappedVariables();
				}
				if (this.exitMode == NestedFSMState.FSMExitMode.StopAndRestart)
				{
					base.currentInstance.Stop(true);
					return;
				}
				base.currentInstance.Pause();
			}
		}

		// Token: 0x040002A9 RID: 681
		[SerializeField]
		[ExposeField]
		[Name("Sub FSM", 0)]
		private BBParameter<FSM> _nestedFSM;

		// Token: 0x040002AA RID: 682
		[Tooltip("What will happen to the sub FSM when this state exits.")]
		public NestedFSMState.FSMExitMode exitMode;

		// Token: 0x02000150 RID: 336
		public enum FSMExitMode
		{
			// Token: 0x040003CE RID: 974
			StopAndRestart,
			// Token: 0x040003CF RID: 975
			PauseAndResume
		}
	}
}

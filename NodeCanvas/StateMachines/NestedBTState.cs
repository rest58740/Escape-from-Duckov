using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.StateMachines
{
	// Token: 0x020000E8 RID: 232
	[Name("Sub BehaviourTree", 0)]
	[Description("Execute a Behaviour Tree OnEnter. OnExit that Behavior Tree will be stoped or paused based on the relevant specified setting. You can optionaly specify a Success Event and a Failure Event which will be sent when the BT's root node status returns either of the two. If so, use alongside with a CheckEvent on a transition.")]
	[DropReferenceType(typeof(BehaviourTree))]
	[ParadoxNotion.Design.Icon("BT", false, "")]
	public class NestedBTState : FSMStateNested<BehaviourTree>
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0000FAC1 File Offset: 0x0000DCC1
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x0000FACE File Offset: 0x0000DCCE
		public override BehaviourTree subGraph
		{
			get
			{
				return this._nestedBT.value;
			}
			set
			{
				this._nestedBT.value = value;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0000FADC File Offset: 0x0000DCDC
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._nestedBT;
			}
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000FAE4 File Offset: 0x0000DCE4
		protected override void OnEnter()
		{
			if (this.subGraph == null)
			{
				base.Finish(false);
				return;
			}
			base.currentInstance = (BehaviourTree)this.CheckInstance();
			base.currentInstance.repeat = (this.executionMode == NestedBTState.BTExecutionMode.Repeat);
			base.currentInstance.updateInterval = 0f;
			this.TryWriteAndBindMappedVariables();
			base.currentInstance.StartGraph(base.graph.agent, base.graph.blackboard.parent, Graph.UpdateMode.Manual, new Action<bool>(this.OnFinish));
			this.OnUpdate();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000FB7C File Offset: 0x0000DD7C
		protected override void OnUpdate()
		{
			base.currentInstance.UpdateGraph(base.graph.deltaTime);
			if (!string.IsNullOrEmpty(this.successEvent) && base.currentInstance.rootStatus == Status.Success)
			{
				base.currentInstance.Stop(true);
			}
			if (!string.IsNullOrEmpty(this.failureEvent) && base.currentInstance.rootStatus == Status.Failure)
			{
				base.currentInstance.Stop(false);
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		private void OnFinish(bool success)
		{
			if (base.status == Status.Running)
			{
				this.TryReadAndUnbindMappedVariables();
				if (!string.IsNullOrEmpty(this.successEvent) && success)
				{
					base.SendEvent(this.successEvent);
				}
				if (!string.IsNullOrEmpty(this.failureEvent) && !success)
				{
					base.SendEvent(this.failureEvent);
				}
				base.Finish(success);
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000FC49 File Offset: 0x0000DE49
		protected override void OnExit()
		{
			if (base.currentInstance != null)
			{
				if (base.status == Status.Running)
				{
					this.TryReadAndUnbindMappedVariables();
				}
				if (this.exitMode == NestedBTState.BTExitMode.StopAndRestart)
				{
					base.currentInstance.Stop(true);
					return;
				}
				base.currentInstance.Pause();
			}
		}

		// Token: 0x040002A1 RID: 673
		[SerializeField]
		[ExposeField]
		[Name("Sub Tree", 0)]
		private BBParameter<BehaviourTree> _nestedBT;

		// Token: 0x040002A2 RID: 674
		[Tooltip("What will happen to the BT when this state exits.")]
		public NestedBTState.BTExitMode exitMode;

		// Token: 0x040002A3 RID: 675
		[Tooltip("Sould the BT repeat?")]
		public NestedBTState.BTExecutionMode executionMode = NestedBTState.BTExecutionMode.Repeat;

		// Token: 0x040002A4 RID: 676
		[DimIfDefault]
		[Tooltip("The event to send when the BT finish in Success.")]
		public string successEvent;

		// Token: 0x040002A5 RID: 677
		[DimIfDefault]
		[Tooltip("The event to send when the BT finish in Failure.")]
		public string failureEvent;

		// Token: 0x0200014E RID: 334
		public enum BTExecutionMode
		{
			// Token: 0x040003C8 RID: 968
			Once,
			// Token: 0x040003C9 RID: 969
			Repeat
		}

		// Token: 0x0200014F RID: 335
		public enum BTExitMode
		{
			// Token: 0x040003CB RID: 971
			StopAndRestart,
			// Token: 0x040003CC RID: 972
			PauseAndResume
		}
	}
}

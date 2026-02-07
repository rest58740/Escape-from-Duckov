using System;
using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200012B RID: 299
	[Name("Sub FSM", 0)]
	[Description("Executes a sub FSM. Returns Running while the sub FSM is active. If a Success or Failure State is selected, then it will return Success or Failure as soon as the Nested FSM enters that state at which point the sub FSM will also be stoped. If the sub FSM ends otherwise, this node will return Success.")]
	[ParadoxNotion.Design.Icon("FSM", false, "")]
	[DropReferenceType(typeof(FSM))]
	public class NestedFSM : BTNodeNested<FSM>
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x00013E83 File Offset: 0x00012083
		// (set) Token: 0x0600064F RID: 1615 RVA: 0x00013E90 File Offset: 0x00012090
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

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000650 RID: 1616 RVA: 0x00013E9E File Offset: 0x0001209E
		public override BBParameter subGraphParameter
		{
			get
			{
				return this._nestedFSM;
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00013EA8 File Offset: 0x000120A8
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.subGraph == null || this.subGraph.primeNode == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				base.status = Status.Running;
				this.TryStartSubGraph(agent, new Action<bool>(this.OnFSMFinish));
			}
			if (base.status == Status.Running)
			{
				base.currentInstance.UpdateGraph(base.graph.deltaTime);
			}
			if (!string.IsNullOrEmpty(this.successState) && base.currentInstance.currentStateName == this.successState)
			{
				base.currentInstance.Stop(true);
				return Status.Success;
			}
			if (!string.IsNullOrEmpty(this.failureState) && base.currentInstance.currentStateName == this.failureState)
			{
				base.currentInstance.Stop(false);
				return Status.Failure;
			}
			return base.status;
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00013F81 File Offset: 0x00012181
		private void OnFSMFinish(bool success)
		{
			if (base.status == Status.Running)
			{
				base.status = (success ? Status.Success : Status.Failure);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00013F99 File Offset: 0x00012199
		protected override void OnReset()
		{
			if (base.currentInstance != null)
			{
				base.currentInstance.Stop(true);
			}
		}

		// Token: 0x04000355 RID: 853
		[SerializeField]
		[ExposeField]
		[Name("Sub FSM", 0)]
		private BBParameter<FSM> _nestedFSM;

		// Token: 0x04000356 RID: 854
		[HideInInspector]
		public string successState;

		// Token: 0x04000357 RID: 855
		[HideInInspector]
		public string failureState;
	}
}

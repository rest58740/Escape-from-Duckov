using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000112 RID: 274
	[Name("Parallel", 8)]
	[Category("Composites")]
	[Description("Executes all children simultaneously and return Success or Failure depending on the selected Policy.")]
	[ParadoxNotion.Design.Icon("Parallel", false, "")]
	[Color("ff64cb")]
	public class Parallel : BTComposite
	{
		// Token: 0x060005D6 RID: 1494 RVA: 0x00012793 File Offset: 0x00010993
		public override void OnGraphStarted()
		{
			this.finishedConnections = new bool[base.outConnections.Count];
			this.finishedConnectionsCount = 0;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000127B4 File Offset: 0x000109B4
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			Status status = Status.Resting;
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				Connection connection = base.outConnections[i];
				bool flag = this.finishedConnections[i];
				if (this.dynamic || !flag)
				{
					if (connection.status != Status.Running && flag)
					{
						connection.Reset(true);
					}
					base.status = connection.Execute(agent, blackboard);
					if (status == Status.Resting)
					{
						if (base.status == Status.Failure && (this.policy == Parallel.ParallelPolicy.FirstFailure || this.policy == Parallel.ParallelPolicy.FirstSuccessOrFailure))
						{
							status = Status.Failure;
						}
						if (base.status == Status.Success && (this.policy == Parallel.ParallelPolicy.FirstSuccess || this.policy == Parallel.ParallelPolicy.FirstSuccessOrFailure))
						{
							status = Status.Success;
						}
					}
					if (base.status != Status.Running && !flag)
					{
						this.finishedConnections[i] = true;
						this.finishedConnectionsCount++;
					}
				}
			}
			if (status != Status.Resting)
			{
				this.ResetRunning();
				base.status = status;
				return status;
			}
			if (this.finishedConnectionsCount == base.outConnections.Count)
			{
				this.ResetRunning();
				Parallel.ParallelPolicy parallelPolicy = this.policy;
				if (parallelPolicy == Parallel.ParallelPolicy.FirstFailure)
				{
					return Status.Success;
				}
				if (parallelPolicy == Parallel.ParallelPolicy.FirstSuccess)
				{
					return Status.Failure;
				}
			}
			return Status.Running;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000128D0 File Offset: 0x00010AD0
		protected override void OnReset()
		{
			for (int i = 0; i < this.finishedConnections.Length; i++)
			{
				this.finishedConnections[i] = false;
			}
			this.finishedConnectionsCount = 0;
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00012900 File Offset: 0x00010B00
		private void ResetRunning()
		{
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				if (base.outConnections[i].status == Status.Running)
				{
					base.outConnections[i].Reset(true);
				}
			}
		}

		// Token: 0x0400030D RID: 781
		[Tooltip("The policy determines when the Parallel node will end and return its Status.")]
		public Parallel.ParallelPolicy policy;

		// Token: 0x0400030E RID: 782
		[Name("Repeat", 0)]
		[Tooltip("If true, finished children are repeated until the Policy set is met, or until all children have had a chance to finish at least once.")]
		public bool dynamic;

		// Token: 0x0400030F RID: 783
		private bool[] finishedConnections;

		// Token: 0x04000310 RID: 784
		private int finishedConnectionsCount;

		// Token: 0x02000167 RID: 359
		public enum ParallelPolicy
		{
			// Token: 0x0400040C RID: 1036
			FirstFailure,
			// Token: 0x0400040D RID: 1037
			FirstSuccess,
			// Token: 0x0400040E RID: 1038
			FirstSuccessOrFailure
		}
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000117 RID: 279
	[Name("Sequencer", 10)]
	[Category("Composites")]
	[Description("Executes its children in order and returns Success if all children return Success. As soon as a child returns Failure, the Sequencer will stop and return Failure as well.")]
	[ParadoxNotion.Design.Icon("Sequencer", false, "")]
	[Color("bf7fff")]
	public class Sequencer : BTComposite
	{
		// Token: 0x060005EE RID: 1518 RVA: 0x00012E60 File Offset: 0x00011060
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			for (int i = this.dynamic ? 0 : this.lastRunningNodeIndex; i < base.outConnections.Count; i++)
			{
				base.status = base.outConnections[i].Execute(agent, blackboard);
				Status status = base.status;
				if (status == Status.Failure)
				{
					if (this.dynamic && i < this.lastRunningNodeIndex)
					{
						for (int j = i + 1; j <= this.lastRunningNodeIndex; j++)
						{
							base.outConnections[j].Reset(true);
						}
					}
					return Status.Failure;
				}
				if (status == Status.Running)
				{
					if (this.dynamic && i < this.lastRunningNodeIndex)
					{
						for (int k = i + 1; k <= this.lastRunningNodeIndex; k++)
						{
							base.outConnections[k].Reset(true);
						}
					}
					this.lastRunningNodeIndex = i;
					return Status.Running;
				}
			}
			return Status.Success;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00012F38 File Offset: 0x00011138
		protected override void OnReset()
		{
			this.lastRunningNodeIndex = 0;
			if (this.random)
			{
				base.outConnections = base.outConnections.Shuffle<Connection>();
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00012F5A File Offset: 0x0001115A
		public override void OnChildDisconnected(int index)
		{
			if (index != 0 && index == this.lastRunningNodeIndex)
			{
				this.lastRunningNodeIndex--;
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00012F76 File Offset: 0x00011176
		public override void OnGraphStarted()
		{
			this.OnReset();
		}

		// Token: 0x04000320 RID: 800
		[Tooltip("If true, then higher priority children are re-evaluated per frame and if either returns Failure, then the Sequencer will immediately stop and return Failure as well.")]
		public bool dynamic;

		// Token: 0x04000321 RID: 801
		[Tooltip("If true, the children order of execution is shuffled each time the Sequencer resets.")]
		public bool random;

		// Token: 0x04000322 RID: 802
		private int lastRunningNodeIndex;
	}
}

using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000116 RID: 278
	[Name("Selector", 9)]
	[Category("Composites")]
	[Description("Executes its childrfen in order and returns Failure if all children return Failure. As soon as a child returns Success, the Selector will stop and return Success as well.")]
	[ParadoxNotion.Design.Icon("Selector", false, "")]
	[Color("b3ff7f")]
	public class Selector : BTComposite
	{
		// Token: 0x060005E9 RID: 1513 RVA: 0x00012D38 File Offset: 0x00010F38
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			for (int i = this.dynamic ? 0 : this.lastRunningNodeIndex; i < base.outConnections.Count; i++)
			{
				base.status = base.outConnections[i].Execute(agent, blackboard);
				Status status = base.status;
				if (status == Status.Success)
				{
					if (this.dynamic && i < this.lastRunningNodeIndex)
					{
						for (int j = i + 1; j <= this.lastRunningNodeIndex; j++)
						{
							base.outConnections[j].Reset(true);
						}
					}
					return Status.Success;
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
			return Status.Failure;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00012E11 File Offset: 0x00011011
		protected override void OnReset()
		{
			this.lastRunningNodeIndex = 0;
			if (this.random)
			{
				base.outConnections = base.outConnections.Shuffle<Connection>();
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00012E33 File Offset: 0x00011033
		public override void OnChildDisconnected(int index)
		{
			if (index != 0 && index == this.lastRunningNodeIndex)
			{
				this.lastRunningNodeIndex--;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00012E4F File Offset: 0x0001104F
		public override void OnGraphStarted()
		{
			this.OnReset();
		}

		// Token: 0x0400031D RID: 797
		[Tooltip("If true, then higher priority children are re-evaluated per frame and if either returns Success, then the Selector will immediately stop and return Success as well.")]
		public bool dynamic;

		// Token: 0x0400031E RID: 798
		[Tooltip("If true, the children order of execution is shuffled each time the Selector resets.")]
		public bool random;

		// Token: 0x0400031F RID: 799
		private int lastRunningNodeIndex;
	}
}

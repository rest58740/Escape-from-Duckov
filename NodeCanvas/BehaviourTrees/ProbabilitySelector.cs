using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000115 RID: 277
	[Category("Composites")]
	[Description("Selects a child to execute based on its chance to be selected and returns Success if the child returns Success, otherwise picks another child.\nReturns Failure if all children return Failure, or a direct 'Failure Chance' is introduced.")]
	[ParadoxNotion.Design.Icon("ProbabilitySelector", false, "")]
	[Color("b3ff7f")]
	public class ProbabilitySelector : BTComposite
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00012B58 File Offset: 0x00010D58
		public override void OnChildConnected(int index)
		{
			if (this.childWeights == null)
			{
				this.childWeights = new List<BBParameter<float>>();
			}
			if (this.childWeights.Count < base.outConnections.Count)
			{
				this.childWeights.Insert(index, new BBParameter<float>
				{
					value = 1f,
					bb = base.graphBlackboard
				});
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00012BB8 File Offset: 0x00010DB8
		public override void OnChildDisconnected(int index)
		{
			this.childWeights.RemoveAt(index);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00012BC6 File Offset: 0x00010DC6
		public override void OnGraphStarted()
		{
			this.OnReset();
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00012BD0 File Offset: 0x00010DD0
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.status == Status.Resting)
			{
				this.tmpDice = Random.value;
				this.tmpFailWeight = this.failChance.value;
				this.tmpTotal = this.tmpFailWeight;
				for (int i = 0; i < this.childWeights.Count; i++)
				{
					float value = this.childWeights[i].value;
					this.tmpTotal += value;
					this.tmpWeights[i] = value;
				}
			}
			float num = this.tmpFailWeight / this.tmpTotal;
			if (this.tmpDice < num)
			{
				return Status.Failure;
			}
			for (int j = 0; j < base.outConnections.Count; j++)
			{
				if (!this.indexFailed[j])
				{
					num += this.tmpWeights[j] / this.tmpTotal;
					if (this.tmpDice <= num)
					{
						base.status = base.outConnections[j].Execute(agent, blackboard);
						if (base.status == Status.Success || base.status == Status.Running)
						{
							return base.status;
						}
						if (base.status == Status.Failure)
						{
							this.indexFailed[j] = true;
							this.tmpTotal -= this.tmpWeights[j];
							return Status.Running;
						}
					}
				}
			}
			return Status.Failure;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00012D02 File Offset: 0x00010F02
		protected override void OnReset()
		{
			this.tmpWeights = new float[base.outConnections.Count];
			this.indexFailed = new bool[base.outConnections.Count];
		}

		// Token: 0x04000316 RID: 790
		[Node.AutoSortWithChildrenConnections]
		[Tooltip("The weights of the children.")]
		public List<BBParameter<float>> childWeights;

		// Token: 0x04000317 RID: 791
		[Tooltip("A chance for the node to fail immediately.")]
		public BBParameter<float> failChance;

		// Token: 0x04000318 RID: 792
		private bool[] indexFailed;

		// Token: 0x04000319 RID: 793
		private float[] tmpWeights;

		// Token: 0x0400031A RID: 794
		private float tmpFailWeight;

		// Token: 0x0400031B RID: 795
		private float tmpTotal;

		// Token: 0x0400031C RID: 796
		private float tmpDice;
	}
}

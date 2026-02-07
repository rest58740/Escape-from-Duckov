using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x02000104 RID: 260
	[Category("Branch")]
	[Description("Select a child to execute based on it's chance to be selected. An optional pre-Condition Task can be assigned to filter the child in or out of the selection probability.\nThe actor selected will be used for the condition checks.")]
	[ParadoxNotion.Design.Icon("ProbabilitySelector", false, "")]
	[Color("b3ff7f")]
	public class ProbabilitySelector : DTNode
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001175C File Offset: 0x0000F95C
		public override int maxOutConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0001175F File Offset: 0x0000F95F
		public override void OnChildConnected(int index)
		{
			if (this.childOptions.Count < base.outConnections.Count)
			{
				this.childOptions.Insert(index, new ProbabilitySelector.Option(1f, base.graphBlackboard));
			}
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x00011795 File Offset: 0x0000F995
		public override void OnChildDisconnected(int index)
		{
			this.childOptions.RemoveAt(index);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x000117A4 File Offset: 0x0000F9A4
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			this.successIndeces = new List<int>();
			for (int i = 0; i < base.outConnections.Count; i++)
			{
				ConditionTask condition = this.childOptions[i].condition;
				if (condition == null || condition.CheckOnce(base.finalActor.transform, blackboard))
				{
					this.successIndeces.Add(i);
				}
			}
			float num = Random.Range(0f, this.GetTotal());
			for (int j = 0; j < base.outConnections.Count; j++)
			{
				if (this.successIndeces.Contains(j))
				{
					if (num <= this.childOptions[j].weight.value)
					{
						base.DLGTree.Continue(j);
						return Status.Success;
					}
					num -= this.childOptions[j].weight.value;
				}
			}
			return Status.Failure;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x00011880 File Offset: 0x0000FA80
		private float GetTotal()
		{
			float num = 0f;
			for (int i = 0; i < this.childOptions.Count; i++)
			{
				ProbabilitySelector.Option option = this.childOptions[i];
				if (this.successIndeces == null || this.successIndeces.Contains(i))
				{
					num += option.weight.value;
				}
			}
			return num;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000118DB File Offset: 0x0000FADB
		protected override void OnReset()
		{
			this.successIndeces = null;
		}

		// Token: 0x040002EA RID: 746
		[SerializeField]
		[Node.AutoSortWithChildrenConnections]
		private List<ProbabilitySelector.Option> childOptions = new List<ProbabilitySelector.Option>();

		// Token: 0x040002EB RID: 747
		private List<int> successIndeces;

		// Token: 0x0200015D RID: 349
		public class Option
		{
			// Token: 0x060006C3 RID: 1731 RVA: 0x00014A32 File Offset: 0x00012C32
			public Option(float weightValue, IBlackboard bbValue)
			{
				this.weight = new BBParameter<float>
				{
					value = weightValue,
					bb = bbValue
				};
				this.condition = null;
			}

			// Token: 0x040003E6 RID: 998
			public BBParameter<float> weight;

			// Token: 0x040003E7 RID: 999
			public ConditionTask condition;
		}
	}
}

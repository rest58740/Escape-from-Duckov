using System;
using System.Collections.Generic;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000119 RID: 281
	[Category("Composites")]
	[Description("Executes one child based on the provided int or enum case and returns its status.")]
	[ParadoxNotion.Design.Icon("IndexSwitcher", false, "")]
	[Color("b3ff7f")]
	public class Switch : BTComposite
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x00012FDC File Offset: 0x000111DC
		public override void OnGraphStarted()
		{
			if (this.selectionMode == Switch.CaseSelectionMode.EnumBased)
			{
				object value = this.enumCase.value;
				if (value != null)
				{
					this.enumCasePairing = new Dictionary<int, int>();
					Array values = Enum.GetValues(value.GetType());
					for (int i = 0; i < values.Length; i++)
					{
						this.enumCasePairing[(int)values.GetValue(i)] = i;
					}
				}
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00013044 File Offset: 0x00011244
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.outConnections.Count == 0)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting || this.dynamic)
			{
				if (this.selectionMode == Switch.CaseSelectionMode.IndexBased)
				{
					this.current = this.intCase.value;
					if (this.outOfRangeMode == Switch.OutOfRangeMode.LoopIndex)
					{
						this.current = Mathf.Abs(this.current) % base.outConnections.Count;
					}
				}
				else
				{
					this.current = this.enumCasePairing[(int)this.enumCase.value];
				}
				if (this.runningIndex != this.current)
				{
					base.outConnections[this.runningIndex].Reset(true);
				}
				if (this.current < 0 || this.current >= base.outConnections.Count)
				{
					return Status.Failure;
				}
			}
			base.status = base.outConnections[this.current].Execute(agent, blackboard);
			if (base.status == Status.Running)
			{
				this.runningIndex = this.current;
			}
			return base.status;
		}

		// Token: 0x04000324 RID: 804
		[Tooltip("If true and the 'case' change while a child is running, that child will immediately be interrupted and the new child will be executed.")]
		public bool dynamic;

		// Token: 0x04000325 RID: 805
		[Tooltip("The selection mode used.")]
		public Switch.CaseSelectionMode selectionMode;

		// Token: 0x04000326 RID: 806
		[ShowIf("selectionMode", 0)]
		public BBParameter<int> intCase;

		// Token: 0x04000327 RID: 807
		[ShowIf("selectionMode", 0)]
		public Switch.OutOfRangeMode outOfRangeMode = Switch.OutOfRangeMode.LoopIndex;

		// Token: 0x04000328 RID: 808
		[ShowIf("selectionMode", 1)]
		[BlackboardOnly]
		public BBObjectParameter enumCase = new BBObjectParameter(typeof(Enum));

		// Token: 0x04000329 RID: 809
		private Dictionary<int, int> enumCasePairing;

		// Token: 0x0400032A RID: 810
		private int current;

		// Token: 0x0400032B RID: 811
		private int runningIndex;

		// Token: 0x0200016A RID: 362
		public enum CaseSelectionMode
		{
			// Token: 0x04000415 RID: 1045
			IndexBased,
			// Token: 0x04000416 RID: 1046
			EnumBased
		}

		// Token: 0x0200016B RID: 363
		public enum OutOfRangeMode
		{
			// Token: 0x04000418 RID: 1048
			ReturnFailure,
			// Token: 0x04000419 RID: 1049
			LoopIndex
		}
	}
}

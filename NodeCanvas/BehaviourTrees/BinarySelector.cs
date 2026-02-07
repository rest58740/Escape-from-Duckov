using System;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000110 RID: 272
	[Category("Composites")]
	[Description("Quick way to execute the left or the right child, based on a Condition Task.")]
	[ParadoxNotion.Design.Icon("Condition", false, "")]
	[Color("b3ff7f")]
	public class BinarySelector : BTNode, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x000125EF File Offset: 0x000107EF
		public override int maxOutConnections
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060005C9 RID: 1481 RVA: 0x000125F2 File Offset: 0x000107F2
		public override Alignment2x2 commentsAlignment
		{
			get
			{
				return Alignment2x2.Right;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x000125F5 File Offset: 0x000107F5
		public override string name
		{
			get
			{
				return base.name.ToUpper();
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x00012602 File Offset: 0x00010802
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0001260A File Offset: 0x0001080A
		public Task task
		{
			get
			{
				return this.condition;
			}
			set
			{
				this.condition = (ConditionTask)value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x00012618 File Offset: 0x00010818
		// (set) Token: 0x060005CE RID: 1486 RVA: 0x00012620 File Offset: 0x00010820
		private ConditionTask condition
		{
			get
			{
				return this._condition;
			}
			set
			{
				this._condition = value;
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0001262C File Offset: 0x0001082C
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (this.condition == null || base.outConnections.Count < 2)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				this.condition.Enable(agent, blackboard);
			}
			if (this.dynamic || base.status == Status.Resting)
			{
				int num = this.succeedIndex;
				this.succeedIndex = (this.condition.Check(agent, blackboard) ? 0 : 1);
				if (this.succeedIndex != num)
				{
					base.outConnections[num].Reset(true);
				}
			}
			return base.outConnections[this.succeedIndex].Execute(agent, blackboard);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000126CB File Offset: 0x000108CB
		protected override void OnReset()
		{
			if (this.condition != null)
			{
				this.condition.Disable();
			}
		}

		// Token: 0x04000309 RID: 777
		[Tooltip("If true, the condition will be re-evaluated per frame.")]
		public bool dynamic;

		// Token: 0x0400030A RID: 778
		[SerializeField]
		private ConditionTask _condition;

		// Token: 0x0400030B RID: 779
		private int succeedIndex;
	}
}

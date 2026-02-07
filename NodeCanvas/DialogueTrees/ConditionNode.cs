using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.DialogueTrees
{
	// Token: 0x020000FF RID: 255
	[ParadoxNotion.Design.Icon("Condition", false, "")]
	[Name("Task Condition", 0)]
	[Category("Branch")]
	[Description("Execute the first child node if a Condition is true, or the second one if that Condition is false. The Actor selected is used for the Condition check")]
	[Color("b3ff7f")]
	public class ConditionNode : DTNode, ITaskAssignable<ConditionTask>, ITaskAssignable, IGraphElement
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x000113D6 File Offset: 0x0000F5D6
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x000113DE File Offset: 0x0000F5DE
		public ConditionTask condition
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

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000113E7 File Offset: 0x0000F5E7
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x000113EF File Offset: 0x0000F5EF
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

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000113FD File Offset: 0x0000F5FD
		public override int maxOutConnections
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00011400 File Offset: 0x0000F600
		public override bool requireActorSelection
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00011404 File Offset: 0x0000F604
		protected override Status OnExecute(Component agent, IBlackboard bb)
		{
			if (base.outConnections.Count == 0)
			{
				return base.Error("There are no connections on the Dialogue Condition Node");
			}
			if (this.condition == null)
			{
				return base.Error("There is no Conidition on the Dialoge Condition Node");
			}
			bool flag = this.condition.CheckOnce(base.finalActor.transform, base.graphBlackboard);
			base.status = (flag ? Status.Success : Status.Failure);
			base.DLGTree.Continue(flag ? 0 : 1);
			return base.status;
		}

		// Token: 0x040002E3 RID: 739
		[SerializeField]
		private ConditionTask _condition;
	}
}

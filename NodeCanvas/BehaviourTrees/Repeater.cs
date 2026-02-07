using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000124 RID: 292
	[Name("Repeat", 0)]
	[Category("Decorators")]
	[Description("Repeats the child either x times or until it returns the specified status, or forever.")]
	[ParadoxNotion.Design.Icon("Repeat", false, "")]
	public class Repeater : BTDecorator
	{
		// Token: 0x06000628 RID: 1576 RVA: 0x000139EC File Offset: 0x00011BEC
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			if (base.decoratedConnection.status != Status.Running)
			{
				base.decoratedConnection.Reset(true);
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			Status status = base.status;
			if (status == Status.Running)
			{
				return Status.Running;
			}
			if (status == Status.Resting)
			{
				return Status.Running;
			}
			Repeater.RepeaterMode repeaterMode = this.repeaterMode;
			if (repeaterMode != Repeater.RepeaterMode.RepeatTimes)
			{
				if (repeaterMode == Repeater.RepeaterMode.RepeatUntil)
				{
					if (base.status == (Status)this.repeatUntilStatus)
					{
						return base.status;
					}
				}
			}
			else
			{
				if (this.currentIteration >= this.repeatTimes.value)
				{
					return base.status;
				}
				this.currentIteration++;
			}
			return Status.Running;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00013A93 File Offset: 0x00011C93
		protected override void OnReset()
		{
			this.currentIteration = 1;
		}

		// Token: 0x04000349 RID: 841
		public Repeater.RepeaterMode repeaterMode;

		// Token: 0x0400034A RID: 842
		[ShowIf("repeaterMode", 0)]
		public BBParameter<int> repeatTimes = 1;

		// Token: 0x0400034B RID: 843
		[ShowIf("repeaterMode", 1)]
		public Repeater.RepeatUntilStatus repeatUntilStatus = Repeater.RepeatUntilStatus.Success;

		// Token: 0x0400034C RID: 844
		private int currentIteration = 1;

		// Token: 0x02000174 RID: 372
		public enum RepeaterMode
		{
			// Token: 0x04000436 RID: 1078
			RepeatTimes,
			// Token: 0x04000437 RID: 1079
			RepeatUntil,
			// Token: 0x04000438 RID: 1080
			RepeatForever
		}

		// Token: 0x02000175 RID: 373
		public enum RepeatUntilStatus
		{
			// Token: 0x0400043A RID: 1082
			Failure,
			// Token: 0x0400043B RID: 1083
			Success
		}
	}
}

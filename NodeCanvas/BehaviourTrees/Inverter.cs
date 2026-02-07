using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200011E RID: 286
	[Name("Invert", 0)]
	[Category("Decorators")]
	[Description("Inverts Success to Failure and Failure to Success.")]
	[ParadoxNotion.Design.Icon("Remap", false, "")]
	public class Inverter : BTDecorator
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x00013664 File Offset: 0x00011864
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			Status status = base.status;
			if (status == Status.Failure)
			{
				return Status.Success;
			}
			if (status == Status.Success)
			{
				return Status.Failure;
			}
			return base.status;
		}
	}
}

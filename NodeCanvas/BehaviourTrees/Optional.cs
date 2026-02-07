using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000122 RID: 290
	[Name("Optional", 0)]
	[Category("Decorators")]
	[Description("Executes the decorated child as normal and returns an Optional status, thus making it optional to the parent node in regards to what status is returned.\nThis has the same effect as disabling the node, but instead it executes normaly.")]
	[ParadoxNotion.Design.Icon("UpwardsArrow", false, "")]
	public class Optional : BTDecorator
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x00013948 File Offset: 0x00011B48
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			if (base.status == Status.Resting)
			{
				base.decoratedConnection.Reset(true);
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			if (base.status != Status.Running)
			{
				return Status.Optional;
			}
			return Status.Running;
		}
	}
}

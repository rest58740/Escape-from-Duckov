using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000126 RID: 294
	[Category("Decorators")]
	[Description("Interupts decorated child node and returns Failure if the child node is still Running after the timeout period.")]
	[ParadoxNotion.Design.Icon("Timeout", false, "")]
	public class Timeout : BTDecorator
	{
		// Token: 0x0600062D RID: 1581 RVA: 0x00013B04 File Offset: 0x00011D04
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			base.status = base.decoratedConnection.Execute(agent, blackboard);
			if (base.status == Status.Running && base.elapsedTime >= this.timeout.value)
			{
				base.decoratedConnection.Reset(true);
				return Status.Failure;
			}
			return base.status;
		}

		// Token: 0x0400034F RID: 847
		[Tooltip("The timeout period in seconds.")]
		public BBParameter<float> timeout = 1f;
	}
}

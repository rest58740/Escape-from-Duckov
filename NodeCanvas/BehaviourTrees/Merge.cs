using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000120 RID: 288
	[Name("Merge", -1)]
	[Description("Merge can accept multiple input connections and thus possible to re-use leaf nodes from multiple parents. Please note that this is experimental and can result in unexpected behaviour.")]
	[Category("Decorators")]
	public class Merge : BTDecorator
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x0001380D File Offset: 0x00011A0D
		public override int maxInConnections
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00013810 File Offset: 0x00011A10
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.status != Status.Running)
			{
				base.decoratedConnection.Reset(true);
			}
			return base.decoratedConnection.Execute(agent, blackboard);
		}
	}
}

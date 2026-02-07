using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x02000125 RID: 293
	[Name("Override Agent", 0)]
	[Category("Decorators")]
	[Description("Set another Agent for the rest of the Tree dynamicaly from this point and on. All nodes under this will be executed with the new agent. You can also use this decorator to revert back to the original graph agent.")]
	[ParadoxNotion.Design.Icon("Agent", false, "")]
	public class Setter : BTDecorator
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x00013ABE File Offset: 0x00011CBE
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (base.decoratedConnection == null)
			{
				return Status.Optional;
			}
			agent = (this.revertToOriginal ? base.graphAgent : this.newAgent.value.transform);
			return base.decoratedConnection.Execute(agent, blackboard);
		}

		// Token: 0x0400034D RID: 845
		[Tooltip("If enabled, will revert back to the original graph agent.")]
		public bool revertToOriginal;

		// Token: 0x0400034E RID: 846
		[ShowIf("revertToOriginal", 0)]
		[Tooltip("The new agent to use.")]
		public BBParameter<GameObject> newAgent;
	}
}

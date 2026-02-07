using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200012E RID: 302
	[Obsolete]
	[Category("Mutators (beta)")]
	[Name("Root Switcher", 0)]
	[Description("Switch the root node of the behaviour tree to a new one defined by tag\nBeta Feature!")]
	public class RootSwitcher : BTNode
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x000141EF File Offset: 0x000123EF
		public override void OnGraphStarted()
		{
			this.targetNode = base.graph.GetNodeWithTag<Node>(this.targetNodeTag);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00014208 File Offset: 0x00012408
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (string.IsNullOrEmpty(this.targetNodeTag))
			{
				return Status.Failure;
			}
			if (this.targetNode == null)
			{
				return Status.Failure;
			}
			if (base.graph.primeNode != this.targetNode)
			{
				base.graph.primeNode = this.targetNode;
			}
			return Status.Success;
		}

		// Token: 0x0400035C RID: 860
		public string targetNodeTag;

		// Token: 0x0400035D RID: 861
		private Node targetNode;
	}
}

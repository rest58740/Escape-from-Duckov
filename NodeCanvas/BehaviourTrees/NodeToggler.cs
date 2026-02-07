using System;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.BehaviourTrees
{
	// Token: 0x0200012D RID: 301
	[Obsolete]
	[Category("Mutators (beta)")]
	[Name("Node Toggler", 0)]
	[Description("Enable, Disable or Toggle one or more nodes with provided tag. In practise their incomming connections are disabled\nBeta Feature!")]
	public class NodeToggler : BTNode
	{
		// Token: 0x0600065B RID: 1627 RVA: 0x00014082 File Offset: 0x00012282
		public override void OnGraphStarted()
		{
			this.targetNodes = base.graph.GetNodesWithTag<Node>(this.targetNodeTag).ToList<Node>();
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x000140A0 File Offset: 0x000122A0
		protected override Status OnExecute(Component agent, IBlackboard blackboard)
		{
			if (string.IsNullOrEmpty(this.targetNodeTag))
			{
				return Status.Failure;
			}
			if (this.targetNodes.Count == 0)
			{
				return Status.Failure;
			}
			if (this.toggleMode == NodeToggler.ToggleMode.Enable)
			{
				foreach (Node node in this.targetNodes)
				{
					node.inConnections[0].isActive = true;
				}
			}
			if (this.toggleMode == NodeToggler.ToggleMode.Disable)
			{
				foreach (Node node2 in this.targetNodes)
				{
					node2.inConnections[0].isActive = false;
				}
			}
			if (this.toggleMode == NodeToggler.ToggleMode.Toggle)
			{
				foreach (Node node3 in this.targetNodes)
				{
					node3.inConnections[0].isActive = !node3.inConnections[0].isActive;
				}
			}
			return Status.Success;
		}

		// Token: 0x04000359 RID: 857
		public NodeToggler.ToggleMode toggleMode = NodeToggler.ToggleMode.Toggle;

		// Token: 0x0400035A RID: 858
		public string targetNodeTag;

		// Token: 0x0400035B RID: 859
		private List<Node> targetNodes;

		// Token: 0x02000176 RID: 374
		public enum ToggleMode
		{
			// Token: 0x0400043D RID: 1085
			Enable,
			// Token: 0x0400043E RID: 1086
			Disable,
			// Token: 0x0400043F RID: 1087
			Toggle
		}
	}
}

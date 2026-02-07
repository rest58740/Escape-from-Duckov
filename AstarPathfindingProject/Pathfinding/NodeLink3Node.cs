using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000081 RID: 129
	public class NodeLink3Node : PointNode
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x00015781 File Offset: 0x00013981
		public NodeLink3Node(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00015790 File Offset: 0x00013990
		public override bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			left = this.portalA;
			right = this.portalB;
			if (this.connections.Length < 2)
			{
				return false;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			return true;
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000157F0 File Offset: 0x000139F0
		public GraphNode GetOther(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (this.connections.Length != 2)
			{
				throw new Exception("Invalid NodeLink3Node. Expected 2 connections, found " + this.connections.Length.ToString());
			}
			if (a != this.connections[0].node)
			{
				return (this.connections[0].node as NodeLink3Node).GetOtherInternal(this);
			}
			return (this.connections[1].node as NodeLink3Node).GetOtherInternal(this);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00015884 File Offset: 0x00013A84
		private GraphNode GetOtherInternal(GraphNode a)
		{
			if (this.connections.Length < 2)
			{
				return null;
			}
			if (a != this.connections[0].node)
			{
				return this.connections[0].node;
			}
			return this.connections[1].node;
		}

		// Token: 0x040002BE RID: 702
		public NodeLink3 link;

		// Token: 0x040002BF RID: 703
		public Vector3 portalA;

		// Token: 0x040002C0 RID: 704
		public Vector3 portalB;
	}
}

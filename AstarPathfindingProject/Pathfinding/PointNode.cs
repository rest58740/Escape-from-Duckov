using System;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000E5 RID: 229
	public class PointNode : GraphNode
	{
		// Token: 0x06000780 RID: 1920 RVA: 0x000277B2 File Offset: 0x000259B2
		[Obsolete("Set node.position instead")]
		public void SetPosition(Int3 value)
		{
			this.position = value;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00018063 File Offset: 0x00016263
		public PointNode()
		{
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00027ED0 File Offset: 0x000260D0
		public PointNode(AstarPath astar)
		{
			astar.InitializeNode(this);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00017C47 File Offset: 0x00015E47
		public override Vector3 ClosestPointOnNode(Vector3 p)
		{
			return (Vector3)this.position;
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool ContainsPoint(Vector3 point)
		{
			return false;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000185BF File Offset: 0x000167BF
		public override bool ContainsPointInGraphSpace(Int3 point)
		{
			return false;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00027EE0 File Offset: 0x000260E0
		public override void GetConnections<T>(GraphNode.GetConnectionsWithData<T> action, ref T data, int connectionFilter)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (((int)this.connections[i].shapeEdgeInfo & connectionFilter) != 0)
				{
					action(this.connections[i].node, ref data);
				}
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00027F38 File Offset: 0x00026138
		public override void ClearConnections(bool alsoReverse)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemovePartialConnection(this);
				}
			}
			this.connections = null;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00027F94 File Offset: 0x00026194
		public override bool ContainsOutgoingConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return false;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node && this.connections[i].isOutgoing)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00027FE8 File Offset: 0x000261E8
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			if (node == null)
			{
				throw new ArgumentNullException();
			}
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node)
					{
						this.connections[i].cost = cost;
						this.connections[i].shapeEdgeInfo = Connection.PackShapeEdgeInfo(isOutgoing, isIncoming);
						return;
					}
				}
			}
			int num = (this.connections != null) ? this.connections.Length : 0;
			Connection[] array = new Connection[num + 1];
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, isOutgoing, isIncoming);
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			PointGraph pointGraph = base.Graph as PointGraph;
			if (pointGraph != null)
			{
				pointGraph.RegisterConnectionLength((node.position - this.position).sqrMagnitudeLong);
			}
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000280F4 File Offset: 0x000262F4
		public override void RemovePartialConnection(GraphNode node)
		{
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				if (this.connections[i].node == node)
				{
					int num = this.connections.Length;
					Connection[] array = new Connection[num - 1];
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000281AC File Offset: 0x000263AC
		public override void Open(Path path, uint pathNodeIndex, uint gScore)
		{
			path.OpenCandidateConnectionsToEndNode(this.position, pathNodeIndex, pathNodeIndex, gScore);
			if (this.connections == null)
			{
				return;
			}
			for (int i = 0; i < this.connections.Length; i++)
			{
				GraphNode node = this.connections[i].node;
				if (this.connections[i].isOutgoing && path.CanTraverse(this, node))
				{
					if (node is PointNode)
					{
						path.OpenCandidateConnection(pathNodeIndex, node.NodeIndex, gScore, this.connections[i].cost, 0U, node.position);
					}
					else
					{
						node.OpenAtPoint(path, pathNodeIndex, this.position, gScore);
					}
				}
			}
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00028254 File Offset: 0x00026454
		public override void OpenAtPoint(Path path, uint pathNodeIndex, Int3 pos, uint gScore)
		{
			if (path.CanTraverse(this))
			{
				path.OpenCandidateConnectionsToEndNode(pos, pathNodeIndex, pathNodeIndex, gScore);
				uint costMagnitude = (uint)(pos - this.position).costMagnitude;
				path.OpenCandidateConnection(pathNodeIndex, base.NodeIndex, gScore, costMagnitude, 0U, this.position);
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000282A4 File Offset: 0x000264A4
		public override int GetGizmoHashCode()
		{
			int num = base.GetGizmoHashCode();
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					num ^= 17 * this.connections[i].GetHashCode();
				}
			}
			return num;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000282F1 File Offset: 0x000264F1
		public override void SerializeNode(GraphSerializationContext ctx)
		{
			base.SerializeNode(ctx);
			ctx.SerializeInt3(this.position);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00028306 File Offset: 0x00026506
		public override void DeserializeNode(GraphSerializationContext ctx)
		{
			base.DeserializeNode(ctx);
			this.position = ctx.DeserializeInt3();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002831B File Offset: 0x0002651B
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			ctx.SerializeConnections(this.connections, true);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002832A File Offset: 0x0002652A
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			this.connections = ctx.DeserializeConnections(ctx.meta.version >= AstarSerializer.V4_3_85);
		}

		// Token: 0x040004E2 RID: 1250
		public Connection[] connections;

		// Token: 0x040004E3 RID: 1251
		public GameObject gameObject;
	}
}

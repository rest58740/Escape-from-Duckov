using System;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000097 RID: 151
	public abstract class MeshNode : GraphNode
	{
		// Token: 0x060004C3 RID: 1219
		public abstract Int3 GetVertex(int i);

		// Token: 0x060004C4 RID: 1220
		public abstract int GetVertexCount();

		// Token: 0x060004C5 RID: 1221
		public abstract Vector3 ClosestPointOnNodeXZ(Vector3 p);

		// Token: 0x060004C6 RID: 1222 RVA: 0x00017D30 File Offset: 0x00015F30
		public override void ClearConnections(bool alsoReverse = true)
		{
			if (alsoReverse && this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					this.connections[i].node.RemovePartialConnection(this);
				}
			}
			ArrayPool<Connection>.Release(ref this.connections, true);
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00017D90 File Offset: 0x00015F90
		public override void GetConnections<T>(GraphNode.GetConnectionsWithData<T> action, ref T data, int connectionFilter = 32)
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

		// Token: 0x060004C8 RID: 1224 RVA: 0x00017DE8 File Offset: 0x00015FE8
		public override bool ContainsOutgoingConnection(GraphNode node)
		{
			if (this.connections != null)
			{
				for (int i = 0; i < this.connections.Length; i++)
				{
					if (this.connections[i].node == node && this.connections[i].isOutgoing)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00017E3A File Offset: 0x0001603A
		public override void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			this.AddPartialConnection(node, cost, Connection.PackShapeEdgeInfo(isOutgoing, isIncoming));
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00017E4C File Offset: 0x0001604C
		public void AddPartialConnection(GraphNode node, uint cost, byte shapeEdgeInfo)
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
						this.connections[i].shapeEdgeInfo = shapeEdgeInfo;
						return;
					}
				}
			}
			int num = (this.connections != null) ? this.connections.Length : 0;
			Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num + 1);
			for (int j = 0; j < num; j++)
			{
				array[j] = this.connections[j];
			}
			array[num] = new Connection(node, cost, shapeEdgeInfo);
			if (this.connections != null)
			{
				ArrayPool<Connection>.Release(ref this.connections, true);
			}
			this.connections = array;
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00017F2C File Offset: 0x0001612C
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
					Connection[] array = ArrayPool<Connection>.ClaimWithExactLength(num - 1);
					for (int j = 0; j < i; j++)
					{
						array[j] = this.connections[j];
					}
					for (int k = i + 1; k < num; k++)
					{
						array[k - 1] = this.connections[k];
					}
					if (this.connections != null)
					{
						ArrayPool<Connection>.Release(ref this.connections, true);
					}
					this.connections = array;
					AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
					return;
				}
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00017FF8 File Offset: 0x000161F8
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

		// Token: 0x060004CD RID: 1229 RVA: 0x00018045 File Offset: 0x00016245
		public override void SerializeReferences(GraphSerializationContext ctx)
		{
			ctx.SerializeConnections(this.connections, true);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00018054 File Offset: 0x00016254
		public override void DeserializeReferences(GraphSerializationContext ctx)
		{
			this.connections = ctx.DeserializeConnections(true);
		}

		// Token: 0x04000332 RID: 818
		public Connection[] connections;
	}
}

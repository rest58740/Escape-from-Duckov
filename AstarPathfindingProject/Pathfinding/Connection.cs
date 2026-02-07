using System;
using System.Runtime.CompilerServices;

namespace Pathfinding
{
	// Token: 0x02000092 RID: 146
	public struct Connection
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x000177E7 File Offset: 0x000159E7
		public int shapeEdge
		{
			get
			{
				return (int)(this.shapeEdgeInfo & 3);
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x000177F1 File Offset: 0x000159F1
		public int adjacentShapeEdge
		{
			get
			{
				return this.shapeEdgeInfo >> 2 & 3;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x000177FD File Offset: 0x000159FD
		public bool edgesAreIdentical
		{
			get
			{
				return (this.shapeEdgeInfo & 64) > 0;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001780B File Offset: 0x00015A0B
		public bool isEdgeShared
		{
			get
			{
				return (this.shapeEdgeInfo & 15) != 15;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001781D File Offset: 0x00015A1D
		public bool isOutgoing
		{
			get
			{
				return (this.shapeEdgeInfo & 32) > 0;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0001782B File Offset: 0x00015A2B
		public bool isIncoming
		{
			get
			{
				return (this.shapeEdgeInfo & 16) > 0;
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00017839 File Offset: 0x00015A39
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Connection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdgeInfo = Connection.PackShapeEdgeInfo(isOutgoing, isIncoming);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00017857 File Offset: 0x00015A57
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte PackShapeEdgeInfo(bool isOutgoing, bool isIncoming)
		{
			return (byte)(15 | (isIncoming ? 16 : 0) | (isOutgoing ? 32 : 0));
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001786E File Offset: 0x00015A6E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte PackShapeEdgeInfo(byte shapeEdge, byte adjacentShapeEdge, bool areEdgesIdentical, bool isOutgoing, bool isIncoming)
		{
			return (byte)((int)shapeEdge | (int)adjacentShapeEdge << 2 | (areEdgesIdentical ? 64 : 0) | (isOutgoing ? 32 : 0) | (isIncoming ? 16 : 0));
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00017892 File Offset: 0x00015A92
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Connection(GraphNode node, uint cost, byte shapeEdgeInfo)
		{
			this.node = node;
			this.cost = cost;
			this.shapeEdgeInfo = shapeEdgeInfo;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x000178A9 File Offset: 0x00015AA9
		public override int GetHashCode()
		{
			return this.node.GetHashCode() ^ (int)this.cost;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000178C0 File Offset: 0x00015AC0
		public override bool Equals(object obj)
		{
			if (!(obj is Connection))
			{
				return false;
			}
			Connection connection = (Connection)obj;
			return connection.node == this.node && connection.cost == this.cost && connection.shapeEdgeInfo == this.shapeEdgeInfo;
		}

		// Token: 0x04000311 RID: 785
		public GraphNode node;

		// Token: 0x04000312 RID: 786
		public uint cost;

		// Token: 0x04000313 RID: 787
		public byte shapeEdgeInfo;

		// Token: 0x04000314 RID: 788
		public const byte NoSharedEdge = 15;

		// Token: 0x04000315 RID: 789
		public const byte IncomingConnection = 16;

		// Token: 0x04000316 RID: 790
		public const byte OutgoingConnection = 32;

		// Token: 0x04000317 RID: 791
		public const byte IdenticalEdge = 64;
	}
}

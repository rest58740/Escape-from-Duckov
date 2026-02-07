using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Serialization;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000093 RID: 147
	public abstract class GraphNode
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001790A File Offset: 0x00015B0A
		public NavGraph Graph
		{
			get
			{
				return AstarData.GetGraph(this);
			}
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00017912 File Offset: 0x00015B12
		public void Destroy()
		{
			if (this.Destroyed)
			{
				return;
			}
			this.ClearConnections(true);
			if (AstarPath.active != null)
			{
				AstarPath.active.DestroyNode(this);
			}
			this.NodeIndex = 268435454U;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x00017947 File Offset: 0x00015B47
		public bool Destroyed
		{
			[IgnoredByDeepProfiler]
			get
			{
				return this.NodeIndex == 268435454U;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x00017956 File Offset: 0x00015B56
		// (set) Token: 0x06000488 RID: 1160 RVA: 0x00017964 File Offset: 0x00015B64
		public uint NodeIndex
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (uint)(this.nodeIndex & 268435455);
			}
			[IgnoredByDeepProfiler]
			internal set
			{
				this.nodeIndex = ((this.nodeIndex & -268435456) | (int)value);
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000489 RID: 1161 RVA: 0x0001797A File Offset: 0x00015B7A
		internal virtual int PathNodeVariants
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001797D File Offset: 0x00015B7D
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0001798E File Offset: 0x00015B8E
		internal bool TemporaryFlag1
		{
			get
			{
				return (this.nodeIndex & 268435456) != 0;
			}
			set
			{
				this.nodeIndex = ((this.nodeIndex & -268435457) | (value ? 268435456 : 0));
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x000179AE File Offset: 0x00015BAE
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x000179BF File Offset: 0x00015BBF
		internal bool TemporaryFlag2
		{
			get
			{
				return (this.nodeIndex & 536870912) != 0;
			}
			set
			{
				this.nodeIndex = ((this.nodeIndex & -536870913) | (value ? 536870912 : 0));
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x000179DF File Offset: 0x00015BDF
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x000179E7 File Offset: 0x00015BE7
		public uint Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x000179F0 File Offset: 0x00015BF0
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x000179F8 File Offset: 0x00015BF8
		public uint Penalty
		{
			get
			{
				return this.penalty;
			}
			set
			{
				if (value > 16777215U)
				{
					Debug.LogWarning("Very high penalty applied. Are you sure negative values haven't underflowed?\nPenalty values this high could with long paths cause overflows and in some cases infinity loops because of that.\nPenalty value applied: " + value.ToString());
				}
				this.penalty = value;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x00017A1F File Offset: 0x00015C1F
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x00017A2C File Offset: 0x00015C2C
		public bool Walkable
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 1U) > 0U;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = ((this.flags & 4294967294U) | (value ? 1U : 0U));
				AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00017A55 File Offset: 0x00015C55
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x00017A65 File Offset: 0x00015C65
		internal int HierarchicalNodeIndex
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (int)((this.flags & 262142U) >> 1);
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.flags = ((this.flags & 4294705153U) | (uint)((uint)value << 1));
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00017A7D File Offset: 0x00015C7D
		// (set) Token: 0x06000497 RID: 1175 RVA: 0x00017A8E File Offset: 0x00015C8E
		internal bool IsHierarchicalNodeDirty
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.flags & 262144U) > 0U;
			}
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				this.flags = ((this.flags & 4294705151U) | (value ? 1U : 0U) << 18);
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x00017AAD File Offset: 0x00015CAD
		public uint Area
		{
			get
			{
				return AstarPath.active.hierarchicalGraph.GetConnectedComponent(this.HierarchicalNodeIndex);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x00017AC4 File Offset: 0x00015CC4
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x00017AD5 File Offset: 0x00015CD5
		public uint GraphIndex
		{
			[IgnoredByDeepProfiler]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (this.flags & 4278190080U) >> 24;
			}
			set
			{
				this.flags = ((this.flags & 16777215U) | value << 24);
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00017AEE File Offset: 0x00015CEE
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x00017AFF File Offset: 0x00015CFF
		public uint Tag
		{
			get
			{
				return (this.flags & 16252928U) >> 19;
			}
			set
			{
				this.flags = ((this.flags & 4278714367U) | (value << 19 & 16252928U));
			}
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00017B1E File Offset: 0x00015D1E
		public void SetConnectivityDirty()
		{
			AstarPath.active.hierarchicalGraph.AddDirtyNode(this);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00017B30 File Offset: 0x00015D30
		public virtual void GetConnections(Action<GraphNode> action, int connectionFilter = 32)
		{
			this.GetConnections<Action<GraphNode>>(delegate(GraphNode node, ref Action<GraphNode> action)
			{
				action(node);
			}, ref action2, connectionFilter);
		}

		// Token: 0x0600049F RID: 1183
		public abstract void GetConnections<T>(GraphNode.GetConnectionsWithData<T> action, ref T data, int connectionFilter = 32);

		// Token: 0x060004A0 RID: 1184 RVA: 0x00017B5A File Offset: 0x00015D5A
		public static void Connect(GraphNode lhs, GraphNode rhs, uint cost, OffMeshLinks.Directionality directionality = OffMeshLinks.Directionality.TwoWay)
		{
			if (lhs.Destroyed || rhs.Destroyed)
			{
				throw new ArgumentException("Cannot connect destroyed nodes");
			}
			lhs.AddPartialConnection(rhs, cost, true, directionality == OffMeshLinks.Directionality.TwoWay);
			rhs.AddPartialConnection(lhs, cost, directionality == OffMeshLinks.Directionality.TwoWay, true);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00017B91 File Offset: 0x00015D91
		public static void Disconnect(GraphNode lhs, GraphNode rhs)
		{
			lhs.RemovePartialConnection(rhs);
			rhs.RemovePartialConnection(lhs);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00017BA1 File Offset: 0x00015DA1
		[Obsolete("Use the static Connect method instead, or AddPartialConnection if you really need to")]
		public void AddConnection(GraphNode node, uint cost)
		{
			this.AddPartialConnection(node, cost, true, true);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00017BAD File Offset: 0x00015DAD
		[Obsolete("Use the static Disconnect method instead, or RemovePartialConnection if you really need to")]
		public void RemoveConnection(GraphNode node)
		{
			this.RemovePartialConnection(node);
		}

		// Token: 0x060004A4 RID: 1188
		public abstract void AddPartialConnection(GraphNode node, uint cost, bool isOutgoing, bool isIncoming);

		// Token: 0x060004A5 RID: 1189
		public abstract void RemovePartialConnection(GraphNode node);

		// Token: 0x060004A6 RID: 1190
		public abstract void ClearConnections(bool alsoReverse = true);

		// Token: 0x060004A7 RID: 1191 RVA: 0x00017BB6 File Offset: 0x00015DB6
		[Obsolete("Use ContainsOutgoingConnection instead")]
		public bool ContainsConnection(GraphNode node)
		{
			return this.ContainsOutgoingConnection(node);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00017BC0 File Offset: 0x00015DC0
		public virtual bool ContainsOutgoingConnection(GraphNode node)
		{
			bool result = false;
			this.GetConnections<bool>(delegate(GraphNode neighbour, ref bool contains)
			{
				contains |= (neighbour == node);
			}, ref result, 32);
			return result;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00017BF4 File Offset: 0x00015DF4
		[Obsolete("Use GetPortal(GraphNode, out Vector3, out Vector3) instead")]
		public bool GetPortal(GraphNode other, List<Vector3> left, List<Vector3> right, bool backwards)
		{
			Vector3 item;
			Vector3 item2;
			if (!backwards && this.GetPortal(other, out item, out item2))
			{
				if (left != null)
				{
					left.Add(item);
					right.Add(item2);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00017C26 File Offset: 0x00015E26
		public virtual bool GetPortal(GraphNode other, out Vector3 left, out Vector3 right)
		{
			left = Vector3.zero;
			right = Vector3.zero;
			return false;
		}

		// Token: 0x060004AB RID: 1195
		public abstract void Open(Path path, uint pathNodeIndex, uint gScore);

		// Token: 0x060004AC RID: 1196
		public abstract void OpenAtPoint(Path path, uint pathNodeIndex, Int3 position, uint gScore);

		// Token: 0x060004AD RID: 1197 RVA: 0x00017C3F File Offset: 0x00015E3F
		public virtual Int3 DecodeVariantPosition(uint pathNodeIndex, uint fractionAlongEdge)
		{
			return this.position;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000059E1 File Offset: 0x00003BE1
		public virtual float SurfaceArea()
		{
			return 0f;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00017C47 File Offset: 0x00015E47
		public virtual Vector3 RandomPointOnSurface()
		{
			return (Vector3)this.position;
		}

		// Token: 0x060004B0 RID: 1200
		public abstract Vector3 ClosestPointOnNode(Vector3 p);

		// Token: 0x060004B1 RID: 1201 RVA: 0x00017C54 File Offset: 0x00015E54
		public virtual bool ContainsPoint(Int3 point)
		{
			return this.ContainsPoint((Vector3)point);
		}

		// Token: 0x060004B2 RID: 1202
		public abstract bool ContainsPoint(Vector3 point);

		// Token: 0x060004B3 RID: 1203
		public abstract bool ContainsPointInGraphSpace(Int3 point);

		// Token: 0x060004B4 RID: 1204 RVA: 0x00017C62 File Offset: 0x00015E62
		public virtual int GetGizmoHashCode()
		{
			return this.position.GetHashCode() ^ (int)(19U * this.Penalty) ^ (int)(41U * (this.flags & 4294443009U));
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00017C8F File Offset: 0x00015E8F
		public virtual void SerializeNode(GraphSerializationContext ctx)
		{
			ctx.writer.Write(this.Penalty);
			ctx.writer.Write(this.Flags & 4294443009U);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00017CBC File Offset: 0x00015EBC
		public virtual void DeserializeNode(GraphSerializationContext ctx)
		{
			this.Penalty = ctx.reader.ReadUInt32();
			this.Flags = ((ctx.reader.ReadUInt32() & 4294443009U) | (this.Flags & 524286U));
			this.GraphIndex = ctx.graphIndex;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void SerializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x000035CE File Offset: 0x000017CE
		public virtual void DeserializeReferences(GraphSerializationContext ctx)
		{
		}

		// Token: 0x04000318 RID: 792
		private int nodeIndex;

		// Token: 0x04000319 RID: 793
		protected uint flags;

		// Token: 0x0400031A RID: 794
		private uint penalty;

		// Token: 0x0400031B RID: 795
		private const uint NodeIndexMask = 268435455U;

		// Token: 0x0400031C RID: 796
		public const uint DestroyedNodeIndex = 268435454U;

		// Token: 0x0400031D RID: 797
		public const int InvalidNodeIndex = 0;

		// Token: 0x0400031E RID: 798
		private const int TemporaryFlag1Mask = 268435456;

		// Token: 0x0400031F RID: 799
		private const int TemporaryFlag2Mask = 536870912;

		// Token: 0x04000320 RID: 800
		public Int3 position;

		// Token: 0x04000321 RID: 801
		private const int FlagsWalkableOffset = 0;

		// Token: 0x04000322 RID: 802
		private const uint FlagsWalkableMask = 1U;

		// Token: 0x04000323 RID: 803
		private const int FlagsHierarchicalIndexOffset = 1;

		// Token: 0x04000324 RID: 804
		private const uint HierarchicalIndexMask = 262142U;

		// Token: 0x04000325 RID: 805
		private const int HierarchicalDirtyOffset = 18;

		// Token: 0x04000326 RID: 806
		private const uint HierarchicalDirtyMask = 262144U;

		// Token: 0x04000327 RID: 807
		private const int FlagsGraphOffset = 24;

		// Token: 0x04000328 RID: 808
		private const uint FlagsGraphMask = 4278190080U;

		// Token: 0x04000329 RID: 809
		public const uint MaxHierarchicalNodeIndex = 131071U;

		// Token: 0x0400032A RID: 810
		public const uint MaxGraphIndex = 254U;

		// Token: 0x0400032B RID: 811
		public const uint InvalidGraphIndex = 255U;

		// Token: 0x0400032C RID: 812
		private const int FlagsTagOffset = 19;

		// Token: 0x0400032D RID: 813
		public const int MaxTagIndex = 31;

		// Token: 0x0400032E RID: 814
		private const uint FlagsTagMask = 16252928U;

		// Token: 0x02000094 RID: 148
		// (Invoke) Token: 0x060004BB RID: 1211
		public delegate void GetConnectionsWithData<T>(GraphNode node, ref T data);
	}
}

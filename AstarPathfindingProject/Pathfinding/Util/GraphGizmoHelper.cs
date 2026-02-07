using System;
using Pathfinding.Drawing;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding.Util
{
	// Token: 0x02000294 RID: 660
	public class GraphGizmoHelper : IAstarPooledObject, IDisposable
	{
		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x00060BD1 File Offset: 0x0005EDD1
		// (set) Token: 0x06000FCD RID: 4045 RVA: 0x00060BD9 File Offset: 0x0005EDD9
		public DrawingData.Hasher hasher { get; private set; }

		// Token: 0x06000FCE RID: 4046 RVA: 0x00060BE2 File Offset: 0x0005EDE2
		public GraphGizmoHelper()
		{
			this.drawConnection = new Action<GraphNode>(this.DrawConnection);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00060BFC File Offset: 0x0005EDFC
		public static GraphGizmoHelper GetSingleFrameGizmoHelper(DrawingData gizmos, AstarPath active, RedrawScope redrawScope)
		{
			return GraphGizmoHelper.GetGizmoHelper(gizmos, active, DrawingData.Hasher.NotSupplied, redrawScope);
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00060C0B File Offset: 0x0005EE0B
		public static GraphGizmoHelper GetGizmoHelper(DrawingData gizmos, AstarPath active, DrawingData.Hasher hasher, RedrawScope redrawScope)
		{
			GraphGizmoHelper graphGizmoHelper = ObjectPool<GraphGizmoHelper>.Claim();
			graphGizmoHelper.Init(active, hasher, gizmos, redrawScope);
			return graphGizmoHelper;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00060C1C File Offset: 0x0005EE1C
		public void Init(AstarPath active, DrawingData.Hasher hasher, DrawingData gizmos, RedrawScope redrawScope)
		{
			if (active != null)
			{
				this.debugData = active.debugPathData;
				this.debugPathID = active.debugPathID;
				this.debugMode = active.debugMode;
				this.debugFloor = active.debugFloor;
				this.debugRoof = active.debugRoof;
				this.nodeStorage = active.nodeStorage;
				this.showSearchTree = false;
			}
			this.hasher = hasher;
			this.builder = gizmos.GetBuilder(hasher, redrawScope, false);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00060C98 File Offset: 0x0005EE98
		public void OnEnterPool()
		{
			this.builder.Dispose();
			this.debugData = null;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00060CAC File Offset: 0x0005EEAC
		public void DrawConnections(GraphNode node)
		{
			if (!this.showSearchTree)
			{
				this.drawConnectionColor = this.NodeColor(node);
				this.drawConnectionStart = (Vector3)node.position;
				node.GetConnections(this.drawConnection, 32);
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00060CE2 File Offset: 0x0005EEE2
		private void DrawConnection(GraphNode other)
		{
			this.builder.Line(this.drawConnectionStart, ((Vector3)other.position + this.drawConnectionStart) * 0.5f, this.drawConnectionColor);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00060D1C File Offset: 0x0005EF1C
		public Color NodeColor(GraphNode node)
		{
			Color result;
			if (node.Walkable)
			{
				switch (this.debugMode)
				{
				case GraphDebugMode.SolidColor:
					return AstarColor.SolidColor;
				case GraphDebugMode.Penalty:
					return Color.Lerp(AstarColor.ConnectionLowLerp, AstarColor.ConnectionHighLerp, (node.Penalty - this.debugFloor) / (this.debugRoof - this.debugFloor));
				case GraphDebugMode.Areas:
					return AstarColor.GetAreaColor(node.Area);
				case GraphDebugMode.Tags:
					return AstarColor.GetTagColor(node.Tag);
				case GraphDebugMode.HierarchicalNode:
				case GraphDebugMode.NavmeshBorderObstacles:
					return AstarColor.GetTagColor((uint)node.HierarchicalNodeIndex);
				}
				result = AstarColor.SolidColor;
			}
			else
			{
				result = AstarColor.UnwalkableNode;
			}
			return result;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00060DD7 File Offset: 0x0005EFD7
		public void DrawWireTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
		{
			this.builder.Line(a, b, color);
			this.builder.Line(b, c, color);
			this.builder.Line(c, a, color);
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00060E08 File Offset: 0x0005F008
		public void DrawTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			int[] array = ArrayPool<int>.Claim(numTriangles * 3);
			for (int i = 0; i < numTriangles * 3; i++)
			{
				array[i] = i;
			}
			this.builder.SolidMesh(vertices, array, colors, numTriangles * 3, numTriangles * 3);
			ArrayPool<int>.Release(ref array, false);
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00060E4C File Offset: 0x0005F04C
		public void DrawWireTriangles(Vector3[] vertices, Color[] colors, int numTriangles)
		{
			for (int i = 0; i < numTriangles; i++)
			{
				this.DrawWireTriangle(vertices[i * 3], vertices[i * 3 + 1], vertices[i * 3 + 2], colors[i * 3]);
			}
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x00060E94 File Offset: 0x0005F094
		void IDisposable.Dispose()
		{
			GraphGizmoHelper graphGizmoHelper = this;
			ObjectPool<GraphGizmoHelper>.Release(ref graphGizmoHelper);
		}

		// Token: 0x04000B99 RID: 2969
		private PathHandler debugData;

		// Token: 0x04000B9A RID: 2970
		private ushort debugPathID;

		// Token: 0x04000B9B RID: 2971
		private GraphDebugMode debugMode;

		// Token: 0x04000B9C RID: 2972
		public bool showSearchTree;

		// Token: 0x04000B9D RID: 2973
		private float debugFloor;

		// Token: 0x04000B9E RID: 2974
		private float debugRoof;

		// Token: 0x04000B9F RID: 2975
		public CommandBuilder builder;

		// Token: 0x04000BA0 RID: 2976
		private Vector3 drawConnectionStart;

		// Token: 0x04000BA1 RID: 2977
		private Color drawConnectionColor;

		// Token: 0x04000BA2 RID: 2978
		private readonly Action<GraphNode> drawConnection;

		// Token: 0x04000BA3 RID: 2979
		private GlobalNodeStorage nodeStorage;
	}
}

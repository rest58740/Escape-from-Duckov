using System;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000C8 RID: 200
	[Preserve]
	public class LayerGridGraph : GridGraph, IUpdatableGraph
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x00021DF3 File Offset: 0x0001FFF3
		protected override void DisposeUnmanagedData()
		{
			base.DisposeUnmanagedData();
			LevelGridNode.ClearGridGraph((int)this.graphIndex, this);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00021E07 File Offset: 0x00020007
		public LayerGridGraph()
		{
			this.newGridNodeDelegate = (() => new LevelGridNode());
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00021E40 File Offset: 0x00020040
		protected override GridNodeBase[] AllocateNodesJob(int size, out JobHandle dependency)
		{
			LevelGridNode[] array = new LevelGridNode[size];
			AstarPath active = this.active;
			GridNodeBase[] result = array;
			dependency = active.AllocateNodes<GridNodeBase>(result, size, this.newGridNodeDelegate, 1U);
			return array;
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00021E73 File Offset: 0x00020073
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x00021E7B File Offset: 0x0002007B
		public override int LayerCount
		{
			get
			{
				return this.layerCount;
			}
			protected set
			{
				this.layerCount = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00021E84 File Offset: 0x00020084
		public override int MaxLayers
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00021E88 File Offset: 0x00020088
		public override int CountNodes()
		{
			if (this.nodes == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00021EC4 File Offset: 0x000200C4
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] != null)
				{
					action(this.nodes[i]);
				}
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00021F08 File Offset: 0x00020108
		public override int GetNodesInRegion(IntRect rect, GridNodeBase[] buffer)
		{
			IntRect b = new IntRect(0, 0, this.width - 1, this.depth - 1);
			rect = IntRect.Intersection(rect, b);
			if (this.nodes == null || !rect.IsValid() || this.nodes.Length != this.width * this.depth * this.layerCount)
			{
				return 0;
			}
			int num = 0;
			try
			{
				for (int i = 0; i < this.layerCount; i++)
				{
					int num2 = i * base.Width * base.Depth;
					for (int j = rect.ymin; j <= rect.ymax; j++)
					{
						int num3 = num2 + j * base.Width;
						for (int k = rect.xmin; k <= rect.xmax; k++)
						{
							GridNodeBase gridNodeBase = this.nodes[num3 + k];
							if (gridNodeBase != null)
							{
								buffer[num] = gridNodeBase;
								num++;
							}
						}
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				throw new ArgumentException("Buffer is too small");
			}
			return num;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00022008 File Offset: 0x00020208
		public GridNodeBase GetNode(int x, int z, int layer)
		{
			if (x < 0 || z < 0 || x >= this.width || z >= this.depth || layer < 0 || layer >= this.layerCount)
			{
				return null;
			}
			return this.nodes[x + z * this.width + layer * this.width * this.depth];
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002205F File Offset: 0x0002025F
		protected override IGraphUpdatePromise ScanInternal(bool async)
		{
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			this.layerCount = 0;
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			return base.ScanInternal(async);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00022094 File Offset: 0x00020294
		protected override GridNodeBase GetNearestFromGraphSpace(Vector3 positionGraphSpace)
		{
			if (this.nodes == null || this.depth * this.width * this.layerCount != this.nodes.Length)
			{
				return null;
			}
			int x = (int)positionGraphSpace.x;
			float z = positionGraphSpace.z;
			int x2 = Mathf.Clamp(x, 0, this.width - 1);
			int z2 = Mathf.Clamp((int)z, 0, this.depth - 1);
			Vector3 position = base.transform.Transform(positionGraphSpace);
			return this.GetNearestNode(position, x2, z2, null);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00022110 File Offset: 0x00020310
		private GridNodeBase GetNearestNode(Vector3 position, int x, int z, NNConstraint constraint)
		{
			int num = this.width * z + x;
			float num2 = float.PositiveInfinity;
			GridNodeBase result = null;
			for (int i = 0; i < this.layerCount; i++)
			{
				GridNodeBase gridNodeBase = this.nodes[num + this.width * this.depth * i];
				if (gridNodeBase != null)
				{
					float sqrMagnitude = ((Vector3)gridNodeBase.position - position).sqrMagnitude;
					if (sqrMagnitude < num2 && (constraint == null || constraint.Suitable(gridNodeBase)))
					{
						num2 = sqrMagnitude;
						result = gridNodeBase;
					}
				}
			}
			return result;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00022198 File Offset: 0x00020398
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
				return;
			}
			ctx.writer.Write(this.nodes.Length);
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
			base.SerializeNodeSurfaceNormals(ctx);
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0002221C File Offset: 0x0002041C
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			GridNodeBase[] nodes = new LevelGridNode[num];
			this.nodes = nodes;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = this.newGridNodeDelegate();
					this.active.InitializeNode(this.nodes[i]);
					this.nodes[i].DeserializeNode(ctx);
				}
				else
				{
					this.nodes[i] = null;
				}
			}
			base.DeserializeNativeData(ctx, ctx.meta.version >= AstarSerializer.V4_3_37);
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x000222CC File Offset: 0x000204CC
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			base.UpdateTransform();
			this.lastScannedWidth = this.width;
			this.lastScannedDepth = this.depth;
			this.SetUpOffsetsAndCosts();
			LevelGridNode.SetGridGraph((int)this.graphIndex, this);
			if (this.nodes == null || this.nodes.Length == 0)
			{
				return;
			}
			if (this.width * this.depth * this.layerCount != this.nodes.Length)
			{
				Debug.LogError("Node data did not match with bounds data. Probably a change to the bounds/width/depth data was made after scanning the graph, just prior to saving it. Nodes will be discarded");
				this.nodes = new GridNodeBase[0];
				return;
			}
			for (int i = 0; i < this.layerCount; i++)
			{
				for (int j = 0; j < this.depth; j++)
				{
					for (int k = 0; k < this.width; k++)
					{
						LevelGridNode levelGridNode = this.nodes[j * this.width + k + this.width * this.depth * i] as LevelGridNode;
						if (levelGridNode != null)
						{
							levelGridNode.NodeInGridIndex = j * this.width + k;
							levelGridNode.LayerCoordinateInGrid = i;
						}
					}
				}
			}
		}

		// Token: 0x0400045F RID: 1119
		[JsonMember]
		internal int layerCount;

		// Token: 0x04000460 RID: 1120
		[JsonMember]
		public float characterHeight = 0.4f;

		// Token: 0x04000461 RID: 1121
		internal int lastScannedWidth;

		// Token: 0x04000462 RID: 1122
		internal int lastScannedDepth;
	}
}

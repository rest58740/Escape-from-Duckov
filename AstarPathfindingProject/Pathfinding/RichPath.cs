using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200001D RID: 29
	public class RichPath
	{
		// Token: 0x06000196 RID: 406 RVA: 0x00007DF6 File Offset: 0x00005FF6
		public RichPath()
		{
			this.Clear();
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007E0F File Offset: 0x0000600F
		public void Clear()
		{
			this.parts.Clear();
			this.currentPart = 0;
			this.Endpoint = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007E40 File Offset: 0x00006040
		public void Initialize(Seeker seeker, Path path, bool mergePartEndpoints, bool simplificationMode)
		{
			if (path.error)
			{
				throw new ArgumentException("Path has an error");
			}
			List<GraphNode> path2 = path.path;
			if (path2.Count == 0)
			{
				throw new ArgumentException("Path traverses no nodes");
			}
			this.seeker = seeker;
			for (int i = 0; i < this.parts.Count; i++)
			{
				RichFunnel richFunnel = this.parts[i] as RichFunnel;
				RichSpecial richSpecial = this.parts[i] as RichSpecial;
				if (richFunnel != null)
				{
					ObjectPool<RichFunnel>.Release(ref richFunnel);
				}
				else if (richSpecial != null)
				{
					ObjectPool<RichSpecial>.Release(ref richSpecial);
				}
			}
			this.Clear();
			this.Endpoint = path.vectorPath[path.vectorPath.Count - 1];
			for (int j = 0; j < path2.Count; j++)
			{
				if (path2[j] is TriangleMeshNode)
				{
					NavmeshBase navmeshBase = AstarData.GetGraph(path2[j]) as NavmeshBase;
					if (navmeshBase == null)
					{
						throw new Exception("Found a TriangleMeshNode that was not in a NavmeshBase graph");
					}
					RichFunnel richFunnel2 = ObjectPool<RichFunnel>.Claim().Initialize(this, navmeshBase);
					richFunnel2.funnelSimplification = simplificationMode;
					int num = j;
					uint graphIndex = path2[num].GraphIndex;
					while (j < path2.Count && (path2[j].GraphIndex == graphIndex || path2[j] is NodeLink3Node))
					{
						j++;
					}
					j--;
					if (num == 0)
					{
						richFunnel2.exactStart = path.vectorPath[0];
					}
					else
					{
						richFunnel2.exactStart = (Vector3)path2[mergePartEndpoints ? (num - 1) : num].position;
					}
					if (j == path2.Count - 1)
					{
						richFunnel2.exactEnd = path.vectorPath[path.vectorPath.Count - 1];
					}
					else
					{
						richFunnel2.exactEnd = (Vector3)path2[mergePartEndpoints ? (j + 1) : j].position;
					}
					richFunnel2.BuildFunnelCorridor(path2, num, j);
					this.parts.Add(richFunnel2);
				}
				else
				{
					LinkNode linkNode = path2[j] as LinkNode;
					if (linkNode != null)
					{
						int num2 = j;
						uint graphIndex2 = path2[num2].GraphIndex;
						while (j < path2.Count && path2[j].GraphIndex == graphIndex2)
						{
							j++;
						}
						j--;
						if (j - num2 > 1)
						{
							throw new Exception("NodeLink2 path length greater than two (2) nodes. " + (j - num2).ToString());
						}
						if (j - num2 != 0)
						{
							RichSpecial item = ObjectPool<RichSpecial>.Claim().Initialize(linkNode.linkConcrete.GetTracer(linkNode));
							this.parts.Add(item);
						}
					}
					else if (!(path2[j] is PointNode))
					{
						throw new InvalidOperationException("The RichAI movment script can only be used on recast/navmesh graphs. A node of type " + path2[j].GetType().Name + " was in the path.");
					}
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000812E File Offset: 0x0000632E
		// (set) Token: 0x0600019A RID: 410 RVA: 0x00008136 File Offset: 0x00006336
		public Vector3 Endpoint { get; private set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000813F File Offset: 0x0000633F
		public bool CompletedAllParts
		{
			get
			{
				return this.currentPart >= this.parts.Count;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00008157 File Offset: 0x00006357
		public bool IsLastPart
		{
			get
			{
				return this.currentPart >= this.parts.Count - 1;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00008171 File Offset: 0x00006371
		public void NextPart()
		{
			this.currentPart = Mathf.Min(this.currentPart + 1, this.parts.Count);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008194 File Offset: 0x00006394
		public RichPathPart GetCurrentPart()
		{
			if (this.parts.Count == 0)
			{
				return null;
			}
			if (this.currentPart >= this.parts.Count)
			{
				return this.parts[this.parts.Count - 1];
			}
			return this.parts[this.currentPart];
		}

		// Token: 0x0600019F RID: 415 RVA: 0x000081F0 File Offset: 0x000063F0
		public void GetRemainingPath(List<Vector3> buffer, List<PathPartWithLinkInfo> partsBuffer, Vector3 currentPosition, out bool requiresRepath)
		{
			buffer.Clear();
			buffer.Add(currentPosition);
			requiresRepath = false;
			for (int i = this.currentPart; i < this.parts.Count; i++)
			{
				RichPathPart richPathPart = this.parts[i];
				RichFunnel richFunnel = richPathPart as RichFunnel;
				if (richFunnel != null)
				{
					int count = buffer.Count;
					if (i != 0)
					{
						buffer.Add(richFunnel.exactStart);
					}
					bool flag;
					richFunnel.Update((i == 0) ? currentPosition : richFunnel.exactStart, buffer, int.MaxValue, out flag, out requiresRepath);
					if (partsBuffer != null)
					{
						partsBuffer.Add(new PathPartWithLinkInfo(count, buffer.Count - 1, default(OffMeshLinks.OffMeshLinkTracer)));
					}
					if (requiresRepath)
					{
						return;
					}
				}
				else
				{
					RichSpecial richSpecial = richPathPart as RichSpecial;
					if (richSpecial != null && partsBuffer != null)
					{
						partsBuffer.Add(new PathPartWithLinkInfo(buffer.Count - 1, buffer.Count, richSpecial.nodeLink));
					}
				}
			}
		}

		// Token: 0x040000F2 RID: 242
		private int currentPart;

		// Token: 0x040000F3 RID: 243
		private readonly List<RichPathPart> parts = new List<RichPathPart>();

		// Token: 0x040000F4 RID: 244
		public Seeker seeker;

		// Token: 0x040000F5 RID: 245
		public ITransform transform;
	}
}

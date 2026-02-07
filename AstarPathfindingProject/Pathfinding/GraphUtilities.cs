using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000070 RID: 112
	public static class GraphUtilities
	{
		// Token: 0x060003C4 RID: 964 RVA: 0x00012FA4 File Offset: 0x000111A4
		public static List<Vector3> GetContours(NavGraph graph)
		{
			List<Vector3> result = ListPool<Vector3>.Claim();
			NavmeshBase navmeshBase = graph as NavmeshBase;
			if (navmeshBase != null)
			{
				GraphUtilities.GetContours(navmeshBase, delegate(List<Int3> vertices, bool cycle)
				{
					int index = cycle ? (vertices.Count - 1) : 0;
					for (int i = 0; i < vertices.Count; i++)
					{
						result.Add((Vector3)vertices[index]);
						result.Add((Vector3)vertices[i]);
						index = i;
					}
				});
			}
			else if (graph is GridGraph)
			{
				GraphUtilities.GetContours(graph as GridGraph, delegate(Vector3[] vertices)
				{
					int num = vertices.Length - 1;
					for (int i = 0; i < vertices.Length; i++)
					{
						result.Add(vertices[num]);
						result.Add(vertices[i]);
						num = i;
					}
				}, 0f, null, null);
			}
			return result;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0001300C File Offset: 0x0001120C
		public static void GetContours(NavmeshBase navmesh, Action<List<Int3>, bool> results)
		{
			bool[] uses = new bool[3];
			Dictionary<int, int> outline = new Dictionary<int, int>();
			Dictionary<int, Int3> vertexPositions = new Dictionary<int, Int3>();
			HashSet<int> hasInEdge = new HashSet<int>();
			navmesh.GetNodes(delegate(GraphNode _node)
			{
				TriangleMeshNode triangleMeshNode = _node as TriangleMeshNode;
				uses[0] = (uses[1] = (uses[2] = false));
				if (triangleMeshNode != null)
				{
					for (int i = 0; i < triangleMeshNode.connections.Length; i++)
					{
						Connection connection = triangleMeshNode.connections[i];
						if (connection.isEdgeShared)
						{
							uses[connection.shapeEdge] = true;
						}
					}
					for (int j = 0; j < 3; j++)
					{
						if (!uses[j])
						{
							int i2 = j;
							int i3 = (j + 1) % triangleMeshNode.GetVertexCount();
							outline[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertexIndex(i3);
							hasInEdge.Add(triangleMeshNode.GetVertexIndex(i3));
							vertexPositions[triangleMeshNode.GetVertexIndex(i2)] = triangleMeshNode.GetVertex(i2);
							vertexPositions[triangleMeshNode.GetVertexIndex(i3)] = triangleMeshNode.GetVertex(i3);
						}
					}
				}
			});
			Polygon.TraceContours(outline, hasInEdge, delegate(List<int> chain, bool cycle)
			{
				List<Int3> list = ListPool<Int3>.Claim();
				for (int i = 0; i < chain.Count; i++)
				{
					list.Add(vertexPositions[chain[i]]);
				}
				results(list, cycle);
			});
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00013084 File Offset: 0x00011284
		public static void GetContours(GridGraph grid, Action<Vector3[]> callback, float yMergeThreshold, GridNodeBase[] nodes = null, Func<GraphNode, GraphNode, bool> connectionFilter = null)
		{
			HashSet<GridNodeBase> hashSet = (nodes != null) ? new HashSet<GridNodeBase>(nodes) : null;
			LayerGridGraph layerGridGraph = grid as LayerGridGraph;
			if (layerGridGraph != null)
			{
				nodes = (nodes ?? layerGridGraph.nodes);
			}
			nodes = (nodes ?? grid.nodes);
			int[] neighbourXOffsets = GridGraph.neighbourXOffsets;
			int[] neighbourZOffsets = GridGraph.neighbourZOffsets;
			int[] array;
			if (grid.neighbours != NumNeighbours.Six)
			{
				RuntimeHelpers.InitializeArray(array = new int[4], fieldof(<PrivateImplementationDetails>.BAED642339816AFFB3FE8719792D0E4CE82F12DB72B7373D244EAA65445800FE).FieldHandle);
			}
			else
			{
				array = GridGraph.hexagonNeighbourIndices;
			}
			int[] array2 = array;
			float num = (grid.neighbours == NumNeighbours.Six) ? 0.33333334f : 0.5f;
			if (nodes != null)
			{
				List<Vector3> list = ListPool<Vector3>.Claim();
				HashSet<int> hashSet2 = new HashSet<int>();
				foreach (GridNodeBase gridNodeBase in nodes)
				{
					if (gridNodeBase != null && gridNodeBase.Walkable && (!gridNodeBase.HasConnectionsToAllEightNeighbours || hashSet != null))
					{
						for (int j = 0; j < array2.Length; j++)
						{
							int num2 = (int)(gridNodeBase.NodeIndex << 4 | (uint)j);
							GridNodeBase gridNodeBase2 = gridNodeBase.GetNeighbourAlongDirection(array2[j]);
							if (connectionFilter != null && gridNodeBase2 != null && !connectionFilter(gridNodeBase, gridNodeBase2))
							{
								gridNodeBase2 = null;
							}
							if ((gridNodeBase2 == null || (hashSet != null && !hashSet.Contains(gridNodeBase2))) && !hashSet2.Contains(num2))
							{
								list.ClearFast<Vector3>();
								int num3 = j;
								GridNodeBase gridNodeBase3 = gridNodeBase;
								for (;;)
								{
									int num4 = (int)(gridNodeBase3.NodeIndex << 4 | (uint)num3);
									if (num4 == num2 && list.Count > 0)
									{
										break;
									}
									hashSet2.Add(num4);
									GridNodeBase gridNodeBase4 = gridNodeBase3.GetNeighbourAlongDirection(array2[num3]);
									if (connectionFilter != null && gridNodeBase4 != null && !connectionFilter(gridNodeBase3, gridNodeBase4))
									{
										gridNodeBase4 = null;
									}
									if (gridNodeBase4 == null || (hashSet != null && !hashSet.Contains(gridNodeBase4)))
									{
										int num5 = array2[num3];
										num3 = (num3 + 1) % array2.Length;
										int num6 = array2[num3];
										Vector3 vector = new Vector3((float)gridNodeBase3.XCoordinateInGrid + 0.5f, 0f, (float)gridNodeBase3.ZCoordinateInGrid + 0.5f);
										vector.x += (float)(neighbourXOffsets[num5] + neighbourXOffsets[num6]) * num;
										vector.z += (float)(neighbourZOffsets[num5] + neighbourZOffsets[num6]) * num;
										vector.y = grid.transform.InverseTransform((Vector3)gridNodeBase3.position).y;
										if (list.Count >= 2)
										{
											Vector3 b = list[list.Count - 2];
											Vector3 vector2 = list[list.Count - 1] - b;
											Vector3 vector3 = vector - b;
											if (((Mathf.Abs(vector2.x) > 0.01f || Mathf.Abs(vector3.x) > 0.01f) && (Mathf.Abs(vector2.z) > 0.01f || Mathf.Abs(vector3.z) > 0.01f)) || Mathf.Abs(vector2.y) > yMergeThreshold || Mathf.Abs(vector3.y) > yMergeThreshold)
											{
												list.Add(vector);
											}
											else
											{
												list[list.Count - 1] = vector;
											}
										}
										else
										{
											list.Add(vector);
										}
									}
									else
									{
										gridNodeBase3 = gridNodeBase4;
										num3 = (num3 + array2.Length / 2 + 1) % array2.Length;
									}
								}
								if (list.Count >= 3)
								{
									Vector3 b2 = list[list.Count - 2];
									Vector3 vector4 = list[list.Count - 1] - b2;
									Vector3 vector5 = list[0] - b2;
									if (((Mathf.Abs(vector4.x) <= 0.01f && Mathf.Abs(vector5.x) <= 0.01f) || (Mathf.Abs(vector4.z) <= 0.01f && Mathf.Abs(vector5.z) <= 0.01f)) && Mathf.Abs(vector4.y) <= yMergeThreshold && Mathf.Abs(vector5.y) <= yMergeThreshold)
									{
										list.RemoveAt(list.Count - 1);
									}
								}
								if (list.Count >= 3)
								{
									Vector3 b3 = list[list.Count - 1];
									Vector3 vector6 = list[0] - b3;
									Vector3 vector7 = list[1] - b3;
									if (((Mathf.Abs(vector6.x) <= 0.01f && Mathf.Abs(vector7.x) <= 0.01f) || (Mathf.Abs(vector6.z) <= 0.01f && Mathf.Abs(vector7.z) <= 0.01f)) && Mathf.Abs(vector6.y) <= yMergeThreshold && Mathf.Abs(vector7.y) <= yMergeThreshold)
									{
										list.RemoveAt(0);
									}
								}
								Vector3[] array3 = list.ToArray();
								grid.transform.Transform(array3);
								callback(array3);
							}
						}
					}
				}
				ListPool<Vector3>.Release(ref list);
			}
		}
	}
}

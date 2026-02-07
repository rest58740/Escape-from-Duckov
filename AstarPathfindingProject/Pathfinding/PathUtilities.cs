using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200015C RID: 348
	public static class PathUtilities
	{
		// Token: 0x06000A7A RID: 2682 RVA: 0x0003B1F5 File Offset: 0x000393F5
		public static bool IsPathPossible(GraphNode node1, GraphNode node2)
		{
			return node1.Walkable && node2.Walkable && node1.Area == node2.Area;
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0003B218 File Offset: 0x00039418
		public static bool IsPathPossible(List<GraphNode> nodes)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			uint area = nodes[0].Area;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (!nodes[i].Walkable || nodes[i].Area != area)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0003B270 File Offset: 0x00039470
		public static bool IsPathPossible(List<GraphNode> nodes, int tagMask)
		{
			if (nodes.Count == 0)
			{
				return true;
			}
			if ((tagMask >> (int)nodes[0].Tag & 1) == 0)
			{
				return false;
			}
			if (!PathUtilities.IsPathPossible(nodes))
			{
				return false;
			}
			List<GraphNode> reachableNodes = PathUtilities.GetReachableNodes(nodes[0], tagMask, null);
			bool result = true;
			for (int i = 1; i < nodes.Count; i++)
			{
				if (!reachableNodes.Contains(nodes[i]))
				{
					result = false;
					break;
				}
			}
			ListPool<GraphNode>.Release(ref reachableNodes);
			return result;
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0003B2E8 File Offset: 0x000394E8
		public static List<GraphNode> GetReachableNodes(GraphNode seed, int tagMask = -1, Func<GraphNode, bool> filter = null)
		{
			Stack<GraphNode> dfsStack = StackPool<GraphNode>.Claim();
			List<GraphNode> reachable = ListPool<GraphNode>.Claim();
			HashSet<GraphNode> map = new HashSet<GraphNode>();
			Action<GraphNode> action;
			if (tagMask == -1 && filter == null)
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && map.Add(node))
					{
						reachable.Add(node);
						dfsStack.Push(node);
					}
				};
			}
			else
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && map.Add(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						reachable.Add(node);
						dfsStack.Push(node);
					}
				};
			}
			action(seed);
			while (dfsStack.Count > 0)
			{
				dfsStack.Pop().GetConnections(action, 32);
			}
			StackPool<GraphNode>.Release(dfsStack);
			return reachable;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0003B394 File Offset: 0x00039594
		public static List<GraphNode> BFS(GraphNode seed, int depth, int tagMask = -1, Func<GraphNode, bool> filter = null)
		{
			PathUtilities.BFSQueue = (PathUtilities.BFSQueue ?? new Queue<GraphNode>());
			Queue<GraphNode> que = PathUtilities.BFSQueue;
			PathUtilities.BFSMap = (PathUtilities.BFSMap ?? new Dictionary<GraphNode, int>());
			Dictionary<GraphNode, int> map = PathUtilities.BFSMap;
			que.Clear();
			map.Clear();
			List<GraphNode> result = ListPool<GraphNode>.Claim();
			int currentDist = -1;
			Action<GraphNode> action;
			if (tagMask == -1)
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && !map.ContainsKey(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			else
			{
				action = delegate(GraphNode node)
				{
					if (node.Walkable && (tagMask >> (int)node.Tag & 1) != 0 && !map.ContainsKey(node))
					{
						if (filter != null && !filter(node))
						{
							return;
						}
						map.Add(node, currentDist + 1);
						result.Add(node);
						que.Enqueue(node);
					}
				};
			}
			action(seed);
			while (que.Count > 0)
			{
				GraphNode graphNode = que.Dequeue();
				currentDist = map[graphNode];
				if (currentDist >= depth)
				{
					break;
				}
				graphNode.GetConnections(action, 32);
			}
			que.Clear();
			map.Clear();
			return result;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0003B4A4 File Offset: 0x000396A4
		public static List<Vector3> GetSpiralPoints(int count, float clearance)
		{
			List<Vector3> list = ListPool<Vector3>.Claim(count);
			float num = clearance / 6.2831855f;
			float num2 = 0f;
			list.Add(PathUtilities.InvoluteOfCircle(num, num2));
			for (int i = 0; i < count; i++)
			{
				Vector3 b = list[list.Count - 1];
				float num3 = -num2 / 2f + Mathf.Sqrt(num2 * num2 / 4f + 2f * clearance / num);
				float num4 = num2 + num3;
				float num5 = num2 + 2f * num3;
				while (num5 - num4 > 0.01f)
				{
					float num6 = (num4 + num5) / 2f;
					if ((PathUtilities.InvoluteOfCircle(num, num6) - b).sqrMagnitude < clearance * clearance)
					{
						num4 = num6;
					}
					else
					{
						num5 = num6;
					}
				}
				list.Add(PathUtilities.InvoluteOfCircle(num, num5));
				num2 = num5;
			}
			return list;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0003B57E File Offset: 0x0003977E
		private static Vector3 InvoluteOfCircle(float a, float t)
		{
			return new Vector3(a * (Mathf.Cos(t) + t * Mathf.Sin(t)), 0f, a * (Mathf.Sin(t) - t * Mathf.Cos(t)));
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0003B5AC File Offset: 0x000397AC
		public static void GetPointsAroundPointWorld(Vector3 p, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (previousPoints.Count == 0)
			{
				return;
			}
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < previousPoints.Count; i++)
			{
				vector += previousPoints[i];
			}
			vector /= (float)previousPoints.Count;
			for (int j = 0; j < previousPoints.Count; j++)
			{
				int index = j;
				previousPoints[index] -= vector;
			}
			PathUtilities.GetPointsAroundPoint(p, g, previousPoints, radius, clearanceRadius);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0003B62C File Offset: 0x0003982C
		public static void GetPointsAroundPoint(Vector3 center, IRaycastableGraph g, List<Vector3> previousPoints, float radius, float clearanceRadius)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			NavGraph navGraph = g as NavGraph;
			if (navGraph == null)
			{
				throw new ArgumentException("g is not a NavGraph");
			}
			NNInfo nearest = navGraph.GetNearest(center, NNConstraint.Walkable);
			center = nearest.position;
			if (nearest.node == null)
			{
				return;
			}
			radius = Mathf.Max(radius, 1.4142f * clearanceRadius * Mathf.Sqrt((float)previousPoints.Count));
			clearanceRadius *= clearanceRadius;
			int i = 0;
			while (i < previousPoints.Count)
			{
				Vector3 vector = previousPoints[i];
				float magnitude = vector.magnitude;
				if (magnitude > 0f)
				{
					vector /= magnitude;
				}
				float num = radius;
				vector *= num;
				int num2 = 0;
				Vector3 vector2;
				for (;;)
				{
					vector2 = center + vector;
					GraphHitInfo graphHitInfo;
					if (g.Linecast(center, vector2, out graphHitInfo, null, null))
					{
						if (graphHitInfo.point == Vector3.zero)
						{
							num2++;
							if (num2 > 8)
							{
								goto Block_7;
							}
						}
						else
						{
							vector2 = graphHitInfo.point;
						}
					}
					bool flag = false;
					for (float num3 = 0.1f; num3 <= 1f; num3 += 0.05f)
					{
						Vector3 vector3 = Vector3.Lerp(center, vector2, num3);
						flag = true;
						for (int j = 0; j < i; j++)
						{
							if ((previousPoints[j] - vector3).sqrMagnitude < clearanceRadius)
							{
								flag = false;
								break;
							}
						}
						if (flag || num2 > 8)
						{
							flag = true;
							previousPoints[i] = vector3;
							break;
						}
					}
					if (flag)
					{
						break;
					}
					clearanceRadius *= 0.9f;
					vector = UnityEngine.Random.onUnitSphere * Mathf.Lerp(num, radius, (float)(num2 / 5));
					vector.y = 0f;
					num2++;
				}
				IL_194:
				i++;
				continue;
				Block_7:
				previousPoints[i] = vector2;
				goto IL_194;
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0003B7E0 File Offset: 0x000399E0
		public static void FormationPacked(List<Vector3> currentPositions, Vector3 destination, float clearanceRadius, NativeMovementPlane movementPlane)
		{
			NativeArray<float3> positions = new NativeArray<float3>(currentPositions.Count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < positions.Length; i++)
			{
				positions[i] = currentPositions[i];
			}
			new PathUtilities.JobFormationPacked
			{
				positions = positions,
				destination = destination,
				agentRadius = clearanceRadius,
				movementPlane = movementPlane
			}.Schedule(default(JobHandle)).Complete();
			for (int j = 0; j < positions.Length; j++)
			{
				currentPositions[j] = positions[j];
			}
			positions.Dispose();
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0003B89C File Offset: 0x00039A9C
		public static List<Vector3> FormationDestinations(List<IAstarAI> group, Vector3 destination, PathUtilities.FormationMode formationMode, float marginFactor = 0.1f)
		{
			if (group.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = (from u in @group
			select u.position).ToList<Vector3>();
			if (formationMode == PathUtilities.FormationMode.SinglePoint)
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i] = destination;
				}
			}
			else
			{
				Vector3 previousMean = Vector3.zero;
				for (int j = 0; j < list.Count; j++)
				{
					previousMean += list[j];
				}
				previousMean /= (float)list.Count;
				NativeMovementPlane movementPlane = group[0].movementPlane;
				float num = Mathf.Sqrt(list.Average((Vector3 p) => Vector3.SqrMagnitude(p - previousMean))) * 1f;
				if (Vector3.Distance(destination, previousMean) > num)
				{
					PathUtilities.FormationPacked(list, destination, group[0].radius * (1f + marginFactor), movementPlane);
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						list[k] = destination;
					}
				}
			}
			return list;
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0003B9D4 File Offset: 0x00039BD4
		public static void GetPointsAroundPointWorldFlexible(Vector3 center, Quaternion rotation, List<Vector3> positions)
		{
			if (positions.Count == 0)
			{
				return;
			}
			NNInfo nearest = AstarPath.active.GetNearest(center, NNConstraint.Walkable);
			Vector3 groupPos = Vector3.Lerp(nearest.position, (Vector3)nearest.node.position, 0.001f);
			Vector3 vector = Vector3.zero;
			for (int i = 0; i < positions.Count; i++)
			{
				vector += positions[i];
			}
			vector /= (float)positions.Count;
			float maxSqrDistance = 0f;
			for (int j = 0; j < positions.Count; j++)
			{
				int index = j;
				positions[index] -= vector;
				maxSqrDistance = Mathf.Max(maxSqrDistance, positions[j].sqrMagnitude);
			}
			maxSqrDistance *= 4f;
			int minNodes = 10;
			List<GraphNode> collection = PathUtilities.BFS(nearest.node, int.MaxValue, -1, delegate(GraphNode node)
			{
				int minNodes = minNodes;
				minNodes--;
				return minNodes > 0 || ((Vector3)node.position - groupPos).sqrMagnitude < maxSqrDistance;
			});
			NNConstraint constraint = new PathUtilities.ConstrainToSet
			{
				nodes = new HashSet<GraphNode>(collection)
			};
			int num = 3;
			for (int k = 0; k < num; k++)
			{
				float num2 = 0f;
				Vector3 a = Vector3.zero;
				for (int l = 0; l < positions.Count; l++)
				{
					Vector3 b = rotation * positions[l];
					Vector3 vector2 = groupPos + b;
					Vector3 position = AstarPath.active.GetNearest(vector2, constraint).position;
					float num3 = Vector3.Distance(vector2, position);
					a += (position - b) * num3;
					num2 += num3;
				}
				if (num2 <= 1E-07f)
				{
					break;
				}
				Vector3 position2 = a / num2;
				groupPos = AstarPath.active.GetNearest(position2, constraint).position;
			}
			for (int m = 0; m < positions.Count; m++)
			{
				positions[m] = groupPos + rotation * positions[m];
			}
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0003BC0C File Offset: 0x00039E0C
		public static List<Vector3> GetPointsOnNodes(List<GraphNode> nodes, int count, float clearanceRadius = 0f)
		{
			if (nodes == null)
			{
				throw new ArgumentNullException("nodes");
			}
			if (nodes.Count == 0)
			{
				throw new ArgumentException("no nodes passed");
			}
			List<Vector3> list = ListPool<Vector3>.Claim(count);
			clearanceRadius *= clearanceRadius;
			if (clearanceRadius > 0f || nodes[0] is TriangleMeshNode || nodes[0] is GridNode)
			{
				List<float> list2 = ListPool<float>.Claim(nodes.Count);
				float num = 0f;
				for (int i = 0; i < nodes.Count; i++)
				{
					float num2 = nodes[i].SurfaceArea();
					num2 += 0.001f;
					num += num2;
					list2.Add(num);
				}
				for (int j = 0; j < count; j++)
				{
					int num3 = 0;
					int num4 = 10;
					Vector3 vector;
					for (;;)
					{
						bool flag = true;
						if (num3 >= num4)
						{
							clearanceRadius *= 0.80999994f;
							num4 += 10;
							if (num4 > 100)
							{
								clearanceRadius = 0f;
							}
						}
						float item = UnityEngine.Random.value * num;
						int num5 = list2.BinarySearch(item);
						if (num5 < 0)
						{
							num5 = ~num5;
						}
						if (num5 < nodes.Count)
						{
							vector = nodes[num5].RandomPointOnSurface();
							if (clearanceRadius > 0f)
							{
								for (int k = 0; k < list.Count; k++)
								{
									if ((list[k] - vector).sqrMagnitude < clearanceRadius)
									{
										flag = false;
										break;
									}
								}
							}
							if (flag)
							{
								break;
							}
							num3++;
						}
					}
					list.Add(vector);
				}
				ListPool<float>.Release(ref list2);
			}
			else
			{
				for (int l = 0; l < count; l++)
				{
					list.Add(nodes[UnityEngine.Random.Range(0, nodes.Count)].RandomPointOnSurface());
				}
			}
			return list;
		}

		// Token: 0x040006FA RID: 1786
		private static Queue<GraphNode> BFSQueue;

		// Token: 0x040006FB RID: 1787
		private static Dictionary<GraphNode, int> BFSMap;

		// Token: 0x0200015D RID: 349
		[BurstCompile(FloatMode = FloatMode.Fast)]
		private struct JobFormationPacked : IJob
		{
			// Token: 0x06000A87 RID: 2695 RVA: 0x0003BDBC File Offset: 0x00039FBC
			public float CollisionTime(float2 pos1, float2 pos2, float2 v1, float2 v2, float r1, float r2)
			{
				float2 @float = v1 - v2;
				if (math.all(@float == float2.zero))
				{
					return float.MaxValue;
				}
				float num = r1 + r2;
				float2 float2 = pos2 - pos1;
				float2 float3 = math.normalize(@float);
				float num2 = math.dot(float2, float3);
				float num3 = math.lengthsq(float2 - float3 * num2);
				float num4 = num * num - num3;
				if (num4 <= 0f)
				{
					return float.MaxValue;
				}
				float num5 = math.sqrt(num4);
				float num6 = num2 - num5;
				if (num6 < -num)
				{
					return float.MaxValue;
				}
				return num6 * math.rsqrt(math.lengthsq(@float));
			}

			// Token: 0x06000A88 RID: 2696 RVA: 0x0003BE5C File Offset: 0x0003A05C
			public void Execute()
			{
				if (this.positions.Length == 0)
				{
					return;
				}
				NativeArray<float2> nativeArray = new NativeArray<float2>(this.positions.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				NativeArray<int> array = new NativeArray<int>(this.positions.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				for (int i = 0; i < this.positions.Length; i++)
				{
					nativeArray[i] = this.movementPlane.ToPlane(this.positions[i]);
					array[i] = i;
				}
				float2 @float = float2.zero;
				for (int j = 0; j < nativeArray.Length; j++)
				{
					@float += nativeArray[j];
				}
				@float /= (float)nativeArray.Length;
				for (int k = 0; k < nativeArray.Length; k++)
				{
					ref NativeArray<float2> ptr = ref nativeArray;
					int index = k;
					ptr[index] -= @float;
				}
				array.Sort(new PathUtilities.JobFormationPacked.DistanceComparer
				{
					positions = nativeArray
				});
				NativeArray<float> nativeArray2 = new NativeArray<float>(this.positions.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
				for (int l = 0; l < this.positions.Length; l++)
				{
					float num = float.MaxValue;
					int num2 = array[l];
					for (int m = 0; m < l; m++)
					{
						int index2 = array[m];
						float y = this.CollisionTime(nativeArray[num2], nativeArray[index2], -nativeArray[num2], float2.zero, this.agentRadius, this.agentRadius);
						num = math.min(num, y);
					}
					nativeArray2[num2] = num;
					ref NativeArray<float2> ptr = ref nativeArray;
					int index = num2;
					ptr[index] -= nativeArray[num2] * math.min(1f, nativeArray2[array[l]]);
				}
				for (int n = 0; n < this.positions.Length; n++)
				{
					this.positions[n] = this.movementPlane.ToWorld(nativeArray[n], 0f) + this.destination;
				}
			}

			// Token: 0x040006FC RID: 1788
			public NativeArray<float3> positions;

			// Token: 0x040006FD RID: 1789
			public float3 destination;

			// Token: 0x040006FE RID: 1790
			public float agentRadius;

			// Token: 0x040006FF RID: 1791
			public NativeMovementPlane movementPlane;

			// Token: 0x0200015E RID: 350
			private struct DistanceComparer : IComparer<int>
			{
				// Token: 0x06000A89 RID: 2697 RVA: 0x0003C0B1 File Offset: 0x0003A2B1
				public int Compare(int x, int y)
				{
					return (int)math.sign(math.lengthsq(this.positions[x]) - math.lengthsq(this.positions[y]));
				}

				// Token: 0x04000700 RID: 1792
				public NativeArray<float2> positions;
			}
		}

		// Token: 0x0200015F RID: 351
		public enum FormationMode
		{
			// Token: 0x04000702 RID: 1794
			SinglePoint,
			// Token: 0x04000703 RID: 1795
			Packed
		}

		// Token: 0x02000160 RID: 352
		private class ConstrainToSet : NNConstraint
		{
			// Token: 0x06000A8A RID: 2698 RVA: 0x0003C0DC File Offset: 0x0003A2DC
			public override bool Suitable(GraphNode node)
			{
				return this.nodes.Contains(node);
			}

			// Token: 0x04000704 RID: 1796
			public HashSet<GraphNode> nodes;
		}
	}
}

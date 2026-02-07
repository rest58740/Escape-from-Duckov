using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000142 RID: 322
	[BurstCompile]
	public static class Funnel
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x00035184 File Offset: 0x00033384
		public static List<Funnel.PathPart> SplitIntoParts(Path path)
		{
			List<GraphNode> path2 = path.path;
			List<Funnel.PathPart> list = ListPool<Funnel.PathPart>.Claim();
			if (path2 == null || path2.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < path2.Count; i++)
			{
				GraphNode graphNode = path2[i];
				if (graphNode is TriangleMeshNode || graphNode is GridNodeBase)
				{
					int num = i;
					uint graphIndex = graphNode.GraphIndex;
					while (i < path2.Count && (path2[i].GraphIndex == graphIndex || path2[i] is NodeLink3Node))
					{
						i++;
					}
					i--;
					int num2 = i;
					List<Funnel.PathPart> list2 = list;
					Funnel.PathPart item = new Funnel.PathPart
					{
						type = Funnel.PartType.NodeSequence,
						startIndex = num,
						endIndex = num2,
						startPoint = ((num == 0) ? path.vectorPath[0] : ((Vector3)path2[num - 1].position)),
						endPoint = ((num2 == path2.Count - 1) ? path.vectorPath[path.vectorPath.Count - 1] : ((Vector3)path2[num2 + 1].position))
					};
					list2.Add(item);
				}
				else
				{
					if (!(graphNode is LinkNode))
					{
						throw new Exception("Unsupported node type or null node");
					}
					int num3 = i;
					uint graphIndex2 = graphNode.GraphIndex;
					while (i < path2.Count && path2[i].GraphIndex == graphIndex2)
					{
						i++;
					}
					i--;
					if (i - num3 == 0)
					{
						if (num3 > 0 && num3 + 1 < path2.Count && path2[num3 - 1] == path2[num3 + 1])
						{
							path2.RemoveRange(num3, 2);
							i--;
							throw new Exception("Link node connected back to the previous node in the path. This should not happen.");
						}
						path2.RemoveAt(num3);
						i--;
					}
					else
					{
						if (i - num3 != 1)
						{
							throw new Exception("Off mesh link included more than two nodes: " + (i - num3 + 1).ToString());
						}
						List<Funnel.PathPart> list3 = list;
						Funnel.PathPart item = new Funnel.PathPart
						{
							type = Funnel.PartType.OffMeshLink,
							startIndex = num3,
							endIndex = i,
							startPoint = (Vector3)path2[num3].position,
							endPoint = (Vector3)path2[i].position
						};
						list3.Add(item);
					}
				}
			}
			if (list[0].type == Funnel.PartType.OffMeshLink)
			{
				list.RemoveAt(0);
			}
			if (list[list.Count - 1].type == Funnel.PartType.OffMeshLink)
			{
				list.RemoveAt(list.Count - 1);
			}
			return list;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00035410 File Offset: 0x00033610
		public static void Simplify(List<Funnel.PathPart> parts, ref List<GraphNode> nodes, Func<GraphNode, bool> filter = null)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			int i = 0;
			while (i < parts.Count)
			{
				Funnel.PathPart pathPart = parts[i];
				Funnel.PathPart value = pathPart;
				value.startIndex = list.Count;
				if (pathPart.type != Funnel.PartType.NodeSequence)
				{
					goto IL_75;
				}
				IRaycastableGraph raycastableGraph = nodes[pathPart.startIndex].Graph as IRaycastableGraph;
				if (raycastableGraph == null)
				{
					goto IL_75;
				}
				Funnel.Simplify(pathPart, raycastableGraph, nodes, list, Path.ZeroTagPenalties, -1, filter);
				value.endIndex = list.Count - 1;
				parts[i] = value;
				IL_B5:
				i++;
				continue;
				IL_75:
				for (int j = pathPart.startIndex; j <= pathPart.endIndex; j++)
				{
					list.Add(nodes[j]);
				}
				value.endIndex = list.Count - 1;
				parts[i] = value;
				goto IL_B5;
			}
			ListPool<GraphNode>.Release(ref nodes);
			nodes = list;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x000354EC File Offset: 0x000336EC
		public static void Simplify(Funnel.PathPart part, IRaycastableGraph graph, List<GraphNode> nodes, List<GraphNode> result, int[] tagPenalties, int traversableTags, Func<GraphNode, bool> filter = null)
		{
			int num = part.startIndex;
			int endIndex = part.endIndex;
			Vector3 startPoint = part.startPoint;
			Vector3 endPoint = part.endPoint;
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (num > endIndex)
			{
				throw new ArgumentException("start > end");
			}
			GraphHitInfo graphHitInfo;
			if (!graph.Linecast(startPoint, endPoint, out graphHitInfo, null, null) && graphHitInfo.node == nodes[endIndex])
			{
				graph.Linecast(startPoint, endPoint, out graphHitInfo, result, null);
				long num2 = 0L;
				long num3 = 0L;
				for (int i = num; i <= endIndex; i++)
				{
					num2 += (long)((ulong)nodes[i].Penalty + (ulong)((long)tagPenalties[(int)nodes[i].Tag]));
				}
				bool flag = true;
				for (int j = 0; j < result.Count; j++)
				{
					num3 += (long)((ulong)result[j].Penalty + (ulong)((long)tagPenalties[(int)result[j].Tag]));
					flag &= ((traversableTags >> (int)result[j].Tag & 1) == 1);
					flag &= (filter == null || filter(result[j]));
				}
				if (flag && (double)num2 * 1.4 * (double)result.Count >= (double)(num3 * (long)(endIndex - num + 1)))
				{
					return;
				}
				result.Clear();
			}
			int num4 = num;
			int num5 = 0;
			while (num5++ <= 1000)
			{
				if (num == endIndex)
				{
					result.Add(nodes[endIndex]);
					Funnel.RemoveBacktracking(result, num4, result.Count - 2);
					return;
				}
				int count = result.Count;
				int k = endIndex + 1;
				int num6 = num + 1;
				bool flag2 = false;
				while (k > num6 + 1)
				{
					int num7 = (k + num6) / 2;
					Vector3 start = (num == num4) ? startPoint : ((Vector3)nodes[num].position);
					Vector3 end = (num7 == endIndex) ? endPoint : ((Vector3)nodes[num7].position);
					GraphHitInfo graphHitInfo2;
					if (graph.Linecast(start, end, out graphHitInfo2, null, null) || graphHitInfo2.node != nodes[num7])
					{
						k = num7;
					}
					else
					{
						flag2 = true;
						num6 = num7;
					}
				}
				if (!flag2)
				{
					result.Add(nodes[num]);
					Funnel.RemoveBacktracking(result, num4, result.Count - 2);
					num = num6;
				}
				else
				{
					Vector3 start2 = (num == num4) ? startPoint : ((Vector3)nodes[num].position);
					Vector3 end2 = (num6 == endIndex) ? endPoint : ((Vector3)nodes[num6].position);
					GraphHitInfo graphHitInfo3;
					graph.Linecast(start2, end2, out graphHitInfo3, result, null);
					long num8 = 0L;
					long num9 = 0L;
					for (int l = num; l <= num6; l++)
					{
						num8 += (long)((ulong)nodes[l].Penalty + (ulong)((long)tagPenalties[(int)nodes[l].Tag]));
					}
					bool flag3 = true;
					for (int m = count; m < result.Count; m++)
					{
						num9 += (long)((ulong)result[m].Penalty + (ulong)((long)tagPenalties[(int)result[m].Tag]));
						flag3 &= ((traversableTags >> (int)result[m].Tag & 1) == 1);
					}
					if (!flag3 || (double)num8 * 1.4 * (double)(result.Count - count) < (double)(num9 * (long)(num6 - num + 1)) || result[result.Count - 1] != nodes[num6])
					{
						result.RemoveRange(count, result.Count - count);
						result.Add(nodes[num]);
						num++;
					}
					else
					{
						Funnel.RemoveBacktracking(result, num4, count);
						result.RemoveAt(result.Count - 1);
						num = num6;
					}
				}
			}
			Debug.LogError("Was the path really long or have we got cought in an infinite loop?");
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000358A0 File Offset: 0x00033AA0
		private static void RemoveBacktracking(List<GraphNode> nodes, int listStartIndex, int aroundIndex)
		{
			while (aroundIndex - 1 > listStartIndex && aroundIndex + 1 < nodes.Count && nodes[aroundIndex - 1] == nodes[aroundIndex + 1])
			{
				nodes.RemoveRange(aroundIndex, 2);
				aroundIndex--;
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x000358D8 File Offset: 0x00033AD8
		public static Funnel.FunnelPortals ConstructFunnelPortals(List<GraphNode> nodes, Funnel.PathPart part)
		{
			if (nodes == null || nodes.Count == 0)
			{
				return new Funnel.FunnelPortals
				{
					left = ListPool<Vector3>.Claim(0),
					right = ListPool<Vector3>.Claim(0)
				};
			}
			if (part.endIndex < part.startIndex || part.startIndex < 0 || part.endIndex > nodes.Count)
			{
				throw new ArgumentOutOfRangeException();
			}
			List<Vector3> list = ListPool<Vector3>.Claim(nodes.Count + 1);
			List<Vector3> list2 = ListPool<Vector3>.Claim(nodes.Count + 1);
			list.Add(part.startPoint);
			list2.Add(part.startPoint);
			for (int i = part.startIndex; i < part.endIndex; i++)
			{
				Vector3 item;
				Vector3 item2;
				if (nodes[i].GetPortal(nodes[i + 1], out item, out item2))
				{
					list.Add(item);
					list2.Add(item2);
				}
				else
				{
					list.Add((Vector3)nodes[i].position);
					list2.Add((Vector3)nodes[i].position);
					list.Add((Vector3)nodes[i + 1].position);
					list2.Add((Vector3)nodes[i + 1].position);
				}
			}
			list.Add(part.endPoint);
			list2.Add(part.endPoint);
			return new Funnel.FunnelPortals
			{
				left = list,
				right = list2
			};
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00035A50 File Offset: 0x00033C50
		private static float2 Unwrap(float3 leftPortal, float3 rightPortal, float2 leftUnwrappedPortal, float2 rightUnwrappedPortal, float3 point, float sideMultiplier, float3 projectionAxis)
		{
			if (!math.all(projectionAxis == 0f))
			{
				leftPortal -= projectionAxis * math.dot(leftPortal, projectionAxis);
				rightPortal -= projectionAxis * math.dot(rightPortal, projectionAxis);
				point -= projectionAxis * math.dot(point, projectionAxis);
			}
			float3 @float = rightPortal - leftPortal;
			float num = 1f / math.lengthsq(@float);
			if (float.IsPositiveInfinity(num))
			{
				return leftUnwrappedPortal + new float2(-math.length(point - leftPortal), 0f);
			}
			float num2 = math.length(math.cross(point - leftPortal, @float)) * num;
			float num3 = math.dot(point - leftPortal, @float) * num;
			if (num2 < 0.002f)
			{
				if (math.abs(num3) < 0.002f)
				{
					return leftUnwrappedPortal;
				}
				if (math.abs(num3 - 1f) < 0.002f)
				{
					return rightUnwrappedPortal;
				}
			}
			float2 float2 = rightUnwrappedPortal - leftUnwrappedPortal;
			float2 lhs = new float2(-float2.y, float2.x);
			return leftUnwrappedPortal + math.mad(float2, num3, lhs * (num2 * sideMultiplier));
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00035B83 File Offset: 0x00033D83
		private static bool RightOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y <= 0f;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00035BAA File Offset: 0x00033DAA
		private static bool LeftOrColinear(Vector2 a, Vector2 b)
		{
			return a.x * b.y - b.x * a.y >= 0f;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00035BD4 File Offset: 0x00033DD4
		public unsafe static List<Vector3> Calculate(Funnel.FunnelPortals funnel, bool splitAtEveryPortal)
		{
			Funnel.FunnelState funnelState = new Funnel.FunnelState(funnel, Allocator.Temp);
			float3 startPoint = *funnelState.leftFunnel.First;
			float3 endPoint = *funnelState.leftFunnel.Last;
			funnelState.PopStart();
			funnelState.PopEnd();
			NativeList<float3> result = new NativeList<float3>(Allocator.Temp);
			funnelState.CalculateNextCorners(int.MaxValue, splitAtEveryPortal, startPoint, endPoint, result);
			funnelState.Dispose();
			List<Vector3> list = ListPool<Vector3>.Claim(result.Length);
			for (int i = 0; i < result.Length; i++)
			{
				list.Add(result[i]);
			}
			result.Dispose();
			return list;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00035C84 File Offset: 0x00033E84
		[BurstCompile]
		private static int Calculate(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
		{
			return Funnel.Calculate_00000954$BurstDirectCall.Invoke(ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, out lastCorner);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00035CA4 File Offset: 0x00033EA4
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static int Calculate$BurstManaged(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
		{
			lastCorner = false;
			if (leftPortals.Length <= 0)
			{
				lastCorner = true;
				return 0;
			}
			if (maxCorners <= 0)
			{
				return 0;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (unwrappedPortals.Length == 0)
			{
				unwrappedPortals.PushEnd(new float4(new float2(0f, 0f), new float2(math.length(rightPortals[0] - leftPortals[0]))));
			}
			float2 rhs = Funnel.Unwrap(leftPortals[0], rightPortals[0], unwrappedPortals[0].xy, unwrappedPortals[0].zw, startPoint, -1f, projectionAxis);
			float2 v = float2.zero;
			float2 v2 = float2.zero;
			int i = 0;
			while (i <= leftPortals.Length)
			{
				float2 float2;
				float2 @float;
				if (i == unwrappedPortals.Length)
				{
					if (i == leftPortals.Length)
					{
						@float = (float2 = Funnel.Unwrap(leftPortals[i - 1], rightPortals[i - 1], unwrappedPortals[i - 1].xy, unwrappedPortals[i - 1].zw, endPoint, 1f, projectionAxis) - rhs);
					}
					else
					{
						float2 float3 = Funnel.Unwrap(leftPortals[i - 1], rightPortals[i - 1], unwrappedPortals[i - 1].xy, unwrappedPortals[i - 1].zw, leftPortals[i], 1f, projectionAxis);
						float2 float4 = Funnel.Unwrap(leftPortals[i], rightPortals[i - 1], float3, unwrappedPortals[i - 1].zw, rightPortals[i], 1f, projectionAxis);
						unwrappedPortals.PushEnd(new float4(float3, float4));
						float2 = float3 - rhs;
						@float = float4 - rhs;
					}
				}
				else
				{
					float2 = unwrappedPortals[i].xy - rhs;
					@float = unwrappedPortals[i].zw - rhs;
				}
				if (!Funnel.LeftOrColinear(v2, @float))
				{
					goto IL_296;
				}
				if (Funnel.RightOrColinear(v, @float))
				{
					v2 = @float;
					num = i;
					goto IL_296;
				}
				v = (v2 = float2.zero);
				int num4 = i = (num = num2);
				rhs = unwrappedPortals[i].xy;
				*funnelPath[num3++] = num4;
				if (num3 >= maxCorners)
				{
					return num3;
				}
				IL_308:
				i++;
				continue;
				IL_296:
				if (!Funnel.RightOrColinear(v, float2))
				{
					goto IL_308;
				}
				if (Funnel.LeftOrColinear(v2, float2))
				{
					v = float2;
					num2 = i;
					goto IL_308;
				}
				v = (v2 = float2.zero);
				num4 = (i = (num2 = num));
				rhs = unwrappedPortals[i].zw;
				*funnelPath[num3++] = (num4 | 1073741824);
				if (num3 >= maxCorners)
				{
					return num3;
				}
				goto IL_308;
			}
			lastCorner = true;
			return num3;
		}

		// Token: 0x040006A5 RID: 1701
		public const int RightSideBit = 1073741824;

		// Token: 0x040006A6 RID: 1702
		public const int FunnelPortalIndexMask = 1073741823;

		// Token: 0x02000143 RID: 323
		public struct FunnelPortals
		{
			// Token: 0x040006A7 RID: 1703
			public List<Vector3> left;

			// Token: 0x040006A8 RID: 1704
			public List<Vector3> right;
		}

		// Token: 0x02000144 RID: 324
		public enum PartType
		{
			// Token: 0x040006AA RID: 1706
			OffMeshLink,
			// Token: 0x040006AB RID: 1707
			NodeSequence
		}

		// Token: 0x02000145 RID: 325
		public struct PathPart
		{
			// Token: 0x040006AC RID: 1708
			public int startIndex;

			// Token: 0x040006AD RID: 1709
			public int endIndex;

			// Token: 0x040006AE RID: 1710
			public Vector3 startPoint;

			// Token: 0x040006AF RID: 1711
			public Vector3 endPoint;

			// Token: 0x040006B0 RID: 1712
			public Funnel.PartType type;
		}

		// Token: 0x02000146 RID: 326
		[BurstCompile]
		public struct FunnelState
		{
			// Token: 0x060009CF RID: 2511 RVA: 0x00035FD4 File Offset: 0x000341D4
			public FunnelState(int initialCapacity, Allocator allocator)
			{
				this.leftFunnel = new NativeCircularBuffer<float3>(initialCapacity, allocator);
				this.rightFunnel = new NativeCircularBuffer<float3>(initialCapacity, allocator);
				this.unwrappedPortals = new NativeCircularBuffer<float4>(initialCapacity, allocator);
				this.projectionAxis = float3.zero;
			}

			// Token: 0x060009D0 RID: 2512 RVA: 0x00036024 File Offset: 0x00034224
			public FunnelState(Funnel.FunnelPortals portals, Allocator allocator)
			{
				this = new Funnel.FunnelState(portals.left.Count, allocator);
				if (portals.left.Count != portals.right.Count)
				{
					throw new ArgumentException("portals.left.Count != portals.right.Count");
				}
				for (int i = 0; i < portals.left.Count; i++)
				{
					this.PushEnd(portals.left[i], portals.right[i]);
				}
			}

			// Token: 0x060009D1 RID: 2513 RVA: 0x0003609C File Offset: 0x0003429C
			public Funnel.FunnelState Clone()
			{
				return new Funnel.FunnelState
				{
					leftFunnel = this.leftFunnel.Clone(),
					rightFunnel = this.rightFunnel.Clone(),
					unwrappedPortals = this.unwrappedPortals.Clone(),
					projectionAxis = this.projectionAxis
				};
			}

			// Token: 0x060009D2 RID: 2514 RVA: 0x000360F5 File Offset: 0x000342F5
			public void Clear()
			{
				this.leftFunnel.Clear();
				this.rightFunnel.Clear();
				this.unwrappedPortals.Clear();
				this.projectionAxis = float3.zero;
			}

			// Token: 0x060009D3 RID: 2515 RVA: 0x00036123 File Offset: 0x00034323
			public void PopStart()
			{
				this.leftFunnel.PopStart();
				this.rightFunnel.PopStart();
				if (this.unwrappedPortals.Length > 0)
				{
					this.unwrappedPortals.PopStart();
				}
			}

			// Token: 0x060009D4 RID: 2516 RVA: 0x00036157 File Offset: 0x00034357
			public void PopEnd()
			{
				this.leftFunnel.PopEnd();
				this.rightFunnel.PopEnd();
				this.unwrappedPortals.TrimTo(this.leftFunnel.Length);
			}

			// Token: 0x060009D5 RID: 2517 RVA: 0x00036187 File Offset: 0x00034387
			public void Pop(bool fromStart)
			{
				if (fromStart)
				{
					this.PopStart();
					return;
				}
				this.PopEnd();
			}

			// Token: 0x060009D6 RID: 2518 RVA: 0x00036199 File Offset: 0x00034399
			public void PushStart(float3 newLeftPortal, float3 newRightPortal)
			{
				Funnel.FunnelState.PushStart(ref this.leftFunnel, ref this.rightFunnel, ref this.unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref this.projectionAxis);
			}

			// Token: 0x060009D7 RID: 2519 RVA: 0x000361BC File Offset: 0x000343BC
			private static bool DifferentSidesOfLine(float3 start, float3 end, float3 a, float3 b)
			{
				float3 @float = math.normalizesafe(end - start, default(float3));
				float3 float2 = a - start;
				float3 float3 = b - start;
				float2 -= @float * math.dot(float2, @float);
				float3 -= @float * math.dot(float3, @float);
				return math.dot(float2, float3) < 0f;
			}

			// Token: 0x060009D8 RID: 2520 RVA: 0x00036228 File Offset: 0x00034428
			public unsafe bool IsReasonableToPopStart(float3 startPoint, float3 endPoint)
			{
				if (this.leftFunnel.Length == 0)
				{
					return false;
				}
				int num = 1;
				while (num < this.leftFunnel.Length && VectorMath.IsColinear(*this.leftFunnel.First, *this.rightFunnel.First, this.leftFunnel[num]))
				{
					num++;
				}
				return !Funnel.FunnelState.DifferentSidesOfLine(*this.leftFunnel.First, *this.rightFunnel.First, startPoint, (num < this.leftFunnel.Length) ? this.leftFunnel[num] : endPoint);
			}

			// Token: 0x060009D9 RID: 2521 RVA: 0x000362E4 File Offset: 0x000344E4
			public unsafe bool IsReasonableToPopEnd(float3 startPoint, float3 endPoint)
			{
				if (this.leftFunnel.Length == 0)
				{
					return false;
				}
				int num = this.leftFunnel.Length - 1;
				while (num >= 0 && VectorMath.IsColinear(*this.leftFunnel.Last, *this.rightFunnel.Last, this.leftFunnel[num]))
				{
					num--;
				}
				return !Funnel.FunnelState.DifferentSidesOfLine(*this.leftFunnel.Last, *this.rightFunnel.Last, endPoint, (num >= 0) ? this.leftFunnel[num] : startPoint);
			}

			// Token: 0x060009DA RID: 2522 RVA: 0x00036396 File Offset: 0x00034596
			[BurstCompile]
			private static void PushStart(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
			{
				Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Invoke(ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis);
			}

			// Token: 0x060009DB RID: 2523 RVA: 0x000363A5 File Offset: 0x000345A5
			public void Splice(int startIndex, int toRemove, List<float3> newLeftPortal, List<float3> newRightPortal)
			{
				this.leftFunnel.Splice(startIndex, toRemove, newLeftPortal);
				this.rightFunnel.Splice(startIndex, toRemove, newRightPortal);
				this.unwrappedPortals.TrimTo(startIndex);
			}

			// Token: 0x060009DC RID: 2524 RVA: 0x000363D0 File Offset: 0x000345D0
			public void PushEnd(Vector3 newLeftPortal, Vector3 newRightPortal)
			{
				this.leftFunnel.PushEnd(newLeftPortal);
				this.rightFunnel.PushEnd(newRightPortal);
			}

			// Token: 0x060009DD RID: 2525 RVA: 0x000363F4 File Offset: 0x000345F4
			public void Push(bool toStart, Vector3 newLeftPortal, Vector3 newRightPortal)
			{
				if (toStart)
				{
					this.PushStart(newLeftPortal, newRightPortal);
					return;
				}
				this.PushEnd(newLeftPortal, newRightPortal);
			}

			// Token: 0x060009DE RID: 2526 RVA: 0x00036414 File Offset: 0x00034614
			public void Dispose()
			{
				this.leftFunnel.Dispose();
				this.rightFunnel.Dispose();
				this.unwrappedPortals.Dispose();
			}

			// Token: 0x060009DF RID: 2527 RVA: 0x00036438 File Offset: 0x00034638
			public int CalculateNextCornerIndices(int maxCorners, NativeArray<int> result, float3 startPoint, float3 endPoint, out bool lastCorner)
			{
				if (result.Length < math.min(maxCorners, this.leftFunnel.Length))
				{
					throw new ArgumentException("result array may not be large enough to hold all corners");
				}
				UnsafeSpan<int> unsafeSpan = result.AsUnsafeSpan<int>();
				return Funnel.Calculate(ref this.unwrappedPortals, ref this.leftFunnel, ref this.rightFunnel, ref startPoint, ref endPoint, ref unsafeSpan, maxCorners, ref this.projectionAxis, out lastCorner);
			}

			// Token: 0x060009E0 RID: 2528 RVA: 0x00036498 File Offset: 0x00034698
			public void CalculateNextCorners(int maxCorners, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, NativeList<float3> result)
			{
				NativeArray<int> nativeArray = new NativeArray<int>(math.min(maxCorners, this.leftFunnel.Length), Allocator.Temp, NativeArrayOptions.ClearMemory);
				bool lastCorner;
				int numCorners = this.CalculateNextCornerIndices(maxCorners, nativeArray, startPoint, endPoint, out lastCorner);
				this.ConvertCornerIndicesToPath(nativeArray, numCorners, splitAtEveryPortal, startPoint, endPoint, lastCorner, result);
				nativeArray.Dispose();
			}

			// Token: 0x060009E1 RID: 2529 RVA: 0x000364E4 File Offset: 0x000346E4
			public unsafe void ConvertCornerIndicesToPath(NativeArray<int> indices, int numCorners, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, bool lastCorner, NativeList<float3> result)
			{
				if (result.Capacity < numCorners)
				{
					result.Capacity = numCorners;
				}
				result.Add(startPoint);
				if (this.leftFunnel.Length == 0)
				{
					if (lastCorner)
					{
						result.Add(endPoint);
					}
					return;
				}
				if (splitAtEveryPortal)
				{
					float2 from = Funnel.Unwrap(this.leftFunnel[0], this.rightFunnel[0], this.unwrappedPortals[0].xy, this.unwrappedPortals[0].zw, startPoint, -1f, this.projectionAxis);
					int num = 0;
					for (int i = 0; i < numCorners; i++)
					{
						int num2 = indices[i] & 1073741823;
						bool flag = (indices[i] & 1073741824) != 0;
						float2 @float = flag ? this.unwrappedPortals[num2].zw : this.unwrappedPortals[num2].xy;
						Funnel.FunnelState.CalculatePortalIntersections(num + 1, num2 - 1, this.leftFunnel, this.rightFunnel, this.unwrappedPortals, from, @float, result);
						num = math.abs(num2);
						from = @float;
						float3 float2 = flag ? this.rightFunnel[num2] : this.leftFunnel[num2];
						result.Add(float2);
					}
					if (lastCorner)
					{
						float2 to = Funnel.Unwrap(*this.leftFunnel.Last, *this.rightFunnel.Last, this.unwrappedPortals.Last.xy, this.unwrappedPortals.Last.zw, endPoint, 1f, this.projectionAxis);
						Funnel.FunnelState.CalculatePortalIntersections(num + 1, this.unwrappedPortals.Length - 1, this.leftFunnel, this.rightFunnel, this.unwrappedPortals, from, to, result);
						result.Add(endPoint);
						return;
					}
				}
				else
				{
					for (int j = 0; j < numCorners; j++)
					{
						int num3 = indices[j];
						float3 float2 = ((num3 & 1073741824) != 0) ? this.rightFunnel[num3 & 1073741823] : this.leftFunnel[num3 & 1073741823];
						result.Add(float2);
					}
					if (lastCorner)
					{
						result.Add(endPoint);
					}
				}
			}

			// Token: 0x060009E2 RID: 2530 RVA: 0x00036738 File Offset: 0x00034938
			public void ConvertCornerIndicesToPathProjected(UnsafeSpan<int> indices, bool splitAtEveryPortal, float3 startPoint, float3 endPoint, bool lastCorner, NativeList<float3> result, float3 up)
			{
				int num = indices.Length + 1 + (lastCorner ? 1 : 0);
				if (result.Capacity < num)
				{
					result.Capacity = num;
				}
				result.ResizeUninitialized(num);
				UnsafeSpan<float3> unsafeSpan = result.AsUnsafeSpan<float3>();
				Funnel.FunnelState.ConvertCornerIndicesToPathProjected(ref this, ref indices, splitAtEveryPortal, startPoint, endPoint, lastCorner, this.projectionAxis, ref unsafeSpan, up);
			}

			// Token: 0x060009E3 RID: 2531 RVA: 0x00036794 File Offset: 0x00034994
			public float4x3 UnwrappedPortalsToWorldMatrix(float3 up)
			{
				int num = 0;
				while (num < this.unwrappedPortals.Length && math.lengthsq(this.unwrappedPortals[num].xy - this.unwrappedPortals[num].zw) <= 1E-05f)
				{
					num++;
				}
				if (num >= this.unwrappedPortals.Length)
				{
					return new float4x3(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 1f);
				}
				float2 xy = this.unwrappedPortals[num].xy;
				float2 zw = this.unwrappedPortals[num].zw;
				float3 @float = this.leftFunnel[num];
				float3 lhs = this.rightFunnel[num];
				float2 float2 = zw - xy;
				float3 float3 = lhs - @float;
				float2 float4 = float2 * math.rcp(math.lengthsq(float2));
				float2x2 float2x = new float2x2(new float2(float4.x, -float4.y), new float2(float4.y, float4.x));
				float2 float5 = math.mul(float2x, -xy);
				float4x3 b = new float4x3(new float4(float2x.c0.x, 0f, float2x.c0.y, 0f), new float4(float2x.c1.x, 0f, float2x.c1.y, 0f), new float4(float5.x, 0f, float5.y, 1f));
				return math.mul(new float4x4(new float4(float3, 0f), new float4(up, 0f), new float4(math.cross(float3, up), 0f), new float4(@float, 1f)), b);
			}

			// Token: 0x060009E4 RID: 2532 RVA: 0x0003699C File Offset: 0x00034B9C
			[BurstCompile]
			public static void ConvertCornerIndicesToPathProjected(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
			{
				Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Invoke(ref funnelState, ref indices, splitAtEveryPortal, startPoint, endPoint, lastCorner, projectionAxis, ref result, up);
			}

			// Token: 0x060009E5 RID: 2533 RVA: 0x000369BC File Offset: 0x00034BBC
			private static void CalculatePortalIntersections(int startIndex, int endIndex, NativeCircularBuffer<float3> leftPortals, NativeCircularBuffer<float3> rightPortals, NativeCircularBuffer<float4> unwrappedPortals, float2 from, float2 to, NativeList<float3> result)
			{
				for (int i = startIndex; i < endIndex; i++)
				{
					float4 @float = unwrappedPortals[i];
					float2 xy = @float.xy;
					float2 zw = @float.zw;
					float t;
					if (!VectorMath.LineLineIntersectionFactor(xy, zw - xy, from, to - from, out t))
					{
						t = 0.5f;
					}
					float3 float2 = math.lerp(leftPortals[i], rightPortals[i], t);
					result.Add(float2);
				}
			}

			// Token: 0x060009E6 RID: 2534 RVA: 0x00036A34 File Offset: 0x00034C34
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void PushStart$BurstManaged(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
			{
				if (unwrappedPortals.Length == 0)
				{
					leftPortals.PushStart(newLeftPortal);
					rightPortals.PushStart(newRightPortal);
					return;
				}
				float4 @float = *unwrappedPortals.First;
				float2 float2 = Funnel.Unwrap(*leftPortals.First, *rightPortals.First, @float.xy, @float.zw, newRightPortal, -1f, projectionAxis);
				float2 xy = Funnel.Unwrap(*leftPortals.First, newRightPortal, @float.xy, float2, newLeftPortal, -1f, projectionAxis);
				leftPortals.PushStart(newLeftPortal);
				rightPortals.PushStart(newRightPortal);
				unwrappedPortals.PushStart(new float4(xy, float2));
			}

			// Token: 0x060009E7 RID: 2535 RVA: 0x00036B04 File Offset: 0x00034D04
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal unsafe static void ConvertCornerIndicesToPathProjected$BurstManaged(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
			{
				int num = 0;
				*result[num++] = startPoint;
				if (funnelState.leftFunnel.Length == 0)
				{
					if (lastCorner)
					{
						*result[num++] = endPoint;
					}
					return;
				}
				float4x3 a = funnelState.UnwrappedPortalsToWorldMatrix(up);
				if (splitAtEveryPortal)
				{
					throw new NotImplementedException();
				}
				for (int i = 0; i < indices.Length; i++)
				{
					int num2 = *indices[i];
					float2 xy = ((num2 & 1073741824) != 0) ? funnelState.unwrappedPortals[num2 & 1073741823].zw : funnelState.unwrappedPortals[num2 & 1073741823].xy;
					*result[num++] = math.mul(a, new float3(xy, 1f)).xyz;
				}
				if (lastCorner)
				{
					float2 xy2 = Funnel.Unwrap(*funnelState.leftFunnel.Last, *funnelState.rightFunnel.Last, funnelState.unwrappedPortals.Last.xy, funnelState.unwrappedPortals.Last.zw, endPoint, 1f, projectionAxis);
					*result[num++] = math.mul(a, new float3(xy2, 1f)).xyz;
				}
			}

			// Token: 0x040006B1 RID: 1713
			public NativeCircularBuffer<float3> leftFunnel;

			// Token: 0x040006B2 RID: 1714
			public NativeCircularBuffer<float3> rightFunnel;

			// Token: 0x040006B3 RID: 1715
			public NativeCircularBuffer<float4> unwrappedPortals;

			// Token: 0x040006B4 RID: 1716
			public float3 projectionAxis;

			// Token: 0x02000147 RID: 327
			// (Invoke) Token: 0x060009E9 RID: 2537
			internal delegate void PushStart_00000960$PostfixBurstDelegate(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis);

			// Token: 0x02000148 RID: 328
			internal static class PushStart_00000960$BurstDirectCall
			{
				// Token: 0x060009EC RID: 2540 RVA: 0x00036C84 File Offset: 0x00034E84
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Pointer == 0)
					{
						Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.FunnelState.PushStart_00000960$BurstDirectCall.DeferredCompilation, methodof(Funnel.FunnelState.PushStart$BurstManaged(NativeCircularBuffer<float3>*, NativeCircularBuffer<float3>*, NativeCircularBuffer<float4>*, float3*, float3*, float3*)).MethodHandle, typeof(Funnel.FunnelState.PushStart_00000960$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Pointer;
				}

				// Token: 0x060009ED RID: 2541 RVA: 0x00036CB0 File Offset: 0x00034EB0
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					Funnel.FunnelState.PushStart_00000960$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x060009EE RID: 2542 RVA: 0x00036CC8 File Offset: 0x00034EC8
				public unsafe static void Constructor()
				{
					Funnel.FunnelState.PushStart_00000960$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.FunnelState.PushStart(NativeCircularBuffer<float3>*, NativeCircularBuffer<float3>*, NativeCircularBuffer<float4>*, float3*, float3*, float3*)).MethodHandle);
				}

				// Token: 0x060009EF RID: 2543 RVA: 0x000035CE File Offset: 0x000017CE
				public static void Initialize()
				{
				}

				// Token: 0x060009F0 RID: 2544 RVA: 0x00036CD9 File Offset: 0x00034ED9
				// Note: this type is marked as 'beforefieldinit'.
				static PushStart_00000960$BurstDirectCall()
				{
					Funnel.FunnelState.PushStart_00000960$BurstDirectCall.Constructor();
				}

				// Token: 0x060009F1 RID: 2545 RVA: 0x00036CE0 File Offset: 0x00034EE0
				public static void Invoke(ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref NativeCircularBuffer<float4> unwrappedPortals, ref float3 newLeftPortal, ref float3 newRightPortal, ref float3 projectionAxis)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = Funnel.FunnelState.PushStart_00000960$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float4>&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&), ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis, functionPointer);
							return;
						}
					}
					Funnel.FunnelState.PushStart$BurstManaged(ref leftPortals, ref rightPortals, ref unwrappedPortals, ref newLeftPortal, ref newRightPortal, ref projectionAxis);
				}

				// Token: 0x040006B5 RID: 1717
				private static IntPtr Pointer;

				// Token: 0x040006B6 RID: 1718
				private static IntPtr DeferredCompilation;
			}

			// Token: 0x02000149 RID: 329
			// (Invoke) Token: 0x060009F3 RID: 2547
			internal delegate void ConvertCornerIndicesToPathProjected_0000096A$PostfixBurstDelegate(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up);

			// Token: 0x0200014A RID: 330
			internal static class ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall
			{
				// Token: 0x060009F6 RID: 2550 RVA: 0x00036D1F File Offset: 0x00034F1F
				[BurstDiscard]
				private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Pointer == 0)
					{
						Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.DeferredCompilation, methodof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected$BurstManaged(Funnel.FunnelState*, UnsafeSpan<int>*, bool, float3*, float3*, bool, float3*, UnsafeSpan<float3>*, float3*)).MethodHandle, typeof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Pointer;
				}

				// Token: 0x060009F7 RID: 2551 RVA: 0x00036D4C File Offset: 0x00034F4C
				private static IntPtr GetFunctionPointer()
				{
					IntPtr result = (IntPtr)0;
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.GetFunctionPointerDiscard(ref result);
					return result;
				}

				// Token: 0x060009F8 RID: 2552 RVA: 0x00036D64 File Offset: 0x00034F64
				public unsafe static void Constructor()
				{
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.FunnelState.ConvertCornerIndicesToPathProjected(Funnel.FunnelState*, UnsafeSpan<int>*, bool, float3*, float3*, bool, float3*, UnsafeSpan<float3>*, float3*)).MethodHandle);
				}

				// Token: 0x060009F9 RID: 2553 RVA: 0x000035CE File Offset: 0x000017CE
				public static void Initialize()
				{
				}

				// Token: 0x060009FA RID: 2554 RVA: 0x00036D75 File Offset: 0x00034F75
				// Note: this type is marked as 'beforefieldinit'.
				static ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall()
				{
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.Constructor();
				}

				// Token: 0x060009FB RID: 2555 RVA: 0x00036D7C File Offset: 0x00034F7C
				public static void Invoke(ref Funnel.FunnelState funnelState, ref UnsafeSpan<int> indices, bool splitAtEveryPortal, in float3 startPoint, in float3 endPoint, bool lastCorner, in float3 projectionAxis, ref UnsafeSpan<float3> result, in float3 up)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = Funnel.FunnelState.ConvertCornerIndicesToPathProjected_0000096A$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Pathfinding.Funnel/FunnelState&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,System.Boolean,Unity.Mathematics.float3&,Unity.Mathematics.float3&,System.Boolean,Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&), ref funnelState, ref indices, splitAtEveryPortal, ref startPoint, ref endPoint, lastCorner, ref projectionAxis, ref result, ref up, functionPointer);
							return;
						}
					}
					Funnel.FunnelState.ConvertCornerIndicesToPathProjected$BurstManaged(ref funnelState, ref indices, splitAtEveryPortal, startPoint, endPoint, lastCorner, projectionAxis, ref result, up);
				}

				// Token: 0x040006B7 RID: 1719
				private static IntPtr Pointer;

				// Token: 0x040006B8 RID: 1720
				private static IntPtr DeferredCompilation;
			}
		}

		// Token: 0x0200014B RID: 331
		// (Invoke) Token: 0x060009FD RID: 2557
		internal delegate int Calculate_00000954$PostfixBurstDelegate(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner);

		// Token: 0x0200014C RID: 332
		internal static class Calculate_00000954$BurstDirectCall
		{
			// Token: 0x06000A00 RID: 2560 RVA: 0x00036DC7 File Offset: 0x00034FC7
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Funnel.Calculate_00000954$BurstDirectCall.Pointer == 0)
				{
					Funnel.Calculate_00000954$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Funnel.Calculate_00000954$BurstDirectCall.DeferredCompilation, methodof(Funnel.Calculate$BurstManaged(NativeCircularBuffer<float4>*, NativeCircularBuffer<float3>*, NativeCircularBuffer<float3>*, float3*, float3*, UnsafeSpan<int>*, int, float3*, bool*)).MethodHandle, typeof(Funnel.Calculate_00000954$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Funnel.Calculate_00000954$BurstDirectCall.Pointer;
			}

			// Token: 0x06000A01 RID: 2561 RVA: 0x00036DF4 File Offset: 0x00034FF4
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Funnel.Calculate_00000954$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000A02 RID: 2562 RVA: 0x00036E0C File Offset: 0x0003500C
			public unsafe static void Constructor()
			{
				Funnel.Calculate_00000954$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Funnel.Calculate(NativeCircularBuffer<float4>*, NativeCircularBuffer<float3>*, NativeCircularBuffer<float3>*, float3*, float3*, UnsafeSpan<int>*, int, float3*, bool*)).MethodHandle);
			}

			// Token: 0x06000A03 RID: 2563 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000A04 RID: 2564 RVA: 0x00036E1D File Offset: 0x0003501D
			// Note: this type is marked as 'beforefieldinit'.
			static Calculate_00000954$BurstDirectCall()
			{
				Funnel.Calculate_00000954$BurstDirectCall.Constructor();
			}

			// Token: 0x06000A05 RID: 2565 RVA: 0x00036E24 File Offset: 0x00035024
			public static int Invoke(ref NativeCircularBuffer<float4> unwrappedPortals, ref NativeCircularBuffer<float3> leftPortals, ref NativeCircularBuffer<float3> rightPortals, ref float3 startPoint, ref float3 endPoint, ref UnsafeSpan<int> funnelPath, int maxCorners, ref float3 projectionAxis, out bool lastCorner)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Funnel.Calculate_00000954$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float4>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Pathfinding.Collections.NativeCircularBuffer`1<Unity.Mathematics.float3>&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Pathfinding.Collections.UnsafeSpan`1<System.Int32>&,System.Int32,Unity.Mathematics.float3&,System.Boolean&), ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, ref lastCorner, functionPointer);
					}
				}
				return Funnel.Calculate$BurstManaged(ref unwrappedPortals, ref leftPortals, ref rightPortals, ref startPoint, ref endPoint, ref funnelPath, maxCorners, ref projectionAxis, out lastCorner);
			}

			// Token: 0x040006B9 RID: 1721
			private static IntPtr Pointer;

			// Token: 0x040006BA RID: 1722
			private static IntPtr DeferredCompilation;
		}
	}
}

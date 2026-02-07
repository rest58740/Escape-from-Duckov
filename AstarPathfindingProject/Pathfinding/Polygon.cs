using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000050 RID: 80
	[BurstCompile]
	public static class Polygon
	{
		// Token: 0x060002DE RID: 734 RVA: 0x0000DA2B File Offset: 0x0000BC2B
		public static bool ContainsPointXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			return VectorMath.IsClockwiseMarginXZ(a, b, p) && VectorMath.IsClockwiseMarginXZ(b, c, p) && VectorMath.IsClockwiseMarginXZ(c, a, p);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000DA4B File Offset: 0x0000BC4B
		public static bool ContainsPointXZ(Int3 a, Int3 b, Int3 c, Int3 p)
		{
			return VectorMath.IsClockwiseOrColinearXZ(a, b, p) && VectorMath.IsClockwiseOrColinearXZ(b, c, p) && VectorMath.IsClockwiseOrColinearXZ(c, a, p);
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000DA6B File Offset: 0x0000BC6B
		public static bool ContainsPoint(Vector2Int a, Vector2Int b, Vector2Int c, Vector2Int p)
		{
			return VectorMath.IsClockwiseOrColinear(a, b, p) && VectorMath.IsClockwiseOrColinear(b, c, p) && VectorMath.IsClockwiseOrColinear(c, a, p);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public static bool ContainsPoint(Vector2[] polyPoints, Vector2 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].y <= p.y && p.y < polyPoints[num].y) || (polyPoints[num].y <= p.y && p.y < polyPoints[i].y)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.y - polyPoints[i].y) / (polyPoints[num].y - polyPoints[i].y) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000DB6C File Offset: 0x0000BD6C
		public static bool ContainsPointXZ(Vector3[] polyPoints, Vector3 p)
		{
			int num = polyPoints.Length - 1;
			bool flag = false;
			int i = 0;
			while (i < polyPoints.Length)
			{
				if (((polyPoints[i].z <= p.z && p.z < polyPoints[num].z) || (polyPoints[num].z <= p.z && p.z < polyPoints[i].z)) && p.x < (polyPoints[num].x - polyPoints[i].x) * (p.z - polyPoints[i].z) / (polyPoints[num].z - polyPoints[i].z) + polyPoints[i].x)
				{
					flag = !flag;
				}
				num = i++;
			}
			return flag;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000DC49 File Offset: 0x0000BE49
		[BurstCompile]
		public static bool ContainsPoint(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
		{
			return Polygon.ContainsPoint_000002E3$BurstDirectCall.Invoke(ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000DC58 File Offset: 0x0000BE58
		public static bool ContainsPoint(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, in float2x3 planeProjection)
		{
			int2x3 int2x = new int2x3(planeProjection * 1024f);
			int4 @int = new int4(aWorld.x, bWorld.x, cWorld.x, pWorld.x);
			int4 int2 = new int4(aWorld.y, bWorld.y, cWorld.y, pWorld.y);
			int4 int3 = new int4(aWorld.z, bWorld.z, cWorld.z, pWorld.z);
			int4 lhs = @int - @int.x;
			int2 -= int2.x;
			int3 -= int3.x;
			int4 int4 = (lhs * int2x.c0.x + int2 * int2x.c1.x + int3 * int2x.c2.x) / 1024;
			int4 int5 = (lhs * int2x.c0.y + int2 * int2x.c1.y + int3 * int2x.c2.y) / 1024;
			int3 int6 = int4.yzx - int4.xyz;
			int3 int7 = int5.www - int5.xyz;
			int3 int8 = int4.www - int4.xyz;
			int3 int9 = int5.yzx - int5.xyz;
			long num = (long)int6.x * (long)int7.x - (long)int8.x * (long)int9.x;
			long num2 = (long)int6.y * (long)int7.y - (long)int8.y * (long)int9.y;
			long num3 = (long)int6.z * (long)int7.z - (long)int8.z * (long)int9.z;
			return (num >= 0L & num2 >= 0L & num3 >= 0L) | (num <= 0L & num2 <= 0L & num3 <= 0L);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000DE88 File Offset: 0x0000C088
		public static Vector3[] ConvexHullXZ(Vector3[] points)
		{
			if (points.Length == 0)
			{
				return new Vector3[0];
			}
			List<Vector3> list = ListPool<Vector3>.Claim();
			int num = 0;
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i].x < points[num].x)
				{
					num = i;
				}
			}
			int num2 = num;
			int num3 = 0;
			for (;;)
			{
				list.Add(points[num]);
				int num4 = 0;
				for (int j = 0; j < points.Length; j++)
				{
					if (num4 == num || !VectorMath.RightOrColinearXZ(points[num], points[num4], points[j]))
					{
						num4 = j;
					}
				}
				num = num4;
				num3++;
				if (num3 > 10000)
				{
					break;
				}
				if (num == num2)
				{
					goto IL_AF;
				}
			}
			Debug.LogWarning("Infinite Loop in Convex Hull Calculation");
			IL_AF:
			Vector3[] result = list.ToArray();
			ListPool<Vector3>.Release(list);
			return result;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000DF54 File Offset: 0x0000C154
		public static Vector2 ClosestPointOnTriangle(Vector2 a, Vector2 b, Vector2 c, Vector2 p)
		{
			Vector2 vector = b - a;
			Vector2 vector2 = c - a;
			Vector2 rhs = p - a;
			float num = Vector2.Dot(vector, rhs);
			float num2 = Vector2.Dot(vector2, rhs);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			Vector2 rhs2 = p - b;
			float num3 = Vector2.Dot(vector, rhs2);
			float num4 = Vector2.Dot(vector2, rhs2);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			if (num >= 0f && num3 <= 0f && num * num4 - num3 * num2 <= 0f)
			{
				float d = num / (num - num3);
				return a + vector * d;
			}
			Vector2 rhs3 = p - c;
			float num5 = Vector2.Dot(vector, rhs3);
			float num6 = Vector2.Dot(vector2, rhs3);
			if (num6 >= 0f && num5 <= num6)
			{
				return c;
			}
			if (num2 >= 0f && num6 <= 0f && num5 * num2 - num * num6 <= 0f)
			{
				float d2 = num2 / (num2 - num6);
				return a + vector2 * d2;
			}
			if (num4 - num3 >= 0f && num5 - num6 >= 0f && num3 * num6 - num5 * num4 <= 0f)
			{
				float d3 = (num4 - num3) / (num4 - num3 + (num5 - num6));
				return b + (c - b) * d3;
			}
			return p;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000E0C4 File Offset: 0x0000C2C4
		public static Vector3 ClosestPointOnTriangleXZ(Vector3 a, Vector3 b, Vector3 c, Vector3 p)
		{
			Vector2 lhs = new Vector2(b.x - a.x, b.z - a.z);
			Vector2 lhs2 = new Vector2(c.x - a.x, c.z - a.z);
			Vector2 rhs = new Vector2(p.x - a.x, p.z - a.z);
			float num = Vector2.Dot(lhs, rhs);
			float num2 = Vector2.Dot(lhs2, rhs);
			if (num <= 0f && num2 <= 0f)
			{
				return a;
			}
			Vector2 rhs2 = new Vector2(p.x - b.x, p.z - b.z);
			float num3 = Vector2.Dot(lhs, rhs2);
			float num4 = Vector2.Dot(lhs2, rhs2);
			if (num3 >= 0f && num4 <= num3)
			{
				return b;
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return (1f - num6) * a + num6 * b;
			}
			Vector2 rhs3 = new Vector2(p.x - c.x, p.z - c.z);
			float num7 = Vector2.Dot(lhs, rhs3);
			float num8 = Vector2.Dot(lhs2, rhs3);
			if (num8 >= 0f && num7 <= num8)
			{
				return c;
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return (1f - num10) * a + num10 * c;
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float d = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return b + (c - b) * d;
			}
			float num12 = 1f / (num11 + num9 + num5);
			float num13 = num9 * num12;
			float num14 = num5 * num12;
			return new Vector3(p.x, (1f - num13 - num14) * a.y + num13 * b.y + num14 * c.y, p.z);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000E328 File Offset: 0x0000C528
		public static float3 ClosestPointOnTriangle(float3 a, float3 b, float3 c, float3 p)
		{
			float3 result;
			Polygon.ClosestPointOnTriangleByRef(a, b, c, p, out result);
			return result;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000E346 File Offset: 0x0000C546
		[BurstCompile]
		public static bool ClosestPointOnTriangleByRef(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
		{
			return Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Invoke(a, b, c, p, out output);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000E354 File Offset: 0x0000C554
		public static float3 ClosestPointOnTriangleBarycentric(float2 a, float2 b, float2 c, float2 p)
		{
			float2 x = b - a;
			float2 x2 = c - a;
			float2 y = p - a;
			float num = math.dot(x, y);
			float num2 = math.dot(x2, y);
			if (num <= 0f && num2 <= 0f)
			{
				return new float3(1f, 0f, 0f);
			}
			float2 y2 = p - b;
			float num3 = math.dot(x, y2);
			float num4 = math.dot(x2, y2);
			if (num3 >= 0f && num4 <= num3)
			{
				return new float3(0f, 1f, 0f);
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return new float3(1f - num6, num6, 0f);
			}
			float2 y3 = p - c;
			float num7 = math.dot(x, y3);
			float num8 = math.dot(x2, y3);
			if (num8 >= 0f && num7 <= num8)
			{
				return new float3(0f, 0f, 1f);
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return new float3(1f - num10, 0f, num10);
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return new float3(0f, 1f - num12, num12);
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			return new float3(1f - num14 - num15, num14, num15);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E544 File Offset: 0x0000C744
		public static float3 ClosestPointOnTriangleBarycentric(float3 a, float3 b, float3 c, float3 p)
		{
			float3 x = b - a;
			float3 x2 = c - a;
			float3 y = p - a;
			float num = math.dot(x, y);
			float num2 = math.dot(x2, y);
			if (num <= 0f && num2 <= 0f)
			{
				return new float3(1f, 0f, 0f);
			}
			float3 y2 = p - b;
			float num3 = math.dot(x, y2);
			float num4 = math.dot(x2, y2);
			if (num3 >= 0f && num4 <= num3)
			{
				return new float3(0f, 1f, 0f);
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float num6 = num / (num - num3);
				return new float3(1f - num6, num6, 0f);
			}
			float3 y3 = p - c;
			float num7 = math.dot(x, y3);
			float num8 = math.dot(x2, y3);
			if (num8 >= 0f && num7 <= num8)
			{
				return new float3(0f, 0f, 1f);
			}
			float num9 = num7 * num2 - num * num8;
			if (num2 >= 0f && num8 <= 0f && num9 <= 0f)
			{
				float num10 = num2 / (num2 - num8);
				return new float3(1f - num10, 0f, num10);
			}
			float num11 = num3 * num8 - num7 * num4;
			if (num4 - num3 >= 0f && num7 - num8 >= 0f && num11 <= 0f)
			{
				float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
				return new float3(0f, 1f - num12, num12);
			}
			float num13 = 1f / (num11 + num9 + num5);
			float num14 = num9 * num13;
			float num15 = num5 * num13;
			return new float3(1f - num14 - num15, num14, num15);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000E733 File Offset: 0x0000C933
		[BurstCompile]
		public static void ClosestPointOnTriangleProjected(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
		{
			Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Invoke(ref vi1, ref vi2, ref vi3, ref projection, ref point, out closest, out sqrDist, out distAlongProjection);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000E748 File Offset: 0x0000C948
		public static void CompressMesh(List<Int3> vertices, List<int> triangles, List<uint> tags, out Int3[] outVertices, out int[] outTriangles, out uint[] outTags)
		{
			Dictionary<Int3, int> dictionary = Polygon.cached_Int3_int_dict;
			dictionary.Clear();
			int[] array = ArrayPool<int>.Claim(vertices.Count);
			int num = 0;
			for (int i = 0; i < vertices.Count; i++)
			{
				int num2;
				if (!dictionary.TryGetValue(vertices[i], out num2) && !dictionary.TryGetValue(vertices[i] + new Int3(0, 1, 0), out num2) && !dictionary.TryGetValue(vertices[i] + new Int3(0, -1, 0), out num2))
				{
					dictionary.Add(vertices[i], num);
					array[i] = num;
					vertices[num] = vertices[i];
					num++;
				}
				else
				{
					array[i] = num2;
				}
			}
			outTriangles = new int[triangles.Count];
			for (int j = 0; j < outTriangles.Length; j++)
			{
				outTriangles[j] = array[triangles[j]];
			}
			outVertices = new Int3[num];
			for (int k = 0; k < num; k++)
			{
				outVertices[k] = vertices[k];
			}
			ArrayPool<int>.Release(ref array, false);
			outTags = tags.ToArray();
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000E868 File Offset: 0x0000CA68
		public static void TraceContours(Dictionary<int, int> outline, HashSet<int> hasInEdge, Action<List<int>, bool> results)
		{
			List<int> list = ListPool<int>.Claim();
			List<int> list2 = ListPool<int>.Claim();
			list2.AddRange(outline.Keys);
			for (int i = 0; i <= 1; i++)
			{
				bool flag = i == 1;
				for (int j = 0; j < list2.Count; j++)
				{
					int num = list2[j];
					if (flag || !hasInEdge.Contains(num))
					{
						int num2 = num;
						list.Clear();
						list.Add(num2);
						while (outline.ContainsKey(num2))
						{
							int num3 = outline[num2];
							outline.Remove(num2);
							list.Add(num3);
							if (num3 == num)
							{
								break;
							}
							num2 = num3;
						}
						if (list.Count > 1)
						{
							results(list, flag);
						}
					}
				}
			}
			ListPool<int>.Release(ref list2);
			ListPool<int>.Release(ref list);
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E934 File Offset: 0x0000CB34
		public static void Subdivide(List<Vector3> points, List<Vector3> result, int subSegments)
		{
			for (int i = 0; i < points.Count - 1; i++)
			{
				for (int j = 0; j < subSegments; j++)
				{
					result.Add(Vector3.Lerp(points[i], points[i + 1], (float)j / (float)subSegments));
				}
			}
			result.Add(points[points.Count - 1]);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ContainsPoint$BurstManaged(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
		{
			float3x3 float3x = new float3x3(movementPlane.rotation.value);
			float2x3 float2x = math.transpose(new float3x2(float3x.c0, float3x.c2));
			return Polygon.ContainsPoint(ref aWorld, ref bWorld, ref cWorld, ref pWorld, float2x);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000E9E8 File Offset: 0x0000CBE8
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static bool ClosestPointOnTriangleByRef$BurstManaged(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
		{
			float3 @float = b - a;
			float3 float2 = c - a;
			float3 y = p - a;
			float num = math.dot(@float, y);
			float num2 = math.dot(float2, y);
			if (num <= 0f && num2 <= 0f)
			{
				output = a;
				return false;
			}
			float3 y2 = p - b;
			float num3 = math.dot(@float, y2);
			float num4 = math.dot(float2, y2);
			if (num3 >= 0f && num4 <= num3)
			{
				output = b;
				return false;
			}
			float num5 = num * num4 - num3 * num2;
			if (num >= 0f && num3 <= 0f && num5 <= 0f)
			{
				float rhs = num / (num - num3);
				output = a + @float * rhs;
				return false;
			}
			float3 y3 = p - c;
			float num6 = math.dot(@float, y3);
			float num7 = math.dot(float2, y3);
			if (num7 >= 0f && num6 <= num7)
			{
				output = c;
				return false;
			}
			float num8 = num6 * num2 - num * num7;
			if (num2 >= 0f && num7 <= 0f && num8 <= 0f)
			{
				float rhs2 = num2 / (num2 - num7);
				output = a + float2 * rhs2;
				return false;
			}
			float num9 = num3 * num7 - num6 * num4;
			if (num4 - num3 >= 0f && num6 - num7 >= 0f && num9 <= 0f)
			{
				float rhs3 = (num4 - num3) / (num4 - num3 + (num6 - num7));
				output = b + (c - b) * rhs3;
				return false;
			}
			float num10 = 1f / (num9 + num8 + num5);
			float rhs4 = num8 * num10;
			float rhs5 = num5 * num10;
			output = a + @float * rhs4 + float2 * rhs5;
			return true;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000EC30 File Offset: 0x0000CE30
		[BurstCompile]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal static void ClosestPointOnTriangleProjected$BurstManaged(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
		{
			float3 @float = (float3)vi1;
			float3 float2 = (float3)vi2;
			float3 float3 = (float3)vi3;
			float2 float4 = math.mul(projection.planeProjection, @float);
			float2 float5 = math.mul(projection.planeProjection, float2);
			float2 float6 = math.mul(projection.planeProjection, float3);
			float2 float7 = math.mul(projection.planeProjection, point);
			float3 float8 = Polygon.ClosestPointOnTriangleBarycentric(float4, float5, float6, float7);
			closest = @float * float8.x + float2 * float8.y + float3 * float8.z;
			float2 lhs = float4 * float8.x + float5 * float8.y + float6 * float8.z;
			distAlongProjection = math.abs(math.dot(closest - point, projection.projectionAxis));
			float num = math.length(lhs - float7);
			if (num < 0.01f)
			{
				int3 @int = (int3)vi1;
				int3 int2 = (int3)vi2;
				int3 int3 = (int3)vi3;
				int3 int4 = (int3)((Int3)point);
				if (Polygon.ContainsPoint(ref @int, ref int2, ref int3, ref int4, projection.planeProjection))
				{
					num = 0f;
				}
			}
			float num2 = num + distAlongProjection * projection.distanceScaleAlongProjectionAxis;
			sqrDist = num2 * num2;
		}

		// Token: 0x040001E1 RID: 481
		private static readonly Dictionary<Int3, int> cached_Int3_int_dict = new Dictionary<Int3, int>();

		// Token: 0x02000051 RID: 81
		public struct BarycentricTriangleInterpolator
		{
			// Token: 0x060002F4 RID: 756 RVA: 0x0000EDC4 File Offset: 0x0000CFC4
			public BarycentricTriangleInterpolator(Int3 p1, Int3 p2, Int3 p3)
			{
				double num = (double)(p2.z - p3.z) * (double)(p1.x - p3.x) + (double)(p3.x - p2.x) * (double)(p1.z - p3.z);
				double2 x = new double2((double)(p2.x - p1.x), (double)(p2.z - p1.z));
				double2 @double = new double2((double)(p3.x - p2.x), (double)(p3.z - p2.z));
				double2 double2 = new double2((double)(p1.x - p3.x), (double)(p1.z - p3.z));
				double num2 = math.lengthsq(x);
				double num3 = math.lengthsq(@double);
				double num4 = math.lengthsq(double2);
				this.thresholds = math.sqrt(new double3(num3, num4, num2)) * (2.0 / math.abs(num));
				this.origin = new int2(p3.x, p3.z);
				this.barycentricMapping = new double2x2(-@double.y, @double.x, -double2.y, double2.x) / num;
				this.ys = new double3((double)p1.y, (double)p2.y, (double)p3.y);
				this.linear1 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p2, p3, num3, this.origin);
				this.linear2 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p3, p1, num4, this.origin);
				this.linear3 = Polygon.BarycentricTriangleInterpolator.<.ctor>g__ProjectPointOnLine|7_0(p1, p2, num2, this.origin);
			}

			// Token: 0x060002F5 RID: 757 RVA: 0x0000EF54 File Offset: 0x0000D154
			public int SampleY(int2 p)
			{
				p -= this.origin;
				double2 @double = math.mul(this.barycentricMapping, new double2((double)p.x, (double)p.y));
				double3 double2 = new double3(@double.x, @double.y, 1.0 - @double.x - @double.y);
				if (double2.x < this.thresholds.x)
				{
					return (int)math.round(math.dot(this.linear1, new double3((double)p.x, (double)p.y, 1.0)));
				}
				if (double2.y < this.thresholds.y)
				{
					return (int)math.round(math.dot(this.linear2, new double3((double)p.x, (double)p.y, 1.0)));
				}
				if (double2.z < this.thresholds.z)
				{
					return (int)math.round(math.dot(this.linear3, new double3((double)p.x, (double)p.y, 1.0)));
				}
				return (int)math.round(math.dot(this.ys, double2));
			}

			// Token: 0x060002F6 RID: 758 RVA: 0x0000F090 File Offset: 0x0000D290
			[CompilerGenerated]
			internal static double3 <.ctor>g__ProjectPointOnLine|7_0(Int3 a, Int3 b, double lengthSq, int2 origin)
			{
				return new double3((double)(b.x - a.x), (double)(b.z - a.z), (double)(origin.x - a.x) * (double)(b.x - a.x) + (double)(origin.y - a.z) * (double)(b.z - a.z)) / lengthSq * (double)(b.y - a.y) + new double3(0.0, 0.0, (double)a.y);
			}

			// Token: 0x040001E2 RID: 482
			private int2 origin;

			// Token: 0x040001E3 RID: 483
			private double2x2 barycentricMapping;

			// Token: 0x040001E4 RID: 484
			private double3 thresholds;

			// Token: 0x040001E5 RID: 485
			private double3 linear1;

			// Token: 0x040001E6 RID: 486
			private double3 linear2;

			// Token: 0x040001E7 RID: 487
			private double3 linear3;

			// Token: 0x040001E8 RID: 488
			private double3 ys;
		}

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x060002F8 RID: 760
		internal delegate bool ContainsPoint_000002E3$PostfixBurstDelegate(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane);

		// Token: 0x02000053 RID: 83
		internal static class ContainsPoint_000002E3$BurstDirectCall
		{
			// Token: 0x060002FB RID: 763 RVA: 0x0000F135 File Offset: 0x0000D335
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ContainsPoint_000002E3$BurstDirectCall.Pointer == 0)
				{
					Polygon.ContainsPoint_000002E3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ContainsPoint_000002E3$BurstDirectCall.DeferredCompilation, methodof(Polygon.ContainsPoint$BurstManaged(int3*, int3*, int3*, int3*, NativeMovementPlane*)).MethodHandle, typeof(Polygon.ContainsPoint_000002E3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ContainsPoint_000002E3$BurstDirectCall.Pointer;
			}

			// Token: 0x060002FC RID: 764 RVA: 0x0000F164 File Offset: 0x0000D364
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Polygon.ContainsPoint_000002E3$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x060002FD RID: 765 RVA: 0x0000F17C File Offset: 0x0000D37C
			public unsafe static void Constructor()
			{
				Polygon.ContainsPoint_000002E3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ContainsPoint(int3*, int3*, int3*, int3*, NativeMovementPlane*)).MethodHandle);
			}

			// Token: 0x060002FE RID: 766 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x060002FF RID: 767 RVA: 0x0000F18D File Offset: 0x0000D38D
			// Note: this type is marked as 'beforefieldinit'.
			static ContainsPoint_000002E3$BurstDirectCall()
			{
				Polygon.ContainsPoint_000002E3$BurstDirectCall.Constructor();
			}

			// Token: 0x06000300 RID: 768 RVA: 0x0000F194 File Offset: 0x0000D394
			public static bool Invoke(ref int3 aWorld, ref int3 bWorld, ref int3 cWorld, ref int3 pWorld, ref NativeMovementPlane movementPlane)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ContainsPoint_000002E3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Mathematics.int3&,Unity.Mathematics.int3&,Unity.Mathematics.int3&,Unity.Mathematics.int3&,Pathfinding.Util.NativeMovementPlane&), ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane, functionPointer);
					}
				}
				return Polygon.ContainsPoint$BurstManaged(ref aWorld, ref bWorld, ref cWorld, ref pWorld, ref movementPlane);
			}

			// Token: 0x040001E9 RID: 489
			private static IntPtr Pointer;

			// Token: 0x040001EA RID: 490
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x06000302 RID: 770
		internal delegate bool ClosestPointOnTriangleByRef_000002E9$PostfixBurstDelegate(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output);

		// Token: 0x02000055 RID: 85
		internal static class ClosestPointOnTriangleByRef_000002E9$BurstDirectCall
		{
			// Token: 0x06000305 RID: 773 RVA: 0x0000F1CF File Offset: 0x0000D3CF
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Pointer == 0)
				{
					Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.DeferredCompilation, methodof(Polygon.ClosestPointOnTriangleByRef$BurstManaged(float3*, float3*, float3*, float3*, float3*)).MethodHandle, typeof(Polygon.ClosestPointOnTriangleByRef_000002E9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Pointer;
			}

			// Token: 0x06000306 RID: 774 RVA: 0x0000F1FC File Offset: 0x0000D3FC
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000307 RID: 775 RVA: 0x0000F214 File Offset: 0x0000D414
			public unsafe static void Constructor()
			{
				Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ClosestPointOnTriangleByRef(float3*, float3*, float3*, float3*, float3*)).MethodHandle);
			}

			// Token: 0x06000308 RID: 776 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000309 RID: 777 RVA: 0x0000F225 File Offset: 0x0000D425
			// Note: this type is marked as 'beforefieldinit'.
			static ClosestPointOnTriangleByRef_000002E9$BurstDirectCall()
			{
				Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.Constructor();
			}

			// Token: 0x0600030A RID: 778 RVA: 0x0000F22C File Offset: 0x0000D42C
			public static bool Invoke(in float3 a, in float3 b, in float3 c, in float3 p, [NoAlias] out float3 output)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ClosestPointOnTriangleByRef_000002E9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,Unity.Mathematics.float3&), ref a, ref b, ref c, ref p, ref output, functionPointer);
					}
				}
				return Polygon.ClosestPointOnTriangleByRef$BurstManaged(a, b, c, p, out output);
			}

			// Token: 0x040001EB RID: 491
			private static IntPtr Pointer;

			// Token: 0x040001EC RID: 492
			private static IntPtr DeferredCompilation;
		}

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x0600030C RID: 780
		internal delegate void ClosestPointOnTriangleProjected_000002EC$PostfixBurstDelegate(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection);

		// Token: 0x02000057 RID: 87
		internal static class ClosestPointOnTriangleProjected_000002EC$BurstDirectCall
		{
			// Token: 0x0600030F RID: 783 RVA: 0x0000F267 File Offset: 0x0000D467
			[BurstDiscard]
			private unsafe static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Pointer == 0)
				{
					Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.DeferredCompilation, methodof(Polygon.ClosestPointOnTriangleProjected$BurstManaged(Int3*, Int3*, Int3*, BBTree.ProjectionParams*, float3*, float3*, float*, float*)).MethodHandle, typeof(Polygon.ClosestPointOnTriangleProjected_000002EC$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Pointer;
			}

			// Token: 0x06000310 RID: 784 RVA: 0x0000F294 File Offset: 0x0000D494
			private static IntPtr GetFunctionPointer()
			{
				IntPtr result = (IntPtr)0;
				Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.GetFunctionPointerDiscard(ref result);
				return result;
			}

			// Token: 0x06000311 RID: 785 RVA: 0x0000F2AC File Offset: 0x0000D4AC
			public unsafe static void Constructor()
			{
				Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Polygon.ClosestPointOnTriangleProjected(Int3*, Int3*, Int3*, BBTree.ProjectionParams*, float3*, float3*, float*, float*)).MethodHandle);
			}

			// Token: 0x06000312 RID: 786 RVA: 0x000035CE File Offset: 0x000017CE
			public static void Initialize()
			{
			}

			// Token: 0x06000313 RID: 787 RVA: 0x0000F2BD File Offset: 0x0000D4BD
			// Note: this type is marked as 'beforefieldinit'.
			static ClosestPointOnTriangleProjected_000002EC$BurstDirectCall()
			{
				Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.Constructor();
			}

			// Token: 0x06000314 RID: 788 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
			public static void Invoke(ref Int3 vi1, ref Int3 vi2, ref Int3 vi3, ref BBTree.ProjectionParams projection, ref float3 point, [NoAlias] out float3 closest, [NoAlias] out float sqrDist, [NoAlias] out float distAlongProjection)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = Polygon.ClosestPointOnTriangleProjected_000002EC$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Int3&,Pathfinding.Collections.BBTree/ProjectionParams&,Unity.Mathematics.float3&,Unity.Mathematics.float3&,System.Single&,System.Single&), ref vi1, ref vi2, ref vi3, ref projection, ref point, ref closest, ref sqrDist, ref distAlongProjection, functionPointer);
						return;
					}
				}
				Polygon.ClosestPointOnTriangleProjected$BurstManaged(ref vi1, ref vi2, ref vi3, ref projection, ref point, out closest, out sqrDist, out distAlongProjection);
			}

			// Token: 0x040001ED RID: 493
			private static IntPtr Pointer;

			// Token: 0x040001EE RID: 494
			private static IntPtr DeferredCompilation;
		}
	}
}

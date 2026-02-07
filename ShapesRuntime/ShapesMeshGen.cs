using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000078 RID: 120
	public static class ShapesMeshGen
	{
		// Token: 0x06000D08 RID: 3336 RVA: 0x0001B6A0 File Offset: 0x000198A0
		private static bool SamePosition(Vector3 a, Vector3 b)
		{
			return Mathf.Max(Mathf.Max(Mathf.Abs(b.x - a.x), Mathf.Abs(b.y - a.y)), Mathf.Abs(b.z - a.z)) < 1E-05f;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0001B6F4 File Offset: 0x000198F4
		public static void GenPolylineMesh(Mesh mesh, IList<PolylinePoint> path, bool closed, PolylineJoins joins, bool flattenZ, bool useColors)
		{
			ShapesMeshGen.meshColors.Clear();
			ShapesMeshGen.meshVertices.Clear();
			ShapesMeshGen.meshUv0.Clear();
			ShapesMeshGen.meshUv1Prevs.Clear();
			ShapesMeshGen.meshUv2Nexts.Clear();
			ShapesMeshGen.meshTriangles.Clear();
			ShapesMeshGen.meshJoinsTriangles.Clear();
			int num = path.Count;
			if (num < 2)
			{
				mesh.Clear();
				return;
			}
			if (num == 2 && closed)
			{
				closed = false;
			}
			PolylinePoint polylinePoint = path[0];
			PolylinePoint polylinePoint2 = path[path.Count - 1];
			if ((closed || num == 2) && ShapesMeshGen.SamePosition(polylinePoint.point, polylinePoint2.point))
			{
				num--;
				if (num < 2)
				{
					return;
				}
				polylinePoint2 = path[path.Count - 2];
			}
			bool flag = joins.HasJoinMesh();
			bool flag2 = joins.HasSimpleJoin();
			int num2 = flag ? 5 : 2;
			int num3 = num * num2;
			int num4 = flag2 ? 3 : 5;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num12 = 0;
			ShapesMeshGen.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.triId = 0;
			int num13 = 0;
			for (int i = 0; i < num; i++)
			{
				bool flag3 = i == num - 1;
				bool flag4 = i == 0;
				bool flag5 = closed || (!flag3 && !flag4);
				float uvEndpointVal = (float)((!closed && (flag4 || flag3)) ? (flag4 ? -1 : 1) : 0);
				float thickness = path[i].thickness;
				Vector3 value = flattenZ ? new Vector3(path[i].point.x, path[i].point.y, 0f) : path[i].point;
				Color value2 = useColors ? path[i].color.ColorSpaceAdjusted() : default(Color);
				int num14 = i * num2;
				int num15;
				if (flag)
				{
					num15 = num14 + 1;
					num5 = num14 + 2;
					num6 = num14 + 3;
					num7 = num14 + 4;
					ShapesMeshGen.meshVertices[num14] = value;
					ShapesMeshGen.meshVertices[num15] = value;
					ShapesMeshGen.meshVertices[num5] = value;
					ShapesMeshGen.meshVertices[num6] = value;
					ShapesMeshGen.meshVertices[num7] = value;
					if (useColors)
					{
						ShapesMeshGen.meshColors[num14] = value2;
						ShapesMeshGen.meshColors[num15] = value2;
						ShapesMeshGen.meshColors[num5] = value2;
						ShapesMeshGen.meshColors[num6] = value2;
						ShapesMeshGen.meshColors[num7] = value2;
					}
					if (flag5)
					{
						num8 = (closed ? i : (i - 1)) * num4 + num3;
						num9 = num8 + 1;
						num10 = num8 + 2;
						num11 = num8 + 3;
						num12 = num8 + 4;
						ShapesMeshGen.meshVertices[num8] = value;
						ShapesMeshGen.meshVertices[num9] = value;
						ShapesMeshGen.meshVertices[num10] = value;
						if (useColors)
						{
							ShapesMeshGen.meshColors[num8] = value2;
							ShapesMeshGen.meshColors[num9] = value2;
							ShapesMeshGen.meshColors[num10] = value2;
						}
						if (!flag2)
						{
							ShapesMeshGen.meshVertices[num11] = value;
							ShapesMeshGen.meshVertices[num12] = value;
							if (useColors)
							{
								ShapesMeshGen.meshColors[num11] = value2;
								ShapesMeshGen.meshColors[num12] = value2;
							}
						}
					}
				}
				else
				{
					num15 = num14 + 1;
					ShapesMeshGen.meshVertices[num14] = value;
					ShapesMeshGen.meshVertices[num15] = value;
					if (useColors)
					{
						ShapesMeshGen.meshColors[num14] = value2;
						ShapesMeshGen.meshColors[num15] = value2;
					}
				}
				ShapesMeshGen.<>c__DisplayClass8_1 CS$<>8__locals2;
				if (i == 0)
				{
					CS$<>8__locals2.prevPos = (closed ? polylinePoint2.point : (polylinePoint.point * 2f - path[1].point));
					CS$<>8__locals2.nextPos = path[i + 1].point;
				}
				else if (i == num - 1)
				{
					CS$<>8__locals2.prevPos = path[i - 1].point;
					CS$<>8__locals2.nextPos = (closed ? polylinePoint.point : (path[num - 1].point * 2f - path[num - 2].point));
				}
				else
				{
					CS$<>8__locals2.prevPos = path[i - 1].point;
					CS$<>8__locals2.nextPos = path[i + 1].point;
				}
				ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num14, ref CS$<>8__locals2);
				ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num15, ref CS$<>8__locals2);
				if (flag)
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num5, ref CS$<>8__locals2);
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num6, ref CS$<>8__locals2);
					ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num7, ref CS$<>8__locals2);
					if (flag5)
					{
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num8, ref CS$<>8__locals2);
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num9, ref CS$<>8__locals2);
						ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num10, ref CS$<>8__locals2);
						if (!flag2)
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num11, ref CS$<>8__locals2);
							ShapesMeshGen.<GenPolylineMesh>g__SetPrevNext|8_0(num12, ref CS$<>8__locals2);
						}
					}
				}
				if (flag)
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num14, 0f, 0f);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num15, -1f, -1f);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num5, -1f, 1f);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num6, 1f, -1f);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num7, 1f, 1f);
					if (flag5)
					{
						ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num8, 0f, 0f);
						if (flag2)
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num9, 1f, -1f);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num10, 1f, 1f);
						}
						else
						{
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num9, 1f, -1f);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num10, -1f, -1f);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num11, -1f, 1f);
							ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num12, 1f, 1f);
						}
					}
				}
				else
				{
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num14, -1f, (float)i);
					ShapesMeshGen.<GenPolylineMesh>g__SetUv0|8_1(ShapesMeshGen.meshUv0, uvEndpointVal, thickness, num15, 1f, (float)i);
				}
				if (!flag3 || closed)
				{
					if (flag)
					{
						int num16 = num14;
						int b = num5;
						int c = num7;
						int num17 = flag3 ? 0 : (num16 + num2);
						int c2 = num17 + 1;
						int b2 = num17 + 3;
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|8_2(num16, b, c2, num17, ref CS$<>8__locals1);
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|8_2(num17, b2, c, num16, ref CS$<>8__locals1);
						if (flag5)
						{
							ShapesMeshGen.meshJoinsTriangles[num13++] = num8;
							ShapesMeshGen.meshJoinsTriangles[num13++] = num9;
							ShapesMeshGen.meshJoinsTriangles[num13++] = num10;
							if (!flag2)
							{
								ShapesMeshGen.meshJoinsTriangles[num13++] = num10;
								ShapesMeshGen.meshJoinsTriangles[num13++] = num11;
								ShapesMeshGen.meshJoinsTriangles[num13++] = num8;
								ShapesMeshGen.meshJoinsTriangles[num13++] = num8;
								ShapesMeshGen.meshJoinsTriangles[num13++] = num11;
								ShapesMeshGen.meshJoinsTriangles[num13++] = num12;
							}
						}
					}
					else
					{
						int num18 = num14;
						int a = num15;
						int num19 = flag3 ? 0 : (num18 + num2);
						int d = num19 + 1;
						ShapesMeshGen.<GenPolylineMesh>g__AddQuad|8_2(a, num18, num19, d, ref CS$<>8__locals1);
					}
				}
			}
			mesh.Clear();
			mesh.SetVertices(ShapesMeshGen.meshVertices.list);
			mesh.subMeshCount = (flag ? 2 : 1);
			mesh.SetTriangles(ShapesMeshGen.meshTriangles.list, 0);
			if (flag)
			{
				mesh.SetTriangles(ShapesMeshGen.meshJoinsTriangles.list, 1);
			}
			mesh.SetUVs(0, ShapesMeshGen.meshUv0.list);
			mesh.SetUVs(1, ShapesMeshGen.meshUv1Prevs.list);
			mesh.SetUVs(2, ShapesMeshGen.meshUv2Nexts.list);
			if (useColors)
			{
				mesh.SetColors(ShapesMeshGen.meshColors.list);
			}
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0001BF18 File Offset: 0x0001A118
		public static void GenPolygonMesh(Mesh mesh, List<Vector2> path, PolygonTriangulation triangulation)
		{
			ShapesMeshGen.generatingClockwisePolygon = (ShapesMath.PolygonSignedArea(path) > 0f);
			float num = ShapesMeshGen.generatingClockwisePolygon ? 1f : -1f;
			mesh.Clear();
			int count = path.Count;
			if (count < 2)
			{
				return;
			}
			int num2 = count - 2;
			int[] array = new int[num2 * 3];
			if (triangulation == PolygonTriangulation.FastConvexOnly)
			{
				int num3 = 0;
				for (int i = 0; i < num2; i++)
				{
					array[num3++] = i + 2;
					array[num3++] = i + 1;
					array[num3++] = 0;
				}
			}
			else
			{
				List<ShapesMeshGen.EarClipPoint> list = new List<ShapesMeshGen.EarClipPoint>(count);
				for (int j = 0; j < count; j++)
				{
					list.Add(new ShapesMeshGen.EarClipPoint(j, new Vector2(path[j].x, path[j].y)));
				}
				for (int k = 0; k < count; k++)
				{
					ShapesMeshGen.EarClipPoint earClipPoint = list[k];
					earClipPoint.prev = list[(k + count - 1) % count];
					earClipPoint.next = list[(k + 1) % count];
				}
				int num4 = 0;
				int num5 = 1000000;
				int count2;
				while ((count2 = list.Count) >= 3 && num5-- > 0)
				{
					if (count2 == 3)
					{
						array[num4++] = list[2].vertIndex;
						array[num4++] = list[1].vertIndex;
						array[num4++] = list[0].vertIndex;
						break;
					}
					bool flag = false;
					for (int l = 0; l < count2; l++)
					{
						ShapesMeshGen.EarClipPoint earClipPoint2 = list[l];
						if (earClipPoint2.ReflexState == ShapesMeshGen.ReflexState.Convex)
						{
							bool flag2 = true;
							int num6 = (l + count2 - 1) % count2;
							int num7 = (l + 1) % count2;
							for (int m = 0; m < count2; m++)
							{
								if (m != l && m != num6 && m != num7 && list[m].ReflexState == ShapesMeshGen.ReflexState.Reflex && ShapesMath.PointInsideTriangle(earClipPoint2.next.pt, earClipPoint2.pt, earClipPoint2.prev.pt, list[m].pt, 0f, num * -0.0001f, 0f))
								{
									flag2 = false;
									break;
								}
							}
							if (flag2)
							{
								array[num4++] = earClipPoint2.next.vertIndex;
								array[num4++] = earClipPoint2.vertIndex;
								array[num4++] = earClipPoint2.prev.vertIndex;
								earClipPoint2.next.MarkReflexUnknown();
								earClipPoint2.prev.MarkReflexUnknown();
								ShapesMeshGen.EarClipPoint next = earClipPoint2.next;
								ShapesMeshGen.EarClipPoint prev = earClipPoint2.prev;
								ShapesMeshGen.EarClipPoint prev2 = earClipPoint2.prev;
								ShapesMeshGen.EarClipPoint next2 = earClipPoint2.next;
								next.prev = prev2;
								prev.next = next2;
								list.RemoveAt(l);
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						Debug.LogError("Invalid polygon triangulation - no convex edges found. Your polygon is likely self-intersecting.\n" + "Failed point set:\n" + string.Join("\n", from p in list
						select string.Format("[{0}]: {1}", p.vertIndex, p.ReflexState)));
						break;
					}
				}
				if (num5 < 1)
				{
					Debug.LogError("Polygon triangulation failed, please report a bug (Shapes/Report Bug) with this exact case included");
				}
			}
			List<Vector3> list2 = new List<Vector3>(count);
			for (int n = 0; n < count; n++)
			{
				list2.Add(path[n]);
			}
			mesh.SetVertices(list2);
			mesh.subMeshCount = 1;
			mesh.SetTriangles(array, 0);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0001C2A5 File Offset: 0x0001A4A5
		public static void CreateDisc(Mesh mesh, int segmentsPerFullTurn, float radius)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, false, false, radius, 0f, 0f, 0f);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0001C2C0 File Offset: 0x0001A4C0
		public static void CreateCircleSector(Mesh mesh, int segmentsPerFullTurn, float radius, float angRadiansStart, float angRadiansEnd)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, 0f, angRadiansStart, angRadiansEnd);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0001C2D4 File Offset: 0x0001A4D4
		public static void CreateAnnulus(Mesh mesh, int segmentsPerFullTurn, float radius, float radiusInner)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, radiusInner, 0f, 0f);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x0001C2EB File Offset: 0x0001A4EB
		public static void CreateAnnulusSector(Mesh mesh, int segmentsPerFullTurn, float radius, float radiusInner, float angRadiansStart, float angRadiansEnd)
		{
			ShapesMeshGen.GenerateDiscMesh(mesh, segmentsPerFullTurn, true, false, radius, radiusInner, angRadiansStart, angRadiansEnd);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0001C2FC File Offset: 0x0001A4FC
		private static void GenerateDiscMesh(Mesh mesh, int segmentsPerFullTurn, bool hasSector, bool hasInnerRadius, float radius, float radiusInner, float angRadiansStart, float angRadiansEnd)
		{
			float num = hasSector ? angRadiansStart : 0f;
			float num2 = hasSector ? angRadiansEnd : 6.2831855f;
			float num3 = Mathf.Abs(num2 - num) / 6.2831855f;
			int num4 = Mathf.Max(1, Mathf.RoundToInt(num3 * (float)segmentsPerFullTurn));
			float num5 = Mathf.Max(radius, radiusInner);
			float num6 = Mathf.Cos(0.5f * Mathf.Abs(num2 - num) / (float)num4) * num5;
			float d = num5 * 2f - num6;
			float d2 = hasInnerRadius ? Mathf.Min(radius, radiusInner) : 0f;
			int num7 = num4 * 2 * 2;
			int num8 = (num4 + 1) * 2;
			ShapesMeshGen.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.triIndices = new int[num7 * 3];
			Vector3[] array = new Vector3[num8];
			Vector3[] array2 = new Vector3[num8];
			for (int i = 0; i < num4 + 1; i++)
			{
				float t = (float)i / (float)num4;
				Vector2 a = ShapesMath.AngToDir(Mathf.Lerp(num, num2, t));
				int num9 = i * 2;
				int num10 = num9 + 1;
				array[num9] = a * d;
				array[num10] = a * d2;
				array2[num9] = Vector3.forward;
				array2[num10] = Vector3.forward;
			}
			CS$<>8__locals1.tri = 0;
			for (int j = 0; j < num4; j++)
			{
				int num11 = j * 2;
				int b = num11 + 1;
				int c = num11 + 2;
				int num12 = num11 + 3;
				ShapesMeshGen.<GenerateDiscMesh>g__DblTri|17_0(num11, num12, c, ref CS$<>8__locals1);
				ShapesMeshGen.<GenerateDiscMesh>g__DblTri|17_0(num11, b, num12, ref CS$<>8__locals1);
			}
			mesh.vertices = array;
			mesh.normals = array2;
			mesh.triangles = CS$<>8__locals1.triIndices;
			mesh.RecalculateBounds();
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x0001C4F3 File Offset: 0x0001A6F3
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__SetPrevNext|8_0(int atIndex, ref ShapesMeshGen.<>c__DisplayClass8_1 A_1)
		{
			ShapesMeshGen.meshUv1Prevs[atIndex] = A_1.prevPos;
			ShapesMeshGen.meshUv2Nexts[atIndex] = A_1.nextPos;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0001C517 File Offset: 0x0001A717
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__SetUv0|8_1(ExpandoList<Vector4> uvArr, float uvEndpointVal, float pathThicc, int id, float x, float y)
		{
			uvArr[id] = new Vector4(x, y, uvEndpointVal, pathThicc);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0001C52C File Offset: 0x0001A72C
		[CompilerGenerated]
		internal static void <GenPolylineMesh>g__AddQuad|8_2(int a, int b, int c, int d, ref ShapesMeshGen.<>c__DisplayClass8_0 A_4)
		{
			ExpandoList<int> expandoList = ShapesMeshGen.meshTriangles;
			int triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList[triId] = a;
			ExpandoList<int> expandoList2 = ShapesMeshGen.meshTriangles;
			triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList2[triId] = b;
			ExpandoList<int> expandoList3 = ShapesMeshGen.meshTriangles;
			triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList3[triId] = c;
			ExpandoList<int> expandoList4 = ShapesMeshGen.meshTriangles;
			triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList4[triId] = c;
			ExpandoList<int> expandoList5 = ShapesMeshGen.meshTriangles;
			triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList5[triId] = d;
			ExpandoList<int> expandoList6 = ShapesMeshGen.meshTriangles;
			triId = A_4.triId;
			A_4.triId = triId + 1;
			expandoList6[triId] = a;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0001C5F0 File Offset: 0x0001A7F0
		[CompilerGenerated]
		internal static void <GenerateDiscMesh>g__DblTri|17_0(int a, int b, int c, ref ShapesMeshGen.<>c__DisplayClass17_0 A_3)
		{
			int[] triIndices = A_3.triIndices;
			int tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices[tri] = a;
			int[] triIndices2 = A_3.triIndices;
			tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices2[tri] = b;
			int[] triIndices3 = A_3.triIndices;
			tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices3[tri] = c;
			int[] triIndices4 = A_3.triIndices;
			tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices4[tri] = c;
			int[] triIndices5 = A_3.triIndices;
			tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices5[tri] = b;
			int[] triIndices6 = A_3.triIndices;
			tri = A_3.tri;
			A_3.tri = tri + 1;
			triIndices6[tri] = a;
		}

		// Token: 0x040002F4 RID: 756
		private static readonly ExpandoList<Color> meshColors = new ExpandoList<Color>();

		// Token: 0x040002F5 RID: 757
		private static readonly ExpandoList<Vector3> meshVertices = new ExpandoList<Vector3>();

		// Token: 0x040002F6 RID: 758
		private static readonly ExpandoList<Vector4> meshUv0 = new ExpandoList<Vector4>();

		// Token: 0x040002F7 RID: 759
		private static readonly ExpandoList<Vector3> meshUv1Prevs = new ExpandoList<Vector3>();

		// Token: 0x040002F8 RID: 760
		private static readonly ExpandoList<Vector3> meshUv2Nexts = new ExpandoList<Vector3>();

		// Token: 0x040002F9 RID: 761
		private static readonly ExpandoList<int> meshTriangles = new ExpandoList<int>();

		// Token: 0x040002FA RID: 762
		private static readonly ExpandoList<int> meshJoinsTriangles = new ExpandoList<int>();

		// Token: 0x040002FB RID: 763
		private static bool generatingClockwisePolygon;

		// Token: 0x020000A3 RID: 163
		private enum ReflexState
		{
			// Token: 0x040003EC RID: 1004
			Unknown,
			// Token: 0x040003ED RID: 1005
			Reflex,
			// Token: 0x040003EE RID: 1006
			Convex
		}

		// Token: 0x020000A4 RID: 164
		private class EarClipPoint
		{
			// Token: 0x06000DA9 RID: 3497 RVA: 0x0001DB5B File Offset: 0x0001BD5B
			public EarClipPoint(int vertIndex, Vector2 pt)
			{
				this.vertIndex = vertIndex;
				this.pt = pt;
			}

			// Token: 0x06000DAA RID: 3498 RVA: 0x0001DB71 File Offset: 0x0001BD71
			public void MarkReflexUnknown()
			{
				this.reflex = ShapesMeshGen.ReflexState.Unknown;
			}

			// Token: 0x170001FE RID: 510
			// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0001DB7C File Offset: 0x0001BD7C
			public ShapesMeshGen.ReflexState ReflexState
			{
				get
				{
					if (this.reflex == ShapesMeshGen.ReflexState.Unknown)
					{
						Vector2 b = ShapesMath.Dir(this.pt, this.next.pt);
						Vector2 a = ShapesMath.Dir(this.prev.pt, this.pt);
						int num = ShapesMeshGen.generatingClockwisePolygon ? 1 : -1;
						this.reflex = (((float)num * ShapesMath.Determinant(a, b) >= -0.001f) ? ShapesMeshGen.ReflexState.Reflex : ShapesMeshGen.ReflexState.Convex);
					}
					return this.reflex;
				}
			}

			// Token: 0x040003EF RID: 1007
			public int vertIndex;

			// Token: 0x040003F0 RID: 1008
			public Vector2 pt;

			// Token: 0x040003F1 RID: 1009
			private ShapesMeshGen.ReflexState reflex;

			// Token: 0x040003F2 RID: 1010
			public ShapesMeshGen.EarClipPoint prev;

			// Token: 0x040003F3 RID: 1011
			public ShapesMeshGen.EarClipPoint next;
		}
	}
}

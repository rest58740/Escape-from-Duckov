using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using UnityEngine.Splines.Interpolators;

namespace sc.modeling.splines.runtime
{
	// Token: 0x02000005 RID: 5
	public static class SplineMeshGenerator
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000031DC File Offset: 0x000013DC
		private static int CalculateSegmentCount(Settings settings, float splineLength, float meshLength, bool closed)
		{
			int segments = settings.distribution.segments;
			if (!settings.distribution.autoSegmentCount)
			{
				return segments;
			}
			if (closed)
			{
				splineLength += 0.001f;
			}
			if (settings.distribution.stretchToFit)
			{
				return (int)math.ceil(splineLength / meshLength);
			}
			if (settings.distribution.evenOnly)
			{
				return (int)math.floor(splineLength / meshLength);
			}
			return (int)math.ceil(splineLength / meshLength);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003248 File Offset: 0x00001448
		public static Mesh CreateMesh(SplineContainer splineContainer, Mesh sourceMesh, float4x4 worldToLocalMatrix, Settings settings, List<SplineData<float3>> scaleData = null, List<SplineData<float>> rollData = null, List<SplineData<SplineMesher.VertexColorChannel>> redVertexColor = null, List<SplineData<SplineMesher.VertexColorChannel>> greenVertexColor = null, List<SplineData<SplineMesher.VertexColorChannel>> blueVertexColor = null, List<SplineData<SplineMesher.VertexColorChannel>> alphaVertexColor = null)
		{
			SplineMeshGenerator.<>c__DisplayClass24_0 CS$<>8__locals1;
			CS$<>8__locals1.settings = settings;
			Mesh mesh = new Mesh();
			int subMeshCount = sourceMesh.subMeshCount;
			int count = splineContainer.Splines.Count;
			SplineMeshGenerator.combineInstances.Clear();
			SplineMeshGenerator.boundsMin = Vector3.one * float.NegativeInfinity;
			SplineMeshGenerator.boundsMax = Vector3.one * float.PositiveInfinity;
			SplineMeshGenerator.sourceVertices = sourceMesh.vertices;
			int num = SplineMeshGenerator.sourceVertices.Length;
			SplineMeshGenerator.sourceNormals = sourceMesh.normals;
			sourceMesh.GetUVs(0, SplineMeshGenerator.sourceUv0);
			SplineMeshGenerator.sourceTangents = sourceMesh.tangents;
			SplineMeshGenerator.sourceColors = sourceMesh.colors;
			SplineMeshGenerator.bounds = sourceMesh.bounds;
			SplineMeshGenerator.sourceTriangles.Clear();
			for (int i = 0; i < subMeshCount; i++)
			{
				SplineMeshGenerator.sourceTriangles.Add(sourceMesh.GetTriangles(i));
			}
			SplineMeshGenerator.hasUV = (SplineMeshGenerator.sourceUv0.Count > 0);
			SplineMeshGenerator.hasTangents = (SplineMeshGenerator.sourceTangents.Length != 0);
			SplineMeshGenerator.hasSourceVertexColor = (SplineMeshGenerator.sourceColors.Length != 0);
			Color color = Color.black;
			SplineMeshGenerator.setVertexColor = SplineMeshGenerator.hasSourceVertexColor;
			SplineMeshGenerator.splineLocalToWorld = splineContainer.transform.localToWorldMatrix;
			int num2 = 0;
			bool flag = scaleData != null;
			bool flag2 = rollData != null;
			bool flag3 = redVertexColor != null;
			bool flag4 = greenVertexColor != null;
			bool flag5 = blueVertexColor != null;
			bool flag6 = alphaVertexColor != null;
			float2 @float = new float2(CS$<>8__locals1.settings.distribution.trimStart, CS$<>8__locals1.settings.distribution.trimEnd);
			for (int j = 0; j < count; j++)
			{
				SplineMeshGenerator.<>c__DisplayClass24_1 CS$<>8__locals2;
				CS$<>8__locals2.spline = splineContainer.Splines[j];
				CS$<>8__locals2.splineLength = CS$<>8__locals2.spline.CalculateLength(SplineMeshGenerator.splineLocalToWorld);
				float2 float2 = new float2(@float.x / CS$<>8__locals2.splineLength, 1f - @float.y / CS$<>8__locals2.splineLength);
				float num3 = @float.x + @float.y;
				CS$<>8__locals2.splineLength -= num3;
				float num4 = CS$<>8__locals1.settings.deforming.scale.z;
				float y = SplineMeshGenerator.bounds.size.y;
				float num5 = math.max(0.1f, SplineMeshGenerator.bounds.size.z * num4);
				CS$<>8__locals2.segmentLength = num5 + CS$<>8__locals1.settings.distribution.spacing;
				if (CS$<>8__locals2.splineLength > 0.02f)
				{
					float splineLength = CS$<>8__locals2.splineLength;
					float segmentLength = CS$<>8__locals2.segmentLength;
					int num6 = SplineMeshGenerator.<CreateMesh>g__CalculateSegments|24_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
					if (num6 != 0)
					{
						if (CS$<>8__locals1.settings.distribution.stretchToFit)
						{
							float num7 = (float)num6 * CS$<>8__locals2.segmentLength;
							float num8 = CS$<>8__locals2.splineLength / num7;
							num4 *= num8;
							num5 = math.max(0.1f, SplineMeshGenerator.bounds.size.z * num4);
							CS$<>8__locals2.segmentLength = num5 + CS$<>8__locals1.settings.distribution.spacing;
							num6 = SplineMeshGenerator.<CreateMesh>g__CalculateSegments|24_0(ref CS$<>8__locals1, ref CS$<>8__locals2);
							if (num6 == 0)
							{
								goto IL_D66;
							}
						}
						Mesh mesh2 = new Mesh();
						mesh2.subMeshCount = subMeshCount;
						SplineMeshGenerator.triangles.Clear();
						for (int k = 0; k < subMeshCount; k++)
						{
							SplineMeshGenerator.triangles.Add(new List<int>());
						}
						SplineMeshGenerator.vertices.Clear();
						SplineMeshGenerator.normals.Clear();
						SplineMeshGenerator.tangents.Clear();
						SplineMeshGenerator.uv0.Clear();
						SplineMeshGenerator.colors.Clear();
						float3 float3 = 0f;
						float3 float4 = 0f;
						float3 float5 = 0f;
						float3 float6 = 0f;
						float3 lhs = 0f;
						quaternion quaternion = quaternion.identity;
						quaternion q = quaternion.identity;
						float3 float7 = new float3(1f);
						for (int l = 0; l < num6; l++)
						{
							float num9 = (float)l * CS$<>8__locals2.segmentLength;
							float num10 = -1f;
							for (int m = 0; m < num; m++)
							{
								float num11 = (SplineMeshGenerator.sourceVertices[m].z - SplineMeshGenerator.bounds.min.z) / (SplineMeshGenerator.bounds.max.z - SplineMeshGenerator.bounds.min.z) * num5 + num9;
								float num12 = 0.5f * num5 + num9;
								bool flag7 = math.abs(num11 - num10) > 0f;
								if (flag7)
								{
									num10 = num11;
								}
								float3 float8 = float3;
								float num13 = num11 / CS$<>8__locals2.splineLength;
								if (flag7)
								{
									num13 = math.lerp(float2.x, float2.y, num13);
									num13 = math.clamp(num13, 1E-06f, 0.999999f);
									CS$<>8__locals2.spline.Evaluate(num13, out float3, out float4, out float5);
									float6 = math.normalize(float4);
									lhs = math.cross(float5, float6);
									quaternion = quaternion.LookRotation(float6, float5);
									if (CS$<>8__locals1.settings.deforming.ignoreKnotRotation && CS$<>8__locals1.settings.deforming.rollAngle == 0f)
									{
										quaternion = SplineMeshGenerator.RollCorrectedRotation(float6);
										lhs = math.rotate(quaternion, math.right());
									}
									if ((CS$<>8__locals1.settings.deforming.rollAngle != 0f || flag2) && (!CS$<>8__locals1.settings.conforming.enable || !CS$<>8__locals1.settings.conforming.align))
									{
										float num14 = (CS$<>8__locals1.settings.deforming.rollMode == Settings.Deforming.RollMode.PerSegment) ? (num12 / CS$<>8__locals2.splineLength) : num13;
										float num15 = (CS$<>8__locals1.settings.deforming.rollFrequency > 0f) ? (CS$<>8__locals1.settings.deforming.rollFrequency * (num14 * CS$<>8__locals2.splineLength)) : 1f;
										float num16 = CS$<>8__locals1.settings.deforming.rollAngle * num15;
										if (flag2 && rollData[j].Count > 0)
										{
											num16 += rollData[j].Evaluate<Spline, LerpFloat>(CS$<>8__locals2.spline, CS$<>8__locals2.spline.ConvertIndexUnit(num14, PathIndexUnit.Normalized, CS$<>8__locals1.settings.deforming.rollPathIndexUnit), CS$<>8__locals1.settings.deforming.rollPathIndexUnit, SplineMeshGenerator.FloatInterpolator);
										}
										quaternion = math.mul(quaternion.AxisAngle(float6, -num16 * 0.017453292f), quaternion);
										lhs = math.mul(quaternion, math.right());
										float5 = math.mul(quaternion, math.up());
									}
									float7 = new float3(1f);
									if (flag && scaleData[j].Count > 0)
									{
										float7 = scaleData[j].Evaluate<Spline, LerpFloat3>(CS$<>8__locals2.spline, CS$<>8__locals2.spline.ConvertIndexUnit(num11, PathIndexUnit.Distance, CS$<>8__locals1.settings.deforming.scalePathIndexUnit), CS$<>8__locals1.settings.deforming.scalePathIndexUnit, SplineMeshGenerator.Float3Interpolator);
									}
									float7.x *= CS$<>8__locals1.settings.deforming.scale.x;
									float7.y *= CS$<>8__locals1.settings.deforming.scale.y;
									float7.z = 0f;
									float8 = float3;
									q = quaternion;
								}
								color = (SplineMeshGenerator.hasSourceVertexColor ? SplineMeshGenerator.sourceColors[m] : Color.clear);
								float t = CS$<>8__locals2.spline.ConvertIndexUnit(num11, PathIndexUnit.Distance, CS$<>8__locals1.settings.color.pathIndexUnit);
								if (flag3 && redVertexColor[j].Count > 0)
								{
									color.r = redVertexColor[j].Evaluate<Spline, SplineMesher.VertexColorChannel.LerpVertexColorData>(CS$<>8__locals2.spline, t, CS$<>8__locals1.settings.color.pathIndexUnit, new SplineMesher.VertexColorChannel.LerpVertexColorData(color.r));
									SplineMeshGenerator.setVertexColor = true;
								}
								if (flag4 && greenVertexColor[j].Count > 0)
								{
									color.g = greenVertexColor[j].Evaluate<Spline, SplineMesher.VertexColorChannel.LerpVertexColorData>(CS$<>8__locals2.spline, t, CS$<>8__locals1.settings.color.pathIndexUnit, new SplineMesher.VertexColorChannel.LerpVertexColorData(color.g));
									SplineMeshGenerator.setVertexColor = true;
								}
								if (flag5 && blueVertexColor[j].Count > 0)
								{
									color.b = blueVertexColor[j].Evaluate<Spline, SplineMesher.VertexColorChannel.LerpVertexColorData>(CS$<>8__locals2.spline, t, CS$<>8__locals1.settings.color.pathIndexUnit, new SplineMesher.VertexColorChannel.LerpVertexColorData(color.b));
									SplineMeshGenerator.setVertexColor = true;
								}
								if (flag6 && alphaVertexColor[j].Count > 0)
								{
									color.a = alphaVertexColor[j].Evaluate<Spline, SplineMesher.VertexColorChannel.LerpVertexColorData>(CS$<>8__locals2.spline, t, CS$<>8__locals1.settings.color.pathIndexUnit, new SplineMesher.VertexColorChannel.LerpVertexColorData(color.a));
									SplineMeshGenerator.setVertexColor = true;
								}
								float3 float9;
								float3 float10;
								if (CS$<>8__locals1.settings.conforming.enable && SplineMeshGenerator.PerformConforming(math.transform(SplineMeshGenerator.splineLocalToWorld, float8), CS$<>8__locals1.settings.conforming, y, out float9, out float10))
								{
									float9 = splineContainer.transform.InverseTransformPoint(float9);
									float10 = splineContainer.transform.InverseTransformVector(float10);
									float8.y = float9.y;
									quaternion quaternion2 = quaternion.LookRotationSafe(float4, float10);
									if (CS$<>8__locals1.settings.conforming.align)
									{
										quaternion = quaternion2;
									}
									if (CS$<>8__locals1.settings.conforming.blendNormal)
									{
										q = quaternion2;
									}
								}
								float8 += lhs * CS$<>8__locals1.settings.deforming.curveOffset.x;
								float8.y += CS$<>8__locals1.settings.deforming.curveOffset.y;
								float3 float11 = SplineMeshGenerator.sourceVertices[m] + math.forward() * CS$<>8__locals1.settings.distribution.spacing;
								float11.x += CS$<>8__locals1.settings.deforming.pivotOffset.x;
								float11.y += CS$<>8__locals1.settings.deforming.pivotOffset.y;
								float3 float12 = float8 + math.rotate(quaternion, float11 * float7);
								float3 xyz = math.mul(SplineMeshGenerator.splineLocalToWorld, new float4(float12, 1f)).xyz;
								xyz = math.mul(worldToLocalMatrix, new float4(xyz, 1f)).xyz;
								float3 v = math.rotate(q, SplineMeshGenerator.sourceNormals[m]);
								if (SplineMeshGenerator.hasTangents)
								{
									float4 float13 = new float4(SplineMeshGenerator.sourceTangents[m]);
									float3 xyz2 = math.rotate(q, float13.xyz);
									SplineMeshGenerator.tangents.Add(new float4(xyz2, 1f));
								}
								SplineMeshGenerator.boundsMin = math.min(float12, SplineMeshGenerator.boundsMin);
								SplineMeshGenerator.boundsMax = math.max(float12, SplineMeshGenerator.boundsMax);
								SplineMeshGenerator.vertices.Add(xyz);
								SplineMeshGenerator.normals.Add(v);
								if (SplineMeshGenerator.hasUV)
								{
									Vector4 vector = SplineMeshGenerator.sourceUv0[m];
									if (CS$<>8__locals1.settings.uv.stretchMode == Settings.UV.StretchMode.U)
									{
										vector.x = num13;
									}
									if (CS$<>8__locals1.settings.uv.stretchMode == Settings.UV.StretchMode.V)
									{
										vector.y = num13;
									}
									vector = vector * CS$<>8__locals1.settings.uv.scale + CS$<>8__locals1.settings.uv.offset;
									if (CS$<>8__locals1.settings.mesh.storeGradientsInUV)
									{
										vector.z = num13;
										vector.w = math.abs(float11.y / (y * float7.y));
									}
									SplineMeshGenerator.uv0.Add(vector);
								}
								if (SplineMeshGenerator.setVertexColor)
								{
									SplineMeshGenerator.colors.Add(color);
								}
							}
							for (int n = 0; n < subMeshCount; n++)
							{
								int num17 = SplineMeshGenerator.sourceTriangles[n].Length;
								for (int num18 = 0; num18 < num17; num18++)
								{
									SplineMeshGenerator.triangles[n].Insert(l * num17 + num18, SplineMeshGenerator.sourceTriangles[n][num18] + num * l);
								}
							}
						}
						int count2 = SplineMeshGenerator.vertices.Count;
						num2 += count2 * subMeshCount;
						mesh2.indexFormat = ((count2 >= 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
						mesh2.SetVertices(SplineMeshGenerator.vertices, 0, count2, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
						mesh2.SetNormals(SplineMeshGenerator.normals, 0, count2, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
						if (SplineMeshGenerator.hasTangents)
						{
							mesh2.SetTangents(SplineMeshGenerator.tangents);
						}
						if (SplineMeshGenerator.hasUV)
						{
							mesh2.SetUVs(0, SplineMeshGenerator.uv0);
						}
						if (SplineMeshGenerator.setVertexColor)
						{
							mesh2.SetColors(SplineMeshGenerator.colors);
						}
						for (int num19 = 0; num19 < subMeshCount; num19++)
						{
							mesh2.SetIndices(SplineMeshGenerator.triangles[num19], MeshTopology.Triangles, num19, false, 0);
							CombineInstance combineInstance = new CombineInstance
							{
								mesh = mesh2,
								subMeshIndex = num19
							};
							SplineMeshGenerator.combineInstances.Add(combineInstance);
						}
					}
				}
				IL_D66:;
			}
			mesh.indexFormat = ((num2 >= 65535) ? IndexFormat.UInt32 : IndexFormat.UInt16);
			mesh.CombineMeshes(SplineMeshGenerator.combineInstances.ToArray(), subMeshCount == 1, false);
			mesh.UploadMeshData(!CS$<>8__locals1.settings.mesh.keepReadable);
			mesh.bounds.SetMinMax(SplineMeshGenerator.boundsMin, SplineMeshGenerator.boundsMax);
			mesh.name = sourceMesh.name + " Spline";
			return mesh;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00004044 File Offset: 0x00002244
		public static bool PerformConforming(float3 positionWS, Settings.Conforming settings, float objectHeight, out float3 hitPosition, out float3 hitNormal)
		{
			bool flag = false;
			float num = math.max(objectHeight + settings.seekDistance, 1f);
			hitPosition = float3.zero;
			hitNormal = float3.zero;
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(positionWS + math.up() * num, -math.up(), out raycastHit, num * 2f, settings.layerMask, QueryTriggerInteraction.Ignore))
			{
				flag = true;
				if (settings.terrainOnly)
				{
					flag = (raycastHit.collider.GetType() == typeof(TerrainCollider));
					if (!flag)
					{
						return false;
					}
				}
				hitPosition = raycastHit.point;
				hitNormal = raycastHit.normal;
			}
			return flag;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004118 File Offset: 0x00002318
		public static Mesh TransformMesh(Mesh input, Vector3 rotation, bool flipX, bool flipY)
		{
			float num = math.abs(math.length(rotation));
			if (num > 0.01f || flipX || flipY)
			{
				Vector3[] array = input.vertices;
				int num2 = array.Length;
				Vector3[] array2 = input.normals;
				int[] array3 = input.triangles;
				int num3 = array3.Length;
				Bounds bounds = input.bounds;
				if (num > 0.01f)
				{
					ref float ptr = ref rotation.x;
					float z = rotation.z;
					float x = rotation.x;
					ptr = z;
					rotation.z = x;
					bounds = default(Bounds);
					Quaternion q = Quaternion.Euler(rotation);
					for (int i = 0; i < num2; i++)
					{
						array[i] = math.rotate(q, array[i]);
						bounds.Encapsulate(array[i]);
						array2[i] = math.rotate(q, array2[i]);
					}
				}
				if (flipX || flipY)
				{
					int num4 = num3 / 3;
					for (int j = 0; j < num4; j++)
					{
						ref int ptr2 = ref array3[j * 3];
						int[] array4 = array3;
						int num5 = j * 3 + 1;
						int num6 = array3[j * 3 + 1];
						int num7 = array3[j * 3];
						ptr2 = num6;
						array4[num5] = num7;
					}
					Quaternion q2 = Quaternion.Euler(flipY ? 180f : 0f, flipX ? 180f : 0f, 0f);
					for (int k = 0; k < num2; k++)
					{
						array2[k] = math.rotate(q2, array2[k]);
					}
				}
				Mesh mesh = new Mesh();
				mesh.name = input.name;
				mesh.SetVertices(array, 0, num2, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontRecalculateBounds);
				mesh.triangles = array3;
				mesh.bounds = bounds;
				mesh.uv = input.uv;
				mesh.uv2 = input.uv2;
				mesh.normals = array2;
				mesh.colors = input.colors;
				mesh.tangents = input.tangents;
				mesh.subMeshCount = input.subMeshCount;
				mesh.UploadMeshData(input.isReadable);
				return mesh;
			}
			return input;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004350 File Offset: 0x00002550
		public static quaternion RollCorrectedRotation(float3 forward)
		{
			float3 @float = Quaternion.LookRotation(forward, math.up()).eulerAngles;
			return quaternion.AxisAngle(math.up(), @float.y * 0.017453292f);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004398 File Offset: 0x00002598
		public static Mesh CreateBoundsMesh(Mesh sourceMesh, int subdivisions = 0, bool caps = false)
		{
			Bounds bounds = sourceMesh.bounds;
			Mesh mesh = new Mesh();
			mesh.name = sourceMesh.name + " Bounds";
			Vector3 size = bounds.size;
			Vector3 center = bounds.center;
			int num = 4;
			subdivisions = Mathf.Max(0, subdivisions);
			int num2 = subdivisions + 1;
			int num3 = num + 1;
			int num4 = num2 + 1;
			int num5 = num3 * num4;
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			float num6 = size.z / (float)num2;
			for (int i = 0; i < num4; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					Vector3 vector;
					vector.x = SplineMeshGenerator.corners[j].x * size.x + center.x;
					vector.y = SplineMeshGenerator.corners[j].y * size.y + center.y;
					vector.z = (float)i * num6 - size.z * 0.5f + center.z;
					list.Add(vector);
				}
				if (i < num4 - 1)
				{
					for (int k = 0; k < num; k++)
					{
						list2.Insert(0, i * num3 + k);
						list2.Insert(1, (i + 1) * num3 + k);
						list2.Insert(2, i * num3 + k + 1);
						list2.Insert(3, (i + 1) * num3 + k);
						list2.Insert(4, (i + 1) * num3 + k + 1);
						list2.Insert(5, i * num3 + k + 1);
					}
				}
			}
			if (caps)
			{
				list2.Add(1);
				list2.Add(2);
				list2.Add(0);
				list2.Add(2);
				list2.Add(3);
				list2.Add(0);
				list2.Add(num5 - 4);
				list2.Add(num5 - 5);
				list2.Add(num5 - 3);
				list2.Add(num5 - 2);
				list2.Add(num5 - 3);
				list2.Add(num5 - 5);
			}
			mesh.SetVertices(list, 0, num5, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers | MeshUpdateFlags.DontRecalculateBounds);
			mesh.subMeshCount = 1;
			mesh.SetIndices(list2, MeshTopology.Triangles, 0, false, 0);
			mesh.RecalculateNormals();
			mesh.bounds = bounds;
			return mesh;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000046D6 File Offset: 0x000028D6
		[CompilerGenerated]
		internal static int <CreateMesh>g__CalculateSegments|24_0(ref SplineMeshGenerator.<>c__DisplayClass24_0 A_0, ref SplineMeshGenerator.<>c__DisplayClass24_1 A_1)
		{
			return SplineMeshGenerator.CalculateSegmentCount(A_0.settings, A_1.splineLength, A_1.segmentLength, A_1.spline.Closed);
		}

		// Token: 0x0400002B RID: 43
		private static readonly List<Vector3> vertices = new List<Vector3>();

		// Token: 0x0400002C RID: 44
		private static readonly List<Vector3> normals = new List<Vector3>();

		// Token: 0x0400002D RID: 45
		private static readonly List<Vector4> tangents = new List<Vector4>();

		// Token: 0x0400002E RID: 46
		private static readonly List<Vector4> uv0 = new List<Vector4>();

		// Token: 0x0400002F RID: 47
		private static readonly List<List<int>> triangles = new List<List<int>>();

		// Token: 0x04000030 RID: 48
		private static readonly List<Color> colors = new List<Color>();

		// Token: 0x04000031 RID: 49
		private static Vector3[] sourceVertices;

		// Token: 0x04000032 RID: 50
		private static List<int[]> sourceTriangles = new List<int[]>();

		// Token: 0x04000033 RID: 51
		private static Vector3[] sourceNormals;

		// Token: 0x04000034 RID: 52
		private static List<Vector4> sourceUv0 = new List<Vector4>();

		// Token: 0x04000035 RID: 53
		private static Vector4[] sourceTangents;

		// Token: 0x04000036 RID: 54
		private static Color[] sourceColors;

		// Token: 0x04000037 RID: 55
		private static bool hasTangents;

		// Token: 0x04000038 RID: 56
		private static bool hasUV;

		// Token: 0x04000039 RID: 57
		private static bool hasSourceVertexColor;

		// Token: 0x0400003A RID: 58
		private static bool setVertexColor;

		// Token: 0x0400003B RID: 59
		private static List<CombineInstance> combineInstances = new List<CombineInstance>();

		// Token: 0x0400003C RID: 60
		private static Bounds bounds;

		// Token: 0x0400003D RID: 61
		private static float3 boundsMin;

		// Token: 0x0400003E RID: 62
		private static float3 boundsMax;

		// Token: 0x0400003F RID: 63
		private static float4x4 splineLocalToWorld;

		// Token: 0x04000040 RID: 64
		public static readonly LerpFloat3 Float3Interpolator = default(LerpFloat3);

		// Token: 0x04000041 RID: 65
		public static readonly LerpFloat FloatInterpolator = default(LerpFloat);

		// Token: 0x04000042 RID: 66
		private static readonly Vector2[] corners = new Vector2[]
		{
			new Vector2(-0.5f, -0.5f),
			new Vector2(-0.5f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(0.5f, -0.5f),
			new Vector2(-0.5f, -0.5f)
		};
	}
}

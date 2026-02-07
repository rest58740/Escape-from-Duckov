using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001D7 RID: 471
	[BurstCompile(CompileSynchronously = true)]
	public struct JobVoxelize : IJob
	{
		// Token: 0x06000C5D RID: 3165 RVA: 0x0004A5E0 File Offset: 0x000487E0
		public unsafe void Execute()
		{
			Matrix4x4 rhs = Matrix4x4.TRS(this.graphSpaceBounds.min, Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(this.cellSize, this.cellHeight, this.cellSize));
			Matrix4x4 inverse = (this.graphTransform * rhs * Matrix4x4.Translate(new Vector3(0.5f, 0f, 0.5f))).inverse;
			float num = math.cos(math.atan(this.cellSize / this.cellHeight * math.tan(this.maxSlope * 0.017453292f)));
			VoxelPolygonClipper voxelPolygonClipper = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper2 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper3 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper4 = default(VoxelPolygonClipper);
			VoxelPolygonClipper voxelPolygonClipper5 = default(VoxelPolygonClipper);
			int num2 = 0;
			for (int i = 0; i < this.bucket.Length; i++)
			{
				num2 = math.max(this.inputMeshes[this.bucket[i]].vertices.Length, num2);
			}
			NativeArray<float3> nativeArray = new NativeArray<float3>(num2, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int width = this.voxelArea.width;
			int depth = this.voxelArea.depth;
			int num3 = Mathf.Min(width - 1, float.IsPositiveInfinity(this.graphSpaceLimits.x) ? int.MaxValue : Mathf.CeilToInt((this.graphSpaceLimits.x - this.graphSpaceBounds.min.x) / this.cellSize));
			int num4 = Mathf.Min(depth - 1, float.IsPositiveInfinity(this.graphSpaceLimits.y) ? int.MaxValue : Mathf.CeilToInt((this.graphSpaceLimits.y - this.graphSpaceBounds.min.z) / this.cellSize));
			for (int j = 0; j < this.bucket.Length; j++)
			{
				RasterizationMesh rasterizationMesh = this.inputMeshes[this.bucket[j]];
				bool flag = VectorMath.ReversesFaceOrientations(rasterizationMesh.matrix);
				UnsafeSpan<float3> vertices = rasterizationMesh.vertices;
				UnsafeSpan<int> triangles = rasterizationMesh.triangles;
				float4x4 a = inverse * rasterizationMesh.matrix;
				for (int k = 0; k < vertices.Length; k++)
				{
					nativeArray[k] = math.transform(a, *vertices[k]);
				}
				int num5 = rasterizationMesh.area;
				if (rasterizationMesh.areaIsTag)
				{
					num5 |= 16384;
				}
				IntRect intRect = default(IntRect);
				for (int l = 0; l < triangles.Length; l += 3)
				{
					float3 @float = nativeArray[*triangles[l]];
					float3 float2 = nativeArray[*triangles[l + 1]];
					float3 float3 = nativeArray[*triangles[l + 2]];
					if (flag)
					{
						float3 float4 = @float;
						@float = float3;
						float3 = float4;
					}
					int num6 = (int)math.min(math.min(@float.x, float2.x), float3.x);
					int num7 = (int)math.min(math.min(@float.z, float2.z), float3.z);
					int num8 = (int)math.ceil(math.max(math.max(@float.x, float2.x), float3.x));
					int num9 = (int)math.ceil(math.max(math.max(@float.z, float2.z), float3.z));
					if (num6 <= num3 && num7 <= num4 && num8 >= 0 && num9 >= 0)
					{
						num6 = math.clamp(num6, 0, num3);
						num8 = math.clamp(num8, 0, num3);
						num7 = math.clamp(num7, 0, num4);
						num9 = math.clamp(num9, num4, num4);
						if (l == 0)
						{
							intRect = new IntRect(num6, num7, num6, num7);
						}
						intRect.xmin = math.min(intRect.xmin, num6);
						intRect.xmax = math.max(intRect.xmax, num8);
						intRect.ymin = math.min(intRect.ymin, num7);
						intRect.ymax = math.max(intRect.ymax, num9);
						float num10 = math.normalizesafe(math.cross(float2 - @float, float3 - @float), default(float3)).y;
						if (rasterizationMesh.doubleSided)
						{
							num10 = math.abs(num10);
						}
						int area = (num10 < num) ? 0 : (1 + num5);
						voxelPolygonClipper[0] = @float;
						voxelPolygonClipper[1] = float2;
						voxelPolygonClipper[2] = float3;
						voxelPolygonClipper.n = 3;
						for (int m = num6; m <= num8; m++)
						{
							voxelPolygonClipper.ClipPolygonAlongX(ref voxelPolygonClipper2, 1f, (float)(-(float)m) + 0.5f);
							if (voxelPolygonClipper2.n >= 3)
							{
								voxelPolygonClipper2.ClipPolygonAlongX(ref voxelPolygonClipper3, -1f, (float)m + 0.5f);
								if (voxelPolygonClipper3.n >= 3)
								{
									float x2;
									float x = x2 = voxelPolygonClipper3.z.FixedElementField;
									for (int n = 1; n < voxelPolygonClipper3.n; n++)
									{
										float y = *(ref voxelPolygonClipper3.z.FixedElementField + (IntPtr)n * 4);
										x2 = math.min(x2, y);
										x = math.max(x, y);
									}
									int num11 = math.clamp((int)math.round(x2), 0, num3);
									int num12 = math.clamp((int)math.round(x), 0, num4);
									for (int num13 = num11; num13 <= num12; num13++)
									{
										voxelPolygonClipper3.ClipPolygonAlongZWithYZ(ref voxelPolygonClipper4, 1f, (float)(-(float)num13) + 0.5f);
										if (voxelPolygonClipper4.n >= 3)
										{
											voxelPolygonClipper4.ClipPolygonAlongZWithY(ref voxelPolygonClipper5, -1f, (float)num13 + 0.5f);
											if (voxelPolygonClipper5.n >= 3)
											{
												if (rasterizationMesh.flatten)
												{
													this.voxelArea.AddFlattenedSpan(num13 * width + m, area);
												}
												else
												{
													float num14;
													float x3 = num14 = voxelPolygonClipper5.y.FixedElementField;
													for (int num15 = 1; num15 < voxelPolygonClipper5.n; num15++)
													{
														float y2 = *(ref voxelPolygonClipper5.y.FixedElementField + (IntPtr)num15 * 4);
														num14 = math.min(num14, y2);
														x3 = math.max(x3, y2);
													}
													int num16 = (int)math.ceil(x3);
													int num17 = (int)num14;
													num16 = math.max(num17 + 1, num16);
													this.voxelArea.AddLinkedSpan(num13 * width + m, num17, num16, area, this.voxelWalkableClimb, j);
												}
											}
										}
									}
								}
							}
						}
					}
				}
				if (rasterizationMesh.solid)
				{
					for (int num18 = intRect.ymin; num18 <= intRect.ymax; num18++)
					{
						for (int num19 = intRect.xmin; num19 <= intRect.xmax; num19++)
						{
							this.voxelArea.ResolveSolid(num18 * this.voxelArea.width + num19, j, this.voxelWalkableClimb);
						}
					}
				}
			}
		}

		// Token: 0x04000894 RID: 2196
		[ReadOnly]
		public NativeArray<RasterizationMesh> inputMeshes;

		// Token: 0x04000895 RID: 2197
		[ReadOnly]
		public NativeArray<int> bucket;

		// Token: 0x04000896 RID: 2198
		public int voxelWalkableClimb;

		// Token: 0x04000897 RID: 2199
		public uint voxelWalkableHeight;

		// Token: 0x04000898 RID: 2200
		public float cellSize;

		// Token: 0x04000899 RID: 2201
		public float cellHeight;

		// Token: 0x0400089A RID: 2202
		public float maxSlope;

		// Token: 0x0400089B RID: 2203
		public Matrix4x4 graphTransform;

		// Token: 0x0400089C RID: 2204
		public Bounds graphSpaceBounds;

		// Token: 0x0400089D RID: 2205
		public Vector2 graphSpaceLimits;

		// Token: 0x0400089E RID: 2206
		public LinkedVoxelField voxelArea;
	}
}

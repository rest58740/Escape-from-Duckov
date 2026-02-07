using System;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DE RID: 478
	[BurstCompile(CompileSynchronously = true)]
	public struct JobBuildRegions : IJob
	{
		// Token: 0x06000C64 RID: 3172 RVA: 0x0004B3CC File Offset: 0x000495CC
		private void MarkRectWithRegion(int minx, int maxx, int minz, int maxz, ushort region, NativeArray<ushort> srcReg)
		{
			int num = maxz * this.field.width;
			for (int i = minz * this.field.width; i < num; i += this.field.width)
			{
				for (int j = minx; j < maxx; j++)
				{
					CompactVoxelCell compactVoxelCell = this.field.cells[i + j];
					int k = compactVoxelCell.index;
					int num2 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num2)
					{
						if (this.field.areaTypes[k] != 0)
						{
							srcReg[k] = region;
						}
						k++;
					}
				}
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0004B470 File Offset: 0x00049670
		public static bool FloodRegion(int x, int z, int i, uint level, ushort r, CompactVoxelField field, NativeArray<ushort> distanceField, NativeArray<ushort> srcReg, NativeArray<ushort> srcDist, NativeArray<Int3> stack, NativeArray<int> flags, NativeArray<bool> closed)
		{
			int num = field.areaTypes[i];
			int j = 1;
			stack[0] = new Int3
			{
				x = x,
				y = i,
				z = z
			};
			srcReg[i] = r;
			srcDist[i] = 0;
			int num2 = (int)((level >= 2U) ? (level - 2U) : 0U);
			int num3 = 0;
			NativeList<CompactVoxelCell> cells = field.cells;
			NativeList<CompactVoxelSpan> spans = field.spans;
			NativeList<int> areaTypes = field.areaTypes;
			while (j > 0)
			{
				j--;
				Int3 @int = stack[j];
				int y = @int.y;
				int x2 = @int.x;
				int z2 = @int.z;
				CompactVoxelSpan compactVoxelSpan = spans[y];
				ushort num4 = 0;
				for (int k = 0; k < 4; k++)
				{
					if ((long)compactVoxelSpan.GetConnection(k) != 63L)
					{
						int num5 = x2 + VoxelUtilityBurst.DX[k];
						int num6 = z2 + VoxelUtilityBurst.DZ[k] * field.width;
						int index = cells[num5 + num6].index + compactVoxelSpan.GetConnection(k);
						if (areaTypes[index] == num)
						{
							ushort num7 = srcReg[index];
							if ((num7 & 32768) != 32768)
							{
								if (num7 != 0 && num7 != r)
								{
									num4 = num7;
									break;
								}
								int num8 = k + 1 & 3;
								int connection = spans[index].GetConnection(num8);
								if ((long)connection != 63L)
								{
									int num9 = num5 + VoxelUtilityBurst.DX[num8];
									int num10 = num6 + VoxelUtilityBurst.DZ[num8] * field.width;
									int index2 = cells[num9 + num10].index + connection;
									if (areaTypes[index2] == num)
									{
										ushort num11 = srcReg[index2];
										if ((num11 & 32768) != 32768 && num11 != 0 && num11 != r)
										{
											num4 = num11;
											break;
										}
									}
								}
							}
						}
					}
				}
				if (num4 != 0)
				{
					srcReg[y] = 0;
					srcDist[y] = ushort.MaxValue;
				}
				else
				{
					num3++;
					closed[y] = true;
					for (int l = 0; l < 4; l++)
					{
						if ((long)compactVoxelSpan.GetConnection(l) != 63L)
						{
							int num12 = x2 + VoxelUtilityBurst.DX[l];
							int num13 = z2 + VoxelUtilityBurst.DZ[l] * field.width;
							int num14 = cells[num12 + num13].index + compactVoxelSpan.GetConnection(l);
							if (areaTypes[num14] == num && srcReg[num14] == 0)
							{
								if ((int)distanceField[num14] >= num2 && flags[num14] == 0)
								{
									srcReg[num14] = r;
									srcDist[num14] = 0;
									stack[j] = new Int3
									{
										x = num12,
										y = num14,
										z = num13
									};
									j++;
								}
								else
								{
									flags[num14] = (int)r;
									srcDist[num14] = 2;
								}
							}
						}
					}
				}
			}
			return num3 > 0;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0004B790 File Offset: 0x00049990
		public void Execute()
		{
			this.srcQue.Clear();
			this.dstQue.Clear();
			int width = this.field.width;
			int depth = this.field.depth;
			int num = width * depth;
			int length = this.field.spans.Length;
			int num2 = 8;
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<ushort> srcDist = new NativeArray<ushort>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<bool> closed = new NativeArray<bool>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<int> flags = new NativeArray<int>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			NativeArray<Int3> stack = new NativeArray<Int3>(length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			for (int i = 0; i < length; i++)
			{
				nativeArray[i] = 0;
				srcDist[i] = ushort.MaxValue;
				closed[i] = false;
				flags[i] = 0;
			}
			NativeList<ushort> nativeList = this.distanceField;
			NativeList<int> areaTypes = this.field.areaTypes;
			NativeList<CompactVoxelCell> cells = this.field.cells;
			ushort num3 = 2;
			this.MarkRectWithRegion(0, this.borderSize, 0, depth, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(width - this.borderSize, width, 0, depth, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(0, width, 0, this.borderSize, num3 | 32768, nativeArray);
			num3 += 1;
			this.MarkRectWithRegion(0, width, depth - this.borderSize, depth, num3 | 32768, nativeArray);
			num3 += 1;
			int num4 = 0;
			for (int j = 0; j < this.distanceField.Length; j++)
			{
				num4 = math.max((int)this.distanceField[j], num4);
			}
			NativeArray<int> nativeArray2 = new NativeArray<int>(num4 / 2 + 1, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int k = 0; k < this.field.spans.Length; k++)
			{
				if ((nativeArray[k] & 32768) != 32768 && areaTypes[k] != 0)
				{
					int num5 = (int)(this.distanceField[k] / 2);
					int num6 = nativeArray2[num5];
					nativeArray2[num5] = num6 + 1;
				}
			}
			NativeArray<int> nativeArray3 = new NativeArray<int>(nativeArray2.Length, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int l = 1; l < nativeArray3.Length; l++)
			{
				nativeArray3[l] = nativeArray3[l - 1] + nativeArray2[l - 1];
			}
			int length2 = nativeArray3[nativeArray3.Length - 1] + nativeArray2[nativeArray2.Length - 1];
			NativeArray<Int3> nativeArray4 = new NativeArray<Int3>(length2, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
			int m = 0;
			int num7 = 0;
			while (m < num)
			{
				for (int n = 0; n < this.field.width; n++)
				{
					CompactVoxelCell compactVoxelCell = cells[m + n];
					int num8 = compactVoxelCell.index;
					int num9 = compactVoxelCell.index + compactVoxelCell.count;
					while (num8 < num9)
					{
						if ((nativeArray[num8] & 32768) != 32768 && areaTypes[num8] != 0)
						{
							int num10 = (int)(this.distanceField[num8] / 2);
							int num6 = num10;
							int num5 = nativeArray3[num6];
							nativeArray3[num6] = num5 + 1;
							nativeArray4[num5] = new Int3(n, num8, m);
						}
						num8++;
					}
				}
				m += width;
				num7++;
			}
			for (int num11 = nativeArray2.Length - 1; num11 >= 0; num11--)
			{
				uint num12 = (uint)(num11 * 2);
				int num13 = nativeArray2[num11];
				for (int num14 = 0; num14 < num13; num14++)
				{
					Int3 @int = nativeArray4[nativeArray3[num11] - num14 - 1];
					int y = @int.y;
					if (flags[y] != 0 && nativeArray[y] == 0)
					{
						nativeArray[y] = (ushort)flags[y];
						this.srcQue.Enqueue(@int);
						closed[y] = true;
					}
				}
				int num15 = 0;
				while (num15 < num2 && this.srcQue.Count > 0)
				{
					while (this.srcQue.Count > 0)
					{
						Int3 int2 = this.srcQue.Dequeue();
						int num16 = areaTypes[int2.y];
						CompactVoxelSpan compactVoxelSpan = this.field.spans[int2.y];
						ushort value = nativeArray[int2.y];
						closed[int2.y] = true;
						ushort num17 = srcDist[int2.y] + 2;
						for (int num18 = 0; num18 < 4; num18++)
						{
							int connection = compactVoxelSpan.GetConnection(num18);
							if ((long)connection != 63L)
							{
								int num19 = int2.x + VoxelUtilityBurst.DX[num18];
								int num20 = int2.z + VoxelUtilityBurst.DZ[num18] * this.field.width;
								int num21 = cells[num19 + num20].index + connection;
								if ((nativeArray[num21] & 32768) != 32768 && num16 == areaTypes[num21] && num17 < srcDist[num21])
								{
									if ((uint)nativeList[num21] < num12)
									{
										srcDist[num21] = num17;
										flags[num21] = (int)value;
									}
									else if (!closed[num21])
									{
										srcDist[num21] = num17;
										if (nativeArray[num21] == 0)
										{
											this.dstQue.Enqueue(new Int3(num19, num21, num20));
										}
										nativeArray[num21] = value;
									}
								}
							}
						}
					}
					Memory.Swap<NativeQueue<Int3>>(ref this.srcQue, ref this.dstQue);
					num15++;
				}
				NativeArray<ushort> nativeArray5 = this.distanceField.AsArray();
				for (int num22 = 0; num22 < num13; num22++)
				{
					Int3 int3 = nativeArray4[nativeArray3[num11] - num22 - 1];
					if (nativeArray[int3.y] == 0 && JobBuildRegions.FloodRegion(int3.x, int3.z, int3.y, num12, num3, this.field, nativeArray5, nativeArray, srcDist, stack, flags, closed))
					{
						num3 += 1;
					}
				}
			}
			ushort maxRegions = num3;
			Matrix4x4 rhs = Matrix4x4.TRS(this.graphSpaceBounds.min, Quaternion.identity, Vector3.one) * Matrix4x4.Scale(new Vector3(this.cellSize, this.cellHeight, this.cellSize));
			Matrix4x4 m2 = this.graphTransform * rhs * Matrix4x4.Translate(new Vector3(0.5f, 0f, 0.5f));
			JobBuildRegions.FilterSmallRegions(this.field, nativeArray, this.minRegionSize, (int)maxRegions, this.relevantGraphSurfaces, this.relevantGraphSurfaceMode, m2);
			for (int num23 = 0; num23 < length; num23++)
			{
				CompactVoxelSpan value2 = this.field.spans[num23];
				value2.reg = (int)nativeArray[num23];
				this.field.spans[num23] = value2;
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0004BEA8 File Offset: 0x0004A0A8
		private static int union_find_find(NativeArray<int> arr, int x)
		{
			if (arr[x] < 0)
			{
				return x;
			}
			return arr[x] = JobBuildRegions.union_find_find(arr, arr[x]);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0004BEDC File Offset: 0x0004A0DC
		private static void union_find_union(NativeArray<int> arr, int a, int b)
		{
			a = JobBuildRegions.union_find_find(arr, a);
			b = JobBuildRegions.union_find_find(arr, b);
			if (a == b)
			{
				return;
			}
			if (arr[a] > arr[b])
			{
				int num = a;
				a = b;
				b = num;
			}
			ref NativeArray<int> ptr = ref arr;
			int index = a;
			ptr[index] += arr[b];
			arr[b] = a;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0004BF40 File Offset: 0x0004A140
		public static void FilterSmallRegions(CompactVoxelField field, NativeArray<ushort> reg, int minRegionSize, int maxRegions, NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces, RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode, float4x4 voxel2worldMatrix)
		{
			bool flag = relevantGraphSurfaces.Length != 0 && relevantGraphSurfaceMode > RecastGraph.RelevantGraphSurfaceMode.DoNotRequire;
			if (!flag && minRegionSize <= 0)
			{
				return;
			}
			NativeArray<int> arr = new NativeArray<int>(maxRegions, Allocator.Temp, NativeArrayOptions.ClearMemory);
			NativeArray<ushort> nativeArray = new NativeArray<ushort>(maxRegions, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = -1;
			}
			int length = arr.Length;
			int num = field.width * field.depth;
			int num2 = 2 | ((relevantGraphSurfaceMode == RecastGraph.RelevantGraphSurfaceMode.OnlyForCompletelyInsideTile) ? 1 : 0);
			if (flag)
			{
				float4x4 a = math.inverse(voxel2worldMatrix);
				for (int j = 0; j < relevantGraphSurfaces.Length; j++)
				{
					JobBuildRegions.RelevantGraphSurfaceInfo relevantGraphSurfaceInfo = relevantGraphSurfaces[j];
					int3 @int = (int3)math.round(math.transform(a, relevantGraphSurfaceInfo.position));
					if (@int.x >= 0 && @int.z >= 0 && @int.x < field.width && @int.z < field.depth)
					{
						float num3 = math.length(voxel2worldMatrix.c1.xyz);
						int num4 = (int)(relevantGraphSurfaceInfo.range / num3);
						CompactVoxelCell compactVoxelCell = field.cells[@int.x + @int.z * field.width];
						for (int k = compactVoxelCell.index; k < compactVoxelCell.index + compactVoxelCell.count; k++)
						{
							if (Math.Abs((int)field.spans[k].y - @int.y) <= num4 && reg[k] != 0)
							{
								ref NativeArray<ushort> ptr = ref nativeArray;
								int index = JobBuildRegions.union_find_find(arr, (int)reg[k] & -32769);
								ptr[index] |= 2;
							}
						}
					}
				}
			}
			for (int l = 0; l < num; l += field.width)
			{
				for (int m = 0; m < field.width; m++)
				{
					CompactVoxelCell compactVoxelCell2 = field.cells[m + l];
					for (int n = compactVoxelCell2.index; n < compactVoxelCell2.index + compactVoxelCell2.count; n++)
					{
						CompactVoxelSpan compactVoxelSpan = field.spans[n];
						int num5 = (int)reg[n];
						if ((num5 & -32769) != 0)
						{
							if (num5 >= length)
							{
								ref NativeArray<ushort> ptr = ref nativeArray;
								int index = JobBuildRegions.union_find_find(arr, num5 & -32769);
								ptr[index] |= 1;
							}
							else
							{
								int num6 = JobBuildRegions.union_find_find(arr, num5);
								int index = num6;
								int num7 = arr[index];
								arr[index] = num7 - 1;
								for (int num8 = 0; num8 < 4; num8++)
								{
									if ((long)compactVoxelSpan.GetConnection(num8) != 63L)
									{
										int num9 = m + VoxelUtilityBurst.DX[num8];
										int num10 = l + VoxelUtilityBurst.DZ[num8] * field.width;
										int index2 = field.cells[num9 + num10].index + compactVoxelSpan.GetConnection(num8);
										int num11 = (int)reg[index2];
										if (num5 != num11 && (num11 & -32769) != 0)
										{
											if ((num11 & 32768) != 0)
											{
												ref NativeArray<ushort> ptr = ref nativeArray;
												num7 = num6;
												ptr[num7] |= 1;
											}
											else
											{
												JobBuildRegions.union_find_union(arr, num6, num11);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			for (int num12 = 0; num12 < arr.Length; num12++)
			{
				ref NativeArray<ushort> ptr = ref nativeArray;
				int num7 = JobBuildRegions.union_find_find(arr, num12);
				ptr[num7] |= nativeArray[num12];
			}
			for (int num13 = 0; num13 < arr.Length; num13++)
			{
				int index3 = JobBuildRegions.union_find_find(arr, num13);
				if ((nativeArray[index3] & 1) != 0)
				{
					arr[index3] = -minRegionSize - 2;
				}
				if (flag && ((int)nativeArray[index3] & num2) == 0)
				{
					arr[index3] = -1;
				}
			}
			for (int num14 = 0; num14 < reg.Length; num14++)
			{
				int num15 = (int)reg[num14];
				if (num15 < length && arr[JobBuildRegions.union_find_find(arr, num15)] >= -minRegionSize - 1)
				{
					reg[num14] = 0;
				}
			}
		}

		// Token: 0x040008AF RID: 2223
		public CompactVoxelField field;

		// Token: 0x040008B0 RID: 2224
		public NativeList<ushort> distanceField;

		// Token: 0x040008B1 RID: 2225
		public int borderSize;

		// Token: 0x040008B2 RID: 2226
		public int minRegionSize;

		// Token: 0x040008B3 RID: 2227
		public NativeQueue<Int3> srcQue;

		// Token: 0x040008B4 RID: 2228
		public NativeQueue<Int3> dstQue;

		// Token: 0x040008B5 RID: 2229
		public RecastGraph.RelevantGraphSurfaceMode relevantGraphSurfaceMode;

		// Token: 0x040008B6 RID: 2230
		public NativeArray<JobBuildRegions.RelevantGraphSurfaceInfo> relevantGraphSurfaces;

		// Token: 0x040008B7 RID: 2231
		public float cellSize;

		// Token: 0x040008B8 RID: 2232
		public float cellHeight;

		// Token: 0x040008B9 RID: 2233
		public Matrix4x4 graphTransform;

		// Token: 0x040008BA RID: 2234
		public Bounds graphSpaceBounds;

		// Token: 0x020001DF RID: 479
		public struct RelevantGraphSurfaceInfo
		{
			// Token: 0x040008BB RID: 2235
			public float3 position;

			// Token: 0x040008BC RID: 2236
			public float range;
		}
	}
}

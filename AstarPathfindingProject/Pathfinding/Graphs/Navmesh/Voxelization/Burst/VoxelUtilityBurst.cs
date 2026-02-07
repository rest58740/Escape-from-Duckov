using System;
using Unity.Collections;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001E0 RID: 480
	internal static class VoxelUtilityBurst
	{
		// Token: 0x06000C6A RID: 3178 RVA: 0x0004C39C File Offset: 0x0004A59C
		public static void CalculateDistanceField(CompactVoxelField field, NativeArray<ushort> output)
		{
			int num = field.width * field.depth;
			for (int i = 0; i < num; i += field.width)
			{
				for (int j = 0; j < field.width; j++)
				{
					CompactVoxelCell compactVoxelCell = field.cells[j + i];
					int k = compactVoxelCell.index;
					int num2 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num2)
					{
						CompactVoxelSpan compactVoxelSpan = field.spans[k];
						int num3 = 0;
						int num4 = 0;
						while (num4 < 4 && (long)compactVoxelSpan.GetConnection(num4) != 63L)
						{
							num3++;
							num4++;
						}
						output[k] = ((num3 == 4) ? ushort.MaxValue : 0);
						k++;
					}
				}
			}
			for (int l = 0; l < num; l += field.width)
			{
				for (int m = 0; m < field.width; m++)
				{
					int index = m + l;
					CompactVoxelCell compactVoxelCell2 = field.cells[index];
					int n = compactVoxelCell2.index;
					int num5 = compactVoxelCell2.index + compactVoxelCell2.count;
					while (n < num5)
					{
						CompactVoxelSpan compactVoxelSpan2 = field.spans[n];
						int num6 = (int)output[n];
						if ((long)compactVoxelSpan2.GetConnection(0) != 63L)
						{
							int neighbourIndex = field.GetNeighbourIndex(index, 0);
							int index2 = field.cells[neighbourIndex].index + compactVoxelSpan2.GetConnection(0);
							num6 = math.min(num6, (int)(output[index2] + 2));
							CompactVoxelSpan compactVoxelSpan3 = field.spans[index2];
							if ((long)compactVoxelSpan3.GetConnection(3) != 63L)
							{
								int neighbourIndex2 = field.GetNeighbourIndex(neighbourIndex, 3);
								int index3 = field.cells[neighbourIndex2].index + compactVoxelSpan3.GetConnection(3);
								num6 = math.min(num6, (int)(output[index3] + 3));
							}
						}
						if ((long)compactVoxelSpan2.GetConnection(3) != 63L)
						{
							int neighbourIndex3 = field.GetNeighbourIndex(index, 3);
							int index4 = field.cells[neighbourIndex3].index + compactVoxelSpan2.GetConnection(3);
							num6 = math.min(num6, (int)(output[index4] + 2));
							CompactVoxelSpan compactVoxelSpan4 = field.spans[index4];
							if ((long)compactVoxelSpan4.GetConnection(2) != 63L)
							{
								int neighbourIndex4 = field.GetNeighbourIndex(neighbourIndex3, 2);
								int index5 = field.cells[neighbourIndex4].index + compactVoxelSpan4.GetConnection(2);
								num6 = math.min(num6, (int)(output[index5] + 3));
							}
						}
						output[n] = (ushort)num6;
						n++;
					}
				}
			}
			for (int num7 = num - field.width; num7 >= 0; num7 -= field.width)
			{
				for (int num8 = field.width - 1; num8 >= 0; num8--)
				{
					int index6 = num8 + num7;
					CompactVoxelCell compactVoxelCell3 = field.cells[index6];
					int num9 = compactVoxelCell3.index;
					int num10 = compactVoxelCell3.index + compactVoxelCell3.count;
					while (num9 < num10)
					{
						CompactVoxelSpan compactVoxelSpan5 = field.spans[num9];
						int num11 = (int)output[num9];
						if ((long)compactVoxelSpan5.GetConnection(2) != 63L)
						{
							int neighbourIndex5 = field.GetNeighbourIndex(index6, 2);
							int index7 = field.cells[neighbourIndex5].index + compactVoxelSpan5.GetConnection(2);
							num11 = math.min(num11, (int)(output[index7] + 2));
							CompactVoxelSpan compactVoxelSpan6 = field.spans[index7];
							if ((long)compactVoxelSpan6.GetConnection(1) != 63L)
							{
								int neighbourIndex6 = field.GetNeighbourIndex(neighbourIndex5, 1);
								int index8 = field.cells[neighbourIndex6].index + compactVoxelSpan6.GetConnection(1);
								num11 = math.min(num11, (int)(output[index8] + 3));
							}
						}
						if ((long)compactVoxelSpan5.GetConnection(1) != 63L)
						{
							int neighbourIndex7 = field.GetNeighbourIndex(index6, 1);
							int index9 = field.cells[neighbourIndex7].index + compactVoxelSpan5.GetConnection(1);
							num11 = math.min(num11, (int)(output[index9] + 2));
							CompactVoxelSpan compactVoxelSpan7 = field.spans[index9];
							if ((long)compactVoxelSpan7.GetConnection(0) != 63L)
							{
								int neighbourIndex8 = field.GetNeighbourIndex(neighbourIndex7, 0);
								int index10 = field.cells[neighbourIndex8].index + compactVoxelSpan7.GetConnection(0);
								num11 = math.min(num11, (int)(output[index10] + 3));
							}
						}
						output[num9] = (ushort)num11;
						num9++;
					}
				}
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0004C864 File Offset: 0x0004AA64
		public static void BoxBlur(CompactVoxelField field, NativeArray<ushort> src, NativeArray<ushort> dst)
		{
			ushort num = 20;
			for (int i = field.width * field.depth - field.width; i >= 0; i -= field.width)
			{
				for (int j = field.width - 1; j >= 0; j--)
				{
					int index = j + i;
					CompactVoxelCell compactVoxelCell = field.cells[index];
					int k = compactVoxelCell.index;
					int num2 = compactVoxelCell.index + compactVoxelCell.count;
					while (k < num2)
					{
						CompactVoxelSpan compactVoxelSpan = field.spans[k];
						ushort num3 = src[k];
						if (num3 < num)
						{
							dst[k] = num3;
						}
						else
						{
							int num4 = (int)num3;
							for (int l = 0; l < 4; l++)
							{
								if ((long)compactVoxelSpan.GetConnection(l) != 63L)
								{
									int neighbourIndex = field.GetNeighbourIndex(index, l);
									int index2 = field.cells[neighbourIndex].index + compactVoxelSpan.GetConnection(l);
									num4 += (int)src[index2];
									CompactVoxelSpan compactVoxelSpan2 = field.spans[index2];
									int num5 = l + 1 & 3;
									if ((long)compactVoxelSpan2.GetConnection(num5) != 63L)
									{
										int neighbourIndex2 = field.GetNeighbourIndex(neighbourIndex, num5);
										int index3 = field.cells[neighbourIndex2].index + compactVoxelSpan2.GetConnection(num5);
										num4 += (int)src[index3];
									}
									else
									{
										num4 += (int)num3;
									}
								}
								else
								{
									num4 += (int)(num3 * 2);
								}
							}
							dst[k] = (ushort)((float)(num4 + 5) / 9f);
						}
						k++;
					}
				}
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0004CA17 File Offset: 0x0004AC17
		// Note: this type is marked as 'beforefieldinit'.
		static VoxelUtilityBurst()
		{
			int[] array = new int[4];
			array[0] = -1;
			array[2] = 1;
			VoxelUtilityBurst.DX = array;
			VoxelUtilityBurst.DZ = new int[]
			{
				0,
				1,
				0,
				-1
			};
		}

		// Token: 0x040008BD RID: 2237
		public const int TagRegMask = 16383;

		// Token: 0x040008BE RID: 2238
		public const int TagReg = 16384;

		// Token: 0x040008BF RID: 2239
		public const ushort BorderReg = 32768;

		// Token: 0x040008C0 RID: 2240
		public const int RC_BORDER_VERTEX = 65536;

		// Token: 0x040008C1 RID: 2241
		public const int RC_AREA_BORDER = 131072;

		// Token: 0x040008C2 RID: 2242
		public const int VERTEX_BUCKET_COUNT = 4096;

		// Token: 0x040008C3 RID: 2243
		public const int RC_CONTOUR_TESS_WALL_EDGES = 1;

		// Token: 0x040008C4 RID: 2244
		public const int RC_CONTOUR_TESS_AREA_EDGES = 2;

		// Token: 0x040008C5 RID: 2245
		public const int RC_CONTOUR_TESS_TILE_EDGES = 4;

		// Token: 0x040008C6 RID: 2246
		public const int ContourRegMask = 65535;

		// Token: 0x040008C7 RID: 2247
		public static readonly int[] DX;

		// Token: 0x040008C8 RID: 2248
		public static readonly int[] DZ;
	}
}

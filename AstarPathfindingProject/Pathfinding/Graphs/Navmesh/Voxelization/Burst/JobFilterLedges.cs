using System;
using Pathfinding.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;

namespace Pathfinding.Graphs.Navmesh.Voxelization.Burst
{
	// Token: 0x020001DD RID: 477
	[BurstCompile(CompileSynchronously = true)]
	internal struct JobFilterLedges : IJob
	{
		// Token: 0x06000C63 RID: 3171 RVA: 0x0004B0FC File Offset: 0x000492FC
		public void Execute()
		{
			UnsafeSpan<LinkedVoxelSpan> unsafeSpan = this.field.linkedSpans.AsUnsafeSpan<LinkedVoxelSpan>();
			int num = this.field.width * this.field.depth;
			int width = this.field.width;
			int i = 0;
			int num2 = 0;
			while (i < num)
			{
				for (int j = 0; j < width; j++)
				{
					if (unsafeSpan[j + i].bottom != 4294967295U)
					{
						for (int num3 = j + i; num3 != -1; num3 = unsafeSpan[num3].next)
						{
							if (unsafeSpan[num3].area != 0)
							{
								if (j == 0 || i == 0 || i == num - width || j == width - 1)
								{
									unsafeSpan[num3].area = 0;
								}
								else
								{
									int top = (int)unsafeSpan[num3].top;
									int num4 = (int)((unsafeSpan[num3].next != -1) ? unsafeSpan[unsafeSpan[num3].next].bottom : 65536U);
									int num5 = 65536;
									int num6 = (int)unsafeSpan[num3].top;
									int num7 = num6;
									for (int k = 0; k < 4; k++)
									{
										int num8 = j + VoxelUtilityBurst.DX[k];
										int num9 = i + VoxelUtilityBurst.DZ[k] * width;
										int num10 = num8 + num9;
										int num11 = -this.voxelWalkableClimb;
										int y = (int)((unsafeSpan[num10].bottom != uint.MaxValue) ? unsafeSpan[num10].bottom : 65536U);
										if ((long)(math.min(num4, y) - math.max(top, num11)) > (long)((ulong)this.voxelWalkableHeight))
										{
											num5 = math.min(num5, num11 - top);
										}
										if (unsafeSpan[num10].bottom != 4294967295U)
										{
											for (int num12 = num10; num12 != -1; num12 = unsafeSpan[num12].next)
											{
												ref LinkedVoxelSpan ptr = ref unsafeSpan[num12];
												num11 = (int)ptr.top;
												if ((long)num11 > (long)num4 - (long)((ulong)this.voxelWalkableHeight))
												{
													break;
												}
												y = (int)((ptr.next != -1) ? unsafeSpan[ptr.next].bottom : 65536U);
												if ((long)(math.min(num4, y) - math.max(top, num11)) > (long)((ulong)this.voxelWalkableHeight))
												{
													num5 = math.min(num5, num11 - top);
													if (math.abs(num11 - top) <= this.voxelWalkableClimb)
													{
														if (num11 < num6)
														{
															num6 = num11;
														}
														if (num11 > num7)
														{
															num7 = num11;
														}
													}
												}
											}
										}
									}
									if (num5 < -this.voxelWalkableClimb || num7 - num6 > this.voxelWalkableClimb)
									{
										unsafeSpan[num3].area = 0;
									}
								}
							}
						}
					}
				}
				i += width;
				num2++;
			}
		}

		// Token: 0x040008AA RID: 2218
		public LinkedVoxelField field;

		// Token: 0x040008AB RID: 2219
		public uint voxelWalkableHeight;

		// Token: 0x040008AC RID: 2220
		public int voxelWalkableClimb;

		// Token: 0x040008AD RID: 2221
		public float cellSize;

		// Token: 0x040008AE RID: 2222
		public float cellHeight;
	}
}

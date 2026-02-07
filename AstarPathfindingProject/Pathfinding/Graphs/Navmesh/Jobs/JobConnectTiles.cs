using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh.Jobs
{
	// Token: 0x020001E9 RID: 489
	public struct JobConnectTiles : IJob
	{
		// Token: 0x06000C7C RID: 3196 RVA: 0x0004D960 File Offset: 0x0004BB60
		public static JobHandle ScheduleBatch(GCHandle tilesHandle, JobHandle dependency, IntRect tileRect, Vector2 tileWorldSize, float maxTileConnectionEdgeDistance)
		{
			int num = Mathf.Max(1, JobsUtility.JobWorkerCount);
			NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			for (int i = 0; i <= 1; i++)
			{
				for (int j = 0; j <= 1; j++)
				{
					for (int k = 0; k < num; k++)
					{
						jobs[k] = new JobConnectTiles
						{
							tiles = tilesHandle,
							tileRect = tileRect,
							tileWorldSize = tileWorldSize,
							coordinateSum = i,
							direction = j,
							maxTileConnectionEdgeDistance = maxTileConnectionEdgeDistance,
							zOffset = k,
							zStride = num
						}.Schedule(dependency);
					}
					dependency = JobHandle.CombineDependencies(jobs);
				}
			}
			return dependency;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0004DA18 File Offset: 0x0004BC18
		public static JobHandle ScheduleRecalculateBorders(GCHandle tilesHandle, JobHandle dependency, IntRect tileRect, IntRect innerRect, Vector2 tileWorldSize, float maxTileConnectionEdgeDistance)
		{
			int width = innerRect.Width;
			int height = innerRect.Height;
			NativeArray<JobHandle> jobs = new NativeArray<JobHandle>(2 * width + 2 * math.max(0, height - 2), Allocator.Temp, NativeArrayOptions.ClearMemory);
			int num = 0;
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					if (j == 0 || i == 0 || j == width - 1 || i == height - 1)
					{
						int num2 = innerRect.xmin + j;
						int num3 = innerRect.ymin + i;
						JobHandle jobHandle = dependency;
						for (int k = 0; k < 4; k++)
						{
							int num4 = num2 + ((k == 0) ? 1 : ((k == 1) ? -1 : 0));
							int num5 = num3 + ((k == 2) ? 1 : ((k == 3) ? -1 : 0));
							if (!innerRect.Contains(num4, num5) && tileRect.Contains(num4, num5))
							{
								jobHandle = new JobConnectTilesSingle
								{
									tiles = tilesHandle,
									tileIndex1 = num2 + num3 * tileRect.Width,
									tileIndex2 = num4 + num5 * tileRect.Width,
									tileWorldSize = tileWorldSize,
									maxTileConnectionEdgeDistance = maxTileConnectionEdgeDistance
								}.Schedule(jobHandle);
							}
						}
						jobs[num++] = jobHandle;
					}
				}
			}
			return JobHandle.CombineDependencies(jobs);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0004DB74 File Offset: 0x0004BD74
		public void Execute()
		{
			NavmeshTile[] array = (NavmeshTile[])this.tiles.Target;
			int height = this.tileRect.Height;
			int width = this.tileRect.Width;
			for (int i = this.zOffset; i < height; i += this.zStride)
			{
				for (int j = 0; j < width; j++)
				{
					if ((j + i) % 2 == this.coordinateSum)
					{
						int num = j + i * width;
						int num2;
						if (this.direction == 0 && j < width - 1)
						{
							num2 = j + 1 + i * width;
						}
						else
						{
							if (this.direction != 1 || i >= height - 1)
							{
								goto IL_AD;
							}
							num2 = j + (i + 1) * width;
						}
						NavmeshBase.ConnectTiles(array[num], array[num2], this.tileWorldSize.x, this.tileWorldSize.y, this.maxTileConnectionEdgeDistance);
					}
					IL_AD:;
				}
			}
		}

		// Token: 0x04000909 RID: 2313
		public GCHandle tiles;

		// Token: 0x0400090A RID: 2314
		public int coordinateSum;

		// Token: 0x0400090B RID: 2315
		public int direction;

		// Token: 0x0400090C RID: 2316
		public int zOffset;

		// Token: 0x0400090D RID: 2317
		public int zStride;

		// Token: 0x0400090E RID: 2318
		private Vector2 tileWorldSize;

		// Token: 0x0400090F RID: 2319
		private IntRect tileRect;

		// Token: 0x04000910 RID: 2320
		public float maxTileConnectionEdgeDistance;

		// Token: 0x04000911 RID: 2321
		private static readonly ProfilerMarker ConnectTilesMarker = new ProfilerMarker("ConnectTiles");
	}
}

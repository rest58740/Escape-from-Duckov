using System;
using Unity.Collections;
using Unity.Mathematics;

namespace FOW
{
	// Token: 0x0200000F RID: 15
	public class SightIteration
	{
		// Token: 0x06000084 RID: 132 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public void InitializeStruct(int NumSteps)
		{
			this.RayAngles = new NativeArray<float>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Hits = new NativeArray<bool>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Distances = new NativeArray<float>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Points = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Directions = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.Normals = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.NextPoints = new NativeArray<float2>(NumSteps, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005D24 File Offset: 0x00003F24
		public void DisposeStruct()
		{
			this.RayAngles.Dispose();
			this.Distances.Dispose();
			this.Hits.Dispose();
			this.Points.Dispose();
			this.Directions.Dispose();
			this.Normals.Dispose();
			this.NextPoints.Dispose();
		}

		// Token: 0x040000C5 RID: 197
		public NativeArray<float> RayAngles;

		// Token: 0x040000C6 RID: 198
		public NativeArray<bool> Hits;

		// Token: 0x040000C7 RID: 199
		public NativeArray<float> Distances;

		// Token: 0x040000C8 RID: 200
		public NativeArray<float2> Points;

		// Token: 0x040000C9 RID: 201
		public NativeArray<float2> Directions;

		// Token: 0x040000CA RID: 202
		public NativeArray<float2> Normals;

		// Token: 0x040000CB RID: 203
		public NativeArray<float2> NextPoints;

		// Token: 0x040000CC RID: 204
		public SightIteration[] NextIterations;
	}
}

using System;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Drawing.Examples
{
	// Token: 0x02000066 RID: 102
	public class BurstExample : MonoBehaviour
	{
		// Token: 0x060003DC RID: 988 RVA: 0x000131B0 File Offset: 0x000113B0
		public void Update()
		{
			CommandBuilder builder = DrawingManager.GetBuilder(true);
			JobHandle dependency = new BurstExample.DrawingJob
			{
				builder = builder,
				offset = new float2(Time.time * 0.2f, Time.time * 0.2f)
			}.Schedule(default(JobHandle));
			builder.DisposeAfter(dependency, AllowedDelay.EndOfFrame);
			dependency.Complete();
		}

		// Token: 0x02000067 RID: 103
		[BurstCompile]
		private struct DrawingJob : IJob
		{
			// Token: 0x060003DE RID: 990 RVA: 0x00013218 File Offset: 0x00011418
			private Color Colormap(float x)
			{
				float r = math.clamp(2.6666667f * x, 0f, 1f);
				float g = math.clamp(2.6666667f * x - 1f, 0f, 1f);
				float b = math.clamp(4f * x - 3f, 0f, 1f);
				return new Color(r, g, b, 1f);
			}

			// Token: 0x060003DF RID: 991 RVA: 0x00013284 File Offset: 0x00011484
			public void Execute(int index)
			{
				int num = index / 100;
				int num2 = index % 100;
				float num3 = Mathf.PerlinNoise((float)num * 0.05f + this.offset.x, (float)num2 * 0.05f + this.offset.y);
				Bounds bounds = new Bounds(new float3((float)num, 0f, (float)num2), new float3(1f, 14f * num3, 1f));
				this.builder.SolidBox(bounds, this.Colormap(num3));
			}

			// Token: 0x060003E0 RID: 992 RVA: 0x00013314 File Offset: 0x00011514
			public void Execute()
			{
				for (int i = 0; i < 10000; i++)
				{
					this.Execute(i);
				}
			}

			// Token: 0x0400019E RID: 414
			public float2 offset;

			// Token: 0x0400019F RID: 415
			public CommandBuilder builder;
		}
	}
}

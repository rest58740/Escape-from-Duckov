using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace UI_Spline_Renderer
{
	// Token: 0x02000007 RID: 7
	internal struct NativeColorGradient : IDisposable
	{
		// Token: 0x06000019 RID: 25 RVA: 0x0000266C File Offset: 0x0000086C
		public Color Evaluate(float t)
		{
			int num = -1;
			for (int i = 0; i < this.alphaKeyFrames.Length; i++)
			{
				if (t <= this.alphaKeyFrames[i].y)
				{
					num = i;
					break;
				}
			}
			int num2 = -1;
			for (int j = 0; j < this.colorKeyFrames.Length; j++)
			{
				if (t <= this.colorKeyFrames[j].w)
				{
					num2 = j;
					break;
				}
			}
			float x;
			if (num == -1)
			{
				x = this.alphaKeyFrames[this.alphaKeyFrames.Length - 1].x;
			}
			else if (num == 0)
			{
				x = this.alphaKeyFrames[0].x;
			}
			else
			{
				float2 @float = this.alphaKeyFrames[num - 1];
				float2 float2 = this.alphaKeyFrames[num];
				float num3 = t.Remap(0f, 1f, @float.y, float2.y);
				x = math.lerp(@float, float2, num3).x;
			}
			Color result;
			if (num2 == -1)
			{
				result = this.toColor(this.colorKeyFrames[this.colorKeyFrames.Length - 1]);
			}
			else if (num2 == 0)
			{
				result = this.toColor(this.colorKeyFrames[0]);
			}
			else
			{
				Color color = this.toColor(this.colorKeyFrames[num2 - 1]);
				Color color2 = this.toColor(this.colorKeyFrames[num2]);
				float num4 = (t - color.a) / (color2.a - color.a);
				result = Color.Lerp(color, color2, num4);
			}
			result.a = x;
			return result;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000027FC File Offset: 0x000009FC
		private Color toColor(float4 f)
		{
			return new Color(f.x, f.y, f.z, f.w);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000281B File Offset: 0x00000A1B
		public void Dispose()
		{
			this.alphaKeyFrames.Dispose();
			this.colorKeyFrames.Dispose();
		}

		// Token: 0x04000008 RID: 8
		[ReadOnly]
		public NativeArray<float2> alphaKeyFrames;

		// Token: 0x04000009 RID: 9
		[ReadOnly]
		public NativeArray<float4> colorKeyFrames;
	}
}

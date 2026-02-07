using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x0200000E RID: 14
	[ExecuteAlways]
	public class SpinningColorDiscs : ImmediateModeShapeDrawer
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00003D90 File Offset: 0x00001F90
		public override void DrawShapes(Camera cam)
		{
			using (Draw.Command(cam, RenderPassEvent.BeforeRenderingPostProcessing))
			{
				Draw.ResetAllDrawStates();
				Draw.Matrix = base.transform.localToWorldMatrix;
				for (int i = 0; i < this.discCount; i++)
				{
					float num = (float)i / (float)this.discCount;
					Color color = Color.HSVToRGB(num, 1f, 1f);
					Draw.Disc(this.GetDiscPosition(num), this.discRadius, color);
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003E24 File Offset: 0x00002024
		private Vector2 GetDiscPosition(float t)
		{
			float num = t * 6.2831855f + 6.2831855f * Time.time * 0.25f;
			return ShapesMath.AngToDir(num + Mathf.Cos(num * 2f + Time.time * 6.2831855f * 0.5f) * 0.16f);
		}

		// Token: 0x04000069 RID: 105
		[Range(3f, 32f)]
		public int discCount = 24;

		// Token: 0x0400006A RID: 106
		[Range(0f, 1f)]
		public float discRadius = 0.1f;
	}
}

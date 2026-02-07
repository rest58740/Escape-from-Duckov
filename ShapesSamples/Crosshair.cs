using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000006 RID: 6
	public class Crosshair : MonoBehaviour
	{
		// Token: 0x06000010 RID: 16 RVA: 0x00002B77 File Offset: 0x00000D77
		public void Fire()
		{
			this.fireDecayer.SetT(1f);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002B89 File Offset: 0x00000D89
		public void FireHit()
		{
			this.hitDecayer.SetT(1f);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002B9B File Offset: 0x00000D9B
		public void UpdateCrosshairDecay()
		{
			this.fireDecayer.Update();
			this.hitDecayer.Update();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002BB4 File Offset: 0x00000DB4
		public void DrawCrosshair()
		{
			Vector2[] dirs = new Vector2[]
			{
				Vector2.up,
				Vector2.right,
				Vector2.down,
				Vector2.left
			};
			Vector2[] dirs2 = new Vector2[]
			{
				(Vector2.up + Vector2.right).normalized,
				(Vector2.right + Vector2.down).normalized,
				(Vector2.down + Vector2.left).normalized,
				(Vector2.left + Vector2.up).normalized
			};
			float thickness = this.crosshairCrossThickness * Mathf.Lerp(1f, this.scaleFire, this.fireDecayer.t);
			Crosshair.<DrawCrosshair>g__DrawCross|12_0(dirs, this.crosshairCrossInnerRad, this.crosshairCrossOuterRad, thickness, this.fireDecayer.value, Color.white);
			Crosshair.<DrawCrosshair>g__DrawCross|12_0(dirs2, this.crosshairHitCrossInnerRad, this.crosshairHitCrossOuterRad, this.crosshairHitCrossThickness, this.hitDecayer.valueInv, new Color(1f, 0f, 0f, this.hitDecayer.t));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002D78 File Offset: 0x00000F78
		[CompilerGenerated]
		internal static void <DrawCrosshair>g__DrawCross|12_0(Vector2[] dirs, float radInner, float radOuter, float thickness, float radialOffset, Color color)
		{
			foreach (Vector2 a in dirs)
			{
				Vector2 v = a * (radInner + radialOffset);
				Vector2 v2 = a * (radOuter + radialOffset);
				Draw.Line(v, v2, thickness, 2, color);
			}
		}

		// Token: 0x0400002B RID: 43
		[Header("Style")]
		[Range(0f, 0.05f)]
		public float crosshairCrossInnerRad = 0.1f;

		// Token: 0x0400002C RID: 44
		[Range(0f, 0.05f)]
		public float crosshairCrossOuterRad = 0.3f;

		// Token: 0x0400002D RID: 45
		[Range(0f, 0.05f)]
		public float crosshairCrossThickness = 0.2f;

		// Token: 0x0400002E RID: 46
		[Range(0f, 0.05f)]
		public float crosshairHitCrossInnerRad = 0.1f;

		// Token: 0x0400002F RID: 47
		[Range(0f, 0.05f)]
		public float crosshairHitCrossOuterRad = 0.3f;

		// Token: 0x04000030 RID: 48
		[Range(0f, 0.05f)]
		public float crosshairHitCrossThickness = 0.2f;

		// Token: 0x04000031 RID: 49
		[Header("Animation")]
		[Range(0f, 1f)]
		public float scaleFire = 0.1f;

		// Token: 0x04000032 RID: 50
		public Decayer fireDecayer = new Decayer();

		// Token: 0x04000033 RID: 51
		public Decayer hitDecayer = new Decayer();
	}
}

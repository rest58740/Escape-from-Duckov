using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000003 RID: 3
	public class AmmoBar : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		private Vector2 GetBulletEjectPos(Vector2 origin, float t)
		{
			Vector2 a = new Vector2(this.bulletEjectX.Evaluate(t), this.bulletEjectY.Evaluate(t));
			return origin + a * this.bulletEjectScale;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020FE File Offset: 0x000002FE
		public bool HasBulletsLeft
		{
			get
			{
				return this.bullets > 0;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000210C File Offset: 0x0000030C
		public void Fire()
		{
			float[] array = this.bulletFireTimes;
			int num = this.bullets - 1;
			this.bullets = num;
			array[num] = Time.time;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002136 File Offset: 0x00000336
		public void Reload()
		{
			this.bullets = this.totalBullets;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002144 File Offset: 0x00000344
		private void Awake()
		{
			this.bulletFireTimes = new float[this.totalBullets];
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002158 File Offset: 0x00000358
		public void DrawBar(FpsController fpsController, float barRadius)
		{
			float ammoBarThickness = fpsController.ammoBarThickness;
			float ammoBarOutlineThickness = fpsController.ammoBarOutlineThickness;
			float num = -fpsController.ammoBarAngularSpanRad / 2f;
			float num2 = fpsController.ammoBarAngularSpanRad / 2f;
			Draw.LineEndCaps = 2;
			float num3 = (barRadius - ammoBarThickness / 2f) * fpsController.ammoBarAngularSpanRad / (float)this.totalBullets * this.bulletThicknessScale;
			for (int i = 0; i < this.totalBullets; i++)
			{
				float t = (float)i / ((float)this.totalBullets - 1f);
				Vector2 a = ShapesMath.AngToDir(Mathf.Lerp(num, num2, t));
				Vector2 vector = a * barRadius;
				Vector2 vector2 = a * (ammoBarThickness / 2f - ammoBarOutlineThickness * 1.5f);
				float a2 = 1f;
				if (i >= this.bullets && Application.isPlaying)
				{
					float num4 = Time.time - this.bulletFireTimes[i];
					float num5 = Mathf.Clamp01(num4 / this.bulletDisappearTime);
					a2 = 1f - num5;
					vector = this.GetBulletEjectPos(vector, num5);
					float num6 = num4 * (this.bulletEjectAngSpeed + Mathf.Cos((float)i * 92372.8f) * this.ejectRotSpeedVariance);
					vector2 = ShapesMath.Rotate(vector2, num6);
				}
				Vector2 v = vector + vector2;
				Vector2 v2 = vector - vector2;
				Draw.Line(v, v2, num3, new Color(1f, 1f, 1f, a2));
			}
			FpsController.DrawRoundedArcOutline(Vector2.zero, barRadius, ammoBarThickness, ammoBarOutlineThickness, num, num2);
		}

		// Token: 0x04000001 RID: 1
		public int totalBullets = 20;

		// Token: 0x04000002 RID: 2
		public int bullets = 15;

		// Token: 0x04000003 RID: 3
		[Header("Style")]
		[Range(0f, 1f)]
		public float bulletThicknessScale = 1f;

		// Token: 0x04000004 RID: 4
		[Range(0f, 0.5f)]
		public float bulletEjectScale = 0.5f;

		// Token: 0x04000005 RID: 5
		[Header("Animation")]
		public float bulletDisappearTime = 1f;

		// Token: 0x04000006 RID: 6
		[Range(0f, 6.2831855f)]
		public float bulletEjectAngSpeed = 0.5f;

		// Token: 0x04000007 RID: 7
		[Range(0f, 6.2831855f)]
		public float ejectRotSpeedVariance = 1f;

		// Token: 0x04000008 RID: 8
		public AnimationCurve bulletEjectX = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x04000009 RID: 9
		public AnimationCurve bulletEjectY = AnimationCurve.Constant(0f, 1f, 0f);

		// Token: 0x0400000A RID: 10
		private float[] bulletFireTimes;
	}
}

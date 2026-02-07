using System;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000004 RID: 4
	public class ChargeBar : MonoBehaviour
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002368 File Offset: 0x00000568
		public void UpdateCharge()
		{
			if (this.isCharging)
			{
				this.charge += this.chargeSpeed * Time.deltaTime;
			}
			else
			{
				this.charge -= this.chargeDecaySpeed * Time.deltaTime;
			}
			this.charge = Mathf.Clamp01(this.charge);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000023C4 File Offset: 0x000005C4
		public void DrawBar(FpsController fpsController, float barRadius)
		{
			float ammoBarThickness = fpsController.ammoBarThickness;
			float ammoBarOutlineThickness = fpsController.ammoBarOutlineThickness;
			float num = -fpsController.ammoBarAngularSpanRad / 2f;
			float num2 = fpsController.ammoBarAngularSpanRad / 2f;
			float num3 = num + 3.1415927f;
			float num4 = num2 + 3.1415927f;
			float d = barRadius + ammoBarThickness / 2f;
			float num5 = this.chargeFillCurve.Evaluate(this.charge);
			float amp = this.animChargeShakeMagnitude.Evaluate(num5) * this.chargeShakeMagnitude;
			Vector2 shake = fpsController.GetShake(this.chargeShakeSpeed, amp);
			float num6 = Mathf.Lerp(num4, num3, num5);
			Color color = this.chargeFillGradient.Evaluate(num5);
			Draw.Arc(shake, fpsController.ammoBarRadius, ammoBarThickness, num4, num6, color);
			Vector2 v = shake + ShapesMath.AngToDir(num6) * barRadius;
			Draw.Disc(shake + ShapesMath.AngToDir(num4) * barRadius, ammoBarThickness / 2f, color);
			Draw.LineEndCaps = 0;
			for (int i = 0; i < 7; i++)
			{
				float num7 = (float)i / 6f;
				float num8 = Mathf.Lerp(num4, num3, num7);
				Vector2 a = ShapesMath.AngToDir(num8);
				Vector2 vector = shake + a * d;
				bool flag = i % 3 == 0;
				Vector2 v2 = vector + a * (flag ? this.tickSizeLorge : this.tickSizeSmol);
				Draw.Line(vector, v2, this.tickTickness, this.tickColor);
				float num9 = num7 - num5;
				float num10 = (num9 < 0f) ? this.fontGrowRangePrev : this.fontGrowRangeNext;
				float num11 = 1f - ShapesMath.SmoothCos01(Mathf.Clamp01(Mathf.Abs(num9) / num10));
				Draw.FontSize = ShapesMath.Eerp(this.fontSize, this.fontSizeLorge, num11);
				Vector2 v3 = vector + a * this.percentLabelOffset;
				string text = Mathf.RoundToInt(num7 * 100f).ToString() + "%";
				Quaternion quaternion = Quaternion.Euler(0f, 0f, (num8 + 3.1415927f) * 57.29578f);
				Draw.Text(v3, quaternion, text, 5);
			}
			Draw.Disc(v, ammoBarThickness / 2f + ammoBarOutlineThickness / 2f);
			Draw.Disc(v, ammoBarThickness / 2f - ammoBarOutlineThickness / 2f, color);
			FpsController.DrawRoundedArcOutline(shake, barRadius, ammoBarThickness, ammoBarOutlineThickness, num3, num4);
			Draw.LineEndCaps = 2;
			Draw.BlendMode = 2;
			Draw.Disc(v, ammoBarThickness * 2f, DiscColors.Radial(color, Color.clear));
			Draw.BlendMode = 1;
		}

		// Token: 0x0400000B RID: 11
		[Header("Gameplay")]
		[SerializeField]
		private float chargeSpeed = 1f;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private float chargeDecaySpeed = 1f;

		// Token: 0x0400000D RID: 13
		[NonSerialized]
		public bool isCharging;

		// Token: 0x0400000E RID: 14
		private float charge;

		// Token: 0x0400000F RID: 15
		[Header("Style")]
		public Color tickColor = Color.white;

		// Token: 0x04000010 RID: 16
		public Gradient chargeFillGradient;

		// Token: 0x04000011 RID: 17
		[Range(0f, 0.1f)]
		public float tickSizeSmol = 0.1f;

		// Token: 0x04000012 RID: 18
		[Range(0f, 0.1f)]
		public float tickSizeLorge = 0.1f;

		// Token: 0x04000013 RID: 19
		[Range(0f, 0.05f)]
		public float tickTickness;

		// Token: 0x04000014 RID: 20
		[Range(0f, 0.5f)]
		public float fontSize = 0.1f;

		// Token: 0x04000015 RID: 21
		[Range(0f, 0.5f)]
		public float fontSizeLorge = 0.1f;

		// Token: 0x04000016 RID: 22
		[Range(0f, 0.1f)]
		public float percentLabelOffset = 0.1f;

		// Token: 0x04000017 RID: 23
		[Range(0f, 0.4f)]
		public float fontGrowRangePrev = 0.1f;

		// Token: 0x04000018 RID: 24
		[Range(0f, 0.4f)]
		public float fontGrowRangeNext = 0.1f;

		// Token: 0x04000019 RID: 25
		[Header("Animation")]
		public AnimationCurve chargeFillCurve;

		// Token: 0x0400001A RID: 26
		public AnimationCurve animChargeShakeMagnitude = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		// Token: 0x0400001B RID: 27
		[Range(0f, 0.05f)]
		public float chargeShakeMagnitude = 0.1f;

		// Token: 0x0400001C RID: 28
		public float chargeShakeSpeed = 1f;
	}
}

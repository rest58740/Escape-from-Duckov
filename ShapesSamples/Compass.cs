using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000005 RID: 5
	public class Compass : MonoBehaviour
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002748 File Offset: 0x00000948
		public void DrawCompass(Vector3 worldDir)
		{
			Compass.<>c__DisplayClass14_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.compArcOrigin = this.position + Vector2.down * this.bendRadius;
			CS$<>8__locals1.angUiMin = 1.5707964f - this.width / 2f / this.bendRadius;
			CS$<>8__locals1.angUiMax = 1.5707964f + this.width / 2f / this.bendRadius;
			float num = ShapesMath.DirToAng(new Vector2(worldDir.x, worldDir.z).normalized);
			CS$<>8__locals1.angWorldMin = num + this.fieldOfView / 2f;
			CS$<>8__locals1.angWorldMax = num - this.fieldOfView / 2f;
			Vector2 v = CS$<>8__locals1.compArcOrigin + Vector2.up * this.bendRadius + this.lookAngLabelOffset * 0.1f;
			string text = Mathf.RoundToInt(-num * 57.29578f + 180f).ToString() + "°";
			Draw.LineEndCaps = 1;
			Draw.Thickness = this.lineThickness;
			Draw.Arc(CS$<>8__locals1.compArcOrigin, this.bendRadius, this.lineThickness, CS$<>8__locals1.angUiMin, CS$<>8__locals1.angUiMax, 1);
			Draw.FontSize = this.fontSizeLookLabel;
			Draw.Text(v, text, 4);
			Draw.RegularPolygon(CS$<>8__locals1.compArcOrigin + Vector2.up * (this.bendRadius + 0.01f), 3, this.triangleNootSize, -1.5707964f);
			int num2 = (this.ticksPerQuarterTurn - 1) * 4;
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)i / (float)num2;
				float num4 = 6.2831855f * num3;
				bool flag = i % (num2 / 4) == 0;
				string label = null;
				if (flag)
				{
					int num5 = Mathf.RoundToInt((1f - num3) * 4f);
					label = this.directionLabels[num5 % 4];
				}
				float num6 = ShapesMath.InverseLerpAngleRad(CS$<>8__locals1.angWorldMax, CS$<>8__locals1.angWorldMin, num4);
				if (num6 < 1f && num6 > 0f)
				{
					this.<DrawCompass>g__DrawTick|14_0(num4, flag ? 0.8f : 0.5f, label, ref CS$<>8__locals1);
				}
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002A4C File Offset: 0x00000C4C
		[CompilerGenerated]
		private void <DrawCompass>g__DrawTick|14_0(float worldAng, float size, string label = null, ref Compass.<>c__DisplayClass14_0 A_4)
		{
			float num = ShapesMath.InverseLerpAngleRad(A_4.angWorldMax, A_4.angWorldMin, worldAng);
			float num2 = Mathf.Lerp(A_4.angUiMin, A_4.angUiMax, num);
			Vector2 a = ShapesMath.AngToDir(num2);
			Vector2 v = A_4.compArcOrigin + a * this.bendRadius;
			Vector2 vector = A_4.compArcOrigin + a * (this.bendRadius - size * this.tickSize);
			float a2 = Mathf.InverseLerp(0f, this.tickEdgeFadeFraction, 1f - Mathf.Abs(num * 2f - 1f));
			Draw.Line(v, vector, 0, new Color(1f, 1f, 1f, a2));
			if (label != null)
			{
				Draw.FontSize = this.fontSizeTickLabel;
				Quaternion quaternion = Quaternion.Euler(0f, 0f, (num2 - 1.5707964f) * 57.29578f);
				Draw.Text(vector - a * this.tickLabelOffset, quaternion, label, 4, new Color(1f, 1f, 1f, a2));
			}
		}

		// Token: 0x0400001D RID: 29
		public Vector2 position;

		// Token: 0x0400001E RID: 30
		public float width = 1f;

		// Token: 0x0400001F RID: 31
		[Range(0f, 0.01f)]
		public float lineThickness = 0.1f;

		// Token: 0x04000020 RID: 32
		[Range(0.1f, 2f)]
		public float bendRadius = 1f;

		// Token: 0x04000021 RID: 33
		[Range(0.05f, 3.0787609f)]
		public float fieldOfView = 1.5707964f;

		// Token: 0x04000022 RID: 34
		[Header("Ticks")]
		public int ticksPerQuarterTurn = 12;

		// Token: 0x04000023 RID: 35
		[Range(0f, 0.2f)]
		public float tickSize = 0.1f;

		// Token: 0x04000024 RID: 36
		[Range(0f, 1f)]
		public float tickEdgeFadeFraction = 0.1f;

		// Token: 0x04000025 RID: 37
		[Range(0.01f, 0.26f)]
		public float fontSizeTickLabel = 1f;

		// Token: 0x04000026 RID: 38
		[Range(0f, 0.1f)]
		public float tickLabelOffset = 0.01f;

		// Token: 0x04000027 RID: 39
		[Header("Degree Marker")]
		[Range(0.01f, 0.26f)]
		public float fontSizeLookLabel = 1f;

		// Token: 0x04000028 RID: 40
		public Vector2 lookAngLabelOffset;

		// Token: 0x04000029 RID: 41
		[Range(0f, 0.05f)]
		public float triangleNootSize = 0.1f;

		// Token: 0x0400002A RID: 42
		private string[] directionLabels = new string[]
		{
			"S",
			"W",
			"N",
			"E"
		};
	}
}

using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Shapes
{
	// Token: 0x0200000B RID: 11
	[ExecuteAlways]
	public class IMColorPickerRenderer : ImmediateModeShapeDrawer
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002D RID: 45 RVA: 0x0000364E File Offset: 0x0000184E
		public Color CurrentPureColor
		{
			get
			{
				return Color.HSVToRGB(this.hue, 1f, 1f);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003665 File Offset: 0x00001865
		public Color CurrentColor
		{
			get
			{
				return Color.HSVToRGB(this.hue, this.saturation, this.value);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000367E File Offset: 0x0000187E
		public float QuadScale
		{
			get
			{
				return (1f - this.hueStripThickness / 2f - this.quadMargin) / Mathf.Sqrt(2f);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000036A4 File Offset: 0x000018A4
		public Rect QuadRect
		{
			get
			{
				return new Rect(default(Vector2), Vector2.one * this.QuadScale * 2f)
				{
					center = default(Vector2)
				};
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000036EC File Offset: 0x000018EC
		public float HueStripRadiusOuter
		{
			get
			{
				return 1f + this.hueStripThickness / 2f + this.outline;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003707 File Offset: 0x00001907
		public float HueStripRadiusInner
		{
			get
			{
				return 1f - this.hueStripThickness / 2f - this.outline;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003722 File Offset: 0x00001922
		public static Vector2 HueToVector(float hue)
		{
			return ShapesMath.AngToDir(hue * 6.2831855f);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003730 File Offset: 0x00001930
		public static float VectorToHue(Vector2 v)
		{
			return ShapesMath.Frac(ShapesMath.DirToAng(v) / 6.2831855f);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003743 File Offset: 0x00001943
		public override void OnEnable()
		{
			base.OnEnable();
			this.ConstructHueStripPolyline();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003751 File Offset: 0x00001951
		public override void OnDisable()
		{
			base.OnDisable();
			this.hueStripPath.Dispose();
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003764 File Offset: 0x00001964
		public override void DrawShapes(Camera cam)
		{
			using (Draw.Command(cam, RenderPassEvent.BeforeRenderingPostProcessing))
			{
				Draw.Matrix = base.transform.localToWorldMatrix;
				Draw.Ring(Vector3.zero, 1f, this.hueStripThickness + this.outline, Color.black);
				Draw.PolylineJoins = 0;
				Draw.PolylineGeometry = 0;
				Draw.Polyline(this.hueStripPath, true, this.hueStripThickness);
				float quadScale = this.QuadScale;
				Draw.Rectangle(Vector3.zero, Vector2.one * (quadScale * 2f + this.outline), Color.black);
				using (Draw.MatrixScope)
				{
					Draw.Scale(quadScale);
					Draw.Quad(new Vector2(-1f, -1f), new Vector2(1f, -1f), new Vector2(1f, 1f), new Vector2(-1f, 1f), Color.black, Color.black, this.CurrentPureColor, Color.white);
				}
				Rect rect = new Rect(-this.labelSize.x / 2f, -quadScale - this.labelSize.y, this.labelSize.x, this.labelSize.y);
				Draw.Rectangle(rect, 0.1f, Color.black);
				string text = "#" + ColorUtility.ToHtmlStringRGB(this.CurrentColor);
				Draw.FontSize = this.labelSize.y * 8.5f;
				Draw.TextAlign = 4;
				Draw.TextRect(rect, text);
				float num = this.hueStripThickness / 2f * this.hueDotScale;
				Vector2 v = IMColorPickerRenderer.HueToVector(this.hue);
				Draw.Disc(v, num + this.outline / 2f, Color.black);
				Draw.Disc(v, num, this.CurrentPureColor);
				Vector2 v2 = ShapesMath.Lerp(this.QuadRect, new Vector2(this.saturation, this.value));
				Draw.Disc(v2, num + this.outline / 2f, Color.black);
				Draw.Disc(v2, num, this.CurrentColor);
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000039F8 File Offset: 0x00001BF8
		private void ConstructHueStripPolyline()
		{
			this.hueStripPath = new PolylinePath();
			for (int i = 0; i < 100; i++)
			{
				float h = (float)i / 100f;
				Color color = Color.HSVToRGB(h, 1f, 1f);
				Vector3 vector = IMColorPickerRenderer.HueToVector(h);
				this.hueStripPath.AddPoint(vector, color);
			}
		}

		// Token: 0x04000051 RID: 81
		[Header("Color value")]
		[Range(0f, 1f)]
		public float hue;

		// Token: 0x04000052 RID: 82
		[Range(0f, 1f)]
		public float saturation = 1f;

		// Token: 0x04000053 RID: 83
		[Range(0f, 1f)]
		public float value = 1f;

		// Token: 0x04000054 RID: 84
		[Header("Styling")]
		[Range(0f, 0.3f)]
		public float hueStripThickness;

		// Token: 0x04000055 RID: 85
		[Range(0f, 0.1f)]
		public float outline;

		// Token: 0x04000056 RID: 86
		[Range(0f, 0.1f)]
		public float quadMargin;

		// Token: 0x04000057 RID: 87
		[Range(0f, 1.5f)]
		public float hueDotScale;

		// Token: 0x04000058 RID: 88
		public Vector2 labelSize;

		// Token: 0x04000059 RID: 89
		private PolylinePath hueStripPath;
	}
}

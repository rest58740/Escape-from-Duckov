using System;
using System.Collections.Generic;
using System.Linq;
using JoshH.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace JoshH.UI
{
	// Token: 0x02000050 RID: 80
	[AddComponentMenu("UI/Effects/UI Gradient")]
	[RequireComponent(typeof(RectTransform))]
	public class UIGradient : BaseMeshEffect
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0000BF32 File Offset: 0x0000A132
		protected RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = (base.transform as RectTransform);
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000BF59 File Offset: 0x0000A159
		// (set) Token: 0x060002EA RID: 746 RVA: 0x0000BF61 File Offset: 0x0000A161
		public UIGradient.UIGradientBlendMode BlendMode
		{
			get
			{
				return this.blendMode;
			}
			set
			{
				this.blendMode = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002EB RID: 747 RVA: 0x0000BF70 File Offset: 0x0000A170
		// (set) Token: 0x060002EC RID: 748 RVA: 0x0000BF78 File Offset: 0x0000A178
		public float Intensity
		{
			get
			{
				return this.intensity;
			}
			set
			{
				this.intensity = Mathf.Clamp01(value);
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002ED RID: 749 RVA: 0x0000BF8C File Offset: 0x0000A18C
		// (set) Token: 0x060002EE RID: 750 RVA: 0x0000BF94 File Offset: 0x0000A194
		public UIGradient.UIGradientType GradientType
		{
			get
			{
				return this.gradientType;
			}
			set
			{
				this.gradientType = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000BFA3 File Offset: 0x0000A1A3
		// (set) Token: 0x060002F0 RID: 752 RVA: 0x0000BFAB File Offset: 0x0000A1AB
		public Color LinearColor1
		{
			get
			{
				return this.linearColor1;
			}
			set
			{
				this.linearColor1 = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000BFBA File Offset: 0x0000A1BA
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000BFC2 File Offset: 0x0000A1C2
		public Color LinearColor2
		{
			get
			{
				return this.linearColor2;
			}
			set
			{
				this.linearColor2 = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x0000BFD1 File Offset: 0x0000A1D1
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x0000BFD9 File Offset: 0x0000A1D9
		public Color CornerColorUpperLeft
		{
			get
			{
				return this.cornerColorUpperLeft;
			}
			set
			{
				this.cornerColorUpperLeft = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000BFE8 File Offset: 0x0000A1E8
		// (set) Token: 0x060002F6 RID: 758 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		public Color CornerColorUpperRight
		{
			get
			{
				return this.cornerColorUpperRight;
			}
			set
			{
				this.cornerColorUpperRight = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x0000BFFF File Offset: 0x0000A1FF
		// (set) Token: 0x060002F8 RID: 760 RVA: 0x0000C007 File Offset: 0x0000A207
		public Color CornerColorLowerRight
		{
			get
			{
				return this.cornerColorLowerRight;
			}
			set
			{
				this.cornerColorLowerRight = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000C016 File Offset: 0x0000A216
		// (set) Token: 0x060002FA RID: 762 RVA: 0x0000C01E File Offset: 0x0000A21E
		public Color CornerColorLowerLeft
		{
			get
			{
				return this.cornerColorLowerLeft;
			}
			set
			{
				this.cornerColorLowerLeft = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000C02D File Offset: 0x0000A22D
		// (set) Token: 0x060002FC RID: 764 RVA: 0x0000C035 File Offset: 0x0000A235
		public float Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				if (value < 0f)
				{
					this.angle = value % 360f + 360f;
				}
				else
				{
					this.angle = value % 360f;
				}
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000C067 File Offset: 0x0000A267
		// (set) Token: 0x060002FE RID: 766 RVA: 0x0000C06F File Offset: 0x0000A26F
		public Gradient LinearGradient
		{
			get
			{
				return this.linearGradient;
			}
			set
			{
				this.linearGradient = value;
				this.ForceUpdateGraphic();
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000C080 File Offset: 0x0000A280
		public override void ModifyMesh(VertexHelper vh)
		{
			if (base.enabled)
			{
				UIVertex uivertex = default(UIVertex);
				if (this.gradientType == UIGradient.UIGradientType.ComplexLinear)
				{
					this.CutMesh(vh);
				}
				for (int i = 0; i < vh.currentVertCount; i++)
				{
					vh.PopulateUIVertex(ref uivertex, i);
					Vector2 vector = (uivertex.position - this.rectTransform.rect.min) / (this.rectTransform.rect.max - this.rectTransform.rect.min);
					vector = this.RotateNormalizedPosition(vector, this.angle);
					Color c = Color.black;
					if (this.gradientType == UIGradient.UIGradientType.Linear)
					{
						c = this.GetColorInGradient(this.linearColor1, this.linearColor1, this.linearColor2, this.linearColor2, vector);
					}
					else if (this.gradientType == UIGradient.UIGradientType.Corner)
					{
						c = this.GetColorInGradient(this.cornerColorUpperLeft, this.cornerColorUpperRight, this.cornerColorLowerRight, this.cornerColorLowerLeft, vector);
					}
					else if (this.gradientType == UIGradient.UIGradientType.ComplexLinear)
					{
						c = this.linearGradient.Evaluate(vector.y);
					}
					uivertex.color = this.BlendColor(uivertex.color, c, this.blendMode, this.intensity);
					vh.SetUIVertex(uivertex, i);
				}
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000C1E0 File Offset: 0x0000A3E0
		protected void CutMesh(VertexHelper vh)
		{
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			vh.Clear();
			List<UIVertex> list2 = new List<UIVertex>();
			Vector2 cutDirection = this.GetCutDirection();
			foreach (float num in (from x in this.linearGradient.alphaKeys
			select x.time).Union(from x in this.linearGradient.colorKeys
			select x.time))
			{
				list2.Clear();
				Vector2 cutOrigin = this.GetCutOrigin(num);
				if ((double)num >= 0.001 && (double)num <= 0.999)
				{
					for (int i = 0; i < list.Count; i += 3)
					{
						this.CutTriangle(list, i, list2, cutDirection, cutOrigin);
					}
					list.Clear();
					list.AddRange(list2);
				}
			}
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000C308 File Offset: 0x0000A508
		private UIVertex UIVertexLerp(UIVertex v1, UIVertex v2, float f)
		{
			return new UIVertex
			{
				position = Vector3.Lerp(v1.position, v2.position, f),
				color = Color.Lerp(v1.color, v2.color, f),
				uv0 = Vector2.Lerp(v1.uv0, v2.uv0, f),
				uv1 = Vector2.Lerp(v1.uv1, v2.uv1, f),
				uv2 = Vector2.Lerp(v1.uv2, v2.uv2, f),
				uv3 = Vector2.Lerp(v1.uv3, v2.uv3, f)
			};
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000C400 File Offset: 0x0000A600
		private Vector2 GetCutDirection()
		{
			Vector2 vector = Vector2.up.Rotate(-this.angle);
			vector = new Vector2(vector.x / this.rectTransform.rect.size.x, vector.y / this.rectTransform.rect.size.y);
			return vector.Rotate(90f);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000C470 File Offset: 0x0000A670
		private void CutTriangle(List<UIVertex> tris, int idx, List<UIVertex> list, Vector2 cutDirection, Vector2 point)
		{
			UIVertex uivertex = tris[idx];
			UIVertex uivertex2 = tris[idx + 1];
			UIVertex uivertex3 = tris[idx + 2];
			float f = this.OnLine(uivertex2.position, uivertex3.position, point, cutDirection);
			float f2 = this.OnLine(uivertex.position, uivertex2.position, point, cutDirection);
			float f3 = this.OnLine(uivertex3.position, uivertex.position, point, cutDirection);
			if (this.IsOnLine(f2))
			{
				if (this.IsOnLine(f))
				{
					UIVertex item = this.UIVertexLerp(uivertex, uivertex2, f2);
					UIVertex item2 = this.UIVertexLerp(uivertex2, uivertex3, f);
					list.AddRange(new List<UIVertex>
					{
						uivertex,
						item,
						uivertex3,
						item,
						item2,
						uivertex3,
						item,
						uivertex2,
						item2
					});
					return;
				}
				UIVertex item3 = this.UIVertexLerp(uivertex, uivertex2, f2);
				UIVertex item4 = this.UIVertexLerp(uivertex3, uivertex, f3);
				list.AddRange(new List<UIVertex>
				{
					uivertex3,
					item4,
					uivertex2,
					item4,
					item3,
					uivertex2,
					item4,
					uivertex,
					item3
				});
				return;
			}
			else
			{
				if (this.IsOnLine(f))
				{
					UIVertex item5 = this.UIVertexLerp(uivertex2, uivertex3, f);
					UIVertex item6 = this.UIVertexLerp(uivertex3, uivertex, f3);
					list.AddRange(new List<UIVertex>
					{
						uivertex2,
						item5,
						uivertex,
						item5,
						item6,
						uivertex,
						item5,
						uivertex3,
						item6
					});
					return;
				}
				list.AddRange(tris.GetRange(idx, 3));
				return;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000C661 File Offset: 0x0000A861
		private bool IsOnLine(float f)
		{
			return f <= 1f && f > 0f;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000C678 File Offset: 0x0000A878
		private float OnLine(Vector2 p1, Vector2 p2, Vector2 o, Vector2 dir)
		{
			float num = (p2.x - p1.x) * dir.y - (p2.y - p1.y) * dir.x;
			if (num == 0f)
			{
				return -1f;
			}
			return ((o.x - p1.x) * dir.y - (o.y - p1.y) * dir.x) / num;
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000C6EC File Offset: 0x0000A8EC
		private float ProjectedDistance(Vector2 p1, Vector2 p2, Vector2 normal)
		{
			return Vector2.Distance(Vector3.Project(p1, normal), Vector3.Project(p2, normal));
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000C720 File Offset: 0x0000A920
		private Vector2 GetCutOrigin(float f)
		{
			Vector2 vector = Vector2.up.Rotate(-this.angle);
			vector = new Vector2(vector.x / this.rectTransform.rect.size.x, vector.y / this.rectTransform.rect.size.y);
			Vector3 v;
			Vector3 v2;
			if (this.angle % 180f < 90f)
			{
				v = Vector3.Project(Vector2.Scale(this.rectTransform.rect.size, Vector2.down + Vector2.left) * 0.5f, vector);
				v2 = Vector3.Project(Vector2.Scale(this.rectTransform.rect.size, Vector2.up + Vector2.right) * 0.5f, vector);
			}
			else
			{
				v = Vector3.Project(Vector2.Scale(this.rectTransform.rect.size, Vector2.up + Vector2.left) * 0.5f, vector);
				v2 = Vector3.Project(Vector2.Scale(this.rectTransform.rect.size, Vector2.down + Vector2.right) * 0.5f, vector);
			}
			if (this.angle % 360f >= 180f)
			{
				return Vector2.Lerp(v2, v, f) + this.rectTransform.rect.center;
			}
			return Vector2.Lerp(v, v2, f) + this.rectTransform.rect.center;
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000C90F File Offset: 0x0000AB0F
		private Color BlendColor(Color c1, Color c2, UIGradient.UIGradientBlendMode mode, float intensity)
		{
			if (mode == UIGradient.UIGradientBlendMode.Override)
			{
				return Color.Lerp(c1, c2, intensity);
			}
			if (mode == UIGradient.UIGradientBlendMode.Multiply)
			{
				return Color.Lerp(c1, c1 * c2, intensity);
			}
			Debug.LogErrorFormat("Mode is not supported: {0}", new object[]
			{
				mode
			});
			return c1;
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000C94C File Offset: 0x0000AB4C
		private Vector2 RotateNormalizedPosition(Vector2 normalizedPosition, float angle)
		{
			float f = 0.017453292f * ((angle < 0f) ? (angle % 90f + 90f) : (angle % 90f));
			float d = Mathf.Sin(f) + Mathf.Cos(f);
			return (normalizedPosition - Vector2.one * 0.5f).Rotate(angle) / d + Vector2.one * 0.5f;
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000C9C1 File Offset: 0x0000ABC1
		public void ForceUpdateGraphic()
		{
			if (base.graphic != null)
			{
				base.graphic.SetVerticesDirty();
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000C9DC File Offset: 0x0000ABDC
		private Color GetColorInGradient(Color ul, Color ur, Color lr, Color ll, Vector2 normalizedPosition)
		{
			return Color.Lerp(Color.Lerp(ll, lr, normalizedPosition.x), Color.Lerp(ul, ur, normalizedPosition.x), normalizedPosition.y);
		}

		// Token: 0x040001CA RID: 458
		[Tooltip("How the gradient color will be blended with the graphics color.")]
		[SerializeField]
		private UIGradient.UIGradientBlendMode blendMode;

		// Token: 0x040001CB RID: 459
		[SerializeField]
		[Range(0f, 1f)]
		private float intensity = 1f;

		// Token: 0x040001CC RID: 460
		[SerializeField]
		private UIGradient.UIGradientType gradientType;

		// Token: 0x040001CD RID: 461
		[SerializeField]
		private Color linearColor1 = Color.yellow;

		// Token: 0x040001CE RID: 462
		[SerializeField]
		private Color linearColor2 = Color.red;

		// Token: 0x040001CF RID: 463
		[SerializeField]
		private Color cornerColorUpperLeft = Color.red;

		// Token: 0x040001D0 RID: 464
		[SerializeField]
		private Color cornerColorUpperRight = Color.yellow;

		// Token: 0x040001D1 RID: 465
		[SerializeField]
		private Color cornerColorLowerRight = Color.green;

		// Token: 0x040001D2 RID: 466
		[SerializeField]
		private Color cornerColorLowerLeft = Color.blue;

		// Token: 0x040001D3 RID: 467
		[SerializeField]
		private Gradient linearGradient;

		// Token: 0x040001D4 RID: 468
		[SerializeField]
		[Range(0f, 360f)]
		private float angle;

		// Token: 0x040001D5 RID: 469
		private RectTransform _rectTransform;

		// Token: 0x020000CB RID: 203
		public enum UIGradientBlendMode
		{
			// Token: 0x0400041A RID: 1050
			Override,
			// Token: 0x0400041B RID: 1051
			Multiply
		}

		// Token: 0x020000CC RID: 204
		public enum UIGradientType
		{
			// Token: 0x0400041D RID: 1053
			Linear,
			// Token: 0x0400041E RID: 1054
			Corner,
			// Token: 0x0400041F RID: 1055
			ComplexLinear
		}
	}
}

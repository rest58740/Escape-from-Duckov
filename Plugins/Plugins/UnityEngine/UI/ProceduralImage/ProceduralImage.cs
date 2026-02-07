using System;
using UnityEngine.Events;

namespace UnityEngine.UI.ProceduralImage
{
	// Token: 0x0200000F RID: 15
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Procedural Image")]
	public class ProceduralImage : Image
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002C79 File Offset: 0x00000E79
		// (set) Token: 0x06000048 RID: 72 RVA: 0x00002CA1 File Offset: 0x00000EA1
		private static Material DefaultProceduralImageMaterial
		{
			get
			{
				if (ProceduralImage.materialInstance == null)
				{
					ProceduralImage.materialInstance = new Material(Shader.Find("UI/Procedural UI Image"));
				}
				return ProceduralImage.materialInstance;
			}
			set
			{
				ProceduralImage.materialInstance = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00002CA9 File Offset: 0x00000EA9
		// (set) Token: 0x0600004A RID: 74 RVA: 0x00002CB1 File Offset: 0x00000EB1
		public float BorderWidth
		{
			get
			{
				return this.borderWidth;
			}
			set
			{
				this.borderWidth = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002CC0 File Offset: 0x00000EC0
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00002CC8 File Offset: 0x00000EC8
		public float FalloffDistance
		{
			get
			{
				return this.falloffDistance;
			}
			set
			{
				this.falloffDistance = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00002CD7 File Offset: 0x00000ED7
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00002D17 File Offset: 0x00000F17
		protected ProceduralImageModifier Modifier
		{
			get
			{
				if (this.modifier == null)
				{
					this.modifier = base.GetComponent<ProceduralImageModifier>();
					if (this.modifier == null)
					{
						this.ModifierType = typeof(FreeModifier);
					}
				}
				return this.modifier;
			}
			set
			{
				this.modifier = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002D20 File Offset: 0x00000F20
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00002D30 File Offset: 0x00000F30
		public System.Type ModifierType
		{
			get
			{
				return this.Modifier.GetType();
			}
			set
			{
				if (this.modifier != null && this.modifier.GetType() != value)
				{
					if (base.GetComponent<ProceduralImageModifier>() != null)
					{
						Object.DestroyImmediate(base.GetComponent<ProceduralImageModifier>());
					}
					base.gameObject.AddComponent(value);
					this.Modifier = base.GetComponent<ProceduralImageModifier>();
					this.SetAllDirty();
					return;
				}
				if (this.modifier == null)
				{
					base.gameObject.AddComponent(value);
					this.Modifier = base.GetComponent<ProceduralImageModifier>();
					this.SetAllDirty();
				}
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002DC4 File Offset: 0x00000FC4
		protected override void OnEnable()
		{
			base.OnEnable();
			this.Init();
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002DD2 File Offset: 0x00000FD2
		protected override void OnDisable()
		{
			base.OnDisable();
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Remove(this.m_OnDirtyVertsCallback, new UnityAction(this.OnVerticesDirty));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002DFC File Offset: 0x00000FFC
		private void Init()
		{
			this.FixTexCoordsInCanvas();
			this.m_OnDirtyVertsCallback = (UnityAction)Delegate.Combine(this.m_OnDirtyVertsCallback, new UnityAction(this.OnVerticesDirty));
			base.preserveAspect = false;
			this.material = null;
			if (base.sprite == null)
			{
				base.sprite = EmptySprite.Get();
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002E58 File Offset: 0x00001058
		protected void OnVerticesDirty()
		{
			if (base.sprite == null)
			{
				base.sprite = EmptySprite.Get();
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002E74 File Offset: 0x00001074
		protected void FixTexCoordsInCanvas()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this.FixTexCoordsInCanvas(componentInParent);
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002E98 File Offset: 0x00001098
		protected void FixTexCoordsInCanvas(Canvas c)
		{
			c.additionalShaderChannels |= (AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002EA8 File Offset: 0x000010A8
		private Vector4 FixRadius(Vector4 vec)
		{
			Rect rect = base.rectTransform.rect;
			vec = new Vector4(Mathf.Max(vec.x, 0f), Mathf.Max(vec.y, 0f), Mathf.Max(vec.z, 0f), Mathf.Max(vec.w, 0f));
			float d = Mathf.Min(Mathf.Min(Mathf.Min(Mathf.Min(rect.width / (vec.x + vec.y), rect.width / (vec.z + vec.w)), rect.height / (vec.x + vec.w)), rect.height / (vec.z + vec.y)), 1f);
			return vec * d;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002F7D File Offset: 0x0000117D
		protected override void OnPopulateMesh(VertexHelper toFill)
		{
			base.OnPopulateMesh(toFill);
			this.EncodeAllInfoIntoVertices(toFill, this.CalculateInfo());
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002F93 File Offset: 0x00001193
		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			this.FixTexCoordsInCanvas();
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002FA1 File Offset: 0x000011A1
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002FA9 File Offset: 0x000011A9
		public ProceduralImageInfo InfoCache { get; private set; }

		// Token: 0x0600005C RID: 92 RVA: 0x00002FB4 File Offset: 0x000011B4
		private ProceduralImageInfo CalculateInfo()
		{
			Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
			float pixelSize = 1f / Mathf.Max(0f, this.falloffDistance);
			Vector4 a = this.FixRadius(this.Modifier.CalculateRadius(pixelAdjustedRect));
			float num = Mathf.Min(pixelAdjustedRect.width, pixelAdjustedRect.height);
			ProceduralImageInfo proceduralImageInfo = new ProceduralImageInfo(pixelAdjustedRect.width + this.falloffDistance, pixelAdjustedRect.height + this.falloffDistance, this.falloffDistance, pixelSize, a / num, this.borderWidth / num * 2f);
			this.InfoCache = proceduralImageInfo;
			return proceduralImageInfo;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003050 File Offset: 0x00001250
		private void EncodeAllInfoIntoVertices(VertexHelper vh, ProceduralImageInfo info)
		{
			UIVertex uivertex = default(UIVertex);
			Vector2 v = new Vector2(info.width, info.height);
			Vector2 v2 = new Vector2(this.EncodeFloats_0_1_16_16(info.radius.x, info.radius.y), this.EncodeFloats_0_1_16_16(info.radius.z, info.radius.w));
			Vector2 v3 = new Vector2((info.borderWidth == 0f) ? 1f : Mathf.Clamp01(info.borderWidth), info.pixelSize);
			for (int i = 0; i < vh.currentVertCount; i++)
			{
				vh.PopulateUIVertex(ref uivertex, i);
				uivertex.position += (uivertex.uv0 - new Vector3(0.5f, 0.5f)) * info.fallOffDistance;
				uivertex.uv1 = v;
				uivertex.uv2 = v2;
				uivertex.uv3 = v3;
				vh.SetUIVertex(uivertex, i);
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x0000317C File Offset: 0x0000137C
		private float EncodeFloats_0_1_16_16(float a, float b)
		{
			Vector2 rhs = new Vector2(1f, 1.5259022E-05f);
			return Vector2.Dot(new Vector2(Mathf.Floor(a * 65534f) / 65535f, Mathf.Floor(b * 65534f) / 65535f), rhs);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600005F RID: 95 RVA: 0x000031C9 File Offset: 0x000013C9
		// (set) Token: 0x06000060 RID: 96 RVA: 0x000031E5 File Offset: 0x000013E5
		public override Material material
		{
			get
			{
				if (this.m_Material == null)
				{
					return ProceduralImage.DefaultProceduralImageMaterial;
				}
				return base.material;
			}
			set
			{
				base.material = value;
			}
		}

		// Token: 0x04000016 RID: 22
		[SerializeField]
		private float borderWidth;

		// Token: 0x04000017 RID: 23
		private ProceduralImageModifier modifier;

		// Token: 0x04000018 RID: 24
		private static Material materialInstance;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		private float falloffDistance = 1f;
	}
}

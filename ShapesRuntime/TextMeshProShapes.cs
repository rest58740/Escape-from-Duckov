using System;
using TMPro;
using UnityEngine;

namespace Shapes
{
	// Token: 0x02000011 RID: 17
	public class TextMeshProShapes : TextMeshPro
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000688A File Offset: 0x00004A8A
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00006892 File Offset: 0x00004A92
		public float Curvature
		{
			get
			{
				return this.curvature;
			}
			set
			{
				if (this.curvature == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.curvature = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000226 RID: 550 RVA: 0x000068B2 File Offset: 0x00004AB2
		// (set) Token: 0x06000227 RID: 551 RVA: 0x000068BA File Offset: 0x00004ABA
		public Vector2 CurvaturePivot
		{
			get
			{
				return this.curvaturePivot;
			}
			set
			{
				if (this.curvaturePivot == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.curvaturePivot = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000228 RID: 552 RVA: 0x000068DF File Offset: 0x00004ADF
		// (set) Token: 0x06000229 RID: 553 RVA: 0x000068EC File Offset: 0x00004AEC
		public TextWrappingModes textWrappingMode
		{
			get
			{
				if (!base.enableWordWrapping)
				{
					return TextWrappingModes.NoWrap;
				}
				return TextWrappingModes.Normal;
			}
			set
			{
				base.enableWordWrapping = (value > TextWrappingModes.NoWrap);
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000068F8 File Offset: 0x00004AF8
		protected override void OnEnable()
		{
			base.OnEnable();
			this.OnPreRenderText += new Action<TMP_TextInfo>(this.ApplyDeformation);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00006912 File Offset: 0x00004B12
		protected override void OnDisable()
		{
			base.OnDisable();
			this.OnPreRenderText -= new Action<TMP_TextInfo>(this.ApplyDeformation);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000692C File Offset: 0x00004B2C
		private void ApplyDeformation(TMP_TextInfo obj)
		{
			if (this.curvature == 0f)
			{
				return;
			}
			Vector3 b = this.curvaturePivot;
			foreach (TMP_CharacterInfo tmp_CharacterInfo in base.textInfo.characterInfo)
			{
				if (tmp_CharacterInfo.isVisible)
				{
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3[] vertices = base.textInfo.meshInfo[tmp_CharacterInfo.materialReferenceIndex].vertices;
					for (int j = 0; j < 4; j++)
					{
						vertices[vertexIndex + j] = TextMeshProShapes.Bend(vertices[vertexIndex + j] - b, this.curvature) + b;
					}
				}
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x000069E4 File Offset: 0x00004BE4
		private static Vector3 Bend(Vector3 p, float curvature)
		{
			float num = 1f - curvature * p.y;
			float num2 = p.x * num;
			float num3 = curvature / num;
			float x = num2 * num3;
			return new Vector3(num2 * ShapesMath.Sinc(x), num2 * ShapesMath.Cosinc(x) + p.y, p.z);
		}

		// Token: 0x04000080 RID: 128
		[SerializeField]
		protected float curvature;

		// Token: 0x04000081 RID: 129
		[SerializeField]
		protected Vector2 curvaturePivot;
	}
}

using System;
using TMPro;
using UnityEngine;

namespace FX
{
	// Token: 0x02000218 RID: 536
	public class PopTextEntity : MonoBehaviour
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x000401FD File Offset: 0x0003E3FD
		private RectTransform spriteRendererRectTransform
		{
			get
			{
				if (this._spriteRendererRectTransform_cache == null)
				{
					this._spriteRendererRectTransform_cache = this.spriteRenderer.GetComponent<RectTransform>();
				}
				return this._spriteRendererRectTransform_cache;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x00040224 File Offset: 0x0003E424
		private TextMeshPro tmp
		{
			get
			{
				return this._tmp;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001023 RID: 4131 RVA: 0x0004022C File Offset: 0x0003E42C
		public TextMeshPro Tmp
		{
			get
			{
				return this.tmp;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x00040234 File Offset: 0x0003E434
		public Color EndColor
		{
			get
			{
				return this.endColor;
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x0004023C File Offset: 0x0003E43C
		// (set) Token: 0x06001026 RID: 4134 RVA: 0x00040244 File Offset: 0x0003E444
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.endColor = this.color;
				this.endColor.a = 0f;
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001027 RID: 4135 RVA: 0x00040269 File Offset: 0x0003E469
		public float timeSinceSpawn
		{
			get
			{
				return Time.time - this.spawnTime;
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x00040277 File Offset: 0x0003E477
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x00040284 File Offset: 0x0003E484
		private string text
		{
			get
			{
				return this.tmp.text;
			}
			set
			{
				this.tmp.text = value;
			}
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00040294 File Offset: 0x0003E494
		public void SetupContent(string text, Sprite sprite = null)
		{
			this.text = text;
			if (sprite == null)
			{
				this.spriteRenderer.gameObject.SetActive(false);
				return;
			}
			this.spriteRenderer.gameObject.SetActive(true);
			this.spriteRenderer.sprite = sprite;
			this.spriteRenderer.transform.localScale = Vector3.one * (0.5f / (sprite.rect.width / sprite.pixelsPerUnit));
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00040315 File Offset: 0x0003E515
		internal void SetColor(Color newColor)
		{
			this.Tmp.color = newColor;
			this.spriteRenderer.color = newColor;
		}

		// Token: 0x04000D18 RID: 3352
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x04000D19 RID: 3353
		private RectTransform _spriteRendererRectTransform_cache;

		// Token: 0x04000D1A RID: 3354
		[SerializeField]
		private TextMeshPro _tmp;

		// Token: 0x04000D1B RID: 3355
		public Vector3 velocity;

		// Token: 0x04000D1C RID: 3356
		public float size;

		// Token: 0x04000D1D RID: 3357
		private Color color;

		// Token: 0x04000D1E RID: 3358
		private Color endColor;

		// Token: 0x04000D1F RID: 3359
		public float spawnTime;
	}
}

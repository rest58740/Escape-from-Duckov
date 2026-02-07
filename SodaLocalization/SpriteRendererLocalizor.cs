using System;
using System.Collections.Generic;
using UnityEngine;

namespace SodaCraft.Localizations
{
	// Token: 0x0200000B RID: 11
	public class SpriteRendererLocalizor : MonoBehaviour
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00002AE3 File Offset: 0x00000CE3
		private void Awake()
		{
			if (this.spriteRenderer == null)
			{
				this.spriteRenderer = base.GetComponent<SpriteRenderer>();
			}
			LocalizationManager.OnSetLanguage += new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B10 File Offset: 0x00000D10
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B23 File Offset: 0x00000D23
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B2B File Offset: 0x00000D2B
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002B34 File Offset: 0x00000D34
		private void Refresh()
		{
			if (this.entries == null)
			{
				return;
			}
			Sprite sprite = this.entries.Find((SpriteRendererLocalizor.LanguageSpritePair e) => e.language == LocalizationManager.CurrentLanguage).sprite;
			if (sprite == null && this.entries.Count > 0)
			{
				sprite = this.entries[0].sprite;
			}
			this.spriteRenderer.sprite = sprite;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BAF File Offset: 0x00000DAF
		private void OnValidate()
		{
			this.Refresh();
		}

		// Token: 0x04000011 RID: 17
		[SerializeField]
		private List<SpriteRendererLocalizor.LanguageSpritePair> entries;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		private SpriteRenderer spriteRenderer;

		// Token: 0x02000014 RID: 20
		[Serializable]
		private struct LanguageSpritePair
		{
			// Token: 0x04000026 RID: 38
			public SystemLanguage language;

			// Token: 0x04000027 RID: 39
			public Sprite sprite;
		}
	}
}

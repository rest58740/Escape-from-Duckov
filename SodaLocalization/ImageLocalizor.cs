using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SodaCraft.Localizations
{
	// Token: 0x0200000A RID: 10
	public class ImageLocalizor : MonoBehaviour
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002A07 File Offset: 0x00000C07
		private void Awake()
		{
			if (this.spriteRenderer == null)
			{
				this.spriteRenderer = base.GetComponent<Image>();
			}
			this.Refresh();
			LocalizationManager.OnSetLanguage += new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A3A File Offset: 0x00000C3A
		private void OnDestroy()
		{
			LocalizationManager.OnSetLanguage -= new Action<SystemLanguage>(this.OnSetLanguage);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A4D File Offset: 0x00000C4D
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A58 File Offset: 0x00000C58
		private void Refresh()
		{
			if (this.entries == null)
			{
				return;
			}
			Sprite sprite = this.entries.Find((ImageLocalizor.LanguageSpritePair e) => e.language == LocalizationManager.CurrentLanguage).sprite;
			if (sprite == null && this.entries.Count > 0)
			{
				sprite = this.entries[0].sprite;
			}
			this.spriteRenderer.sprite = sprite;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002AD3 File Offset: 0x00000CD3
		private void OnValidate()
		{
			this.Refresh();
		}

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private List<ImageLocalizor.LanguageSpritePair> entries;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		private Image spriteRenderer;

		// Token: 0x02000012 RID: 18
		[Serializable]
		private struct LanguageSpritePair
		{
			// Token: 0x04000022 RID: 34
			public SystemLanguage language;

			// Token: 0x04000023 RID: 35
			public Sprite sprite;
		}
	}
}

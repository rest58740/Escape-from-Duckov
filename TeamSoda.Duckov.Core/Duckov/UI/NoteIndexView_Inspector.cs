using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A6 RID: 934
	public class NoteIndexView_Inspector : MonoBehaviour
	{
		// Token: 0x0600209C RID: 8348 RVA: 0x000727F8 File Offset: 0x000709F8
		private void Awake()
		{
			this.placeHolder.Show();
			this.content.SkipHide();
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x00072810 File Offset: 0x00070A10
		internal void Setup(Note value)
		{
			if (value == null)
			{
				this.placeHolder.Show();
				this.content.Hide();
				return;
			}
			this.note = value;
			this.SetupContent(this.note);
			this.placeHolder.Hide();
			this.content.Show();
			NoteIndex.SetNoteRead(value.key);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0007286C File Offset: 0x00070A6C
		private void SetupContent(Note value)
		{
			this.textTitle.text = value.Title;
			this.textContent.text = value.Content;
			this.image.sprite = value.image;
			this.image.gameObject.SetActive(value.image == null);
		}

		// Token: 0x0400163F RID: 5695
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x04001640 RID: 5696
		[SerializeField]
		private FadeGroup content;

		// Token: 0x04001641 RID: 5697
		[SerializeField]
		private TextMeshProUGUI textTitle;

		// Token: 0x04001642 RID: 5698
		[SerializeField]
		private TextMeshProUGUI textContent;

		// Token: 0x04001643 RID: 5699
		[SerializeField]
		private Image image;

		// Token: 0x04001644 RID: 5700
		private Note note;
	}
}

using System;
using Duckov.UI;

namespace Duckov.NoteIndexs
{
	// Token: 0x02000275 RID: 629
	public class NoteInteract : InteractableBase
	{
		// Token: 0x060013D0 RID: 5072 RVA: 0x0004A66D File Offset: 0x0004886D
		protected override void Start()
		{
			base.Start();
			if (NoteIndex.GetNoteUnlocked(this.noteKey))
			{
				base.gameObject.SetActive(false);
			}
			this.finishWhenTimeOut = true;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0004A695 File Offset: 0x00048895
		protected override void OnInteractFinished()
		{
			NoteIndex.SetNoteUnlocked(this.noteKey);
			NoteIndexView.ShowNote(this.noteKey, true);
			base.gameObject.SetActive(false);
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0004A6BA File Offset: 0x000488BA
		private void OnValidate()
		{
			this.noteTitle = "Note_" + this.noteKey + "_Title";
			this.noteContent = "Note_" + this.noteKey + "_Content";
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0004A6F2 File Offset: 0x000488F2
		public void ReName()
		{
			base.gameObject.name = "Note_" + this.noteKey;
		}

		// Token: 0x04000ECE RID: 3790
		public string noteKey;

		// Token: 0x04000ECF RID: 3791
		[LocalizationKey("Default")]
		public string noteTitle;

		// Token: 0x04000ED0 RID: 3792
		[LocalizationKey("Default")]
		public string noteContent;
	}
}

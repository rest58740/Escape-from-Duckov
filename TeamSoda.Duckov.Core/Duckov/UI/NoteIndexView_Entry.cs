using System;
using Duckov.NoteIndexs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Duckov.UI
{
	// Token: 0x020003A5 RID: 933
	public class NoteIndexView_Entry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00072688 File Offset: 0x00070888
		public string key
		{
			get
			{
				return this.note.key;
			}
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00072695 File Offset: 0x00070895
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x000726B7 File Offset: 0x000708B7
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x000726D9 File Offset: 0x000708D9
		private void OnNoteStatusChanged(string key)
		{
			if (key != this.note.key)
			{
				return;
			}
			this.RefreshNotReadIndicator();
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x000726F5 File Offset: 0x000708F5
		private void RefreshNotReadIndicator()
		{
			this.notReadIndicator.SetActive(NoteIndex.GetNoteUnlocked(this.key) && !NoteIndex.GetNoteRead(this.key));
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00072720 File Offset: 0x00070920
		internal void NotifySelectedDisplayingNoteChanged(string displayingNote)
		{
			this.RefreshHighlight();
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x00072728 File Offset: 0x00070928
		private void RefreshHighlight()
		{
			bool active = false;
			if (this.getDisplayingNote != null)
			{
				Func<string> func = this.getDisplayingNote;
				active = (((func != null) ? func() : null) == this.key);
			}
			this.highlightIndicator.SetActive(active);
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x0007276C File Offset: 0x0007096C
		internal void Setup(Note note, Action<NoteIndexView_Entry> onClicked, Func<string> getDisplayingNote, int index)
		{
			bool noteUnlocked = NoteIndex.GetNoteUnlocked(note.key);
			this.note = note;
			this.titleText.text = (noteUnlocked ? note.Title : "???");
			this.onClicked = onClicked;
			this.getDisplayingNote = getDisplayingNote;
			if (index > 0)
			{
				this.indexText.text = index.ToString("000");
			}
			this.RefreshNotReadIndicator();
			this.RefreshHighlight();
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x000727DD File Offset: 0x000709DD
		public void OnPointerClick(PointerEventData eventData)
		{
			Action<NoteIndexView_Entry> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x04001638 RID: 5688
		[SerializeField]
		private GameObject highlightIndicator;

		// Token: 0x04001639 RID: 5689
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x0400163A RID: 5690
		[SerializeField]
		private TextMeshProUGUI indexText;

		// Token: 0x0400163B RID: 5691
		[SerializeField]
		private GameObject notReadIndicator;

		// Token: 0x0400163C RID: 5692
		private Note note;

		// Token: 0x0400163D RID: 5693
		private Action<NoteIndexView_Entry> onClicked;

		// Token: 0x0400163E RID: 5694
		private Func<string> getDisplayingNote;
	}
}

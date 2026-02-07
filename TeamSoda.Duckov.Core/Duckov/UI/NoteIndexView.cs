using System;
using Duckov.NoteIndexs;
using Duckov.UI.Animations;
using Duckov.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003A4 RID: 932
	public class NoteIndexView : View
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002080 RID: 8320 RVA: 0x000722C4 File Offset: 0x000704C4
		private PrefabPool<NoteIndexView_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<NoteIndexView_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x000722FD File Offset: 0x000704FD
		private void OnEnable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Combine(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x0007231F File Offset: 0x0007051F
		private void OnDisable()
		{
			NoteIndex.onNoteStatusChanged = (Action<string>)Delegate.Remove(NoteIndex.onNoteStatusChanged, new Action<string>(this.OnNoteStatusChanged));
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00072341 File Offset: 0x00070541
		private void Update()
		{
			if (this.needFocus)
			{
				this.needFocus = false;
				this.MoveScrollViewToActiveEntry();
			}
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00072358 File Offset: 0x00070558
		private void OnNoteStatusChanged(string noteKey)
		{
			this.RefreshEntries();
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00072360 File Offset: 0x00070560
		public void DoOpen()
		{
			base.Open(null);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00072369 File Offset: 0x00070569
		protected override void OnOpen()
		{
			base.OnOpen();
			this.mainFadeGroup.Show();
			this.RefreshEntries();
			this.SetDisplayTargetNote(this.displayingNote);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x0007238E File Offset: 0x0007058E
		protected override void OnClose()
		{
			base.OnClose();
			this.mainFadeGroup.Hide();
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x000723A1 File Offset: 0x000705A1
		protected override void OnCancel()
		{
			base.Close();
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x000723AC File Offset: 0x000705AC
		private void RefreshNoteCount()
		{
			int totalNoteCount = NoteIndex.GetTotalNoteCount();
			int unlockedNoteCount = NoteIndex.GetUnlockedNoteCount();
			this.noteCountText.text = string.Format("{0} / {1}", unlockedNoteCount, totalNoteCount);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x000723E8 File Offset: 0x000705E8
		private void RefreshEntries()
		{
			this.RefreshNoteCount();
			this.Pool.ReleaseAll();
			if (NoteIndex.Instance == null)
			{
				return;
			}
			int num = 0;
			foreach (string key in NoteIndex.GetAllNotes(false))
			{
				Note note = NoteIndex.GetNote(key);
				if (note != null)
				{
					NoteIndexView_Entry noteIndexView_Entry = this.Pool.Get(null);
					num++;
					noteIndexView_Entry.Setup(note, new Action<NoteIndexView_Entry>(this.OnEntryClicked), new Func<string>(this.GetDisplayingNote), num);
				}
			}
			this.noEntryIndicator.SetActive(num <= 0);
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x00072498 File Offset: 0x00070698
		private string GetDisplayingNote()
		{
			return this.displayingNote;
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000724A0 File Offset: 0x000706A0
		public void SetDisplayTargetNote(string noteKey)
		{
			Note note = null;
			if (!string.IsNullOrWhiteSpace(noteKey))
			{
				note = NoteIndex.GetNote(noteKey);
			}
			if (note == null)
			{
				this.displayingNote = null;
			}
			else
			{
				this.displayingNote = note.key;
			}
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				noteIndexView_Entry.NotifySelectedDisplayingNoteChanged(this.displayingNote);
			}
			this.inspector.Setup(note);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0007252C File Offset: 0x0007072C
		private void OnEntryClicked(NoteIndexView_Entry entry)
		{
			string key = entry.key;
			if (!NoteIndex.GetNoteUnlocked(key))
			{
				this.SetDisplayTargetNote("");
				return;
			}
			this.SetDisplayTargetNote(key);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0007255C File Offset: 0x0007075C
		public static void ShowNote(string noteKey, bool unlock = true)
		{
			NoteIndexView viewInstance = View.GetViewInstance<NoteIndexView>();
			if (viewInstance == null)
			{
				return;
			}
			if (unlock)
			{
				NoteIndex.SetNoteUnlocked(noteKey);
			}
			if (!(View.ActiveView is NoteIndexView))
			{
				viewInstance.Open(null);
			}
			viewInstance.SetDisplayTargetNote(noteKey);
			viewInstance.needFocus = true;
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x000725A4 File Offset: 0x000707A4
		private void MoveScrollViewToActiveEntry()
		{
			NoteIndexView_Entry displayingEntry = this.GetDisplayingEntry();
			if (displayingEntry == null)
			{
				return;
			}
			RectTransform rectTransform = displayingEntry.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			float num = -rectTransform.anchoredPosition.y;
			float height = this.indexScrollView.content.rect.height;
			float verticalNormalizedPosition = 1f - num / height;
			this.indexScrollView.verticalNormalizedPosition = verticalNormalizedPosition;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x00072618 File Offset: 0x00070818
		private NoteIndexView_Entry GetDisplayingEntry()
		{
			foreach (NoteIndexView_Entry noteIndexView_Entry in this.Pool.ActiveEntries)
			{
				if (noteIndexView_Entry.key == this.displayingNote)
				{
					return noteIndexView_Entry;
				}
			}
			return null;
		}

		// Token: 0x0400162F RID: 5679
		[SerializeField]
		private FadeGroup mainFadeGroup;

		// Token: 0x04001630 RID: 5680
		[SerializeField]
		private GameObject noEntryIndicator;

		// Token: 0x04001631 RID: 5681
		[SerializeField]
		private NoteIndexView_Entry entryTemplate;

		// Token: 0x04001632 RID: 5682
		[SerializeField]
		private NoteIndexView_Inspector inspector;

		// Token: 0x04001633 RID: 5683
		[SerializeField]
		private TextMeshProUGUI noteCountText;

		// Token: 0x04001634 RID: 5684
		[SerializeField]
		private ScrollRect indexScrollView;

		// Token: 0x04001635 RID: 5685
		private PrefabPool<NoteIndexView_Entry> _pool;

		// Token: 0x04001636 RID: 5686
		private string displayingNote;

		// Token: 0x04001637 RID: 5687
		private bool needFocus;
	}
}

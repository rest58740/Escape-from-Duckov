using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov.Quests.UI
{
	// Token: 0x02000360 RID: 864
	public class QuestGiverTabs : MonoBehaviour, ISingleSelectionMenu<QuestGiverTabButton>
	{
		// Token: 0x140000D9 RID: 217
		// (add) Token: 0x06001DF9 RID: 7673 RVA: 0x0006C230 File Offset: 0x0006A430
		// (remove) Token: 0x06001DFA RID: 7674 RVA: 0x0006C268 File Offset: 0x0006A468
		public event Action<QuestGiverTabs> onSelectionChanged;

		// Token: 0x06001DFB RID: 7675 RVA: 0x0006C29D File Offset: 0x0006A49D
		public QuestGiverTabButton GetSelection()
		{
			return this.selectedButton;
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0006C2A5 File Offset: 0x0006A4A5
		public QuestStatus GetStatus()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			return this.selectedButton.Status;
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0006C2C0 File Offset: 0x0006A4C0
		public bool SetSelection(QuestGiverTabButton selection)
		{
			this.selectedButton = selection;
			this.RefreshAllButtons();
			Action<QuestGiverTabs> action = this.onSelectionChanged;
			if (action != null)
			{
				action(this);
			}
			return true;
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0006C2E4 File Offset: 0x0006A4E4
		private void Initialize()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Setup(this);
			}
			if (this.buttons.Count > 0)
			{
				this.SetSelection(this.buttons[0]);
			}
			this.initialized = true;
		}

		// Token: 0x06001DFF RID: 7679 RVA: 0x0006C360 File Offset: 0x0006A560
		private void Awake()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x0006C370 File Offset: 0x0006A570
		private void RefreshAllButtons()
		{
			foreach (QuestGiverTabButton questGiverTabButton in this.buttons)
			{
				questGiverTabButton.Refresh();
			}
		}

		// Token: 0x040014D9 RID: 5337
		[SerializeField]
		private List<QuestGiverTabButton> buttons = new List<QuestGiverTabButton>();

		// Token: 0x040014DA RID: 5338
		private QuestGiverTabButton selectedButton;

		// Token: 0x040014DC RID: 5340
		private bool initialized;
	}
}

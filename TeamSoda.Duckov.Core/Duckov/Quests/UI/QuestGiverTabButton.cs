using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x0200035E RID: 862
	public class QuestGiverTabButton : MonoBehaviour
	{
		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0006C19D File Offset: 0x0006A39D
		public QuestStatus Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0006C1A5 File Offset: 0x0006A3A5
		internal void Setup(QuestGiverTabs questGiverTabs)
		{
			this.master = questGiverTabs;
			this.Refresh();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0006C1B4 File Offset: 0x0006A3B4
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnClick));
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x0006C1D2 File Offset: 0x0006A3D2
		private void OnClick()
		{
			if (this.master == null)
			{
				return;
			}
			this.master.SetSelection(this);
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0006C1F0 File Offset: 0x0006A3F0
		private bool Selected
		{
			get
			{
				return !(this.master == null) && this.master.GetSelection() == this;
			}
		}

		// Token: 0x06001DF7 RID: 7671 RVA: 0x0006C213 File Offset: 0x0006A413
		internal void Refresh()
		{
			this.selectedIndicator.SetActive(this.Selected);
		}

		// Token: 0x040014D0 RID: 5328
		[SerializeField]
		private Button button;

		// Token: 0x040014D1 RID: 5329
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040014D2 RID: 5330
		[SerializeField]
		private QuestStatus status;

		// Token: 0x040014D3 RID: 5331
		private QuestGiverTabs master;
	}
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Rules.UI
{
	// Token: 0x02000411 RID: 1041
	public class DifficultySelection_Entry : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x000835CF File Offset: 0x000817CF
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x000835D7 File Offset: 0x000817D7
		public DifficultySelection Master { get; private set; }

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x000835E0 File Offset: 0x000817E0
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x000835E8 File Offset: 0x000817E8
		public DifficultySelection.SettingEntry Setting { get; private set; }

		// Token: 0x060025BB RID: 9659 RVA: 0x000835F1 File Offset: 0x000817F1
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.locked)
			{
				return;
			}
			this.Master.NotifySelected(this);
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x00083608 File Offset: 0x00081808
		public void OnPointerEnter(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerEnter(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerEnter;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x0008362D File Offset: 0x0008182D
		public void OnPointerExit(PointerEventData eventData)
		{
			DifficultySelection master = this.Master;
			if (master != null)
			{
				master.NotifyEntryPointerExit(this);
			}
			Action<DifficultySelection_Entry> action = this.onPointerExit;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x00083652 File Offset: 0x00081852
		internal void Refresh()
		{
			if (this.Master == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Master.SelectedRuleIndex == this.Setting.ruleIndex);
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00083688 File Offset: 0x00081888
		internal void Setup(DifficultySelection master, DifficultySelection.SettingEntry setting, bool locked)
		{
			this.Master = master;
			this.Setting = setting;
			this.title.text = setting.Title;
			this.icon.sprite = setting.icon;
			this.recommendationIndicator.SetActive(setting.recommended);
			this.locked = locked;
			this.lockedIndicator.SetActive(locked);
			this.Refresh();
		}

		// Token: 0x040019AF RID: 6575
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x040019B0 RID: 6576
		[SerializeField]
		private Image icon;

		// Token: 0x040019B1 RID: 6577
		[SerializeField]
		private GameObject recommendationIndicator;

		// Token: 0x040019B2 RID: 6578
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x040019B3 RID: 6579
		[SerializeField]
		private GameObject lockedIndicator;

		// Token: 0x040019B4 RID: 6580
		internal Action<DifficultySelection_Entry> onPointerEnter;

		// Token: 0x040019B5 RID: 6581
		internal Action<DifficultySelection_Entry> onPointerExit;

		// Token: 0x040019B8 RID: 6584
		private bool locked;
	}
}

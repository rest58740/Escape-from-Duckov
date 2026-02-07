using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.Endowment.UI
{
	// Token: 0x02000309 RID: 777
	public class EndowmentSelectionEntry : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600197C RID: 6524 RVA: 0x0005D69F File Offset: 0x0005B89F
		public string DisplayName
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DisplayName;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600197D RID: 6525 RVA: 0x0005D6C0 File Offset: 0x0005B8C0
		public string Description
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.Description;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600197E RID: 6526 RVA: 0x0005D6E1 File Offset: 0x0005B8E1
		public string DescriptionAndEffects
		{
			get
			{
				if (this.Target == null)
				{
					return "-";
				}
				return this.Target.DescriptionAndEffects;
			}
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x0600197F RID: 6527 RVA: 0x0005D702 File Offset: 0x0005B902
		public EndowmentIndex Index
		{
			get
			{
				if (this.Target == null)
				{
					return EndowmentIndex.None;
				}
				return this.Target.Index;
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001980 RID: 6528 RVA: 0x0005D71F File Offset: 0x0005B91F
		// (set) Token: 0x06001981 RID: 6529 RVA: 0x0005D727 File Offset: 0x0005B927
		public EndowmentEntry Target { get; private set; }

		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06001982 RID: 6530 RVA: 0x0005D730 File Offset: 0x0005B930
		// (set) Token: 0x06001983 RID: 6531 RVA: 0x0005D738 File Offset: 0x0005B938
		public bool Selected { get; private set; }

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x0005D741 File Offset: 0x0005B941
		public bool Unlocked
		{
			get
			{
				return EndowmentManager.GetEndowmentUnlocked(this.Index);
			}
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x0005D74E File Offset: 0x0005B94E
		public bool Locked
		{
			get
			{
				return !this.Unlocked;
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0005D75C File Offset: 0x0005B95C
		public void Setup(EndowmentEntry target)
		{
			this.Target = target;
			if (this.Target == null)
			{
				return;
			}
			this.displayNameText.text = this.Target.DisplayName;
			this.icon.sprite = this.Target.Icon;
			this.requirementText.text = "- " + this.Target.RequirementText + " -";
			this.Refresh();
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0005D7D6 File Offset: 0x0005B9D6
		private void Refresh()
		{
			if (this.Target == null)
			{
				return;
			}
			this.selectedIndicator.SetActive(this.Selected);
			this.lockedIndcator.SetActive(this.Locked);
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0005D809 File Offset: 0x0005BA09
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.Locked)
			{
				return;
			}
			Action<EndowmentSelectionEntry, PointerEventData> action = this.onClicked;
			if (action == null)
			{
				return;
			}
			action(this, eventData);
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0005D826 File Offset: 0x0005BA26
		internal void SetSelection(bool value)
		{
			this.Selected = value;
			this.Refresh();
		}

		// Token: 0x04001292 RID: 4754
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x04001293 RID: 4755
		[SerializeField]
		private Image icon;

		// Token: 0x04001294 RID: 4756
		[SerializeField]
		private GameObject selectedIndicator;

		// Token: 0x04001295 RID: 4757
		[SerializeField]
		private GameObject lockedIndcator;

		// Token: 0x04001296 RID: 4758
		[SerializeField]
		private TextMeshProUGUI requirementText;

		// Token: 0x04001297 RID: 4759
		public Action<EndowmentSelectionEntry, PointerEventData> onClicked;
	}
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duckov.MiniGames.GoldMiner.UI
{
	// Token: 0x020002BB RID: 699
	public class NavEntry : MonoBehaviour
	{
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00055552 File Offset: 0x00053752
		// (set) Token: 0x060016FE RID: 5886 RVA: 0x0005555A File Offset: 0x0005375A
		public bool selectionState { get; private set; }

		// Token: 0x060016FF RID: 5887 RVA: 0x00055564 File Offset: 0x00053764
		private void Awake()
		{
			if (this.masterGroup == null)
			{
				this.masterGroup = base.GetComponentInParent<NavGroup>();
			}
			this.VCT = base.GetComponent<VirtualCursorTarget>();
			if (this.VCT)
			{
				this.VCT.onEnter.AddListener(new UnityAction(this.TrySelectThis));
				this.VCT.onClick.AddListener(new UnityAction(this.Interact));
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000555DC File Offset: 0x000537DC
		private void Interact()
		{
			this.NotifyInteract();
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000555E4 File Offset: 0x000537E4
		public void NotifySelectionState(bool value)
		{
			this.selectionState = value;
			this.selectedIndicator.SetActive(this.selectionState);
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x000555FE File Offset: 0x000537FE
		internal void NotifyInteract()
		{
			Action<NavEntry> action = this.onInteract;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00055611 File Offset: 0x00053811
		public void TrySelectThis()
		{
			if (this.masterGroup == null)
			{
				return;
			}
			this.masterGroup.TrySelect(this);
		}

		// Token: 0x0400110E RID: 4366
		public GameObject selectedIndicator;

		// Token: 0x0400110F RID: 4367
		public Action<NavEntry> onInteract;

		// Token: 0x04001110 RID: 4368
		public Action<NavEntry> onTrySelectThis;

		// Token: 0x04001111 RID: 4369
		public NavGroup masterGroup;

		// Token: 0x04001112 RID: 4370
		public VirtualCursorTarget VCT;
	}
}

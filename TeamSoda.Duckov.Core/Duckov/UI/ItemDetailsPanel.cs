using System;
using Duckov.UI.Animations;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AE RID: 942
	public class ItemDetailsPanel : ManagedUIElement
	{
		// Token: 0x06002142 RID: 8514 RVA: 0x00074E1E File Offset: 0x0007301E
		protected override void Awake()
		{
			base.Awake();
			if (ItemDetailsPanel.instance == null)
			{
				ItemDetailsPanel.instance = this;
			}
			this.closeButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00074E55 File Offset: 0x00073055
		private void OnCloseButtonClicked()
		{
			base.Close();
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x00074E5D File Offset: 0x0007305D
		public static void Show(Item target, ManagedUIElement source = null)
		{
			if (ItemDetailsPanel.instance == null)
			{
				return;
			}
			ItemDetailsPanel.instance.Open(target, source);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x00074E79 File Offset: 0x00073079
		public void Open(Item target, ManagedUIElement source)
		{
			this.target = target;
			this.source = source;
			base.Open(source);
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x00074E90 File Offset: 0x00073090
		protected override void OnOpen()
		{
			if (this.target == null)
			{
				return;
			}
			base.gameObject.SetActive(true);
			this.Setup(this.target);
			this.fadeGroup.Show();
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x00074EC4 File Offset: 0x000730C4
		protected override void OnClose()
		{
			this.UnregisterEvents();
			this.target = null;
			this.fadeGroup.Hide();
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00074EDE File Offset: 0x000730DE
		private void OnDisable()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x00074EE6 File Offset: 0x000730E6
		internal void Setup(Item target)
		{
			this.display.Setup(target);
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00074EF4 File Offset: 0x000730F4
		private void UnregisterEvents()
		{
			this.display.UnregisterEvents();
		}

		// Token: 0x040016B6 RID: 5814
		private static ItemDetailsPanel instance;

		// Token: 0x040016B7 RID: 5815
		private Item target;

		// Token: 0x040016B8 RID: 5816
		[SerializeField]
		private ItemDetailsDisplay display;

		// Token: 0x040016B9 RID: 5817
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040016BA RID: 5818
		[SerializeField]
		private Button closeButton;

		// Token: 0x040016BB RID: 5819
		private ManagedUIElement source;
	}
}

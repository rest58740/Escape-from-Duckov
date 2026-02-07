using System;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI.Inventories
{
	// Token: 0x020003E6 RID: 998
	public class PagesControl : MonoBehaviour
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x06002468 RID: 9320 RVA: 0x0007FF80 File Offset: 0x0007E180
		private PrefabPool<PagesControl_Entry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<PagesControl_Entry>(this.template, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0007FFB9 File Offset: 0x0007E1B9
		private void Start()
		{
			if (this.target != null)
			{
				this.Setup(this.target);
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0007FFD5 File Offset: 0x0007E1D5
		public void Setup(InventoryDisplay target)
		{
			this.UnregisterEvents();
			this.target = target;
			this.RegisterEvents();
			this.Refresh();
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0007FFF0 File Offset: 0x0007E1F0
		private void RegisterEvents()
		{
			this.UnregisterEvents();
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed += this.OnPageInfoRefreshed;
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0008001E File Offset: 0x0007E21E
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onPageInfoRefreshed -= this.OnPageInfoRefreshed;
		}

		// Token: 0x0600246D RID: 9325 RVA: 0x00080046 File Offset: 0x0007E246
		private void OnPageInfoRefreshed()
		{
			this.Refresh();
		}

		// Token: 0x0600246E RID: 9326 RVA: 0x00080050 File Offset: 0x0007E250
		private void Refresh()
		{
			this.Pool.ReleaseAll();
			if (this.inputIndicators)
			{
				GameObject gameObject = this.inputIndicators;
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.target == null)
			{
				return;
			}
			if (!this.target.UsePages)
			{
				return;
			}
			if (this.target.MaxPage <= 1)
			{
				return;
			}
			for (int i = 0; i < this.target.MaxPage; i++)
			{
				this.Pool.Get(null).Setup(this, i, this.target.SelectedPage == i);
			}
			if (this.inputIndicators)
			{
				GameObject gameObject2 = this.inputIndicators;
				if (gameObject2 == null)
				{
					return;
				}
				gameObject2.SetActive(true);
			}
		}

		// Token: 0x0600246F RID: 9327 RVA: 0x00080108 File Offset: 0x0007E308
		internal void NotifySelect(int i)
		{
			if (this.target == null)
			{
				return;
			}
			this.target.SetPage(i);
		}

		// Token: 0x040018C5 RID: 6341
		[SerializeField]
		private InventoryDisplay target;

		// Token: 0x040018C6 RID: 6342
		[SerializeField]
		private PagesControl_Entry template;

		// Token: 0x040018C7 RID: 6343
		[SerializeField]
		private GameObject inputIndicators;

		// Token: 0x040018C8 RID: 6344
		private PrefabPool<PagesControl_Entry> _pool;
	}
}

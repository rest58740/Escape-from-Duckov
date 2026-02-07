using System;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003A1 RID: 929
	public class FormulasIndexView : View, ISingleSelectionMenu<FormulasIndexEntry>
	{
		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x00071888 File Offset: 0x0006FA88
		private PrefabPool<FormulasIndexEntry> Pool
		{
			get
			{
				if (this._pool == null)
				{
					this._pool = new PrefabPool<FormulasIndexEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._pool;
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000718C1 File Offset: 0x0006FAC1
		public FormulasIndexEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000718C9 File Offset: 0x0006FAC9
		public static FormulasIndexView Instance
		{
			get
			{
				return View.GetViewInstance<FormulasIndexView>();
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000718D0 File Offset: 0x0006FAD0
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000718D8 File Offset: 0x0006FAD8
		public static void Show()
		{
			if (FormulasIndexView.Instance == null)
			{
				return;
			}
			FormulasIndexView.Instance.Open(null);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000718F3 File Offset: 0x0006FAF3
		public bool SetSelection(FormulasIndexEntry selection)
		{
			this.selectedEntry = selection;
			return true;
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x00071900 File Offset: 0x0006FB00
		protected override void OnOpen()
		{
			base.OnOpen();
			this.selectedEntry = null;
			this.Pool.ReleaseAll();
			foreach (CraftingFormula craftingFormula in CraftingFormulaCollection.Instance.Entries)
			{
				if (!craftingFormula.hideInIndex && (!GameMetaData.Instance.IsDemo || !craftingFormula.lockInDemo))
				{
					this.Pool.Get(null).Setup(this, craftingFormula);
				}
			}
			this.RefreshDetails();
			this.fadeGroup.Show();
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000719A4 File Offset: 0x0006FBA4
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000719B8 File Offset: 0x0006FBB8
		internal void OnEntryClicked(FormulasIndexEntry entry)
		{
			FormulasIndexEntry formulasIndexEntry = this.selectedEntry;
			this.selectedEntry = entry;
			this.selectedEntry.Refresh();
			if (formulasIndexEntry)
			{
				formulasIndexEntry.Refresh();
			}
			this.RefreshDetails();
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000719F4 File Offset: 0x0006FBF4
		private void RefreshDetails()
		{
			if (this.selectedEntry && this.selectedEntry.Valid)
			{
				this.detailsDisplay.Setup(new CraftingFormula?(this.selectedEntry.Formula));
				return;
			}
			this.detailsDisplay.Setup(null);
		}

		// Token: 0x04001612 RID: 5650
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001613 RID: 5651
		[SerializeField]
		private FormulasIndexEntry entryTemplate;

		// Token: 0x04001614 RID: 5652
		[SerializeField]
		private FormulasDetailsDisplay detailsDisplay;

		// Token: 0x04001615 RID: 5653
		private PrefabPool<FormulasIndexEntry> _pool;

		// Token: 0x04001616 RID: 5654
		private FormulasIndexEntry selectedEntry;
	}
}

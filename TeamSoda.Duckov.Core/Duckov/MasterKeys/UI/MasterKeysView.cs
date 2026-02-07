using System;
using Duckov.UI;
using Duckov.UI.Animations;
using UnityEngine;

namespace Duckov.MasterKeys.UI
{
	// Token: 0x020002F2 RID: 754
	public class MasterKeysView : View, ISingleSelectionMenu<MasterKeysIndexEntry>
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001871 RID: 6257 RVA: 0x0005A249 File Offset: 0x00058449
		public static MasterKeysView Instance
		{
			get
			{
				return View.GetViewInstance<MasterKeysView>();
			}
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0005A250 File Offset: 0x00058450
		protected override void Awake()
		{
			base.Awake();
			this.listDisplay.onEntryPointerClicked += this.OnEntryClicked;
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x0005A26F File Offset: 0x0005846F
		private void OnEntryClicked(MasterKeysIndexEntry entry)
		{
			this.RefreshInspectorDisplay();
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x0005A277 File Offset: 0x00058477
		public MasterKeysIndexEntry GetSelection()
		{
			return this.listDisplay.GetSelection();
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x0005A284 File Offset: 0x00058484
		public bool SetSelection(MasterKeysIndexEntry selection)
		{
			this.listDisplay.GetSelection();
			return true;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x0005A293 File Offset: 0x00058493
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.SetSelection(null);
			this.RefreshListDisplay();
			this.RefreshInspectorDisplay();
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x0005A2BA File Offset: 0x000584BA
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x0005A2CD File Offset: 0x000584CD
		private void RefreshListDisplay()
		{
			this.listDisplay.Refresh();
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x0005A2DC File Offset: 0x000584DC
		private void RefreshInspectorDisplay()
		{
			MasterKeysIndexEntry selection = this.GetSelection();
			this.inspector.Setup(selection);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x0005A2FC File Offset: 0x000584FC
		internal static void Show()
		{
			if (MasterKeysView.Instance == null)
			{
				Debug.Log(" Master keys view Instance is null");
				return;
			}
			MasterKeysView.Instance.Open(null);
		}

		// Token: 0x040011DA RID: 4570
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040011DB RID: 4571
		[SerializeField]
		private MasterKeysIndexList listDisplay;

		// Token: 0x040011DC RID: 4572
		[SerializeField]
		private MasterKeysIndexInspector inspector;
	}
}

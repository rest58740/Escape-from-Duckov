using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x02000220 RID: 544
	public class BaitSelectPanel : MonoBehaviour, ISingleSelectionMenu<BaitSelectPanelEntry>
	{
		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x00040D20 File Offset: 0x0003EF20
		private PrefabPool<BaitSelectPanelEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<BaitSelectPanelEntry>(this.entry, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x14000078 RID: 120
		// (add) Token: 0x06001064 RID: 4196 RVA: 0x00040D5C File Offset: 0x0003EF5C
		// (remove) Token: 0x06001065 RID: 4197 RVA: 0x00040D94 File Offset: 0x0003EF94
		internal event Action onSetSelection;

		// Token: 0x06001066 RID: 4198 RVA: 0x00040DCC File Offset: 0x0003EFCC
		internal UniTask DoBaitSelection(ICollection<Item> availableBaits, Func<Item, bool> baitSelectionResultCallback)
		{
			BaitSelectPanel.<DoBaitSelection>d__12 <DoBaitSelection>d__;
			<DoBaitSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoBaitSelection>d__.<>4__this = this;
			<DoBaitSelection>d__.availableBaits = availableBaits;
			<DoBaitSelection>d__.baitSelectionResultCallback = baitSelectionResultCallback;
			<DoBaitSelection>d__.<>1__state = -1;
			<DoBaitSelection>d__.<>t__builder.Start<BaitSelectPanel.<DoBaitSelection>d__12>(ref <DoBaitSelection>d__);
			return <DoBaitSelection>d__.<>t__builder.Task;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x00040E1F File Offset: 0x0003F01F
		private void Open()
		{
			this.fadeGroup.Show();
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x00040E2C File Offset: 0x0003F02C
		private void Close()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x00040E39 File Offset: 0x0003F039
		private Item SelectedItem
		{
			get
			{
				BaitSelectPanelEntry baitSelectPanelEntry = this.selectedEntry;
				if (baitSelectPanelEntry == null)
				{
					return null;
				}
				return baitSelectPanelEntry.Target;
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00040E4C File Offset: 0x0003F04C
		private UniTask<Item> WaitForSelection()
		{
			BaitSelectPanel.<WaitForSelection>d__20 <WaitForSelection>d__;
			<WaitForSelection>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForSelection>d__.<>4__this = this;
			<WaitForSelection>d__.<>1__state = -1;
			<WaitForSelection>d__.<>t__builder.Start<BaitSelectPanel.<WaitForSelection>d__20>(ref <WaitForSelection>d__);
			return <WaitForSelection>d__.<>t__builder.Task;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00040E90 File Offset: 0x0003F090
		private void Setup(ICollection<Item> availableBaits)
		{
			this.selectedEntry = null;
			this.EntryPool.ReleaseAll();
			foreach (Item cur in availableBaits)
			{
				this.EntryPool.Get(null).Setup(this, cur);
			}
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x00040EF8 File Offset: 0x0003F0F8
		internal void NotifyStop()
		{
			this.Close();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x00040F00 File Offset: 0x0003F100
		private void Awake()
		{
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00040F3A File Offset: 0x0003F13A
		private void OnConfirmButtonClicked()
		{
			if (this.SelectedItem == null)
			{
				NotificationText.Push(LocalizationManager.ToPlainText("Fishing_PleaseSelectBait"));
				return;
			}
			this.confirmed = true;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00040F61 File Offset: 0x0003F161
		private void OnCancelButtonClicked()
		{
			this.canceled = true;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00040F6A File Offset: 0x0003F16A
		internal void NotifySelect(BaitSelectPanelEntry baitSelectPanelEntry)
		{
			this.SetSelection(baitSelectPanelEntry);
			if (this.SelectedItem != null)
			{
				this.details.Setup(this.SelectedItem);
				this.detailsFadeGroup.Show();
				return;
			}
			this.detailsFadeGroup.SkipHide();
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00040FAA File Offset: 0x0003F1AA
		public BaitSelectPanelEntry GetSelection()
		{
			return this.selectedEntry;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00040FB2 File Offset: 0x0003F1B2
		public bool SetSelection(BaitSelectPanelEntry selection)
		{
			this.selectedEntry = selection;
			Action action = this.onSetSelection;
			if (action != null)
			{
				action();
			}
			return true;
		}

		// Token: 0x04000D3B RID: 3387
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000D3C RID: 3388
		[SerializeField]
		private Button confirmButton;

		// Token: 0x04000D3D RID: 3389
		[SerializeField]
		private Button cancelButton;

		// Token: 0x04000D3E RID: 3390
		[SerializeField]
		private ItemDetailsDisplay details;

		// Token: 0x04000D3F RID: 3391
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x04000D40 RID: 3392
		[SerializeField]
		private BaitSelectPanelEntry entry;

		// Token: 0x04000D41 RID: 3393
		private PrefabPool<BaitSelectPanelEntry> _entryPool;

		// Token: 0x04000D43 RID: 3395
		private BaitSelectPanelEntry selectedEntry;

		// Token: 0x04000D44 RID: 3396
		private bool canceled;

		// Token: 0x04000D45 RID: 3397
		private bool confirmed;
	}
}

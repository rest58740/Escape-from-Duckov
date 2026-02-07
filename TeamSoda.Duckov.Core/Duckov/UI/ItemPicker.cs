using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x02000395 RID: 917
	public class ItemPicker : MonoBehaviour
	{
		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x00070C70 File Offset: 0x0006EE70
		// (set) Token: 0x06001FF2 RID: 8178 RVA: 0x00070C77 File Offset: 0x0006EE77
		public static ItemPicker Instance { get; private set; }

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06001FF3 RID: 8179 RVA: 0x00070C80 File Offset: 0x0006EE80
		private PrefabPool<ItemPickerEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<ItemPickerEntry>(this.entryPrefab, this.contentParent ? this.contentParent : base.transform, new Action<ItemPickerEntry>(this.OnGetEntry), null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x00070CDE File Offset: 0x0006EEDE
		private void OnGetEntry(ItemPickerEntry entry)
		{
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06001FF5 RID: 8181 RVA: 0x00070CE0 File Offset: 0x0006EEE0
		public bool Picking
		{
			get
			{
				return this.picking;
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00070CE8 File Offset: 0x0006EEE8
		private UniTask<Item> WaitForUserPick(ICollection<Item> candidates)
		{
			ItemPicker.<WaitForUserPick>d__19 <WaitForUserPick>d__;
			<WaitForUserPick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<WaitForUserPick>d__.<>4__this = this;
			<WaitForUserPick>d__.candidates = candidates;
			<WaitForUserPick>d__.<>1__state = -1;
			<WaitForUserPick>d__.<>t__builder.Start<ItemPicker.<WaitForUserPick>d__19>(ref <WaitForUserPick>d__);
			return <WaitForUserPick>d__.<>t__builder.Task;
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x00070D34 File Offset: 0x0006EF34
		private void Awake()
		{
			if (ItemPicker.Instance == null)
			{
				ItemPicker.Instance = this;
			}
			else
			{
				Debug.LogError("场景中存在两个ItemPicker，请检查。");
			}
			this.confirmButton.onClick.AddListener(new UnityAction(this.OnConfirmButtonClicked));
			this.cancelButton.onClick.AddListener(new UnityAction(this.OnCancelButtonClicked));
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x00070D98 File Offset: 0x0006EF98
		private void OnCancelButtonClicked()
		{
			this.Cancel();
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x00070DA0 File Offset: 0x0006EFA0
		private void OnConfirmButtonClicked()
		{
			this.ConfirmPick(this.pickedItem);
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x00070DAE File Offset: 0x0006EFAE
		private void OnDestroy()
		{
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x00070DB0 File Offset: 0x0006EFB0
		private void Update()
		{
			if (!this.picking && this.fadeGroup.IsShown)
			{
				this.fadeGroup.Hide();
			}
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x00070DD4 File Offset: 0x0006EFD4
		public static UniTask<Item> Pick(ICollection<Item> candidates)
		{
			ItemPicker.<Pick>d__25 <Pick>d__;
			<Pick>d__.<>t__builder = AsyncUniTaskMethodBuilder<Item>.Create();
			<Pick>d__.candidates = candidates;
			<Pick>d__.<>1__state = -1;
			<Pick>d__.<>t__builder.Start<ItemPicker.<Pick>d__25>(ref <Pick>d__);
			return <Pick>d__.<>t__builder.Task;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00070E17 File Offset: 0x0006F017
		public void ConfirmPick(Item item)
		{
			this.confirmed = true;
			this.pickedItem = item;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00070E27 File Offset: 0x0006F027
		public void Cancel()
		{
			this.canceled = true;
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x00070E30 File Offset: 0x0006F030
		private void SetupUI(ICollection<Item> candidates)
		{
			this.EntryPool.ReleaseAll();
			foreach (Item item in candidates)
			{
				if (!(item == null))
				{
					ItemPickerEntry itemPickerEntry = this.EntryPool.Get(null);
					itemPickerEntry.Setup(this, item);
					itemPickerEntry.transform.SetAsLastSibling();
				}
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x00070EA4 File Offset: 0x0006F0A4
		internal void NotifyEntryClicked(ItemPickerEntry itemPickerEntry, Item target)
		{
			this.pickedItem = target;
		}

		// Token: 0x040015E4 RID: 5604
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040015E5 RID: 5605
		[SerializeField]
		private ItemPickerEntry entryPrefab;

		// Token: 0x040015E6 RID: 5606
		[SerializeField]
		private Transform contentParent;

		// Token: 0x040015E7 RID: 5607
		[SerializeField]
		private Button confirmButton;

		// Token: 0x040015E8 RID: 5608
		[SerializeField]
		private Button cancelButton;

		// Token: 0x040015E9 RID: 5609
		private PrefabPool<ItemPickerEntry> _entryPool;

		// Token: 0x040015EA RID: 5610
		private bool picking;

		// Token: 0x040015EB RID: 5611
		private bool canceled;

		// Token: 0x040015EC RID: 5612
		private bool confirmed;

		// Token: 0x040015ED RID: 5613
		private Item pickedItem;
	}
}

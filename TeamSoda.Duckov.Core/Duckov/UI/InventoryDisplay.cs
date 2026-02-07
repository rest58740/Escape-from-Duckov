using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003AB RID: 939
	public class InventoryDisplay : MonoBehaviour, IPoolable
	{
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x00073037 File Offset: 0x00071237
		private bool shortcuts
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x0007303A File Offset: 0x0007123A
		public bool UsePages
		{
			get
			{
				return this.usePages;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x060020CA RID: 8394 RVA: 0x00073042 File Offset: 0x00071242
		// (set) Token: 0x060020CB RID: 8395 RVA: 0x0007304A File Offset: 0x0007124A
		public bool Editable
		{
			get
			{
				return this.editable;
			}
			internal set
			{
				this.editable = value;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x060020CC RID: 8396 RVA: 0x00073053 File Offset: 0x00071253
		// (set) Token: 0x060020CD RID: 8397 RVA: 0x0007305B File Offset: 0x0007125B
		public bool ShowOperationButtons
		{
			get
			{
				return this.showOperationButtons;
			}
			internal set
			{
				this.showOperationButtons = value;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x060020CE RID: 8398 RVA: 0x00073064 File Offset: 0x00071264
		// (set) Token: 0x060020CF RID: 8399 RVA: 0x0007306C File Offset: 0x0007126C
		public bool Movable { get; private set; }

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x060020D0 RID: 8400 RVA: 0x00073075 File Offset: 0x00071275
		// (set) Token: 0x060020D1 RID: 8401 RVA: 0x0007307D File Offset: 0x0007127D
		public Inventory Target { get; private set; }

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00073088 File Offset: 0x00071288
		private PrefabPool<InventoryEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null && this.entryPrefab != null)
				{
					this._entryPool = new PrefabPool<InventoryEntry>(this.entryPrefab, this.contentLayout.transform, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x140000E0 RID: 224
		// (add) Token: 0x060020D3 RID: 8403 RVA: 0x000730DC File Offset: 0x000712DC
		// (remove) Token: 0x060020D4 RID: 8404 RVA: 0x00073114 File Offset: 0x00071314
		public event Action<InventoryDisplay, InventoryEntry, PointerEventData> onDisplayDoubleClicked;

		// Token: 0x140000E1 RID: 225
		// (add) Token: 0x060020D5 RID: 8405 RVA: 0x0007314C File Offset: 0x0007134C
		// (remove) Token: 0x060020D6 RID: 8406 RVA: 0x00073184 File Offset: 0x00071384
		public event Action onPageInfoRefreshed;

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x060020D7 RID: 8407 RVA: 0x000731B9 File Offset: 0x000713B9
		public Func<Item, bool> Func_ShouldHighlight
		{
			get
			{
				return this._func_ShouldHighlight;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x060020D8 RID: 8408 RVA: 0x000731C1 File Offset: 0x000713C1
		public Func<Item, bool> Func_CanOperate
		{
			get
			{
				return this._func_CanOperate;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x060020D9 RID: 8409 RVA: 0x000731C9 File Offset: 0x000713C9
		// (set) Token: 0x060020DA RID: 8410 RVA: 0x000731D1 File Offset: 0x000713D1
		public bool ShowSortButton
		{
			get
			{
				return this.showSortButton;
			}
			internal set
			{
				this.showSortButton = value;
			}
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000731DC File Offset: 0x000713DC
		private void RegisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.UnregisterEvents();
			this.Target.onContentChanged += this.OnTargetContentChanged;
			this.Target.onInventorySorted += this.OnTargetSorted;
			this.Target.onSetIndexLock += this.OnTargetSetIndexLock;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00073244 File Offset: 0x00071444
		private void UnregisterEvents()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.onContentChanged -= this.OnTargetContentChanged;
			this.Target.onInventorySorted -= this.OnTargetSorted;
			this.Target.onSetIndexLock -= this.OnTargetSetIndexLock;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000732A8 File Offset: 0x000714A8
		private void OnTargetSetIndexLock(Inventory inventory, int index)
		{
			foreach (InventoryEntry inventoryEntry in this.entries)
			{
				if (!(inventoryEntry == null) && inventoryEntry.isActiveAndEnabled && inventoryEntry.Index == index)
				{
					inventoryEntry.Refresh();
				}
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00073314 File Offset: 0x00071514
		private void OnTargetSorted(Inventory inventory)
		{
			if (this.filter == null)
			{
				using (List<InventoryEntry>.Enumerator enumerator = this.entries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						InventoryEntry inventoryEntry = enumerator.Current;
						inventoryEntry.Refresh();
					}
					return;
				}
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00073378 File Offset: 0x00071578
		private void OnTargetContentChanged(Inventory inventory, int position)
		{
			if (this.Target.Loading)
			{
				return;
			}
			if (this.filter != null)
			{
				this.RefreshCapacityText();
				this.LoadEntriesTask().Forget();
				return;
			}
			this.RefreshCapacityText();
			InventoryEntry inventoryEntry = this.entries.Find((InventoryEntry e) => e != null && e.Index == position);
			if (!inventoryEntry)
			{
				return;
			}
			InventoryEntry inventoryEntry2 = inventoryEntry;
			inventoryEntry2.Refresh();
			inventoryEntry2.Punch();
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000733F0 File Offset: 0x000715F0
		private void RefreshCapacityText()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.capacityText)
			{
				return;
			}
			this.capacityText.text = string.Format(this.capacityTextFormat, this.Target.Capacity, this.Target.GetItemCount());
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00073450 File Offset: 0x00071650
		public void Setup(Inventory target, Func<Item, bool> funcShouldHighLight = null, Func<Item, bool> funcCanOperate = null, bool movable = false, Func<Item, bool> filter = null)
		{
			this.UnregisterEvents();
			this.Target = target;
			this.Clear();
			if (this.Target == null)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			if (funcShouldHighLight == null)
			{
				this._func_ShouldHighlight = ((Item e) => false);
			}
			else
			{
				this._func_ShouldHighlight = funcShouldHighLight;
			}
			if (funcCanOperate == null)
			{
				this._func_CanOperate = ((Item e) => true);
			}
			else
			{
				this._func_CanOperate = funcCanOperate;
			}
			this.displayNameText.text = target.DisplayName;
			this.Movable = movable;
			this.cachedCapacity = target.Capacity;
			this.filter = filter;
			this.RefreshCapacityText();
			this.RegisterEvents();
			this.sortButton.gameObject.SetActive(this.editable && this.showSortButton);
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00073554 File Offset: 0x00071754
		private void RefreshGridLayoutPreferredHeight()
		{
			if (this.Target == null)
			{
				this.placeHolder.gameObject.SetActive(true);
				return;
			}
			int num = this.cachedIndexesToDisplay.Count;
			if (this.usePages && num > 0)
			{
				int num2 = this.cachedSelectedPage * this.itemsEachPage;
				int num3 = Mathf.Min(num2 + this.itemsEachPage, this.cachedIndexesToDisplay.Count);
				num = Mathf.Max(0, num3 - num2);
			}
			float preferredHeight = (float)Mathf.CeilToInt((float)num / (float)this.contentLayout.constraintCount) * this.contentLayout.cellSize.y + (float)this.contentLayout.padding.top + (float)this.contentLayout.padding.bottom;
			this.gridLayoutElement.preferredHeight = preferredHeight;
			this.placeHolder.gameObject.SetActive(num <= 0);
		}

		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x00073638 File Offset: 0x00071838
		public int MaxPage
		{
			get
			{
				return this.cachedMaxPage;
			}
		}

		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x00073640 File Offset: 0x00071840
		public int SelectedPage
		{
			get
			{
				return this.cachedSelectedPage;
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00073648 File Offset: 0x00071848
		public void SetPage(int page)
		{
			this.cachedSelectedPage = page;
			Action action = this.onPageInfoRefreshed;
			if (action != null)
			{
				action();
			}
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00073670 File Offset: 0x00071870
		public void NextPage()
		{
			int num = this.cachedSelectedPage + 1;
			if (num >= this.cachedMaxPage)
			{
				num = 0;
			}
			this.SetPage(num);
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00073698 File Offset: 0x00071898
		public void PreviousPage()
		{
			int num = this.cachedSelectedPage - 1;
			if (num < 0)
			{
				num = this.cachedMaxPage - 1;
			}
			this.SetPage(num);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000736C4 File Offset: 0x000718C4
		private void CacheIndexesToDisplay()
		{
			this.cachedIndexesToDisplay.Clear();
			int i = 0;
			while (i < this.Target.Capacity)
			{
				if (this.filter == null)
				{
					goto IL_32;
				}
				Item itemAt = this.Target.GetItemAt(i);
				if (this.filter(itemAt))
				{
					goto IL_32;
				}
				IL_3E:
				i++;
				continue;
				IL_32:
				this.cachedIndexesToDisplay.Add(i);
				goto IL_3E;
			}
			int count = this.cachedIndexesToDisplay.Count;
			this.cachedMaxPage = count / this.itemsEachPage + ((count % this.itemsEachPage > 0) ? 1 : 0);
			if (this.cachedSelectedPage >= this.cachedMaxPage)
			{
				this.cachedSelectedPage = Mathf.Max(0, this.cachedMaxPage - 1);
			}
			Action action = this.onPageInfoRefreshed;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00073780 File Offset: 0x00071980
		private UniTask LoadEntriesTask()
		{
			InventoryDisplay.<LoadEntriesTask>d__76 <LoadEntriesTask>d__;
			<LoadEntriesTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadEntriesTask>d__.<>4__this = this;
			<LoadEntriesTask>d__.<>1__state = -1;
			<LoadEntriesTask>d__.<>t__builder.Start<InventoryDisplay.<LoadEntriesTask>d__76>(ref <LoadEntriesTask>d__);
			return <LoadEntriesTask>d__.<>t__builder.Task;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000737C3 File Offset: 0x000719C3
		public void SetFilter(Func<Item, bool> filter)
		{
			this.filter = filter;
			this.cachedSelectedPage = 0;
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000737DE File Offset: 0x000719DE
		private void Clear()
		{
			this.EntryPool.ReleaseAll();
			this.entries.Clear();
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000737F6 File Offset: 0x000719F6
		private void Awake()
		{
			this.sortButton.onClick.AddListener(new UnityAction(this.OnSortButtonClicked));
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00073814 File Offset: 0x00071A14
		private void OnSortButtonClicked()
		{
			if (!this.Editable)
			{
				return;
			}
			if (!this.Target)
			{
				return;
			}
			if (this.Target.Loading)
			{
				return;
			}
			this.Target.Sort();
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00073846 File Offset: 0x00071A46
		private void OnEnable()
		{
			this.RegisterEvents();
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x0007384E File Offset: 0x00071A4E
		private void OnDisable()
		{
			this.UnregisterEvents();
			this.activeTaskToken++;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00073864 File Offset: 0x00071A64
		private void Update()
		{
			if (this.Target && this.cachedCapacity != this.Target.Capacity)
			{
				this.OnCapacityChanged();
			}
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x0007388C File Offset: 0x00071A8C
		private void OnCapacityChanged()
		{
			if (this.Target == null)
			{
				return;
			}
			this.cachedCapacity = this.Target.Capacity;
			this.RefreshCapacityText();
			this.LoadEntriesTask().Forget();
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000738BF File Offset: 0x00071ABF
		public bool IsShortcut(int index)
		{
			return this.shortcuts && index >= this.shortcutsRange.x && index <= this.shortcutsRange.y;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000738EC File Offset: 0x00071AEC
		private InventoryEntry GetNewInventoryEntry()
		{
			return this.EntryPool.Get(null);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000738FA File Offset: 0x00071AFA
		internal void NotifyItemDoubleClicked(InventoryEntry inventoryEntry, PointerEventData data)
		{
			Action<InventoryDisplay, InventoryEntry, PointerEventData> action = this.onDisplayDoubleClicked;
			if (action == null)
			{
				return;
			}
			action(this, inventoryEntry, data);
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x0007390F File Offset: 0x00071B0F
		public void NotifyPooled()
		{
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x00073911 File Offset: 0x00071B11
		public void NotifyReleased()
		{
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x00073914 File Offset: 0x00071B14
		public void DisableItem(Item item)
		{
			foreach (InventoryEntry inventoryEntry in from e in this.entries
			where e.Content == item
			select e)
			{
				inventoryEntry.Disabled = true;
			}
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x00073980 File Offset: 0x00071B80
		internal bool EvaluateShouldHighlight(Item content)
		{
			if (this.Func_ShouldHighlight != null && this.Func_ShouldHighlight(content))
			{
				return true;
			}
			content == null;
			return false;
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x00073A09 File Offset: 0x00071C09
		[CompilerGenerated]
		private bool <LoadEntriesTask>g__TaskValid|76_0(ref InventoryDisplay.<>c__DisplayClass76_0 A_1)
		{
			return Application.isPlaying && A_1.token == this.activeTaskToken;
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x00073A24 File Offset: 0x00071C24
		[CompilerGenerated]
		private List<int> <LoadEntriesTask>g__GetRange|76_1(int begin, int end_exclusive, List<int> list, ref InventoryDisplay.<>c__DisplayClass76_0 A_4)
		{
			if (begin < 0)
			{
				begin = 0;
			}
			if (end_exclusive < 0)
			{
				end_exclusive = 0;
			}
			A_4.indexes = new List<int>();
			if (end_exclusive > list.Count)
			{
				end_exclusive = list.Count;
			}
			if (begin >= end_exclusive)
			{
				return A_4.indexes;
			}
			for (int i = begin; i < end_exclusive; i++)
			{
				A_4.indexes.Add(list[i]);
			}
			return A_4.indexes;
		}

		// Token: 0x04001668 RID: 5736
		[SerializeField]
		private InventoryEntry entryPrefab;

		// Token: 0x04001669 RID: 5737
		[SerializeField]
		private TextMeshProUGUI displayNameText;

		// Token: 0x0400166A RID: 5738
		[SerializeField]
		private TextMeshProUGUI capacityText;

		// Token: 0x0400166B RID: 5739
		[SerializeField]
		private string capacityTextFormat = "({1}/{0})";

		// Token: 0x0400166C RID: 5740
		[SerializeField]
		private FadeGroup loadingIndcator;

		// Token: 0x0400166D RID: 5741
		[SerializeField]
		private FadeGroup contentFadeGroup;

		// Token: 0x0400166E RID: 5742
		[SerializeField]
		private GridLayoutGroup contentLayout;

		// Token: 0x0400166F RID: 5743
		[SerializeField]
		private LayoutElement gridLayoutElement;

		// Token: 0x04001670 RID: 5744
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x04001671 RID: 5745
		[SerializeField]
		private Transform entriesParent;

		// Token: 0x04001672 RID: 5746
		[SerializeField]
		private Button sortButton;

		// Token: 0x04001673 RID: 5747
		[SerializeField]
		private Vector2Int shortcutsRange = new Vector2Int(0, 3);

		// Token: 0x04001674 RID: 5748
		[SerializeField]
		private bool editable = true;

		// Token: 0x04001675 RID: 5749
		[SerializeField]
		private bool showOperationButtons = true;

		// Token: 0x04001676 RID: 5750
		[SerializeField]
		private bool showSortButton;

		// Token: 0x04001677 RID: 5751
		[SerializeField]
		private bool usePages;

		// Token: 0x04001678 RID: 5752
		[SerializeField]
		private int itemsEachPage = 30;

		// Token: 0x04001679 RID: 5753
		public Func<Item, bool> filter;

		// Token: 0x0400167C RID: 5756
		[SerializeField]
		private List<InventoryEntry> entries = new List<InventoryEntry>();

		// Token: 0x0400167D RID: 5757
		private PrefabPool<InventoryEntry> _entryPool;

		// Token: 0x04001680 RID: 5760
		private Func<Item, bool> _func_ShouldHighlight;

		// Token: 0x04001681 RID: 5761
		private Func<Item, bool> _func_CanOperate;

		// Token: 0x04001682 RID: 5762
		private int cachedCapacity = -1;

		// Token: 0x04001683 RID: 5763
		private int activeTaskToken;

		// Token: 0x04001684 RID: 5764
		private int cachedMaxPage = 1;

		// Token: 0x04001685 RID: 5765
		private int cachedSelectedPage;

		// Token: 0x04001686 RID: 5766
		private List<int> cachedIndexesToDisplay = new List<int>();
	}
}

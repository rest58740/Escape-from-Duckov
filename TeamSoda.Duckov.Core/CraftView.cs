using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001B3 RID: 435
public class CraftView : View, ISingleSelectionMenu<CraftView_ListEntry>
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0003742C File Offset: 0x0003562C
	private static CraftView Instance
	{
		get
		{
			return View.GetViewInstance<CraftView>();
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00037434 File Offset: 0x00035634
	private PrefabPool<CraftView_ListEntry> ListEntryPool
	{
		get
		{
			if (this._listEntryPool == null)
			{
				this._listEntryPool = new PrefabPool<CraftView_ListEntry>(this.listEntryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._listEntryPool;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0003746D File Offset: 0x0003566D
	private string NotificationFormat
	{
		get
		{
			return LocalizationManager.ToPlainText(this.notificationFormatKey);
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000D0B RID: 3339 RVA: 0x0003747C File Offset: 0x0003567C
	private PrefabPool<CraftViewFilterBtnEntry> FilterBtnPool
	{
		get
		{
			if (this._filterBtnPool == null)
			{
				this._filterBtnPool = new PrefabPool<CraftViewFilterBtnEntry>(this.filterBtnTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._filterBtnPool;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000D0C RID: 3340 RVA: 0x000374B5 File Offset: 0x000356B5
	private CraftView.FilterInfo CurrentFilter
	{
		get
		{
			if (this.currentFilterIndex < 0 || this.currentFilterIndex >= this.filters.Length)
			{
				this.currentFilterIndex = 0;
			}
			return this.filters[this.currentFilterIndex];
		}
	}

	// Token: 0x06000D0D RID: 3341 RVA: 0x000374E8 File Offset: 0x000356E8
	public void SetFilter(int index)
	{
		if (index < 0 || index >= this.filters.Length)
		{
			return;
		}
		this.currentFilterIndex = index;
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(this.predicate);
		this.RefreshFilterButtons();
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x00037520 File Offset: 0x00035720
	private static bool CheckFilter(CraftingFormula formula, CraftView.FilterInfo filter)
	{
		if (formula.result.id < 0)
		{
			return false;
		}
		if (filter.requireTags.Length == 0)
		{
			return true;
		}
		ItemMetaData metaData = ItemAssetsCollection.GetMetaData(formula.result.id);
		foreach (Tag value in filter.requireTags)
		{
			if (metaData.tags.Contains(value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00037583 File Offset: 0x00035783
	protected override void Awake()
	{
		base.Awake();
		this.listEntryTemplate.gameObject.SetActive(false);
		this.craftButton.onClick.AddListener(new UnityAction(this.OnCraftButtonClicked));
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x000375B8 File Offset: 0x000357B8
	private void OnCraftButtonClicked()
	{
		this.CraftTask().Forget();
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x000375C8 File Offset: 0x000357C8
	private UniTask CraftTask()
	{
		CraftView.<CraftTask>d__33 <CraftTask>d__;
		<CraftTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CraftTask>d__.<>4__this = this;
		<CraftTask>d__.<>1__state = -1;
		<CraftTask>d__.<>t__builder.Start<CraftView.<CraftTask>d__33>(ref <CraftTask>d__);
		return <CraftTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0003760C File Offset: 0x0003580C
	private void OnCraftFinished(Item item)
	{
		if (item == null)
		{
			return;
		}
		string displayName = item.DisplayName;
		NotificationText.Push(StringExtensions.Format(this.NotificationFormat, new
		{
			itemDisplayName = displayName
		}));
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x00037640 File Offset: 0x00035840
	protected override void OnOpen()
	{
		base.OnOpen();
		this.fadeGroup.Show();
		this.SetFilter(0);
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x0003765A File Offset: 0x0003585A
	protected override void OnClose()
	{
		base.OnClose();
		this.fadeGroup.Hide();
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x0003766D File Offset: 0x0003586D
	public static void SetupAndOpenView(Predicate<CraftingFormula> predicate)
	{
		if (!CraftView.Instance)
		{
			return;
		}
		CraftView.Instance.SetupAndOpen(predicate);
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x00037688 File Offset: 0x00035888
	public void SetupAndOpen(Predicate<CraftingFormula> predicate)
	{
		this.predicate = predicate;
		this.detailsFadeGroup.SkipHide();
		this.loadingIndicator.SkipHide();
		this.placeHolderFadeGroup.SkipShow();
		this.selectedEntry = null;
		this.RefreshDetails();
		this.RefreshList(predicate);
		this.RefreshFilterButtons();
		base.Open(null);
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x000376E0 File Offset: 0x000358E0
	private void RefreshList(Predicate<CraftingFormula> predicate)
	{
		this.ListEntryPool.ReleaseAll();
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		CraftView.FilterInfo currentFilter = this.CurrentFilter;
		bool flag = currentFilter.requireTags != null && currentFilter.requireTags.Length != 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, currentFilter)))
				{
					this.ListEntryPool.Get(null).Setup(this, craftingFormula);
				}
			}
		}
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x00037780 File Offset: 0x00035980
	private int CountFilter(CraftView.FilterInfo filter)
	{
		IEnumerable<string> unlockedFormulaIDs = CraftingManager.UnlockedFormulaIDs;
		bool flag = filter.requireTags != null && filter.requireTags.Length != 0;
		int num = 0;
		using (IEnumerator<string> enumerator = unlockedFormulaIDs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CraftingFormula craftingFormula;
				if (CraftingFormulaCollection.TryGetFormula(enumerator.Current, out craftingFormula) && this.predicate(craftingFormula) && (!flag || CraftView.CheckFilter(craftingFormula, filter)))
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x00037804 File Offset: 0x00035A04
	private void RefreshFilterButtons()
	{
		this.FilterBtnPool.ReleaseAll();
		int num = 0;
		foreach (CraftView.FilterInfo filterInfo in this.filters)
		{
			if (this.CountFilter(filterInfo) < 1)
			{
				num++;
			}
			else
			{
				this.FilterBtnPool.Get(null).Setup(this, filterInfo, num, num == this.currentFilterIndex);
				num++;
			}
		}
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0003786C File Offset: 0x00035A6C
	public CraftView_ListEntry GetSelection()
	{
		return this.selectedEntry;
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x00037874 File Offset: 0x00035A74
	public bool SetSelection(CraftView_ListEntry selection)
	{
		if (this.selectedEntry != null)
		{
			CraftView_ListEntry craftView_ListEntry = this.selectedEntry;
			this.selectedEntry = null;
			craftView_ListEntry.NotifyUnselected();
		}
		this.selectedEntry = selection;
		this.selectedEntry.NotifySelected();
		this.RefreshDetails();
		return true;
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x000378AF File Offset: 0x00035AAF
	private void RefreshDetails()
	{
		this.RefreshTask(this.NewRefreshToken()).Forget();
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x000378C4 File Offset: 0x00035AC4
	private int NewRefreshToken()
	{
		int num;
		do
		{
			num = Random.Range(0, int.MaxValue);
		}
		while (num == this.refreshTaskToken);
		this.refreshTaskToken = num;
		return num;
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x000378F0 File Offset: 0x00035AF0
	private UniTask RefreshTask(int token)
	{
		CraftView.<RefreshTask>d__50 <RefreshTask>d__;
		<RefreshTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<RefreshTask>d__.<>4__this = this;
		<RefreshTask>d__.token = token;
		<RefreshTask>d__.<>1__state = -1;
		<RefreshTask>d__.<>t__builder.Start<CraftView.<RefreshTask>d__50>(ref <RefreshTask>d__);
		return <RefreshTask>d__.<>t__builder.Task;
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0003793B File Offset: 0x00035B3B
	private void TestShow()
	{
		CraftingManager.UnlockFormula("Biscuit");
		CraftingManager.UnlockFormula("Character");
		this.SetupAndOpen((CraftingFormula e) => true);
	}

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	private FadeGroup fadeGroup;

	// Token: 0x04000B57 RID: 2903
	[SerializeField]
	private CraftView_ListEntry listEntryTemplate;

	// Token: 0x04000B58 RID: 2904
	private PrefabPool<CraftView_ListEntry> _listEntryPool;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private FadeGroup detailsFadeGroup;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	private FadeGroup loadingIndicator;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private FadeGroup placeHolderFadeGroup;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private ItemDetailsDisplay detailsDisplay;

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private CostDisplay costDisplay;

	// Token: 0x04000B5E RID: 2910
	[SerializeField]
	private Color crafableColor;

	// Token: 0x04000B5F RID: 2911
	[SerializeField]
	private Color notCraftableColor;

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	private Image buttonImage;

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private Button craftButton;

	// Token: 0x04000B62 RID: 2914
	[LocalizationKey("Default")]
	[SerializeField]
	private string notificationFormatKey;

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	private CraftViewFilterBtnEntry filterBtnTemplate;

	// Token: 0x04000B64 RID: 2916
	[SerializeField]
	private CraftView.FilterInfo[] filters;

	// Token: 0x04000B65 RID: 2917
	private PrefabPool<CraftViewFilterBtnEntry> _filterBtnPool;

	// Token: 0x04000B66 RID: 2918
	private int currentFilterIndex;

	// Token: 0x04000B67 RID: 2919
	private bool crafting;

	// Token: 0x04000B68 RID: 2920
	private Predicate<CraftingFormula> predicate;

	// Token: 0x04000B69 RID: 2921
	private CraftView_ListEntry selectedEntry;

	// Token: 0x04000B6A RID: 2922
	private int refreshTaskToken;

	// Token: 0x04000B6B RID: 2923
	private Item tempItem;

	// Token: 0x020004E8 RID: 1256
	[Serializable]
	public struct FilterInfo
	{
		// Token: 0x04001DA1 RID: 7585
		[LocalizationKey("Default")]
		[SerializeField]
		public string displayNameKey;

		// Token: 0x04001DA2 RID: 7586
		[SerializeField]
		public Sprite icon;

		// Token: 0x04001DA3 RID: 7587
		[SerializeField]
		public Tag[] requireTags;
	}
}

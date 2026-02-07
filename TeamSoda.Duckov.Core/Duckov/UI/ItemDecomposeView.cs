using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI.Animations;
using ItemStatsSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D1 RID: 977
	public class ItemDecomposeView : View
	{
		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x06002354 RID: 9044 RVA: 0x0007B87A File Offset: 0x00079A7A
		public static ItemDecomposeView Instance
		{
			get
			{
				return View.GetViewInstance<ItemDecomposeView>();
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x0007B881 File Offset: 0x00079A81
		private Item SelectedItem
		{
			get
			{
				return ItemUIUtilities.SelectedItem;
			}
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x0007B888 File Offset: 0x00079A88
		protected override void Awake()
		{
			base.Awake();
			this.decomposeButton.onClick.AddListener(new UnityAction(this.OnDecomposeButtonClick));
			this.countSlider.OnValueChangedEvent += this.OnSliderValueChanged;
			this.SetupEmpty();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x0007B8D4 File Offset: 0x00079AD4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.countSlider.OnValueChangedEvent -= this.OnSliderValueChanged;
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x0007B8F4 File Offset: 0x00079AF4
		private void OnDecomposeButtonClick()
		{
			if (this.decomposing)
			{
				return;
			}
			if (this.SelectedItem == null)
			{
				return;
			}
			int value = this.countSlider.Value;
			this.DecomposeTask(this.SelectedItem, value).Forget();
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x0007B937 File Offset: 0x00079B37
		private void OnFastPick(UIInputEventData data)
		{
			this.OnDecomposeButtonClick();
			data.Use();
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0007B948 File Offset: 0x00079B48
		private UniTask DecomposeTask(Item item, int count)
		{
			ItemDecomposeView.<DecomposeTask>d__21 <DecomposeTask>d__;
			<DecomposeTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DecomposeTask>d__.<>4__this = this;
			<DecomposeTask>d__.item = item;
			<DecomposeTask>d__.count = count;
			<DecomposeTask>d__.<>1__state = -1;
			<DecomposeTask>d__.<>t__builder.Start<ItemDecomposeView.<DecomposeTask>d__21>(ref <DecomposeTask>d__);
			return <DecomposeTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0007B99C File Offset: 0x00079B9C
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			ItemUIUtilities.Select(null);
			this.detailsFadeGroup.SkipHide();
			if (CharacterMainControl.Main != null)
			{
				this.characterInventoryDisplay.gameObject.SetActive(true);
				this.characterInventoryDisplay.Setup(CharacterMainControl.Main.CharacterItem.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.characterInventoryDisplay.gameObject.SetActive(false);
			}
			if (PlayerStorage.Inventory != null)
			{
				this.storageDisplay.gameObject.SetActive(true);
				this.storageDisplay.Setup(PlayerStorage.Inventory, null, (Item e) => e == null || DecomposeDatabase.CanDecompose(e.TypeID), false, null);
			}
			else
			{
				this.storageDisplay.gameObject.SetActive(false);
			}
			this.Refresh();
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0007BAA1 File Offset: 0x00079CA1
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0007BAB4 File Offset: 0x00079CB4
		private void OnEnable()
		{
			ItemUIUtilities.OnSelectionChanged += this.OnSelectionChanged;
			UIInputManager.OnFastPick += this.OnFastPick;
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0007BAD8 File Offset: 0x00079CD8
		private void OnDisable()
		{
			ItemUIUtilities.OnSelectionChanged -= this.OnSelectionChanged;
			UIInputManager.OnFastPick -= this.OnFastPick;
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007BAFC File Offset: 0x00079CFC
		private void OnSelectionChanged()
		{
			this.Refresh();
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0007BB04 File Offset: 0x00079D04
		private void OnSliderValueChanged(float value)
		{
			this.RefreshResult(this.SelectedItem, Mathf.RoundToInt(value));
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0007BB18 File Offset: 0x00079D18
		private void Refresh()
		{
			if (this.SelectedItem == null)
			{
				this.SetupEmpty();
				return;
			}
			this.Setup(this.SelectedItem);
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0007BB3C File Offset: 0x00079D3C
		private void SetupEmpty()
		{
			this.detailsFadeGroup.Hide();
			this.targetNameDisplay.text = "-";
			this.resultDisplay.Clear();
			this.cannotDecomposeIndicator.SetActive(false);
			this.decomposeButton.gameObject.SetActive(false);
			this.noItemSelectedIndicator.SetActive(true);
			this.busyIndicator.SetActive(false);
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0007BBC0 File Offset: 0x00079DC0
		private void Setup(Item selectedItem)
		{
			if (selectedItem == null)
			{
				return;
			}
			this.noItemSelectedIndicator.SetActive(false);
			this.detailsDisplay.Setup(selectedItem);
			this.detailsFadeGroup.Show();
			this.targetNameDisplay.text = selectedItem.DisplayName;
			bool valid = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID).valid;
			this.decomposeButton.gameObject.SetActive(valid);
			this.cannotDecomposeIndicator.gameObject.SetActive(!valid);
			this.SetupSlider(selectedItem);
			this.RefreshResult(selectedItem, Mathf.RoundToInt((float)this.countSlider.Value));
			this.busyIndicator.SetActive(this.decomposing);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0007BC74 File Offset: 0x00079E74
		private void SetupSlider(Item selectedItem)
		{
			if (selectedItem.Stackable)
			{
				this.countSlider.SetMinMax(1, selectedItem.StackCount);
				this.countSlider.Value = selectedItem.StackCount;
				return;
			}
			this.countSlider.SetMinMax(1, 1);
			this.countSlider.Value = 1;
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0007BCC8 File Offset: 0x00079EC8
		private void RefreshResult(Item selectedItem, int count)
		{
			if (selectedItem == null)
			{
				this.countSlider.SetMinMax(1, 1);
				this.countSlider.Value = 1;
				return;
			}
			DecomposeFormula decomposeFormula = DecomposeDatabase.GetDecomposeFormula(selectedItem.TypeID);
			if (decomposeFormula.valid)
			{
				bool stackable = selectedItem.Stackable;
				this.resultDisplay.Setup(decomposeFormula.result, count);
				return;
			}
			this.resultDisplay.Clear();
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0007BD34 File Offset: 0x00079F34
		internal static void Show()
		{
			ItemDecomposeView instance = ItemDecomposeView.Instance;
			if (instance == null)
			{
				return;
			}
			instance.Open(null);
		}

		// Token: 0x040017F7 RID: 6135
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040017F8 RID: 6136
		[SerializeField]
		private InventoryDisplay characterInventoryDisplay;

		// Token: 0x040017F9 RID: 6137
		[SerializeField]
		private InventoryDisplay storageDisplay;

		// Token: 0x040017FA RID: 6138
		[SerializeField]
		private FadeGroup detailsFadeGroup;

		// Token: 0x040017FB RID: 6139
		[SerializeField]
		private ItemDetailsDisplay detailsDisplay;

		// Token: 0x040017FC RID: 6140
		[SerializeField]
		private DecomposeSlider countSlider;

		// Token: 0x040017FD RID: 6141
		[SerializeField]
		private TextMeshProUGUI targetNameDisplay;

		// Token: 0x040017FE RID: 6142
		[SerializeField]
		private CostDisplay resultDisplay;

		// Token: 0x040017FF RID: 6143
		[SerializeField]
		private GameObject cannotDecomposeIndicator;

		// Token: 0x04001800 RID: 6144
		[SerializeField]
		private GameObject noItemSelectedIndicator;

		// Token: 0x04001801 RID: 6145
		[SerializeField]
		private Button decomposeButton;

		// Token: 0x04001802 RID: 6146
		[SerializeField]
		private GameObject busyIndicator;

		// Token: 0x04001803 RID: 6147
		private bool decomposing;
	}
}

using System;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Buildings.UI
{
	// Token: 0x02000331 RID: 817
	public class BuildingBtnEntry : MonoBehaviour
	{
		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x0006363A File Offset: 0x0006183A
		private string TokenFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.tokenFormatKey);
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x00063647 File Offset: 0x00061847
		public BuildingInfo Info
		{
			get
			{
				return this.info;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001B4F RID: 6991 RVA: 0x0006364F File Offset: 0x0006184F
		public bool CostEnough
		{
			get
			{
				return this.info.TokenAmount > 0 || this.info.cost.Enough;
			}
		}

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06001B50 RID: 6992 RVA: 0x00063678 File Offset: 0x00061878
		// (remove) Token: 0x06001B51 RID: 6993 RVA: 0x000636B0 File Offset: 0x000618B0
		public event Action<BuildingBtnEntry> onButtonClicked;

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x06001B52 RID: 6994 RVA: 0x000636E8 File Offset: 0x000618E8
		// (remove) Token: 0x06001B53 RID: 6995 RVA: 0x00063720 File Offset: 0x00061920
		public event Action<BuildingBtnEntry> onRecycleRequested;

		// Token: 0x06001B54 RID: 6996 RVA: 0x00063755 File Offset: 0x00061955
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			this.recycleButton.onPressFullfilled.AddListener(new UnityAction(this.OnRecycleButtonTriggered));
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0006378F File Offset: 0x0006198F
		private void OnRecycleButtonTriggered()
		{
			Action<BuildingBtnEntry> action = this.onRecycleRequested;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x000637A2 File Offset: 0x000619A2
		private void OnEnable()
		{
			BuildingManager.OnBuildingListChanged += this.Refresh;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x000637B5 File Offset: 0x000619B5
		private void OnDisable()
		{
			BuildingManager.OnBuildingListChanged -= this.Refresh;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000637C8 File Offset: 0x000619C8
		private void OnButtonClicked()
		{
			Action<BuildingBtnEntry> action = this.onButtonClicked;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000637DB File Offset: 0x000619DB
		internal void Setup(BuildingInfo buildingInfo)
		{
			this.info = buildingInfo;
			this.Refresh();
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000637EC File Offset: 0x000619EC
		private void Refresh()
		{
			int tokenAmount = this.info.TokenAmount;
			this.nameText.text = this.info.DisplayName;
			this.descriptionText.text = this.info.Description;
			this.tokenText.text = StringExtensions.Format(this.TokenFormat, new
			{
				tokenAmount
			});
			this.icon.sprite = this.info.iconReference;
			this.costDisplay.Setup(this.info.cost, 1);
			this.costDisplay.gameObject.SetActive(tokenAmount <= 0);
			bool reachedAmountLimit = this.info.ReachedAmountLimit;
			this.amountText.text = ((this.info.maxAmount > 0) ? string.Format("{0}/{1}", this.info.CurrentAmount, this.info.maxAmount) : string.Format("{0}/∞", this.info.CurrentAmount));
			this.reachedAmountLimitationIndicator.SetActive(reachedAmountLimit);
			bool flag = !this.info.ReachedAmountLimit && this.CostEnough;
			this.backGround.color = (flag ? this.avaliableColor : this.normalColor);
			this.recycleButton.gameObject.SetActive(this.info.CurrentAmount > 0);
		}

		// Token: 0x04001392 RID: 5010
		[SerializeField]
		private Button button;

		// Token: 0x04001393 RID: 5011
		[SerializeField]
		private Image icon;

		// Token: 0x04001394 RID: 5012
		[SerializeField]
		private TextMeshProUGUI nameText;

		// Token: 0x04001395 RID: 5013
		[SerializeField]
		private TextMeshProUGUI descriptionText;

		// Token: 0x04001396 RID: 5014
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x04001397 RID: 5015
		[SerializeField]
		private LongPressButton recycleButton;

		// Token: 0x04001398 RID: 5016
		[SerializeField]
		private TextMeshProUGUI amountText;

		// Token: 0x04001399 RID: 5017
		[SerializeField]
		[LocalizationKey("Default")]
		private string tokenFormatKey;

		// Token: 0x0400139A RID: 5018
		[SerializeField]
		private TextMeshProUGUI tokenText;

		// Token: 0x0400139B RID: 5019
		[SerializeField]
		private GameObject reachedAmountLimitationIndicator;

		// Token: 0x0400139C RID: 5020
		[SerializeField]
		private Image backGround;

		// Token: 0x0400139D RID: 5021
		[SerializeField]
		private Color normalColor;

		// Token: 0x0400139E RID: 5022
		[SerializeField]
		private Color avaliableColor;

		// Token: 0x0400139F RID: 5023
		private BuildingInfo info;
	}
}

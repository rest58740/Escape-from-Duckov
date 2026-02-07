using System;
using Duckov.UI;
using Duckov.UI.Animations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.BlackMarkets.UI
{
	// Token: 0x0200031F RID: 799
	public class BlackMarketView : View
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001A46 RID: 6726 RVA: 0x0005FFB2 File Offset: 0x0005E1B2
		public static BlackMarketView Instance
		{
			get
			{
				return View.GetViewInstance<BlackMarketView>();
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0005FFB9 File Offset: 0x0005E1B9
		protected override bool ShowOpenCloseButtons
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001A48 RID: 6728 RVA: 0x0005FFBC File Offset: 0x0005E1BC
		protected override void Awake()
		{
			base.Awake();
			this.btn_demandPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Demand);
			});
			this.btn_supplyPanel.onClick.AddListener(delegate()
			{
				this.SetMode(BlackMarketView.Mode.Supply);
			});
			this.btn_refresh.onClick.AddListener(new UnityAction(this.OnRefreshBtnClicked));
		}

		// Token: 0x06001A49 RID: 6729 RVA: 0x00060023 File Offset: 0x0005E223
		private void OnEnable()
		{
			BlackMarket.onRefreshChanceChanged += this.OnRefreshChanceChanced;
		}

		// Token: 0x06001A4A RID: 6730 RVA: 0x00060036 File Offset: 0x0005E236
		private void OnDisable()
		{
			BlackMarket.onRefreshChanceChanged -= this.OnRefreshChanceChanced;
		}

		// Token: 0x06001A4B RID: 6731 RVA: 0x00060049 File Offset: 0x0005E249
		private void OnRefreshChanceChanced(BlackMarket market)
		{
			this.RefreshRefreshButton();
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x00060054 File Offset: 0x0005E254
		private void RefreshRefreshButton()
		{
			if (this.Target == null)
			{
				this.refreshChanceText.text = "ERROR";
				this.refreshInteractableIndicator.SetActive(false);
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			this.refreshChanceText.text = string.Format("{0}/{1}", refreshChance, maxRefreshChance);
			this.refreshInteractableIndicator.SetActive(refreshChance > 0);
		}

		// Token: 0x06001A4D RID: 6733 RVA: 0x000600D3 File Offset: 0x0005E2D3
		private void OnRefreshBtnClicked()
		{
			if (this.Target == null)
			{
				return;
			}
			this.Target.PayAndRegenerate();
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001A4E RID: 6734 RVA: 0x000600EF File Offset: 0x0005E2EF
		// (set) Token: 0x06001A4F RID: 6735 RVA: 0x000600F7 File Offset: 0x0005E2F7
		public BlackMarket Target { get; private set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001A50 RID: 6736 RVA: 0x00060100 File Offset: 0x0005E300
		private bool ShowDemand
		{
			get
			{
				return (BlackMarketView.Mode.Demand | this.mode) == this.mode;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00060112 File Offset: 0x0005E312
		private bool ShowSupply
		{
			get
			{
				return (BlackMarketView.Mode.Supply | this.mode) == this.mode;
			}
		}

		// Token: 0x06001A52 RID: 6738 RVA: 0x00060124 File Offset: 0x0005E324
		public static void Show(BlackMarketView.Mode mode)
		{
			if (BlackMarketView.Instance == null)
			{
				return;
			}
			if (BlackMarket.Instance == null)
			{
				return;
			}
			BlackMarketView.Instance.Setup(BlackMarket.Instance, mode);
			BlackMarketView.Instance.Open(null);
		}

		// Token: 0x06001A53 RID: 6739 RVA: 0x0006015D File Offset: 0x0005E35D
		private void Setup(BlackMarket target, BlackMarketView.Mode mode)
		{
			this.Target = target;
			this.demandPanel.Setup(target);
			this.supplyPanel.Setup(target);
			this.RefreshRefreshButton();
			this.SetMode(mode);
			base.Open(null);
		}

		// Token: 0x06001A54 RID: 6740 RVA: 0x00060192 File Offset: 0x0005E392
		private void SetMode(BlackMarketView.Mode mode)
		{
			this.mode = mode;
			this.demandPanel.gameObject.SetActive(this.ShowDemand);
			this.supplyPanel.gameObject.SetActive(this.ShowSupply);
		}

		// Token: 0x06001A55 RID: 6741 RVA: 0x000601C7 File Offset: 0x0005E3C7
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x000601DA File Offset: 0x0005E3DA
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x000601F0 File Offset: 0x0005E3F0
		private void Update()
		{
			if (this.Target == null)
			{
				return;
			}
			int refreshChance = this.Target.RefreshChance;
			int maxRefreshChance = this.Target.MaxRefreshChance;
			string text;
			if (refreshChance < maxRefreshChance)
			{
				TimeSpan remainingTimeBeforeRefresh = this.Target.RemainingTimeBeforeRefresh;
				text = string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt((float)remainingTimeBeforeRefresh.TotalHours), remainingTimeBeforeRefresh.Minutes, remainingTimeBeforeRefresh.Seconds);
			}
			else
			{
				text = "--:--:--";
			}
			this.refreshETAText.text = text;
		}

		// Token: 0x0400130F RID: 4879
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001310 RID: 4880
		[SerializeField]
		private DemandPanel demandPanel;

		// Token: 0x04001311 RID: 4881
		[SerializeField]
		private SupplyPanel supplyPanel;

		// Token: 0x04001312 RID: 4882
		[SerializeField]
		private TextMeshProUGUI refreshETAText;

		// Token: 0x04001313 RID: 4883
		[SerializeField]
		private TextMeshProUGUI refreshChanceText;

		// Token: 0x04001314 RID: 4884
		[SerializeField]
		private Button btn_demandPanel;

		// Token: 0x04001315 RID: 4885
		[SerializeField]
		private Button btn_supplyPanel;

		// Token: 0x04001316 RID: 4886
		[SerializeField]
		private Button btn_refresh;

		// Token: 0x04001317 RID: 4887
		[SerializeField]
		private GameObject refreshInteractableIndicator;

		// Token: 0x04001319 RID: 4889
		private BlackMarketView.Mode mode;

		// Token: 0x020005C4 RID: 1476
		public enum Mode
		{
			// Token: 0x04002119 RID: 8473
			None,
			// Token: 0x0400211A RID: 8474
			Demand,
			// Token: 0x0400211B RID: 8475
			Supply
		}
	}
}

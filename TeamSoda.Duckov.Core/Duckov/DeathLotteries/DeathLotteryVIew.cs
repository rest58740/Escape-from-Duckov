using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Economy;
using Duckov.UI;
using Duckov.UI.Animations;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;

namespace Duckov.DeathLotteries
{
	// Token: 0x02000318 RID: 792
	public class DeathLotteryVIew : View
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x060019FC RID: 6652 RVA: 0x0005F135 File Offset: 0x0005D335
		private string RemainingTextFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.remainingTextFormatKey);
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x0005F142 File Offset: 0x0005D342
		public DeathLottery Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x060019FE RID: 6654 RVA: 0x0005F14A File Offset: 0x0005D34A
		public int RemainingChances
		{
			get
			{
				if (this.Target == null)
				{
					return 0;
				}
				return this.Target.RemainingChances;
			}
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x0005F167 File Offset: 0x0005D367
		protected override void OnOpen()
		{
			base.OnOpen();
			this.fadeGroup.Show();
			this.selectionBusyIndicator.SkipHide();
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x0005F185 File Offset: 0x0005D385
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0005F198 File Offset: 0x0005D398
		protected override void Awake()
		{
			base.Awake();
			DeathLottery.OnRequestUI += this.Show;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x0005F1B1 File Offset: 0x0005D3B1
		protected override void OnDestroy()
		{
			base.OnDestroy();
			DeathLottery.OnRequestUI -= this.Show;
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x0005F1CA File Offset: 0x0005D3CA
		private void Show(DeathLottery target)
		{
			this.target = target;
			this.Setup();
			base.Open(null);
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x0005F1E0 File Offset: 0x0005D3E0
		private void RefreshTexts()
		{
			this.remainingCountText.text = ((this.RemainingChances > 0) ? StringExtensions.Format(this.RemainingTextFormat, new
			{
				amount = this.RemainingChances
			}) : LocalizationManager.ToPlainText(this.noRemainingChances));
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x0005F21C File Offset: 0x0005D41C
		private void Setup()
		{
			if (this.target == null)
			{
				return;
			}
			if (this.target.Loading)
			{
				return;
			}
			DeathLottery.Status currentStatus = this.target.CurrentStatus;
			if (!currentStatus.valid)
			{
				return;
			}
			for (int i = 0; i < currentStatus.candidates.Count; i++)
			{
				this.cards[i].Setup(this, i);
			}
			this.RefreshTexts();
			this.HandleRemaining();
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x0005F28C File Offset: 0x0005D48C
		internal void NotifyEntryClicked(DeathLotteryCard deathLotteryCard, Cost cost)
		{
			if (deathLotteryCard == null)
			{
				return;
			}
			if (this.ProcessingSelection)
			{
				return;
			}
			if (this.RemainingChances <= 0)
			{
				return;
			}
			int index = deathLotteryCard.Index;
			if (this.target.CurrentStatus.selectedItems.Contains(index))
			{
				return;
			}
			this.selectTask = this.SelectTask(index, cost);
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0005F2E4 File Offset: 0x0005D4E4
		private bool ProcessingSelection
		{
			get
			{
				return this.selectTask.Status == UniTaskStatus.Pending;
			}
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x0005F2F4 File Offset: 0x0005D4F4
		private UniTask SelectTask(int index, Cost cost)
		{
			DeathLotteryVIew.<SelectTask>d__24 <SelectTask>d__;
			<SelectTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SelectTask>d__.<>4__this = this;
			<SelectTask>d__.index = index;
			<SelectTask>d__.cost = cost;
			<SelectTask>d__.<>1__state = -1;
			<SelectTask>d__.<>t__builder.Start<DeathLotteryVIew.<SelectTask>d__24>(ref <SelectTask>d__);
			return <SelectTask>d__.<>t__builder.Task;
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x0005F348 File Offset: 0x0005D548
		private void HandleRemaining()
		{
			if (this.RemainingChances > 0)
			{
				return;
			}
			DeathLotteryCard[] array = this.cards;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].NotifyFacing(true);
			}
		}

		// Token: 0x040012E5 RID: 4837
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x040012E6 RID: 4838
		[LocalizationKey("Default")]
		[SerializeField]
		private string remainingTextFormatKey = "DeathLottery_Remaining";

		// Token: 0x040012E7 RID: 4839
		[LocalizationKey("Default")]
		[SerializeField]
		private string noRemainingChances = "DeathLottery_NoRemainingChances";

		// Token: 0x040012E8 RID: 4840
		[SerializeField]
		private TextMeshProUGUI remainingCountText;

		// Token: 0x040012E9 RID: 4841
		[SerializeField]
		private DeathLotteryCard[] cards;

		// Token: 0x040012EA RID: 4842
		[SerializeField]
		private FadeGroup selectionBusyIndicator;

		// Token: 0x040012EB RID: 4843
		private DeathLottery target;

		// Token: 0x040012EC RID: 4844
		private UniTask selectTask;
	}
}

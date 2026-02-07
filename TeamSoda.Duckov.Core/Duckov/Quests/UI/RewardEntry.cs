using System;
using Duckov.Utilities;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.Quests.UI
{
	// Token: 0x02000366 RID: 870
	public class RewardEntry : MonoBehaviour, IPoolable
	{
		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x0006CE3D File Offset: 0x0006B03D
		// (set) Token: 0x06001E46 RID: 7750 RVA: 0x0006CE45 File Offset: 0x0006B045
		public bool Interactable
		{
			get
			{
				return this.interactable;
			}
			internal set
			{
				this.interactable = value;
			}
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0006CE4E File Offset: 0x0006B04E
		private void Awake()
		{
			this.claimButton.onClick.AddListener(new UnityAction(this.OnClaimButtonClicked));
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0006CE6C File Offset: 0x0006B06C
		private void OnClaimButtonClicked()
		{
			Reward reward = this.target;
			if (reward == null)
			{
				return;
			}
			reward.Claim();
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0006CE7E File Offset: 0x0006B07E
		public void NotifyPooled()
		{
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0006CE80 File Offset: 0x0006B080
		public void NotifyReleased()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0006CE88 File Offset: 0x0006B088
		internal void Setup(Reward target)
		{
			this.UnregisterEvents();
			this.target = target;
			if (target == null)
			{
				return;
			}
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0006CEAD File Offset: 0x0006B0AD
		private void RegisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged += this.OnTargetStatusChanged;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0006CED5 File Offset: 0x0006B0D5
		private void UnregisterEvents()
		{
			if (this.target == null)
			{
				return;
			}
			this.target.onStatusChanged -= this.OnTargetStatusChanged;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0006CEFD File Offset: 0x0006B0FD
		private void OnTargetStatusChanged()
		{
			this.Refresh();
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0006CF08 File Offset: 0x0006B108
		private void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.rewardText.text = this.target.Description;
			Sprite icon = this.target.Icon;
			this.rewardIcon.gameObject.SetActive(icon);
			this.rewardIcon.sprite = icon;
			bool claimed = this.target.Claimed;
			bool claimable = this.target.Claimable;
			bool flag = this.Interactable && claimable;
			bool active = !this.Interactable && claimable && !claimed;
			this.claimButton.gameObject.SetActive(flag);
			if (this.claimableIndicator != null)
			{
				this.claimableIndicator.SetActive(active);
			}
			if (flag)
			{
				if (this.buttonText)
				{
					this.buttonText.text = (claimed ? LocalizationManager.ToPlainText(this.claimedTextKey) : LocalizationManager.ToPlainText(this.claimTextKey));
				}
				this.statusIcon.sprite = (claimed ? this.claimedIcon : this.claimIcon);
				this.claimButton.interactable = !claimed;
				this.statusIcon.gameObject.SetActive(!this.target.Claiming);
				this.claimingIcon.gameObject.SetActive(this.target.Claiming);
			}
		}

		// Token: 0x04001504 RID: 5380
		[SerializeField]
		private Image rewardIcon;

		// Token: 0x04001505 RID: 5381
		[SerializeField]
		private TextMeshProUGUI rewardText;

		// Token: 0x04001506 RID: 5382
		[SerializeField]
		private Button claimButton;

		// Token: 0x04001507 RID: 5383
		[SerializeField]
		private GameObject claimableIndicator;

		// Token: 0x04001508 RID: 5384
		[SerializeField]
		private Image statusIcon;

		// Token: 0x04001509 RID: 5385
		[SerializeField]
		private TextMeshProUGUI buttonText;

		// Token: 0x0400150A RID: 5386
		[SerializeField]
		private GameObject claimingIcon;

		// Token: 0x0400150B RID: 5387
		[SerializeField]
		private Sprite claimIcon;

		// Token: 0x0400150C RID: 5388
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimTextKey = "UI_Quest_RewardClaim";

		// Token: 0x0400150D RID: 5389
		[SerializeField]
		private Sprite claimedIcon;

		// Token: 0x0400150E RID: 5390
		[LocalizationKey("Default")]
		[SerializeField]
		private string claimedTextKey = "UI_Quest_RewardClaimed";

		// Token: 0x0400150F RID: 5391
		[SerializeField]
		private bool interactable;

		// Token: 0x04001510 RID: 5392
		private Reward target;
	}
}

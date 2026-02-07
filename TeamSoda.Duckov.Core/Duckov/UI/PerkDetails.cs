using System;
using Duckov.PerkTrees;
using Duckov.UI.Animations;
using Duckov.Utilities;
using LeTai.TrueShadow;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003D6 RID: 982
	public class PerkDetails : MonoBehaviour
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x0007D4B3 File Offset: 0x0007B6B3
		[SerializeField]
		private string RequireLevelFormatKey
		{
			get
			{
				return "UI_Perk_RequireLevel";
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x060023B7 RID: 9143 RVA: 0x0007D4BA File Offset: 0x0007B6BA
		[SerializeField]
		private string RequireLevelFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.RequireLevelFormatKey);
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0007D4C7 File Offset: 0x0007B6C7
		private void Awake()
		{
			this.beginButton.onClick.AddListener(new UnityAction(this.OnBeginButtonClicked));
			this.activateButton.onClick.AddListener(new UnityAction(this.OnActivateButtonClicked));
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0007D501 File Offset: 0x0007B701
		private void OnActivateButtonClicked()
		{
			this.showingPerk.ConfirmUnlock();
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0007D50F File Offset: 0x0007B70F
		private void OnBeginButtonClicked()
		{
			this.showingPerk.SubmitItemsAndBeginUnlocking();
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0007D51D File Offset: 0x0007B71D
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0007D525 File Offset: 0x0007B725
		public void Setup(Perk perk, bool editable = false)
		{
			this.UnregisterEvents();
			this.showingPerk = perk;
			this.editable = editable;
			this.Refresh();
			this.RegisterEvents();
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0007D547 File Offset: 0x0007B747
		private void RegisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged += this.OnTargetStateChanged;
			}
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0007D56D File Offset: 0x0007B76D
		private void OnTargetStateChanged(Perk perk, bool arg2)
		{
			this.Refresh();
		}

		// Token: 0x060023BF RID: 9151 RVA: 0x0007D575 File Offset: 0x0007B775
		private void UnregisterEvents()
		{
			if (this.showingPerk)
			{
				this.showingPerk.onUnlockStateChanged -= this.OnTargetStateChanged;
			}
		}

		// Token: 0x060023C0 RID: 9152 RVA: 0x0007D59C File Offset: 0x0007B79C
		private void Refresh()
		{
			if (this.showingPerk == null)
			{
				this.content.Hide();
				this.placeHolder.Show();
				return;
			}
			this.text_Name.text = this.showingPerk.DisplayName;
			this.text_Description.text = this.showingPerk.Description;
			this.icon.sprite = this.showingPerk.Icon;
			ValueTuple<float, Color, bool> shadowOffsetAndColorOfQuality = GameplayDataSettings.UIStyle.GetShadowOffsetAndColorOfQuality(this.showingPerk.DisplayQuality);
			this.iconShadow.IgnoreCasterColor = true;
			this.iconShadow.Color = shadowOffsetAndColorOfQuality.Item2;
			this.iconShadow.Inset = shadowOffsetAndColorOfQuality.Item3;
			this.iconShadow.OffsetDistance = shadowOffsetAndColorOfQuality.Item1;
			bool flag = !this.showingPerk.Unlocked && this.editable;
			bool flag2 = this.showingPerk.AreAllParentsUnlocked();
			bool flag3 = false;
			if (flag2)
			{
				flag3 = this.showingPerk.Requirement.AreSatisfied();
			}
			this.activateButton.gameObject.SetActive(false);
			this.beginButton.gameObject.SetActive(false);
			this.buttonUnavaliablePlaceHolder.SetActive(false);
			this.buttonUnsatisfiedPlaceHolder.SetActive(false);
			this.inProgressPlaceHolder.SetActive(false);
			this.unlockedIndicator.SetActive(this.showingPerk.Unlocked);
			if (!this.showingPerk.Unlocked)
			{
				if (this.showingPerk.Unlocking)
				{
					if (this.showingPerk.GetRemainingTime() <= TimeSpan.Zero)
					{
						this.activateButton.gameObject.SetActive(true);
					}
					else
					{
						this.inProgressPlaceHolder.SetActive(true);
					}
				}
				else if (flag2)
				{
					if (flag3)
					{
						this.beginButton.gameObject.SetActive(true);
					}
					else
					{
						this.buttonUnsatisfiedPlaceHolder.SetActive(true);
					}
				}
				else
				{
					this.buttonUnavaliablePlaceHolder.SetActive(true);
				}
			}
			if (flag)
			{
				this.SetupActivationInfo();
			}
			this.activationInfoParent.SetActive(flag);
			this.content.Show();
			this.placeHolder.Hide();
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x0007D7AC File Offset: 0x0007B9AC
		private void SetupActivationInfo()
		{
			if (!this.showingPerk)
			{
				return;
			}
			int level = this.showingPerk.Requirement.level;
			if (level > 0)
			{
				bool flag = EXPManager.Level >= level;
				string text = "#" + (flag ? this.normalTextColor.ToHexString() : this.unsatisfiedTextColor.ToHexString());
				this.text_RequireLevel.gameObject.SetActive(true);
				int level2 = this.showingPerk.Requirement.level;
				string color = text;
				this.text_RequireLevel.text = StringExtensions.Format(this.RequireLevelFormat, new
				{
					level = level2,
					color = color
				});
			}
			else
			{
				this.text_RequireLevel.gameObject.SetActive(false);
			}
			this.costDisplay.Setup(this.showingPerk.Requirement.cost, 1);
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x0007D87C File Offset: 0x0007BA7C
		private void Update()
		{
			if (this.showingPerk && this.showingPerk.Unlocking && this.inProgressPlaceHolder.activeSelf)
			{
				this.UpdateCountDown();
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x0007D8AC File Offset: 0x0007BAAC
		private void UpdateCountDown()
		{
			TimeSpan remainingTime = this.showingPerk.GetRemainingTime();
			if (remainingTime <= TimeSpan.Zero)
			{
				this.Refresh();
				return;
			}
			this.progressFillImage.fillAmount = this.showingPerk.GetProgress01();
			this.countDownText.text = string.Format("{0} {1:00}:{2:00}:{3:00}.{4:000}", new object[]
			{
				remainingTime.Days,
				remainingTime.Hours,
				remainingTime.Minutes,
				remainingTime.Seconds,
				remainingTime.Milliseconds
			});
		}

		// Token: 0x04001845 RID: 6213
		[SerializeField]
		private FadeGroup content;

		// Token: 0x04001846 RID: 6214
		[SerializeField]
		private FadeGroup placeHolder;

		// Token: 0x04001847 RID: 6215
		[SerializeField]
		private TextMeshProUGUI text_Name;

		// Token: 0x04001848 RID: 6216
		[SerializeField]
		private TextMeshProUGUI text_Description;

		// Token: 0x04001849 RID: 6217
		[SerializeField]
		private Image icon;

		// Token: 0x0400184A RID: 6218
		[SerializeField]
		private TrueShadow iconShadow;

		// Token: 0x0400184B RID: 6219
		[SerializeField]
		private GameObject unlockedIndicator;

		// Token: 0x0400184C RID: 6220
		[SerializeField]
		private GameObject activationInfoParent;

		// Token: 0x0400184D RID: 6221
		[SerializeField]
		private TextMeshProUGUI text_RequireLevel;

		// Token: 0x0400184E RID: 6222
		[SerializeField]
		private CostDisplay costDisplay;

		// Token: 0x0400184F RID: 6223
		[SerializeField]
		private Color normalTextColor = Color.white;

		// Token: 0x04001850 RID: 6224
		[SerializeField]
		private Color unsatisfiedTextColor = Color.red;

		// Token: 0x04001851 RID: 6225
		[SerializeField]
		private Button activateButton;

		// Token: 0x04001852 RID: 6226
		[SerializeField]
		private Button beginButton;

		// Token: 0x04001853 RID: 6227
		[SerializeField]
		private GameObject buttonUnsatisfiedPlaceHolder;

		// Token: 0x04001854 RID: 6228
		[SerializeField]
		private GameObject buttonUnavaliablePlaceHolder;

		// Token: 0x04001855 RID: 6229
		[SerializeField]
		private GameObject inProgressPlaceHolder;

		// Token: 0x04001856 RID: 6230
		[SerializeField]
		private Image progressFillImage;

		// Token: 0x04001857 RID: 6231
		[SerializeField]
		private TextMeshProUGUI countDownText;

		// Token: 0x04001858 RID: 6232
		private Perk showingPerk;

		// Token: 0x04001859 RID: 6233
		private bool editable;
	}
}

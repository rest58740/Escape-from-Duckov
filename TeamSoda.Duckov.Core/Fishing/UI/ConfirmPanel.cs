using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.UI;
using Duckov.UI.Animations;
using ItemStatsSystem;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fishing.UI
{
	// Token: 0x02000222 RID: 546
	public class ConfirmPanel : MonoBehaviour
	{
		// Token: 0x0600107E RID: 4222 RVA: 0x000410D4 File Offset: 0x0003F2D4
		private void Awake()
		{
			this.continueButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
			this.quitButton.onClick.AddListener(new UnityAction(this.OnQuitButtonClicked));
			this.itemDisplay.onPointerClick += this.OnItemDisplayClick;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00041130 File Offset: 0x0003F330
		private void OnItemDisplayClick(ItemDisplay display, PointerEventData data)
		{
			data.Use();
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00041138 File Offset: 0x0003F338
		private void OnContinueButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = true;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00041148 File Offset: 0x0003F348
		private void OnQuitButtonClicked()
		{
			this.confirmed = true;
			this.continueFishing = false;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00041158 File Offset: 0x0003F358
		internal UniTask DoConfirmDialogue(Item catchedItem, Action<bool> confirmCallback)
		{
			ConfirmPanel.<DoConfirmDialogue>d__13 <DoConfirmDialogue>d__;
			<DoConfirmDialogue>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<DoConfirmDialogue>d__.<>4__this = this;
			<DoConfirmDialogue>d__.catchedItem = catchedItem;
			<DoConfirmDialogue>d__.confirmCallback = confirmCallback;
			<DoConfirmDialogue>d__.<>1__state = -1;
			<DoConfirmDialogue>d__.<>t__builder.Start<ConfirmPanel.<DoConfirmDialogue>d__13>(ref <DoConfirmDialogue>d__);
			return <DoConfirmDialogue>d__.<>t__builder.Task;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x000411AC File Offset: 0x0003F3AC
		private void Setup(Item item)
		{
			if (item == null)
			{
				this.titleText.text = LocalizationManager.ToPlainText(this.failedTextKey);
				this.itemDisplay.gameObject.SetActive(false);
				return;
			}
			this.titleText.text = LocalizationManager.ToPlainText(this.succeedTextKey);
			this.itemDisplay.Setup(item);
			this.itemDisplay.gameObject.SetActive(true);
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0004121D File Offset: 0x0003F41D
		internal void NotifyStop()
		{
			this.fadeGroup.Hide();
		}

		// Token: 0x04000D4A RID: 3402
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04000D4B RID: 3403
		[SerializeField]
		private TextMeshProUGUI titleText;

		// Token: 0x04000D4C RID: 3404
		[SerializeField]
		[LocalizationKey("Default")]
		private string succeedTextKey = "Fishing_Succeed";

		// Token: 0x04000D4D RID: 3405
		[SerializeField]
		[LocalizationKey("Default")]
		private string failedTextKey = "Fishing_Failed";

		// Token: 0x04000D4E RID: 3406
		[SerializeField]
		private ItemDisplay itemDisplay;

		// Token: 0x04000D4F RID: 3407
		[SerializeField]
		private Button continueButton;

		// Token: 0x04000D50 RID: 3408
		[SerializeField]
		private Button quitButton;

		// Token: 0x04000D51 RID: 3409
		private bool confirmed;

		// Token: 0x04000D52 RID: 3410
		private bool continueFishing;
	}
}

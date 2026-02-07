using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Rules;
using Duckov.UI.Animations;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x02000408 RID: 1032
	public class SavesButton : MonoBehaviour
	{
		// Token: 0x06002565 RID: 9573 RVA: 0x00082900 File Offset: 0x00080B00
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
			SavesSystem.OnSetFile += this.Refresh;
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
			SavesSystem.OnSaveDeleted += this.Refresh;
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x0008295C File Offset: 0x00080B5C
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
			SavesSystem.OnSaveDeleted -= this.Refresh;
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x00082991 File Offset: 0x00080B91
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x00082999 File Offset: 0x00080B99
		private void OnButtonClick()
		{
			if (!this.executing)
			{
				this.SavesSelectionTask().Forget();
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000829B0 File Offset: 0x00080BB0
		private UniTask SavesSelectionTask()
		{
			SavesButton.<SavesSelectionTask>d__12 <SavesSelectionTask>d__;
			<SavesSelectionTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<SavesSelectionTask>d__.<>4__this = this;
			<SavesSelectionTask>d__.<>1__state = -1;
			<SavesSelectionTask>d__.<>t__builder.Start<SavesButton.<SavesSelectionTask>d__12>(ref <SavesSelectionTask>d__);
			return <SavesSelectionTask>d__.<>t__builder.Task;
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000829F3 File Offset: 0x00080BF3
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000829FC File Offset: 0x00080BFC
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			string difficulty = flag ? GameRulesManager.Current.DisplayName : "";
			this.text.text = StringExtensions.Format(this.textFormat, new
			{
				text = LocalizationManager.ToPlainText(this.textKey),
				slotNumber = SavesSystem.CurrentSlot,
				difficulty = difficulty
			});
			bool active = flag && SavesSystem.IsOldSave(SavesSystem.CurrentSlot);
			this.oldSaveIndicator.SetActive(active);
		}

		// Token: 0x0400196B RID: 6507
		[SerializeField]
		private FadeGroup currentMenuFadeGroup;

		// Token: 0x0400196C RID: 6508
		[SerializeField]
		private SaveSlotSelectionMenu selectionMenu;

		// Token: 0x0400196D RID: 6509
		[SerializeField]
		private GameObject oldSaveIndicator;

		// Token: 0x0400196E RID: 6510
		[SerializeField]
		private Button button;

		// Token: 0x0400196F RID: 6511
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001970 RID: 6512
		[SerializeField]
		[LocalizationKey("Default")]
		private string textKey = "MainMenu_SaveSlot";

		// Token: 0x04001971 RID: 6513
		[SerializeField]
		private string textFormat = "{text}: {slotNumber}";

		// Token: 0x04001972 RID: 6514
		private bool executing;
	}
}

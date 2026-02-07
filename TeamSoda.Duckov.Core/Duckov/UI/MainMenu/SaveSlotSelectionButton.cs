using System;
using Duckov.Rules;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x02000409 RID: 1033
	public class SaveSlotSelectionButton : MonoBehaviour
	{
		// Token: 0x0600256D RID: 9581 RVA: 0x00082A89 File Offset: 0x00080C89
		private void Awake()
		{
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x00082AA7 File Offset: 0x00080CA7
		private void OnDestroy()
		{
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x00082AA9 File Offset: 0x00080CA9
		private void OnEnable()
		{
			SavesSystem.OnSetFile += this.Refresh;
			this.Refresh();
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x00082AC2 File Offset: 0x00080CC2
		private void OnDisable()
		{
			SavesSystem.OnSetFile -= this.Refresh;
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x00082AD5 File Offset: 0x00080CD5
		private void OnButtonClick()
		{
			SavesSystem.SetFile(this.index);
			this.menu.Finish();
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x00082AED File Offset: 0x00080CED
		private void OnValidate()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
			if (this.text == null)
			{
				this.text = base.GetComponentInChildren<TextMeshProUGUI>();
			}
			this.Refresh();
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x00082B2C File Offset: 0x00080D2C
		private void Refresh()
		{
			new ES3Settings(SavesSystem.GetFilePath(this.index), null).location = 0;
			this.text.text = StringExtensions.Format(this.format, new
			{
				slotText = LocalizationManager.ToPlainText(this.slotTextKey),
				index = this.index
			});
			bool active = SavesSystem.CurrentSlot == this.index;
			GameObject gameObject = this.activeIndicator;
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
			if (SavesSystem.IsOldGame(this.index))
			{
				this.difficultyText.text = (GameRulesManager.GetRuleIndexDisplayNameOfSlot(this.index) ?? "");
				this.playTimeText.gameObject.SetActive(true);
				TimeSpan realTimePlayedOfSaveSlot = GameClock.GetRealTimePlayedOfSaveSlot(this.index);
				this.playTimeText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt((float)realTimePlayedOfSaveSlot.TotalHours), realTimePlayedOfSaveSlot.Minutes);
				bool active2 = SavesSystem.IsOldSave(this.index);
				this.oldSlotIndicator.SetActive(active2);
				long num = SavesSystem.Load<long>("SaveTime", this.index);
				string text = (num > 0L) ? DateTime.FromBinary(num).ToLocalTime().ToString("yyyy/MM/dd HH:mm") : "???";
				this.saveTimeText.text = text;
				return;
			}
			this.difficultyText.text = LocalizationManager.ToPlainText(this.newGameTextKey);
			this.playTimeText.gameObject.SetActive(false);
			this.oldSlotIndicator.SetActive(false);
			this.saveTimeText.text = "----/--/-- --:--";
		}

		// Token: 0x04001973 RID: 6515
		[SerializeField]
		private SaveSlotSelectionMenu menu;

		// Token: 0x04001974 RID: 6516
		[SerializeField]
		private Button button;

		// Token: 0x04001975 RID: 6517
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001976 RID: 6518
		[SerializeField]
		private TextMeshProUGUI difficultyText;

		// Token: 0x04001977 RID: 6519
		[SerializeField]
		private TextMeshProUGUI playTimeText;

		// Token: 0x04001978 RID: 6520
		[SerializeField]
		private TextMeshProUGUI saveTimeText;

		// Token: 0x04001979 RID: 6521
		[SerializeField]
		private string slotTextKey = "MainMenu_SaveSelection_Slot";

		// Token: 0x0400197A RID: 6522
		[SerializeField]
		private string format = "{slotText} {index}";

		// Token: 0x0400197B RID: 6523
		[LocalizationKey("Default")]
		[SerializeField]
		private string newGameTextKey = "NewGame";

		// Token: 0x0400197C RID: 6524
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x0400197D RID: 6525
		[SerializeField]
		private GameObject oldSlotIndicator;

		// Token: 0x0400197E RID: 6526
		[Min(1f)]
		[SerializeField]
		private int index;
	}
}

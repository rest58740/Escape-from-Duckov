using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Modding.UI;
using Duckov.Scenes;
using Duckov.Utilities;
using Eflatun.SceneReference;
using Saves;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI.MainMenu
{
	// Token: 0x02000407 RID: 1031
	public class ContinueButton : MonoBehaviour
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x0008273D File Offset: 0x0008093D
		[SerializeField]
		private string Text_NewGame
		{
			get
			{
				return LocalizationManager.ToPlainText(this.text_NewGame);
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x0008274A File Offset: 0x0008094A
		[SerializeField]
		private string Text_Continue
		{
			get
			{
				return LocalizationManager.ToPlainText(this.text_Continue);
			}
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x00082758 File Offset: 0x00080958
		private void Awake()
		{
			SavesSystem.OnSetFile += this.Refresh;
			SavesSystem.OnSaveDeleted += this.Refresh;
			this.button.onClick.AddListener(new UnityAction(this.OnButtonClicked));
			LocalizationManager.OnSetLanguage += this.OnSetLanguage;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000827B4 File Offset: 0x000809B4
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Refresh;
			SavesSystem.OnSaveDeleted -= this.Refresh;
			LocalizationManager.OnSetLanguage -= this.OnSetLanguage;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000827E9 File Offset: 0x000809E9
		private void OnSetLanguage(SystemLanguage language)
		{
			this.Refresh();
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000827F4 File Offset: 0x000809F4
		private void OnButtonClicked()
		{
			GameManager.newBoot = true;
			if (MultiSceneCore.GetVisited("Base"))
			{
				this.LoadGame().Forget();
				return;
			}
			SavesSystem.Save<VersionData>("CreatedWithVersion", GameMetaData.Instance.Version);
			SceneLoader.Instance.LoadScene(GameplayDataSettings.SceneManagement.PrologueScene, this.overrideCurtainScene, false, false, true, false, default(MultiSceneLocation), true, false).Forget();
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x00082861 File Offset: 0x00080A61
		private void Start()
		{
			this.Refresh();
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x0008286C File Offset: 0x00080A6C
		private void Refresh()
		{
			bool flag = SavesSystem.IsOldGame();
			this.text.text = (flag ? this.Text_Continue : this.Text_NewGame);
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x0008289C File Offset: 0x00080A9C
		private UniTask LoadGame()
		{
			ContinueButton.<LoadGame>d__17 <LoadGame>d__;
			<LoadGame>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<LoadGame>d__.<>4__this = this;
			<LoadGame>d__.<>1__state = -1;
			<LoadGame>d__.<>t__builder.Start<ContinueButton.<LoadGame>d__17>(ref <LoadGame>d__);
			return <LoadGame>d__.<>t__builder.Task;
		}

		// Token: 0x04001964 RID: 6500
		[SerializeField]
		private Button button;

		// Token: 0x04001965 RID: 6501
		[SerializeField]
		private TextMeshProUGUI text;

		// Token: 0x04001966 RID: 6502
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_NewGame = "新游戏";

		// Token: 0x04001967 RID: 6503
		[LocalizationKey("Default")]
		[SerializeField]
		private string text_Continue = "继续";

		// Token: 0x04001968 RID: 6504
		[SerializeField]
		private SceneReference overrideCurtainScene;

		// Token: 0x04001969 RID: 6505
		[SerializeField]
		private ModChangedWarning modChangedWarning;

		// Token: 0x0400196A RID: 6506
		private bool loading;
	}
}

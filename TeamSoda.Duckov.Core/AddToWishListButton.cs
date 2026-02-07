using System;
using Duckov;
using SodaCraft.Localizations;
using Steamworks;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x0200016A RID: 362
public class AddToWishListButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06000B35 RID: 2869 RVA: 0x00030C1B File Offset: 0x0002EE1B
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == null)
		{
			AddToWishListButton.ShowPage();
		}
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x00030C2C File Offset: 0x0002EE2C
	public static void ShowPage()
	{
		if (SteamManager.Initialized)
		{
			SteamFriends.ActivateGameOverlayToStore(new AppId_t(3167020U), 0);
			return;
		}
		if (GameMetaData.Instance.Platform == Platform.Steam)
		{
			Application.OpenURL("https://store.steampowered.com/app/3167020/");
			return;
		}
		if (LocalizationManager.CurrentLanguage == 40)
		{
			Application.OpenURL("https://game.bilibili.com/duckov/");
			return;
		}
		Application.OpenURL("https://www.duckov.com");
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x00030C87 File Offset: 0x0002EE87
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x040009C3 RID: 2499
	private const string url = "https://store.steampowered.com/app/3167020/";

	// Token: 0x040009C4 RID: 2500
	private const string CNUrl = "https://game.bilibili.com/duckov/";

	// Token: 0x040009C5 RID: 2501
	private const string ENUrl = "https://www.duckov.com";

	// Token: 0x040009C6 RID: 2502
	private const uint appid = 3167020U;
}

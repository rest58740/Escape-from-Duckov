using System;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F3 RID: 499
public class SteamLogoImage : MonoBehaviour
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x0003CA4F File Offset: 0x0003AC4F
	private void Start()
	{
		this.Refresh();
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x0003CA58 File Offset: 0x0003AC58
	private void Refresh()
	{
		if (!SteamManager.Initialized)
		{
			this.image.sprite = this.steamLogo;
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.image.sprite = this.steamChinaLogo;
			return;
		}
		this.image.sprite = this.steamLogo;
	}

	// Token: 0x04000C6D RID: 3181
	[SerializeField]
	private Image image;

	// Token: 0x04000C6E RID: 3182
	[SerializeField]
	private Sprite steamLogo;

	// Token: 0x04000C6F RID: 3183
	[SerializeField]
	private Sprite steamChinaLogo;
}

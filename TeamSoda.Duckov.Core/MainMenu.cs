using System;
using UnityEngine;

// Token: 0x0200016E RID: 366
public class MainMenu : MonoBehaviour
{
	// Token: 0x06000B4D RID: 2893 RVA: 0x00030E90 File Offset: 0x0002F090
	private void Awake()
	{
		Action onMainMenuAwake = MainMenu.OnMainMenuAwake;
		if (onMainMenuAwake == null)
		{
			return;
		}
		onMainMenuAwake();
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x00030EA1 File Offset: 0x0002F0A1
	private void OnDestroy()
	{
		Action onMainMenuDestroy = MainMenu.OnMainMenuDestroy;
		if (onMainMenuDestroy == null)
		{
			return;
		}
		onMainMenuDestroy();
	}

	// Token: 0x040009CF RID: 2511
	public static Action OnMainMenuAwake;

	// Token: 0x040009D0 RID: 2512
	public static Action OnMainMenuDestroy;
}

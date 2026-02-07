using System;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001F2 RID: 498
public class EventIfSteamChina : MonoBehaviour
{
	// Token: 0x06000EED RID: 3821 RVA: 0x0003CA1F File Offset: 0x0003AC1F
	private void Start()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (SteamUtils.IsSteamChinaLauncher())
		{
			this.onStart_IsSteamChina.Invoke();
			return;
		}
		this.onStart_IsNotSteamChina.Invoke();
	}

	// Token: 0x04000C6B RID: 3179
	public UnityEvent onStart_IsSteamChina;

	// Token: 0x04000C6C RID: 3180
	public UnityEvent onStart_IsNotSteamChina;
}

using System;
using UnityEngine;

// Token: 0x0200009F RID: 159
public class TimeScaleManager : MonoBehaviour
{
	// Token: 0x0600056D RID: 1389 RVA: 0x000186A1 File Offset: 0x000168A1
	private void Awake()
	{
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x000186A4 File Offset: 0x000168A4
	private void Update()
	{
		float timeScale = 1f;
		if (GameManager.Paused)
		{
			timeScale = 0f;
		}
		if (CameraMode.Active)
		{
			timeScale = 0f;
		}
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = Mathf.Max(0.0005f, Time.timeScale * 0.02f);
	}
}

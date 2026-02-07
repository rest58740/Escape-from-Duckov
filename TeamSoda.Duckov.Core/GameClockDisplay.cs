using System;
using TMPro;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public class GameClockDisplay : MonoBehaviour
{
	// Token: 0x06000D9A RID: 3482 RVA: 0x00039215 File Offset: 0x00037415
	private void Awake()
	{
		this.Refresh();
	}

	// Token: 0x06000D9B RID: 3483 RVA: 0x0003921D File Offset: 0x0003741D
	private void OnEnable()
	{
		GameClock.OnGameClockStep += this.Refresh;
	}

	// Token: 0x06000D9C RID: 3484 RVA: 0x00039230 File Offset: 0x00037430
	private void OnDisable()
	{
		GameClock.OnGameClockStep -= this.Refresh;
	}

	// Token: 0x06000D9D RID: 3485 RVA: 0x00039244 File Offset: 0x00037444
	private void Refresh()
	{
		string text;
		if (GameClock.Instance == null)
		{
			text = "--:--";
		}
		else
		{
			text = string.Format("{0:00}:{1:00}", GameClock.Hour, GameClock.Minut);
		}
		this.text.text = text;
	}

	// Token: 0x04000BC0 RID: 3008
	[SerializeField]
	private TextMeshProUGUI text;
}

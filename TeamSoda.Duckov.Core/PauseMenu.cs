using System;

// Token: 0x02000172 RID: 370
public class PauseMenu : UIPanel
{
	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000B5C RID: 2908 RVA: 0x00031087 File Offset: 0x0002F287
	public static PauseMenu Instance
	{
		get
		{
			return GameManager.PauseMenu;
		}
	}

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x06000B5D RID: 2909 RVA: 0x00031090 File Offset: 0x0002F290
	// (remove) Token: 0x06000B5E RID: 2910 RVA: 0x000310C4 File Offset: 0x0002F2C4
	public static event Action onPauseMenuOn;

	// Token: 0x14000055 RID: 85
	// (add) Token: 0x06000B5F RID: 2911 RVA: 0x000310F8 File Offset: 0x0002F2F8
	// (remove) Token: 0x06000B60 RID: 2912 RVA: 0x0003112C File Offset: 0x0002F32C
	public static event Action onPauseMenuOff;

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0003115F File Offset: 0x0002F35F
	public bool Shown
	{
		get
		{
			return !(this.fadeGroup == null) && this.fadeGroup.IsShown;
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0003117C File Offset: 0x0002F37C
	public static void Show()
	{
		PauseMenu.Instance.Open(null, true);
		Action action = PauseMenu.onPauseMenuOn;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x00031199 File Offset: 0x0002F399
	public static void Hide()
	{
		PauseMenu.Instance.Close();
		Action action = PauseMenu.onPauseMenuOff;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x000311B4 File Offset: 0x0002F3B4
	public static void Toggle()
	{
		if (PauseMenu.Instance.fadeGroup.IsShown)
		{
			PauseMenu.Hide();
			return;
		}
		PauseMenu.Show();
	}
}

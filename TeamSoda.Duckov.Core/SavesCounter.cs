using System;
using Saves;

// Token: 0x0200012C RID: 300
public class SavesCounter
{
	// Token: 0x060009F9 RID: 2553 RVA: 0x0002BAA4 File Offset: 0x00029CA4
	public static int AddCount(string countKey)
	{
		int num = SavesSystem.Load<int>("Count/" + countKey);
		num++;
		SavesSystem.Save<int>("Count/" + countKey, num);
		return num;
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0002BAD8 File Offset: 0x00029CD8
	public static int GetCount(string countKey)
	{
		return SavesSystem.Load<int>("Count/" + countKey);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0002BAEC File Offset: 0x00029CEC
	public static int AddKillCount(string key)
	{
		int num = SavesCounter.AddCount("Kills/" + key);
		Action<string, int> onKillCountChanged = SavesCounter.OnKillCountChanged;
		if (onKillCountChanged != null)
		{
			onKillCountChanged(key, num);
		}
		return num;
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x0002BB1D File Offset: 0x00029D1D
	public static int GetKillCount(string key)
	{
		return SavesCounter.GetCount("Kills/" + key);
	}

	// Token: 0x040008D7 RID: 2263
	public static Action<string, int> OnKillCountChanged;
}

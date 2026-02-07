using System;
using Saves;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class GameClock : MonoBehaviour
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00038E5B File Offset: 0x0003705B
	// (set) Token: 0x06000D82 RID: 3458 RVA: 0x00038E62 File Offset: 0x00037062
	public static GameClock Instance { get; private set; }

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06000D83 RID: 3459 RVA: 0x00038E6C File Offset: 0x0003706C
	// (remove) Token: 0x06000D84 RID: 3460 RVA: 0x00038EA0 File Offset: 0x000370A0
	public static event Action OnGameClockStep;

	// Token: 0x06000D85 RID: 3461 RVA: 0x00038ED3 File Offset: 0x000370D3
	private void Awake()
	{
		if (GameClock.Instance != null)
		{
			Debug.LogError("检测到多个Game Clock");
			return;
		}
		GameClock.Instance = this;
		SavesSystem.OnCollectSaveData += this.Save;
		this.Load();
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x00038F0A File Offset: 0x0003710A
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00038F1D File Offset: 0x0003711D
	private static string SaveKey
	{
		get
		{
			return "GameClock";
		}
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x00038F24 File Offset: 0x00037124
	private void Save()
	{
		SavesSystem.Save<GameClock.SaveData>(GameClock.SaveKey, new GameClock.SaveData
		{
			days = this.days,
			secondsOfDay = this.secondsOfDay,
			realTimePlayedTicks = this.RealTimePlayed.Ticks
		});
	}

	// Token: 0x06000D89 RID: 3465 RVA: 0x00038F74 File Offset: 0x00037174
	private void Load()
	{
		GameClock.SaveData saveData = SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey);
		this.days = saveData.days;
		this.secondsOfDay = saveData.secondsOfDay;
		this.realTimePlayed = saveData.RealTimePlayed;
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D8A RID: 3466 RVA: 0x00038FC0 File Offset: 0x000371C0
	public static TimeSpan GetRealTimePlayedOfSaveSlot(int saveSlot)
	{
		return SavesSystem.Load<GameClock.SaveData>(GameClock.SaveKey, saveSlot).RealTimePlayed;
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000D8B RID: 3467 RVA: 0x00038FE0 File Offset: 0x000371E0
	private TimeSpan RealTimePlayed
	{
		get
		{
			return this.realTimePlayed;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00038FE8 File Offset: 0x000371E8
	private static double SecondsOfDay
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0.0;
			}
			return GameClock.Instance.secondsOfDay;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000D8D RID: 3469 RVA: 0x0003900C File Offset: 0x0003720C
	[TimeSpan]
	private long _TimeOfDayTicks
	{
		get
		{
			return GameClock.TimeOfDay.Ticks;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00039026 File Offset: 0x00037226
	public static TimeSpan TimeOfDay
	{
		get
		{
			return TimeSpan.FromSeconds(GameClock.SecondsOfDay);
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000D8F RID: 3471 RVA: 0x00039032 File Offset: 0x00037232
	public static long Day
	{
		get
		{
			if (GameClock.Instance == null)
			{
				return 0L;
			}
			return GameClock.Instance.days;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000D90 RID: 3472 RVA: 0x0003904E File Offset: 0x0003724E
	public static TimeSpan Now
	{
		get
		{
			return GameClock.TimeOfDay + TimeSpan.FromDays((double)GameClock.Day);
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000D91 RID: 3473 RVA: 0x00039068 File Offset: 0x00037268
	public static int Hour
	{
		get
		{
			return GameClock.TimeOfDay.Hours;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00039084 File Offset: 0x00037284
	public static int Minut
	{
		get
		{
			return GameClock.TimeOfDay.Minutes;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000D93 RID: 3475 RVA: 0x000390A0 File Offset: 0x000372A0
	public static int Seconds
	{
		get
		{
			return GameClock.TimeOfDay.Seconds;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000D94 RID: 3476 RVA: 0x000390BC File Offset: 0x000372BC
	public static int Milliseconds
	{
		get
		{
			return GameClock.TimeOfDay.Milliseconds;
		}
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x000390D6 File Offset: 0x000372D6
	private void Update()
	{
		this.StepTime(Time.deltaTime * this.clockTimeScale);
		this.realTimePlayed += TimeSpan.FromSeconds((double)Time.unscaledDeltaTime);
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x00039108 File Offset: 0x00037308
	private void StepTime(float deltaTime)
	{
		this.secondsOfDay += (double)deltaTime;
		while (this.secondsOfDay > 86300.0)
		{
			this.days += 1L;
			this.secondsOfDay -= 86300.0;
		}
		Action onGameClockStep = GameClock.OnGameClockStep;
		if (onGameClockStep == null)
		{
			return;
		}
		onGameClockStep();
	}

	// Token: 0x06000D97 RID: 3479 RVA: 0x0003916C File Offset: 0x0003736C
	public void StepTimeTil(TimeSpan time)
	{
		if (time.Days > 0)
		{
			time = new TimeSpan(time.Hours, time.Minutes, time.Seconds);
		}
		TimeSpan timeSpan;
		if (time > GameClock.TimeOfDay)
		{
			timeSpan = time - GameClock.TimeOfDay;
		}
		else
		{
			timeSpan = time + TimeSpan.FromDays(1.0) - GameClock.TimeOfDay;
		}
		this.StepTime((float)timeSpan.TotalSeconds);
	}

	// Token: 0x06000D98 RID: 3480 RVA: 0x000391E7 File Offset: 0x000373E7
	internal static void Step(float seconds)
	{
		if (GameClock.Instance == null)
		{
			return;
		}
		GameClock.Instance.StepTime(seconds);
	}

	// Token: 0x04000BBB RID: 3003
	public float clockTimeScale = 60f;

	// Token: 0x04000BBC RID: 3004
	private long days;

	// Token: 0x04000BBD RID: 3005
	private double secondsOfDay;

	// Token: 0x04000BBE RID: 3006
	private TimeSpan realTimePlayed;

	// Token: 0x04000BBF RID: 3007
	private const double SecondsPerDay = 86300.0;

	// Token: 0x020004EF RID: 1263
	[Serializable]
	private struct SaveData
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600283C RID: 10300 RVA: 0x00092988 File Offset: 0x00090B88
		public TimeSpan RealTimePlayed
		{
			get
			{
				return TimeSpan.FromTicks(this.realTimePlayedTicks);
			}
		}

		// Token: 0x04001DBE RID: 7614
		public long days;

		// Token: 0x04001DBF RID: 7615
		public double secondsOfDay;

		// Token: 0x04001DC0 RID: 7616
		public long realTimePlayedTicks;
	}
}

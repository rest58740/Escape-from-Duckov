using System;
using Duckov;
using Duckov.Scenes;
using Duckov.UI;
using Duckov.Weathers;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000198 RID: 408
public class TimeOfDayController : MonoBehaviour
{
	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000C46 RID: 3142 RVA: 0x00034840 File Offset: 0x00032A40
	public static TimeOfDayController Instance
	{
		get
		{
			if (!LevelManager.Instance)
			{
				return null;
			}
			return LevelManager.Instance.TimeOfDayController;
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0003485A File Offset: 0x00032A5A
	public bool AtNight
	{
		get
		{
			return this.atNight;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000C48 RID: 3144 RVA: 0x00034862 File Offset: 0x00032A62
	public TimeOfDayPhase CurrentPhase
	{
		get
		{
			return this.currentPhase;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0003486A File Offset: 0x00032A6A
	public Weather CurrentWeather
	{
		get
		{
			return this.currentWeather;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000C4A RID: 3146 RVA: 0x00034872 File Offset: 0x00032A72
	public float Time
	{
		get
		{
			return this.time;
		}
	}

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06000C4B RID: 3147 RVA: 0x0003487C File Offset: 0x00032A7C
	// (remove) Token: 0x06000C4C RID: 3148 RVA: 0x000348B0 File Offset: 0x00032AB0
	public static event Action OnStormStarted;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06000C4D RID: 3149 RVA: 0x000348E4 File Offset: 0x00032AE4
	// (remove) Token: 0x06000C4E RID: 3150 RVA: 0x00034918 File Offset: 0x00032B18
	public static event Action OnStormEnded;

	// Token: 0x06000C4F RID: 3151 RVA: 0x0003494C File Offset: 0x00032B4C
	private void Start()
	{
		this.config = LevelConfig.Instance.timeOfDayConfig;
		TimeOfDayController.coldLevel = 0f;
		TimeOfDayController.heatLevel = 0f;
		if (this.config.forceSetTime)
		{
			TimeSpan timeSpan = new TimeSpan(0, this.config.forceSetTimeTo, 0, 0);
			GameClock.Instance.StepTimeTil(timeSpan);
		}
		if (this.config.forceSetWeather)
		{
			WeatherManager.SetForceWeather(true, this.config.forceSetWeatherTo);
		}
		this.time = (float)GameClock.TimeOfDay.TotalHours % 24f;
		TimePhaseTags timePhaseTagByTime = this.GetTimePhaseTagByTime(this.time);
		this.atNight = (timePhaseTagByTime == TimePhaseTags.night);
		this.currentWeather = WeatherManager.GetWeather();
		this.OnWeatherChanged(this.currentWeather);
		this.currentPhase = this.config.GetCurrentEntry(this.CurrentWeather).GetPhase(timePhaseTagByTime);
		this.weatherVolumeControl.ForceSetProfile(this.currentPhase.volumeProfile);
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00034A44 File Offset: 0x00032C44
	private void Update()
	{
		this.time = (float)GameClock.TimeOfDay.TotalHours % 24f;
		TimePhaseTags timePhaseTagByTime = this.GetTimePhaseTagByTime(this.time);
		this.atNight = (timePhaseTagByTime == TimePhaseTags.night);
		Weather weather = WeatherManager.GetWeather();
		if (weather != this.currentWeather)
		{
			this.lastWeather = this.currentWeather;
			this.currentWeather = weather;
			this.OnWeatherChanged(this.currentWeather);
		}
		this.currentPhase = this.config.GetCurrentEntry(this.CurrentWeather).GetPhase(timePhaseTagByTime);
		if (this.weatherVolumeControl.CurrentProfile != this.currentPhase.volumeProfile && this.weatherVolumeControl.BufferTargetProfile != this.currentPhase.volumeProfile)
		{
			this.weatherVolumeControl.SetTargetProfile(this.currentPhase.volumeProfile);
		}
	}

	// Token: 0x06000C51 RID: 3153 RVA: 0x00034B20 File Offset: 0x00032D20
	private void OnWeatherChanged(Weather newWeather)
	{
		bool flag = false;
		if (MultiSceneCore.Instance)
		{
			SubSceneEntry subSceneInfo = MultiSceneCore.Instance.GetSubSceneInfo();
			if (subSceneInfo != null)
			{
				flag = subSceneInfo.IsInDoor;
			}
		}
		if (newWeather == Weather.Stormy_I)
		{
			this.stormIObject.SetActive(true);
			this.stormIIObject.SetActive(false);
			NotificationText.Push(LocalizationManager.ToPlainText("Weather_Storm_I"));
			if (!flag && LevelManager.AfterInit)
			{
				AudioManager.Post(this.stormPhaseISoundKey, base.gameObject);
			}
			try
			{
				if (this.lastWeather != Weather.Stormy_I && this.lastWeather != Weather.Stormy_II)
				{
					Action onStormStarted = TimeOfDayController.OnStormStarted;
					if (onStormStarted != null)
					{
						onStormStarted();
					}
				}
				return;
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
				return;
			}
		}
		if (newWeather == Weather.Stormy_II)
		{
			this.stormIObject.SetActive(false);
			this.stormIIObject.SetActive(true);
			NotificationText.Push(LocalizationManager.ToPlainText("Weather_Storm_II"));
			if (!flag && LevelManager.AfterInit)
			{
				AudioManager.Post(this.stormPhaseIISoundKey, base.gameObject);
			}
			try
			{
				if (this.lastWeather != Weather.Stormy_I && this.lastWeather != Weather.Stormy_II)
				{
					Action onStormStarted2 = TimeOfDayController.OnStormStarted;
					if (onStormStarted2 != null)
					{
						onStormStarted2();
					}
				}
				return;
			}
			catch (Exception ex2)
			{
				Debug.LogException(ex2);
				return;
			}
		}
		try
		{
			if (this.lastWeather == Weather.Stormy_I || this.lastWeather == Weather.Stormy_II)
			{
				Action onStormEnded = TimeOfDayController.OnStormEnded;
				if (onStormEnded != null)
				{
					onStormEnded();
				}
			}
		}
		catch (Exception ex3)
		{
			Debug.LogException(ex3);
		}
		this.stormIObject.SetActive(false);
		this.stormIIObject.SetActive(false);
	}

	// Token: 0x06000C52 RID: 3154 RVA: 0x00034CA8 File Offset: 0x00032EA8
	private TimePhaseTags GetTimePhaseTagByTime(float hourTime)
	{
		hourTime %= 24f;
		if (hourTime < this.morningStart || hourTime >= this.nightStart)
		{
			return TimePhaseTags.night;
		}
		if (hourTime >= this.morningStart && hourTime < this.dawnStart)
		{
			return TimePhaseTags.day;
		}
		if (hourTime >= this.dawnStart && hourTime < this.nightStart)
		{
			return TimePhaseTags.dawn;
		}
		return TimePhaseTags.day;
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x00034CFC File Offset: 0x00032EFC
	public static string GetTimePhaseNameByPhaseTag(TimePhaseTags phaseTag)
	{
		TimeOfDayController instance = TimeOfDayController.Instance;
		if (!instance)
		{
			return string.Empty;
		}
		switch (phaseTag)
		{
		case TimePhaseTags.day:
			return LocalizationManager.ToPlainText(instance.timePhaseKey_Day);
		case TimePhaseTags.dawn:
			return LocalizationManager.ToPlainText(instance.timePhaseKey_Dawn);
		case TimePhaseTags.night:
			return LocalizationManager.ToPlainText(instance.timePhaseKey_Night);
		default:
			return LocalizationManager.ToPlainText(instance.timePhaseKey_Day);
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00034D60 File Offset: 0x00032F60
	public static string GetWeatherNameByWeather(Weather weather)
	{
		TimeOfDayController instance = TimeOfDayController.Instance;
		if (!instance)
		{
			return string.Empty;
		}
		switch (weather)
		{
		case Weather.Sunny:
			return LocalizationManager.ToPlainText(instance.WeatherKey_Sunny);
		case Weather.Cloudy:
			return LocalizationManager.ToPlainText(instance.WeatherKey_Cloudy);
		case Weather.Rainy:
			return LocalizationManager.ToPlainText(instance.WeatherKey_Rainy);
		case Weather.Stormy_I:
			return LocalizationManager.ToPlainText(instance.WeatherKey_Storm_I);
		case Weather.Stormy_II:
			return LocalizationManager.ToPlainText(instance.WeatherKey_Storm_II);
		default:
			if (weather != Weather.Snow)
			{
				return LocalizationManager.ToPlainText(instance.WeatherKey_Sunny);
			}
			return LocalizationManager.ToPlainText(instance.WeatherKey_Snow);
		}
	}

	// Token: 0x04000AA2 RID: 2722
	private TimeOfDayConfig config;

	// Token: 0x04000AA3 RID: 2723
	private bool atNight;

	// Token: 0x04000AA4 RID: 2724
	[FormerlySerializedAs("volumeControl")]
	[SerializeField]
	private TimeOfDayVolumeControl weatherVolumeControl;

	// Token: 0x04000AA5 RID: 2725
	private TimeOfDayPhase currentPhase;

	// Token: 0x04000AA6 RID: 2726
	private Weather currentWeather;

	// Token: 0x04000AA7 RID: 2727
	public float morningStart = 5f;

	// Token: 0x04000AA8 RID: 2728
	public float dawnStart = 16f;

	// Token: 0x04000AA9 RID: 2729
	public float nightStart = 19f;

	// Token: 0x04000AAA RID: 2730
	public static float NightViewAngleFactor;

	// Token: 0x04000AAB RID: 2731
	public static float NightViewDistanceFactor;

	// Token: 0x04000AAC RID: 2732
	public static float NightSenseRangeFactor;

	// Token: 0x04000AAD RID: 2733
	public static float coldLevel;

	// Token: 0x04000AAE RID: 2734
	public static float heatLevel;

	// Token: 0x04000AAF RID: 2735
	[LocalizationKey("Default")]
	public string timePhaseKey_Day;

	// Token: 0x04000AB0 RID: 2736
	[LocalizationKey("Default")]
	public string timePhaseKey_Dawn;

	// Token: 0x04000AB1 RID: 2737
	[LocalizationKey("Default")]
	public string timePhaseKey_Night;

	// Token: 0x04000AB2 RID: 2738
	[LocalizationKey("Default")]
	public string WeatherKey_Sunny;

	// Token: 0x04000AB3 RID: 2739
	[LocalizationKey("Default")]
	public string WeatherKey_Cloudy;

	// Token: 0x04000AB4 RID: 2740
	[LocalizationKey("Default")]
	public string WeatherKey_Rainy;

	// Token: 0x04000AB5 RID: 2741
	[LocalizationKey("Default")]
	public string WeatherKey_Snow;

	// Token: 0x04000AB6 RID: 2742
	[LocalizationKey("Default")]
	public string WeatherKey_Storm_I;

	// Token: 0x04000AB7 RID: 2743
	[LocalizationKey("Default")]
	public string WeatherKey_Storm_II;

	// Token: 0x04000AB8 RID: 2744
	private string stormPhaseISoundKey = "Music/Stinger/stg_storm_1";

	// Token: 0x04000AB9 RID: 2745
	private string stormPhaseIISoundKey = "Music/Stinger/stg_storm_2";

	// Token: 0x04000ABA RID: 2746
	public GameObject stormIObject;

	// Token: 0x04000ABB RID: 2747
	public GameObject stormIIObject;

	// Token: 0x04000ABC RID: 2748
	private float time;

	// Token: 0x04000ABD RID: 2749
	private Weather lastWeather;
}

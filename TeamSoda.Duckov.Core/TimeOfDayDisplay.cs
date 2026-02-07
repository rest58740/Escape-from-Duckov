using System;
using Duckov.Weathers;
using SodaCraft.Localizations;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

// Token: 0x020000D4 RID: 212
public class TimeOfDayDisplay : MonoBehaviour
{
	// Token: 0x060006A0 RID: 1696 RVA: 0x0001E1C9 File Offset: 0x0001C3C9
	private void Start()
	{
		this.RefreshPhase(TimeOfDayController.Instance.CurrentPhase.timePhaseTag);
		this.RefreshWeather(TimeOfDayController.Instance.CurrentWeather);
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
	private void Update()
	{
		this.refreshTimer -= Time.unscaledDeltaTime;
		if (this.refreshTimer > 0f)
		{
			return;
		}
		this.refreshTimer = this.refreshTimeSpace;
		TimePhaseTags timePhaseTag = TimeOfDayController.Instance.CurrentPhase.timePhaseTag;
		if (this.currentPhaseTag != timePhaseTag)
		{
			this.RefreshPhase(timePhaseTag);
		}
		Weather weather = TimeOfDayController.Instance.CurrentWeather;
		if (this.currentWeather != weather)
		{
			this.RefreshWeather(weather);
		}
		this.RefreshStormText(weather);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0001E26C File Offset: 0x0001C46C
	private void RefreshStormText(Weather _weather)
	{
		TimeSpan timeSpan = default(TimeSpan);
		float fillAmount;
		if (_weather == Weather.Stormy_I)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = LocalizationManager.ToPlainText(this.StormPhaseIIETAKey);
			timeSpan = WeatherManager.Instance.Storm.GetStormIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else if (_weather == Weather.Stormy_II)
		{
			this.stormIndicatorAnimator.SetBool("Grow", false);
			this.stormTitleText.text = LocalizationManager.ToPlainText(this.StormOverETAKey);
			timeSpan = WeatherManager.Instance.Storm.GetStormIIOverETA(GameClock.Now);
			fillAmount = WeatherManager.Instance.Storm.GetStormRemainPercent(GameClock.Now);
			this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
		}
		else
		{
			this.stormIndicatorAnimator.SetBool("Grow", true);
			fillAmount = WeatherManager.Instance.Storm.GetSleepPercent(GameClock.Now);
			timeSpan = WeatherManager.Instance.Storm.GetStormETA(GameClock.Now);
			if (timeSpan.TotalHours < 24.0)
			{
				this.stormTitleText.text = LocalizationManager.ToPlainText(this.StormComingOneDayKey);
				this.stormDescObject.SetActive(LevelManager.Instance.IsBaseLevel);
			}
			else
			{
				this.stormTitleText.text = LocalizationManager.ToPlainText(this.StormComingETAKey);
				this.stormDescObject.SetActive(false);
			}
		}
		this.stormFillImage.fillAmount = fillAmount;
		this.stormText.text = string.Format("{0:000}:{1:00}", Mathf.FloorToInt((float)timeSpan.TotalHours), timeSpan.Minutes);
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001E440 File Offset: 0x0001C640
	private void RefreshPhase(TimePhaseTags _phase)
	{
		this.currentPhaseTag = _phase;
		this.phaseText.text = TimeOfDayController.GetTimePhaseNameByPhaseTag(_phase);
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0001E45A File Offset: 0x0001C65A
	private void RefreshWeather(Weather _weather)
	{
		this.currentWeather = _weather;
		this.weatherText.text = TimeOfDayController.GetWeatherNameByWeather(_weather);
	}

	// Token: 0x04000670 RID: 1648
	private TimePhaseTags currentPhaseTag;

	// Token: 0x04000671 RID: 1649
	private Weather currentWeather;

	// Token: 0x04000672 RID: 1650
	public TextMeshProUGUI phaseText;

	// Token: 0x04000673 RID: 1651
	public TextMeshProUGUI weatherText;

	// Token: 0x04000674 RID: 1652
	public TextMeshProUGUI stormTitleText;

	// Token: 0x04000675 RID: 1653
	public TextMeshProUGUI stormText;

	// Token: 0x04000676 RID: 1654
	[LocalizationKey("Default")]
	public string StormComingETAKey = "StormETA";

	// Token: 0x04000677 RID: 1655
	[LocalizationKey("Default")]
	public string StormComingOneDayKey = "StormOneDayETA";

	// Token: 0x04000678 RID: 1656
	[LocalizationKey("Default")]
	public string StormPhaseIIETAKey = "StormPhaseIIETA";

	// Token: 0x04000679 RID: 1657
	[LocalizationKey("Default")]
	public string StormOverETAKey = "StormOverETA";

	// Token: 0x0400067A RID: 1658
	public GameObject stormDescObject;

	// Token: 0x0400067B RID: 1659
	private float refreshTimeSpace = 0.5f;

	// Token: 0x0400067C RID: 1660
	private float refreshTimer;

	// Token: 0x0400067D RID: 1661
	public Animator stormIndicatorAnimator;

	// Token: 0x0400067E RID: 1662
	public ProceduralImage stormFillImage;
}

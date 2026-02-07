using System;
using Duckov.Weathers;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000197 RID: 407
public class TimeOfDayConfig : MonoBehaviour
{
	// Token: 0x06000C43 RID: 3139 RVA: 0x000346DC File Offset: 0x000328DC
	public TimeOfDayEntry GetCurrentEntry(Weather weather)
	{
		switch (weather)
		{
		case Weather.Sunny:
			return this.defaultEntry;
		case Weather.Cloudy:
			return this.cloudyEntry;
		case Weather.Rainy:
			return this.rainyEntry;
		case Weather.Stormy_I:
			return this.stormIEntry;
		case Weather.Stormy_II:
			return this.stormIIEntry;
		default:
			if (weather != Weather.Snow)
			{
				return this.defaultEntry;
			}
			return this.rainyEntry;
		}
	}

	// Token: 0x06000C44 RID: 3140 RVA: 0x0003473C File Offset: 0x0003293C
	public void InvokeDebug()
	{
		TimeOfDayEntry currentEntry = this.GetCurrentEntry(this.debugWeather);
		if (!currentEntry)
		{
			Debug.Log("No entry found");
			return;
		}
		TimeOfDayPhase phase = currentEntry.GetPhase(this.debugPhase);
		if (!Application.isPlaying)
		{
			if (this.lookDevVolume && this.lookDevVolume.profile != phase.volumeProfile)
			{
				this.lookDevVolume.profile = phase.volumeProfile;
				return;
			}
		}
		else
		{
			int num;
			switch (this.debugPhase)
			{
			case TimePhaseTags.day:
				num = 9;
				break;
			case TimePhaseTags.dawn:
				num = 17;
				break;
			case TimePhaseTags.night:
				num = 22;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			WeatherManager.SetForceWeather(true, this.debugWeather);
			TimeSpan time = new TimeSpan(num, 10, 0);
			GameClock.Instance.StepTimeTil(time);
			Debug.Log(string.Format("Set Weather to {0},and time to {1}", this.debugWeather, num));
		}
	}

	// Token: 0x04000A96 RID: 2710
	[SerializeField]
	private TimeOfDayEntry defaultEntry;

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	private TimeOfDayEntry cloudyEntry;

	// Token: 0x04000A98 RID: 2712
	[SerializeField]
	private TimeOfDayEntry rainyEntry;

	// Token: 0x04000A99 RID: 2713
	[SerializeField]
	private TimeOfDayEntry stormIEntry;

	// Token: 0x04000A9A RID: 2714
	[SerializeField]
	private TimeOfDayEntry stormIIEntry;

	// Token: 0x04000A9B RID: 2715
	public bool forceSetTime;

	// Token: 0x04000A9C RID: 2716
	[Range(0f, 24f)]
	public int forceSetTimeTo = 8;

	// Token: 0x04000A9D RID: 2717
	public bool forceSetWeather;

	// Token: 0x04000A9E RID: 2718
	public Weather forceSetWeatherTo;

	// Token: 0x04000A9F RID: 2719
	[SerializeField]
	private Volume lookDevVolume;

	// Token: 0x04000AA0 RID: 2720
	[SerializeField]
	private TimePhaseTags debugPhase;

	// Token: 0x04000AA1 RID: 2721
	[SerializeField]
	private Weather debugWeather;
}

using System;
using Saves;
using UnityEngine;

namespace Duckov.Weathers
{
	// Token: 0x0200024F RID: 591
	public class WeatherManager : MonoBehaviour
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x00047B09 File Offset: 0x00045D09
		public static Seasons Season
		{
			get
			{
				return LevelConfig.Season;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x0600129F RID: 4767 RVA: 0x00047B10 File Offset: 0x00045D10
		// (set) Token: 0x060012A0 RID: 4768 RVA: 0x00047B17 File Offset: 0x00045D17
		public static WeatherManager Instance { get; private set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x060012A1 RID: 4769 RVA: 0x00047B1F File Offset: 0x00045D1F
		// (set) Token: 0x060012A2 RID: 4770 RVA: 0x00047B27 File Offset: 0x00045D27
		public bool ForceWeather { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x00047B30 File Offset: 0x00045D30
		// (set) Token: 0x060012A4 RID: 4772 RVA: 0x00047B38 File Offset: 0x00045D38
		public Weather ForceWeatherValue { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x060012A5 RID: 4773 RVA: 0x00047B41 File Offset: 0x00045D41
		public Storm Storm
		{
			get
			{
				return this.storm;
			}
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x00047B49 File Offset: 0x00045D49
		private void Awake()
		{
			WeatherManager.Instance = this;
			SavesSystem.OnCollectSaveData += this.Save;
			this.Load();
			this._weatherDirty = true;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00047B6F File Offset: 0x00045D6F
		private void OnDestroy()
		{
			SavesSystem.OnCollectSaveData -= this.Save;
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00047B82 File Offset: 0x00045D82
		private void Save()
		{
			SavesSystem.Save<WeatherManager.SaveData>("WeatherManagerData", new WeatherManager.SaveData(this));
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x00047B94 File Offset: 0x00045D94
		private void Load()
		{
			WeatherManager.SaveData saveData = SavesSystem.Load<WeatherManager.SaveData>("WeatherManagerData");
			if (!saveData.valid)
			{
				this.SetRandomKey();
			}
			else
			{
				saveData.Setup(this);
			}
			this.SetupModules();
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00047BCA File Offset: 0x00045DCA
		private void SetRandomKey()
		{
			this.seed = Random.Range(0, 100000);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x00047BDD File Offset: 0x00045DDD
		private void SetupModules()
		{
			this.precipitation.SetSeed(this.seed);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00047BF0 File Offset: 0x00045DF0
		private Weather M_GetWeather(TimeSpan dayAndTime)
		{
			if (this.ForceWeather)
			{
				return this.ForceWeatherValue;
			}
			if (!this._weatherDirty && dayAndTime == this._cachedDayAndTime)
			{
				return this._cachedWeather;
			}
			int stormLevel = this.storm.GetStormLevel(dayAndTime);
			Weather weather;
			if (stormLevel > 0)
			{
				if (stormLevel == 1)
				{
					weather = Weather.Stormy_I;
				}
				else
				{
					weather = Weather.Stormy_II;
				}
			}
			else
			{
				float num = this.precipitation.Get(dayAndTime);
				if (num > this.precipitation.RainyThreshold)
				{
					weather = ((WeatherManager.Season == Seasons.winter) ? Weather.Snow : Weather.Rainy);
				}
				else if (num > this.precipitation.CloudyThreshold)
				{
					weather = Weather.Cloudy;
				}
				else
				{
					weather = Weather.Sunny;
				}
			}
			this._cachedDayAndTime = dayAndTime;
			this._cachedWeather = weather;
			this._weatherDirty = false;
			return weather;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x00047C9B File Offset: 0x00045E9B
		private void M_SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			this.ForceWeather = forceWeather;
			this.ForceWeatherValue = value;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x00047CAB File Offset: 0x00045EAB
		public static Weather GetWeather(TimeSpan dayAndTime)
		{
			if (WeatherManager.Instance == null)
			{
				return Weather.Sunny;
			}
			return WeatherManager.Instance.M_GetWeather(dayAndTime);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00047CC8 File Offset: 0x00045EC8
		public static Weather GetWeather()
		{
			TimeSpan now = GameClock.Now;
			if (WeatherManager.Instance && WeatherManager.Instance.ForceWeather)
			{
				return WeatherManager.Instance.ForceWeatherValue;
			}
			return WeatherManager.GetWeather(now);
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x00047D04 File Offset: 0x00045F04
		public static void SetForceWeather(bool forceWeather, Weather value = Weather.Sunny)
		{
			if (WeatherManager.Instance == null)
			{
				return;
			}
			WeatherManager.Instance.M_SetForceWeather(forceWeather, value);
		}

		// Token: 0x04000E54 RID: 3668
		private int seed = -1;

		// Token: 0x04000E55 RID: 3669
		[SerializeField]
		private Storm storm = new Storm();

		// Token: 0x04000E56 RID: 3670
		[SerializeField]
		private Precipitation precipitation = new Precipitation();

		// Token: 0x04000E57 RID: 3671
		private const string SaveKey = "WeatherManagerData";

		// Token: 0x04000E58 RID: 3672
		private Weather _cachedWeather;

		// Token: 0x04000E59 RID: 3673
		private TimeSpan _cachedDayAndTime;

		// Token: 0x04000E5A RID: 3674
		private bool _weatherDirty;

		// Token: 0x02000553 RID: 1363
		[Serializable]
		private struct SaveData
		{
			// Token: 0x060028F1 RID: 10481 RVA: 0x00096AB2 File Offset: 0x00094CB2
			public SaveData(WeatherManager weatherManager)
			{
				this = default(WeatherManager.SaveData);
				this.seed = weatherManager.seed;
				this.valid = true;
			}

			// Token: 0x060028F2 RID: 10482 RVA: 0x00096ACE File Offset: 0x00094CCE
			internal void Setup(WeatherManager weatherManager)
			{
				weatherManager.seed = this.seed;
			}

			// Token: 0x04001F66 RID: 8038
			public bool valid;

			// Token: 0x04001F67 RID: 8039
			public int seed;
		}
	}
}

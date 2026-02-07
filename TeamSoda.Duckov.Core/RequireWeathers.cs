using System;
using System.Collections.Generic;
using Duckov.Quests;
using Duckov.Weathers;

// Token: 0x02000125 RID: 293
public class RequireWeathers : Condition
{
	// Token: 0x060009D6 RID: 2518 RVA: 0x0002B624 File Offset: 0x00029824
	public override bool Evaluate()
	{
		if (!LevelManager.LevelInited)
		{
			return false;
		}
		Weather currentWeather = LevelManager.Instance.TimeOfDayController.CurrentWeather;
		return this.weathers.Contains(currentWeather);
	}

	// Token: 0x040008C7 RID: 2247
	public List<Weather> weathers;
}

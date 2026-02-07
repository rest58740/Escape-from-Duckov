using System;
using System.Collections.Generic;
using Duckov.Quests;
using Duckov.Weathers;

// Token: 0x02000124 RID: 292
public class RequireSeasons : Condition
{
	// Token: 0x060009D4 RID: 2516 RVA: 0x0002B5F4 File Offset: 0x000297F4
	public override bool Evaluate()
	{
		if (!LevelManager.LevelInited)
		{
			return false;
		}
		Seasons season = WeatherManager.Season;
		return this.seasons.Contains(season);
	}

	// Token: 0x040008C6 RID: 2246
	public List<Seasons> seasons;
}

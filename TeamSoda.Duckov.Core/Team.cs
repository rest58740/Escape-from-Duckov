using System;

// Token: 0x02000074 RID: 116
public class Team
{
	// Token: 0x06000459 RID: 1113 RVA: 0x00013F11 File Offset: 0x00012111
	public static bool IsEnemy(Teams selfTeam, Teams targetTeam)
	{
		return selfTeam != Teams.middle && (selfTeam == Teams.all || (targetTeam != Teams.middle && selfTeam != targetTeam));
	}
}

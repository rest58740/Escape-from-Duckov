using System;
using Duckov.Quests;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class Condition_TimeOfDay : Condition
{
	// Token: 0x060009C4 RID: 2500 RVA: 0x0002B374 File Offset: 0x00029574
	public override bool Evaluate()
	{
		float num = (float)GameClock.TimeOfDay.TotalHours % 24f;
		return (num >= this.from && num <= this.to) || (this.to < this.from && (num >= this.from || num <= this.to));
	}

	// Token: 0x040008B6 RID: 2230
	[Range(0f, 24f)]
	public float from;

	// Token: 0x040008B7 RID: 2231
	[Range(0f, 24f)]
	public float to;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class TimeOfDayEntry : MonoBehaviour
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x00034E34 File Offset: 0x00033034
	private void Start()
	{
		if (this.phases.Count > 0)
		{
			TimeOfDayPhase value = this.phases[0];
			this.phases[0] = value;
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x00034E6C File Offset: 0x0003306C
	public TimeOfDayPhase GetPhase(TimePhaseTags timePhaseTags)
	{
		for (int i = 0; i < this.phases.Count; i++)
		{
			TimeOfDayPhase timeOfDayPhase = this.phases[i];
			if (timeOfDayPhase.timePhaseTag == timePhaseTags)
			{
				return timeOfDayPhase;
			}
		}
		if (timePhaseTags == TimePhaseTags.dawn)
		{
			return this.GetPhase(TimePhaseTags.day);
		}
		return this.phases[0];
	}

	// Token: 0x04000AC0 RID: 2752
	[SerializeField]
	private List<TimeOfDayPhase> phases;
}

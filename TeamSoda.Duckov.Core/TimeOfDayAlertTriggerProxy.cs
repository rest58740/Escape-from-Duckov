using System;
using UnityEngine;

// Token: 0x020000D3 RID: 211
public class TimeOfDayAlertTriggerProxy : MonoBehaviour
{
	// Token: 0x0600069D RID: 1693 RVA: 0x0001E1B3 File Offset: 0x0001C3B3
	public void OnEnter()
	{
		TimeOfDayAlert.EnterAlertTrigger();
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x0001E1BA File Offset: 0x0001C3BA
	public void OnLeave()
	{
		TimeOfDayAlert.LeaveAlertTrigger();
	}
}

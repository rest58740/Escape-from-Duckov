using System;
using UnityEngine;

// Token: 0x0200013E RID: 318
public class LogOnEnableAndDisable : MonoBehaviour
{
	// Token: 0x06000A6F RID: 2671 RVA: 0x0002D208 File Offset: 0x0002B408
	private void OnEnable()
	{
		Debug.Log("OnEnable");
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0002D214 File Offset: 0x0002B414
	private void OnDisable()
	{
		Debug.Log("OnDisable");
	}
}

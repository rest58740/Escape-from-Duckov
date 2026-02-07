using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000193 RID: 403
public class SetActiveByEnding : MonoBehaviour
{
	// Token: 0x06000C36 RID: 3126 RVA: 0x00034449 File Offset: 0x00032649
	private void Start()
	{
		this.target.SetActive(this.endingIndexs.Contains(Ending.endingIndex));
	}

	// Token: 0x04000A8B RID: 2699
	public GameObject target;

	// Token: 0x04000A8C RID: 2700
	public List<int> endingIndexs;
}

using System;
using UnityEngine;

// Token: 0x02000157 RID: 343
public class SetActiveOnAwake : MonoBehaviour
{
	// Token: 0x06000ACC RID: 2764 RVA: 0x0002FD6C File Offset: 0x0002DF6C
	private void Awake()
	{
		this.target.SetActive(true);
	}

	// Token: 0x04000986 RID: 2438
	public GameObject target;
}

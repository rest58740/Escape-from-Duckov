using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class ShadowDistance : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x0000307F File Offset: 0x0000127F
	private void Awake()
	{
		QualitySettings.shadowDistance = this.shadowDistance;
	}

	// Token: 0x04000025 RID: 37
	public float shadowDistance = 2000f;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000009 RID: 9
[ExecuteInEditMode]
public class ES3GameObject : MonoBehaviour
{
	// Token: 0x060000DE RID: 222 RVA: 0x00004969 File Offset: 0x00002B69
	private void Update()
	{
		bool isPlaying = Application.isPlaying;
	}

	// Token: 0x04000017 RID: 23
	public List<Component> components = new List<Component>();
}

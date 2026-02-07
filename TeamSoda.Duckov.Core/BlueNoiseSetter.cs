using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000142 RID: 322
public class BlueNoiseSetter : MonoBehaviour
{
	// Token: 0x06000A7D RID: 2685 RVA: 0x0002D4B4 File Offset: 0x0002B6B4
	private void Update()
	{
		Shader.SetGlobalTexture("GlobalBlueNoise", this.blueNoises[this.index]);
		this.index++;
		if (this.index >= this.blueNoises.Count)
		{
			this.index = 0;
		}
	}

	// Token: 0x0400093C RID: 2364
	public List<Texture2D> blueNoises;

	// Token: 0x0400093D RID: 2365
	private int index;
}

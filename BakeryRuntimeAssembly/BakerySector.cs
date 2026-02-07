using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class BakerySector : MonoBehaviour
{
	// Token: 0x0600000B RID: 11 RVA: 0x00002430 File Offset: 0x00000630
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		for (int i = 0; i < this.cpoints.Count; i++)
		{
			if (this.cpoints[i] != null)
			{
				Gizmos.DrawWireSphere(this.cpoints[i].position, 1f);
			}
		}
	}

	// Token: 0x0400007C RID: 124
	public BakerySector.CaptureMode captureMode;

	// Token: 0x0400007D RID: 125
	public string captureAssetName = "";

	// Token: 0x0400007E RID: 126
	public BakerySectorCapture captureAsset;

	// Token: 0x0400007F RID: 127
	public bool allowUVPaddingAdjustment;

	// Token: 0x04000080 RID: 128
	public List<Transform> tforms = new List<Transform>();

	// Token: 0x04000081 RID: 129
	public List<Transform> cpoints = new List<Transform>();

	// Token: 0x0200001C RID: 28
	public enum CaptureMode
	{
		// Token: 0x040000E9 RID: 233
		None = -1,
		// Token: 0x040000EA RID: 234
		CaptureInPlace,
		// Token: 0x040000EB RID: 235
		CaptureToAsset,
		// Token: 0x040000EC RID: 236
		LoadCaptured
	}
}

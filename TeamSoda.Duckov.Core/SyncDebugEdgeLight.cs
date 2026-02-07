using System;
using Soda;
using UnityEngine;

// Token: 0x020000D1 RID: 209
public class SyncDebugEdgeLight : MonoBehaviour
{
	// Token: 0x06000690 RID: 1680 RVA: 0x0001DF24 File Offset: 0x0001C124
	private void Awake()
	{
		DebugView.OnDebugViewConfigChanged += this.OnDebugConfigChanged;
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0001DF37 File Offset: 0x0001C137
	private void OnDestroy()
	{
		DebugView.OnDebugViewConfigChanged -= this.OnDebugConfigChanged;
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0001DF4A File Offset: 0x0001C14A
	private void OnDebugConfigChanged(DebugView debugView)
	{
		if (debugView == null)
		{
			return;
		}
		base.gameObject.SetActive(debugView.EdgeLightActive);
	}
}

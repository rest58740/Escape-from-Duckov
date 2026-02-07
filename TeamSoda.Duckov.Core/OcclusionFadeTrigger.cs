using System;
using UnityEngine;

// Token: 0x02000190 RID: 400
public class OcclusionFadeTrigger : MonoBehaviour
{
	// Token: 0x06000C2B RID: 3115 RVA: 0x00034266 File Offset: 0x00032466
	private void Awake()
	{
		base.gameObject.layer = LayerMask.NameToLayer("VisualOcclusion");
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0003427D File Offset: 0x0003247D
	public void Enter()
	{
		this.parent.OnEnter();
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x0003428A File Offset: 0x0003248A
	public void Leave()
	{
		this.parent.OnLeave();
	}

	// Token: 0x04000A83 RID: 2691
	public OcclusionFadeObject parent;
}

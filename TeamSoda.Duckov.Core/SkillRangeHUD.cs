using System;
using UnityEngine;

// Token: 0x020000D0 RID: 208
public class SkillRangeHUD : MonoBehaviour
{
	// Token: 0x0600068D RID: 1677 RVA: 0x0001DEB8 File Offset: 0x0001C0B8
	public void SetRange(float range)
	{
		this.rangeTarget.localScale = Vector3.one * range;
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x0001DED0 File Offset: 0x0001C0D0
	public void SetProgress(float progress)
	{
		if (this.rangeMat == null)
		{
			this.rangeMat = this.rangeRenderer.material;
		}
		if (this.rangeMat == null)
		{
			return;
		}
		this.rangeMat.SetFloat("_Progress", progress);
	}

	// Token: 0x04000665 RID: 1637
	public Transform rangeTarget;

	// Token: 0x04000666 RID: 1638
	public Renderer rangeRenderer;

	// Token: 0x04000667 RID: 1639
	private Material rangeMat;
}

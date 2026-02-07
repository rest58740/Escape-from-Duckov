using System;
using UnityEngine;

// Token: 0x0200007B RID: 123
public class SingleCrosshair : MonoBehaviour
{
	// Token: 0x060004C1 RID: 1217 RVA: 0x00015E30 File Offset: 0x00014030
	public void UpdateScatter(float _scatter)
	{
		this.currentScatter = _scatter;
		RectTransform rectTransform = base.transform as RectTransform;
		rectTransform.localRotation = Quaternion.Euler(0f, 0f, this.rotation);
		Vector3 vector = Vector3.zero;
		if (this.axis != Vector3.zero)
		{
			vector = rectTransform.parent.InverseTransformDirection(rectTransform.TransformDirection(this.axis));
		}
		rectTransform.anchoredPosition = vector * (this.minDistance + this.currentScatter * this.scatterMoveScale);
		if (this.controlRectWidthHeight)
		{
			float num = this.minScale + this.currentScatter * this.scatterScaleFactor;
			rectTransform.sizeDelta = Vector2.one * num;
		}
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00015EEE File Offset: 0x000140EE
	private void OnValidate()
	{
		this.UpdateScatter(0f);
	}

	// Token: 0x04000403 RID: 1027
	public float rotation;

	// Token: 0x04000404 RID: 1028
	public Vector3 axis;

	// Token: 0x04000405 RID: 1029
	public float minDistance;

	// Token: 0x04000406 RID: 1030
	public float scatterMoveScale = 5f;

	// Token: 0x04000407 RID: 1031
	private float currentScatter;

	// Token: 0x04000408 RID: 1032
	public bool controlRectWidthHeight;

	// Token: 0x04000409 RID: 1033
	public float minScale = 100f;

	// Token: 0x0400040A RID: 1034
	public float scatterScaleFactor = 5f;
}

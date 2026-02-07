using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200013F RID: 319
public class MapImageToShader : MonoBehaviour
{
	// Token: 0x06000A72 RID: 2674 RVA: 0x0002D228 File Offset: 0x0002B428
	private void Start()
	{
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0002D22C File Offset: 0x0002B42C
	private void Update()
	{
		if (!this.material)
		{
			this.material = base.GetComponent<Image>().material;
		}
		if (!this.material)
		{
			return;
		}
		Rect rect = this.rect.rect;
		Vector3 vector = rect.min;
		Vector3 vector2 = rect.max;
		Vector3 vector3;
		vector3..ctor(vector.x, vector.y);
		Vector3 vector4;
		vector4..ctor(vector.x, vector2.y);
		Vector3 vector5;
		vector5..ctor(vector2.x, vector.y);
		Vector3 vector6 = base.transform.TransformPoint(vector3);
		Vector3 vector7 = base.transform.TransformVector(vector4 - vector3);
		Vector3 vector8 = base.transform.TransformVector(vector5 - vector3);
		this.material.SetVector(this.zeroPointID, vector6);
		this.material.SetVector(this.upVectorID, vector7);
		this.material.SetVector(this.rightVectorID, vector8);
		this.material.SetFloat(this.scaleID, this.rect.lossyScale.x);
	}

	// Token: 0x04000932 RID: 2354
	public RectTransform rect;

	// Token: 0x04000933 RID: 2355
	private Material material;

	// Token: 0x04000934 RID: 2356
	private int zeroPointID = Shader.PropertyToID("_ZeroPoint");

	// Token: 0x04000935 RID: 2357
	private int upVectorID = Shader.PropertyToID("_UpVector");

	// Token: 0x04000936 RID: 2358
	private int rightVectorID = Shader.PropertyToID("_RightVector");

	// Token: 0x04000937 RID: 2359
	private int scaleID = Shader.PropertyToID("_Scale");
}

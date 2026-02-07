using System;
using UnityEngine;

// Token: 0x02000004 RID: 4
public class RotateOnScroll : MonoBehaviour
{
	// Token: 0x06000012 RID: 18 RVA: 0x000027B0 File Offset: 0x000009B0
	private void Update()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			float angle = axis * this.rotationSpeed * Time.deltaTime * 10f;
			base.transform.Rotate(Vector3.up, angle);
		}
	}

	// Token: 0x0400000D RID: 13
	public float rotationSpeed = 2000f;
}

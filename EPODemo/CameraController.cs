using System;
using UnityEngine;

namespace EPOOutline.Demo
{
	// Token: 0x02000004 RID: 4
	public class CameraController : MonoBehaviour
	{
		// Token: 0x0600000D RID: 13 RVA: 0x00002210 File Offset: 0x00000410
		private void Update()
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.target.position + this.shift, Time.deltaTime * this.moveSpeed);
		}

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private Vector3 shift;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private float moveSpeed = 2f;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private Transform target;
	}
}

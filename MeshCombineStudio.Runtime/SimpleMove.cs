using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000041 RID: 65
	public class SimpleMove : MonoBehaviour
	{
		// Token: 0x06000175 RID: 373 RVA: 0x0000D804 File Offset: 0x0000BA04
		private void Start()
		{
			this.dir = UnityEngine.Random.onUnitSphere;
			this.t = UnityEngine.Random.value * this.moveMulti;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000D824 File Offset: 0x0000BA24
		private void Update()
		{
			float d = Mathf.Sin(Time.time + this.t) * this.moveMulti;
			if (this.moveMulti != 0f)
			{
				base.transform.Translate(this.dir * d * Time.deltaTime, Space.World);
			}
			base.transform.Rotate(Vector3.Scale(this.dir, this.rotDirMulti) * Time.deltaTime * this.rotMulti, Space.Self);
		}

		// Token: 0x040001A6 RID: 422
		public Vector3 rotDirMulti = Vector3.one;

		// Token: 0x040001A7 RID: 423
		public float moveMulti = 50f;

		// Token: 0x040001A8 RID: 424
		public float rotMulti = 50f;

		// Token: 0x040001A9 RID: 425
		private Vector3 dir;

		// Token: 0x040001AA RID: 426
		private float t;
	}
}

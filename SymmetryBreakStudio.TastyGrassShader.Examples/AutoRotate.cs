using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader.Example
{
	// Token: 0x02000004 RID: 4
	public class AutoRotate : MonoBehaviour
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021AC File Offset: 0x000003AC
		private void Update()
		{
			Camera camera;
			if (base.TryGetComponent<Camera>(out camera))
			{
				camera.fieldOfView += this.fovDelta * Time.deltaTime;
			}
			base.transform.Translate(this.pan * (this.panSpeed * Time.deltaTime));
			base.transform.Rotate(this.rotationAxis * (this.speed * Time.deltaTime));
		}

		// Token: 0x04000004 RID: 4
		public Vector3 rotationAxis;

		// Token: 0x04000005 RID: 5
		public float speed;

		// Token: 0x04000006 RID: 6
		public Vector3 pan;

		// Token: 0x04000007 RID: 7
		public float panSpeed;

		// Token: 0x04000008 RID: 8
		public float fovDelta;
	}
}

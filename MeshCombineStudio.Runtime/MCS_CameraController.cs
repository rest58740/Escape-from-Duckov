using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x0200003B RID: 59
	public class MCS_CameraController : MonoBehaviour
	{
		// Token: 0x06000142 RID: 322 RVA: 0x0000BE76 File Offset: 0x0000A076
		private void Awake()
		{
			this.t = base.transform;
			this.CreateParents();
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000BE8C File Offset: 0x0000A08C
		private void CreateParents()
		{
			this.cameraMountGO = new GameObject("CameraMount");
			this.cameraChildGO = new GameObject("CameraChild");
			this.cameraMountT = this.cameraMountGO.transform;
			this.cameraChildT = this.cameraChildGO.transform;
			this.cameraChildT.SetParent(this.cameraMountT);
			this.cameraMountT.position = this.t.position;
			this.cameraMountT.rotation = this.t.rotation;
			this.t.SetParent(this.cameraChildT);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000BF2C File Offset: 0x0000A12C
		private void Update()
		{
			Vector3 vector = (Input.mousePosition - this.oldMousePosition) * this.mouseMoveSpeed * (Time.deltaTime * 60f);
			if (Input.GetMouseButton(1))
			{
				this.cameraMountT.Rotate(0f, vector.x, 0f, Space.Self);
				this.cameraChildT.Rotate(-vector.y, 0f, 0f, Space.Self);
			}
			this.oldMousePosition = Input.mousePosition;
			Vector3 vector2 = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				vector2.z = this.speed;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				vector2.z = -this.speed;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				vector2.x = -this.speed;
			}
			else if (Input.GetKey(KeyCode.D))
			{
				vector2.x = this.speed;
			}
			else if (Input.GetKey(KeyCode.Q))
			{
				vector2.y = -this.speed;
			}
			else if (Input.GetKey(KeyCode.E))
			{
				vector2.y = this.speed;
			}
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				vector2 *= this.shiftMulti;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				vector2 *= this.controlMulti;
			}
			vector2 *= Time.deltaTime * 60f;
			Quaternion identity = Quaternion.identity;
			identity.eulerAngles = new Vector3(this.cameraChildT.eulerAngles.x, this.cameraMountT.eulerAngles.y, 0f);
			vector2 = identity * vector2;
			this.cameraMountT.position += vector2;
		}

		// Token: 0x04000157 RID: 343
		public float speed = 10f;

		// Token: 0x04000158 RID: 344
		public float mouseMoveSpeed = 1f;

		// Token: 0x04000159 RID: 345
		public float shiftMulti = 3f;

		// Token: 0x0400015A RID: 346
		public float controlMulti = 0.5f;

		// Token: 0x0400015B RID: 347
		private Vector3 oldMousePosition;

		// Token: 0x0400015C RID: 348
		private GameObject cameraMountGO;

		// Token: 0x0400015D RID: 349
		private GameObject cameraChildGO;

		// Token: 0x0400015E RID: 350
		private Transform cameraMountT;

		// Token: 0x0400015F RID: 351
		private Transform cameraChildT;

		// Token: 0x04000160 RID: 352
		private Transform t;
	}
}

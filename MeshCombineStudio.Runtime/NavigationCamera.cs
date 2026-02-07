using System;
using UnityEngine;

namespace MeshCombineStudio
{
	// Token: 0x02000044 RID: 68
	public class NavigationCamera : MonoBehaviour
	{
		// Token: 0x0600017D RID: 381 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
		private void Awake()
		{
			this.tStamp = Time.realtimeSinceStartup;
			this.startPosition = (this.position = base.transform.position);
			this.startRotation = (this.rot = base.transform.rotation);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000DC30 File Offset: 0x0000BE30
		private void OnDestroy()
		{
			this.RestoreCam();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000DC38 File Offset: 0x0000BE38
		private void Update()
		{
			this.scrollWheel = Input.mouseScrollDelta.y * this.data.mouseScrollWheelMulti;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000DC58 File Offset: 0x0000BE58
		private void LateUpdate()
		{
			this.deltaTime = Time.realtimeSinceStartup - this.tStamp;
			this.tStamp = Time.realtimeSinceStartup;
			Vector2 zero = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			if (Input.GetMouseButtonDown(1))
			{
				this.rot = base.transform.rotation;
				zero = Vector2.zero;
			}
			Vector3 vector = Vector3.zero;
			if (Input.GetMouseButton(1))
			{
				Quaternion rotation = base.transform.rotation;
				base.transform.Rotate(0f, zero.x * this.data.mouseSensitity * 1.66f, 0f, Space.World);
				base.transform.Rotate(-zero.y * this.data.mouseSensitity * 1.66f, 0f, 0f, Space.Self);
				this.rot = base.transform.rotation;
				base.transform.rotation = rotation;
				if (Input.GetKey(KeyCode.W))
				{
					vector.z = 1f;
				}
				else if (Input.GetKey(KeyCode.S))
				{
					vector.z = -1f;
				}
				if (Input.GetKey(KeyCode.D))
				{
					vector.x = 1f;
				}
				else if (Input.GetKey(KeyCode.A))
				{
					vector.x = -1f;
				}
				if (Input.GetKey(KeyCode.E))
				{
					vector.y = 1f;
				}
				else if (Input.GetKey(KeyCode.Q))
				{
					vector.y = -1f;
				}
				vector *= this.GetSpeedMulti();
			}
			if (Input.GetMouseButton(2))
			{
				vector.x = -zero.x;
				vector.y = -zero.y;
				vector *= this.GetSpeedMulti();
				this.currentSpeed = vector;
			}
			else
			{
				this.Lerp2Way(ref this.currentSpeed, vector, this.data.speedUpLerpMulti, this.data.speedDownLerpMulti);
			}
			this.position += base.transform.TransformDirection(this.currentSpeed * this.deltaTime) + base.transform.forward * this.scrollWheel * this.deltaTime;
			base.transform.rotation = this.rot;
			base.transform.position = this.position;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000DEB6 File Offset: 0x0000C0B6
		public void SetCam()
		{
			base.transform.rotation = this.rot;
			base.transform.position = this.position;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000DEDA File Offset: 0x0000C0DA
		public void RestoreCam()
		{
			base.transform.position = this.startPosition;
			base.transform.rotation = this.startRotation;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000DEFE File Offset: 0x0000C0FE
		private float GetSpeedMulti()
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{
				return this.data.speedFast;
			}
			if (Input.GetKey(KeyCode.LeftControl))
			{
				return this.data.speedSlow;
			}
			return this.data.speedNormal;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000DF3C File Offset: 0x0000C13C
		private void Lerp2Way(ref Vector3 v, Vector3 targetV, float upMulti, float downMulti)
		{
			this.Lerp2Way(ref v.x, targetV.x, upMulti, downMulti);
			this.Lerp2Way(ref v.y, targetV.y, upMulti, downMulti);
			this.Lerp2Way(ref v.z, targetV.z, upMulti, downMulti);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000DF88 File Offset: 0x0000C188
		private void Lerp2Way(ref float v, float targetV, float upMulti, float downMulti)
		{
			float num;
			if (Mathf.Abs(v) < Mathf.Abs(targetV))
			{
				num = upMulti;
			}
			else
			{
				num = downMulti;
			}
			v = Mathf.Lerp(v, targetV, num * this.deltaTime);
		}

		// Token: 0x040001B1 RID: 433
		public static float fov;

		// Token: 0x040001B2 RID: 434
		public SO_NavigationCamera data;

		// Token: 0x040001B3 RID: 435
		private Quaternion rot;

		// Token: 0x040001B4 RID: 436
		private Vector3 currentSpeed;

		// Token: 0x040001B5 RID: 437
		private float tStamp;

		// Token: 0x040001B6 RID: 438
		private float deltaTime;

		// Token: 0x040001B7 RID: 439
		private Vector3 startPosition;

		// Token: 0x040001B8 RID: 440
		private Vector3 position;

		// Token: 0x040001B9 RID: 441
		private Quaternion startRotation;

		// Token: 0x040001BA RID: 442
		private float scrollWheel;
	}
}

using System;
using UnityEngine;

namespace VLB_Samples
{
	// Token: 0x0200004C RID: 76
	public class FreeCameraController : MonoBehaviour
	{
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000B8CC File Offset: 0x00009ACC
		// (set) Token: 0x060002DF RID: 735 RVA: 0x0000B8D4 File Offset: 0x00009AD4
		private bool useMouseView
		{
			get
			{
				return this.m_UseMouseView;
			}
			set
			{
				this.m_UseMouseView = value;
				Cursor.lockState = (value ? CursorLockMode.Locked : CursorLockMode.None);
				Cursor.visible = !value;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B8F4 File Offset: 0x00009AF4
		private void Start()
		{
			this.useMouseView = true;
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			this.rotationH = eulerAngles.y;
			this.rotationV = eulerAngles.x;
			if (this.rotationV > 180f)
			{
				this.rotationV -= 360f;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B954 File Offset: 0x00009B54
		private void Update()
		{
			if (this.useMouseView)
			{
				this.rotationH += Input.GetAxis("Mouse X") * this.cameraSensitivity * Time.deltaTime;
				this.rotationV -= Input.GetAxis("Mouse Y") * this.cameraSensitivity * Time.deltaTime;
			}
			this.rotationV = Mathf.Clamp(this.rotationV, -90f, 90f);
			base.transform.rotation = Quaternion.AngleAxis(this.rotationH, Vector3.up);
			base.transform.rotation *= Quaternion.AngleAxis(this.rotationV, Vector3.right);
			float num = this.speedNormal;
			if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			{
				num *= this.speedFactorFast;
			}
			else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				num *= this.speedFactorSlow;
			}
			base.transform.position += num * Input.GetAxis("Vertical") * Time.deltaTime * base.transform.forward;
			base.transform.position += num * Input.GetAxis("Horizontal") * Time.deltaTime * base.transform.right;
			if (Input.GetKey(KeyCode.Q))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.up;
			}
			if (Input.GetKey(KeyCode.E))
			{
				base.transform.position += this.speedClimb * Time.deltaTime * Vector3.down;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
			{
				this.useMouseView = !this.useMouseView;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this.useMouseView = false;
			}
		}

		// Token: 0x040001BB RID: 443
		public float cameraSensitivity = 90f;

		// Token: 0x040001BC RID: 444
		public float speedNormal = 10f;

		// Token: 0x040001BD RID: 445
		public float speedFactorSlow = 0.25f;

		// Token: 0x040001BE RID: 446
		public float speedFactorFast = 3f;

		// Token: 0x040001BF RID: 447
		public float speedClimb = 4f;

		// Token: 0x040001C0 RID: 448
		private float rotationH;

		// Token: 0x040001C1 RID: 449
		private float rotationV;

		// Token: 0x040001C2 RID: 450
		private bool m_UseMouseView = true;
	}
}

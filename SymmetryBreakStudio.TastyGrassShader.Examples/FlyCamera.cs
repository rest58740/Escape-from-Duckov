using System;
using UnityEngine;

namespace SymmetryBreakStudio.TastyGrassShader.Example
{
	// Token: 0x02000006 RID: 6
	public class FlyCamera : MonoBehaviour
	{
		// Token: 0x06000016 RID: 22 RVA: 0x000026C9 File Offset: 0x000008C9
		private void Start()
		{
			this._rotation = base.transform.localRotation;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026DC File Offset: 0x000008DC
		private void Update()
		{
			Vector3 vector = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
			{
				vector += Vector3.forward;
			}
			if (Input.GetKey(KeyCode.S))
			{
				vector += Vector3.back;
			}
			if (Input.GetKey(KeyCode.A))
			{
				vector -= Vector3.right;
			}
			if (Input.GetKey(KeyCode.D))
			{
				vector += Vector3.right;
			}
			if (Input.GetKey(KeyCode.Q))
			{
				vector -= Vector3.up;
			}
			if (Input.GetKey(KeyCode.E))
			{
				vector += Vector3.up;
			}
			vector = vector.normalized;
			this._movement = Vector3.SmoothDamp(this._movement, vector, ref this._movementVelocity, this.smoothing);
			base.transform.Translate(this._movement * (this.speed * Time.deltaTime), Space.Self);
			float x = 0f;
			float y = 0f;
			if (Input.GetMouseButton(1))
			{
				x = Input.GetAxis("Mouse X");
				y = Input.GetAxis("Mouse Y");
			}
			this._mouse = Vector2.SmoothDamp(this._mouse, new Vector2(x, y), ref this._mouseVelocity, this.mouseSmoothing);
			Vector3 vector2 = this._rotation.eulerAngles;
			vector2 += new Vector3(-this._mouse.y, this._mouse.x, 0f) * (this.mouseSpeed * Time.deltaTime);
			float x2 = vector2.x;
			if (x2 <= 90f && x2 >= 0f)
			{
				vector2.x = Mathf.Clamp(vector2.x, 0f, 90f);
			}
			if (vector2.x >= 270f)
			{
				vector2.x = Mathf.Clamp(vector2.x, 270f, 360f);
			}
			this._rotation.eulerAngles = vector2;
			Quaternion rotation = this._rotation;
			Vector3 eulerAngles = rotation.eulerAngles;
			eulerAngles.z = 0f;
			base.transform.localRotation = Quaternion.Euler(eulerAngles);
		}

		// Token: 0x04000016 RID: 22
		public float speed;

		// Token: 0x04000017 RID: 23
		public float smoothing;

		// Token: 0x04000018 RID: 24
		public float mouseSpeed;

		// Token: 0x04000019 RID: 25
		public float mouseSmoothing;

		// Token: 0x0400001A RID: 26
		private Vector2 _mouse;

		// Token: 0x0400001B RID: 27
		private Vector2 _mouseVelocity;

		// Token: 0x0400001C RID: 28
		private Vector3 _movement;

		// Token: 0x0400001D RID: 29
		private Vector3 _movementVelocity;

		// Token: 0x0400001E RID: 30
		private Quaternion _rotation;
	}
}

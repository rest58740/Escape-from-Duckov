using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex71
{
	// Token: 0x0200006A RID: 106
	[RequireComponent(typeof(Rigidbody))]
	public class KinematicRotate : MonoBehaviour
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0000F2E3 File Offset: 0x0000D4E3
		// (set) Token: 0x06000371 RID: 881 RVA: 0x0000F2EB File Offset: 0x0000D4EB
		public float rotationSpeed
		{
			get
			{
				return this._rotationSpeed;
			}
			set
			{
				this._rotationSpeed = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000372 RID: 882 RVA: 0x0000F2F4 File Offset: 0x0000D4F4
		// (set) Token: 0x06000373 RID: 883 RVA: 0x0000F2FC File Offset: 0x0000D4FC
		public float angle
		{
			get
			{
				return this._angle;
			}
			set
			{
				this._angle = MathLib.ClampAngle(value, 0f, 360f);
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000F314 File Offset: 0x0000D514
		public void OnValidate()
		{
			this.rotationSpeed = this._rotationSpeed;
			this.rotationAxis = this.rotationAxis.normalized;
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000F333 File Offset: 0x0000D533
		public void Awake()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._rigidbody.isKinematic = true;
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0000F350 File Offset: 0x0000D550
		public void FixedUpdate()
		{
			this.angle += this.rotationSpeed * Time.deltaTime;
			Quaternion rhs = Quaternion.AngleAxis(this.rotationSpeed * Time.deltaTime, this.rotationAxis.normalized);
			this._rigidbody.MoveRotation(this._rigidbody.rotation * rhs);
		}

		// Token: 0x04000261 RID: 609
		[SerializeField]
		private float _rotationSpeed = 30f;

		// Token: 0x04000262 RID: 610
		public Vector3 rotationAxis = Vector3.up;

		// Token: 0x04000263 RID: 611
		private Rigidbody _rigidbody;

		// Token: 0x04000264 RID: 612
		private float _angle;
	}
}

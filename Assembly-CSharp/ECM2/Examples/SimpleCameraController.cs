using System;
using UnityEngine;

namespace ECM2.Examples
{
	// Token: 0x0200007C RID: 124
	public sealed class SimpleCameraController : MonoBehaviour
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x00010866 File Offset: 0x0000EA66
		// (set) Token: 0x060003D2 RID: 978 RVA: 0x0001086E File Offset: 0x0000EA6E
		public Transform target
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003D3 RID: 979 RVA: 0x00010877 File Offset: 0x0000EA77
		// (set) Token: 0x060003D4 RID: 980 RVA: 0x0001087F File Offset: 0x0000EA7F
		public float distanceToTarget
		{
			get
			{
				return this._distanceToTarget;
			}
			set
			{
				this._distanceToTarget = Mathf.Max(0f, value);
			}
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00010892 File Offset: 0x0000EA92
		public void OnValidate()
		{
			this.distanceToTarget = this._distanceToTarget;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x000108A0 File Offset: 0x0000EAA0
		public void Start()
		{
			if (this._target == null)
			{
				return;
			}
			base.transform.position = this.target.position - base.transform.forward * this.distanceToTarget;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000108F0 File Offset: 0x0000EAF0
		public void LateUpdate()
		{
			if (this._target == null)
			{
				return;
			}
			Vector3 target = this.target.position - base.transform.forward * this.distanceToTarget;
			base.transform.position = Vector3.SmoothDamp(base.transform.position, target, ref this._followVelocity, this._smoothTime);
		}

		// Token: 0x04000289 RID: 649
		[SerializeField]
		private Transform _target;

		// Token: 0x0400028A RID: 650
		[SerializeField]
		private float _distanceToTarget = 10f;

		// Token: 0x0400028B RID: 651
		[SerializeField]
		private float _smoothTime = 0.1f;

		// Token: 0x0400028C RID: 652
		private Vector3 _followVelocity;
	}
}

using System;
using UnityEngine;

namespace ECM2.Walkthrough.Ex71
{
	// Token: 0x02000069 RID: 105
	[RequireComponent(typeof(Rigidbody))]
	public class KinematicMove : MonoBehaviour
	{
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000F1DF File Offset: 0x0000D3DF
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000F1E7 File Offset: 0x0000D3E7
		public float moveTime
		{
			get
			{
				return this._moveTime;
			}
			set
			{
				this._moveTime = Mathf.Max(0.0001f, value);
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0000F1FA File Offset: 0x0000D3FA
		// (set) Token: 0x0600036A RID: 874 RVA: 0x0000F202 File Offset: 0x0000D402
		public Vector3 offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000F20B File Offset: 0x0000D40B
		public static float EaseInOut(float time, float duration)
		{
			return -0.5f * (Mathf.Cos(3.1415927f * time / duration) - 1f);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000F227 File Offset: 0x0000D427
		public void OnValidate()
		{
			this.moveTime = this._moveTime;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000F238 File Offset: 0x0000D438
		public void Awake()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._rigidbody.isKinematic = true;
			this._startPosition = base.transform.position;
			this._targetPosition = this._startPosition + this.offset;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000F288 File Offset: 0x0000D488
		public void FixedUpdate()
		{
			float t = KinematicMove.EaseInOut(Mathf.PingPong(Time.time, this._moveTime), this._moveTime);
			Vector3 position = Vector3.Lerp(this._startPosition, this._targetPosition, t);
			this._rigidbody.MovePosition(position);
		}

		// Token: 0x0400025C RID: 604
		[SerializeField]
		public float _moveTime = 3f;

		// Token: 0x0400025D RID: 605
		[SerializeField]
		private Vector3 _offset;

		// Token: 0x0400025E RID: 606
		private Rigidbody _rigidbody;

		// Token: 0x0400025F RID: 607
		private Vector3 _startPosition;

		// Token: 0x04000260 RID: 608
		private Vector3 _targetPosition;
	}
}

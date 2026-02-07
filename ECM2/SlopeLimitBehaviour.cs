using System;
using UnityEngine;

namespace ECM2
{
	// Token: 0x0200000F RID: 15
	public sealed class SlopeLimitBehaviour : MonoBehaviour
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000A0B5 File Offset: 0x000082B5
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000A0BD File Offset: 0x000082BD
		public SlopeBehaviour walkableSlopeBehaviour
		{
			get
			{
				return this._slopeBehaviour;
			}
			set
			{
				this._slopeBehaviour = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000A0C6 File Offset: 0x000082C6
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000A0CE File Offset: 0x000082CE
		public float slopeLimit
		{
			get
			{
				return this._slopeLimit;
			}
			set
			{
				this._slopeLimit = Mathf.Clamp(value, 0f, 89f);
				this._slopeLimitCos = Mathf.Cos(this._slopeLimit * 0.017453292f);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A0FD File Offset: 0x000082FD
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000A105 File Offset: 0x00008305
		public float slopeLimitCos
		{
			get
			{
				return this._slopeLimitCos;
			}
			set
			{
				this._slopeLimitCos = Mathf.Clamp01(value);
				this._slopeLimit = Mathf.Clamp(Mathf.Acos(this._slopeLimitCos) * 57.29578f, 0f, 89f);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000A139 File Offset: 0x00008339
		private void OnValidate()
		{
			this.slopeLimit = this._slopeLimit;
		}

		// Token: 0x040000DA RID: 218
		[Tooltip("The desired behaviour.")]
		[SerializeField]
		private SlopeBehaviour _slopeBehaviour;

		// Token: 0x040000DB RID: 219
		[SerializeField]
		private float _slopeLimit;

		// Token: 0x040000DC RID: 220
		[SerializeField]
		[HideInInspector]
		private float _slopeLimitCos;
	}
}

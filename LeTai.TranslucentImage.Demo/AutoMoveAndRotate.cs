using System;
using UnityEngine;

namespace LeTai.Asset.TranslucentImage.Demo
{
	// Token: 0x02000003 RID: 3
	public class AutoMoveAndRotate : MonoBehaviour
	{
		// Token: 0x06000003 RID: 3 RVA: 0x000020C0 File Offset: 0x000002C0
		private void Start()
		{
			this.m_LastRealTime = Time.realtimeSinceStartup;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020CD File Offset: 0x000002CD
		private void Update()
		{
			if (!this.lateUpdate)
			{
				this.DoWork();
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020DD File Offset: 0x000002DD
		private void LateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoWork();
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020F0 File Offset: 0x000002F0
		private void DoWork()
		{
			float d = Time.deltaTime;
			if (this.ignoreTimescale)
			{
				d = Time.realtimeSinceStartup - this.m_LastRealTime;
				this.m_LastRealTime = Time.realtimeSinceStartup;
			}
			base.transform.Translate(this.moveUnitsPerSecond.value * d, this.moveUnitsPerSecond.space);
			base.transform.Rotate(this.rotateDegreesPerSecond.value * d, this.moveUnitsPerSecond.space);
		}

		// Token: 0x04000001 RID: 1
		public AutoMoveAndRotate.Vector3andSpace moveUnitsPerSecond;

		// Token: 0x04000002 RID: 2
		public AutoMoveAndRotate.Vector3andSpace rotateDegreesPerSecond;

		// Token: 0x04000003 RID: 3
		public bool ignoreTimescale;

		// Token: 0x04000004 RID: 4
		public bool lateUpdate;

		// Token: 0x04000005 RID: 5
		private float m_LastRealTime;

		// Token: 0x02000012 RID: 18
		[Serializable]
		public class Vector3andSpace
		{
			// Token: 0x04000034 RID: 52
			public Vector3 value;

			// Token: 0x04000035 RID: 53
			public Space space = Space.Self;
		}
	}
}

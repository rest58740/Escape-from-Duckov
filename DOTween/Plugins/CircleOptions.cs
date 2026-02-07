using System;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening.Plugins
{
	// Token: 0x0200001E RID: 30
	public struct CircleOptions : IPlugOptions
	{
		// Token: 0x0600019E RID: 414 RVA: 0x00008D24 File Offset: 0x00006F24
		public void Reset()
		{
			this.initialized = false;
			this.startValueDegrees = (this.endValueDegrees = 0f);
			this.relativeCenter = false;
			this.snapping = false;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008D5C File Offset: 0x00006F5C
		public void Initialize(Vector2 startValue, Vector2 endValue)
		{
			this.initialized = true;
			this.center = endValue;
			if (this.relativeCenter)
			{
				this.center = startValue + this.center;
			}
			this.radius = Vector2.Distance(this.center, startValue);
			Vector2 vector = startValue - this.center;
			this.startValueDegrees = Mathf.Atan2(vector.x, vector.y) * 57.29578f;
		}

		// Token: 0x040000D6 RID: 214
		public float endValueDegrees;

		// Token: 0x040000D7 RID: 215
		public bool relativeCenter;

		// Token: 0x040000D8 RID: 216
		public bool snapping;

		// Token: 0x040000D9 RID: 217
		internal Vector2 center;

		// Token: 0x040000DA RID: 218
		internal float radius;

		// Token: 0x040000DB RID: 219
		internal float startValueDegrees;

		// Token: 0x040000DC RID: 220
		internal bool initialized;
	}
}

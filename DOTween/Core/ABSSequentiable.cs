using System;

namespace DG.Tweening.Core
{
	// Token: 0x0200004B RID: 75
	public abstract class ABSSequentiable
	{
		// Token: 0x04000143 RID: 323
		internal TweenType tweenType;

		// Token: 0x04000144 RID: 324
		internal float sequencedPosition;

		// Token: 0x04000145 RID: 325
		internal float sequencedEndPosition;

		// Token: 0x04000146 RID: 326
		internal TweenCallback onStart;
	}
}

using System;

namespace DG.Tweening.Core
{
	// Token: 0x02000054 RID: 84
	internal class SequenceCallback : ABSSequentiable
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x0000FDFF File Offset: 0x0000DFFF
		public SequenceCallback(float sequencedPosition, TweenCallback callback)
		{
			this.tweenType = TweenType.Callback;
			this.sequencedPosition = sequencedPosition;
			this.onStart = callback;
		}
	}
}

using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public struct EaseMode
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003135 File Offset: 0x00001335
		public EaseMode(EEaseFunc func)
		{
			this.easeFunc = func;
			this.curve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
		}

		// Token: 0x0400003F RID: 63
		public EEaseFunc easeFunc;

		// Token: 0x04000040 RID: 64
		public AnimationCurve curve;
	}
}

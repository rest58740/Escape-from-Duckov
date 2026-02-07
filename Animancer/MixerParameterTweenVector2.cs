using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000045 RID: 69
	public class MixerParameterTweenVector2 : MixerParameterTween<Vector2>
	{
		// Token: 0x06000454 RID: 1108 RVA: 0x0000C8B1 File Offset: 0x0000AAB1
		public MixerParameterTweenVector2()
		{
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000C8B9 File Offset: 0x0000AAB9
		public MixerParameterTweenVector2(MixerState<Vector2> mixer) : base(mixer)
		{
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000C8C2 File Offset: 0x0000AAC2
		protected override Vector2 CalculateCurrentValue()
		{
			return Vector2.LerpUnclamped(base.StartValue, base.EndValue, base.Progress);
		}
	}
}

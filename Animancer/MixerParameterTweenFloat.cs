using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x02000044 RID: 68
	public class MixerParameterTweenFloat : MixerParameterTween<float>
	{
		// Token: 0x06000451 RID: 1105 RVA: 0x0000C887 File Offset: 0x0000AA87
		public MixerParameterTweenFloat()
		{
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000C88F File Offset: 0x0000AA8F
		public MixerParameterTweenFloat(MixerState<float> mixer) : base(mixer)
		{
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000C898 File Offset: 0x0000AA98
		protected override float CalculateCurrentValue()
		{
			return Mathf.LerpUnclamped(base.StartValue, base.EndValue, base.Progress);
		}
	}
}

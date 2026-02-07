using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x02000010 RID: 16
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-pulse/")]
	[AddComponentMenu("VLB/Common/Effect Pulse")]
	public class EffectPulse : EffectAbstractBase
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00003994 File Offset: 0x00001B94
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectPulse effectPulse = source as EffectPulse;
			if (effectPulse)
			{
				this.frequency = effectPulse.frequency;
				this.intensityAmplitude = effectPulse.intensityAmplitude;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x000039CF File Offset: 0x00001BCF
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000039E4 File Offset: 0x00001BE4
		private IEnumerator CoUpdate()
		{
			float t = 0f;
			for (;;)
			{
				float num = Mathf.Sin(this.frequency * t);
				float lerpedValue = this.intensityAmplitude.GetLerpedValue(num * 0.5f + 0.5f);
				base.SetAdditiveIntensity(lerpedValue);
				yield return null;
				t += Time.deltaTime;
			}
			yield break;
		}

		// Token: 0x04000055 RID: 85
		public new const string ClassName = "EffectPulse";

		// Token: 0x04000056 RID: 86
		[Range(0.1f, 60f)]
		public float frequency = 10f;

		// Token: 0x04000057 RID: 87
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;
	}
}

using System;
using System.Collections;
using UnityEngine;

namespace VLB
{
	// Token: 0x0200000E RID: 14
	[HelpURL("http://saladgamer.com/vlb-doc/comp-effect-flicker/")]
	[AddComponentMenu("VLB/Common/Effect Flicker")]
	public class EffectFlicker : EffectAbstractBase
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000037BC File Offset: 0x000019BC
		public override void InitFrom(EffectAbstractBase source)
		{
			base.InitFrom(source);
			EffectFlicker effectFlicker = source as EffectFlicker;
			if (effectFlicker)
			{
				this.frequency = effectFlicker.frequency;
				this.performPauses = effectFlicker.performPauses;
				this.flickeringDuration = effectFlicker.flickeringDuration;
				this.pauseDuration = effectFlicker.pauseDuration;
				this.restoreIntensityOnPause = effectFlicker.restoreIntensityOnPause;
				this.intensityAmplitude = effectFlicker.intensityAmplitude;
				this.smoothing = effectFlicker.smoothing;
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003833 File Offset: 0x00001A33
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this.CoUpdate());
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003848 File Offset: 0x00001A48
		private IEnumerator CoUpdate()
		{
			for (;;)
			{
				yield return this.CoFlicker();
				if (this.performPauses)
				{
					yield return this.CoChangeIntensity(this.pauseDuration.randomValue, this.restoreIntensityOnPause ? 0f : this.m_CurrentAdditiveIntensity);
				}
			}
			yield break;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003857 File Offset: 0x00001A57
		private IEnumerator CoFlicker()
		{
			float remainingDuration = this.flickeringDuration.randomValue;
			float deltaTime = Time.deltaTime;
			while (!this.performPauses || remainingDuration > 0f)
			{
				float freqDuration = 1f / this.frequency;
				yield return this.CoChangeIntensity(freqDuration, this.intensityAmplitude.randomValue);
				remainingDuration -= freqDuration;
			}
			yield break;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003866 File Offset: 0x00001A66
		private IEnumerator CoChangeIntensity(float expectedDuration, float nextIntensity)
		{
			float velocity = 0f;
			float t = 0f;
			while (t < expectedDuration)
			{
				this.m_CurrentAdditiveIntensity = Mathf.SmoothDamp(this.m_CurrentAdditiveIntensity, nextIntensity, ref velocity, this.smoothing);
				base.SetAdditiveIntensity(this.m_CurrentAdditiveIntensity);
				t += Time.deltaTime;
				yield return null;
			}
			yield break;
		}

		// Token: 0x04000049 RID: 73
		public new const string ClassName = "EffectFlicker";

		// Token: 0x0400004A RID: 74
		[Range(1f, 60f)]
		public float frequency = 10f;

		// Token: 0x0400004B RID: 75
		public bool performPauses;

		// Token: 0x0400004C RID: 76
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat flickeringDuration = Consts.Effects.FlickeringDurationDefault;

		// Token: 0x0400004D RID: 77
		[MinMaxRange(0f, 10f)]
		public MinMaxRangeFloat pauseDuration = Consts.Effects.PauseDurationDefault;

		// Token: 0x0400004E RID: 78
		public bool restoreIntensityOnPause;

		// Token: 0x0400004F RID: 79
		[MinMaxRange(-5f, 5f)]
		public MinMaxRangeFloat intensityAmplitude = Consts.Effects.IntensityAmplitudeDefault;

		// Token: 0x04000050 RID: 80
		[Range(0f, 0.25f)]
		public float smoothing = 0.05f;

		// Token: 0x04000051 RID: 81
		private float m_CurrentAdditiveIntensity;
	}
}

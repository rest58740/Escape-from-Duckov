using System;

namespace FMOD
{
	// Token: 0x0200003E RID: 62
	public struct REVERB_PROPERTIES
	{
		// Token: 0x06000069 RID: 105 RVA: 0x000024B0 File Offset: 0x000006B0
		public REVERB_PROPERTIES(float decayTime, float earlyDelay, float lateDelay, float hfReference, float hfDecayRatio, float diffusion, float density, float lowShelfFrequency, float lowShelfGain, float highCut, float earlyLateMix, float wetLevel)
		{
			this.DecayTime = decayTime;
			this.EarlyDelay = earlyDelay;
			this.LateDelay = lateDelay;
			this.HFReference = hfReference;
			this.HFDecayRatio = hfDecayRatio;
			this.Diffusion = diffusion;
			this.Density = density;
			this.LowShelfFrequency = lowShelfFrequency;
			this.LowShelfGain = lowShelfGain;
			this.HighCut = highCut;
			this.EarlyLateMix = earlyLateMix;
			this.WetLevel = wetLevel;
		}

		// Token: 0x040001E1 RID: 481
		public float DecayTime;

		// Token: 0x040001E2 RID: 482
		public float EarlyDelay;

		// Token: 0x040001E3 RID: 483
		public float LateDelay;

		// Token: 0x040001E4 RID: 484
		public float HFReference;

		// Token: 0x040001E5 RID: 485
		public float HFDecayRatio;

		// Token: 0x040001E6 RID: 486
		public float Diffusion;

		// Token: 0x040001E7 RID: 487
		public float Density;

		// Token: 0x040001E8 RID: 488
		public float LowShelfFrequency;

		// Token: 0x040001E9 RID: 489
		public float LowShelfGain;

		// Token: 0x040001EA RID: 490
		public float HighCut;

		// Token: 0x040001EB RID: 491
		public float EarlyLateMix;

		// Token: 0x040001EC RID: 492
		public float WetLevel;
	}
}

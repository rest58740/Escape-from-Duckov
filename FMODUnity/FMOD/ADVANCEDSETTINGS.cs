using System;

namespace FMOD
{
	// Token: 0x02000040 RID: 64
	public struct ADVANCEDSETTINGS
	{
		// Token: 0x040001ED RID: 493
		public int cbSize;

		// Token: 0x040001EE RID: 494
		public int maxMPEGCodecs;

		// Token: 0x040001EF RID: 495
		public int maxADPCMCodecs;

		// Token: 0x040001F0 RID: 496
		public int maxXMACodecs;

		// Token: 0x040001F1 RID: 497
		public int maxVorbisCodecs;

		// Token: 0x040001F2 RID: 498
		public int maxAT9Codecs;

		// Token: 0x040001F3 RID: 499
		public int maxFADPCMCodecs;

		// Token: 0x040001F4 RID: 500
		public int maxOpusCodecs;

		// Token: 0x040001F5 RID: 501
		public int ASIONumChannels;

		// Token: 0x040001F6 RID: 502
		public IntPtr ASIOChannelList;

		// Token: 0x040001F7 RID: 503
		public IntPtr ASIOSpeakerList;

		// Token: 0x040001F8 RID: 504
		public float vol0virtualvol;

		// Token: 0x040001F9 RID: 505
		public uint defaultDecodeBufferSize;

		// Token: 0x040001FA RID: 506
		public ushort profilePort;

		// Token: 0x040001FB RID: 507
		public uint geometryMaxFadeTime;

		// Token: 0x040001FC RID: 508
		public float distanceFilterCenterFreq;

		// Token: 0x040001FD RID: 509
		public int reverb3Dinstance;

		// Token: 0x040001FE RID: 510
		public int DSPBufferPoolSize;

		// Token: 0x040001FF RID: 511
		public DSP_RESAMPLER resamplerMethod;

		// Token: 0x04000200 RID: 512
		public uint randomSeed;

		// Token: 0x04000201 RID: 513
		public int maxConvolutionThreads;

		// Token: 0x04000202 RID: 514
		public int maxSpatialObjects;
	}
}

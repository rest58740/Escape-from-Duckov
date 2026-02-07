using System;

namespace FMOD
{
	// Token: 0x0200004C RID: 76
	internal interface IChannelControl
	{
		// Token: 0x060001AE RID: 430
		RESULT getSystemObject(out System system);

		// Token: 0x060001AF RID: 431
		RESULT stop();

		// Token: 0x060001B0 RID: 432
		RESULT setPaused(bool paused);

		// Token: 0x060001B1 RID: 433
		RESULT getPaused(out bool paused);

		// Token: 0x060001B2 RID: 434
		RESULT setVolume(float volume);

		// Token: 0x060001B3 RID: 435
		RESULT getVolume(out float volume);

		// Token: 0x060001B4 RID: 436
		RESULT setVolumeRamp(bool ramp);

		// Token: 0x060001B5 RID: 437
		RESULT getVolumeRamp(out bool ramp);

		// Token: 0x060001B6 RID: 438
		RESULT getAudibility(out float audibility);

		// Token: 0x060001B7 RID: 439
		RESULT setPitch(float pitch);

		// Token: 0x060001B8 RID: 440
		RESULT getPitch(out float pitch);

		// Token: 0x060001B9 RID: 441
		RESULT setMute(bool mute);

		// Token: 0x060001BA RID: 442
		RESULT getMute(out bool mute);

		// Token: 0x060001BB RID: 443
		RESULT setReverbProperties(int instance, float wet);

		// Token: 0x060001BC RID: 444
		RESULT getReverbProperties(int instance, out float wet);

		// Token: 0x060001BD RID: 445
		RESULT setLowPassGain(float gain);

		// Token: 0x060001BE RID: 446
		RESULT getLowPassGain(out float gain);

		// Token: 0x060001BF RID: 447
		RESULT setMode(MODE mode);

		// Token: 0x060001C0 RID: 448
		RESULT getMode(out MODE mode);

		// Token: 0x060001C1 RID: 449
		RESULT setCallback(CHANNELCONTROL_CALLBACK callback);

		// Token: 0x060001C2 RID: 450
		RESULT isPlaying(out bool isplaying);

		// Token: 0x060001C3 RID: 451
		RESULT setPan(float pan);

		// Token: 0x060001C4 RID: 452
		RESULT setMixLevelsOutput(float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);

		// Token: 0x060001C5 RID: 453
		RESULT setMixLevelsInput(float[] levels, int numlevels);

		// Token: 0x060001C6 RID: 454
		RESULT setMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannel_hop);

		// Token: 0x060001C7 RID: 455
		RESULT getMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannel_hop);

		// Token: 0x060001C8 RID: 456
		RESULT getDSPClock(out ulong dspclock, out ulong parentclock);

		// Token: 0x060001C9 RID: 457
		RESULT setDelay(ulong dspclock_start, ulong dspclock_end, bool stopchannels);

		// Token: 0x060001CA RID: 458
		RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end);

		// Token: 0x060001CB RID: 459
		RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end, out bool stopchannels);

		// Token: 0x060001CC RID: 460
		RESULT addFadePoint(ulong dspclock, float volume);

		// Token: 0x060001CD RID: 461
		RESULT setFadePointRamp(ulong dspclock, float volume);

		// Token: 0x060001CE RID: 462
		RESULT removeFadePoints(ulong dspclock_start, ulong dspclock_end);

		// Token: 0x060001CF RID: 463
		RESULT getFadePoints(ref uint numpoints, ulong[] point_dspclock, float[] point_volume);

		// Token: 0x060001D0 RID: 464
		RESULT getDSP(int index, out DSP dsp);

		// Token: 0x060001D1 RID: 465
		RESULT addDSP(int index, DSP dsp);

		// Token: 0x060001D2 RID: 466
		RESULT removeDSP(DSP dsp);

		// Token: 0x060001D3 RID: 467
		RESULT getNumDSPs(out int numdsps);

		// Token: 0x060001D4 RID: 468
		RESULT setDSPIndex(DSP dsp, int index);

		// Token: 0x060001D5 RID: 469
		RESULT getDSPIndex(DSP dsp, out int index);

		// Token: 0x060001D6 RID: 470
		RESULT set3DAttributes(ref VECTOR pos, ref VECTOR vel);

		// Token: 0x060001D7 RID: 471
		RESULT get3DAttributes(out VECTOR pos, out VECTOR vel);

		// Token: 0x060001D8 RID: 472
		RESULT set3DMinMaxDistance(float mindistance, float maxdistance);

		// Token: 0x060001D9 RID: 473
		RESULT get3DMinMaxDistance(out float mindistance, out float maxdistance);

		// Token: 0x060001DA RID: 474
		RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume);

		// Token: 0x060001DB RID: 475
		RESULT get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume);

		// Token: 0x060001DC RID: 476
		RESULT set3DConeOrientation(ref VECTOR orientation);

		// Token: 0x060001DD RID: 477
		RESULT get3DConeOrientation(out VECTOR orientation);

		// Token: 0x060001DE RID: 478
		RESULT set3DCustomRolloff(ref VECTOR points, int numpoints);

		// Token: 0x060001DF RID: 479
		RESULT get3DCustomRolloff(out IntPtr points, out int numpoints);

		// Token: 0x060001E0 RID: 480
		RESULT set3DOcclusion(float directocclusion, float reverbocclusion);

		// Token: 0x060001E1 RID: 481
		RESULT get3DOcclusion(out float directocclusion, out float reverbocclusion);

		// Token: 0x060001E2 RID: 482
		RESULT set3DSpread(float angle);

		// Token: 0x060001E3 RID: 483
		RESULT get3DSpread(out float angle);

		// Token: 0x060001E4 RID: 484
		RESULT set3DLevel(float level);

		// Token: 0x060001E5 RID: 485
		RESULT get3DLevel(out float level);

		// Token: 0x060001E6 RID: 486
		RESULT set3DDopplerLevel(float level);

		// Token: 0x060001E7 RID: 487
		RESULT get3DDopplerLevel(out float level);

		// Token: 0x060001E8 RID: 488
		RESULT set3DDistanceFilter(bool custom, float customLevel, float centerFreq);

		// Token: 0x060001E9 RID: 489
		RESULT get3DDistanceFilter(out bool custom, out float customLevel, out float centerFreq);

		// Token: 0x060001EA RID: 490
		RESULT setUserData(IntPtr userdata);

		// Token: 0x060001EB RID: 491
		RESULT getUserData(out IntPtr userdata);
	}
}

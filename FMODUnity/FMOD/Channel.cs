using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200004D RID: 77
	public struct Channel : IChannelControl
	{
		// Token: 0x060001EC RID: 492 RVA: 0x00003B16 File Offset: 0x00001D16
		public RESULT setFrequency(float frequency)
		{
			return Channel.FMOD5_Channel_SetFrequency(this.handle, frequency);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00003B24 File Offset: 0x00001D24
		public RESULT getFrequency(out float frequency)
		{
			return Channel.FMOD5_Channel_GetFrequency(this.handle, out frequency);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00003B32 File Offset: 0x00001D32
		public RESULT setPriority(int priority)
		{
			return Channel.FMOD5_Channel_SetPriority(this.handle, priority);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00003B40 File Offset: 0x00001D40
		public RESULT getPriority(out int priority)
		{
			return Channel.FMOD5_Channel_GetPriority(this.handle, out priority);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00003B4E File Offset: 0x00001D4E
		public RESULT setPosition(uint position, TIMEUNIT postype)
		{
			return Channel.FMOD5_Channel_SetPosition(this.handle, position, postype);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00003B5D File Offset: 0x00001D5D
		public RESULT getPosition(out uint position, TIMEUNIT postype)
		{
			return Channel.FMOD5_Channel_GetPosition(this.handle, out position, postype);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00003B6C File Offset: 0x00001D6C
		public RESULT setChannelGroup(ChannelGroup channelgroup)
		{
			return Channel.FMOD5_Channel_SetChannelGroup(this.handle, channelgroup.handle);
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00003B7F File Offset: 0x00001D7F
		public RESULT getChannelGroup(out ChannelGroup channelgroup)
		{
			return Channel.FMOD5_Channel_GetChannelGroup(this.handle, out channelgroup.handle);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00003B92 File Offset: 0x00001D92
		public RESULT setLoopCount(int loopcount)
		{
			return Channel.FMOD5_Channel_SetLoopCount(this.handle, loopcount);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x00003BA0 File Offset: 0x00001DA0
		public RESULT getLoopCount(out int loopcount)
		{
			return Channel.FMOD5_Channel_GetLoopCount(this.handle, out loopcount);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00003BAE File Offset: 0x00001DAE
		public RESULT setLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
		{
			return Channel.FMOD5_Channel_SetLoopPoints(this.handle, loopstart, loopstarttype, loopend, loopendtype);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public RESULT getLoopPoints(out uint loopstart, TIMEUNIT loopstarttype, out uint loopend, TIMEUNIT loopendtype)
		{
			return Channel.FMOD5_Channel_GetLoopPoints(this.handle, out loopstart, loopstarttype, out loopend, loopendtype);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00003BD2 File Offset: 0x00001DD2
		public RESULT isVirtual(out bool isvirtual)
		{
			return Channel.FMOD5_Channel_IsVirtual(this.handle, out isvirtual);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00003BE0 File Offset: 0x00001DE0
		public RESULT getCurrentSound(out Sound sound)
		{
			return Channel.FMOD5_Channel_GetCurrentSound(this.handle, out sound.handle);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00003BF3 File Offset: 0x00001DF3
		public RESULT getIndex(out int index)
		{
			return Channel.FMOD5_Channel_GetIndex(this.handle, out index);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00003C01 File Offset: 0x00001E01
		public RESULT getSystemObject(out System system)
		{
			return Channel.FMOD5_Channel_GetSystemObject(this.handle, out system.handle);
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00003C14 File Offset: 0x00001E14
		public RESULT stop()
		{
			return Channel.FMOD5_Channel_Stop(this.handle);
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00003C21 File Offset: 0x00001E21
		public RESULT setPaused(bool paused)
		{
			return Channel.FMOD5_Channel_SetPaused(this.handle, paused);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00003C2F File Offset: 0x00001E2F
		public RESULT getPaused(out bool paused)
		{
			return Channel.FMOD5_Channel_GetPaused(this.handle, out paused);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00003C3D File Offset: 0x00001E3D
		public RESULT setVolume(float volume)
		{
			return Channel.FMOD5_Channel_SetVolume(this.handle, volume);
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00003C4B File Offset: 0x00001E4B
		public RESULT getVolume(out float volume)
		{
			return Channel.FMOD5_Channel_GetVolume(this.handle, out volume);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00003C59 File Offset: 0x00001E59
		public RESULT setVolumeRamp(bool ramp)
		{
			return Channel.FMOD5_Channel_SetVolumeRamp(this.handle, ramp);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00003C67 File Offset: 0x00001E67
		public RESULT getVolumeRamp(out bool ramp)
		{
			return Channel.FMOD5_Channel_GetVolumeRamp(this.handle, out ramp);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00003C75 File Offset: 0x00001E75
		public RESULT getAudibility(out float audibility)
		{
			return Channel.FMOD5_Channel_GetAudibility(this.handle, out audibility);
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00003C83 File Offset: 0x00001E83
		public RESULT setPitch(float pitch)
		{
			return Channel.FMOD5_Channel_SetPitch(this.handle, pitch);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00003C91 File Offset: 0x00001E91
		public RESULT getPitch(out float pitch)
		{
			return Channel.FMOD5_Channel_GetPitch(this.handle, out pitch);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00003C9F File Offset: 0x00001E9F
		public RESULT setMute(bool mute)
		{
			return Channel.FMOD5_Channel_SetMute(this.handle, mute);
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00003CAD File Offset: 0x00001EAD
		public RESULT getMute(out bool mute)
		{
			return Channel.FMOD5_Channel_GetMute(this.handle, out mute);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00003CBB File Offset: 0x00001EBB
		public RESULT setReverbProperties(int instance, float wet)
		{
			return Channel.FMOD5_Channel_SetReverbProperties(this.handle, instance, wet);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00003CCA File Offset: 0x00001ECA
		public RESULT getReverbProperties(int instance, out float wet)
		{
			return Channel.FMOD5_Channel_GetReverbProperties(this.handle, instance, out wet);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x00003CD9 File Offset: 0x00001ED9
		public RESULT setLowPassGain(float gain)
		{
			return Channel.FMOD5_Channel_SetLowPassGain(this.handle, gain);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00003CE7 File Offset: 0x00001EE7
		public RESULT getLowPassGain(out float gain)
		{
			return Channel.FMOD5_Channel_GetLowPassGain(this.handle, out gain);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00003CF5 File Offset: 0x00001EF5
		public RESULT setMode(MODE mode)
		{
			return Channel.FMOD5_Channel_SetMode(this.handle, mode);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00003D03 File Offset: 0x00001F03
		public RESULT getMode(out MODE mode)
		{
			return Channel.FMOD5_Channel_GetMode(this.handle, out mode);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00003D11 File Offset: 0x00001F11
		public RESULT setCallback(CHANNELCONTROL_CALLBACK callback)
		{
			return Channel.FMOD5_Channel_SetCallback(this.handle, callback);
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00003D1F File Offset: 0x00001F1F
		public RESULT isPlaying(out bool isplaying)
		{
			return Channel.FMOD5_Channel_IsPlaying(this.handle, out isplaying);
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00003D2D File Offset: 0x00001F2D
		public RESULT setPan(float pan)
		{
			return Channel.FMOD5_Channel_SetPan(this.handle, pan);
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00003D3C File Offset: 0x00001F3C
		public RESULT setMixLevelsOutput(float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright)
		{
			return Channel.FMOD5_Channel_SetMixLevelsOutput(this.handle, frontleft, frontright, center, lfe, surroundleft, surroundright, backleft, backright);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00003D61 File Offset: 0x00001F61
		public RESULT setMixLevelsInput(float[] levels, int numlevels)
		{
			return Channel.FMOD5_Channel_SetMixLevelsInput(this.handle, levels, numlevels);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00003D70 File Offset: 0x00001F70
		public RESULT setMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannel_hop = 0)
		{
			return Channel.FMOD5_Channel_SetMixMatrix(this.handle, matrix, outchannels, inchannels, inchannel_hop);
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00003D82 File Offset: 0x00001F82
		public RESULT getMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannel_hop = 0)
		{
			return Channel.FMOD5_Channel_GetMixMatrix(this.handle, matrix, out outchannels, out inchannels, inchannel_hop);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00003D94 File Offset: 0x00001F94
		public RESULT getDSPClock(out ulong dspclock, out ulong parentclock)
		{
			return Channel.FMOD5_Channel_GetDSPClock(this.handle, out dspclock, out parentclock);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00003DA3 File Offset: 0x00001FA3
		public RESULT setDelay(ulong dspclock_start, ulong dspclock_end, bool stopchannels = true)
		{
			return Channel.FMOD5_Channel_SetDelay(this.handle, dspclock_start, dspclock_end, stopchannels);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00003DB3 File Offset: 0x00001FB3
		public RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end)
		{
			return Channel.FMOD5_Channel_GetDelay(this.handle, out dspclock_start, out dspclock_end, IntPtr.Zero);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00003DC7 File Offset: 0x00001FC7
		public RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end, out bool stopchannels)
		{
			return Channel.FMOD5_Channel_GetDelay(this.handle, out dspclock_start, out dspclock_end, out stopchannels);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00003DD7 File Offset: 0x00001FD7
		public RESULT addFadePoint(ulong dspclock, float volume)
		{
			return Channel.FMOD5_Channel_AddFadePoint(this.handle, dspclock, volume);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00003DE6 File Offset: 0x00001FE6
		public RESULT setFadePointRamp(ulong dspclock, float volume)
		{
			return Channel.FMOD5_Channel_SetFadePointRamp(this.handle, dspclock, volume);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public RESULT removeFadePoints(ulong dspclock_start, ulong dspclock_end)
		{
			return Channel.FMOD5_Channel_RemoveFadePoints(this.handle, dspclock_start, dspclock_end);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00003E04 File Offset: 0x00002004
		public RESULT getFadePoints(ref uint numpoints, ulong[] point_dspclock, float[] point_volume)
		{
			return Channel.FMOD5_Channel_GetFadePoints(this.handle, ref numpoints, point_dspclock, point_volume);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00003E14 File Offset: 0x00002014
		public RESULT getDSP(int index, out DSP dsp)
		{
			return Channel.FMOD5_Channel_GetDSP(this.handle, index, out dsp.handle);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00003E28 File Offset: 0x00002028
		public RESULT addDSP(int index, DSP dsp)
		{
			return Channel.FMOD5_Channel_AddDSP(this.handle, index, dsp.handle);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00003E3C File Offset: 0x0000203C
		public RESULT removeDSP(DSP dsp)
		{
			return Channel.FMOD5_Channel_RemoveDSP(this.handle, dsp.handle);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00003E4F File Offset: 0x0000204F
		public RESULT getNumDSPs(out int numdsps)
		{
			return Channel.FMOD5_Channel_GetNumDSPs(this.handle, out numdsps);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00003E5D File Offset: 0x0000205D
		public RESULT setDSPIndex(DSP dsp, int index)
		{
			return Channel.FMOD5_Channel_SetDSPIndex(this.handle, dsp.handle, index);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00003E71 File Offset: 0x00002071
		public RESULT getDSPIndex(DSP dsp, out int index)
		{
			return Channel.FMOD5_Channel_GetDSPIndex(this.handle, dsp.handle, out index);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00003E85 File Offset: 0x00002085
		public RESULT set3DAttributes(ref VECTOR pos, ref VECTOR vel)
		{
			return Channel.FMOD5_Channel_Set3DAttributes(this.handle, ref pos, ref vel);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00003E94 File Offset: 0x00002094
		public RESULT get3DAttributes(out VECTOR pos, out VECTOR vel)
		{
			return Channel.FMOD5_Channel_Get3DAttributes(this.handle, out pos, out vel);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00003EA3 File Offset: 0x000020A3
		public RESULT set3DMinMaxDistance(float mindistance, float maxdistance)
		{
			return Channel.FMOD5_Channel_Set3DMinMaxDistance(this.handle, mindistance, maxdistance);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00003EB2 File Offset: 0x000020B2
		public RESULT get3DMinMaxDistance(out float mindistance, out float maxdistance)
		{
			return Channel.FMOD5_Channel_Get3DMinMaxDistance(this.handle, out mindistance, out maxdistance);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00003EC1 File Offset: 0x000020C1
		public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
		{
			return Channel.FMOD5_Channel_Set3DConeSettings(this.handle, insideconeangle, outsideconeangle, outsidevolume);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00003ED1 File Offset: 0x000020D1
		public RESULT get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
		{
			return Channel.FMOD5_Channel_Get3DConeSettings(this.handle, out insideconeangle, out outsideconeangle, out outsidevolume);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00003EE1 File Offset: 0x000020E1
		public RESULT set3DConeOrientation(ref VECTOR orientation)
		{
			return Channel.FMOD5_Channel_Set3DConeOrientation(this.handle, ref orientation);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00003EEF File Offset: 0x000020EF
		public RESULT get3DConeOrientation(out VECTOR orientation)
		{
			return Channel.FMOD5_Channel_Get3DConeOrientation(this.handle, out orientation);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00003EFD File Offset: 0x000020FD
		public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
		{
			return Channel.FMOD5_Channel_Set3DCustomRolloff(this.handle, ref points, numpoints);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00003F0C File Offset: 0x0000210C
		public RESULT get3DCustomRolloff(out IntPtr points, out int numpoints)
		{
			return Channel.FMOD5_Channel_Get3DCustomRolloff(this.handle, out points, out numpoints);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00003F1B File Offset: 0x0000211B
		public RESULT set3DOcclusion(float directocclusion, float reverbocclusion)
		{
			return Channel.FMOD5_Channel_Set3DOcclusion(this.handle, directocclusion, reverbocclusion);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00003F2A File Offset: 0x0000212A
		public RESULT get3DOcclusion(out float directocclusion, out float reverbocclusion)
		{
			return Channel.FMOD5_Channel_Get3DOcclusion(this.handle, out directocclusion, out reverbocclusion);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00003F39 File Offset: 0x00002139
		public RESULT set3DSpread(float angle)
		{
			return Channel.FMOD5_Channel_Set3DSpread(this.handle, angle);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00003F47 File Offset: 0x00002147
		public RESULT get3DSpread(out float angle)
		{
			return Channel.FMOD5_Channel_Get3DSpread(this.handle, out angle);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00003F55 File Offset: 0x00002155
		public RESULT set3DLevel(float level)
		{
			return Channel.FMOD5_Channel_Set3DLevel(this.handle, level);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00003F63 File Offset: 0x00002163
		public RESULT get3DLevel(out float level)
		{
			return Channel.FMOD5_Channel_Get3DLevel(this.handle, out level);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00003F71 File Offset: 0x00002171
		public RESULT set3DDopplerLevel(float level)
		{
			return Channel.FMOD5_Channel_Set3DDopplerLevel(this.handle, level);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00003F7F File Offset: 0x0000217F
		public RESULT get3DDopplerLevel(out float level)
		{
			return Channel.FMOD5_Channel_Get3DDopplerLevel(this.handle, out level);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00003F8D File Offset: 0x0000218D
		public RESULT set3DDistanceFilter(bool custom, float customLevel, float centerFreq)
		{
			return Channel.FMOD5_Channel_Set3DDistanceFilter(this.handle, custom, customLevel, centerFreq);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00003F9D File Offset: 0x0000219D
		public RESULT get3DDistanceFilter(out bool custom, out float customLevel, out float centerFreq)
		{
			return Channel.FMOD5_Channel_Get3DDistanceFilter(this.handle, out custom, out customLevel, out centerFreq);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00003FAD File Offset: 0x000021AD
		public RESULT setUserData(IntPtr userdata)
		{
			return Channel.FMOD5_Channel_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00003FBB File Offset: 0x000021BB
		public RESULT getUserData(out IntPtr userdata)
		{
			return Channel.FMOD5_Channel_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000239 RID: 569
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetFrequency(IntPtr channel, float frequency);

		// Token: 0x0600023A RID: 570
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetFrequency(IntPtr channel, out float frequency);

		// Token: 0x0600023B RID: 571
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetPriority(IntPtr channel, int priority);

		// Token: 0x0600023C RID: 572
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetPriority(IntPtr channel, out int priority);

		// Token: 0x0600023D RID: 573
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetPosition(IntPtr channel, uint position, TIMEUNIT postype);

		// Token: 0x0600023E RID: 574
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetPosition(IntPtr channel, out uint position, TIMEUNIT postype);

		// Token: 0x0600023F RID: 575
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetChannelGroup(IntPtr channel, IntPtr channelgroup);

		// Token: 0x06000240 RID: 576
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetChannelGroup(IntPtr channel, out IntPtr channelgroup);

		// Token: 0x06000241 RID: 577
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetLoopCount(IntPtr channel, int loopcount);

		// Token: 0x06000242 RID: 578
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetLoopCount(IntPtr channel, out int loopcount);

		// Token: 0x06000243 RID: 579
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetLoopPoints(IntPtr channel, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);

		// Token: 0x06000244 RID: 580
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetLoopPoints(IntPtr channel, out uint loopstart, TIMEUNIT loopstarttype, out uint loopend, TIMEUNIT loopendtype);

		// Token: 0x06000245 RID: 581
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_IsVirtual(IntPtr channel, out bool isvirtual);

		// Token: 0x06000246 RID: 582
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetCurrentSound(IntPtr channel, out IntPtr sound);

		// Token: 0x06000247 RID: 583
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetIndex(IntPtr channel, out int index);

		// Token: 0x06000248 RID: 584
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetSystemObject(IntPtr channel, out IntPtr system);

		// Token: 0x06000249 RID: 585
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Stop(IntPtr channel);

		// Token: 0x0600024A RID: 586
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetPaused(IntPtr channel, bool paused);

		// Token: 0x0600024B RID: 587
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetPaused(IntPtr channel, out bool paused);

		// Token: 0x0600024C RID: 588
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetVolume(IntPtr channel, float volume);

		// Token: 0x0600024D RID: 589
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetVolume(IntPtr channel, out float volume);

		// Token: 0x0600024E RID: 590
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetVolumeRamp(IntPtr channel, bool ramp);

		// Token: 0x0600024F RID: 591
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetVolumeRamp(IntPtr channel, out bool ramp);

		// Token: 0x06000250 RID: 592
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetAudibility(IntPtr channel, out float audibility);

		// Token: 0x06000251 RID: 593
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetPitch(IntPtr channel, float pitch);

		// Token: 0x06000252 RID: 594
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetPitch(IntPtr channel, out float pitch);

		// Token: 0x06000253 RID: 595
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetMute(IntPtr channel, bool mute);

		// Token: 0x06000254 RID: 596
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetMute(IntPtr channel, out bool mute);

		// Token: 0x06000255 RID: 597
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetReverbProperties(IntPtr channel, int instance, float wet);

		// Token: 0x06000256 RID: 598
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetReverbProperties(IntPtr channel, int instance, out float wet);

		// Token: 0x06000257 RID: 599
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetLowPassGain(IntPtr channel, float gain);

		// Token: 0x06000258 RID: 600
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetLowPassGain(IntPtr channel, out float gain);

		// Token: 0x06000259 RID: 601
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetMode(IntPtr channel, MODE mode);

		// Token: 0x0600025A RID: 602
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetMode(IntPtr channel, out MODE mode);

		// Token: 0x0600025B RID: 603
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetCallback(IntPtr channel, CHANNELCONTROL_CALLBACK callback);

		// Token: 0x0600025C RID: 604
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_IsPlaying(IntPtr channel, out bool isplaying);

		// Token: 0x0600025D RID: 605
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetPan(IntPtr channel, float pan);

		// Token: 0x0600025E RID: 606
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetMixLevelsOutput(IntPtr channel, float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);

		// Token: 0x0600025F RID: 607
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetMixLevelsInput(IntPtr channel, float[] levels, int numlevels);

		// Token: 0x06000260 RID: 608
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetMixMatrix(IntPtr channel, float[] matrix, int outchannels, int inchannels, int inchannel_hop);

		// Token: 0x06000261 RID: 609
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetMixMatrix(IntPtr channel, float[] matrix, out int outchannels, out int inchannels, int inchannel_hop);

		// Token: 0x06000262 RID: 610
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetDSPClock(IntPtr channel, out ulong dspclock, out ulong parentclock);

		// Token: 0x06000263 RID: 611
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetDelay(IntPtr channel, ulong dspclock_start, ulong dspclock_end, bool stopchannels);

		// Token: 0x06000264 RID: 612
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetDelay(IntPtr channel, out ulong dspclock_start, out ulong dspclock_end, IntPtr zero);

		// Token: 0x06000265 RID: 613
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetDelay(IntPtr channel, out ulong dspclock_start, out ulong dspclock_end, out bool stopchannels);

		// Token: 0x06000266 RID: 614
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_AddFadePoint(IntPtr channel, ulong dspclock, float volume);

		// Token: 0x06000267 RID: 615
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetFadePointRamp(IntPtr channel, ulong dspclock, float volume);

		// Token: 0x06000268 RID: 616
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_RemoveFadePoints(IntPtr channel, ulong dspclock_start, ulong dspclock_end);

		// Token: 0x06000269 RID: 617
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetFadePoints(IntPtr channel, ref uint numpoints, ulong[] point_dspclock, float[] point_volume);

		// Token: 0x0600026A RID: 618
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetDSP(IntPtr channel, int index, out IntPtr dsp);

		// Token: 0x0600026B RID: 619
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_AddDSP(IntPtr channel, int index, IntPtr dsp);

		// Token: 0x0600026C RID: 620
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_RemoveDSP(IntPtr channel, IntPtr dsp);

		// Token: 0x0600026D RID: 621
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetNumDSPs(IntPtr channel, out int numdsps);

		// Token: 0x0600026E RID: 622
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetDSPIndex(IntPtr channel, IntPtr dsp, int index);

		// Token: 0x0600026F RID: 623
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetDSPIndex(IntPtr channel, IntPtr dsp, out int index);

		// Token: 0x06000270 RID: 624
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DAttributes(IntPtr channel, ref VECTOR pos, ref VECTOR vel);

		// Token: 0x06000271 RID: 625
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DAttributes(IntPtr channel, out VECTOR pos, out VECTOR vel);

		// Token: 0x06000272 RID: 626
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DMinMaxDistance(IntPtr channel, float mindistance, float maxdistance);

		// Token: 0x06000273 RID: 627
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DMinMaxDistance(IntPtr channel, out float mindistance, out float maxdistance);

		// Token: 0x06000274 RID: 628
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DConeSettings(IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);

		// Token: 0x06000275 RID: 629
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DConeSettings(IntPtr channel, out float insideconeangle, out float outsideconeangle, out float outsidevolume);

		// Token: 0x06000276 RID: 630
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DConeOrientation(IntPtr channel, ref VECTOR orientation);

		// Token: 0x06000277 RID: 631
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DConeOrientation(IntPtr channel, out VECTOR orientation);

		// Token: 0x06000278 RID: 632
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DCustomRolloff(IntPtr channel, ref VECTOR points, int numpoints);

		// Token: 0x06000279 RID: 633
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DCustomRolloff(IntPtr channel, out IntPtr points, out int numpoints);

		// Token: 0x0600027A RID: 634
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DOcclusion(IntPtr channel, float directocclusion, float reverbocclusion);

		// Token: 0x0600027B RID: 635
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DOcclusion(IntPtr channel, out float directocclusion, out float reverbocclusion);

		// Token: 0x0600027C RID: 636
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DSpread(IntPtr channel, float angle);

		// Token: 0x0600027D RID: 637
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DSpread(IntPtr channel, out float angle);

		// Token: 0x0600027E RID: 638
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DLevel(IntPtr channel, float level);

		// Token: 0x0600027F RID: 639
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DLevel(IntPtr channel, out float level);

		// Token: 0x06000280 RID: 640
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DDopplerLevel(IntPtr channel, float level);

		// Token: 0x06000281 RID: 641
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DDopplerLevel(IntPtr channel, out float level);

		// Token: 0x06000282 RID: 642
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Set3DDistanceFilter(IntPtr channel, bool custom, float customLevel, float centerFreq);

		// Token: 0x06000283 RID: 643
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_Get3DDistanceFilter(IntPtr channel, out bool custom, out float customLevel, out float centerFreq);

		// Token: 0x06000284 RID: 644
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_SetUserData(IntPtr channel, IntPtr userdata);

		// Token: 0x06000285 RID: 645
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Channel_GetUserData(IntPtr channel, out IntPtr userdata);

		// Token: 0x06000286 RID: 646 RVA: 0x00003FC9 File Offset: 0x000021C9
		public Channel(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00003FD2 File Offset: 0x000021D2
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x00003FE4 File Offset: 0x000021E4
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000260 RID: 608
		public IntPtr handle;
	}
}

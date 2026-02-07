using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200004E RID: 78
	public struct ChannelGroup : IChannelControl
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00003FF1 File Offset: 0x000021F1
		public RESULT release()
		{
			return ChannelGroup.FMOD5_ChannelGroup_Release(this.handle);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00003FFE File Offset: 0x000021FE
		public RESULT addGroup(ChannelGroup group, bool propagatedspclock = true)
		{
			return ChannelGroup.FMOD5_ChannelGroup_AddGroup(this.handle, group.handle, propagatedspclock, IntPtr.Zero);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00004017 File Offset: 0x00002217
		public RESULT addGroup(ChannelGroup group, bool propagatedspclock, out DSPConnection connection)
		{
			return ChannelGroup.FMOD5_ChannelGroup_AddGroup(this.handle, group.handle, propagatedspclock, out connection.handle);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00004031 File Offset: 0x00002231
		public RESULT getNumGroups(out int numgroups)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetNumGroups(this.handle, out numgroups);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000403F File Offset: 0x0000223F
		public RESULT getGroup(int index, out ChannelGroup group)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetGroup(this.handle, index, out group.handle);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00004053 File Offset: 0x00002253
		public RESULT getParentGroup(out ChannelGroup group)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetParentGroup(this.handle, out group.handle);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00004068 File Offset: 0x00002268
		public RESULT getName(out string name, int namelen)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = ChannelGroup.FMOD5_ChannelGroup_GetName(this.handle, intPtr, namelen);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000040BC File Offset: 0x000022BC
		public RESULT getNumChannels(out int numchannels)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetNumChannels(this.handle, out numchannels);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000040CA File Offset: 0x000022CA
		public RESULT getChannel(int index, out Channel channel)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetChannel(this.handle, index, out channel.handle);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x000040DE File Offset: 0x000022DE
		public RESULT getSystemObject(out System system)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetSystemObject(this.handle, out system.handle);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000040F1 File Offset: 0x000022F1
		public RESULT stop()
		{
			return ChannelGroup.FMOD5_ChannelGroup_Stop(this.handle);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000040FE File Offset: 0x000022FE
		public RESULT setPaused(bool paused)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetPaused(this.handle, paused);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000410C File Offset: 0x0000230C
		public RESULT getPaused(out bool paused)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetPaused(this.handle, out paused);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000411A File Offset: 0x0000231A
		public RESULT setVolume(float volume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetVolume(this.handle, volume);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00004128 File Offset: 0x00002328
		public RESULT getVolume(out float volume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetVolume(this.handle, out volume);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00004136 File Offset: 0x00002336
		public RESULT setVolumeRamp(bool ramp)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetVolumeRamp(this.handle, ramp);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00004144 File Offset: 0x00002344
		public RESULT getVolumeRamp(out bool ramp)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetVolumeRamp(this.handle, out ramp);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00004152 File Offset: 0x00002352
		public RESULT getAudibility(out float audibility)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetAudibility(this.handle, out audibility);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00004160 File Offset: 0x00002360
		public RESULT setPitch(float pitch)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetPitch(this.handle, pitch);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000416E File Offset: 0x0000236E
		public RESULT getPitch(out float pitch)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetPitch(this.handle, out pitch);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000417C File Offset: 0x0000237C
		public RESULT setMute(bool mute)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetMute(this.handle, mute);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000418A File Offset: 0x0000238A
		public RESULT getMute(out bool mute)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetMute(this.handle, out mute);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00004198 File Offset: 0x00002398
		public RESULT setReverbProperties(int instance, float wet)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetReverbProperties(this.handle, instance, wet);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x000041A7 File Offset: 0x000023A7
		public RESULT getReverbProperties(int instance, out float wet)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetReverbProperties(this.handle, instance, out wet);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000041B6 File Offset: 0x000023B6
		public RESULT setLowPassGain(float gain)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetLowPassGain(this.handle, gain);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000041C4 File Offset: 0x000023C4
		public RESULT getLowPassGain(out float gain)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetLowPassGain(this.handle, out gain);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000041D2 File Offset: 0x000023D2
		public RESULT setMode(MODE mode)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetMode(this.handle, mode);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x000041E0 File Offset: 0x000023E0
		public RESULT getMode(out MODE mode)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetMode(this.handle, out mode);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x000041EE File Offset: 0x000023EE
		public RESULT setCallback(CHANNELCONTROL_CALLBACK callback)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetCallback(this.handle, callback);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000041FC File Offset: 0x000023FC
		public RESULT isPlaying(out bool isplaying)
		{
			return ChannelGroup.FMOD5_ChannelGroup_IsPlaying(this.handle, out isplaying);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000420A File Offset: 0x0000240A
		public RESULT setPan(float pan)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetPan(this.handle, pan);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00004218 File Offset: 0x00002418
		public RESULT setMixLevelsOutput(float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetMixLevelsOutput(this.handle, frontleft, frontright, center, lfe, surroundleft, surroundright, backleft, backright);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000423D File Offset: 0x0000243D
		public RESULT setMixLevelsInput(float[] levels, int numlevels)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetMixLevelsInput(this.handle, levels, numlevels);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000424C File Offset: 0x0000244C
		public RESULT setMixMatrix(float[] matrix, int outchannels, int inchannels, int inchannel_hop)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetMixMatrix(this.handle, matrix, outchannels, inchannels, inchannel_hop);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000425E File Offset: 0x0000245E
		public RESULT getMixMatrix(float[] matrix, out int outchannels, out int inchannels, int inchannel_hop)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetMixMatrix(this.handle, matrix, out outchannels, out inchannels, inchannel_hop);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00004270 File Offset: 0x00002470
		public RESULT getDSPClock(out ulong dspclock, out ulong parentclock)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetDSPClock(this.handle, out dspclock, out parentclock);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000427F File Offset: 0x0000247F
		public RESULT setDelay(ulong dspclock_start, ulong dspclock_end, bool stopchannels)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetDelay(this.handle, dspclock_start, dspclock_end, stopchannels);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000428F File Offset: 0x0000248F
		public RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetDelay(this.handle, out dspclock_start, out dspclock_end, IntPtr.Zero);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000042A3 File Offset: 0x000024A3
		public RESULT getDelay(out ulong dspclock_start, out ulong dspclock_end, out bool stopchannels)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetDelay(this.handle, out dspclock_start, out dspclock_end, out stopchannels);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000042B3 File Offset: 0x000024B3
		public RESULT addFadePoint(ulong dspclock, float volume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_AddFadePoint(this.handle, dspclock, volume);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000042C2 File Offset: 0x000024C2
		public RESULT setFadePointRamp(ulong dspclock, float volume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetFadePointRamp(this.handle, dspclock, volume);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000042D1 File Offset: 0x000024D1
		public RESULT removeFadePoints(ulong dspclock_start, ulong dspclock_end)
		{
			return ChannelGroup.FMOD5_ChannelGroup_RemoveFadePoints(this.handle, dspclock_start, dspclock_end);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x000042E0 File Offset: 0x000024E0
		public RESULT getFadePoints(ref uint numpoints, ulong[] point_dspclock, float[] point_volume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetFadePoints(this.handle, ref numpoints, point_dspclock, point_volume);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000042F0 File Offset: 0x000024F0
		public RESULT getDSP(int index, out DSP dsp)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetDSP(this.handle, index, out dsp.handle);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00004304 File Offset: 0x00002504
		public RESULT addDSP(int index, DSP dsp)
		{
			return ChannelGroup.FMOD5_ChannelGroup_AddDSP(this.handle, index, dsp.handle);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00004318 File Offset: 0x00002518
		public RESULT removeDSP(DSP dsp)
		{
			return ChannelGroup.FMOD5_ChannelGroup_RemoveDSP(this.handle, dsp.handle);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000432B File Offset: 0x0000252B
		public RESULT getNumDSPs(out int numdsps)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetNumDSPs(this.handle, out numdsps);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00004339 File Offset: 0x00002539
		public RESULT setDSPIndex(DSP dsp, int index)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetDSPIndex(this.handle, dsp.handle, index);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000434D File Offset: 0x0000254D
		public RESULT getDSPIndex(DSP dsp, out int index)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetDSPIndex(this.handle, dsp.handle, out index);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x00004361 File Offset: 0x00002561
		public RESULT set3DAttributes(ref VECTOR pos, ref VECTOR vel)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DAttributes(this.handle, ref pos, ref vel);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00004370 File Offset: 0x00002570
		public RESULT get3DAttributes(out VECTOR pos, out VECTOR vel)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DAttributes(this.handle, out pos, out vel);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000437F File Offset: 0x0000257F
		public RESULT set3DMinMaxDistance(float mindistance, float maxdistance)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DMinMaxDistance(this.handle, mindistance, maxdistance);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000438E File Offset: 0x0000258E
		public RESULT get3DMinMaxDistance(out float mindistance, out float maxdistance)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DMinMaxDistance(this.handle, out mindistance, out maxdistance);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000439D File Offset: 0x0000259D
		public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DConeSettings(this.handle, insideconeangle, outsideconeangle, outsidevolume);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000043AD File Offset: 0x000025AD
		public RESULT get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DConeSettings(this.handle, out insideconeangle, out outsideconeangle, out outsidevolume);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x000043BD File Offset: 0x000025BD
		public RESULT set3DConeOrientation(ref VECTOR orientation)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DConeOrientation(this.handle, ref orientation);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x000043CB File Offset: 0x000025CB
		public RESULT get3DConeOrientation(out VECTOR orientation)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DConeOrientation(this.handle, out orientation);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x000043D9 File Offset: 0x000025D9
		public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DCustomRolloff(this.handle, ref points, numpoints);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x000043E8 File Offset: 0x000025E8
		public RESULT get3DCustomRolloff(out IntPtr points, out int numpoints)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DCustomRolloff(this.handle, out points, out numpoints);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000043F7 File Offset: 0x000025F7
		public RESULT set3DOcclusion(float directocclusion, float reverbocclusion)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DOcclusion(this.handle, directocclusion, reverbocclusion);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00004406 File Offset: 0x00002606
		public RESULT get3DOcclusion(out float directocclusion, out float reverbocclusion)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DOcclusion(this.handle, out directocclusion, out reverbocclusion);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00004415 File Offset: 0x00002615
		public RESULT set3DSpread(float angle)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DSpread(this.handle, angle);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x00004423 File Offset: 0x00002623
		public RESULT get3DSpread(out float angle)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DSpread(this.handle, out angle);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00004431 File Offset: 0x00002631
		public RESULT set3DLevel(float level)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DLevel(this.handle, level);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000443F File Offset: 0x0000263F
		public RESULT get3DLevel(out float level)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DLevel(this.handle, out level);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000444D File Offset: 0x0000264D
		public RESULT set3DDopplerLevel(float level)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DDopplerLevel(this.handle, level);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000445B File Offset: 0x0000265B
		public RESULT get3DDopplerLevel(out float level)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DDopplerLevel(this.handle, out level);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00004469 File Offset: 0x00002669
		public RESULT set3DDistanceFilter(bool custom, float customLevel, float centerFreq)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Set3DDistanceFilter(this.handle, custom, customLevel, centerFreq);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00004479 File Offset: 0x00002679
		public RESULT get3DDistanceFilter(out bool custom, out float customLevel, out float centerFreq)
		{
			return ChannelGroup.FMOD5_ChannelGroup_Get3DDistanceFilter(this.handle, out custom, out customLevel, out centerFreq);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00004489 File Offset: 0x00002689
		public RESULT setUserData(IntPtr userdata)
		{
			return ChannelGroup.FMOD5_ChannelGroup_SetUserData(this.handle, userdata);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00004497 File Offset: 0x00002697
		public RESULT getUserData(out IntPtr userdata)
		{
			return ChannelGroup.FMOD5_ChannelGroup_GetUserData(this.handle, out userdata);
		}

		// Token: 0x060002D0 RID: 720
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Release(IntPtr channelgroup);

		// Token: 0x060002D1 RID: 721
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group, bool propagatedspclock, IntPtr zero);

		// Token: 0x060002D2 RID: 722
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group, bool propagatedspclock, out IntPtr connection);

		// Token: 0x060002D3 RID: 723
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetNumGroups(IntPtr channelgroup, out int numgroups);

		// Token: 0x060002D4 RID: 724
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetGroup(IntPtr channelgroup, int index, out IntPtr group);

		// Token: 0x060002D5 RID: 725
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetParentGroup(IntPtr channelgroup, out IntPtr group);

		// Token: 0x060002D6 RID: 726
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetName(IntPtr channelgroup, IntPtr name, int namelen);

		// Token: 0x060002D7 RID: 727
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetNumChannels(IntPtr channelgroup, out int numchannels);

		// Token: 0x060002D8 RID: 728
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetChannel(IntPtr channelgroup, int index, out IntPtr channel);

		// Token: 0x060002D9 RID: 729
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetSystemObject(IntPtr channelgroup, out IntPtr system);

		// Token: 0x060002DA RID: 730
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Stop(IntPtr channelgroup);

		// Token: 0x060002DB RID: 731
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetPaused(IntPtr channelgroup, bool paused);

		// Token: 0x060002DC RID: 732
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetPaused(IntPtr channelgroup, out bool paused);

		// Token: 0x060002DD RID: 733
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetVolume(IntPtr channelgroup, float volume);

		// Token: 0x060002DE RID: 734
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetVolume(IntPtr channelgroup, out float volume);

		// Token: 0x060002DF RID: 735
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetVolumeRamp(IntPtr channelgroup, bool ramp);

		// Token: 0x060002E0 RID: 736
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetVolumeRamp(IntPtr channelgroup, out bool ramp);

		// Token: 0x060002E1 RID: 737
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetAudibility(IntPtr channelgroup, out float audibility);

		// Token: 0x060002E2 RID: 738
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetPitch(IntPtr channelgroup, float pitch);

		// Token: 0x060002E3 RID: 739
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetPitch(IntPtr channelgroup, out float pitch);

		// Token: 0x060002E4 RID: 740
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetMute(IntPtr channelgroup, bool mute);

		// Token: 0x060002E5 RID: 741
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetMute(IntPtr channelgroup, out bool mute);

		// Token: 0x060002E6 RID: 742
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetReverbProperties(IntPtr channelgroup, int instance, float wet);

		// Token: 0x060002E7 RID: 743
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetReverbProperties(IntPtr channelgroup, int instance, out float wet);

		// Token: 0x060002E8 RID: 744
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetLowPassGain(IntPtr channelgroup, float gain);

		// Token: 0x060002E9 RID: 745
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetLowPassGain(IntPtr channelgroup, out float gain);

		// Token: 0x060002EA RID: 746
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetMode(IntPtr channelgroup, MODE mode);

		// Token: 0x060002EB RID: 747
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetMode(IntPtr channelgroup, out MODE mode);

		// Token: 0x060002EC RID: 748
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetCallback(IntPtr channelgroup, CHANNELCONTROL_CALLBACK callback);

		// Token: 0x060002ED RID: 749
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_IsPlaying(IntPtr channelgroup, out bool isplaying);

		// Token: 0x060002EE RID: 750
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetPan(IntPtr channelgroup, float pan);

		// Token: 0x060002EF RID: 751
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetMixLevelsOutput(IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float surroundleft, float surroundright, float backleft, float backright);

		// Token: 0x060002F0 RID: 752
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetMixLevelsInput(IntPtr channelgroup, float[] levels, int numlevels);

		// Token: 0x060002F1 RID: 753
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetMixMatrix(IntPtr channelgroup, float[] matrix, int outchannels, int inchannels, int inchannel_hop);

		// Token: 0x060002F2 RID: 754
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetMixMatrix(IntPtr channelgroup, float[] matrix, out int outchannels, out int inchannels, int inchannel_hop);

		// Token: 0x060002F3 RID: 755
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetDSPClock(IntPtr channelgroup, out ulong dspclock, out ulong parentclock);

		// Token: 0x060002F4 RID: 756
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetDelay(IntPtr channelgroup, ulong dspclock_start, ulong dspclock_end, bool stopchannels);

		// Token: 0x060002F5 RID: 757
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetDelay(IntPtr channelgroup, out ulong dspclock_start, out ulong dspclock_end, IntPtr zero);

		// Token: 0x060002F6 RID: 758
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetDelay(IntPtr channelgroup, out ulong dspclock_start, out ulong dspclock_end, out bool stopchannels);

		// Token: 0x060002F7 RID: 759
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_AddFadePoint(IntPtr channelgroup, ulong dspclock, float volume);

		// Token: 0x060002F8 RID: 760
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetFadePointRamp(IntPtr channelgroup, ulong dspclock, float volume);

		// Token: 0x060002F9 RID: 761
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_RemoveFadePoints(IntPtr channelgroup, ulong dspclock_start, ulong dspclock_end);

		// Token: 0x060002FA RID: 762
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetFadePoints(IntPtr channelgroup, ref uint numpoints, ulong[] point_dspclock, float[] point_volume);

		// Token: 0x060002FB RID: 763
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetDSP(IntPtr channelgroup, int index, out IntPtr dsp);

		// Token: 0x060002FC RID: 764
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_AddDSP(IntPtr channelgroup, int index, IntPtr dsp);

		// Token: 0x060002FD RID: 765
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_RemoveDSP(IntPtr channelgroup, IntPtr dsp);

		// Token: 0x060002FE RID: 766
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetNumDSPs(IntPtr channelgroup, out int numdsps);

		// Token: 0x060002FF RID: 767
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetDSPIndex(IntPtr channelgroup, IntPtr dsp, int index);

		// Token: 0x06000300 RID: 768
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetDSPIndex(IntPtr channelgroup, IntPtr dsp, out int index);

		// Token: 0x06000301 RID: 769
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DAttributes(IntPtr channelgroup, ref VECTOR pos, ref VECTOR vel);

		// Token: 0x06000302 RID: 770
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DAttributes(IntPtr channelgroup, out VECTOR pos, out VECTOR vel);

		// Token: 0x06000303 RID: 771
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DMinMaxDistance(IntPtr channelgroup, float mindistance, float maxdistance);

		// Token: 0x06000304 RID: 772
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DMinMaxDistance(IntPtr channelgroup, out float mindistance, out float maxdistance);

		// Token: 0x06000305 RID: 773
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DConeSettings(IntPtr channelgroup, float insideconeangle, float outsideconeangle, float outsidevolume);

		// Token: 0x06000306 RID: 774
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DConeSettings(IntPtr channelgroup, out float insideconeangle, out float outsideconeangle, out float outsidevolume);

		// Token: 0x06000307 RID: 775
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DConeOrientation(IntPtr channelgroup, ref VECTOR orientation);

		// Token: 0x06000308 RID: 776
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DConeOrientation(IntPtr channelgroup, out VECTOR orientation);

		// Token: 0x06000309 RID: 777
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DCustomRolloff(IntPtr channelgroup, ref VECTOR points, int numpoints);

		// Token: 0x0600030A RID: 778
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DCustomRolloff(IntPtr channelgroup, out IntPtr points, out int numpoints);

		// Token: 0x0600030B RID: 779
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DOcclusion(IntPtr channelgroup, float directocclusion, float reverbocclusion);

		// Token: 0x0600030C RID: 780
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DOcclusion(IntPtr channelgroup, out float directocclusion, out float reverbocclusion);

		// Token: 0x0600030D RID: 781
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DSpread(IntPtr channelgroup, float angle);

		// Token: 0x0600030E RID: 782
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DSpread(IntPtr channelgroup, out float angle);

		// Token: 0x0600030F RID: 783
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DLevel(IntPtr channelgroup, float level);

		// Token: 0x06000310 RID: 784
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DLevel(IntPtr channelgroup, out float level);

		// Token: 0x06000311 RID: 785
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DDopplerLevel(IntPtr channelgroup, float level);

		// Token: 0x06000312 RID: 786
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DDopplerLevel(IntPtr channelgroup, out float level);

		// Token: 0x06000313 RID: 787
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Set3DDistanceFilter(IntPtr channelgroup, bool custom, float customLevel, float centerFreq);

		// Token: 0x06000314 RID: 788
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_Get3DDistanceFilter(IntPtr channelgroup, out bool custom, out float customLevel, out float centerFreq);

		// Token: 0x06000315 RID: 789
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_SetUserData(IntPtr channelgroup, IntPtr userdata);

		// Token: 0x06000316 RID: 790
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_ChannelGroup_GetUserData(IntPtr channelgroup, out IntPtr userdata);

		// Token: 0x06000317 RID: 791 RVA: 0x000044A5 File Offset: 0x000026A5
		public ChannelGroup(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000044AE File Offset: 0x000026AE
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000044C0 File Offset: 0x000026C0
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000261 RID: 609
		public IntPtr handle;
	}
}

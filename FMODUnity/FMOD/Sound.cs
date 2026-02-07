using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200004B RID: 75
	public struct Sound
	{
		// Token: 0x06000152 RID: 338 RVA: 0x00003710 File Offset: 0x00001910
		public RESULT release()
		{
			return Sound.FMOD5_Sound_Release(this.handle);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000371D File Offset: 0x0000191D
		public RESULT getSystemObject(out System system)
		{
			return Sound.FMOD5_Sound_GetSystemObject(this.handle, out system.handle);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00003730 File Offset: 0x00001930
		public RESULT @lock(uint offset, uint length, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2)
		{
			return Sound.FMOD5_Sound_Lock(this.handle, offset, length, out ptr1, out ptr2, out len1, out len2);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00003746 File Offset: 0x00001946
		public RESULT unlock(IntPtr ptr1, IntPtr ptr2, uint len1, uint len2)
		{
			return Sound.FMOD5_Sound_Unlock(this.handle, ptr1, ptr2, len1, len2);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00003758 File Offset: 0x00001958
		public RESULT setDefaults(float frequency, int priority)
		{
			return Sound.FMOD5_Sound_SetDefaults(this.handle, frequency, priority);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00003767 File Offset: 0x00001967
		public RESULT getDefaults(out float frequency, out int priority)
		{
			return Sound.FMOD5_Sound_GetDefaults(this.handle, out frequency, out priority);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00003776 File Offset: 0x00001976
		public RESULT set3DMinMaxDistance(float min, float max)
		{
			return Sound.FMOD5_Sound_Set3DMinMaxDistance(this.handle, min, max);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00003785 File Offset: 0x00001985
		public RESULT get3DMinMaxDistance(out float min, out float max)
		{
			return Sound.FMOD5_Sound_Get3DMinMaxDistance(this.handle, out min, out max);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00003794 File Offset: 0x00001994
		public RESULT set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
		{
			return Sound.FMOD5_Sound_Set3DConeSettings(this.handle, insideconeangle, outsideconeangle, outsidevolume);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000037A4 File Offset: 0x000019A4
		public RESULT get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
		{
			return Sound.FMOD5_Sound_Get3DConeSettings(this.handle, out insideconeangle, out outsideconeangle, out outsidevolume);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000037B4 File Offset: 0x000019B4
		public RESULT set3DCustomRolloff(ref VECTOR points, int numpoints)
		{
			return Sound.FMOD5_Sound_Set3DCustomRolloff(this.handle, ref points, numpoints);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000037C3 File Offset: 0x000019C3
		public RESULT get3DCustomRolloff(out IntPtr points, out int numpoints)
		{
			return Sound.FMOD5_Sound_Get3DCustomRolloff(this.handle, out points, out numpoints);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x000037D2 File Offset: 0x000019D2
		public RESULT getSubSound(int index, out Sound subsound)
		{
			return Sound.FMOD5_Sound_GetSubSound(this.handle, index, out subsound.handle);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x000037E6 File Offset: 0x000019E6
		public RESULT getSubSoundParent(out Sound parentsound)
		{
			return Sound.FMOD5_Sound_GetSubSoundParent(this.handle, out parentsound.handle);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000037FC File Offset: 0x000019FC
		public RESULT getName(out string name, int namelen)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = Sound.FMOD5_Sound_GetName(this.handle, intPtr, namelen);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003850 File Offset: 0x00001A50
		public RESULT getLength(out uint length, TIMEUNIT lengthtype)
		{
			return Sound.FMOD5_Sound_GetLength(this.handle, out length, lengthtype);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000385F File Offset: 0x00001A5F
		public RESULT getFormat(out SOUND_TYPE type, out SOUND_FORMAT format, out int channels, out int bits)
		{
			return Sound.FMOD5_Sound_GetFormat(this.handle, out type, out format, out channels, out bits);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003871 File Offset: 0x00001A71
		public RESULT getNumSubSounds(out int numsubsounds)
		{
			return Sound.FMOD5_Sound_GetNumSubSounds(this.handle, out numsubsounds);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000387F File Offset: 0x00001A7F
		public RESULT getNumTags(out int numtags, out int numtagsupdated)
		{
			return Sound.FMOD5_Sound_GetNumTags(this.handle, out numtags, out numtagsupdated);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00003890 File Offset: 0x00001A90
		public RESULT getTag(string name, int index, out TAG tag)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = Sound.FMOD5_Sound_GetTag(this.handle, freeHelper.byteFromStringUTF8(name), index, out tag);
			}
			return result;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000038D8 File Offset: 0x00001AD8
		public RESULT getOpenState(out OPENSTATE openstate, out uint percentbuffered, out bool starving, out bool diskbusy)
		{
			return Sound.FMOD5_Sound_GetOpenState(this.handle, out openstate, out percentbuffered, out starving, out diskbusy);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x000038EA File Offset: 0x00001AEA
		public RESULT readData(byte[] buffer)
		{
			return Sound.FMOD5_Sound_ReadData(this.handle, buffer, (uint)buffer.Length, IntPtr.Zero);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003900 File Offset: 0x00001B00
		public RESULT readData(byte[] buffer, out uint read)
		{
			return Sound.FMOD5_Sound_ReadData(this.handle, buffer, (uint)buffer.Length, out read);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00003912 File Offset: 0x00001B12
		public RESULT seekData(uint pcm)
		{
			return Sound.FMOD5_Sound_SeekData(this.handle, pcm);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00003920 File Offset: 0x00001B20
		public RESULT setSoundGroup(SoundGroup soundgroup)
		{
			return Sound.FMOD5_Sound_SetSoundGroup(this.handle, soundgroup.handle);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00003933 File Offset: 0x00001B33
		public RESULT getSoundGroup(out SoundGroup soundgroup)
		{
			return Sound.FMOD5_Sound_GetSoundGroup(this.handle, out soundgroup.handle);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00003946 File Offset: 0x00001B46
		public RESULT getNumSyncPoints(out int numsyncpoints)
		{
			return Sound.FMOD5_Sound_GetNumSyncPoints(this.handle, out numsyncpoints);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00003954 File Offset: 0x00001B54
		public RESULT getSyncPoint(int index, out IntPtr point)
		{
			return Sound.FMOD5_Sound_GetSyncPoint(this.handle, index, out point);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00003964 File Offset: 0x00001B64
		public RESULT getSyncPointInfo(IntPtr point, out string name, int namelen, out uint offset, TIMEUNIT offsettype)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = Sound.FMOD5_Sound_GetSyncPointInfo(this.handle, point, intPtr, namelen, out offset, offsettype);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000039C0 File Offset: 0x00001BC0
		public RESULT getSyncPointInfo(IntPtr point, out uint offset, TIMEUNIT offsettype)
		{
			return Sound.FMOD5_Sound_GetSyncPointInfo(this.handle, point, IntPtr.Zero, 0, out offset, offsettype);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x000039D8 File Offset: 0x00001BD8
		public RESULT addSyncPoint(uint offset, TIMEUNIT offsettype, string name, out IntPtr point)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = Sound.FMOD5_Sound_AddSyncPoint(this.handle, offset, offsettype, freeHelper.byteFromStringUTF8(name), out point);
			}
			return result;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00003A20 File Offset: 0x00001C20
		public RESULT deleteSyncPoint(IntPtr point)
		{
			return Sound.FMOD5_Sound_DeleteSyncPoint(this.handle, point);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00003A2E File Offset: 0x00001C2E
		public RESULT setMode(MODE mode)
		{
			return Sound.FMOD5_Sound_SetMode(this.handle, mode);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00003A3C File Offset: 0x00001C3C
		public RESULT getMode(out MODE mode)
		{
			return Sound.FMOD5_Sound_GetMode(this.handle, out mode);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00003A4A File Offset: 0x00001C4A
		public RESULT setLoopCount(int loopcount)
		{
			return Sound.FMOD5_Sound_SetLoopCount(this.handle, loopcount);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00003A58 File Offset: 0x00001C58
		public RESULT getLoopCount(out int loopcount)
		{
			return Sound.FMOD5_Sound_GetLoopCount(this.handle, out loopcount);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00003A66 File Offset: 0x00001C66
		public RESULT setLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
		{
			return Sound.FMOD5_Sound_SetLoopPoints(this.handle, loopstart, loopstarttype, loopend, loopendtype);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00003A78 File Offset: 0x00001C78
		public RESULT getLoopPoints(out uint loopstart, TIMEUNIT loopstarttype, out uint loopend, TIMEUNIT loopendtype)
		{
			return Sound.FMOD5_Sound_GetLoopPoints(this.handle, out loopstart, loopstarttype, out loopend, loopendtype);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00003A8A File Offset: 0x00001C8A
		public RESULT getMusicNumChannels(out int numchannels)
		{
			return Sound.FMOD5_Sound_GetMusicNumChannels(this.handle, out numchannels);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00003A98 File Offset: 0x00001C98
		public RESULT setMusicChannelVolume(int channel, float volume)
		{
			return Sound.FMOD5_Sound_SetMusicChannelVolume(this.handle, channel, volume);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00003AA7 File Offset: 0x00001CA7
		public RESULT getMusicChannelVolume(int channel, out float volume)
		{
			return Sound.FMOD5_Sound_GetMusicChannelVolume(this.handle, channel, out volume);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00003AB6 File Offset: 0x00001CB6
		public RESULT setMusicSpeed(float speed)
		{
			return Sound.FMOD5_Sound_SetMusicSpeed(this.handle, speed);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00003AC4 File Offset: 0x00001CC4
		public RESULT getMusicSpeed(out float speed)
		{
			return Sound.FMOD5_Sound_GetMusicSpeed(this.handle, out speed);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00003AD2 File Offset: 0x00001CD2
		public RESULT setUserData(IntPtr userdata)
		{
			return Sound.FMOD5_Sound_SetUserData(this.handle, userdata);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00003AE0 File Offset: 0x00001CE0
		public RESULT getUserData(out IntPtr userdata)
		{
			return Sound.FMOD5_Sound_GetUserData(this.handle, out userdata);
		}

		// Token: 0x0600017F RID: 383
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Release(IntPtr sound);

		// Token: 0x06000180 RID: 384
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSystemObject(IntPtr sound, out IntPtr system);

		// Token: 0x06000181 RID: 385
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Lock(IntPtr sound, uint offset, uint length, out IntPtr ptr1, out IntPtr ptr2, out uint len1, out uint len2);

		// Token: 0x06000182 RID: 386
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Unlock(IntPtr sound, IntPtr ptr1, IntPtr ptr2, uint len1, uint len2);

		// Token: 0x06000183 RID: 387
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetDefaults(IntPtr sound, float frequency, int priority);

		// Token: 0x06000184 RID: 388
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetDefaults(IntPtr sound, out float frequency, out int priority);

		// Token: 0x06000185 RID: 389
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Set3DMinMaxDistance(IntPtr sound, float min, float max);

		// Token: 0x06000186 RID: 390
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Get3DMinMaxDistance(IntPtr sound, out float min, out float max);

		// Token: 0x06000187 RID: 391
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Set3DConeSettings(IntPtr sound, float insideconeangle, float outsideconeangle, float outsidevolume);

		// Token: 0x06000188 RID: 392
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Get3DConeSettings(IntPtr sound, out float insideconeangle, out float outsideconeangle, out float outsidevolume);

		// Token: 0x06000189 RID: 393
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Set3DCustomRolloff(IntPtr sound, ref VECTOR points, int numpoints);

		// Token: 0x0600018A RID: 394
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_Get3DCustomRolloff(IntPtr sound, out IntPtr points, out int numpoints);

		// Token: 0x0600018B RID: 395
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSubSound(IntPtr sound, int index, out IntPtr subsound);

		// Token: 0x0600018C RID: 396
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSubSoundParent(IntPtr sound, out IntPtr parentsound);

		// Token: 0x0600018D RID: 397
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetName(IntPtr sound, IntPtr name, int namelen);

		// Token: 0x0600018E RID: 398
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetLength(IntPtr sound, out uint length, TIMEUNIT lengthtype);

		// Token: 0x0600018F RID: 399
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetFormat(IntPtr sound, out SOUND_TYPE type, out SOUND_FORMAT format, out int channels, out int bits);

		// Token: 0x06000190 RID: 400
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetNumSubSounds(IntPtr sound, out int numsubsounds);

		// Token: 0x06000191 RID: 401
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetNumTags(IntPtr sound, out int numtags, out int numtagsupdated);

		// Token: 0x06000192 RID: 402
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetTag(IntPtr sound, byte[] name, int index, out TAG tag);

		// Token: 0x06000193 RID: 403
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetOpenState(IntPtr sound, out OPENSTATE openstate, out uint percentbuffered, out bool starving, out bool diskbusy);

		// Token: 0x06000194 RID: 404
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_ReadData(IntPtr sound, byte[] buffer, uint length, IntPtr zero);

		// Token: 0x06000195 RID: 405
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_ReadData(IntPtr sound, byte[] buffer, uint length, out uint read);

		// Token: 0x06000196 RID: 406
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SeekData(IntPtr sound, uint pcm);

		// Token: 0x06000197 RID: 407
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetSoundGroup(IntPtr sound, IntPtr soundgroup);

		// Token: 0x06000198 RID: 408
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSoundGroup(IntPtr sound, out IntPtr soundgroup);

		// Token: 0x06000199 RID: 409
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetNumSyncPoints(IntPtr sound, out int numsyncpoints);

		// Token: 0x0600019A RID: 410
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSyncPoint(IntPtr sound, int index, out IntPtr point);

		// Token: 0x0600019B RID: 411
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetSyncPointInfo(IntPtr sound, IntPtr point, IntPtr name, int namelen, out uint offset, TIMEUNIT offsettype);

		// Token: 0x0600019C RID: 412
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_AddSyncPoint(IntPtr sound, uint offset, TIMEUNIT offsettype, byte[] name, out IntPtr point);

		// Token: 0x0600019D RID: 413
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_DeleteSyncPoint(IntPtr sound, IntPtr point);

		// Token: 0x0600019E RID: 414
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetMode(IntPtr sound, MODE mode);

		// Token: 0x0600019F RID: 415
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetMode(IntPtr sound, out MODE mode);

		// Token: 0x060001A0 RID: 416
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetLoopCount(IntPtr sound, int loopcount);

		// Token: 0x060001A1 RID: 417
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetLoopCount(IntPtr sound, out int loopcount);

		// Token: 0x060001A2 RID: 418
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetLoopPoints(IntPtr sound, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);

		// Token: 0x060001A3 RID: 419
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetLoopPoints(IntPtr sound, out uint loopstart, TIMEUNIT loopstarttype, out uint loopend, TIMEUNIT loopendtype);

		// Token: 0x060001A4 RID: 420
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetMusicNumChannels(IntPtr sound, out int numchannels);

		// Token: 0x060001A5 RID: 421
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetMusicChannelVolume(IntPtr sound, int channel, float volume);

		// Token: 0x060001A6 RID: 422
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetMusicChannelVolume(IntPtr sound, int channel, out float volume);

		// Token: 0x060001A7 RID: 423
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetMusicSpeed(IntPtr sound, float speed);

		// Token: 0x060001A8 RID: 424
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetMusicSpeed(IntPtr sound, out float speed);

		// Token: 0x060001A9 RID: 425
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_SetUserData(IntPtr sound, IntPtr userdata);

		// Token: 0x060001AA RID: 426
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Sound_GetUserData(IntPtr sound, out IntPtr userdata);

		// Token: 0x060001AB RID: 427 RVA: 0x00003AEE File Offset: 0x00001CEE
		public Sound(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00003AF7 File Offset: 0x00001CF7
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00003B09 File Offset: 0x00001D09
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x0400025F RID: 607
		public IntPtr handle;
	}
}

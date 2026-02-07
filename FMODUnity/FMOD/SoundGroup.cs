using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x0200004F RID: 79
	public struct SoundGroup
	{
		// Token: 0x0600031A RID: 794 RVA: 0x000044CD File Offset: 0x000026CD
		public RESULT release()
		{
			return SoundGroup.FMOD5_SoundGroup_Release(this.handle);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000044DA File Offset: 0x000026DA
		public RESULT getSystemObject(out System system)
		{
			return SoundGroup.FMOD5_SoundGroup_GetSystemObject(this.handle, out system.handle);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x000044ED File Offset: 0x000026ED
		public RESULT setMaxAudible(int maxaudible)
		{
			return SoundGroup.FMOD5_SoundGroup_SetMaxAudible(this.handle, maxaudible);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x000044FB File Offset: 0x000026FB
		public RESULT getMaxAudible(out int maxaudible)
		{
			return SoundGroup.FMOD5_SoundGroup_GetMaxAudible(this.handle, out maxaudible);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00004509 File Offset: 0x00002709
		public RESULT setMaxAudibleBehavior(SOUNDGROUP_BEHAVIOR behavior)
		{
			return SoundGroup.FMOD5_SoundGroup_SetMaxAudibleBehavior(this.handle, behavior);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00004517 File Offset: 0x00002717
		public RESULT getMaxAudibleBehavior(out SOUNDGROUP_BEHAVIOR behavior)
		{
			return SoundGroup.FMOD5_SoundGroup_GetMaxAudibleBehavior(this.handle, out behavior);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00004525 File Offset: 0x00002725
		public RESULT setMuteFadeSpeed(float speed)
		{
			return SoundGroup.FMOD5_SoundGroup_SetMuteFadeSpeed(this.handle, speed);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00004533 File Offset: 0x00002733
		public RESULT getMuteFadeSpeed(out float speed)
		{
			return SoundGroup.FMOD5_SoundGroup_GetMuteFadeSpeed(this.handle, out speed);
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00004541 File Offset: 0x00002741
		public RESULT setVolume(float volume)
		{
			return SoundGroup.FMOD5_SoundGroup_SetVolume(this.handle, volume);
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000454F File Offset: 0x0000274F
		public RESULT getVolume(out float volume)
		{
			return SoundGroup.FMOD5_SoundGroup_GetVolume(this.handle, out volume);
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000455D File Offset: 0x0000275D
		public RESULT stop()
		{
			return SoundGroup.FMOD5_SoundGroup_Stop(this.handle);
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000456C File Offset: 0x0000276C
		public RESULT getName(out string name, int namelen)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(namelen);
			RESULT result = SoundGroup.FMOD5_SoundGroup_GetName(this.handle, intPtr, namelen);
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				name = freeHelper.stringFromNative(intPtr);
			}
			Marshal.FreeHGlobal(intPtr);
			return result;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x000045C0 File Offset: 0x000027C0
		public RESULT getNumSounds(out int numsounds)
		{
			return SoundGroup.FMOD5_SoundGroup_GetNumSounds(this.handle, out numsounds);
		}

		// Token: 0x06000327 RID: 807 RVA: 0x000045CE File Offset: 0x000027CE
		public RESULT getSound(int index, out Sound sound)
		{
			return SoundGroup.FMOD5_SoundGroup_GetSound(this.handle, index, out sound.handle);
		}

		// Token: 0x06000328 RID: 808 RVA: 0x000045E2 File Offset: 0x000027E2
		public RESULT getNumPlaying(out int numplaying)
		{
			return SoundGroup.FMOD5_SoundGroup_GetNumPlaying(this.handle, out numplaying);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x000045F0 File Offset: 0x000027F0
		public RESULT setUserData(IntPtr userdata)
		{
			return SoundGroup.FMOD5_SoundGroup_SetUserData(this.handle, userdata);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000045FE File Offset: 0x000027FE
		public RESULT getUserData(out IntPtr userdata)
		{
			return SoundGroup.FMOD5_SoundGroup_GetUserData(this.handle, out userdata);
		}

		// Token: 0x0600032B RID: 811
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_Release(IntPtr soundgroup);

		// Token: 0x0600032C RID: 812
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetSystemObject(IntPtr soundgroup, out IntPtr system);

		// Token: 0x0600032D RID: 813
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_SetMaxAudible(IntPtr soundgroup, int maxaudible);

		// Token: 0x0600032E RID: 814
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetMaxAudible(IntPtr soundgroup, out int maxaudible);

		// Token: 0x0600032F RID: 815
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_SetMaxAudibleBehavior(IntPtr soundgroup, SOUNDGROUP_BEHAVIOR behavior);

		// Token: 0x06000330 RID: 816
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetMaxAudibleBehavior(IntPtr soundgroup, out SOUNDGROUP_BEHAVIOR behavior);

		// Token: 0x06000331 RID: 817
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_SetMuteFadeSpeed(IntPtr soundgroup, float speed);

		// Token: 0x06000332 RID: 818
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetMuteFadeSpeed(IntPtr soundgroup, out float speed);

		// Token: 0x06000333 RID: 819
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_SetVolume(IntPtr soundgroup, float volume);

		// Token: 0x06000334 RID: 820
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetVolume(IntPtr soundgroup, out float volume);

		// Token: 0x06000335 RID: 821
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_Stop(IntPtr soundgroup);

		// Token: 0x06000336 RID: 822
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetName(IntPtr soundgroup, IntPtr name, int namelen);

		// Token: 0x06000337 RID: 823
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetNumSounds(IntPtr soundgroup, out int numsounds);

		// Token: 0x06000338 RID: 824
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetSound(IntPtr soundgroup, int index, out IntPtr sound);

		// Token: 0x06000339 RID: 825
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetNumPlaying(IntPtr soundgroup, out int numplaying);

		// Token: 0x0600033A RID: 826
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_SetUserData(IntPtr soundgroup, IntPtr userdata);

		// Token: 0x0600033B RID: 827
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_SoundGroup_GetUserData(IntPtr soundgroup, out IntPtr userdata);

		// Token: 0x0600033C RID: 828 RVA: 0x0000460C File Offset: 0x0000280C
		public SoundGroup(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00004615 File Offset: 0x00002815
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00004627 File Offset: 0x00002827
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x04000262 RID: 610
		public IntPtr handle;
	}
}

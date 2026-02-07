using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F2 RID: 242
	public struct Bus
	{
		// Token: 0x060005C2 RID: 1474 RVA: 0x00006754 File Offset: 0x00004954
		public RESULT getID(out GUID id)
		{
			return Bus.FMOD_Studio_Bus_GetID(this.handle, out id);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00006764 File Offset: 0x00004964
		public RESULT getPath(out string path)
		{
			path = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = Bus.FMOD_Studio_Bus_GetPath(this.handle, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = Bus.FMOD_Studio_Bus_GetPath(this.handle, intPtr, num, out num);
				}
				if (result == RESULT.OK)
				{
					path = freeHelper.stringFromNative(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000067F0 File Offset: 0x000049F0
		public RESULT getVolume(out float volume)
		{
			float num;
			return this.getVolume(out volume, out num);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00006806 File Offset: 0x00004A06
		public RESULT getVolume(out float volume, out float finalvolume)
		{
			return Bus.FMOD_Studio_Bus_GetVolume(this.handle, out volume, out finalvolume);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00006815 File Offset: 0x00004A15
		public RESULT setVolume(float volume)
		{
			return Bus.FMOD_Studio_Bus_SetVolume(this.handle, volume);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00006823 File Offset: 0x00004A23
		public RESULT getPaused(out bool paused)
		{
			return Bus.FMOD_Studio_Bus_GetPaused(this.handle, out paused);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00006831 File Offset: 0x00004A31
		public RESULT setPaused(bool paused)
		{
			return Bus.FMOD_Studio_Bus_SetPaused(this.handle, paused);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0000683F File Offset: 0x00004A3F
		public RESULT getMute(out bool mute)
		{
			return Bus.FMOD_Studio_Bus_GetMute(this.handle, out mute);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0000684D File Offset: 0x00004A4D
		public RESULT setMute(bool mute)
		{
			return Bus.FMOD_Studio_Bus_SetMute(this.handle, mute);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0000685B File Offset: 0x00004A5B
		public RESULT stopAllEvents(STOP_MODE mode)
		{
			return Bus.FMOD_Studio_Bus_StopAllEvents(this.handle, mode);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00006869 File Offset: 0x00004A69
		public RESULT lockChannelGroup()
		{
			return Bus.FMOD_Studio_Bus_LockChannelGroup(this.handle);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00006876 File Offset: 0x00004A76
		public RESULT unlockChannelGroup()
		{
			return Bus.FMOD_Studio_Bus_UnlockChannelGroup(this.handle);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00006883 File Offset: 0x00004A83
		public RESULT getChannelGroup(out ChannelGroup group)
		{
			return Bus.FMOD_Studio_Bus_GetChannelGroup(this.handle, out group.handle);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00006896 File Offset: 0x00004A96
		public RESULT getCPUUsage(out uint exclusive, out uint inclusive)
		{
			return Bus.FMOD_Studio_Bus_GetCPUUsage(this.handle, out exclusive, out inclusive);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000068A5 File Offset: 0x00004AA5
		public RESULT getMemoryUsage(out MEMORY_USAGE memoryusage)
		{
			return Bus.FMOD_Studio_Bus_GetMemoryUsage(this.handle, out memoryusage);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000068B3 File Offset: 0x00004AB3
		public RESULT getPortIndex(out ulong index)
		{
			return Bus.FMOD_Studio_Bus_GetPortIndex(this.handle, out index);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000068C1 File Offset: 0x00004AC1
		public RESULT setPortIndex(ulong index)
		{
			return Bus.FMOD_Studio_Bus_SetPortIndex(this.handle, index);
		}

		// Token: 0x060005D3 RID: 1491
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_Bus_IsValid(IntPtr bus);

		// Token: 0x060005D4 RID: 1492
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetID(IntPtr bus, out GUID id);

		// Token: 0x060005D5 RID: 1493
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetPath(IntPtr bus, IntPtr path, int size, out int retrieved);

		// Token: 0x060005D6 RID: 1494
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetVolume(IntPtr bus, out float volume, out float finalvolume);

		// Token: 0x060005D7 RID: 1495
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_SetVolume(IntPtr bus, float volume);

		// Token: 0x060005D8 RID: 1496
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetPaused(IntPtr bus, out bool paused);

		// Token: 0x060005D9 RID: 1497
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_SetPaused(IntPtr bus, bool paused);

		// Token: 0x060005DA RID: 1498
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetMute(IntPtr bus, out bool mute);

		// Token: 0x060005DB RID: 1499
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_SetMute(IntPtr bus, bool mute);

		// Token: 0x060005DC RID: 1500
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_StopAllEvents(IntPtr bus, STOP_MODE mode);

		// Token: 0x060005DD RID: 1501
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_LockChannelGroup(IntPtr bus);

		// Token: 0x060005DE RID: 1502
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_UnlockChannelGroup(IntPtr bus);

		// Token: 0x060005DF RID: 1503
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetChannelGroup(IntPtr bus, out IntPtr group);

		// Token: 0x060005E0 RID: 1504
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetCPUUsage(IntPtr bus, out uint exclusive, out uint inclusive);

		// Token: 0x060005E1 RID: 1505
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetMemoryUsage(IntPtr bus, out MEMORY_USAGE memoryusage);

		// Token: 0x060005E2 RID: 1506
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_GetPortIndex(IntPtr bus, out ulong index);

		// Token: 0x060005E3 RID: 1507
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bus_SetPortIndex(IntPtr bus, ulong index);

		// Token: 0x060005E4 RID: 1508 RVA: 0x000068CF File Offset: 0x00004ACF
		public Bus(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x000068D8 File Offset: 0x00004AD8
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000068EA File Offset: 0x00004AEA
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x000068F7 File Offset: 0x00004AF7
		public bool isValid()
		{
			return this.hasHandle() && Bus.FMOD_Studio_Bus_IsValid(this.handle);
		}

		// Token: 0x0400054B RID: 1355
		public IntPtr handle;
	}
}

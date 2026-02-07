using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F3 RID: 243
	public struct VCA
	{
		// Token: 0x060005E8 RID: 1512 RVA: 0x0000690E File Offset: 0x00004B0E
		public RESULT getID(out GUID id)
		{
			return VCA.FMOD_Studio_VCA_GetID(this.handle, out id);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000691C File Offset: 0x00004B1C
		public RESULT getPath(out string path)
		{
			path = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = VCA.FMOD_Studio_VCA_GetPath(this.handle, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = VCA.FMOD_Studio_VCA_GetPath(this.handle, intPtr, num, out num);
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

		// Token: 0x060005EA RID: 1514 RVA: 0x000069A8 File Offset: 0x00004BA8
		public RESULT getVolume(out float volume)
		{
			float num;
			return this.getVolume(out volume, out num);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x000069BE File Offset: 0x00004BBE
		public RESULT getVolume(out float volume, out float finalvolume)
		{
			return VCA.FMOD_Studio_VCA_GetVolume(this.handle, out volume, out finalvolume);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000069CD File Offset: 0x00004BCD
		public RESULT setVolume(float volume)
		{
			return VCA.FMOD_Studio_VCA_SetVolume(this.handle, volume);
		}

		// Token: 0x060005ED RID: 1517
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_VCA_IsValid(IntPtr vca);

		// Token: 0x060005EE RID: 1518
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_VCA_GetID(IntPtr vca, out GUID id);

		// Token: 0x060005EF RID: 1519
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_VCA_GetPath(IntPtr vca, IntPtr path, int size, out int retrieved);

		// Token: 0x060005F0 RID: 1520
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_VCA_GetVolume(IntPtr vca, out float volume, out float finalvolume);

		// Token: 0x060005F1 RID: 1521
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_VCA_SetVolume(IntPtr vca, float volume);

		// Token: 0x060005F2 RID: 1522 RVA: 0x000069DB File Offset: 0x00004BDB
		public VCA(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000069E4 File Offset: 0x00004BE4
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000069F6 File Offset: 0x00004BF6
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00006A03 File Offset: 0x00004C03
		public bool isValid()
		{
			return this.hasHandle() && VCA.FMOD_Studio_VCA_IsValid(this.handle);
		}

		// Token: 0x0400054C RID: 1356
		public IntPtr handle;
	}
}

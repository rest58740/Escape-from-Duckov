using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F4 RID: 244
	public struct Bank
	{
		// Token: 0x060005F6 RID: 1526 RVA: 0x00006A1A File Offset: 0x00004C1A
		public RESULT getID(out GUID id)
		{
			return Bank.FMOD_Studio_Bank_GetID(this.handle, out id);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00006A28 File Offset: 0x00004C28
		public RESULT getPath(out string path)
		{
			path = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = Bank.FMOD_Studio_Bank_GetPath(this.handle, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = Bank.FMOD_Studio_Bank_GetPath(this.handle, intPtr, num, out num);
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

		// Token: 0x060005F8 RID: 1528 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public RESULT unload()
		{
			return Bank.FMOD_Studio_Bank_Unload(this.handle);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00006AC1 File Offset: 0x00004CC1
		public RESULT loadSampleData()
		{
			return Bank.FMOD_Studio_Bank_LoadSampleData(this.handle);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00006ACE File Offset: 0x00004CCE
		public RESULT unloadSampleData()
		{
			return Bank.FMOD_Studio_Bank_UnloadSampleData(this.handle);
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00006ADB File Offset: 0x00004CDB
		public RESULT getLoadingState(out LOADING_STATE state)
		{
			return Bank.FMOD_Studio_Bank_GetLoadingState(this.handle, out state);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00006AE9 File Offset: 0x00004CE9
		public RESULT getSampleLoadingState(out LOADING_STATE state)
		{
			return Bank.FMOD_Studio_Bank_GetSampleLoadingState(this.handle, out state);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00006AF7 File Offset: 0x00004CF7
		public RESULT getStringCount(out int count)
		{
			return Bank.FMOD_Studio_Bank_GetStringCount(this.handle, out count);
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00006B08 File Offset: 0x00004D08
		public RESULT getStringInfo(int index, out GUID id, out string path)
		{
			path = null;
			id = default(GUID);
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				int num = 0;
				RESULT result = Bank.FMOD_Studio_Bank_GetStringInfo(this.handle, index, out id, intPtr, 256, out num);
				if (result == RESULT.ERR_TRUNCATED)
				{
					Marshal.FreeHGlobal(intPtr);
					intPtr = Marshal.AllocHGlobal(num);
					result = Bank.FMOD_Studio_Bank_GetStringInfo(this.handle, index, out id, intPtr, num, out num);
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

		// Token: 0x060005FF RID: 1535 RVA: 0x00006BA0 File Offset: 0x00004DA0
		public RESULT getEventCount(out int count)
		{
			return Bank.FMOD_Studio_Bank_GetEventCount(this.handle, out count);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00006BB0 File Offset: 0x00004DB0
		public RESULT getEventList(out EventDescription[] array)
		{
			array = null;
			int num;
			RESULT result = Bank.FMOD_Studio_Bank_GetEventCount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new EventDescription[0];
				return result;
			}
			IntPtr[] array2 = new IntPtr[num];
			int num2;
			result = Bank.FMOD_Studio_Bank_GetEventList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 > num)
			{
				num2 = num;
			}
			array = new EventDescription[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i].handle = array2[i];
			}
			return RESULT.OK;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00006C2D File Offset: 0x00004E2D
		public RESULT getBusCount(out int count)
		{
			return Bank.FMOD_Studio_Bank_GetBusCount(this.handle, out count);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00006C3C File Offset: 0x00004E3C
		public RESULT getBusList(out Bus[] array)
		{
			array = null;
			int num;
			RESULT result = Bank.FMOD_Studio_Bank_GetBusCount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new Bus[0];
				return result;
			}
			IntPtr[] array2 = new IntPtr[num];
			int num2;
			result = Bank.FMOD_Studio_Bank_GetBusList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 > num)
			{
				num2 = num;
			}
			array = new Bus[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i].handle = array2[i];
			}
			return RESULT.OK;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00006CB9 File Offset: 0x00004EB9
		public RESULT getVCACount(out int count)
		{
			return Bank.FMOD_Studio_Bank_GetVCACount(this.handle, out count);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00006CC8 File Offset: 0x00004EC8
		public RESULT getVCAList(out VCA[] array)
		{
			array = null;
			int num;
			RESULT result = Bank.FMOD_Studio_Bank_GetVCACount(this.handle, out num);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num == 0)
			{
				array = new VCA[0];
				return result;
			}
			IntPtr[] array2 = new IntPtr[num];
			int num2;
			result = Bank.FMOD_Studio_Bank_GetVCAList(this.handle, array2, num, out num2);
			if (result != RESULT.OK)
			{
				return result;
			}
			if (num2 > num)
			{
				num2 = num;
			}
			array = new VCA[num2];
			for (int i = 0; i < num2; i++)
			{
				array[i].handle = array2[i];
			}
			return RESULT.OK;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00006D45 File Offset: 0x00004F45
		public RESULT getUserData(out IntPtr userdata)
		{
			return Bank.FMOD_Studio_Bank_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00006D53 File Offset: 0x00004F53
		public RESULT setUserData(IntPtr userdata)
		{
			return Bank.FMOD_Studio_Bank_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000607 RID: 1543
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_Bank_IsValid(IntPtr bank);

		// Token: 0x06000608 RID: 1544
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetID(IntPtr bank, out GUID id);

		// Token: 0x06000609 RID: 1545
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetPath(IntPtr bank, IntPtr path, int size, out int retrieved);

		// Token: 0x0600060A RID: 1546
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_Unload(IntPtr bank);

		// Token: 0x0600060B RID: 1547
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_LoadSampleData(IntPtr bank);

		// Token: 0x0600060C RID: 1548
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_UnloadSampleData(IntPtr bank);

		// Token: 0x0600060D RID: 1549
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetLoadingState(IntPtr bank, out LOADING_STATE state);

		// Token: 0x0600060E RID: 1550
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetSampleLoadingState(IntPtr bank, out LOADING_STATE state);

		// Token: 0x0600060F RID: 1551
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetStringCount(IntPtr bank, out int count);

		// Token: 0x06000610 RID: 1552
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetStringInfo(IntPtr bank, int index, out GUID id, IntPtr path, int size, out int retrieved);

		// Token: 0x06000611 RID: 1553
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetEventCount(IntPtr bank, out int count);

		// Token: 0x06000612 RID: 1554
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetEventList(IntPtr bank, IntPtr[] array, int capacity, out int count);

		// Token: 0x06000613 RID: 1555
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetBusCount(IntPtr bank, out int count);

		// Token: 0x06000614 RID: 1556
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetBusList(IntPtr bank, IntPtr[] array, int capacity, out int count);

		// Token: 0x06000615 RID: 1557
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetVCACount(IntPtr bank, out int count);

		// Token: 0x06000616 RID: 1558
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetVCAList(IntPtr bank, IntPtr[] array, int capacity, out int count);

		// Token: 0x06000617 RID: 1559
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_GetUserData(IntPtr bank, out IntPtr userdata);

		// Token: 0x06000618 RID: 1560
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_Bank_SetUserData(IntPtr bank, IntPtr userdata);

		// Token: 0x06000619 RID: 1561 RVA: 0x00006D61 File Offset: 0x00004F61
		public Bank(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00006D6A File Offset: 0x00004F6A
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00006D7C File Offset: 0x00004F7C
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00006D89 File Offset: 0x00004F89
		public bool isValid()
		{
			return this.hasHandle() && Bank.FMOD_Studio_Bank_IsValid(this.handle);
		}

		// Token: 0x0400054D RID: 1357
		public IntPtr handle;
	}
}

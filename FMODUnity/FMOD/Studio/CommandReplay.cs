using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000F5 RID: 245
	public struct CommandReplay
	{
		// Token: 0x0600061D RID: 1565 RVA: 0x00006DA0 File Offset: 0x00004FA0
		public RESULT getSystem(out System system)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetSystem(this.handle, out system.handle);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00006DB3 File Offset: 0x00004FB3
		public RESULT getLength(out float length)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetLength(this.handle, out length);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00006DC1 File Offset: 0x00004FC1
		public RESULT getCommandCount(out int count)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetCommandCount(this.handle, out count);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00006DCF File Offset: 0x00004FCF
		public RESULT getCommandInfo(int commandIndex, out COMMAND_INFO info)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetCommandInfo(this.handle, commandIndex, out info);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x00006DE0 File Offset: 0x00004FE0
		public RESULT getCommandString(int commandIndex, out string buffer)
		{
			buffer = null;
			RESULT result2;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				int num = 256;
				IntPtr intPtr = Marshal.AllocHGlobal(256);
				RESULT result;
				for (result = CommandReplay.FMOD_Studio_CommandReplay_GetCommandString(this.handle, commandIndex, intPtr, num); result == RESULT.ERR_TRUNCATED; result = CommandReplay.FMOD_Studio_CommandReplay_GetCommandString(this.handle, commandIndex, intPtr, num))
				{
					Marshal.FreeHGlobal(intPtr);
					num *= 2;
					intPtr = Marshal.AllocHGlobal(num);
				}
				if (result == RESULT.OK)
				{
					buffer = freeHelper.stringFromNative(intPtr);
				}
				Marshal.FreeHGlobal(intPtr);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00006E70 File Offset: 0x00005070
		public RESULT getCommandAtTime(float time, out int commandIndex)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetCommandAtTime(this.handle, time, out commandIndex);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00006E80 File Offset: 0x00005080
		public RESULT setBankPath(string bankPath)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = CommandReplay.FMOD_Studio_CommandReplay_SetBankPath(this.handle, freeHelper.byteFromStringUTF8(bankPath));
			}
			return result;
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00006EC4 File Offset: 0x000050C4
		public RESULT start()
		{
			return CommandReplay.FMOD_Studio_CommandReplay_Start(this.handle);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00006ED1 File Offset: 0x000050D1
		public RESULT stop()
		{
			return CommandReplay.FMOD_Studio_CommandReplay_Stop(this.handle);
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00006EDE File Offset: 0x000050DE
		public RESULT seekToTime(float time)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SeekToTime(this.handle, time);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00006EEC File Offset: 0x000050EC
		public RESULT seekToCommand(int commandIndex)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SeekToCommand(this.handle, commandIndex);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00006EFA File Offset: 0x000050FA
		public RESULT getPaused(out bool paused)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetPaused(this.handle, out paused);
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00006F08 File Offset: 0x00005108
		public RESULT setPaused(bool paused)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SetPaused(this.handle, paused);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00006F16 File Offset: 0x00005116
		public RESULT getPlaybackState(out PLAYBACK_STATE state)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetPlaybackState(this.handle, out state);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00006F24 File Offset: 0x00005124
		public RESULT getCurrentCommand(out int commandIndex, out float currentTime)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetCurrentCommand(this.handle, out commandIndex, out currentTime);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00006F33 File Offset: 0x00005133
		public RESULT release()
		{
			return CommandReplay.FMOD_Studio_CommandReplay_Release(this.handle);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00006F40 File Offset: 0x00005140
		public RESULT setFrameCallback(COMMANDREPLAY_FRAME_CALLBACK callback)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SetFrameCallback(this.handle, callback);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00006F4E File Offset: 0x0000514E
		public RESULT setLoadBankCallback(COMMANDREPLAY_LOAD_BANK_CALLBACK callback)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SetLoadBankCallback(this.handle, callback);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00006F5C File Offset: 0x0000515C
		public RESULT setCreateInstanceCallback(COMMANDREPLAY_CREATE_INSTANCE_CALLBACK callback)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SetCreateInstanceCallback(this.handle, callback);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00006F6A File Offset: 0x0000516A
		public RESULT getUserData(out IntPtr userdata)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_GetUserData(this.handle, out userdata);
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00006F78 File Offset: 0x00005178
		public RESULT setUserData(IntPtr userdata)
		{
			return CommandReplay.FMOD_Studio_CommandReplay_SetUserData(this.handle, userdata);
		}

		// Token: 0x06000632 RID: 1586
		[DllImport("fmodstudio")]
		private static extern bool FMOD_Studio_CommandReplay_IsValid(IntPtr replay);

		// Token: 0x06000633 RID: 1587
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetSystem(IntPtr replay, out IntPtr system);

		// Token: 0x06000634 RID: 1588
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetLength(IntPtr replay, out float length);

		// Token: 0x06000635 RID: 1589
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetCommandCount(IntPtr replay, out int count);

		// Token: 0x06000636 RID: 1590
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetCommandInfo(IntPtr replay, int commandindex, out COMMAND_INFO info);

		// Token: 0x06000637 RID: 1591
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetCommandString(IntPtr replay, int commandIndex, IntPtr buffer, int length);

		// Token: 0x06000638 RID: 1592
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetCommandAtTime(IntPtr replay, float time, out int commandIndex);

		// Token: 0x06000639 RID: 1593
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetBankPath(IntPtr replay, byte[] bankPath);

		// Token: 0x0600063A RID: 1594
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_Start(IntPtr replay);

		// Token: 0x0600063B RID: 1595
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_Stop(IntPtr replay);

		// Token: 0x0600063C RID: 1596
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SeekToTime(IntPtr replay, float time);

		// Token: 0x0600063D RID: 1597
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SeekToCommand(IntPtr replay, int commandIndex);

		// Token: 0x0600063E RID: 1598
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetPaused(IntPtr replay, out bool paused);

		// Token: 0x0600063F RID: 1599
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetPaused(IntPtr replay, bool paused);

		// Token: 0x06000640 RID: 1600
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetPlaybackState(IntPtr replay, out PLAYBACK_STATE state);

		// Token: 0x06000641 RID: 1601
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetCurrentCommand(IntPtr replay, out int commandIndex, out float currentTime);

		// Token: 0x06000642 RID: 1602
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_Release(IntPtr replay);

		// Token: 0x06000643 RID: 1603
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetFrameCallback(IntPtr replay, COMMANDREPLAY_FRAME_CALLBACK callback);

		// Token: 0x06000644 RID: 1604
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetLoadBankCallback(IntPtr replay, COMMANDREPLAY_LOAD_BANK_CALLBACK callback);

		// Token: 0x06000645 RID: 1605
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetCreateInstanceCallback(IntPtr replay, COMMANDREPLAY_CREATE_INSTANCE_CALLBACK callback);

		// Token: 0x06000646 RID: 1606
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_GetUserData(IntPtr replay, out IntPtr userdata);

		// Token: 0x06000647 RID: 1607
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD_Studio_CommandReplay_SetUserData(IntPtr replay, IntPtr userdata);

		// Token: 0x06000648 RID: 1608 RVA: 0x00006F86 File Offset: 0x00005186
		public CommandReplay(IntPtr ptr)
		{
			this.handle = ptr;
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00006F8F File Offset: 0x0000518F
		public bool hasHandle()
		{
			return this.handle != IntPtr.Zero;
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00006FA1 File Offset: 0x000051A1
		public void clearHandle()
		{
			this.handle = IntPtr.Zero;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00006FAE File Offset: 0x000051AE
		public bool isValid()
		{
			return this.hasHandle() && CommandReplay.FMOD_Studio_CommandReplay_IsValid(this.handle);
		}

		// Token: 0x0400054E RID: 1358
		public IntPtr handle;
	}
}

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000190 RID: 400
	public class ISteamMatchmakingPingResponse
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
		public ISteamMatchmakingPingResponse(ISteamMatchmakingPingResponse.ServerResponded onServerResponded, ISteamMatchmakingPingResponse.ServerFailedToRespond onServerFailedToRespond)
		{
			if (onServerResponded == null || onServerFailedToRespond == null)
			{
				throw new ArgumentNullException();
			}
			this.m_ServerResponded = onServerResponded;
			this.m_ServerFailedToRespond = onServerFailedToRespond;
			this.m_VTable = new ISteamMatchmakingPingResponse.VTable
			{
				m_VTServerResponded = new ISteamMatchmakingPingResponse.InternalServerResponded(this.InternalOnServerResponded),
				m_VTServerFailedToRespond = new ISteamMatchmakingPingResponse.InternalServerFailedToRespond(this.InternalOnServerFailedToRespond)
			};
			this.m_pVTable = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ISteamMatchmakingPingResponse.VTable)));
			Marshal.StructureToPtr<ISteamMatchmakingPingResponse.VTable>(this.m_VTable, this.m_pVTable, false);
			this.m_pGCHandle = GCHandle.Alloc(this.m_pVTable, GCHandleType.Pinned);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0000DA70 File Offset: 0x0000BC70
		~ISteamMatchmakingPingResponse()
		{
			if (this.m_pVTable != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(this.m_pVTable);
			}
			if (this.m_pGCHandle.IsAllocated)
			{
				this.m_pGCHandle.Free();
			}
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0000DACC File Offset: 0x0000BCCC
		private void InternalOnServerResponded(IntPtr thisptr, gameserveritem_t server)
		{
			this.m_ServerResponded(server);
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x0000DADA File Offset: 0x0000BCDA
		private void InternalOnServerFailedToRespond(IntPtr thisptr)
		{
			this.m_ServerFailedToRespond();
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x0000DAE7 File Offset: 0x0000BCE7
		public static explicit operator IntPtr(ISteamMatchmakingPingResponse that)
		{
			return that.m_pGCHandle.AddrOfPinnedObject();
		}

		// Token: 0x04000A7A RID: 2682
		private ISteamMatchmakingPingResponse.VTable m_VTable;

		// Token: 0x04000A7B RID: 2683
		private IntPtr m_pVTable;

		// Token: 0x04000A7C RID: 2684
		private GCHandle m_pGCHandle;

		// Token: 0x04000A7D RID: 2685
		private ISteamMatchmakingPingResponse.ServerResponded m_ServerResponded;

		// Token: 0x04000A7E RID: 2686
		private ISteamMatchmakingPingResponse.ServerFailedToRespond m_ServerFailedToRespond;

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000BE9 RID: 3049
		public delegate void ServerResponded(gameserveritem_t server);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000BED RID: 3053
		public delegate void ServerFailedToRespond();

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000BF1 RID: 3057
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerResponded(IntPtr thisptr, gameserveritem_t server);

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x06000BF5 RID: 3061
		[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
		private delegate void InternalServerFailedToRespond(IntPtr thisptr);

		// Token: 0x020001E7 RID: 487
		[StructLayout(LayoutKind.Sequential)]
		private class VTable
		{
			// Token: 0x04000B70 RID: 2928
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerResponded m_VTServerResponded;

			// Token: 0x04000B71 RID: 2929
			[NonSerialized]
			[MarshalAs(UnmanagedType.FunctionPtr)]
			public ISteamMatchmakingPingResponse.InternalServerFailedToRespond m_VTServerFailedToRespond;
		}
	}
}

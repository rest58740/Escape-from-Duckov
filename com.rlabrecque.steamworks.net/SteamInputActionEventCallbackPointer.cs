using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AA RID: 426
	// (Invoke) Token: 0x06000A36 RID: 2614
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SteamInputActionEventCallbackPointer(IntPtr SteamInputActionEvent);
}

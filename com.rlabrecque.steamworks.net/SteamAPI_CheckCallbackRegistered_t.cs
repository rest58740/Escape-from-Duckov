using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200019C RID: 412
	// (Invoke) Token: 0x06000991 RID: 2449
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate void SteamAPI_CheckCallbackRegistered_t(int iCallbackNum);
}

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x020001B6 RID: 438
	// (Invoke) Token: 0x06000A92 RID: 2706
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void FSteamNetworkingSocketsDebugOutput(ESteamNetworkingSocketsDebugOutputType nType, StringBuilder pszMsg);
}

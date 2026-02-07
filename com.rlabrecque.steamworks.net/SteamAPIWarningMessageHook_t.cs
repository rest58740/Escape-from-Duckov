using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Steamworks
{
	// Token: 0x0200019B RID: 411
	// (Invoke) Token: 0x0600098D RID: 2445
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SteamAPIWarningMessageHook_t(int nSeverity, StringBuilder pchDebugText);
}

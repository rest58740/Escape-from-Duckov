using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Threading
{
	// Token: 0x020002DC RID: 732
	// (Invoke) Token: 0x06002007 RID: 8199
	[ComVisible(true)]
	[SecurityCritical]
	[CLSCompliant(false)]
	public unsafe delegate void IOCompletionCallback(uint errorCode, uint numBytes, NativeOverlapped* pOVERLAP);
}

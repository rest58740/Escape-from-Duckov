using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000177 RID: 375
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InputDigitalActionData_t
	{
		// Token: 0x04000A00 RID: 2560
		public byte bState;

		// Token: 0x04000A01 RID: 2561
		public byte bActive;
	}
}

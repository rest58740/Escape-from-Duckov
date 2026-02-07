using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000176 RID: 374
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InputAnalogActionData_t
	{
		// Token: 0x040009FC RID: 2556
		public EInputSourceMode eMode;

		// Token: 0x040009FD RID: 2557
		public float x;

		// Token: 0x040009FE RID: 2558
		public float y;

		// Token: 0x040009FF RID: 2559
		public byte bActive;
	}
}

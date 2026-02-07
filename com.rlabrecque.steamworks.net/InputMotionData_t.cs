using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000178 RID: 376
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct InputMotionData_t
	{
		// Token: 0x04000A02 RID: 2562
		public float rotQuatX;

		// Token: 0x04000A03 RID: 2563
		public float rotQuatY;

		// Token: 0x04000A04 RID: 2564
		public float rotQuatZ;

		// Token: 0x04000A05 RID: 2565
		public float rotQuatW;

		// Token: 0x04000A06 RID: 2566
		public float posAccelX;

		// Token: 0x04000A07 RID: 2567
		public float posAccelY;

		// Token: 0x04000A08 RID: 2568
		public float posAccelZ;

		// Token: 0x04000A09 RID: 2569
		public float rotVelX;

		// Token: 0x04000A0A RID: 2570
		public float rotVelY;

		// Token: 0x04000A0B RID: 2571
		public float rotVelZ;
	}
}

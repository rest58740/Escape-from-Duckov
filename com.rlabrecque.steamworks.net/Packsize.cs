using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000193 RID: 403
	public static class Packsize
	{
		// Token: 0x06000928 RID: 2344 RVA: 0x0000DDA4 File Offset: 0x0000BFA4
		public static bool Test()
		{
			int num = Marshal.SizeOf(typeof(Packsize.ValvePackingSentinel_t));
			int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));
			return num == 32 && num2 == 616;
		}

		// Token: 0x04000A8B RID: 2699
		public const int value = 8;

		// Token: 0x020001F6 RID: 502
		[StructLayout(LayoutKind.Sequential, Pack = 8)]
		private struct ValvePackingSentinel_t
		{
			// Token: 0x04000B78 RID: 2936
			private uint m_u32;

			// Token: 0x04000B79 RID: 2937
			private ulong m_u64;

			// Token: 0x04000B7A RID: 2938
			private ushort m_u16;

			// Token: 0x04000B7B RID: 2939
			private double m_d;
		}
	}
}

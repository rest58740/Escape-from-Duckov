using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000046 RID: 70
	public struct Factory
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00002CA2 File Offset: 0x00000EA2
		public static RESULT System_Create(out System system)
		{
			return Factory.FMOD5_System_Create(out system.handle, 131848U);
		}

		// Token: 0x06000084 RID: 132
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_System_Create(out IntPtr system, uint headerversion);
	}
}

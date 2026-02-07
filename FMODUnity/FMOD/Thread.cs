using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000049 RID: 73
	public struct Thread
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00002D10 File Offset: 0x00000F10
		public static RESULT SetAttributes(THREAD_TYPE type, THREAD_AFFINITY affinity = THREAD_AFFINITY.GROUP_DEFAULT, THREAD_PRIORITY priority = THREAD_PRIORITY.DEFAULT, THREAD_STACK_SIZE stacksize = THREAD_STACK_SIZE.DEFAULT)
		{
			return Thread.FMOD5_Thread_SetAttributes(type, affinity, priority, stacksize);
		}

		// Token: 0x0600008C RID: 140
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Thread_SetAttributes(THREAD_TYPE type, THREAD_AFFINITY affinity, THREAD_PRIORITY priority, THREAD_STACK_SIZE stacksize);
	}
}

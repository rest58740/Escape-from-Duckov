using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000047 RID: 71
	public struct Memory
	{
		// Token: 0x06000085 RID: 133 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public static RESULT Initialize(IntPtr poolmem, int poollen, MEMORY_ALLOC_CALLBACK useralloc, MEMORY_REALLOC_CALLBACK userrealloc, MEMORY_FREE_CALLBACK userfree, MEMORY_TYPE memtypeflags = MEMORY_TYPE.ALL)
		{
			return Memory.FMOD5_Memory_Initialize(poolmem, poollen, useralloc, userrealloc, userfree, memtypeflags);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00002CC3 File Offset: 0x00000EC3
		public static RESULT GetStats(out int currentalloced, out int maxalloced, bool blocking = true)
		{
			return Memory.FMOD5_Memory_GetStats(out currentalloced, out maxalloced, blocking);
		}

		// Token: 0x06000087 RID: 135
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Memory_Initialize(IntPtr poolmem, int poollen, MEMORY_ALLOC_CALLBACK useralloc, MEMORY_REALLOC_CALLBACK userrealloc, MEMORY_FREE_CALLBACK userfree, MEMORY_TYPE memtypeflags);

		// Token: 0x06000088 RID: 136
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Memory_GetStats(out int currentalloced, out int maxalloced, bool blocking);
	}
}

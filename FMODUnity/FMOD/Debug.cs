using System;
using System.Runtime.InteropServices;

namespace FMOD
{
	// Token: 0x02000048 RID: 72
	public struct Debug
	{
		// Token: 0x06000089 RID: 137 RVA: 0x00002CD0 File Offset: 0x00000ED0
		public static RESULT Initialize(DEBUG_FLAGS flags, DEBUG_MODE mode = DEBUG_MODE.TTY, DEBUG_CALLBACK callback = null, string filename = null)
		{
			RESULT result;
			using (StringHelper.ThreadSafeEncoding freeHelper = StringHelper.GetFreeHelper())
			{
				result = Debug.FMOD5_Debug_Initialize(flags, mode, callback, freeHelper.byteFromStringUTF8(filename));
			}
			return result;
		}

		// Token: 0x0600008A RID: 138
		[DllImport("fmodstudio")]
		private static extern RESULT FMOD5_Debug_Initialize(DEBUG_FLAGS flags, DEBUG_MODE mode, DEBUG_CALLBACK callback, byte[] filename);
	}
}

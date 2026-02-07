using System;

namespace FMOD
{
	// Token: 0x0200002C RID: 44
	// (Invoke) Token: 0x06000034 RID: 52
	public delegate RESULT FILE_READ_CALLBACK(IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
}

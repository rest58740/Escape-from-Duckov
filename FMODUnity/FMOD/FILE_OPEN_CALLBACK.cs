using System;

namespace FMOD
{
	// Token: 0x0200002A RID: 42
	// (Invoke) Token: 0x0600002C RID: 44
	public delegate RESULT FILE_OPEN_CALLBACK(IntPtr name, ref uint filesize, ref IntPtr handle, IntPtr userdata);
}

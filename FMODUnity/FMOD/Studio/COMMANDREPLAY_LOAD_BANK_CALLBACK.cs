using System;

namespace FMOD.Studio
{
	// Token: 0x020000E9 RID: 233
	// (Invoke) Token: 0x060004A5 RID: 1189
	public delegate RESULT COMMANDREPLAY_LOAD_BANK_CALLBACK(IntPtr replay, int commandindex, GUID bankguid, IntPtr bankfilename, LOAD_BANK_FLAGS flags, out IntPtr bank, IntPtr userdata);
}

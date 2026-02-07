using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000FC RID: 252
	[CallbackIdentity(714)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GamepadTextInputDismissed_t
	{
		// Token: 0x04000315 RID: 789
		public const int k_iCallback = 714;

		// Token: 0x04000316 RID: 790
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSubmitted;

		// Token: 0x04000317 RID: 791
		public uint m_unSubmittedText;

		// Token: 0x04000318 RID: 792
		public AppId_t m_unAppID;
	}
}

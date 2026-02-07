using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020000C7 RID: 199
	[CallbackIdentity(6001)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SteamTimelineGamePhaseRecordingExists_t
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0000C89B File Offset: 0x0000AA9B
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0000C8A8 File Offset: 0x0000AAA8
		public string m_rgchPhaseID
		{
			get
			{
				return InteropHelp.ByteArrayToStringUTF8(this.m_rgchPhaseID_);
			}
			set
			{
				InteropHelp.StringToByteArrayUTF8(value, this.m_rgchPhaseID_, 64);
			}
		}

		// Token: 0x04000251 RID: 593
		public const int k_iCallback = 6001;

		// Token: 0x04000252 RID: 594
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
		private byte[] m_rgchPhaseID_;

		// Token: 0x04000253 RID: 595
		public ulong m_ulRecordingMS;

		// Token: 0x04000254 RID: 596
		public ulong m_ulLongestClipMS;

		// Token: 0x04000255 RID: 597
		public uint m_unClipCount;

		// Token: 0x04000256 RID: 598
		public uint m_unScreenshotCount;
	}
}

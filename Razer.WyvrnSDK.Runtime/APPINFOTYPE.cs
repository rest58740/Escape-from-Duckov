using System;
using System.Runtime.InteropServices;

namespace WyvrnSDK
{
	// Token: 0x02000004 RID: 4
	[StructLayout(0, CharSet = 3)]
	public struct APPINFOTYPE
	{
		// Token: 0x04000014 RID: 20
		[MarshalAs(23, SizeConst = 256)]
		public string Title;

		// Token: 0x04000015 RID: 21
		[MarshalAs(23, SizeConst = 1024)]
		public string Description;

		// Token: 0x04000016 RID: 22
		[MarshalAs(23, SizeConst = 256)]
		public string Author_Name;

		// Token: 0x04000017 RID: 23
		[MarshalAs(23, SizeConst = 256)]
		public string Author_Contact;

		// Token: 0x04000018 RID: 24
		public uint SupportedDevice;

		// Token: 0x04000019 RID: 25
		public uint Category;
	}
}

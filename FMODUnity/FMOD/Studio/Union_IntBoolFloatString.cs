using System;
using System.Runtime.InteropServices;

namespace FMOD.Studio
{
	// Token: 0x020000DE RID: 222
	[StructLayout(LayoutKind.Explicit)]
	internal struct Union_IntBoolFloatString
	{
		// Token: 0x040004F4 RID: 1268
		[FieldOffset(0)]
		public int intvalue;

		// Token: 0x040004F5 RID: 1269
		[FieldOffset(0)]
		public bool boolvalue;

		// Token: 0x040004F6 RID: 1270
		[FieldOffset(0)]
		public float floatvalue;

		// Token: 0x040004F7 RID: 1271
		[FieldOffset(0)]
		public StringWrapper stringvalue;
	}
}

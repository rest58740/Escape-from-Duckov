using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200075B RID: 1883
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete]
	[Guid("00020404-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIEnumVARIANT
	{
		// Token: 0x06004266 RID: 16998
		[PreserveSig]
		int Next(int celt, int rgvar, int pceltFetched);

		// Token: 0x06004267 RID: 16999
		[PreserveSig]
		int Skip(int celt);

		// Token: 0x06004268 RID: 17000
		[PreserveSig]
		int Reset();

		// Token: 0x06004269 RID: 17001
		void Clone(int ppenum);
	}
}

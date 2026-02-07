using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020006D9 RID: 1753
	public interface ICustomMarshaler
	{
		// Token: 0x06004034 RID: 16436
		object MarshalNativeToManaged(IntPtr pNativeData);

		// Token: 0x06004035 RID: 16437
		IntPtr MarshalManagedToNative(object ManagedObj);

		// Token: 0x06004036 RID: 16438
		void CleanUpNativeData(IntPtr pNativeData);

		// Token: 0x06004037 RID: 16439
		void CleanUpManagedData(object ManagedObj);

		// Token: 0x06004038 RID: 16440
		int GetNativeDataSize();
	}
}

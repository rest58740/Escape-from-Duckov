using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000772 RID: 1906
	[CLSCompliant(false)]
	[Guid("f7102fa9-cabb-3a74-a6da-b4567ef1b079")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	[TypeLibImportClass(typeof(MemberInfo))]
	public interface _MemberInfo
	{
		// Token: 0x0600436B RID: 17259
		bool Equals(object other);

		// Token: 0x0600436C RID: 17260
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x0600436D RID: 17261
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x0600436E RID: 17262
		int GetHashCode();

		// Token: 0x0600436F RID: 17263
		Type GetType();

		// Token: 0x06004370 RID: 17264
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06004371 RID: 17265
		string ToString();

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06004372 RID: 17266
		Type DeclaringType { get; }

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06004373 RID: 17267
		MemberTypes MemberType { get; }

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06004374 RID: 17268
		string Name { get; }

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06004375 RID: 17269
		Type ReflectedType { get; }

		// Token: 0x06004376 RID: 17270
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004377 RID: 17271
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004378 RID: 17272
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004379 RID: 17273
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);
	}
}

using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076C RID: 1900
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[TypeLibImportClass(typeof(EventInfo))]
	[Guid("9DE59C64-D889-35A1-B897-587D74469E5B")]
	[ComVisible(true)]
	public interface _EventInfo
	{
		// Token: 0x06004313 RID: 17171
		void AddEventHandler(object target, Delegate handler);

		// Token: 0x06004314 RID: 17172
		bool Equals(object other);

		// Token: 0x06004315 RID: 17173
		MethodInfo GetAddMethod();

		// Token: 0x06004316 RID: 17174
		MethodInfo GetAddMethod(bool nonPublic);

		// Token: 0x06004317 RID: 17175
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06004318 RID: 17176
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06004319 RID: 17177
		int GetHashCode();

		// Token: 0x0600431A RID: 17178
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x0600431B RID: 17179
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x0600431C RID: 17180
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x0600431D RID: 17181
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x0600431E RID: 17182
		MethodInfo GetRaiseMethod();

		// Token: 0x0600431F RID: 17183
		MethodInfo GetRaiseMethod(bool nonPublic);

		// Token: 0x06004320 RID: 17184
		MethodInfo GetRemoveMethod();

		// Token: 0x06004321 RID: 17185
		MethodInfo GetRemoveMethod(bool nonPublic);

		// Token: 0x06004322 RID: 17186
		Type GetType();

		// Token: 0x06004323 RID: 17187
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06004324 RID: 17188
		void RemoveEventHandler(object target, Delegate handler);

		// Token: 0x06004325 RID: 17189
		string ToString();

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06004326 RID: 17190
		EventAttributes Attributes { get; }

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06004327 RID: 17191
		Type DeclaringType { get; }

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06004328 RID: 17192
		Type EventHandlerType { get; }

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06004329 RID: 17193
		bool IsMulticast { get; }

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x0600432A RID: 17194
		bool IsSpecialName { get; }

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x0600432B RID: 17195
		MemberTypes MemberType { get; }

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x0600432C RID: 17196
		string Name { get; }

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x0600432D RID: 17197
		Type ReflectedType { get; }
	}
}

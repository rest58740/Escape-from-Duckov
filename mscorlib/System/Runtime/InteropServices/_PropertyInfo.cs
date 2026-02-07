using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200077C RID: 1916
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[CLSCompliant(false)]
	[Guid("F59ED4E4-E68F-3218-BD77-061AA82824BF")]
	[TypeLibImportClass(typeof(PropertyInfo))]
	[ComVisible(true)]
	public interface _PropertyInfo
	{
		// Token: 0x060043DF RID: 17375
		bool Equals(object other);

		// Token: 0x060043E0 RID: 17376
		MethodInfo[] GetAccessors();

		// Token: 0x060043E1 RID: 17377
		MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x060043E2 RID: 17378
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x060043E3 RID: 17379
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x060043E4 RID: 17380
		MethodInfo GetGetMethod();

		// Token: 0x060043E5 RID: 17381
		MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x060043E6 RID: 17382
		int GetHashCode();

		// Token: 0x060043E7 RID: 17383
		ParameterInfo[] GetIndexParameters();

		// Token: 0x060043E8 RID: 17384
		MethodInfo GetSetMethod();

		// Token: 0x060043E9 RID: 17385
		MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x060043EA RID: 17386
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043EB RID: 17387
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043EC RID: 17388
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043ED RID: 17389
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x060043EE RID: 17390
		Type GetType();

		// Token: 0x060043EF RID: 17391
		object GetValue(object obj, object[] index);

		// Token: 0x060043F0 RID: 17392
		object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x060043F1 RID: 17393
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x060043F2 RID: 17394
		void SetValue(object obj, object value, object[] index);

		// Token: 0x060043F3 RID: 17395
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x060043F4 RID: 17396
		string ToString();

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060043F5 RID: 17397
		PropertyAttributes Attributes { get; }

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060043F6 RID: 17398
		bool CanRead { get; }

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060043F7 RID: 17399
		bool CanWrite { get; }

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060043F8 RID: 17400
		Type DeclaringType { get; }

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060043F9 RID: 17401
		bool IsSpecialName { get; }

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060043FA RID: 17402
		MemberTypes MemberType { get; }

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060043FB RID: 17403
		string Name { get; }

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060043FC RID: 17404
		Type PropertyType { get; }

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060043FD RID: 17405
		Type ReflectedType { get; }
	}
}

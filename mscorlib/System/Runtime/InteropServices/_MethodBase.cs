using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000773 RID: 1907
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[TypeLibImportClass(typeof(MethodBase))]
	[ComVisible(true)]
	[Guid("6240837A-707F-3181-8E98-A36AE086766B")]
	[CLSCompliant(false)]
	public interface _MethodBase
	{
		// Token: 0x0600437A RID: 17274
		bool Equals(object other);

		// Token: 0x0600437B RID: 17275
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x0600437C RID: 17276
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x0600437D RID: 17277
		int GetHashCode();

		// Token: 0x0600437E RID: 17278
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x0600437F RID: 17279
		ParameterInfo[] GetParameters();

		// Token: 0x06004380 RID: 17280
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004381 RID: 17281
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004382 RID: 17282
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004383 RID: 17283
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06004384 RID: 17284
		Type GetType();

		// Token: 0x06004385 RID: 17285
		object Invoke(object obj, object[] parameters);

		// Token: 0x06004386 RID: 17286
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06004387 RID: 17287
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06004388 RID: 17288
		string ToString();

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06004389 RID: 17289
		MethodAttributes Attributes { get; }

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x0600438A RID: 17290
		CallingConventions CallingConvention { get; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x0600438B RID: 17291
		Type DeclaringType { get; }

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600438C RID: 17292
		bool IsAbstract { get; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x0600438D RID: 17293
		bool IsAssembly { get; }

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x0600438E RID: 17294
		bool IsConstructor { get; }

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x0600438F RID: 17295
		bool IsFamily { get; }

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06004390 RID: 17296
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06004391 RID: 17297
		bool IsFamilyOrAssembly { get; }

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06004392 RID: 17298
		bool IsFinal { get; }

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06004393 RID: 17299
		bool IsHideBySig { get; }

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06004394 RID: 17300
		bool IsPrivate { get; }

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06004395 RID: 17301
		bool IsPublic { get; }

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06004396 RID: 17302
		bool IsSpecialName { get; }

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06004397 RID: 17303
		bool IsStatic { get; }

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06004398 RID: 17304
		bool IsVirtual { get; }

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06004399 RID: 17305
		MemberTypes MemberType { get; }

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x0600439A RID: 17306
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x0600439B RID: 17307
		string Name { get; }

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x0600439C RID: 17308
		Type ReflectedType { get; }
	}
}

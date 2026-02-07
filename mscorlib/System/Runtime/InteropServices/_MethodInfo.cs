using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000775 RID: 1909
	[TypeLibImportClass(typeof(MethodInfo))]
	[CLSCompliant(false)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FFCC1B5D-ECB8-38DD-9B01-3DC8ABC2AA5F")]
	[ComVisible(true)]
	public interface _MethodInfo
	{
		// Token: 0x060043A1 RID: 17313
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060043A2 RID: 17314
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060043A3 RID: 17315
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060043A4 RID: 17316
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x060043A5 RID: 17317
		string ToString();

		// Token: 0x060043A6 RID: 17318
		bool Equals(object other);

		// Token: 0x060043A7 RID: 17319
		int GetHashCode();

		// Token: 0x060043A8 RID: 17320
		Type GetType();

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060043A9 RID: 17321
		MemberTypes MemberType { get; }

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060043AA RID: 17322
		string Name { get; }

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060043AB RID: 17323
		Type DeclaringType { get; }

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060043AC RID: 17324
		Type ReflectedType { get; }

		// Token: 0x060043AD RID: 17325
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x060043AE RID: 17326
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x060043AF RID: 17327
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x060043B0 RID: 17328
		ParameterInfo[] GetParameters();

		// Token: 0x060043B1 RID: 17329
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x060043B2 RID: 17330
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060043B3 RID: 17331
		MethodAttributes Attributes { get; }

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x060043B4 RID: 17332
		CallingConventions CallingConvention { get; }

		// Token: 0x060043B5 RID: 17333
		object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x060043B6 RID: 17334
		bool IsPublic { get; }

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x060043B7 RID: 17335
		bool IsPrivate { get; }

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x060043B8 RID: 17336
		bool IsFamily { get; }

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x060043B9 RID: 17337
		bool IsAssembly { get; }

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x060043BA RID: 17338
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x060043BB RID: 17339
		bool IsFamilyOrAssembly { get; }

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x060043BC RID: 17340
		bool IsStatic { get; }

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x060043BD RID: 17341
		bool IsFinal { get; }

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060043BE RID: 17342
		bool IsVirtual { get; }

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060043BF RID: 17343
		bool IsHideBySig { get; }

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060043C0 RID: 17344
		bool IsAbstract { get; }

		// Token: 0x17000A63 RID: 2659
		// (get) Token: 0x060043C1 RID: 17345
		bool IsSpecialName { get; }

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x060043C2 RID: 17346
		bool IsConstructor { get; }

		// Token: 0x060043C3 RID: 17347
		object Invoke(object obj, object[] parameters);

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060043C4 RID: 17348
		Type ReturnType { get; }

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060043C5 RID: 17349
		ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x060043C6 RID: 17350
		MethodInfo GetBaseDefinition();
	}
}

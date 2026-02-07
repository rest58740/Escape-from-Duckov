using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000768 RID: 1896
	[CLSCompliant(false)]
	[Guid("E9A19478-9646-3679-9B10-8411AE1FD57D")]
	[TypeLibImportClass(typeof(ConstructorInfo))]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface _ConstructorInfo
	{
		// Token: 0x060042E2 RID: 17122
		bool Equals(object other);

		// Token: 0x060042E3 RID: 17123
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x060042E4 RID: 17124
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x060042E5 RID: 17125
		int GetHashCode();

		// Token: 0x060042E6 RID: 17126
		MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x060042E7 RID: 17127
		ParameterInfo[] GetParameters();

		// Token: 0x060042E8 RID: 17128
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x060042E9 RID: 17129
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x060042EA RID: 17130
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x060042EB RID: 17131
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x060042EC RID: 17132
		Type GetType();

		// Token: 0x060042ED RID: 17133
		object Invoke_5(object[] parameters);

		// Token: 0x060042EE RID: 17134
		object Invoke_3(object obj, object[] parameters);

		// Token: 0x060042EF RID: 17135
		object Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x060042F0 RID: 17136
		object Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x060042F1 RID: 17137
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x060042F2 RID: 17138
		string ToString();

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060042F3 RID: 17139
		MethodAttributes Attributes { get; }

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060042F4 RID: 17140
		CallingConventions CallingConvention { get; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060042F5 RID: 17141
		Type DeclaringType { get; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060042F6 RID: 17142
		bool IsAbstract { get; }

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060042F7 RID: 17143
		bool IsAssembly { get; }

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060042F8 RID: 17144
		bool IsConstructor { get; }

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060042F9 RID: 17145
		bool IsFamily { get; }

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060042FA RID: 17146
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060042FB RID: 17147
		bool IsFamilyOrAssembly { get; }

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060042FC RID: 17148
		bool IsFinal { get; }

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060042FD RID: 17149
		bool IsHideBySig { get; }

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060042FE RID: 17150
		bool IsPrivate { get; }

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060042FF RID: 17151
		bool IsPublic { get; }

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06004300 RID: 17152
		bool IsSpecialName { get; }

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06004301 RID: 17153
		bool IsStatic { get; }

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06004302 RID: 17154
		bool IsVirtual { get; }

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06004303 RID: 17155
		MemberTypes MemberType { get; }

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06004304 RID: 17156
		RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06004305 RID: 17157
		string Name { get; }

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06004306 RID: 17158
		Type ReflectedType { get; }
	}
}

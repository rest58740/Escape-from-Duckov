using System;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200076F RID: 1903
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8A7C1442-A9FB-366B-80D8-4939FFA6DBE0")]
	[TypeLibImportClass(typeof(FieldInfo))]
	[ComVisible(true)]
	[CLSCompliant(false)]
	public interface _FieldInfo
	{
		// Token: 0x06004340 RID: 17216
		bool Equals(object other);

		// Token: 0x06004341 RID: 17217
		object[] GetCustomAttributes(bool inherit);

		// Token: 0x06004342 RID: 17218
		object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x06004343 RID: 17219
		int GetHashCode();

		// Token: 0x06004344 RID: 17220
		Type GetType();

		// Token: 0x06004345 RID: 17221
		void GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId);

		// Token: 0x06004346 RID: 17222
		void GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo);

		// Token: 0x06004347 RID: 17223
		void GetTypeInfoCount(out uint pcTInfo);

		// Token: 0x06004348 RID: 17224
		void Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr);

		// Token: 0x06004349 RID: 17225
		object GetValue(object obj);

		// Token: 0x0600434A RID: 17226
		object GetValueDirect(TypedReference obj);

		// Token: 0x0600434B RID: 17227
		bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x0600434C RID: 17228
		void SetValue(object obj, object value);

		// Token: 0x0600434D RID: 17229
		void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x0600434E RID: 17230
		void SetValueDirect(TypedReference obj, object value);

		// Token: 0x0600434F RID: 17231
		string ToString();

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06004350 RID: 17232
		FieldAttributes Attributes { get; }

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06004351 RID: 17233
		Type DeclaringType { get; }

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004352 RID: 17234
		RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004353 RID: 17235
		Type FieldType { get; }

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06004354 RID: 17236
		bool IsAssembly { get; }

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004355 RID: 17237
		bool IsFamily { get; }

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06004356 RID: 17238
		bool IsFamilyAndAssembly { get; }

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06004357 RID: 17239
		bool IsFamilyOrAssembly { get; }

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004358 RID: 17240
		bool IsInitOnly { get; }

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06004359 RID: 17241
		bool IsLiteral { get; }

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x0600435A RID: 17242
		bool IsNotSerialized { get; }

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x0600435B RID: 17243
		bool IsPinvokeImpl { get; }

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x0600435C RID: 17244
		bool IsPrivate { get; }

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600435D RID: 17245
		bool IsPublic { get; }

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x0600435E RID: 17246
		bool IsSpecialName { get; }

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600435F RID: 17247
		bool IsStatic { get; }

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06004360 RID: 17248
		MemberTypes MemberType { get; }

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06004361 RID: 17249
		string Name { get; }

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06004362 RID: 17250
		Type ReflectedType { get; }
	}
}

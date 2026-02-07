using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000920 RID: 2336
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_EnumBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	public sealed class EnumBuilder : TypeInfo, _EnumBuilder
	{
		// Token: 0x06004FA5 RID: 20389 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EnumBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EnumBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EnumBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x000479FC File Offset: 0x00045BFC
		void _EnumBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x000FA308 File Offset: 0x000F8508
		internal EnumBuilder(ModuleBuilder mb, string name, TypeAttributes visibility, Type underlyingType)
		{
			this._tb = new TypeBuilder(mb, name, visibility | TypeAttributes.Sealed, typeof(Enum), null, PackingSize.Unspecified, 0, null);
			this._underlyingType = underlyingType;
			this._underlyingField = this._tb.DefineField("value__", underlyingType, FieldAttributes.Private | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
			this.setup_enum_type(this._tb);
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x000FA36E File Offset: 0x000F856E
		internal TypeBuilder GetTypeBuilder()
		{
			return this._tb;
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x000FA376 File Offset: 0x000F8576
		internal override Type InternalResolve()
		{
			return this._tb.InternalResolve();
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x000FA383 File Offset: 0x000F8583
		internal override Type RuntimeResolve()
		{
			return this._tb.RuntimeResolve();
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x000FA390 File Offset: 0x000F8590
		public override Assembly Assembly
		{
			get
			{
				return this._tb.Assembly;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004FAE RID: 20398 RVA: 0x000FA39D File Offset: 0x000F859D
		public override string AssemblyQualifiedName
		{
			get
			{
				return this._tb.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06004FAF RID: 20399 RVA: 0x000FA3AA File Offset: 0x000F85AA
		public override Type BaseType
		{
			get
			{
				return this._tb.BaseType;
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004FB0 RID: 20400 RVA: 0x000FA3B7 File Offset: 0x000F85B7
		public override Type DeclaringType
		{
			get
			{
				return this._tb.DeclaringType;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06004FB1 RID: 20401 RVA: 0x000FA3C4 File Offset: 0x000F85C4
		public override string FullName
		{
			get
			{
				return this._tb.FullName;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06004FB2 RID: 20402 RVA: 0x000FA3D1 File Offset: 0x000F85D1
		public override Guid GUID
		{
			get
			{
				return this._tb.GUID;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06004FB3 RID: 20403 RVA: 0x000FA3DE File Offset: 0x000F85DE
		public override Module Module
		{
			get
			{
				return this._tb.Module;
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06004FB4 RID: 20404 RVA: 0x000FA3EB File Offset: 0x000F85EB
		public override string Name
		{
			get
			{
				return this._tb.Name;
			}
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x000FA3F8 File Offset: 0x000F85F8
		public override string Namespace
		{
			get
			{
				return this._tb.Namespace;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x06004FB6 RID: 20406 RVA: 0x000FA405 File Offset: 0x000F8605
		public override Type ReflectedType
		{
			get
			{
				return this._tb.ReflectedType;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06004FB7 RID: 20407 RVA: 0x000FA412 File Offset: 0x000F8612
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this._tb.TypeHandle;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06004FB8 RID: 20408 RVA: 0x000FA41F File Offset: 0x000F861F
		public TypeToken TypeToken
		{
			get
			{
				return this._tb.TypeToken;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x000FA42C File Offset: 0x000F862C
		public FieldBuilder UnderlyingField
		{
			get
			{
				return this._underlyingField;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06004FBA RID: 20410 RVA: 0x000FA434 File Offset: 0x000F8634
		public override Type UnderlyingSystemType
		{
			get
			{
				return this._underlyingType;
			}
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x000FA43C File Offset: 0x000F863C
		public Type CreateType()
		{
			return this._tb.CreateType();
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x000FA449 File Offset: 0x000F8649
		public TypeInfo CreateTypeInfo()
		{
			return this._tb.CreateTypeInfo();
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x000FA434 File Offset: 0x000F8634
		public override Type GetEnumUnderlyingType()
		{
			return this._underlyingType;
		}

		// Token: 0x06004FBE RID: 20414
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void setup_enum_type(Type t);

		// Token: 0x06004FBF RID: 20415 RVA: 0x000FA458 File Offset: 0x000F8658
		public FieldBuilder DefineLiteral(string literalName, object literalValue)
		{
			FieldBuilder fieldBuilder = this._tb.DefineField(literalName, this, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.Literal);
			fieldBuilder.SetConstant(literalValue);
			return fieldBuilder;
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x000FA47D File Offset: 0x000F867D
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this._tb.attrs;
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x000FA48A File Offset: 0x000F868A
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this._tb.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004FC2 RID: 20418 RVA: 0x000FA49E File Offset: 0x000F869E
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this._tb.GetConstructors(bindingAttr);
		}

		// Token: 0x06004FC3 RID: 20419 RVA: 0x000FA4AC File Offset: 0x000F86AC
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this._tb.GetCustomAttributes(inherit);
		}

		// Token: 0x06004FC4 RID: 20420 RVA: 0x000FA4BA File Offset: 0x000F86BA
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this._tb.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004FC5 RID: 20421 RVA: 0x000FA4C9 File Offset: 0x000F86C9
		public override Type GetElementType()
		{
			return this._tb.GetElementType();
		}

		// Token: 0x06004FC6 RID: 20422 RVA: 0x000FA4D6 File Offset: 0x000F86D6
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this._tb.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004FC7 RID: 20423 RVA: 0x000FA4E5 File Offset: 0x000F86E5
		public override EventInfo[] GetEvents()
		{
			return this._tb.GetEvents();
		}

		// Token: 0x06004FC8 RID: 20424 RVA: 0x000FA4F2 File Offset: 0x000F86F2
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this._tb.GetEvents(bindingAttr);
		}

		// Token: 0x06004FC9 RID: 20425 RVA: 0x000FA500 File Offset: 0x000F8700
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this._tb.GetField(name, bindingAttr);
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x000FA50F File Offset: 0x000F870F
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this._tb.GetFields(bindingAttr);
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x000FA51D File Offset: 0x000F871D
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this._tb.GetInterface(name, ignoreCase);
		}

		// Token: 0x06004FCC RID: 20428 RVA: 0x000FA52C File Offset: 0x000F872C
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this._tb.GetInterfaceMap(interfaceType);
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x000FA53A File Offset: 0x000F873A
		public override Type[] GetInterfaces()
		{
			return this._tb.GetInterfaces();
		}

		// Token: 0x06004FCE RID: 20430 RVA: 0x000FA547 File Offset: 0x000F8747
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this._tb.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06004FCF RID: 20431 RVA: 0x000FA557 File Offset: 0x000F8757
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this._tb.GetMembers(bindingAttr);
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x000FA565 File Offset: 0x000F8765
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				return this._tb.GetMethod(name, bindingAttr);
			}
			return this._tb.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x000FA58D File Offset: 0x000F878D
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this._tb.GetMethods(bindingAttr);
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x000FA59B File Offset: 0x000F879B
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this._tb.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x000FA5AA File Offset: 0x000F87AA
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this._tb.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x000FA5B8 File Offset: 0x000F87B8
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this._tb.GetProperties(bindingAttr);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x000FA5C6 File Offset: 0x000F87C6
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.CreateNotSupportedException();
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x000FA5CE File Offset: 0x000F87CE
		protected override bool HasElementTypeImpl()
		{
			return this._tb.HasElementType;
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x000FA5DC File Offset: 0x000F87DC
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this._tb.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x06004FD8 RID: 20440 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06004FD9 RID: 20441 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06004FDB RID: 20443 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06004FDC RID: 20444 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x000040F7 File Offset: 0x000022F7
		protected override bool IsValueTypeImpl()
		{
			return true;
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x000FA601 File Offset: 0x000F8801
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this._tb.IsDefined(attributeType, inherit);
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x000F6535 File Offset: 0x000F4735
		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x000F653E File Offset: 0x000F473E
		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x000F6551 File Offset: 0x000F4751
		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x000F6559 File Offset: 0x000F4759
		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x000FA610 File Offset: 0x000F8810
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this._tb.SetCustomAttribute(customBuilder);
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x000FA61E File Offset: 0x000F881E
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception CreateNotSupportedException()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06004FE6 RID: 20454 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override bool IsUserType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsConstructedGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x000FA62D File Offset: 0x000F882D
		public override bool IsAssignableFrom(TypeInfo typeInfo)
		{
			return base.IsAssignableFrom(typeInfo);
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x06004FE9 RID: 20457 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool IsTypeDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x000173AD File Offset: 0x000155AD
		internal EnumBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400314B RID: 12619
		private TypeBuilder _tb;

		// Token: 0x0400314C RID: 12620
		private FieldBuilder _underlyingField;

		// Token: 0x0400314D RID: 12621
		private Type _underlyingType;
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000924 RID: 2340
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_FieldBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class FieldBuilder : FieldInfo, _FieldBuilder
	{
		// Token: 0x0600500F RID: 20495 RVA: 0x000479FC File Offset: 0x00045BFC
		void _FieldBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x000479FC File Offset: 0x00045BFC
		void _FieldBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x000479FC File Offset: 0x00045BFC
		void _FieldBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x000479FC File Offset: 0x00045BFC
		void _FieldBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x000FAAD8 File Offset: 0x000F8CD8
		internal FieldBuilder(TypeBuilder tb, string fieldName, Type type, FieldAttributes attributes, Type[] modReq, Type[] modOpt)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.attrs = attributes;
			this.name = fieldName;
			this.type = type;
			this.modReq = modReq;
			this.modOpt = modOpt;
			this.offset = -1;
			this.typeb = tb;
			((ModuleBuilder)tb.Module).RegisterToken(this, this.GetToken().Token);
		}

		// Token: 0x17000D26 RID: 3366
		// (get) Token: 0x06005014 RID: 20500 RVA: 0x000FAB52 File Offset: 0x000F8D52
		public override FieldAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		// Token: 0x17000D27 RID: 3367
		// (get) Token: 0x06005015 RID: 20501 RVA: 0x000FAB5A File Offset: 0x000F8D5A
		public override Type DeclaringType
		{
			get
			{
				return this.typeb;
			}
		}

		// Token: 0x17000D28 RID: 3368
		// (get) Token: 0x06005016 RID: 20502 RVA: 0x000FAB62 File Offset: 0x000F8D62
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				throw this.CreateNotSupportedException();
			}
		}

		// Token: 0x17000D29 RID: 3369
		// (get) Token: 0x06005017 RID: 20503 RVA: 0x000FAB6A File Offset: 0x000F8D6A
		public override Type FieldType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06005018 RID: 20504 RVA: 0x000FAB72 File Offset: 0x000F8D72
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06005019 RID: 20505 RVA: 0x000FAB5A File Offset: 0x000F8D5A
		public override Type ReflectedType
		{
			get
			{
				return this.typeb;
			}
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x000FAB7A File Offset: 0x000F8D7A
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.typeb.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, inherit);
			}
			throw this.CreateNotSupportedException();
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x000FAB97 File Offset: 0x000F8D97
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.typeb.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
			}
			throw this.CreateNotSupportedException();
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x0600501C RID: 20508 RVA: 0x000FABB5 File Offset: 0x000F8DB5
		public override int MetadataToken
		{
			get
			{
				return ((ModuleBuilder)this.typeb.Module).GetToken(this);
			}
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x000FABCD File Offset: 0x000F8DCD
		public FieldToken GetToken()
		{
			return new FieldToken(this.MetadataToken);
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x000FAB62 File Offset: 0x000F8D62
		public override object GetValue(object obj)
		{
			throw this.CreateNotSupportedException();
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x000FAB62 File Offset: 0x000F8D62
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.CreateNotSupportedException();
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override int GetFieldOffset()
		{
			return 0;
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x000FABDA File Offset: 0x000F8DDA
		internal void SetRVAData(byte[] data)
		{
			this.rva_data = (byte[])data.Clone();
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x000FABED File Offset: 0x000F8DED
		public void SetConstant(object defaultValue)
		{
			this.RejectIfCreated();
			this.def_value = defaultValue;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x000FABFC File Offset: 0x000F8DFC
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.RejectIfCreated();
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.InteropServices.FieldOffsetAttribute")
			{
				byte[] data = customBuilder.Data;
				this.offset = (int)data[2];
				this.offset |= (int)data[3] << 8;
				this.offset |= (int)data[4] << 16;
				this.offset |= (int)data[5] << 24;
				return;
			}
			if (fullName == "System.NonSerializedAttribute")
			{
				this.attrs |= FieldAttributes.NotSerialized;
				return;
			}
			if (fullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= FieldAttributes.SpecialName;
				return;
			}
			if (fullName == "System.Runtime.InteropServices.MarshalAsAttribute")
			{
				this.attrs |= FieldAttributes.HasFieldMarshal;
				this.marshal_info = CustomAttributeBuilder.get_umarshal(customBuilder, true);
				return;
			}
			if (this.cattrs != null)
			{
				CustomAttributeBuilder[] array = new CustomAttributeBuilder[this.cattrs.Length + 1];
				this.cattrs.CopyTo(array, 0);
				array[this.cattrs.Length] = customBuilder;
				this.cattrs = array;
				return;
			}
			this.cattrs = new CustomAttributeBuilder[1];
			this.cattrs[0] = customBuilder;
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x000FAD3E File Offset: 0x000F8F3E
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.RejectIfCreated();
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x000FAD53 File Offset: 0x000F8F53
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.RejectIfCreated();
			this.marshal_info = unmanagedMarshal;
			this.attrs |= FieldAttributes.HasFieldMarshal;
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x000FAD74 File Offset: 0x000F8F74
		public void SetOffset(int iOffset)
		{
			this.RejectIfCreated();
			if (iOffset < 0)
			{
				throw new ArgumentException("Negative field offset is not allowed");
			}
			this.offset = iOffset;
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x000FAB62 File Offset: 0x000F8D62
		public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			throw this.CreateNotSupportedException();
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception CreateNotSupportedException()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x000FAD92 File Offset: 0x000F8F92
		private void RejectIfCreated()
		{
			if (this.typeb.is_created)
			{
				throw new InvalidOperationException("Unable to change after type has been created.");
			}
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x000FADAC File Offset: 0x000F8FAC
		internal void ResolveUserTypes()
		{
			this.type = TypeBuilder.ResolveUserType(this.type);
			TypeBuilder.ResolveUserTypes(this.modReq);
			TypeBuilder.ResolveUserTypes(this.modOpt);
			if (this.marshal_info != null)
			{
				this.marshal_info.marshaltyperef = TypeBuilder.ResolveUserType(this.marshal_info.marshaltyperef);
			}
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x000FAE04 File Offset: 0x000F9004
		internal FieldInfo RuntimeResolve()
		{
			RuntimeTypeHandle declaringType = new RuntimeTypeHandle(this.typeb.CreateType() as RuntimeType);
			return FieldInfo.GetFieldFromHandle(this.handle, declaringType);
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x0600502C RID: 20524 RVA: 0x000FAE34 File Offset: 0x000F9034
		public override Module Module
		{
			get
			{
				return base.Module;
			}
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x000173AD File Offset: 0x000155AD
		internal FieldBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400315D RID: 12637
		private FieldAttributes attrs;

		// Token: 0x0400315E RID: 12638
		private Type type;

		// Token: 0x0400315F RID: 12639
		private string name;

		// Token: 0x04003160 RID: 12640
		private object def_value;

		// Token: 0x04003161 RID: 12641
		private int offset;

		// Token: 0x04003162 RID: 12642
		internal TypeBuilder typeb;

		// Token: 0x04003163 RID: 12643
		private byte[] rva_data;

		// Token: 0x04003164 RID: 12644
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x04003165 RID: 12645
		private UnmanagedMarshal marshal_info;

		// Token: 0x04003166 RID: 12646
		private RuntimeFieldHandle handle;

		// Token: 0x04003167 RID: 12647
		private Type[] modReq;

		// Token: 0x04003168 RID: 12648
		private Type[] modOpt;
	}
}

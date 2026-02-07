using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000934 RID: 2356
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_MethodBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class MethodBuilder : MethodInfo, _MethodBuilder
	{
		// Token: 0x06005100 RID: 20736 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x000FD828 File Offset: 0x000FBA28
		internal MethodBuilder(TypeBuilder tb, string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnModReq, Type[] returnModOpt, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
		{
			this.init_locals = true;
			base..ctor();
			this.name = name;
			this.attrs = attributes;
			this.call_conv = callingConvention;
			this.rtype = returnType;
			this.returnModReq = returnModReq;
			this.returnModOpt = returnModOpt;
			this.paramModReq = paramModReq;
			this.paramModOpt = paramModOpt;
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				this.call_conv |= CallingConventions.HasThis;
			}
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentException("Elements of the parameterTypes array cannot be null", "parameterTypes");
					}
				}
				this.parameters = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.parameters, parameterTypes.Length);
			}
			this.type = tb;
			this.table_idx = this.get_next_table_index(this, 6, 1);
			((ModuleBuilder)tb.Module).RegisterToken(this, this.GetToken().Token);
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x000FD91C File Offset: 0x000FBB1C
		internal MethodBuilder(TypeBuilder tb, string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnModReq, Type[] returnModOpt, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt, string dllName, string entryName, CallingConvention nativeCConv, CharSet nativeCharset) : this(tb, name, attributes, callingConvention, returnType, returnModReq, returnModOpt, parameterTypes, paramModReq, paramModOpt)
		{
			this.pi_dll = dllName;
			this.pi_entry = entryName;
			this.native_cc = nativeCConv;
			this.charset = nativeCharset;
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06005106 RID: 20742 RVA: 0x000472CC File Offset: 0x000454CC
		public override bool ContainsGenericParameters
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06005107 RID: 20743 RVA: 0x000FD960 File Offset: 0x000FBB60
		// (set) Token: 0x06005108 RID: 20744 RVA: 0x000FD968 File Offset: 0x000FBB68
		public bool InitLocals
		{
			get
			{
				return this.init_locals;
			}
			set
			{
				this.init_locals = value;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06005109 RID: 20745 RVA: 0x000FD971 File Offset: 0x000FBB71
		internal TypeBuilder TypeBuilder
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x0600510A RID: 20746 RVA: 0x000FD979 File Offset: 0x000FBB79
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw this.NotSupported();
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x0600510B RID: 20747 RVA: 0x000FD981 File Offset: 0x000FBB81
		internal RuntimeMethodHandle MethodHandleInternal
		{
			get
			{
				return this.mhandle;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x0600510C RID: 20748 RVA: 0x000FD989 File Offset: 0x000FBB89
		public override Type ReturnType
		{
			get
			{
				return this.rtype;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x0600510D RID: 20749 RVA: 0x000FD971 File Offset: 0x000FBB71
		public override Type ReflectedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600510E RID: 20750 RVA: 0x000FD971 File Offset: 0x000FBB71
		public override Type DeclaringType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x0600510F RID: 20751 RVA: 0x000FD991 File Offset: 0x000FBB91
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06005110 RID: 20752 RVA: 0x000FD999 File Offset: 0x000FBB99
		public override MethodAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06005111 RID: 20753 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06005112 RID: 20754 RVA: 0x000FD9A1 File Offset: 0x000FBBA1
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.call_conv;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06005113 RID: 20755 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public string Signature
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (set) Token: 0x06005114 RID: 20756 RVA: 0x000FD9A9 File Offset: 0x000FBBA9
		internal bool BestFitMapping
		{
			set
			{
				this.extra_flags = (uint)(((ulong)this.extra_flags & 18446744073709551567UL) | (value ? 16UL : 32UL));
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (set) Token: 0x06005115 RID: 20757 RVA: 0x000FD9C8 File Offset: 0x000FBBC8
		internal bool ThrowOnUnmappableChar
		{
			set
			{
				this.extra_flags = (uint)(((ulong)this.extra_flags & 18446744073709539327UL) | (value ? 4096UL : 8192UL));
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (set) Token: 0x06005116 RID: 20758 RVA: 0x000FD9F0 File Offset: 0x000FBBF0
		internal bool ExactSpelling
		{
			set
			{
				this.extra_flags = (uint)(((ulong)this.extra_flags & 18446744073709551614UL) | (value ? 1UL : 0UL));
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (set) Token: 0x06005117 RID: 20759 RVA: 0x000FDA0D File Offset: 0x000FBC0D
		internal bool SetLastError
		{
			set
			{
				this.extra_flags = (uint)(((ulong)this.extra_flags & 18446744073709551551UL) | (value ? 64UL : 0UL));
			}
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x000FDA2B File Offset: 0x000FBC2B
		public MethodToken GetToken()
		{
			return new MethodToken(100663296 | this.table_idx);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x0000270D File Offset: 0x0000090D
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x000FDA3E File Offset: 0x000FBC3E
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.iattrs;
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x000FDA46 File Offset: 0x000FBC46
		public override ParameterInfo[] GetParameters()
		{
			if (!this.type.is_created)
			{
				throw this.NotSupported();
			}
			return this.GetParametersInternal();
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x000FDA64 File Offset: 0x000FBC64
		internal override ParameterInfo[] GetParametersInternal()
		{
			if (this.parameters == null)
			{
				return null;
			}
			ParameterInfo[] array = new ParameterInfo[this.parameters.Length];
			for (int i = 0; i < this.parameters.Length; i++)
			{
				ParameterInfo[] array2 = array;
				int num = i;
				ParameterBuilder[] array3 = this.pinfo;
				array2[num] = RuntimeParameterInfo.New((array3 != null) ? array3[i + 1] : null, this.parameters[i], this, i + 1);
			}
			return array;
		}

		// Token: 0x0600511D RID: 20765 RVA: 0x000FDAC2 File Offset: 0x000FBCC2
		internal override int GetParametersCount()
		{
			if (this.parameters == null)
			{
				return 0;
			}
			return this.parameters.Length;
		}

		// Token: 0x0600511E RID: 20766 RVA: 0x000FDAD6 File Offset: 0x000FBCD6
		internal override Type GetParameterType(int pos)
		{
			return this.parameters[pos];
		}

		// Token: 0x0600511F RID: 20767 RVA: 0x000FDAE0 File Offset: 0x000FBCE0
		internal MethodBase RuntimeResolve()
		{
			return this.type.RuntimeResolve().GetMethod(this);
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x000FDAF3 File Offset: 0x000FBCF3
		public Module GetModule()
		{
			return this.type.Module;
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x000FDB00 File Offset: 0x000FBD00
		public void CreateMethodBody(byte[] il, int count)
		{
			if (il != null && (count < 0 || count > il.Length))
			{
				throw new ArgumentOutOfRangeException("Index was out of range.  Must be non-negative and less than the size of the collection.");
			}
			if (this.code != null || this.type.is_created)
			{
				throw new InvalidOperationException("Type definition of the method is complete.");
			}
			if (il == null)
			{
				this.code = null;
				return;
			}
			this.code = new byte[count];
			Array.Copy(il, this.code, count);
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x000FDB69 File Offset: 0x000FBD69
		public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
		{
			this.GetILGenerator().Init(il, maxStack, localSignature, exceptionHandlers, tokenFixups);
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x000FD979 File Offset: 0x000FBB79
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw this.NotSupported();
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x000FD979 File Offset: 0x000FBB79
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.NotSupported();
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x000FDB7D File Offset: 0x000FBD7D
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.type.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, inherit);
			}
			throw this.NotSupported();
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x000FDB9A File Offset: 0x000FBD9A
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.type.is_created)
			{
				return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
			}
			throw this.NotSupported();
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x000FDBB8 File Offset: 0x000FBDB8
		public ILGenerator GetILGenerator()
		{
			return this.GetILGenerator(64);
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x000FDBC4 File Offset: 0x000FBDC4
		public ILGenerator GetILGenerator(int size)
		{
			if ((this.iattrs & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL || (this.iattrs & MethodImplAttributes.ManagedMask) != MethodImplAttributes.IL)
			{
				throw new InvalidOperationException("Method body should not exist.");
			}
			if (this.ilgen != null)
			{
				return this.ilgen;
			}
			this.ilgen = new ILGenerator(this.type.Module, ((ModuleBuilder)this.type.Module).GetTokenGenerator(), size);
			return this.ilgen;
		}

		// Token: 0x06005129 RID: 20777 RVA: 0x000FDC34 File Offset: 0x000FBE34
		public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
		{
			this.RejectIfCreated();
			if (position < 0 || this.parameters == null || position > this.parameters.Length)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			ParameterBuilder parameterBuilder = new ParameterBuilder(this, position, attributes, strParamName);
			if (this.pinfo == null)
			{
				this.pinfo = new ParameterBuilder[this.parameters.Length + 1];
			}
			this.pinfo[position] = parameterBuilder;
			return parameterBuilder;
		}

		// Token: 0x0600512A RID: 20778 RVA: 0x000FDC9C File Offset: 0x000FBE9C
		internal void check_override()
		{
			if (this.override_methods != null)
			{
				foreach (MethodInfo methodInfo in this.override_methods)
				{
					if (methodInfo.IsVirtual && !base.IsVirtual)
					{
						throw new TypeLoadException(string.Format("Method '{0}' override '{1}' but it is not virtual", this.name, methodInfo));
					}
				}
			}
		}

		// Token: 0x0600512B RID: 20779 RVA: 0x000FDCF4 File Offset: 0x000FBEF4
		internal void fixup()
		{
			if ((this.attrs & (MethodAttributes.Abstract | MethodAttributes.PinvokeImpl)) == MethodAttributes.PrivateScope && (this.iattrs & (MethodImplAttributes)4099) == MethodImplAttributes.IL && (this.ilgen == null || this.ilgen.ILOffset == 0) && (this.code == null || this.code.Length == 0))
			{
				throw new InvalidOperationException(string.Format("Method '{0}.{1}' does not have a method body.", this.DeclaringType.FullName, this.Name));
			}
			if (this.ilgen != null)
			{
				this.ilgen.label_fixup(this);
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x000FDD78 File Offset: 0x000FBF78
		internal void ResolveUserTypes()
		{
			this.rtype = TypeBuilder.ResolveUserType(this.rtype);
			TypeBuilder.ResolveUserTypes(this.parameters);
			TypeBuilder.ResolveUserTypes(this.returnModReq);
			TypeBuilder.ResolveUserTypes(this.returnModOpt);
			if (this.paramModReq != null)
			{
				Type[][] array = this.paramModReq;
				for (int i = 0; i < array.Length; i++)
				{
					TypeBuilder.ResolveUserTypes(array[i]);
				}
			}
			if (this.paramModOpt != null)
			{
				Type[][] array = this.paramModOpt;
				for (int i = 0; i < array.Length; i++)
				{
					TypeBuilder.ResolveUserTypes(array[i]);
				}
			}
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x000FDE01 File Offset: 0x000FC001
		internal void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map)
		{
			if (this.ilgen != null)
			{
				this.ilgen.FixupTokens(token_map, member_map);
			}
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x000FDE18 File Offset: 0x000FC018
		internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
		{
			if (this.ilgen != null && this.ilgen.HasDebugInfo)
			{
				SymbolToken symbolToken = new SymbolToken(this.GetToken().Token);
				symbolWriter.OpenMethod(symbolToken);
				symbolWriter.SetSymAttribute(symbolToken, "__name", Encoding.UTF8.GetBytes(this.Name));
				this.ilgen.GenerateDebugInfo(symbolWriter);
				symbolWriter.CloseMethod();
			}
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x000FDE84 File Offset: 0x000FC084
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.CompilerServices.MethodImplAttribute")
			{
				byte[] data = customBuilder.Data;
				int num = (int)data[2];
				num |= (int)data[3] << 8;
				this.iattrs |= (MethodImplAttributes)num;
				return;
			}
			if (!(fullName == "System.Runtime.InteropServices.DllImportAttribute"))
			{
				if (fullName == "System.Runtime.InteropServices.PreserveSigAttribute")
				{
					this.iattrs |= MethodImplAttributes.PreserveSig;
					return;
				}
				if (fullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
				{
					this.attrs |= MethodAttributes.SpecialName;
					return;
				}
				if (fullName == "System.Security.SuppressUnmanagedCodeSecurityAttribute")
				{
					this.attrs |= MethodAttributes.HasSecurity;
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
				return;
			}
			else
			{
				CustomAttributeBuilder.CustomAttributeInfo customAttributeInfo = CustomAttributeBuilder.decode_cattr(customBuilder);
				bool flag = true;
				this.pi_dll = (string)customAttributeInfo.ctorArgs[0];
				if (this.pi_dll == null || this.pi_dll.Length == 0)
				{
					throw new ArgumentException("DllName cannot be empty");
				}
				this.native_cc = System.Runtime.InteropServices.CallingConvention.Winapi;
				for (int i = 0; i < customAttributeInfo.namedParamNames.Length; i++)
				{
					string a = customAttributeInfo.namedParamNames[i];
					object obj = customAttributeInfo.namedParamValues[i];
					if (a == "CallingConvention")
					{
						this.native_cc = (CallingConvention)obj;
					}
					else if (a == "CharSet")
					{
						this.charset = (CharSet)obj;
					}
					else if (a == "EntryPoint")
					{
						this.pi_entry = (string)obj;
					}
					else if (a == "ExactSpelling")
					{
						this.ExactSpelling = (bool)obj;
					}
					else if (a == "SetLastError")
					{
						this.SetLastError = (bool)obj;
					}
					else if (a == "PreserveSig")
					{
						flag = (bool)obj;
					}
					else if (a == "BestFitMapping")
					{
						this.BestFitMapping = (bool)obj;
					}
					else if (a == "ThrowOnUnmappableChar")
					{
						this.ThrowOnUnmappableChar = (bool)obj;
					}
				}
				this.attrs |= MethodAttributes.PinvokeImpl;
				if (flag)
				{
					this.iattrs |= MethodImplAttributes.PreserveSig;
				}
				return;
			}
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x000FE138 File Offset: 0x000FC338
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x000FE169 File Offset: 0x000FC369
		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			this.RejectIfCreated();
			this.iattrs = attributes;
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x000FE178 File Offset: 0x000FC378
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("Request* values are not permitted", "action");
			}
			this.RejectIfCreated();
			if (this.permissions != null)
			{
				RefEmitPermissionSet[] array = this.permissions;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].action == action)
					{
						throw new InvalidOperationException("Multiple permission sets specified with the same SecurityAction.");
					}
				}
				RefEmitPermissionSet[] array2 = new RefEmitPermissionSet[this.permissions.Length + 1];
				this.permissions.CopyTo(array2, 0);
				this.permissions = array2;
			}
			else
			{
				this.permissions = new RefEmitPermissionSet[1];
			}
			this.permissions[this.permissions.Length - 1] = new RefEmitPermissionSet(action, pset.ToXml().ToString());
			this.attrs |= MethodAttributes.HasSecurity;
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x000FE258 File Offset: 0x000FC458
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
		public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			this.RejectIfCreated();
			throw new NotImplementedException();
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x000FE258 File Offset: 0x000FC458
		[MonoTODO]
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			this.RejectIfCreated();
			throw new NotImplementedException();
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x000FE265 File Offset: 0x000FC465
		[SecuritySafeCritical]
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MethodBuilder [",
				this.type.Name,
				"::",
				this.name,
				"]"
			});
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x000FE2A1 File Offset: 0x000FC4A1
		[MonoTODO]
		[SecuritySafeCritical]
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x000FE2AA File Offset: 0x000FC4AA
		public override int GetHashCode()
		{
			return this.name.GetHashCode();
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x000FE2B7 File Offset: 0x000FC4B7
		internal override int get_next_table_index(object obj, int table, int count)
		{
			return this.type.get_next_table_index(obj, table, count);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x000FE2C8 File Offset: 0x000FC4C8
		private void ExtendArray<T>(ref T[] array, T elem)
		{
			if (array == null)
			{
				array = new T[1];
			}
			else
			{
				T[] array2 = new T[array.Length + 1];
				Array.Copy(array, array2, array.Length);
				array = array2;
			}
			array[array.Length - 1] = elem;
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x000FE30C File Offset: 0x000FC50C
		internal void set_override(MethodInfo mdecl)
		{
			this.ExtendArray<MethodInfo>(ref this.override_methods, mdecl);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x000FE31B File Offset: 0x000FC51B
		private void RejectIfCreated()
		{
			if (this.type.is_created)
			{
				throw new InvalidOperationException("Type definition of the method is complete.");
			}
		}

		// Token: 0x0600513C RID: 20796 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception NotSupported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x000FE338 File Offset: 0x000FC538
		public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException("Method is not a generic method definition");
			}
			if (typeArguments == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
			if (this.generic_params.Length != typeArguments.Length)
			{
				throw new ArgumentException("Incorrect length", "typeArguments");
			}
			for (int i = 0; i < typeArguments.Length; i++)
			{
				if (typeArguments[i] == null)
				{
					throw new ArgumentNullException("typeArguments");
				}
			}
			return new MethodOnTypeBuilderInst(this, typeArguments);
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x0600513E RID: 20798 RVA: 0x000FE3B0 File Offset: 0x000FC5B0
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.generic_params != null;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x0600513F RID: 20799 RVA: 0x000FE3B0 File Offset: 0x000FC5B0
		public override bool IsGenericMethod
		{
			get
			{
				return this.generic_params != null;
			}
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x000FE3BB File Offset: 0x000FC5BB
		public override MethodInfo GetGenericMethodDefinition()
		{
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException();
			}
			return this;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x000FE3CC File Offset: 0x000FC5CC
		public override Type[] GetGenericArguments()
		{
			if (this.generic_params == null)
			{
				return null;
			}
			Type[] array = new Type[this.generic_params.Length];
			for (int i = 0; i < this.generic_params.Length; i++)
			{
				array[i] = this.generic_params[i];
			}
			return array;
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x000FE410 File Offset: 0x000FC610
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			if (names.Length == 0)
			{
				throw new ArgumentException("names");
			}
			this.generic_params = new GenericTypeParameterBuilder[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				string text = names[i];
				if (text == null)
				{
					throw new ArgumentNullException("names");
				}
				this.generic_params[i] = new GenericTypeParameterBuilder(this.type, this, text, i);
			}
			return this.generic_params;
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x000FE484 File Offset: 0x000FC684
		public void SetReturnType(Type returnType)
		{
			this.rtype = returnType;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x000FE490 File Offset: 0x000FC690
		public void SetParameters(params Type[] parameterTypes)
		{
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentException("Elements of the parameterTypes array cannot be null", "parameterTypes");
					}
				}
				this.parameters = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.parameters, parameterTypes.Length);
			}
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x000FE4E6 File Offset: 0x000FC6E6
		public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.SetReturnType(returnType);
			this.SetParameters(parameterTypes);
			this.returnModReq = returnTypeRequiredCustomModifiers;
			this.returnModOpt = returnTypeOptionalCustomModifiers;
			this.paramModReq = parameterTypeRequiredCustomModifiers;
			this.paramModOpt = parameterTypeOptionalCustomModifiers;
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06005146 RID: 20806 RVA: 0x000FE515 File Offset: 0x000FC715
		public override Module Module
		{
			get
			{
				return this.GetModule();
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06005147 RID: 20807 RVA: 0x000FE51D File Offset: 0x000FC71D
		public override ParameterInfo ReturnParameter
		{
			get
			{
				return base.ReturnParameter;
			}
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x000173AD File Offset: 0x000155AD
		internal MethodBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040031B7 RID: 12727
		private RuntimeMethodHandle mhandle;

		// Token: 0x040031B8 RID: 12728
		private Type rtype;

		// Token: 0x040031B9 RID: 12729
		internal Type[] parameters;

		// Token: 0x040031BA RID: 12730
		private MethodAttributes attrs;

		// Token: 0x040031BB RID: 12731
		private MethodImplAttributes iattrs;

		// Token: 0x040031BC RID: 12732
		private string name;

		// Token: 0x040031BD RID: 12733
		private int table_idx;

		// Token: 0x040031BE RID: 12734
		private byte[] code;

		// Token: 0x040031BF RID: 12735
		private ILGenerator ilgen;

		// Token: 0x040031C0 RID: 12736
		private TypeBuilder type;

		// Token: 0x040031C1 RID: 12737
		internal ParameterBuilder[] pinfo;

		// Token: 0x040031C2 RID: 12738
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x040031C3 RID: 12739
		private MethodInfo[] override_methods;

		// Token: 0x040031C4 RID: 12740
		private string pi_dll;

		// Token: 0x040031C5 RID: 12741
		private string pi_entry;

		// Token: 0x040031C6 RID: 12742
		private CharSet charset;

		// Token: 0x040031C7 RID: 12743
		private uint extra_flags;

		// Token: 0x040031C8 RID: 12744
		private CallingConvention native_cc;

		// Token: 0x040031C9 RID: 12745
		private CallingConventions call_conv;

		// Token: 0x040031CA RID: 12746
		private bool init_locals;

		// Token: 0x040031CB RID: 12747
		private IntPtr generic_container;

		// Token: 0x040031CC RID: 12748
		internal GenericTypeParameterBuilder[] generic_params;

		// Token: 0x040031CD RID: 12749
		private Type[] returnModReq;

		// Token: 0x040031CE RID: 12750
		private Type[] returnModOpt;

		// Token: 0x040031CF RID: 12751
		private Type[][] paramModReq;

		// Token: 0x040031D0 RID: 12752
		private Type[][] paramModOpt;

		// Token: 0x040031D1 RID: 12753
		private RefEmitPermissionSet[] permissions;
	}
}

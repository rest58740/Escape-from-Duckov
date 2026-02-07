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
	// Token: 0x02000915 RID: 2325
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ConstructorBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ConstructorBuilder : ConstructorInfo, _ConstructorBuilder
	{
		// Token: 0x06004EEC RID: 20204 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ConstructorBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ConstructorBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ConstructorBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ConstructorBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x000F7F54 File Offset: 0x000F6154
		internal ConstructorBuilder(TypeBuilder tb, MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
		{
			this.init_locals = true;
			base..ctor();
			this.attrs = (attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
			this.call_conv = callingConvention;
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
			this.paramModReq = paramModReq;
			this.paramModOpt = paramModOpt;
			this.table_idx = this.get_next_table_index(this, 6, 1);
			((ModuleBuilder)tb.Module).RegisterToken(this, this.GetToken().Token);
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x000F801C File Offset: 0x000F621C
		[MonoTODO]
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.call_conv;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06004EF2 RID: 20210 RVA: 0x000F8024 File Offset: 0x000F6224
		// (set) Token: 0x06004EF3 RID: 20211 RVA: 0x000F802C File Offset: 0x000F622C
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

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004EF4 RID: 20212 RVA: 0x000F8035 File Offset: 0x000F6235
		internal TypeBuilder TypeBuilder
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x000F803D File Offset: 0x000F623D
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.iattrs;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x000F8045 File Offset: 0x000F6245
		public override ParameterInfo[] GetParameters()
		{
			if (!this.type.is_created)
			{
				throw this.not_created();
			}
			return this.GetParametersInternal();
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x000F8064 File Offset: 0x000F6264
		internal override ParameterInfo[] GetParametersInternal()
		{
			if (this.parameters == null)
			{
				return EmptyArray<ParameterInfo>.Value;
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

		// Token: 0x06004EF8 RID: 20216 RVA: 0x000F80C6 File Offset: 0x000F62C6
		internal override int GetParametersCount()
		{
			if (this.parameters == null)
			{
				return 0;
			}
			return this.parameters.Length;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x000F80DA File Offset: 0x000F62DA
		internal override Type GetParameterType(int pos)
		{
			return this.parameters[pos];
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x000F80E4 File Offset: 0x000F62E4
		internal MethodBase RuntimeResolve()
		{
			return this.type.RuntimeResolve().GetConstructor(this);
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw this.not_supported();
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw this.not_supported();
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004EFD RID: 20221 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw this.not_supported();
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06004EFE RID: 20222 RVA: 0x000F80FF File Offset: 0x000F62FF
		public override MethodAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06004EFF RID: 20223 RVA: 0x000F8035 File Offset: 0x000F6235
		public override Type ReflectedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06004F00 RID: 20224 RVA: 0x000F8035 File Offset: 0x000F6235
		public override Type DeclaringType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06004F01 RID: 20225 RVA: 0x0000AF5E File Offset: 0x0000915E
		[Obsolete]
		public Type ReturnType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06004F02 RID: 20226 RVA: 0x000F8107 File Offset: 0x000F6307
		public override string Name
		{
			get
			{
				if ((this.attrs & MethodAttributes.Static) == MethodAttributes.PrivateScope)
				{
					return ConstructorInfo.ConstructorName;
				}
				return ConstructorInfo.TypeConstructorName;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06004F03 RID: 20227 RVA: 0x000F811F File Offset: 0x000F631F
		public string Signature
		{
			get
			{
				return "constructor signature";
			}
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x000F8128 File Offset: 0x000F6328
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("action", "Request* values are not permitted");
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

		// Token: 0x06004F05 RID: 20229 RVA: 0x000F8208 File Offset: 0x000F6408
		public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string strParamName)
		{
			if (iSequence < 0 || iSequence > this.GetParametersCount())
			{
				throw new ArgumentOutOfRangeException("iSequence");
			}
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
			ParameterBuilder parameterBuilder = new ParameterBuilder(this, iSequence, attributes, strParamName);
			if (this.pinfo == null)
			{
				this.pinfo = new ParameterBuilder[this.parameters.Length + 1];
			}
			this.pinfo[iSequence] = parameterBuilder;
			return parameterBuilder;
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x000F80F7 File Offset: 0x000F62F7
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x000F8272 File Offset: 0x000F6472
		public ILGenerator GetILGenerator()
		{
			return this.GetILGenerator(64);
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x000F827C File Offset: 0x000F647C
		public ILGenerator GetILGenerator(int streamSize)
		{
			if (this.ilgen != null)
			{
				return this.ilgen;
			}
			this.ilgen = new ILGenerator(this.type.Module, ((ModuleBuilder)this.type.Module).GetTokenGenerator(), streamSize);
			return this.ilgen;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x000F82CA File Offset: 0x000F64CA
		public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
		{
			this.GetILGenerator().Init(il, maxStack, localSignature, exceptionHandlers, tokenFixups);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x000F82E0 File Offset: 0x000F64E0
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			if (customBuilder.Ctor.ReflectedType.FullName == "System.Runtime.CompilerServices.MethodImplAttribute")
			{
				byte[] data = customBuilder.Data;
				int num = (int)data[2];
				num |= (int)data[3] << 8;
				this.SetImplementationFlags((MethodImplAttributes)num);
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

		// Token: 0x06004F0D RID: 20237 RVA: 0x000F837F File Offset: 0x000F657F
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

		// Token: 0x06004F0E RID: 20238 RVA: 0x000F83B0 File Offset: 0x000F65B0
		public void SetImplementationFlags(MethodImplAttributes attributes)
		{
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
			this.iattrs = attributes;
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x000F83CD File Offset: 0x000F65CD
		public Module GetModule()
		{
			return this.type.Module;
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x000F83DA File Offset: 0x000F65DA
		public MethodToken GetToken()
		{
			return new MethodToken(100663296 | this.table_idx);
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x000F83ED File Offset: 0x000F65ED
		[MonoTODO]
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			if (this.type.is_created)
			{
				throw this.not_after_created();
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06004F12 RID: 20242 RVA: 0x000F8403 File Offset: 0x000F6603
		public override Module Module
		{
			get
			{
				return this.GetModule();
			}
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x000F840B File Offset: 0x000F660B
		public override string ToString()
		{
			return "ConstructorBuilder ['" + this.type.Name + "']";
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x000F8428 File Offset: 0x000F6628
		internal void fixup()
		{
			if ((this.attrs & (MethodAttributes.Abstract | MethodAttributes.PinvokeImpl)) == MethodAttributes.PrivateScope && (this.iattrs & (MethodImplAttributes)4099) == MethodImplAttributes.IL && (this.ilgen == null || this.ilgen.ILOffset == 0))
			{
				throw new InvalidOperationException("Method '" + this.Name + "' does not have a method body.");
			}
			if (this.ilgen != null)
			{
				this.ilgen.label_fixup(this);
			}
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x000F8498 File Offset: 0x000F6698
		internal void ResolveUserTypes()
		{
			TypeBuilder.ResolveUserTypes(this.parameters);
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

		// Token: 0x06004F16 RID: 20246 RVA: 0x000F84FA File Offset: 0x000F66FA
		internal void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map)
		{
			if (this.ilgen != null)
			{
				this.ilgen.FixupTokens(token_map, member_map);
			}
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x000F8514 File Offset: 0x000F6714
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

		// Token: 0x06004F18 RID: 20248 RVA: 0x000F8580 File Offset: 0x000F6780
		internal override int get_next_table_index(object obj, int table, int count)
		{
			return this.type.get_next_table_index(obj, table, count);
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x000F8590 File Offset: 0x000F6790
		private void RejectIfCreated()
		{
			if (this.type.is_created)
			{
				throw new InvalidOperationException("Type definition of the method is complete.");
			}
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x000F85AA File Offset: 0x000F67AA
		private Exception not_after_created()
		{
			return new InvalidOperationException("Unable to change after type has been created.");
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x000F85B6 File Offset: 0x000F67B6
		private Exception not_created()
		{
			return new NotSupportedException("The type is not yet created.");
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x000173AD File Offset: 0x000155AD
		internal ConstructorBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04003119 RID: 12569
		private RuntimeMethodHandle mhandle;

		// Token: 0x0400311A RID: 12570
		private ILGenerator ilgen;

		// Token: 0x0400311B RID: 12571
		internal Type[] parameters;

		// Token: 0x0400311C RID: 12572
		private MethodAttributes attrs;

		// Token: 0x0400311D RID: 12573
		private MethodImplAttributes iattrs;

		// Token: 0x0400311E RID: 12574
		private int table_idx;

		// Token: 0x0400311F RID: 12575
		private CallingConventions call_conv;

		// Token: 0x04003120 RID: 12576
		private TypeBuilder type;

		// Token: 0x04003121 RID: 12577
		internal ParameterBuilder[] pinfo;

		// Token: 0x04003122 RID: 12578
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x04003123 RID: 12579
		private bool init_locals;

		// Token: 0x04003124 RID: 12580
		private Type[][] paramModReq;

		// Token: 0x04003125 RID: 12581
		private Type[][] paramModOpt;

		// Token: 0x04003126 RID: 12582
		private RefEmitPermissionSet[] permissions;
	}
}

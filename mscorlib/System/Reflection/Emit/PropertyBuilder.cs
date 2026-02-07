using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000940 RID: 2368
	[ComDefaultInterface(typeof(_PropertyBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class PropertyBuilder : PropertyInfo, _PropertyBuilder
	{
		// Token: 0x06005217 RID: 21015 RVA: 0x000479FC File Offset: 0x00045BFC
		void _PropertyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005218 RID: 21016 RVA: 0x000479FC File Offset: 0x00045BFC
		void _PropertyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005219 RID: 21017 RVA: 0x000479FC File Offset: 0x00045BFC
		void _PropertyBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600521A RID: 21018 RVA: 0x000479FC File Offset: 0x00045BFC
		void _PropertyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600521B RID: 21019 RVA: 0x00102470 File Offset: 0x00100670
		internal PropertyBuilder(TypeBuilder tb, string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnModReq, Type[] returnModOpt, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
		{
			this.name = name;
			this.attrs = attributes;
			this.callingConvention = callingConvention;
			this.type = returnType;
			this.returnModReq = returnModReq;
			this.returnModOpt = returnModOpt;
			this.paramModReq = paramModReq;
			this.paramModOpt = paramModOpt;
			if (parameterTypes != null)
			{
				this.parameters = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.parameters, this.parameters.Length);
			}
			this.typeb = tb;
			this.table_idx = tb.get_next_table_index(this, 23, 1);
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x0600521C RID: 21020 RVA: 0x00102500 File Offset: 0x00100700
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.attrs;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x0600521D RID: 21021 RVA: 0x00102508 File Offset: 0x00100708
		public override bool CanRead
		{
			get
			{
				return this.get_method != null;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x0600521E RID: 21022 RVA: 0x00102516 File Offset: 0x00100716
		public override bool CanWrite
		{
			get
			{
				return this.set_method != null;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x0600521F RID: 21023 RVA: 0x00102524 File Offset: 0x00100724
		public override Type DeclaringType
		{
			get
			{
				return this.typeb;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x06005220 RID: 21024 RVA: 0x0010252C File Offset: 0x0010072C
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x06005221 RID: 21025 RVA: 0x00102534 File Offset: 0x00100734
		public PropertyToken PropertyToken
		{
			get
			{
				return default(PropertyToken);
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x06005222 RID: 21026 RVA: 0x0010254A File Offset: 0x0010074A
		public override Type PropertyType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x00102524 File Offset: 0x00100724
		public override Type ReflectedType
		{
			get
			{
				return this.typeb;
			}
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void AddOtherMethod(MethodBuilder mdBuilder)
		{
		}

		// Token: 0x06005225 RID: 21029 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return null;
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x00102552 File Offset: 0x00100752
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06005227 RID: 21031 RVA: 0x00102552 File Offset: 0x00100752
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06005228 RID: 21032 RVA: 0x0010255A File Offset: 0x0010075A
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return this.get_method;
		}

		// Token: 0x06005229 RID: 21033 RVA: 0x00102552 File Offset: 0x00100752
		public override ParameterInfo[] GetIndexParameters()
		{
			throw this.not_supported();
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x00102562 File Offset: 0x00100762
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return this.set_method;
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override object GetValue(object obj, object[] index)
		{
			return null;
		}

		// Token: 0x0600522C RID: 21036 RVA: 0x00102552 File Offset: 0x00100752
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			throw this.not_supported();
		}

		// Token: 0x0600522D RID: 21037 RVA: 0x00102552 File Offset: 0x00100752
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x0600522E RID: 21038 RVA: 0x0010256A File Offset: 0x0010076A
		public void SetConstant(object defaultValue)
		{
			this.def_value = defaultValue;
		}

		// Token: 0x0600522F RID: 21039 RVA: 0x00102574 File Offset: 0x00100774
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Ctor.ReflectedType.FullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= PropertyAttributes.SpecialName;
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

		// Token: 0x06005230 RID: 21040 RVA: 0x001025FD File Offset: 0x001007FD
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06005231 RID: 21041 RVA: 0x0010260C File Offset: 0x0010080C
		public void SetGetMethod(MethodBuilder mdBuilder)
		{
			this.get_method = mdBuilder;
		}

		// Token: 0x06005232 RID: 21042 RVA: 0x00102615 File Offset: 0x00100815
		public void SetSetMethod(MethodBuilder mdBuilder)
		{
			this.set_method = mdBuilder;
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public override void SetValue(object obj, object value, object[] index)
		{
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x06005235 RID: 21045 RVA: 0x000FAE34 File Offset: 0x000F9034
		public override Module Module
		{
			get
			{
				return base.Module;
			}
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x000173AD File Offset: 0x000155AD
		internal PropertyBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040032F8 RID: 13048
		private PropertyAttributes attrs;

		// Token: 0x040032F9 RID: 13049
		private string name;

		// Token: 0x040032FA RID: 13050
		private Type type;

		// Token: 0x040032FB RID: 13051
		private Type[] parameters;

		// Token: 0x040032FC RID: 13052
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x040032FD RID: 13053
		private object def_value;

		// Token: 0x040032FE RID: 13054
		private MethodBuilder set_method;

		// Token: 0x040032FF RID: 13055
		private MethodBuilder get_method;

		// Token: 0x04003300 RID: 13056
		private int table_idx;

		// Token: 0x04003301 RID: 13057
		internal TypeBuilder typeb;

		// Token: 0x04003302 RID: 13058
		private Type[] returnModReq;

		// Token: 0x04003303 RID: 13059
		private Type[] returnModOpt;

		// Token: 0x04003304 RID: 13060
		private Type[][] paramModReq;

		// Token: 0x04003305 RID: 13061
		private Type[][] paramModOpt;

		// Token: 0x04003306 RID: 13062
		private CallingConventions callingConvention;
	}
}

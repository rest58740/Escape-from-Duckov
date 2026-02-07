using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000943 RID: 2371
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_SignatureHelper))]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class SignatureHelper : _SignatureHelper
	{
		// Token: 0x06005252 RID: 21074 RVA: 0x0010283E File Offset: 0x00100A3E
		internal SignatureHelper(ModuleBuilder module, SignatureHelper.SignatureHelperType type)
		{
			this.type = type;
			this.module = module;
		}

		// Token: 0x06005253 RID: 21075 RVA: 0x00102854 File Offset: 0x00100A54
		public static SignatureHelper GetFieldSigHelper(Module mod)
		{
			if (mod != null && !(mod is ModuleBuilder))
			{
				throw new ArgumentException("ModuleBuilder is expected");
			}
			return new SignatureHelper((ModuleBuilder)mod, SignatureHelper.SignatureHelperType.HELPER_FIELD);
		}

		// Token: 0x06005254 RID: 21076 RVA: 0x0010287E File Offset: 0x00100A7E
		public static SignatureHelper GetLocalVarSigHelper(Module mod)
		{
			if (mod != null && !(mod is ModuleBuilder))
			{
				throw new ArgumentException("ModuleBuilder is expected");
			}
			return new SignatureHelper((ModuleBuilder)mod, SignatureHelper.SignatureHelperType.HELPER_LOCAL);
		}

		// Token: 0x06005255 RID: 21077 RVA: 0x001028A8 File Offset: 0x00100AA8
		public static SignatureHelper GetLocalVarSigHelper()
		{
			return new SignatureHelper(null, SignatureHelper.SignatureHelperType.HELPER_LOCAL);
		}

		// Token: 0x06005256 RID: 21078 RVA: 0x001028B1 File Offset: 0x00100AB1
		public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, callingConvention, (CallingConvention)0, returnType, null);
		}

		// Token: 0x06005257 RID: 21079 RVA: 0x001028BD File Offset: 0x00100ABD
		public static SignatureHelper GetMethodSigHelper(CallingConvention unmanagedCallingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(null, CallingConventions.Standard, unmanagedCallingConvention, returnType, null);
		}

		// Token: 0x06005258 RID: 21080 RVA: 0x001028C9 File Offset: 0x00100AC9
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(mod, callingConvention, (CallingConvention)0, returnType, null);
		}

		// Token: 0x06005259 RID: 21081 RVA: 0x001028D5 File Offset: 0x00100AD5
		public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
		{
			return SignatureHelper.GetMethodSigHelper(mod, CallingConventions.Standard, unmanagedCallConv, returnType, null);
		}

		// Token: 0x0600525A RID: 21082 RVA: 0x001028E1 File Offset: 0x00100AE1
		public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			return SignatureHelper.GetMethodSigHelper(mod, CallingConventions.Standard, (CallingConvention)0, returnType, parameterTypes);
		}

		// Token: 0x0600525B RID: 21083 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600525C RID: 21084 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600525D RID: 21085 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600525E RID: 21086 RVA: 0x001028F0 File Offset: 0x00100AF0
		private static int AppendArray(ref Type[] array, Type t)
		{
			if (array != null)
			{
				Type[] array2 = new Type[array.Length + 1];
				Array.Copy(array, array2, array.Length);
				array2[array.Length] = t;
				array = array2;
				return array.Length - 1;
			}
			array = new Type[1];
			array[0] = t;
			return 0;
		}

		// Token: 0x0600525F RID: 21087 RVA: 0x00102938 File Offset: 0x00100B38
		private static void AppendArrayAt(ref Type[][] array, Type[] t, int pos)
		{
			int num = Math.Max(pos, (array == null) ? 0 : array.Length);
			Type[][] array2 = new Type[num + 1][];
			if (array != null)
			{
				Array.Copy(array, array2, num);
			}
			array2[pos] = t;
			array = array2;
		}

		// Token: 0x06005260 RID: 21088 RVA: 0x00102974 File Offset: 0x00100B74
		private static void ValidateParameterModifiers(string name, Type[] parameter_modifiers)
		{
			foreach (Type type in parameter_modifiers)
			{
				if (type == null)
				{
					throw new ArgumentNullException(name);
				}
				if (type.IsArray)
				{
					throw new ArgumentException(Locale.GetText("Array type not permitted"), name);
				}
				if (type.ContainsGenericParameters)
				{
					throw new ArgumentException(Locale.GetText("Open Generic Type not permitted"), name);
				}
			}
		}

		// Token: 0x06005261 RID: 21089 RVA: 0x001029D8 File Offset: 0x00100BD8
		private static void ValidateCustomModifier(int n, Type[][] custom_modifiers, string name)
		{
			if (custom_modifiers == null)
			{
				return;
			}
			if (custom_modifiers.Length != n)
			{
				throw new ArgumentException(Locale.GetText(string.Format("Custom modifiers length `{0}' does not match the size of the arguments", Array.Empty<object>())));
			}
			foreach (Type[] array in custom_modifiers)
			{
				if (array != null)
				{
					SignatureHelper.ValidateParameterModifiers(name, array);
				}
			}
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x00102A27 File Offset: 0x00100C27
		private static Exception MissingFeature()
		{
			throw new NotImplementedException("Mono does not currently support setting modOpt/modReq through SignatureHelper");
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x00102A34 File Offset: 0x00100C34
		[MonoTODO("Currently we ignore requiredCustomModifiers and optionalCustomModifiers")]
		public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			if (requiredCustomModifiers != null || optionalCustomModifiers != null)
			{
				throw SignatureHelper.MissingFeature();
			}
			SignatureHelper.ValidateCustomModifier(arguments.Length, requiredCustomModifiers, "requiredCustomModifiers");
			SignatureHelper.ValidateCustomModifier(arguments.Length, optionalCustomModifiers, "optionalCustomModifiers");
			for (int i = 0; i < arguments.Length; i++)
			{
				this.AddArgument(arguments[i], (requiredCustomModifiers != null) ? requiredCustomModifiers[i] : null, (optionalCustomModifiers != null) ? optionalCustomModifiers[i] : null);
			}
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x00102AA0 File Offset: 0x00100CA0
		[MonoTODO("pinned is ignored")]
		public void AddArgument(Type argument, bool pinned)
		{
			this.AddArgument(argument);
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x00102AAC File Offset: 0x00100CAC
		public void AddArgument(Type argument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			if (requiredCustomModifiers != null)
			{
				SignatureHelper.ValidateParameterModifiers("requiredCustomModifiers", requiredCustomModifiers);
			}
			if (optionalCustomModifiers != null)
			{
				SignatureHelper.ValidateParameterModifiers("optionalCustomModifiers", optionalCustomModifiers);
			}
			int pos = SignatureHelper.AppendArray(ref this.arguments, argument);
			if (requiredCustomModifiers != null)
			{
				SignatureHelper.AppendArrayAt(ref this.modreqs, requiredCustomModifiers, pos);
			}
			if (optionalCustomModifiers != null)
			{
				SignatureHelper.AppendArrayAt(ref this.modopts, optionalCustomModifiers, pos);
			}
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x00102B16 File Offset: 0x00100D16
		public void AddArgument(Type clsArgument)
		{
			if (clsArgument == null)
			{
				throw new ArgumentNullException("clsArgument");
			}
			SignatureHelper.AppendArray(ref this.arguments, clsArgument);
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO("Not implemented")]
		public void AddSentinel()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x00102B3C File Offset: 0x00100D3C
		private static bool CompareOK(Type[][] one, Type[][] two)
		{
			if (one == null)
			{
				return two == null;
			}
			if (two == null)
			{
				return false;
			}
			if (one.Length != two.Length)
			{
				return false;
			}
			int i = 0;
			while (i < one.Length)
			{
				Type[] array = one[i];
				Type[] array2 = two[i];
				if (array == null)
				{
					if (array2 != null)
					{
						goto IL_32;
					}
				}
				else
				{
					if (array2 == null)
					{
						return false;
					}
					goto IL_32;
				}
				IL_83:
				i++;
				continue;
				IL_32:
				if (array.Length != array2.Length)
				{
					return false;
				}
				for (int j = 0; j < array.Length; j++)
				{
					Type type = array[j];
					Type type2 = array2[j];
					if (type == null)
					{
						if (!(type2 == null))
						{
							return false;
						}
					}
					else
					{
						if (type2 == null)
						{
							return false;
						}
						if (!type.Equals(type2))
						{
							return false;
						}
					}
				}
				goto IL_83;
			}
			return true;
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x00102BD8 File Offset: 0x00100DD8
		public override bool Equals(object obj)
		{
			SignatureHelper signatureHelper = obj as SignatureHelper;
			if (signatureHelper == null)
			{
				return false;
			}
			if (signatureHelper.module != this.module || signatureHelper.returnType != this.returnType || signatureHelper.callConv != this.callConv || signatureHelper.unmanagedCallConv != this.unmanagedCallConv)
			{
				return false;
			}
			if (this.arguments != null)
			{
				if (signatureHelper.arguments == null)
				{
					return false;
				}
				if (this.arguments.Length != signatureHelper.arguments.Length)
				{
					return false;
				}
				for (int i = 0; i < this.arguments.Length; i++)
				{
					if (!signatureHelper.arguments[i].Equals(this.arguments[i]))
					{
						return false;
					}
				}
			}
			else if (signatureHelper.arguments != null)
			{
				return false;
			}
			return SignatureHelper.CompareOK(signatureHelper.modreqs, this.modreqs) && SignatureHelper.CompareOK(signatureHelper.modopts, this.modopts);
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x0600526B RID: 21099
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern byte[] get_signature_local();

		// Token: 0x0600526C RID: 21100
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern byte[] get_signature_field();

		// Token: 0x0600526D RID: 21101 RVA: 0x00102CBC File Offset: 0x00100EBC
		public byte[] GetSignature()
		{
			TypeBuilder.ResolveUserTypes(this.arguments);
			SignatureHelper.SignatureHelperType signatureHelperType = this.type;
			if (signatureHelperType == SignatureHelper.SignatureHelperType.HELPER_FIELD)
			{
				return this.get_signature_field();
			}
			if (signatureHelperType == SignatureHelper.SignatureHelperType.HELPER_LOCAL)
			{
				return this.get_signature_local();
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00102CF5 File Offset: 0x00100EF5
		public override string ToString()
		{
			return "SignatureHelper";
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00102CFC File Offset: 0x00100EFC
		internal static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, CallingConvention unmanagedCallingConvention, Type returnType, Type[] parameters)
		{
			if (mod != null && !(mod is ModuleBuilder))
			{
				throw new ArgumentException("ModuleBuilder is expected");
			}
			if (returnType == null)
			{
				returnType = typeof(void);
			}
			if (returnType.IsUserType)
			{
				throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
			}
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					if (parameters[i].IsUserType)
					{
						throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
					}
				}
			}
			SignatureHelper signatureHelper = new SignatureHelper((ModuleBuilder)mod, SignatureHelper.SignatureHelperType.HELPER_METHOD);
			signatureHelper.returnType = returnType;
			signatureHelper.callConv = callingConvention;
			signatureHelper.unmanagedCallConv = unmanagedCallingConvention;
			if (parameters != null)
			{
				signatureHelper.arguments = new Type[parameters.Length];
				for (int j = 0; j < parameters.Length; j++)
				{
					signatureHelper.arguments[j] = parameters[j];
				}
			}
			return signatureHelper;
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x000479FC File Offset: 0x00045BFC
		void _SignatureHelper.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x000479FC File Offset: 0x00045BFC
		void _SignatureHelper.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x000479FC File Offset: 0x00045BFC
		void _SignatureHelper.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x000479FC File Offset: 0x00045BFC
		void _SignatureHelper.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x000173AD File Offset: 0x000155AD
		internal SignatureHelper()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400330B RID: 13067
		private ModuleBuilder module;

		// Token: 0x0400330C RID: 13068
		private Type[] arguments;

		// Token: 0x0400330D RID: 13069
		private SignatureHelper.SignatureHelperType type;

		// Token: 0x0400330E RID: 13070
		private Type returnType;

		// Token: 0x0400330F RID: 13071
		private CallingConventions callConv;

		// Token: 0x04003310 RID: 13072
		private CallingConvention unmanagedCallConv;

		// Token: 0x04003311 RID: 13073
		private Type[][] modreqs;

		// Token: 0x04003312 RID: 13074
		private Type[][] modopts;

		// Token: 0x02000944 RID: 2372
		internal enum SignatureHelperType
		{
			// Token: 0x04003314 RID: 13076
			HELPER_FIELD,
			// Token: 0x04003315 RID: 13077
			HELPER_LOCAL,
			// Token: 0x04003316 RID: 13078
			HELPER_METHOD,
			// Token: 0x04003317 RID: 13079
			HELPER_PROPERTY
		}
	}
}

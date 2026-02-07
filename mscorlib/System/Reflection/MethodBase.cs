using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Unity;

namespace System.Reflection
{
	// Token: 0x020008AD RID: 2221
	[Serializable]
	public abstract class MethodBase : MemberInfo, _MethodBase
	{
		// Token: 0x06004929 RID: 18729
		public abstract ParameterInfo[] GetParameters();

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x0600492A RID: 18730
		public abstract MethodAttributes Attributes { get; }

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x000EEAE1 File Offset: 0x000ECCE1
		public virtual MethodImplAttributes MethodImplementationFlags
		{
			get
			{
				return this.GetMethodImplementationFlags();
			}
		}

		// Token: 0x0600492C RID: 18732
		public abstract MethodImplAttributes GetMethodImplementationFlags();

		// Token: 0x0600492D RID: 18733 RVA: 0x00084B99 File Offset: 0x00082D99
		public virtual MethodBody GetMethodBody()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x0600492E RID: 18734 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual CallingConventions CallingConvention
		{
			get
			{
				return CallingConventions.Standard;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x0600492F RID: 18735 RVA: 0x000EEAE9 File Offset: 0x000ECCE9
		public bool IsAbstract
		{
			get
			{
				return (this.Attributes & MethodAttributes.Abstract) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06004930 RID: 18736 RVA: 0x000EEAFA File Offset: 0x000ECCFA
		public bool IsConstructor
		{
			get
			{
				return this is ConstructorInfo && !this.IsStatic && (this.Attributes & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06004931 RID: 18737 RVA: 0x000EEB21 File Offset: 0x000ECD21
		public bool IsFinal
		{
			get
			{
				return (this.Attributes & MethodAttributes.Final) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06004932 RID: 18738 RVA: 0x000EEB2F File Offset: 0x000ECD2F
		public bool IsHideBySig
		{
			get
			{
				return (this.Attributes & MethodAttributes.HideBySig) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06004933 RID: 18739 RVA: 0x000EEB40 File Offset: 0x000ECD40
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & MethodAttributes.SpecialName) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06004934 RID: 18740 RVA: 0x000EEB51 File Offset: 0x000ECD51
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & MethodAttributes.Static) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06004935 RID: 18741 RVA: 0x000EEB5F File Offset: 0x000ECD5F
		public bool IsVirtual
		{
			get
			{
				return (this.Attributes & MethodAttributes.Virtual) > MethodAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06004936 RID: 18742 RVA: 0x000EEB6D File Offset: 0x000ECD6D
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06004937 RID: 18743 RVA: 0x000EEB7A File Offset: 0x000ECD7A
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family;
			}
		}

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x000EEB87 File Offset: 0x000ECD87
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004939 RID: 18745 RVA: 0x000EEB94 File Offset: 0x000ECD94
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
			}
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x0600493A RID: 18746 RVA: 0x000EEBA1 File Offset: 0x000ECDA1
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Private;
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x000EEBAE File Offset: 0x000ECDAE
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public;
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x0600493C RID: 18748 RVA: 0x000EEBBB File Offset: 0x000ECDBB
		public virtual bool IsConstructedGenericMethod
		{
			get
			{
				return this.IsGenericMethod && !this.IsGenericMethodDefinition;
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x0600493D RID: 18749 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06004940 RID: 18752 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x000EEBD0 File Offset: 0x000ECDD0
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object Invoke(object obj, object[] parameters)
		{
			return this.Invoke(obj, BindingFlags.Default, null, parameters, null);
		}

		// Token: 0x06004942 RID: 18754
		public abstract object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06004943 RID: 18755
		public abstract RuntimeMethodHandle MethodHandle { get; }

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06004944 RID: 18756 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecurityCritical
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06004945 RID: 18757 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecuritySafeCritical
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06004946 RID: 18758 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecurityTransparent
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x000EE37E File Offset: 0x000EC57E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x000EE387 File Offset: 0x000EC587
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x000EEBE0 File Offset: 0x000ECDE0
		public static bool operator ==(MethodBase left, MethodBase right)
		{
			if (left == right)
			{
				return true;
			}
			if (left == null || right == null)
			{
				return false;
			}
			MethodInfo left2;
			MethodInfo right2;
			if ((left2 = (left as MethodInfo)) != null && (right2 = (right as MethodInfo)) != null)
			{
				return left2 == right2;
			}
			ConstructorInfo left3;
			ConstructorInfo right3;
			return (left3 = (left as ConstructorInfo)) != null && (right3 = (right as ConstructorInfo)) != null && left3 == right3;
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x000EEC4C File Offset: 0x000ECE4C
		public static bool operator !=(MethodBase left, MethodBase right)
		{
			return !(left == right);
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x000EEC58 File Offset: 0x000ECE58
		internal virtual ParameterInfo[] GetParametersInternal()
		{
			return this.GetParameters();
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x000EEC60 File Offset: 0x000ECE60
		internal virtual int GetParametersCount()
		{
			return this.GetParametersInternal().Length;
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Type GetParameterType(int pos)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x000EEC6A File Offset: 0x000ECE6A
		internal virtual int get_next_table_index(object obj, int table, int count)
		{
			if (this is MethodBuilder)
			{
				return ((MethodBuilder)this).get_next_table_index(obj, table, count);
			}
			if (this is ConstructorBuilder)
			{
				return ((ConstructorBuilder)this).get_next_table_index(obj, table, count);
			}
			throw new Exception("Method is not a builder method");
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x000EECA4 File Offset: 0x000ECEA4
		internal virtual string FormatNameAndSig(bool serialization)
		{
			StringBuilder stringBuilder = new StringBuilder(this.Name);
			stringBuilder.Append("(");
			stringBuilder.Append(MethodBase.ConstructParameters(this.GetParameterTypes(), this.CallingConvention, serialization));
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x000EECF4 File Offset: 0x000ECEF4
		internal virtual Type[] GetParameterTypes()
		{
			ParameterInfo[] parametersNoCopy = this.GetParametersNoCopy();
			Type[] array = new Type[parametersNoCopy.Length];
			for (int i = 0; i < parametersNoCopy.Length; i++)
			{
				array[i] = parametersNoCopy[i].ParameterType;
			}
			return array;
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x000EEC58 File Offset: 0x000ECE58
		internal virtual ParameterInfo[] GetParametersNoCopy()
		{
			return this.GetParameters();
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x000EED2C File Offset: 0x000ECF2C
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("The handle is invalid."));
			}
			MethodBase methodFromHandleInternalType = RuntimeMethodInfo.GetMethodFromHandleInternalType(handle.Value, IntPtr.Zero);
			if (methodFromHandleInternalType == null)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			Type declaringType = methodFromHandleInternalType.DeclaringType;
			if (declaringType != null && declaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Cannot resolve method {0} because the declaring type of the method handle {1} is generic. Explicitly provide the declaring type to GetMethodFromHandle."), methodFromHandleInternalType, declaringType.GetGenericTypeDefinition()));
			}
			return methodFromHandleInternalType;
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x000EEDB4 File Offset: 0x000ECFB4
		[ComVisible(false)]
		public static MethodBase GetMethodFromHandle(RuntimeMethodHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.IsNullHandle())
			{
				throw new ArgumentException(Environment.GetResourceString("The handle is invalid."));
			}
			MethodBase methodFromHandleInternalType = RuntimeMethodInfo.GetMethodFromHandleInternalType(handle.Value, declaringType.Value);
			if (methodFromHandleInternalType == null)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			return methodFromHandleInternalType;
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x000EEE04 File Offset: 0x000ED004
		internal static string ConstructParameters(Type[] parameterTypes, CallingConventions callingConvention, bool serialization)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = "";
			foreach (Type type in parameterTypes)
			{
				stringBuilder.Append(value);
				string text = type.FormatTypeName(serialization);
				if (type.IsByRef && !serialization)
				{
					stringBuilder.Append(text.TrimEnd(new char[]
					{
						'&'
					}));
					stringBuilder.Append(" ByRef");
				}
				else
				{
					stringBuilder.Append(text);
				}
				value = ", ";
			}
			if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
			{
				stringBuilder.Append(value);
				stringBuilder.Append("...");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004955 RID: 18773
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern MethodBase GetCurrentMethod();

		// Token: 0x06004956 RID: 18774 RVA: 0x000173AD File Offset: 0x000155AD
		void _MethodBase.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x00052959 File Offset: 0x00050B59
		Type _MethodBase.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x000173AD File Offset: 0x000155AD
		void _MethodBase.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x000173AD File Offset: 0x000155AD
		void _MethodBase.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x000173AD File Offset: 0x000155AD
		void _MethodBase.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}

using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020008AF RID: 2223
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_MethodInfo))]
	[Serializable]
	public abstract class MethodInfo : MethodBase, _MethodInfo
	{
		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x0600495C RID: 18780 RVA: 0x00047F75 File Offset: 0x00046175
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x0600495D RID: 18781 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual ParameterInfo ReturnParameter
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x0600495E RID: 18782 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type ReturnType
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x0004728E File Offset: 0x0004548E
		public override Type[] GetGenericArguments()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x06004960 RID: 18784 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual MethodInfo GetGenericMethodDefinition()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual MethodInfo MakeGenericMethod(params Type[] typeArguments)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x06004962 RID: 18786
		public abstract MethodInfo GetBaseDefinition();

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06004963 RID: 18787
		public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

		// Token: 0x06004964 RID: 18788 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Delegate CreateDelegate(Type delegateType)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Delegate CreateDelegate(Type delegateType, object target)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x000EE1DE File Offset: 0x000EC3DE
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x000EE1E7 File Offset: 0x000EC3E7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(MethodInfo left, MethodInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x000EEE9C File Offset: 0x000ED09C
		public static bool operator !=(MethodInfo left, MethodInfo right)
		{
			return !(left == right);
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x000479FC File Offset: 0x00045BFC
		void _MethodInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x00047214 File Offset: 0x00045414
		Type _MethodInfo.GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x000EEEA8 File Offset: 0x000ED0A8
		internal virtual int GenericParameterCount
		{
			get
			{
				return this.GetGenericArguments().Length;
			}
		}
	}
}

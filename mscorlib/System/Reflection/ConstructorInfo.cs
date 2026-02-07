using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection
{
	// Token: 0x02000895 RID: 2197
	[Serializable]
	public abstract class ConstructorInfo : MethodBase, _ConstructorInfo
	{
		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06004883 RID: 18563 RVA: 0x000040F7 File Offset: 0x000022F7
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x000EE1CE File Offset: 0x000EC3CE
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object Invoke(object[] parameters)
		{
			return this.Invoke(BindingFlags.CreateInstance, null, parameters, null);
		}

		// Token: 0x06004885 RID: 18565
		public abstract object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x06004886 RID: 18566 RVA: 0x000EE1DE File Offset: 0x000EC3DE
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x000EE1E7 File Offset: 0x000EC3E7
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(ConstructorInfo left, ConstructorInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x000EE1EF File Offset: 0x000EC3EF
		public static bool operator !=(ConstructorInfo left, ConstructorInfo right)
		{
			return !(left == right);
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x000173AD File Offset: 0x000155AD
		void _ConstructorInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x00052959 File Offset: 0x00050B59
		Type _ConstructorInfo.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x000173AD File Offset: 0x000155AD
		void _ConstructorInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x000173AD File Offset: 0x000155AD
		void _ConstructorInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x000173AD File Offset: 0x000155AD
		void _ConstructorInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004890 RID: 18576 RVA: 0x00052959 File Offset: 0x00050B59
		object _ConstructorInfo.Invoke_2(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004891 RID: 18577 RVA: 0x00052959 File Offset: 0x00050B59
		object _ConstructorInfo.Invoke_3(object obj, object[] parameters)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004892 RID: 18578 RVA: 0x00052959 File Offset: 0x00050B59
		object _ConstructorInfo.Invoke_4(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x00052959 File Offset: 0x00050B59
		object _ConstructorInfo.Invoke_5(object[] parameters)
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x04002E83 RID: 11907
		public static readonly string ConstructorName = ".ctor";

		// Token: 0x04002E84 RID: 11908
		public static readonly string TypeConstructorName = ".cctor";
	}
}

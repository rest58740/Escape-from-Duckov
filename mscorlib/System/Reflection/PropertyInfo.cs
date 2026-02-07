using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection
{
	// Token: 0x020008BC RID: 2236
	[Serializable]
	public abstract class PropertyInfo : MemberInfo, _PropertyInfo
	{
		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x00026EE5 File Offset: 0x000250E5
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Property;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060049E7 RID: 18919
		public abstract Type PropertyType { get; }

		// Token: 0x060049E8 RID: 18920
		public abstract ParameterInfo[] GetIndexParameters();

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060049E9 RID: 18921
		public abstract PropertyAttributes Attributes { get; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x000EF4BA File Offset: 0x000ED6BA
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & PropertyAttributes.SpecialName) > PropertyAttributes.None;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060049EB RID: 18923
		public abstract bool CanRead { get; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060049EC RID: 18924
		public abstract bool CanWrite { get; }

		// Token: 0x060049ED RID: 18925 RVA: 0x000EF4CB File Offset: 0x000ED6CB
		public MethodInfo[] GetAccessors()
		{
			return this.GetAccessors(false);
		}

		// Token: 0x060049EE RID: 18926
		public abstract MethodInfo[] GetAccessors(bool nonPublic);

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060049EF RID: 18927 RVA: 0x000EF4D4 File Offset: 0x000ED6D4
		public virtual MethodInfo GetMethod
		{
			get
			{
				return this.GetGetMethod(true);
			}
		}

		// Token: 0x060049F0 RID: 18928 RVA: 0x000EF4DD File Offset: 0x000ED6DD
		public MethodInfo GetGetMethod()
		{
			return this.GetGetMethod(false);
		}

		// Token: 0x060049F1 RID: 18929
		public abstract MethodInfo GetGetMethod(bool nonPublic);

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x000EF4E6 File Offset: 0x000ED6E6
		public virtual MethodInfo SetMethod
		{
			get
			{
				return this.GetSetMethod(true);
			}
		}

		// Token: 0x060049F3 RID: 18931 RVA: 0x000EF4EF File Offset: 0x000ED6EF
		public MethodInfo GetSetMethod()
		{
			return this.GetSetMethod(false);
		}

		// Token: 0x060049F4 RID: 18932
		public abstract MethodInfo GetSetMethod(bool nonPublic);

		// Token: 0x060049F5 RID: 18933 RVA: 0x000EF2C2 File Offset: 0x000ED4C2
		public virtual Type[] GetOptionalCustomModifiers()
		{
			return Array.Empty<Type>();
		}

		// Token: 0x060049F6 RID: 18934 RVA: 0x000EF2C2 File Offset: 0x000ED4C2
		public virtual Type[] GetRequiredCustomModifiers()
		{
			return Array.Empty<Type>();
		}

		// Token: 0x060049F7 RID: 18935 RVA: 0x000EF4F8 File Offset: 0x000ED6F8
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object GetValue(object obj)
		{
			return this.GetValue(obj, null);
		}

		// Token: 0x060049F8 RID: 18936 RVA: 0x000EF502 File Offset: 0x000ED702
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual object GetValue(object obj, object[] index)
		{
			return this.GetValue(obj, BindingFlags.Default, null, index, null);
		}

		// Token: 0x060049F9 RID: 18937
		public abstract object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x060049FA RID: 18938 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object GetConstantValue()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060049FB RID: 18939 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object GetRawConstantValue()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060049FC RID: 18940 RVA: 0x000EF50F File Offset: 0x000ED70F
		[DebuggerHidden]
		[DebuggerStepThrough]
		public void SetValue(object obj, object value)
		{
			this.SetValue(obj, value, null);
		}

		// Token: 0x060049FD RID: 18941 RVA: 0x000EF51A File Offset: 0x000ED71A
		[DebuggerHidden]
		[DebuggerStepThrough]
		public virtual void SetValue(object obj, object value, object[] index)
		{
			this.SetValue(obj, value, BindingFlags.Default, null, index, null);
		}

		// Token: 0x060049FE RID: 18942
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x060049FF RID: 18943 RVA: 0x000EE37E File Offset: 0x000EC57E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x06004A00 RID: 18944 RVA: 0x000EE387 File Offset: 0x000EC587
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(PropertyInfo left, PropertyInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x000EF528 File Offset: 0x000ED728
		public static bool operator !=(PropertyInfo left, PropertyInfo right)
		{
			return !(left == right);
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x000173AD File Offset: 0x000155AD
		void _PropertyInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x00052959 File Offset: 0x00050B59
		Type _PropertyInfo.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x000173AD File Offset: 0x000155AD
		void _PropertyInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x000173AD File Offset: 0x000155AD
		void _PropertyInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x000173AD File Offset: 0x000155AD
		void _PropertyInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}

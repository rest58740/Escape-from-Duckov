using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection
{
	// Token: 0x020008AA RID: 2218
	[Serializable]
	public abstract class MemberInfo : ICustomAttributeProvider, _MemberInfo
	{
		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06004911 RID: 18705
		public abstract MemberTypes MemberType { get; }

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06004912 RID: 18706
		public abstract string Name { get; }

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06004913 RID: 18707
		public abstract Type DeclaringType { get; }

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06004914 RID: 18708
		public abstract Type ReflectedType { get; }

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06004915 RID: 18709 RVA: 0x000EE960 File Offset: 0x000ECB60
		public virtual Module Module
		{
			get
			{
				Type type = this as Type;
				if (type != null)
				{
					return type.Module;
				}
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool HasSameMetadataDefinitionAs(MemberInfo other)
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x06004917 RID: 18711
		public abstract bool IsDefined(Type attributeType, bool inherit);

		// Token: 0x06004918 RID: 18712
		public abstract object[] GetCustomAttributes(bool inherit);

		// Token: 0x06004919 RID: 18713
		public abstract object[] GetCustomAttributes(Type attributeType, bool inherit);

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x0600491A RID: 18714 RVA: 0x000EE989 File Offset: 0x000ECB89
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				return this.GetCustomAttributesData();
			}
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x0600491C RID: 18716 RVA: 0x00084B99 File Offset: 0x00082D99
		public virtual int MetadataToken
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x00097E36 File Offset: 0x00096036
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x000930F4 File Offset: 0x000912F4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x000EE994 File Offset: 0x000ECB94
		public static bool operator ==(MemberInfo left, MemberInfo right)
		{
			if (left == right)
			{
				return true;
			}
			if (left == null || right == null)
			{
				return false;
			}
			Type left2;
			Type right2;
			if ((left2 = (left as Type)) != null && (right2 = (right as Type)) != null)
			{
				return left2 == right2;
			}
			MethodBase left3;
			MethodBase right3;
			if ((left3 = (left as MethodBase)) != null && (right3 = (right as MethodBase)) != null)
			{
				return left3 == right3;
			}
			FieldInfo left4;
			FieldInfo right4;
			if ((left4 = (left as FieldInfo)) != null && (right4 = (right as FieldInfo)) != null)
			{
				return left4 == right4;
			}
			EventInfo left5;
			EventInfo right5;
			if ((left5 = (left as EventInfo)) != null && (right5 = (right as EventInfo)) != null)
			{
				return left5 == right5;
			}
			PropertyInfo left6;
			PropertyInfo right6;
			return (left6 = (left as PropertyInfo)) != null && (right6 = (right as PropertyInfo)) != null && left6 == right6;
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x000EEA84 File Offset: 0x000ECC84
		public static bool operator !=(MemberInfo left, MemberInfo right)
		{
			return !(left == right);
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual bool CacheEquals(object o)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x000EEA90 File Offset: 0x000ECC90
		internal bool HasSameMetadataDefinitionAsCore<TOther>(MemberInfo other) where TOther : MemberInfo
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			return other is TOther && this.MetadataToken == other.MetadataToken && this.Module.Equals(other.Module);
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x000173AD File Offset: 0x000155AD
		void _MemberInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00052959 File Offset: 0x00050B59
		Type _MemberInfo.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x000173AD File Offset: 0x000155AD
		void _MemberInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x000173AD File Offset: 0x000155AD
		void _MemberInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x000173AD File Offset: 0x000155AD
		void _MemberInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}

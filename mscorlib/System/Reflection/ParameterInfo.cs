using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using Unity;

namespace System.Reflection
{
	// Token: 0x020008B6 RID: 2230
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ParameterInfo : ICustomAttributeProvider, IObjectReference, _ParameterInfo
	{
		// Token: 0x060049BF RID: 18879 RVA: 0x0000259F File Offset: 0x0000079F
		protected ParameterInfo()
		{
		}

		// Token: 0x17000B8B RID: 2955
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x000EF217 File Offset: 0x000ED417
		public virtual ParameterAttributes Attributes
		{
			get
			{
				return this.AttrsImpl;
			}
		}

		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x000EF21F File Offset: 0x000ED41F
		public virtual MemberInfo Member
		{
			get
			{
				return this.MemberImpl;
			}
		}

		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x000EF227 File Offset: 0x000ED427
		public virtual string Name
		{
			get
			{
				return this.NameImpl;
			}
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x000EF22F File Offset: 0x000ED42F
		public virtual Type ParameterType
		{
			get
			{
				return this.ClassImpl;
			}
		}

		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x000EF237 File Offset: 0x000ED437
		public virtual int Position
		{
			get
			{
				return this.PositionImpl;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x000EF23F File Offset: 0x000ED43F
		public bool IsIn
		{
			get
			{
				return (this.Attributes & ParameterAttributes.In) > ParameterAttributes.None;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x000EF24C File Offset: 0x000ED44C
		public bool IsLcid
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Lcid) > ParameterAttributes.None;
			}
		}

		// Token: 0x17000B92 RID: 2962
		// (get) Token: 0x060049C7 RID: 18887 RVA: 0x000EF259 File Offset: 0x000ED459
		public bool IsOptional
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Optional) > ParameterAttributes.None;
			}
		}

		// Token: 0x17000B93 RID: 2963
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x000EF267 File Offset: 0x000ED467
		public bool IsOut
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Out) > ParameterAttributes.None;
			}
		}

		// Token: 0x17000B94 RID: 2964
		// (get) Token: 0x060049C9 RID: 18889 RVA: 0x000EF274 File Offset: 0x000ED474
		public bool IsRetval
		{
			get
			{
				return (this.Attributes & ParameterAttributes.Retval) > ParameterAttributes.None;
			}
		}

		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object DefaultValue
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060049CB RID: 18891 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual object RawDefaultValue
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool HasDefaultValue
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x000EF281 File Offset: 0x000ED481
		public virtual bool IsDefined(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			return false;
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x000EF298 File Offset: 0x000ED498
		public virtual IEnumerable<CustomAttributeData> CustomAttributes
		{
			get
			{
				return this.GetCustomAttributesData();
			}
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual IList<CustomAttributeData> GetCustomAttributesData()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x000EF2A0 File Offset: 0x000ED4A0
		public virtual object[] GetCustomAttributes(bool inherit)
		{
			return Array.Empty<object>();
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x000EF2A7 File Offset: 0x000ED4A7
		public virtual object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			return Array.Empty<object>();
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x000EF2C2 File Offset: 0x000ED4C2
		public virtual Type[] GetOptionalCustomModifiers()
		{
			return Array.Empty<Type>();
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x000EF2C2 File Offset: 0x000ED4C2
		public virtual Type[] GetRequiredCustomModifiers()
		{
			return Array.Empty<Type>();
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x000EF2C9 File Offset: 0x000ED4C9
		public virtual int MetadataToken
		{
			get
			{
				return 134217728;
			}
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x000EF2D0 File Offset: 0x000ED4D0
		[SecurityCritical]
		public object GetRealObject(StreamingContext context)
		{
			if (this.MemberImpl == null)
			{
				throw new SerializationException("Insufficient state to return the real object.");
			}
			MemberTypes memberType = this.MemberImpl.MemberType;
			if (memberType != MemberTypes.Constructor && memberType != MemberTypes.Method)
			{
				if (memberType != MemberTypes.Property)
				{
					throw new SerializationException("Serialized member does not have a ParameterInfo.");
				}
				ParameterInfo[] array = ((PropertyInfo)this.MemberImpl).GetIndexParameters();
				if (array != null && this.PositionImpl > -1 && this.PositionImpl < array.Length)
				{
					return array[this.PositionImpl];
				}
				throw new SerializationException("Non existent ParameterInfo. Position bigger than member's parameters length.");
			}
			else if (this.PositionImpl == -1)
			{
				if (this.MemberImpl.MemberType == MemberTypes.Method)
				{
					return ((MethodInfo)this.MemberImpl).ReturnParameter;
				}
				throw new SerializationException("Non existent ParameterInfo. Position bigger than member's parameters length.");
			}
			else
			{
				ParameterInfo[] array = ((MethodBase)this.MemberImpl).GetParametersNoCopy();
				if (array != null && this.PositionImpl < array.Length)
				{
					return array[this.PositionImpl];
				}
				throw new SerializationException("Non existent ParameterInfo. Position bigger than member's parameters length.");
			}
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x000EF3C2 File Offset: 0x000ED5C2
		public override string ToString()
		{
			return this.ParameterType.FormatTypeName() + " " + this.Name;
		}

		// Token: 0x060049D7 RID: 18903 RVA: 0x000173AD File Offset: 0x000155AD
		void _ParameterInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049D8 RID: 18904 RVA: 0x000173AD File Offset: 0x000155AD
		void _ParameterInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049D9 RID: 18905 RVA: 0x000173AD File Offset: 0x000155AD
		void _ParameterInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060049DA RID: 18906 RVA: 0x000173AD File Offset: 0x000155AD
		void _ParameterInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002F05 RID: 12037
		protected ParameterAttributes AttrsImpl;

		// Token: 0x04002F06 RID: 12038
		protected Type ClassImpl;

		// Token: 0x04002F07 RID: 12039
		protected object DefaultValueImpl;

		// Token: 0x04002F08 RID: 12040
		protected MemberInfo MemberImpl;

		// Token: 0x04002F09 RID: 12041
		protected string NameImpl;

		// Token: 0x04002F0A RID: 12042
		protected int PositionImpl;

		// Token: 0x04002F0B RID: 12043
		private const int MetadataToken_ParamDef = 134217728;
	}
}

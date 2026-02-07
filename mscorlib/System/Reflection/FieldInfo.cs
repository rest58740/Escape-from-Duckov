using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection
{
	// Token: 0x0200089F RID: 2207
	[Serializable]
	public abstract class FieldInfo : MemberInfo, _FieldInfo
	{
		// Token: 0x17000B40 RID: 2880
		// (get) Token: 0x060048C8 RID: 18632 RVA: 0x0002280B File Offset: 0x00020A0B
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Field;
			}
		}

		// Token: 0x17000B41 RID: 2881
		// (get) Token: 0x060048C9 RID: 18633
		public abstract FieldAttributes Attributes { get; }

		// Token: 0x17000B42 RID: 2882
		// (get) Token: 0x060048CA RID: 18634
		public abstract Type FieldType { get; }

		// Token: 0x17000B43 RID: 2883
		// (get) Token: 0x060048CB RID: 18635 RVA: 0x000EE59A File Offset: 0x000EC79A
		public bool IsInitOnly
		{
			get
			{
				return (this.Attributes & FieldAttributes.InitOnly) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B44 RID: 2884
		// (get) Token: 0x060048CC RID: 18636 RVA: 0x000EE5A8 File Offset: 0x000EC7A8
		public bool IsLiteral
		{
			get
			{
				return (this.Attributes & FieldAttributes.Literal) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x060048CD RID: 18637 RVA: 0x000EE5B6 File Offset: 0x000EC7B6
		public bool IsNotSerialized
		{
			get
			{
				return (this.Attributes & FieldAttributes.NotSerialized) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x060048CE RID: 18638 RVA: 0x000EE5C7 File Offset: 0x000EC7C7
		public bool IsPinvokeImpl
		{
			get
			{
				return (this.Attributes & FieldAttributes.PinvokeImpl) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x060048CF RID: 18639 RVA: 0x000EE5D8 File Offset: 0x000EC7D8
		public bool IsSpecialName
		{
			get
			{
				return (this.Attributes & FieldAttributes.SpecialName) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x060048D0 RID: 18640 RVA: 0x000EE5E9 File Offset: 0x000EC7E9
		public bool IsStatic
		{
			get
			{
				return (this.Attributes & FieldAttributes.Static) > FieldAttributes.PrivateScope;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x000EE5F7 File Offset: 0x000EC7F7
		public bool IsAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x060048D2 RID: 18642 RVA: 0x000EE604 File Offset: 0x000EC804
		public bool IsFamily
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x000EE611 File Offset: 0x000EC811
		public bool IsFamilyAndAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamANDAssem;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x000EE61E File Offset: 0x000EC81E
		public bool IsFamilyOrAssembly
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x060048D5 RID: 18645 RVA: 0x000EE62B File Offset: 0x000EC82B
		public bool IsPrivate
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x060048D6 RID: 18646 RVA: 0x000EE638 File Offset: 0x000EC838
		public bool IsPublic
		{
			get
			{
				return (this.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual bool IsSecurityCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsSecuritySafeCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x060048D9 RID: 18649 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsSecurityTransparent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x060048DA RID: 18650
		public abstract RuntimeFieldHandle FieldHandle { get; }

		// Token: 0x060048DB RID: 18651 RVA: 0x000EE37E File Offset: 0x000EC57E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x000EE387 File Offset: 0x000EC587
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x0006456C File Offset: 0x0006276C
		public static bool operator ==(FieldInfo left, FieldInfo right)
		{
			return left == right || (left != null && right != null && left.Equals(right));
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x000EE645 File Offset: 0x000EC845
		public static bool operator !=(FieldInfo left, FieldInfo right)
		{
			return !(left == right);
		}

		// Token: 0x060048DF RID: 18655
		public abstract object GetValue(object obj);

		// Token: 0x060048E0 RID: 18656 RVA: 0x000EE651 File Offset: 0x000EC851
		[DebuggerHidden]
		[DebuggerStepThrough]
		public void SetValue(object obj, object value)
		{
			this.SetValue(obj, value, BindingFlags.Default, Type.DefaultBinder, null);
		}

		// Token: 0x060048E1 RID: 18657
		public abstract void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture);

		// Token: 0x060048E2 RID: 18658 RVA: 0x000EE662 File Offset: 0x000EC862
		[CLSCompliant(false)]
		public virtual void SetValueDirect(TypedReference obj, object value)
		{
			throw new NotSupportedException("This non-CLS method is not implemented.");
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x000EE662 File Offset: 0x000EC862
		[CLSCompliant(false)]
		public virtual object GetValueDirect(TypedReference obj)
		{
			throw new NotSupportedException("This non-CLS method is not implemented.");
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x000EE662 File Offset: 0x000EC862
		public virtual object GetRawConstantValue()
		{
			throw new NotSupportedException("This non-CLS method is not implemented.");
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type[] GetOptionalCustomModifiers()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual Type[] GetRequiredCustomModifiers()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060048E7 RID: 18663
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern FieldInfo internal_from_handle_type(IntPtr field_handle, IntPtr type_handle);

		// Token: 0x060048E8 RID: 18664 RVA: 0x000EE66E File Offset: 0x000EC86E
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle)
		{
			if (handle.Value == IntPtr.Zero)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			return FieldInfo.internal_from_handle_type(handle.Value, IntPtr.Zero);
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x000EE6A0 File Offset: 0x000EC8A0
		[ComVisible(false)]
		public static FieldInfo GetFieldFromHandle(RuntimeFieldHandle handle, RuntimeTypeHandle declaringType)
		{
			if (handle.Value == IntPtr.Zero)
			{
				throw new ArgumentException("The handle is invalid.");
			}
			FieldInfo fieldInfo = FieldInfo.internal_from_handle_type(handle.Value, declaringType.Value);
			if (fieldInfo == null)
			{
				throw new ArgumentException("The field handle and the type handle are incompatible.");
			}
			return fieldInfo;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x000EE6F2 File Offset: 0x000EC8F2
		internal virtual int GetFieldOffset()
		{
			throw new SystemException("This method should not be called");
		}

		// Token: 0x060048EB RID: 18667
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern MarshalAsAttribute get_marshal_info();

		// Token: 0x060048EC RID: 18668 RVA: 0x000EE700 File Offset: 0x000EC900
		internal object[] GetPseudoCustomAttributes()
		{
			int num = 0;
			if (this.IsNotSerialized)
			{
				num++;
			}
			if (this.DeclaringType.IsExplicitLayout)
			{
				num++;
			}
			MarshalAsAttribute marshal_info = this.get_marshal_info();
			if (marshal_info != null)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			object[] array = new object[num];
			num = 0;
			if (this.IsNotSerialized)
			{
				array[num++] = new NonSerializedAttribute();
			}
			if (this.DeclaringType.IsExplicitLayout)
			{
				array[num++] = new FieldOffsetAttribute(this.GetFieldOffset());
			}
			if (marshal_info != null)
			{
				array[num++] = marshal_info;
			}
			return array;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x000EE788 File Offset: 0x000EC988
		internal CustomAttributeData[] GetPseudoCustomAttributesData()
		{
			int num = 0;
			if (this.IsNotSerialized)
			{
				num++;
			}
			if (this.DeclaringType.IsExplicitLayout)
			{
				num++;
			}
			MarshalAsAttribute marshal_info = this.get_marshal_info();
			if (marshal_info != null)
			{
				num++;
			}
			if (num == 0)
			{
				return null;
			}
			CustomAttributeData[] array = new CustomAttributeData[num];
			num = 0;
			if (this.IsNotSerialized)
			{
				array[num++] = new CustomAttributeData(typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes));
			}
			if (this.DeclaringType.IsExplicitLayout)
			{
				CustomAttributeTypedArgument[] ctorArgs = new CustomAttributeTypedArgument[]
				{
					new CustomAttributeTypedArgument(typeof(int), this.GetFieldOffset())
				};
				array[num++] = new CustomAttributeData(typeof(FieldOffsetAttribute).GetConstructor(new Type[]
				{
					typeof(int)
				}), ctorArgs, EmptyArray<CustomAttributeNamedArgument>.Value);
			}
			if (marshal_info != null)
			{
				CustomAttributeTypedArgument[] ctorArgs2 = new CustomAttributeTypedArgument[]
				{
					new CustomAttributeTypedArgument(typeof(UnmanagedType), marshal_info.Value)
				};
				array[num++] = new CustomAttributeData(typeof(MarshalAsAttribute).GetConstructor(new Type[]
				{
					typeof(UnmanagedType)
				}), ctorArgs2, EmptyArray<CustomAttributeNamedArgument>.Value);
			}
			return array;
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x000173AD File Offset: 0x000155AD
		void _FieldInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x00052959 File Offset: 0x00050B59
		Type _FieldInfo.GetType()
		{
			ThrowStub.ThrowNotSupportedException();
			return null;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x000173AD File Offset: 0x000155AD
		void _FieldInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x000173AD File Offset: 0x000155AD
		void _FieldInfo.GetTypeInfoCount(out uint pcTInfo)
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x000173AD File Offset: 0x000155AD
		void _FieldInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}

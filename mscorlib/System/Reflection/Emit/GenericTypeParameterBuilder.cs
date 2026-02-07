using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000928 RID: 2344
	[ComVisible(true)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class GenericTypeParameterBuilder : TypeInfo
	{
		// Token: 0x06005045 RID: 20549 RVA: 0x000FAF2A File Offset: 0x000F912A
		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			this.base_type = (baseTypeConstraint ?? typeof(object));
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x000FAF41 File Offset: 0x000F9141
		[ComVisible(true)]
		public void SetInterfaceConstraints(params Type[] interfaceConstraints)
		{
			this.iface_constraints = interfaceConstraints;
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x000FAF4A File Offset: 0x000F914A
		public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
		{
			this.attrs = genericParameterAttributes;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x000FAF53 File Offset: 0x000F9153
		internal GenericTypeParameterBuilder(TypeBuilder tbuilder, MethodBuilder mbuilder, string name, int index)
		{
			this.tbuilder = tbuilder;
			this.mbuilder = mbuilder;
			this.name = name;
			this.index = index;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x000FAF78 File Offset: 0x000F9178
		internal override Type InternalResolve()
		{
			if (this.mbuilder != null)
			{
				return MethodBase.GetMethodFromHandle(this.mbuilder.MethodHandleInternal, this.mbuilder.TypeBuilder.InternalResolve().TypeHandle).GetGenericArguments()[this.index];
			}
			return this.tbuilder.InternalResolve().GetGenericArguments()[this.index];
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x000FAFDC File Offset: 0x000F91DC
		internal override Type RuntimeResolve()
		{
			if (this.mbuilder != null)
			{
				return MethodBase.GetMethodFromHandle(this.mbuilder.MethodHandleInternal, this.mbuilder.TypeBuilder.RuntimeResolve().TypeHandle).GetGenericArguments()[this.index];
			}
			return this.tbuilder.RuntimeResolve().GetGenericArguments()[this.index];
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x000FB040 File Offset: 0x000F9240
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			throw this.not_supported();
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x000040F7 File Offset: 0x000022F7
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return TypeAttributes.Public;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x000FB040 File Offset: 0x000F9240
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.not_supported();
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x000FB040 File Offset: 0x000F9240
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x000FB040 File Offset: 0x000F9240
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x000FB040 File Offset: 0x000F9240
		public override EventInfo[] GetEvents()
		{
			throw this.not_supported();
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x000FB040 File Offset: 0x000F9240
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x000FB040 File Offset: 0x000F9240
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x000FB040 File Offset: 0x000F9240
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Type GetInterface(string name, bool ignoreCase)
		{
			throw this.not_supported();
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Type[] GetInterfaces()
		{
			throw this.not_supported();
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x000FB040 File Offset: 0x000F9240
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x000FB040 File Offset: 0x000F9240
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x000FB040 File Offset: 0x000F9240
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x000FB040 File Offset: 0x000F9240
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.not_supported();
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x000FB040 File Offset: 0x000F9240
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			throw this.not_supported();
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x000FB040 File Offset: 0x000F9240
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.not_supported();
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x000FB040 File Offset: 0x000F9240
		public override bool IsAssignableFrom(Type c)
		{
			throw this.not_supported();
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x00057EF9 File Offset: 0x000560F9
		public override bool IsAssignableFrom(TypeInfo typeInfo)
		{
			return !(typeInfo == null) && this.IsAssignableFrom(typeInfo.AsType());
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x000FB040 File Offset: 0x000F9240
		public override bool IsInstanceOfType(object o)
		{
			throw this.not_supported();
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsCOMObjectImpl()
		{
			return false;
		}

		// Token: 0x06005065 RID: 20581 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x06005066 RID: 20582 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x000FB048 File Offset: 0x000F9248
		protected override bool IsValueTypeImpl()
		{
			return this.base_type != null && this.base_type.IsValueType;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x000FB040 File Offset: 0x000F9240
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			throw this.not_supported();
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Type GetElementType()
		{
			throw this.not_supported();
		}

		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x0000270D File Offset: 0x0000090D
		public override Type UnderlyingSystemType
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D37 RID: 3383
		// (get) Token: 0x0600506B RID: 20587 RVA: 0x000FB065 File Offset: 0x000F9265
		public override Assembly Assembly
		{
			get
			{
				return this.tbuilder.Assembly;
			}
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string AssemblyQualifiedName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D39 RID: 3385
		// (get) Token: 0x0600506D RID: 20589 RVA: 0x000FB072 File Offset: 0x000F9272
		public override Type BaseType
		{
			get
			{
				return this.base_type;
			}
		}

		// Token: 0x17000D3A RID: 3386
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string FullName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D3B RID: 3387
		// (get) Token: 0x0600506F RID: 20591 RVA: 0x000FB040 File Offset: 0x000F9240
		public override Guid GUID
		{
			get
			{
				throw this.not_supported();
			}
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x000FB040 File Offset: 0x000F9240
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06005071 RID: 20593 RVA: 0x000FB040 File Offset: 0x000F9240
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06005072 RID: 20594 RVA: 0x000FB040 File Offset: 0x000F9240
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw this.not_supported();
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x000FB040 File Offset: 0x000F9240
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw this.not_supported();
		}

		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x000FB07A File Offset: 0x000F927A
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override string Namespace
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x000FB082 File Offset: 0x000F9282
		public override Module Module
		{
			get
			{
				return this.tbuilder.Module;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x06005077 RID: 20599 RVA: 0x000FB08F File Offset: 0x000F928F
		public override Type DeclaringType
		{
			get
			{
				if (!(this.mbuilder != null))
				{
					return this.tbuilder;
				}
				return this.mbuilder.DeclaringType;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x00058E5D File Offset: 0x0005705D
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06005079 RID: 20601 RVA: 0x000FB040 File Offset: 0x000F9240
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw this.not_supported();
			}
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x00084B99 File Offset: 0x00082D99
		public override Type[] GetGenericArguments()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x00084B99 File Offset: 0x00082D99
		public override Type GetGenericTypeDefinition()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x0600507C RID: 20604 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool ContainsGenericParameters
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600507D RID: 20605 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool IsGenericParameter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x06005080 RID: 20608 RVA: 0x000472CC File Offset: 0x000454CC
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x06005081 RID: 20609 RVA: 0x000FB0B1 File Offset: 0x000F92B1
		public override int GenericParameterPosition
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x00084B99 File Offset: 0x00082D99
		public override Type[] GetGenericParameterConstraints()
		{
			throw new InvalidOperationException();
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x06005083 RID: 20611 RVA: 0x000FB0B9 File Offset: 0x000F92B9
		public override MethodBase DeclaringMethod
		{
			get
			{
				return this.mbuilder;
			}
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x000FB0C4 File Offset: 0x000F92C4
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
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

		// Token: 0x06005085 RID: 20613 RVA: 0x000FB12C File Offset: 0x000F932C
		[MonoTODO("unverified implementation")]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0004DD30 File Offset: 0x0004BF30
		private Exception not_supported()
		{
			return new NotSupportedException();
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x000FB07A File Offset: 0x000F927A
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x000FB13B File Offset: 0x000F933B
		[MonoTODO]
		public override bool Equals(object o)
		{
			return base.Equals(o);
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x000FB144 File Offset: 0x000F9344
		[MonoTODO]
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x000F6535 File Offset: 0x000F4735
		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x000F653E File Offset: 0x000F473E
		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x000F6551 File Offset: 0x000F4751
		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x000FB14C File Offset: 0x000F934C
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			throw new InvalidOperationException(Environment.GetResourceString("{0} is not a GenericTypeDefinition. MakeGenericType may only be called on a type for which Type.IsGenericTypeDefinition is true."));
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x000F6559 File Offset: 0x000F4759
		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x0600508F RID: 20623 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override bool IsUserType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x000173AD File Offset: 0x000155AD
		internal GenericTypeParameterBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04003177 RID: 12663
		private TypeBuilder tbuilder;

		// Token: 0x04003178 RID: 12664
		private MethodBuilder mbuilder;

		// Token: 0x04003179 RID: 12665
		private string name;

		// Token: 0x0400317A RID: 12666
		private int index;

		// Token: 0x0400317B RID: 12667
		private Type base_type;

		// Token: 0x0400317C RID: 12668
		private Type[] iface_constraints;

		// Token: 0x0400317D RID: 12669
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x0400317E RID: 12670
		private GenericParameterAttributes attrs;
	}
}

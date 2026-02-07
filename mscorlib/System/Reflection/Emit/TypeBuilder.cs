using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit
{
	// Token: 0x02000947 RID: 2375
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_TypeBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TypeBuilder : TypeInfo, _TypeBuilder
	{
		// Token: 0x06005284 RID: 21124 RVA: 0x000479FC File Offset: 0x00045BFC
		void _TypeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x000479FC File Offset: 0x00045BFC
		void _TypeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x000479FC File Offset: 0x00045BFC
		void _TypeBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x000479FC File Offset: 0x00045BFC
		void _TypeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x00102EBE File Offset: 0x001010BE
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.attrs;
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x00102EC6 File Offset: 0x001010C6
		private TypeBuilder()
		{
			if (RuntimeType.MakeTypeBuilderInstantiation == null)
			{
				RuntimeType.MakeTypeBuilderInstantiation = new Func<Type, Type[], Type>(TypeBuilderInstantiation.MakeGenericType);
			}
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x00102EE8 File Offset: 0x001010E8
		[PreserveDependency("DoTypeBuilderResolve", "System.AppDomain")]
		internal TypeBuilder(ModuleBuilder mb, TypeAttributes attr, int table_idx) : this()
		{
			this.parent = null;
			this.attrs = attr;
			this.class_size = 0;
			this.table_idx = table_idx;
			this.tname = ((table_idx == 1) ? "<Module>" : ("type_" + table_idx.ToString()));
			this.nspace = string.Empty;
			this.fullname = TypeIdentifiers.WithoutEscape(this.tname);
			this.pmodule = mb;
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x00102F5C File Offset: 0x0010115C
		internal TypeBuilder(ModuleBuilder mb, string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packing_size, int type_size, Type nesting_type) : this()
		{
			this.parent = TypeBuilder.ResolveUserType(parent);
			this.attrs = attr;
			this.class_size = type_size;
			this.packing_size = packing_size;
			this.nesting_type = nesting_type;
			this.check_name("fullname", name);
			if (parent == null && (attr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
			{
				throw new InvalidOperationException("Interface must be declared abstract.");
			}
			int num = name.LastIndexOf('.');
			if (num != -1)
			{
				this.tname = name.Substring(num + 1);
				this.nspace = name.Substring(0, num);
			}
			else
			{
				this.tname = name;
				this.nspace = string.Empty;
			}
			if (interfaces != null)
			{
				this.interfaces = new Type[interfaces.Length];
				Array.Copy(interfaces, this.interfaces, interfaces.Length);
			}
			this.pmodule = mb;
			if ((attr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && parent == null)
			{
				this.parent = typeof(object);
			}
			this.table_idx = mb.get_next_table_index(this, 2, 1);
			this.fullname = this.GetFullName();
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x0600528C RID: 21132 RVA: 0x0010306F File Offset: 0x0010126F
		public override Assembly Assembly
		{
			get
			{
				return this.pmodule.Assembly;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x0600528D RID: 21133 RVA: 0x0010307C File Offset: 0x0010127C
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.fullname.DisplayName + ", " + this.Assembly.FullName;
			}
		}

		// Token: 0x17000DA8 RID: 3496
		// (get) Token: 0x0600528E RID: 21134 RVA: 0x0010309E File Offset: 0x0010129E
		public override Type BaseType
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17000DA9 RID: 3497
		// (get) Token: 0x0600528F RID: 21135 RVA: 0x001030A6 File Offset: 0x001012A6
		public override Type DeclaringType
		{
			get
			{
				return this.nesting_type;
			}
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x001030B0 File Offset: 0x001012B0
		[ComVisible(true)]
		public override bool IsSubclassOf(Type c)
		{
			if (c == null)
			{
				return false;
			}
			if (c == this)
			{
				return false;
			}
			Type baseType = this.parent;
			while (baseType != null)
			{
				if (c == baseType)
				{
					return true;
				}
				baseType = baseType.BaseType;
			}
			return false;
		}

		// Token: 0x17000DAA RID: 3498
		// (get) Token: 0x06005291 RID: 21137 RVA: 0x001030F8 File Offset: 0x001012F8
		public override Type UnderlyingSystemType
		{
			get
			{
				if (this.is_created)
				{
					return this.created.UnderlyingSystemType;
				}
				if (!this.IsEnum)
				{
					return this;
				}
				if (this.underlying_type != null)
				{
					return this.underlying_type;
				}
				throw new InvalidOperationException("Enumeration type is not defined.");
			}
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x00103138 File Offset: 0x00101338
		private TypeName GetFullName()
		{
			TypeIdentifier typeIdentifier = TypeIdentifiers.FromInternal(this.tname);
			if (this.nesting_type != null)
			{
				return TypeNames.FromDisplay(this.nesting_type.FullName).NestedName(typeIdentifier);
			}
			if (this.nspace != null && this.nspace.Length > 0)
			{
				return TypeIdentifiers.FromInternal(this.nspace, typeIdentifier);
			}
			return typeIdentifier;
		}

		// Token: 0x17000DAB RID: 3499
		// (get) Token: 0x06005293 RID: 21139 RVA: 0x0010319A File Offset: 0x0010139A
		public override string FullName
		{
			get
			{
				return this.fullname.DisplayName;
			}
		}

		// Token: 0x17000DAC RID: 3500
		// (get) Token: 0x06005294 RID: 21140 RVA: 0x001031A7 File Offset: 0x001013A7
		public override Guid GUID
		{
			get
			{
				this.check_created();
				return this.created.GUID;
			}
		}

		// Token: 0x17000DAD RID: 3501
		// (get) Token: 0x06005295 RID: 21141 RVA: 0x001031BA File Offset: 0x001013BA
		public override Module Module
		{
			get
			{
				return this.pmodule;
			}
		}

		// Token: 0x17000DAE RID: 3502
		// (get) Token: 0x06005296 RID: 21142 RVA: 0x001031C2 File Offset: 0x001013C2
		public override string Name
		{
			get
			{
				return this.tname;
			}
		}

		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06005297 RID: 21143 RVA: 0x001031CA File Offset: 0x001013CA
		public override string Namespace
		{
			get
			{
				return this.nspace;
			}
		}

		// Token: 0x17000DB0 RID: 3504
		// (get) Token: 0x06005298 RID: 21144 RVA: 0x001031D2 File Offset: 0x001013D2
		public PackingSize PackingSize
		{
			get
			{
				return this.packing_size;
			}
		}

		// Token: 0x17000DB1 RID: 3505
		// (get) Token: 0x06005299 RID: 21145 RVA: 0x001031DA File Offset: 0x001013DA
		public int Size
		{
			get
			{
				return this.class_size;
			}
		}

		// Token: 0x17000DB2 RID: 3506
		// (get) Token: 0x0600529A RID: 21146 RVA: 0x001030A6 File Offset: 0x001012A6
		public override Type ReflectedType
		{
			get
			{
				return this.nesting_type;
			}
		}

		// Token: 0x0600529B RID: 21147 RVA: 0x001031E4 File Offset: 0x001013E4
		public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
		{
			if (pset == null)
			{
				throw new ArgumentNullException("pset");
			}
			if (action == SecurityAction.RequestMinimum || action == SecurityAction.RequestOptional || action == SecurityAction.RequestRefuse)
			{
				throw new ArgumentOutOfRangeException("Request* values are not permitted", "action");
			}
			this.check_not_created();
			if (this.permissions != null)
			{
				RefEmitPermissionSet[] array = this.permissions;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].action == action)
					{
						throw new InvalidOperationException("Multiple permission sets specified with the same SecurityAction.");
					}
				}
				RefEmitPermissionSet[] array2 = new RefEmitPermissionSet[this.permissions.Length + 1];
				this.permissions.CopyTo(array2, 0);
				this.permissions = array2;
			}
			else
			{
				this.permissions = new RefEmitPermissionSet[1];
			}
			this.permissions[this.permissions.Length - 1] = new RefEmitPermissionSet(action, pset.ToXml().ToString());
			this.attrs |= TypeAttributes.HasSecurity;
		}

		// Token: 0x0600529C RID: 21148 RVA: 0x001032C4 File Offset: 0x001014C4
		[ComVisible(true)]
		public void AddInterfaceImplementation(Type interfaceType)
		{
			if (interfaceType == null)
			{
				throw new ArgumentNullException("interfaceType");
			}
			this.check_not_created();
			if (this.interfaces != null)
			{
				Type[] array = this.interfaces;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == interfaceType)
					{
						return;
					}
				}
				Type[] array2 = new Type[this.interfaces.Length + 1];
				this.interfaces.CopyTo(array2, 0);
				array2[this.interfaces.Length] = interfaceType;
				this.interfaces = array2;
				return;
			}
			this.interfaces = new Type[1];
			this.interfaces[0] = interfaceType;
		}

		// Token: 0x0600529D RID: 21149 RVA: 0x0010335C File Offset: 0x0010155C
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			this.check_created();
			if (!(this.created == typeof(object)))
			{
				return this.created.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
			}
			if (this.ctors == null)
			{
				return null;
			}
			ConstructorBuilder constructorBuilder = null;
			int num = 0;
			foreach (ConstructorBuilder constructorBuilder2 in this.ctors)
			{
				if (callConvention == CallingConventions.Any || constructorBuilder2.CallingConvention == callConvention)
				{
					constructorBuilder = constructorBuilder2;
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (types != null)
			{
				MethodBase[] array2 = new MethodBase[num];
				if (num == 1)
				{
					array2[0] = constructorBuilder;
				}
				else
				{
					num = 0;
					foreach (ConstructorBuilder constructorInfo in this.ctors)
					{
						if (callConvention == CallingConventions.Any || constructorInfo.CallingConvention == callConvention)
						{
							array2[num++] = constructorInfo;
						}
					}
				}
				if (binder == null)
				{
					binder = Type.DefaultBinder;
				}
				return (ConstructorInfo)binder.SelectMethod(bindingAttr, array2, types, modifiers);
			}
			if (num > 1)
			{
				throw new AmbiguousMatchException();
			}
			return constructorBuilder;
		}

		// Token: 0x0600529E RID: 21150 RVA: 0x00103457 File Offset: 0x00101657
		[SecuritySafeCritical]
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			if (!this.is_created)
			{
				throw new NotSupportedException();
			}
			return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
		}

		// Token: 0x0600529F RID: 21151 RVA: 0x0010346F File Offset: 0x0010166F
		[SecuritySafeCritical]
		public override object[] GetCustomAttributes(bool inherit)
		{
			this.check_created();
			return this.created.GetCustomAttributes(inherit);
		}

		// Token: 0x060052A0 RID: 21152 RVA: 0x00103483 File Offset: 0x00101683
		[SecuritySafeCritical]
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			this.check_created();
			return this.created.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060052A1 RID: 21153 RVA: 0x00103498 File Offset: 0x00101698
		public TypeBuilder DefineNestedType(string name)
		{
			return this.DefineNestedType(name, TypeAttributes.NestedPrivate, this.pmodule.assemblyb.corlib_object_type, null);
		}

		// Token: 0x060052A2 RID: 21154 RVA: 0x001034B3 File Offset: 0x001016B3
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
		{
			return this.DefineNestedType(name, attr, this.pmodule.assemblyb.corlib_object_type, null);
		}

		// Token: 0x060052A3 RID: 21155 RVA: 0x001034CE File Offset: 0x001016CE
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
		{
			return this.DefineNestedType(name, attr, parent, null);
		}

		// Token: 0x060052A4 RID: 21156 RVA: 0x001034DC File Offset: 0x001016DC
		private TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packSize, int typeSize)
		{
			if (interfaces != null)
			{
				for (int i = 0; i < interfaces.Length; i++)
				{
					if (interfaces[i] == null)
					{
						throw new ArgumentNullException("interfaces");
					}
				}
			}
			TypeBuilder typeBuilder = new TypeBuilder(this.pmodule, name, attr, parent, interfaces, packSize, typeSize, this);
			typeBuilder.fullname = typeBuilder.GetFullName();
			this.pmodule.RegisterTypeName(typeBuilder, typeBuilder.fullname);
			if (this.subtypes != null)
			{
				TypeBuilder[] array = new TypeBuilder[this.subtypes.Length + 1];
				Array.Copy(this.subtypes, array, this.subtypes.Length);
				array[this.subtypes.Length] = typeBuilder;
				this.subtypes = array;
			}
			else
			{
				this.subtypes = new TypeBuilder[1];
				this.subtypes[0] = typeBuilder;
			}
			return typeBuilder;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x0010359E File Offset: 0x0010179E
		[ComVisible(true)]
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			return this.DefineNestedType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001035AD File Offset: 0x001017AD
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
		{
			return this.DefineNestedType(name, attr, parent, null, PackingSize.Unspecified, typeSize);
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x001035BC File Offset: 0x001017BC
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
		{
			return this.DefineNestedType(name, attr, parent, null, packSize, 0);
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x001035CB File Offset: 0x001017CB
		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize, int typeSize)
		{
			return this.DefineNestedType(name, attr, parent, null, packSize, typeSize);
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x001035DB File Offset: 0x001017DB
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
		{
			return this.DefineConstructor(attributes, callingConvention, parameterTypes, null, null);
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001035E8 File Offset: 0x001017E8
		[ComVisible(true)]
		public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			this.check_not_created();
			ConstructorBuilder constructorBuilder = new ConstructorBuilder(this, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
			if (this.ctors != null)
			{
				ConstructorBuilder[] array = new ConstructorBuilder[this.ctors.Length + 1];
				Array.Copy(this.ctors, array, this.ctors.Length);
				array[this.ctors.Length] = constructorBuilder;
				this.ctors = array;
			}
			else
			{
				this.ctors = new ConstructorBuilder[1];
				this.ctors[0] = constructorBuilder;
			}
			return constructorBuilder;
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x00103660 File Offset: 0x00101860
		[ComVisible(true)]
		public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
		{
			Type type;
			if (this.parent != null)
			{
				type = this.parent;
			}
			else
			{
				type = this.pmodule.assemblyb.corlib_object_type;
			}
			Type type2 = type;
			type = type.InternalResolve();
			if (type == typeof(object) || type == typeof(ValueType))
			{
				type = type2;
			}
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			if (constructor == null)
			{
				throw new NotSupportedException("Parent does not have a default constructor. The default constructor must be explicitly defined.");
			}
			ConstructorBuilder constructorBuilder = this.DefineConstructor(attributes, CallingConventions.Standard, Type.EmptyTypes);
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return constructorBuilder;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x0010371C File Offset: 0x0010191C
		private void append_method(MethodBuilder mb)
		{
			if (this.methods != null)
			{
				if (this.methods.Length == this.num_methods)
				{
					MethodBuilder[] destinationArray = new MethodBuilder[this.methods.Length * 2];
					Array.Copy(this.methods, destinationArray, this.num_methods);
					this.methods = destinationArray;
				}
			}
			else
			{
				this.methods = new MethodBuilder[1];
			}
			this.methods[this.num_methods] = mb;
			this.num_methods++;
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x00103794 File Offset: 0x00101994
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x060052AE RID: 21166 RVA: 0x001037A4 File Offset: 0x001019A4
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x060052AF RID: 21167 RVA: 0x001037C4 File Offset: 0x001019C4
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.check_name("name", name);
			this.check_not_created();
			if (base.IsInterface && ((attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope || (attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope) && (attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("Interface method must be abstract and virtual.");
			}
			if (returnType == null)
			{
				returnType = this.pmodule.assemblyb.corlib_void_type;
			}
			MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			this.append_method(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x060052B0 RID: 21168 RVA: 0x00103848 File Offset: 0x00101A48
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
		}

		// Token: 0x060052B1 RID: 21169 RVA: 0x00103870 File Offset: 0x00101A70
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			this.check_name("name", name);
			this.check_name("dllName", dllName);
			this.check_name("entryName", entryName);
			if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("PInvoke methods must be static and native and cannot be abstract.");
			}
			if (base.IsInterface)
			{
				throw new ArgumentException("PInvoke methods cannot exist on interfaces.");
			}
			this.check_not_created();
			MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, dllName, entryName, nativeCallConv, nativeCharSet);
			this.append_method(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x060052B2 RID: 21170 RVA: 0x001038F8 File Offset: 0x00101AF8
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x060052B3 RID: 21171 RVA: 0x00103919 File Offset: 0x00101B19
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
		{
			return this.DefineMethod(name, attributes, CallingConventions.Standard);
		}

		// Token: 0x060052B4 RID: 21172 RVA: 0x00103924 File Offset: 0x00101B24
		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			return this.DefineMethod(name, attributes, callingConvention, null, null);
		}

		// Token: 0x060052B5 RID: 21173 RVA: 0x00103934 File Offset: 0x00101B34
		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			if (methodInfoBody == null)
			{
				throw new ArgumentNullException("methodInfoBody");
			}
			if (methodInfoDeclaration == null)
			{
				throw new ArgumentNullException("methodInfoDeclaration");
			}
			this.check_not_created();
			if (methodInfoBody.DeclaringType != this)
			{
				throw new ArgumentException("method body must belong to this type");
			}
			if (methodInfoBody is MethodBuilder)
			{
				((MethodBuilder)methodInfoBody).set_override(methodInfoDeclaration);
			}
		}

		// Token: 0x060052B6 RID: 21174 RVA: 0x0010399C File Offset: 0x00101B9C
		public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
		{
			return this.DefineField(fieldName, type, null, null, attributes);
		}

		// Token: 0x060052B7 RID: 21175 RVA: 0x001039AC File Offset: 0x00101BAC
		public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			this.check_name("fieldName", fieldName);
			if (type == typeof(void))
			{
				throw new ArgumentException("Bad field type in defining field.");
			}
			this.check_not_created();
			FieldBuilder fieldBuilder = new FieldBuilder(this, fieldName, type, attributes, requiredCustomModifiers, optionalCustomModifiers);
			if (this.fields != null)
			{
				if (this.fields.Length == this.num_fields)
				{
					FieldBuilder[] destinationArray = new FieldBuilder[this.fields.Length * 2];
					Array.Copy(this.fields, destinationArray, this.num_fields);
					this.fields = destinationArray;
				}
				this.fields[this.num_fields] = fieldBuilder;
				this.num_fields++;
			}
			else
			{
				this.fields = new FieldBuilder[1];
				this.fields[0] = fieldBuilder;
				this.num_fields++;
			}
			if (this.IsEnum && this.underlying_type == null && (attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope)
			{
				this.underlying_type = type;
			}
			return fieldBuilder;
		}

		// Token: 0x060052B8 RID: 21176 RVA: 0x00103AA0 File Offset: 0x00101CA0
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineProperty(name, attributes, (CallingConventions)0, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x060052B9 RID: 21177 RVA: 0x00103AC0 File Offset: 0x00101CC0
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineProperty(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x060052BA RID: 21178 RVA: 0x00103AE0 File Offset: 0x00101CE0
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			return this.DefineProperty(name, attributes, (CallingConventions)0, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		}

		// Token: 0x060052BB RID: 21179 RVA: 0x00103B04 File Offset: 0x00101D04
		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			this.check_name("name", name);
			if (parameterTypes != null)
			{
				for (int i = 0; i < parameterTypes.Length; i++)
				{
					if (parameterTypes[i] == null)
					{
						throw new ArgumentNullException("parameterTypes");
					}
				}
			}
			this.check_not_created();
			PropertyBuilder propertyBuilder = new PropertyBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			if (this.properties != null)
			{
				Array.Resize<PropertyBuilder>(ref this.properties, this.properties.Length + 1);
				this.properties[this.properties.Length - 1] = propertyBuilder;
			}
			else
			{
				this.properties = new PropertyBuilder[]
				{
					propertyBuilder
				};
			}
			return propertyBuilder;
		}

		// Token: 0x060052BC RID: 21180 RVA: 0x00103BA6 File Offset: 0x00101DA6
		[ComVisible(true)]
		public ConstructorBuilder DefineTypeInitializer()
		{
			return this.DefineConstructor(MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, null);
		}

		// Token: 0x060052BD RID: 21181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern TypeInfo create_runtime_class();

		// Token: 0x060052BE RID: 21182 RVA: 0x00103BB5 File Offset: 0x00101DB5
		private bool is_nested_in(Type t)
		{
			while (t != null)
			{
				if (t == this)
				{
					return true;
				}
				t = t.DeclaringType;
			}
			return false;
		}

		// Token: 0x060052BF RID: 21183 RVA: 0x00103BD8 File Offset: 0x00101DD8
		private bool has_ctor_method()
		{
			MethodAttributes methodAttributes = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
			for (int i = 0; i < this.num_methods; i++)
			{
				MethodBuilder methodBuilder = this.methods[i];
				if (methodBuilder.Name == ConstructorInfo.ConstructorName && (methodBuilder.Attributes & methodAttributes) == methodAttributes)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060052C0 RID: 21184 RVA: 0x00103C25 File Offset: 0x00101E25
		public Type CreateType()
		{
			return this.CreateTypeInfo();
		}

		// Token: 0x060052C1 RID: 21185 RVA: 0x00103C30 File Offset: 0x00101E30
		public TypeInfo CreateTypeInfo()
		{
			if (this.createTypeCalled)
			{
				return this.created;
			}
			if (!base.IsInterface && this.parent == null && this != this.pmodule.assemblyb.corlib_object_type && this.FullName != "<Module>")
			{
				this.SetParent(this.pmodule.assemblyb.corlib_object_type);
			}
			if (this.fields != null)
			{
				foreach (FieldBuilder fieldBuilder in this.fields)
				{
					if (!(fieldBuilder == null))
					{
						Type fieldType = fieldBuilder.FieldType;
						if (!fieldBuilder.IsStatic && fieldType is TypeBuilder && fieldType.IsValueType && fieldType != this && this.is_nested_in(fieldType))
						{
							TypeBuilder typeBuilder = (TypeBuilder)fieldType;
							if (!typeBuilder.is_created)
							{
								AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder);
								bool is_created = typeBuilder.is_created;
							}
						}
					}
				}
			}
			if (!base.IsInterface && !base.IsValueType && this.ctors == null && this.tname != "<Module>" && ((this.GetAttributeFlagsImpl() & TypeAttributes.Abstract) | TypeAttributes.Sealed) != (TypeAttributes.Abstract | TypeAttributes.Sealed) && !this.has_ctor_method())
			{
				this.DefineDefaultConstructor(MethodAttributes.Public);
			}
			this.createTypeCalled = true;
			if (this.parent != null && this.parent.IsSealed)
			{
				string[] array2 = new string[5];
				array2[0] = "Could not load type '";
				array2[1] = this.FullName;
				array2[2] = "' from assembly '";
				int num = 3;
				Assembly assembly = this.Assembly;
				array2[num] = ((assembly != null) ? assembly.ToString() : null);
				array2[4] = "' because the parent type is sealed.";
				throw new TypeLoadException(string.Concat(array2));
			}
			if (this.parent == this.pmodule.assemblyb.corlib_enum_type && this.methods != null)
			{
				string[] array3 = new string[5];
				array3[0] = "Could not load type '";
				array3[1] = this.FullName;
				array3[2] = "' from assembly '";
				int num2 = 3;
				Assembly assembly2 = this.Assembly;
				array3[num2] = ((assembly2 != null) ? assembly2.ToString() : null);
				array3[4] = "' because it is an enum with methods.";
				throw new TypeLoadException(string.Concat(array3));
			}
			if (this.interfaces != null)
			{
				foreach (Type type in this.interfaces)
				{
					if (type.IsNestedPrivate && type.Assembly != this.Assembly)
					{
						string[] array5 = new string[7];
						array5[0] = "Could not load type '";
						array5[1] = this.FullName;
						array5[2] = "' from assembly '";
						int num3 = 3;
						Assembly assembly3 = this.Assembly;
						array5[num3] = ((assembly3 != null) ? assembly3.ToString() : null);
						array5[4] = "' because it is implements the inaccessible interface '";
						array5[5] = type.FullName;
						array5[6] = "'.";
						throw new TypeLoadException(string.Concat(array5));
					}
				}
			}
			if (this.methods != null)
			{
				bool flag = !base.IsAbstract;
				for (int j = 0; j < this.num_methods; j++)
				{
					MethodBuilder methodBuilder = this.methods[j];
					if (flag && methodBuilder.IsAbstract)
					{
						string str = "Type is concrete but has abstract method ";
						MethodBuilder methodBuilder2 = methodBuilder;
						throw new InvalidOperationException(str + ((methodBuilder2 != null) ? methodBuilder2.ToString() : null));
					}
					methodBuilder.check_override();
					methodBuilder.fixup();
				}
			}
			if (this.ctors != null)
			{
				ConstructorBuilder[] array6 = this.ctors;
				for (int i = 0; i < array6.Length; i++)
				{
					array6[i].fixup();
				}
			}
			this.ResolveUserTypes();
			this.created = this.create_runtime_class();
			if (this.created != null)
			{
				return this.created;
			}
			return this;
		}

		// Token: 0x060052C2 RID: 21186 RVA: 0x00103FAC File Offset: 0x001021AC
		private void ResolveUserTypes()
		{
			this.parent = TypeBuilder.ResolveUserType(this.parent);
			TypeBuilder.ResolveUserTypes(this.interfaces);
			if (this.fields != null)
			{
				foreach (FieldBuilder fieldBuilder in this.fields)
				{
					if (fieldBuilder != null)
					{
						fieldBuilder.ResolveUserTypes();
					}
				}
			}
			if (this.methods != null)
			{
				foreach (MethodBuilder methodBuilder in this.methods)
				{
					if (methodBuilder != null)
					{
						methodBuilder.ResolveUserTypes();
					}
				}
			}
			if (this.ctors != null)
			{
				foreach (ConstructorBuilder constructorBuilder in this.ctors)
				{
					if (constructorBuilder != null)
					{
						constructorBuilder.ResolveUserTypes();
					}
				}
			}
		}

		// Token: 0x060052C3 RID: 21187 RVA: 0x00104070 File Offset: 0x00102270
		internal static void ResolveUserTypes(Type[] types)
		{
			if (types != null)
			{
				for (int i = 0; i < types.Length; i++)
				{
					types[i] = TypeBuilder.ResolveUserType(types[i]);
				}
			}
		}

		// Token: 0x060052C4 RID: 21188 RVA: 0x0010409C File Offset: 0x0010229C
		internal static Type ResolveUserType(Type t)
		{
			if (!(t != null) || (!(t.GetType().Assembly != typeof(int).Assembly) && !(t is TypeDelegator)))
			{
				return t;
			}
			t = t.UnderlyingSystemType;
			if (t != null && (t.GetType().Assembly != typeof(int).Assembly || t is TypeDelegator))
			{
				throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
			}
			return t;
		}

		// Token: 0x060052C5 RID: 21189 RVA: 0x00104124 File Offset: 0x00102324
		internal void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map)
		{
			if (this.methods != null)
			{
				for (int i = 0; i < this.num_methods; i++)
				{
					this.methods[i].FixupTokens(token_map, member_map);
				}
			}
			if (this.ctors != null)
			{
				ConstructorBuilder[] array = this.ctors;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].FixupTokens(token_map, member_map);
				}
			}
			if (this.subtypes != null)
			{
				TypeBuilder[] array2 = this.subtypes;
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].FixupTokens(token_map, member_map);
				}
			}
		}

		// Token: 0x060052C6 RID: 21190 RVA: 0x001041A8 File Offset: 0x001023A8
		internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
		{
			symbolWriter.OpenNamespace(this.Namespace);
			if (this.methods != null)
			{
				for (int i = 0; i < this.num_methods; i++)
				{
					this.methods[i].GenerateDebugInfo(symbolWriter);
				}
			}
			if (this.ctors != null)
			{
				ConstructorBuilder[] array = this.ctors;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].GenerateDebugInfo(symbolWriter);
				}
			}
			symbolWriter.CloseNamespace();
			if (this.subtypes != null)
			{
				for (int k = 0; k < this.subtypes.Length; k++)
				{
					this.subtypes[k].GenerateDebugInfo(symbolWriter);
				}
			}
		}

		// Token: 0x060052C7 RID: 21191 RVA: 0x0010423D File Offset: 0x0010243D
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetConstructors(bindingAttr);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060052C8 RID: 21192 RVA: 0x0010425C File Offset: 0x0010245C
		internal ConstructorInfo[] GetConstructorsInternal(BindingFlags bindingAttr)
		{
			if (this.ctors == null)
			{
				return new ConstructorInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (ConstructorBuilder constructorBuilder in this.ctors)
			{
				bool flag = false;
				MethodAttributes attributes = constructorBuilder.Attributes;
				if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
				{
					if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
					{
						flag = true;
					}
				}
				else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
				{
					flag = true;
				}
				if (flag)
				{
					flag = false;
					if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
					{
						if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						arrayList.Add(constructorBuilder);
					}
				}
			}
			ConstructorInfo[] array2 = new ConstructorInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		// Token: 0x060052C9 RID: 21193 RVA: 0x000472CC File Offset: 0x000454CC
		public override Type GetElementType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060052CA RID: 21194 RVA: 0x001042FD File Offset: 0x001024FD
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetEvent(name, bindingAttr);
		}

		// Token: 0x060052CB RID: 21195 RVA: 0x00047520 File Offset: 0x00045720
		public override EventInfo[] GetEvents()
		{
			return this.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x060052CC RID: 21196 RVA: 0x00104312 File Offset: 0x00102512
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetEvents(bindingAttr);
			}
			throw new NotSupportedException();
		}

		// Token: 0x060052CD RID: 21197 RVA: 0x00104330 File Offset: 0x00102530
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (this.created != null)
			{
				return this.created.GetField(name, bindingAttr);
			}
			if (this.fields == null)
			{
				return null;
			}
			foreach (FieldBuilder fieldInfo in this.fields)
			{
				if (!(fieldInfo == null) && !(fieldInfo.Name != name))
				{
					bool flag = false;
					FieldAttributes attributes = fieldInfo.Attributes;
					if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						flag = false;
						if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							return fieldInfo;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060052CE RID: 21198 RVA: 0x001043DC File Offset: 0x001025DC
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			if (this.created != null)
			{
				return this.created.GetFields(bindingAttr);
			}
			if (this.fields == null)
			{
				return new FieldInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (FieldBuilder fieldInfo in this.fields)
			{
				if (!(fieldInfo == null))
				{
					bool flag = false;
					FieldAttributes attributes = fieldInfo.Attributes;
					if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						flag = false;
						if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							arrayList.Add(fieldInfo);
						}
					}
				}
			}
			FieldInfo[] array2 = new FieldInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		// Token: 0x060052CF RID: 21199 RVA: 0x001044A2 File Offset: 0x001026A2
		public override Type GetInterface(string name, bool ignoreCase)
		{
			this.check_created();
			return this.created.GetInterface(name, ignoreCase);
		}

		// Token: 0x060052D0 RID: 21200 RVA: 0x001044B8 File Offset: 0x001026B8
		public override Type[] GetInterfaces()
		{
			if (this.is_created)
			{
				return this.created.GetInterfaces();
			}
			if (this.interfaces != null)
			{
				Type[] array = new Type[this.interfaces.Length];
				this.interfaces.CopyTo(array, 0);
				return array;
			}
			return Type.EmptyTypes;
		}

		// Token: 0x060052D1 RID: 21201 RVA: 0x00104503 File Offset: 0x00102703
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetMember(name, type, bindingAttr);
		}

		// Token: 0x060052D2 RID: 21202 RVA: 0x00104519 File Offset: 0x00102719
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			this.check_created();
			return this.created.GetMembers(bindingAttr);
		}

		// Token: 0x060052D3 RID: 21203 RVA: 0x00104530 File Offset: 0x00102730
		private MethodInfo[] GetMethodsByName(string name, BindingFlags bindingAttr, bool ignoreCase, Type reflected_type)
		{
			MethodInfo[] array2;
			if ((bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default && this.parent != null)
			{
				MethodInfo[] array = this.parent.GetMethods(bindingAttr);
				ArrayList arrayList = new ArrayList(array.Length);
				bool flag = (bindingAttr & BindingFlags.FlattenHierarchy) > BindingFlags.Default;
				foreach (MethodInfo methodInfo in array)
				{
					MethodAttributes attributes = methodInfo.Attributes;
					if (!methodInfo.IsStatic || flag)
					{
						MethodAttributes methodAttributes = attributes & MethodAttributes.MemberAccessMask;
						bool flag2;
						if (methodAttributes != MethodAttributes.Private)
						{
							if (methodAttributes != MethodAttributes.Assembly)
							{
								if (methodAttributes == MethodAttributes.Public)
								{
									flag2 = ((bindingAttr & BindingFlags.Public) > BindingFlags.Default);
								}
								else
								{
									flag2 = ((bindingAttr & BindingFlags.NonPublic) > BindingFlags.Default);
								}
							}
							else
							{
								flag2 = ((bindingAttr & BindingFlags.NonPublic) > BindingFlags.Default);
							}
						}
						else
						{
							flag2 = false;
						}
						if (flag2)
						{
							arrayList.Add(methodInfo);
						}
					}
				}
				if (this.methods == null)
				{
					array2 = new MethodInfo[arrayList.Count];
					arrayList.CopyTo(array2);
				}
				else
				{
					array2 = new MethodInfo[this.methods.Length + arrayList.Count];
					arrayList.CopyTo(array2, 0);
					this.methods.CopyTo(array2, arrayList.Count);
				}
			}
			else
			{
				MethodInfo[] array3 = this.methods;
				array2 = array3;
			}
			if (array2 == null)
			{
				return new MethodInfo[0];
			}
			ArrayList arrayList2 = new ArrayList();
			foreach (MethodInfo methodInfo2 in array2)
			{
				if (!(methodInfo2 == null) && (name == null || string.Compare(methodInfo2.Name, name, ignoreCase) == 0))
				{
					bool flag2 = false;
					MethodAttributes attributes = methodInfo2.Attributes;
					if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag2 = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag2 = true;
					}
					if (flag2)
					{
						flag2 = false;
						if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag2 = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag2 = true;
						}
						if (flag2)
						{
							arrayList2.Add(methodInfo2);
						}
					}
				}
			}
			MethodInfo[] array4 = new MethodInfo[arrayList2.Count];
			arrayList2.CopyTo(array4);
			return array4;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x001046F0 File Offset: 0x001028F0
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.GetMethodsByName(null, bindingAttr, false, this);
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x001046FC File Offset: 0x001028FC
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			this.check_created();
			if (types == null)
			{
				return this.created.GetMethod(name, bindingAttr);
			}
			return this.created.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x0010472C File Offset: 0x0010292C
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			this.check_created();
			if (this.subtypes == null)
			{
				return null;
			}
			foreach (TypeBuilder typeBuilder in this.subtypes)
			{
				if (typeBuilder.is_created)
				{
					if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
					{
						if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
						{
							goto IL_55;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
					{
						goto IL_55;
					}
					if (typeBuilder.Name == name)
					{
						return typeBuilder.created;
					}
				}
				IL_55:;
			}
			return null;
		}

		// Token: 0x060052D7 RID: 21207 RVA: 0x0010479C File Offset: 0x0010299C
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			if (!this.is_created)
			{
				throw new NotSupportedException();
			}
			ArrayList arrayList = new ArrayList();
			if (this.subtypes == null)
			{
				return Type.EmptyTypes;
			}
			foreach (TypeBuilder typeBuilder in this.subtypes)
			{
				bool flag = false;
				if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
				{
					if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
					{
						flag = true;
					}
				}
				else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
				{
					flag = true;
				}
				if (flag)
				{
					arrayList.Add(typeBuilder);
				}
			}
			Type[] array2 = new Type[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		// Token: 0x060052D8 RID: 21208 RVA: 0x0010482C File Offset: 0x00102A2C
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			if (this.is_created)
			{
				return this.created.GetProperties(bindingAttr);
			}
			if (this.properties == null)
			{
				return new PropertyInfo[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (PropertyBuilder propertyInfo in this.properties)
			{
				bool flag = false;
				MethodInfo methodInfo = propertyInfo.GetGetMethod(true);
				if (methodInfo == null)
				{
					methodInfo = propertyInfo.GetSetMethod(true);
				}
				if (!(methodInfo == null))
				{
					MethodAttributes attributes = methodInfo.Attributes;
					if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
					{
						if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
						{
							flag = true;
						}
					}
					else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
					{
						flag = true;
					}
					if (flag)
					{
						flag = false;
						if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
						{
							if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
							{
								flag = true;
							}
						}
						else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
						{
							flag = true;
						}
						if (flag)
						{
							arrayList.Add(propertyInfo);
						}
					}
				}
			}
			PropertyInfo[] array2 = new PropertyInfo[arrayList.Count];
			arrayList.CopyTo(array2);
			return array2;
		}

		// Token: 0x060052D9 RID: 21209 RVA: 0x0010490B File Offset: 0x00102B0B
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			throw this.not_supported();
		}

		// Token: 0x060052DA RID: 21210 RVA: 0x00104913 File Offset: 0x00102B13
		protected override bool HasElementTypeImpl()
		{
			return this.is_created && this.created.HasElementType;
		}

		// Token: 0x060052DB RID: 21211 RVA: 0x0010492C File Offset: 0x00102B2C
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			this.check_created();
			return this.created.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x060052DC RID: 21212 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsArrayImpl()
		{
			return false;
		}

		// Token: 0x060052DD RID: 21213 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsByRefImpl()
		{
			return false;
		}

		// Token: 0x060052DE RID: 21214 RVA: 0x00047306 File Offset: 0x00045506
		protected override bool IsCOMObjectImpl()
		{
			return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) > TypeAttributes.NotPublic;
		}

		// Token: 0x060052DF RID: 21215 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPointerImpl()
		{
			return false;
		}

		// Token: 0x060052E0 RID: 21216 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		protected override bool IsPrimitiveImpl()
		{
			return false;
		}

		// Token: 0x060052E1 RID: 21217 RVA: 0x00104958 File Offset: 0x00102B58
		protected override bool IsValueTypeImpl()
		{
			if (this == this.pmodule.assemblyb.corlib_value_type || this == this.pmodule.assemblyb.corlib_enum_type)
			{
				return false;
			}
			Type baseType = this.parent;
			while (baseType != null)
			{
				if (baseType == this.pmodule.assemblyb.corlib_value_type)
				{
					return true;
				}
				baseType = baseType.BaseType;
			}
			return false;
		}

		// Token: 0x060052E2 RID: 21218 RVA: 0x000F6535 File Offset: 0x000F4735
		public override Type MakeArrayType()
		{
			return new ArrayType(this, 0);
		}

		// Token: 0x060052E3 RID: 21219 RVA: 0x000F653E File Offset: 0x000F473E
		public override Type MakeArrayType(int rank)
		{
			if (rank < 1)
			{
				throw new IndexOutOfRangeException();
			}
			return new ArrayType(this, rank);
		}

		// Token: 0x060052E4 RID: 21220 RVA: 0x000F6551 File Offset: 0x000F4751
		public override Type MakeByRefType()
		{
			return new ByRefType(this);
		}

		// Token: 0x060052E5 RID: 21221 RVA: 0x001049CC File Offset: 0x00102BCC
		public override Type MakeGenericType(params Type[] typeArguments)
		{
			if (!this.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException("not a generic type definition");
			}
			if (typeArguments == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
			if (this.generic_params.Length != typeArguments.Length)
			{
				throw new ArgumentException(string.Format("The type or method has {0} generic parameter(s) but {1} generic argument(s) where provided. A generic argument must be provided for each generic parameter.", this.generic_params.Length, typeArguments.Length), "typeArguments");
			}
			for (int i = 0; i < typeArguments.Length; i++)
			{
				if (typeArguments[i] == null)
				{
					throw new ArgumentNullException("typeArguments");
				}
			}
			Type[] array = new Type[typeArguments.Length];
			typeArguments.CopyTo(array, 0);
			return this.pmodule.assemblyb.MakeGenericType(this, array);
		}

		// Token: 0x060052E6 RID: 21222 RVA: 0x000F6559 File Offset: 0x000F4759
		public override Type MakePointerType()
		{
			return new PointerType(this);
		}

		// Token: 0x17000DB3 RID: 3507
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x00104A7A File Offset: 0x00102C7A
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				this.check_created();
				return this.created.TypeHandle;
			}
		}

		// Token: 0x060052E8 RID: 21224 RVA: 0x00104A90 File Offset: 0x00102C90
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			string fullName = customBuilder.Ctor.ReflectedType.FullName;
			if (fullName == "System.Runtime.InteropServices.StructLayoutAttribute")
			{
				byte[] data = customBuilder.Data;
				int num = (int)data[2] | (int)data[3] << 8;
				this.attrs &= ~TypeAttributes.LayoutMask;
				switch (num)
				{
				case 0:
					this.attrs |= TypeAttributes.SequentialLayout;
					goto IL_A5;
				case 2:
					this.attrs |= TypeAttributes.ExplicitLayout;
					goto IL_A5;
				case 3:
					this.attrs |= TypeAttributes.NotPublic;
					goto IL_A5;
				}
				throw new Exception("Error in customattr");
				IL_A5:
				Type type = (customBuilder.Ctor is ConstructorBuilder) ? ((ConstructorBuilder)customBuilder.Ctor).parameters[0] : customBuilder.Ctor.GetParametersInternal()[0].ParameterType;
				int num2 = 6;
				if (type.FullName == "System.Int16")
				{
					num2 = 4;
				}
				int num3 = (int)data[num2++];
				num3 |= (int)data[num2++] << 8;
				for (int i = 0; i < num3; i++)
				{
					num2++;
					int num4;
					if (data[num2++] == 85)
					{
						num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
						CustomAttributeBuilder.string_from_bytes(data, num2, num4);
						num2 += num4;
					}
					num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
					string a = CustomAttributeBuilder.string_from_bytes(data, num2, num4);
					num2 += num4;
					int num5 = (int)data[num2++];
					num5 |= (int)data[num2++] << 8;
					num5 |= (int)data[num2++] << 16;
					num5 |= (int)data[num2++] << 24;
					if (!(a == "CharSet"))
					{
						if (!(a == "Pack"))
						{
							if (a == "Size")
							{
								this.class_size = num5;
							}
						}
						else
						{
							this.packing_size = (PackingSize)num5;
						}
					}
					else
					{
						switch (num5)
						{
						case 1:
						case 2:
							this.attrs &= ~TypeAttributes.StringFormatMask;
							break;
						case 3:
							this.attrs &= ~TypeAttributes.AutoClass;
							this.attrs |= TypeAttributes.UnicodeClass;
							break;
						case 4:
							this.attrs &= ~TypeAttributes.UnicodeClass;
							this.attrs |= TypeAttributes.AutoClass;
							break;
						}
					}
				}
				return;
			}
			if (fullName == "System.Runtime.CompilerServices.SpecialNameAttribute")
			{
				this.attrs |= TypeAttributes.SpecialName;
				return;
			}
			if (fullName == "System.SerializableAttribute")
			{
				this.attrs |= TypeAttributes.Serializable;
				return;
			}
			if (fullName == "System.Runtime.InteropServices.ComImportAttribute")
			{
				this.attrs |= TypeAttributes.Import;
				return;
			}
			if (fullName == "System.Security.SuppressUnmanagedCodeSecurityAttribute")
			{
				this.attrs |= TypeAttributes.HasSecurity;
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

		// Token: 0x060052E9 RID: 21225 RVA: 0x00104DC4 File Offset: 0x00102FC4
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x00104DD4 File Offset: 0x00102FD4
		public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
		{
			this.check_name("name", name);
			if (eventtype == null)
			{
				throw new ArgumentNullException("type");
			}
			this.check_not_created();
			EventBuilder eventBuilder = new EventBuilder(this, name, attributes, eventtype);
			if (this.events != null)
			{
				EventBuilder[] array = new EventBuilder[this.events.Length + 1];
				Array.Copy(this.events, array, this.events.Length);
				array[this.events.Length] = eventBuilder;
				this.events = array;
			}
			else
			{
				this.events = new EventBuilder[1];
				this.events[0] = eventBuilder;
			}
			return eventBuilder;
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x00104E67 File Offset: 0x00103067
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			FieldBuilder fieldBuilder = this.DefineUninitializedData(name, data.Length, attributes);
			fieldBuilder.SetRVAData(data);
			return fieldBuilder;
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x00104E8C File Offset: 0x0010308C
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal", "name");
			}
			if (size <= 0 || size > 4128768)
			{
				throw new ArgumentException("Data size must be > 0 and < 0x3f0000");
			}
			this.check_not_created();
			string text = "$ArrayType$" + size.ToString();
			TypeIdentifier innerName = TypeIdentifiers.WithoutEscape(text);
			Type type = this.pmodule.GetRegisteredType(this.fullname.NestedName(innerName));
			if (type == null)
			{
				TypeBuilder typeBuilder = this.DefineNestedType(text, TypeAttributes.Public | TypeAttributes.NestedPublic | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, this.pmodule.assemblyb.corlib_value_type, null, PackingSize.Size1, size);
				typeBuilder.CreateType();
				type = typeBuilder;
			}
			return this.DefineField(name, type, attributes | FieldAttributes.Static | FieldAttributes.HasFieldRVA);
		}

		// Token: 0x17000DB4 RID: 3508
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x00104F4F File Offset: 0x0010314F
		public TypeToken TypeToken
		{
			get
			{
				return new TypeToken(33554432 | this.table_idx);
			}
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x00104F64 File Offset: 0x00103164
		public void SetParent(Type parent)
		{
			this.check_not_created();
			if (parent == null)
			{
				if ((this.attrs & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic)
				{
					if ((this.attrs & TypeAttributes.Abstract) == TypeAttributes.NotPublic)
					{
						throw new InvalidOperationException("Interface must be declared abstract.");
					}
					this.parent = null;
				}
				else
				{
					this.parent = typeof(object);
				}
			}
			else
			{
				this.parent = parent;
			}
			this.parent = TypeBuilder.ResolveUserType(this.parent);
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x00104FD7 File Offset: 0x001031D7
		internal int get_next_table_index(object obj, int table, int count)
		{
			return this.pmodule.get_next_table_index(obj, table, count);
		}

		// Token: 0x060052F0 RID: 21232 RVA: 0x00104FE7 File Offset: 0x001031E7
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			if (this.created == null)
			{
				throw new NotSupportedException("This method is not implemented for incomplete types.");
			}
			return this.created.GetInterfaceMap(interfaceType);
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x0010500E File Offset: 0x0010320E
		internal override Type InternalResolve()
		{
			this.check_created();
			return this.created;
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x0010500E File Offset: 0x0010320E
		internal override Type RuntimeResolve()
		{
			this.check_created();
			return this.created;
		}

		// Token: 0x17000DB5 RID: 3509
		// (get) Token: 0x060052F3 RID: 21235 RVA: 0x0010501C File Offset: 0x0010321C
		internal bool is_created
		{
			get
			{
				return this.createTypeCalled;
			}
		}

		// Token: 0x060052F4 RID: 21236 RVA: 0x000F79CE File Offset: 0x000F5BCE
		private Exception not_supported()
		{
			return new NotSupportedException("The invoked member is not supported in a dynamic module.");
		}

		// Token: 0x060052F5 RID: 21237 RVA: 0x00105024 File Offset: 0x00103224
		private void check_not_created()
		{
			if (this.is_created)
			{
				throw new InvalidOperationException("Unable to change after type has been created.");
			}
		}

		// Token: 0x060052F6 RID: 21238 RVA: 0x00105039 File Offset: 0x00103239
		private void check_created()
		{
			if (!this.is_created)
			{
				throw this.not_supported();
			}
		}

		// Token: 0x060052F7 RID: 21239 RVA: 0x0010504A File Offset: 0x0010324A
		private void check_name(string argName, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException(argName);
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Empty name is not legal", argName);
			}
			if (name[0] == '\0')
			{
				throw new ArgumentException("Illegal name", argName);
			}
		}

		// Token: 0x060052F8 RID: 21240 RVA: 0x0010507F File Offset: 0x0010327F
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x060052F9 RID: 21241 RVA: 0x00105087 File Offset: 0x00103287
		[MonoTODO]
		public override bool IsAssignableFrom(Type c)
		{
			return base.IsAssignableFrom(c);
		}

		// Token: 0x060052FA RID: 21242 RVA: 0x00105090 File Offset: 0x00103290
		[MonoTODO("arrays")]
		internal bool IsAssignableTo(Type c)
		{
			if (c == this)
			{
				return true;
			}
			if (c.IsInterface)
			{
				if (this.parent != null && this.is_created && c.IsAssignableFrom(this.parent))
				{
					return true;
				}
				if (this.interfaces == null)
				{
					return false;
				}
				foreach (Type c2 in this.interfaces)
				{
					if (c.IsAssignableFrom(c2))
					{
						return true;
					}
				}
				if (!this.is_created)
				{
					return false;
				}
			}
			if (this.parent == null)
			{
				return c == typeof(object);
			}
			return c.IsAssignableFrom(this.parent);
		}

		// Token: 0x060052FB RID: 21243 RVA: 0x00105139 File Offset: 0x00103339
		public bool IsCreated()
		{
			return this.is_created;
		}

		// Token: 0x060052FC RID: 21244 RVA: 0x00105144 File Offset: 0x00103344
		public override Type[] GetGenericArguments()
		{
			if (this.generic_params == null)
			{
				return null;
			}
			Type[] array = new Type[this.generic_params.Length];
			this.generic_params.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060052FD RID: 21245 RVA: 0x00105177 File Offset: 0x00103377
		public override Type GetGenericTypeDefinition()
		{
			if (this.generic_params == null)
			{
				throw new InvalidOperationException("Type is not generic");
			}
			return this;
		}

		// Token: 0x17000DB6 RID: 3510
		// (get) Token: 0x060052FE RID: 21246 RVA: 0x0010518D File Offset: 0x0010338D
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.generic_params != null;
			}
		}

		// Token: 0x17000DB7 RID: 3511
		// (get) Token: 0x060052FF RID: 21247 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DB8 RID: 3512
		// (get) Token: 0x06005300 RID: 21248 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				return GenericParameterAttributes.None;
			}
		}

		// Token: 0x17000DB9 RID: 3513
		// (get) Token: 0x06005301 RID: 21249 RVA: 0x0010518D File Offset: 0x0010338D
		public override bool IsGenericTypeDefinition
		{
			get
			{
				return this.generic_params != null;
			}
		}

		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x00105198 File Offset: 0x00103398
		public override bool IsGenericType
		{
			get
			{
				return this.IsGenericTypeDefinition;
			}
		}

		// Token: 0x17000DBB RID: 3515
		// (get) Token: 0x06005303 RID: 21251 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		[MonoTODO]
		public override int GenericParameterPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000DBC RID: 3516
		// (get) Token: 0x06005304 RID: 21252 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005305 RID: 21253 RVA: 0x001051A0 File Offset: 0x001033A0
		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			if (names == null)
			{
				throw new ArgumentNullException("names");
			}
			if (names.Length == 0)
			{
				throw new ArgumentException("names");
			}
			this.generic_params = new GenericTypeParameterBuilder[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				string text = names[i];
				if (text == null)
				{
					throw new ArgumentNullException("names");
				}
				this.generic_params[i] = new GenericTypeParameterBuilder(this, null, text, i);
			}
			return this.generic_params;
		}

		// Token: 0x06005306 RID: 21254 RVA: 0x00105210 File Offset: 0x00103410
		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			if (type == null)
			{
				throw new ArgumentException("Type is not generic", "type");
			}
			if (!type.IsGenericType)
			{
				throw new ArgumentException("Type is not a generic type", "type");
			}
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type cannot be a generic type definition", "type");
			}
			if (constructor == null)
			{
				throw new NullReferenceException();
			}
			if (!constructor.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("constructor declaring type is not a generic type definition", "constructor");
			}
			if (constructor.DeclaringType != type.GetGenericTypeDefinition())
			{
				throw new ArgumentException("constructor declaring type is not the generic type definition of type", "constructor");
			}
			ConstructorInfo constructor2 = type.GetConstructor(constructor);
			if (constructor2 == null)
			{
				throw new ArgumentException("constructor not found");
			}
			return constructor2;
		}

		// Token: 0x06005307 RID: 21255 RVA: 0x001052D0 File Offset: 0x001034D0
		private static bool IsValidGetMethodType(Type type)
		{
			if (type is TypeBuilder || type is TypeBuilderInstantiation)
			{
				return true;
			}
			if (type.Module is ModuleBuilder)
			{
				return true;
			}
			if (type.IsGenericParameter)
			{
				return false;
			}
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments == null)
			{
				return false;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (TypeBuilder.IsValidGetMethodType(genericArguments[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005308 RID: 21256 RVA: 0x00105330 File Offset: 0x00103530
		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			if (!TypeBuilder.IsValidGetMethodType(type))
			{
				string str = "type is not TypeBuilder but ";
				Type type2 = type.GetType();
				throw new ArgumentException(str + ((type2 != null) ? type2.ToString() : null), "type");
			}
			if (type is TypeBuilder && type.ContainsGenericParameters)
			{
				type = type.MakeGenericType(type.GetGenericArguments());
			}
			if (!type.IsGenericType)
			{
				throw new ArgumentException("type is not a generic type", "type");
			}
			if (!method.DeclaringType.IsGenericTypeDefinition)
			{
				throw new ArgumentException("method declaring type is not a generic type definition", "method");
			}
			if (method.DeclaringType != type.GetGenericTypeDefinition())
			{
				throw new ArgumentException("method declaring type is not the generic type definition of type", "method");
			}
			if (method == null)
			{
				throw new NullReferenceException();
			}
			MethodInfo method2 = type.GetMethod(method);
			if (method2 == null)
			{
				throw new ArgumentException(string.Format("method {0} not found in type {1}", method.Name, type));
			}
			return method2;
		}

		// Token: 0x06005309 RID: 21257 RVA: 0x00105418 File Offset: 0x00103618
		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			if (!type.IsGenericType)
			{
				throw new ArgumentException("Type is not a generic type", "type");
			}
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type cannot be a generic type definition", "type");
			}
			if (field is FieldOnTypeBuilderInst)
			{
				throw new ArgumentException("The specified field must be declared on a generic type definition.", "field");
			}
			if (field.DeclaringType != type.GetGenericTypeDefinition())
			{
				throw new ArgumentException("field declaring type is not the generic type definition of type", "method");
			}
			FieldInfo field2 = type.GetField(field);
			if (field2 == null)
			{
				throw new Exception("field not found");
			}
			return field2;
		}

		// Token: 0x17000DBD RID: 3517
		// (get) Token: 0x0600530A RID: 21258 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal override bool IsUserType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DBE RID: 3518
		// (get) Token: 0x0600530B RID: 21259 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsConstructedGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600530C RID: 21260 RVA: 0x000FA62D File Offset: 0x000F882D
		public override bool IsAssignableFrom(TypeInfo typeInfo)
		{
			return base.IsAssignableFrom(typeInfo);
		}

		// Token: 0x0600530D RID: 21261 RVA: 0x001054B0 File Offset: 0x001036B0
		internal static bool SetConstantValue(Type destType, object value, ref object destValue)
		{
			if (value != null)
			{
				Type type = value.GetType();
				if (destType.IsByRef)
				{
					destType = destType.GetElementType();
				}
				destType = (Nullable.GetUnderlyingType(destType) ?? destType);
				if (destType.IsEnum)
				{
					EnumBuilder enumBuilder;
					Type type2;
					TypeBuilder typeBuilder;
					if ((enumBuilder = (destType as EnumBuilder)) != null)
					{
						type2 = enumBuilder.GetEnumUnderlyingType();
						if ((!enumBuilder.GetTypeBuilder().is_created || !(type == enumBuilder.GetTypeBuilder().created)) && !(type == type2))
						{
							TypeBuilder.throw_argument_ConstantDoesntMatch();
						}
					}
					else if ((typeBuilder = (destType as TypeBuilder)) != null)
					{
						type2 = typeBuilder.underlying_type;
						if (type2 == null || (type != typeBuilder.UnderlyingSystemType && type != type2))
						{
							TypeBuilder.throw_argument_ConstantDoesntMatch();
						}
					}
					else
					{
						type2 = Enum.GetUnderlyingType(destType);
						if (type != destType && type != type2)
						{
							TypeBuilder.throw_argument_ConstantDoesntMatch();
						}
					}
					type = type2;
				}
				else if (!destType.IsAssignableFrom(type))
				{
					TypeBuilder.throw_argument_ConstantDoesntMatch();
				}
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
				case TypeCode.Int64:
				case TypeCode.UInt64:
				case TypeCode.Single:
				case TypeCode.Double:
					destValue = value;
					return true;
				case TypeCode.DateTime:
				{
					long ticks = ((DateTime)value).Ticks;
					destValue = ticks;
					return true;
				}
				case TypeCode.String:
					destValue = value;
					return true;
				}
				throw new ArgumentException(type.ToString() + " is not a supported constant type.");
			}
			destValue = null;
			return true;
		}

		// Token: 0x0600530E RID: 21262 RVA: 0x0010563B File Offset: 0x0010383B
		private static void throw_argument_ConstantDoesntMatch()
		{
			throw new ArgumentException("Constant does not match the defined type.");
		}

		// Token: 0x17000DBF RID: 3519
		// (get) Token: 0x0600530F RID: 21263 RVA: 0x000040F7 File Offset: 0x000022F7
		public override bool IsTypeDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400331B RID: 13083
		private string tname;

		// Token: 0x0400331C RID: 13084
		private string nspace;

		// Token: 0x0400331D RID: 13085
		private Type parent;

		// Token: 0x0400331E RID: 13086
		private Type nesting_type;

		// Token: 0x0400331F RID: 13087
		internal Type[] interfaces;

		// Token: 0x04003320 RID: 13088
		internal int num_methods;

		// Token: 0x04003321 RID: 13089
		internal MethodBuilder[] methods;

		// Token: 0x04003322 RID: 13090
		internal ConstructorBuilder[] ctors;

		// Token: 0x04003323 RID: 13091
		internal PropertyBuilder[] properties;

		// Token: 0x04003324 RID: 13092
		internal int num_fields;

		// Token: 0x04003325 RID: 13093
		internal FieldBuilder[] fields;

		// Token: 0x04003326 RID: 13094
		internal EventBuilder[] events;

		// Token: 0x04003327 RID: 13095
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x04003328 RID: 13096
		internal TypeBuilder[] subtypes;

		// Token: 0x04003329 RID: 13097
		internal TypeAttributes attrs;

		// Token: 0x0400332A RID: 13098
		private int table_idx;

		// Token: 0x0400332B RID: 13099
		private ModuleBuilder pmodule;

		// Token: 0x0400332C RID: 13100
		private int class_size;

		// Token: 0x0400332D RID: 13101
		private PackingSize packing_size;

		// Token: 0x0400332E RID: 13102
		private IntPtr generic_container;

		// Token: 0x0400332F RID: 13103
		private GenericTypeParameterBuilder[] generic_params;

		// Token: 0x04003330 RID: 13104
		private RefEmitPermissionSet[] permissions;

		// Token: 0x04003331 RID: 13105
		private TypeInfo created;

		// Token: 0x04003332 RID: 13106
		private int state;

		// Token: 0x04003333 RID: 13107
		private TypeName fullname;

		// Token: 0x04003334 RID: 13108
		private bool createTypeCalled;

		// Token: 0x04003335 RID: 13109
		private Type underlying_type;

		// Token: 0x04003336 RID: 13110
		public const int UnspecifiedTypeSize = 0;
	}
}

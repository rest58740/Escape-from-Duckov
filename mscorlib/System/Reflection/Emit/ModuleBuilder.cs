using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity;

namespace System.Reflection.Emit
{
	// Token: 0x02000938 RID: 2360
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_ModuleBuilder))]
	[ClassInterface(ClassInterfaceType.None)]
	[StructLayout(LayoutKind.Sequential)]
	public class ModuleBuilder : Module, _ModuleBuilder
	{
		// Token: 0x06005178 RID: 20856 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ModuleBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005179 RID: 20857 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ModuleBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600517A RID: 20858 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ModuleBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600517B RID: 20859 RVA: 0x000479FC File Offset: 0x00045BFC
		void _ModuleBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600517C RID: 20860
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void basic_init(ModuleBuilder ab);

		// Token: 0x0600517D RID: 20861
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_wrappers_type(ModuleBuilder mb, Type ab);

		// Token: 0x0600517E RID: 20862 RVA: 0x000FE9FC File Offset: 0x000FCBFC
		internal ModuleBuilder(AssemblyBuilder assb, string name, string fullyqname, bool emitSymbolInfo, bool transient)
		{
			this.scopename = name;
			this.name = name;
			this.fqname = fullyqname;
			this.assemblyb = assb;
			this.assembly = assb;
			this.transient = transient;
			this.guid = Guid.FastNewGuidArray();
			this.table_idx = this.get_next_table_index(this, 0, 1);
			this.name_cache = new Dictionary<TypeName, TypeBuilder>();
			this.us_string_cache = new Dictionary<string, int>(512);
			ModuleBuilder.basic_init(this);
			this.CreateGlobalType();
			if (assb.IsRun)
			{
				Type ab = new TypeBuilder(this, TypeAttributes.Abstract, 16777215).CreateType();
				ModuleBuilder.set_wrappers_type(this, ab);
			}
			if (emitSymbolInfo)
			{
				Assembly assembly = Assembly.LoadWithPartialName("Mono.CompilerServices.SymbolWriter");
				Type type = null;
				if (assembly != null)
				{
					type = assembly.GetType("Mono.CompilerServices.SymbolWriter.SymbolWriterImpl");
				}
				if (type == null)
				{
					ModuleBuilder.WarnAboutSymbolWriter("Failed to load the default Mono.CompilerServices.SymbolWriter assembly");
				}
				else
				{
					try
					{
						this.symbolWriter = (ISymbolWriter)Activator.CreateInstance(type, new object[]
						{
							this
						});
					}
					catch (MissingMethodException)
					{
						ModuleBuilder.WarnAboutSymbolWriter("The default Mono.CompilerServices.SymbolWriter is not available on this platform");
						return;
					}
				}
				string text = this.fqname;
				if (this.assemblyb.AssemblyDir != null)
				{
					text = Path.Combine(this.assemblyb.AssemblyDir, text);
				}
				this.symbolWriter.Initialize(IntPtr.Zero, text, true);
			}
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x000FEB60 File Offset: 0x000FCD60
		private static void WarnAboutSymbolWriter(string message)
		{
			if (ModuleBuilder.has_warned_about_symbolWriter)
			{
				return;
			}
			ModuleBuilder.has_warned_about_symbolWriter = true;
			Console.Error.WriteLine("WARNING: {0}", message);
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06005180 RID: 20864 RVA: 0x000FEB80 File Offset: 0x000FCD80
		public override string FullyQualifiedName
		{
			get
			{
				string text = this.fqname;
				if (text == null)
				{
					return null;
				}
				if (this.assemblyb.AssemblyDir != null)
				{
					text = Path.Combine(this.assemblyb.AssemblyDir, text);
					text = Path.GetFullPath(text);
				}
				return text;
			}
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x000FEBC0 File Offset: 0x000FCDC0
		public bool IsTransient()
		{
			return this.transient;
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x000FEBC8 File Offset: 0x000FCDC8
		public void CreateGlobalFunctions()
		{
			if (this.global_type_created != null)
			{
				throw new InvalidOperationException("global methods already created");
			}
			if (this.global_type != null)
			{
				this.global_type_created = this.global_type.CreateType();
			}
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x000FEC04 File Offset: 0x000FCE04
		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			FieldAttributes fieldAttributes = attributes & ~(FieldAttributes.RTSpecialName | FieldAttributes.HasFieldMarshal | FieldAttributes.HasDefault | FieldAttributes.HasFieldRVA);
			FieldBuilder fieldBuilder = this.DefineDataImpl(name, data.Length, fieldAttributes | FieldAttributes.HasFieldRVA);
			fieldBuilder.SetRVAData(data);
			return fieldBuilder;
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x000FEC3F File Offset: 0x000FCE3F
		public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
		{
			return this.DefineDataImpl(name, size, attributes & ~(FieldAttributes.RTSpecialName | FieldAttributes.HasFieldMarshal | FieldAttributes.HasDefault | FieldAttributes.HasFieldRVA));
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x000FEC50 File Offset: 0x000FCE50
		private FieldBuilder DefineDataImpl(string name, int size, FieldAttributes attributes)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException("name cannot be empty", "name");
			}
			if (this.global_type_created != null)
			{
				throw new InvalidOperationException("global fields already created");
			}
			if (size <= 0 || size >= 4128768)
			{
				throw new ArgumentException("Data size must be > 0 and < 0x3f0000", null);
			}
			this.CreateGlobalType();
			string className = "$ArrayType$" + size.ToString();
			Type type = this.GetType(className, false, false);
			if (type == null)
			{
				TypeBuilder typeBuilder = this.DefineType(className, TypeAttributes.Public | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, this.assemblyb.corlib_value_type, null, PackingSize.Size1, size);
				typeBuilder.CreateType();
				type = typeBuilder;
			}
			FieldBuilder fieldBuilder = this.global_type.DefineField(name, type, attributes | FieldAttributes.Static);
			if (this.global_fields != null)
			{
				FieldBuilder[] array = new FieldBuilder[this.global_fields.Length + 1];
				Array.Copy(this.global_fields, array, this.global_fields.Length);
				array[this.global_fields.Length] = fieldBuilder;
				this.global_fields = array;
			}
			else
			{
				this.global_fields = new FieldBuilder[1];
				this.global_fields[0] = fieldBuilder;
			}
			return fieldBuilder;
		}

		// Token: 0x06005186 RID: 20870 RVA: 0x000FED70 File Offset: 0x000FCF70
		private void addGlobalMethod(MethodBuilder mb)
		{
			if (this.global_methods != null)
			{
				MethodBuilder[] array = new MethodBuilder[this.global_methods.Length + 1];
				Array.Copy(this.global_methods, array, this.global_methods.Length);
				array[this.global_methods.Length] = mb;
				this.global_methods = array;
				return;
			}
			this.global_methods = new MethodBuilder[1];
			this.global_methods[0] = mb;
		}

		// Token: 0x06005187 RID: 20871 RVA: 0x000FEDD1 File Offset: 0x000FCFD1
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
		}

		// Token: 0x06005188 RID: 20872 RVA: 0x000FEDE0 File Offset: 0x000FCFE0
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.DefineGlobalMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06005189 RID: 20873 RVA: 0x000FEE00 File Offset: 0x000FD000
		public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("global methods must be static");
			}
			if (this.global_type_created != null)
			{
				throw new InvalidOperationException("global methods already created");
			}
			this.CreateGlobalType();
			MethodBuilder methodBuilder = this.global_type.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
			this.addGlobalMethod(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x0600518A RID: 20874 RVA: 0x000FEE70 File Offset: 0x000FD070
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			return this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		}

		// Token: 0x0600518B RID: 20875 RVA: 0x000FEE94 File Offset: 0x000FD094
		public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
			{
				throw new ArgumentException("global methods must be static");
			}
			if (this.global_type_created != null)
			{
				throw new InvalidOperationException("global methods already created");
			}
			this.CreateGlobalType();
			MethodBuilder methodBuilder = this.global_type.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
			this.addGlobalMethod(methodBuilder);
			return methodBuilder;
		}

		// Token: 0x0600518C RID: 20876 RVA: 0x000FEF03 File Offset: 0x000FD103
		public TypeBuilder DefineType(string name)
		{
			return this.DefineType(name, TypeAttributes.NotPublic);
		}

		// Token: 0x0600518D RID: 20877 RVA: 0x000FEF0D File Offset: 0x000FD10D
		public TypeBuilder DefineType(string name, TypeAttributes attr)
		{
			if ((attr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic)
			{
				return this.DefineType(name, attr, null, null);
			}
			return this.DefineType(name, attr, typeof(object), null);
		}

		// Token: 0x0600518E RID: 20878 RVA: 0x000FEF33 File Offset: 0x000FD133
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
		{
			return this.DefineType(name, attr, parent, null);
		}

		// Token: 0x0600518F RID: 20879 RVA: 0x000FEF40 File Offset: 0x000FD140
		private void AddType(TypeBuilder tb)
		{
			if (this.types != null)
			{
				if (this.types.Length == this.num_types)
				{
					TypeBuilder[] destinationArray = new TypeBuilder[this.types.Length * 2];
					Array.Copy(this.types, destinationArray, this.num_types);
					this.types = destinationArray;
				}
			}
			else
			{
				this.types = new TypeBuilder[1];
			}
			this.types[this.num_types] = tb;
			this.num_types++;
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x000FEFB8 File Offset: 0x000FD1B8
		private TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packingSize, int typesize)
		{
			if (name == null)
			{
				throw new ArgumentNullException("fullname");
			}
			TypeIdentifier key = TypeIdentifiers.FromInternal(name);
			if (this.name_cache.ContainsKey(key))
			{
				throw new ArgumentException("Duplicate type name within an assembly.");
			}
			TypeBuilder typeBuilder = new TypeBuilder(this, name, attr, parent, interfaces, packingSize, typesize, null);
			this.AddType(typeBuilder);
			this.name_cache.Add(key, typeBuilder);
			return typeBuilder;
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x000FF019 File Offset: 0x000FD219
		internal void RegisterTypeName(TypeBuilder tb, TypeName name)
		{
			this.name_cache.Add(name, tb);
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x000FF028 File Offset: 0x000FD228
		internal TypeBuilder GetRegisteredType(TypeName name)
		{
			TypeBuilder result = null;
			this.name_cache.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x000FF047 File Offset: 0x000FD247
		[ComVisible(true)]
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			return this.DefineType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x000FF056 File Offset: 0x000FD256
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
		{
			return this.DefineType(name, attr, parent, null, PackingSize.Unspecified, typesize);
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x000FF065 File Offset: 0x000FD265
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
		{
			return this.DefineType(name, attr, parent, null, packsize, 0);
		}

		// Token: 0x06005196 RID: 20886 RVA: 0x000FF074 File Offset: 0x000FD274
		public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
		{
			return this.DefineType(name, attr, parent, null, packingSize, typesize);
		}

		// Token: 0x06005197 RID: 20887 RVA: 0x000FF084 File Offset: 0x000FD284
		public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return new MonoArrayMethod(arrayClass, methodName, callingConvention, returnType, parameterTypes);
		}

		// Token: 0x06005198 RID: 20888 RVA: 0x000FF094 File Offset: 0x000FD294
		public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
		{
			TypeIdentifier key = TypeIdentifiers.FromInternal(name);
			if (this.name_cache.ContainsKey(key))
			{
				throw new ArgumentException("Duplicate type name within an assembly.");
			}
			EnumBuilder enumBuilder = new EnumBuilder(this, name, visibility, underlyingType);
			TypeBuilder typeBuilder = enumBuilder.GetTypeBuilder();
			this.AddType(typeBuilder);
			this.name_cache.Add(key, typeBuilder);
			return enumBuilder;
		}

		// Token: 0x06005199 RID: 20889 RVA: 0x000EEF82 File Offset: 0x000ED182
		[ComVisible(true)]
		public override Type GetType(string className)
		{
			return this.GetType(className, false, false);
		}

		// Token: 0x0600519A RID: 20890 RVA: 0x000EEF8D File Offset: 0x000ED18D
		[ComVisible(true)]
		public override Type GetType(string className, bool ignoreCase)
		{
			return this.GetType(className, false, ignoreCase);
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x000FF0E8 File Offset: 0x000FD2E8
		private TypeBuilder search_in_array(TypeBuilder[] arr, int validElementsInArray, TypeName className)
		{
			for (int i = 0; i < validElementsInArray; i++)
			{
				if (string.Compare(className.DisplayName, arr[i].FullName, true, CultureInfo.InvariantCulture) == 0)
				{
					return arr[i];
				}
			}
			return null;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x000FF124 File Offset: 0x000FD324
		private TypeBuilder search_nested_in_array(TypeBuilder[] arr, int validElementsInArray, TypeName className)
		{
			for (int i = 0; i < validElementsInArray; i++)
			{
				if (string.Compare(className.DisplayName, arr[i].Name, true, CultureInfo.InvariantCulture) == 0)
				{
					return arr[i];
				}
			}
			return null;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x000FF160 File Offset: 0x000FD360
		private TypeBuilder GetMaybeNested(TypeBuilder t, IEnumerable<TypeName> nested)
		{
			TypeBuilder typeBuilder = t;
			foreach (TypeName className in nested)
			{
				if (typeBuilder.subtypes == null)
				{
					return null;
				}
				typeBuilder = this.search_nested_in_array(typeBuilder.subtypes, typeBuilder.subtypes.Length, className);
				if (typeBuilder == null)
				{
					return null;
				}
			}
			return typeBuilder;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x000FF1D8 File Offset: 0x000FD3D8
		[ComVisible(true)]
		public override Type GetType(string className, bool throwOnError, bool ignoreCase)
		{
			if (className == null)
			{
				throw new ArgumentNullException("className");
			}
			if (className.Length == 0)
			{
				throw new ArgumentException("className");
			}
			TypeBuilder typeBuilder = null;
			if (this.types == null && throwOnError)
			{
				throw new TypeLoadException(className);
			}
			TypeSpec typeSpec = TypeSpec.Parse(className);
			if (!ignoreCase)
			{
				TypeName key = typeSpec.TypeNameWithoutModifiers();
				this.name_cache.TryGetValue(key, out typeBuilder);
			}
			else
			{
				if (this.types != null)
				{
					typeBuilder = this.search_in_array(this.types, this.num_types, typeSpec.Name);
				}
				if (!typeSpec.IsNested && typeBuilder != null)
				{
					typeBuilder = this.GetMaybeNested(typeBuilder, typeSpec.Nested);
				}
			}
			if (typeBuilder == null && throwOnError)
			{
				throw new TypeLoadException(className);
			}
			if (typeBuilder != null && (typeSpec.HasModifiers || typeSpec.IsByRef))
			{
				Type type = typeBuilder;
				if (typeBuilder != null)
				{
					TypeBuilder typeBuilder2 = typeBuilder;
					if (typeBuilder2.is_created)
					{
						type = typeBuilder2.CreateType();
					}
				}
				foreach (ModifierSpec modifierSpec in typeSpec.Modifiers)
				{
					if (modifierSpec is PointerSpec)
					{
						type = type.MakePointerType();
					}
					else if (modifierSpec is ArraySpec)
					{
						ArraySpec arraySpec = modifierSpec as ArraySpec;
						if (arraySpec.IsBound)
						{
							return null;
						}
						if (arraySpec.Rank == 1)
						{
							type = type.MakeArrayType();
						}
						else
						{
							type = type.MakeArrayType(arraySpec.Rank);
						}
					}
				}
				if (typeSpec.IsByRef)
				{
					type = type.MakeByRefType();
				}
				typeBuilder = (type as TypeBuilder);
				if (typeBuilder == null)
				{
					return type;
				}
			}
			if (typeBuilder != null && typeBuilder.is_created)
			{
				return typeBuilder.CreateType();
			}
			return typeBuilder;
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x000FF398 File Offset: 0x000FD598
		internal int get_next_table_index(object obj, int table, int count)
		{
			if (this.table_indexes == null)
			{
				this.table_indexes = new int[64];
				for (int i = 0; i < 64; i++)
				{
					this.table_indexes[i] = 1;
				}
				this.table_indexes[2] = 2;
			}
			int result = this.table_indexes[table];
			this.table_indexes[table] += count;
			return result;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x000FF3F4 File Offset: 0x000FD5F4
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
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

		// Token: 0x060051A1 RID: 20897 RVA: 0x000FF44E File Offset: 0x000FD64E
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			this.SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x000FF45D File Offset: 0x000FD65D
		public ISymbolWriter GetSymWriter()
		{
			return this.symbolWriter;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x000FF465 File Offset: 0x000FD665
		public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
		{
			if (this.symbolWriter != null)
			{
				return this.symbolWriter.DefineDocument(url, language, languageVendor, documentType);
			}
			return null;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x000FF484 File Offset: 0x000FD684
		public override Type[] GetTypes()
		{
			if (this.types == null)
			{
				return Type.EmptyTypes;
			}
			int num = this.num_types;
			Type[] array = new Type[num];
			Array.Copy(this.types, array, num);
			for (int i = 0; i < array.Length; i++)
			{
				if (this.types[i].is_created)
				{
					array[i] = this.types[i].CreateType();
				}
			}
			return array;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x000FF4E8 File Offset: 0x000FD6E8
		public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException("name cannot be empty");
			}
			if (this.transient)
			{
				throw new InvalidOperationException("The module is transient");
			}
			if (!this.assemblyb.IsSave)
			{
				throw new InvalidOperationException("The assembly is transient");
			}
			ResourceWriter resourceWriter = new ResourceWriter(new MemoryStream());
			if (this.resource_writers == null)
			{
				this.resource_writers = new Hashtable();
			}
			this.resource_writers[name] = resourceWriter;
			if (this.resources != null)
			{
				MonoResource[] destinationArray = new MonoResource[this.resources.Length + 1];
				Array.Copy(this.resources, destinationArray, this.resources.Length);
				this.resources = destinationArray;
			}
			else
			{
				this.resources = new MonoResource[1];
			}
			int num = this.resources.Length - 1;
			this.resources[num].name = name;
			this.resources[num].attrs = attribute;
			return resourceWriter;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x000FF5E2 File Offset: 0x000FD7E2
		public IResourceWriter DefineResource(string name, string description)
		{
			return this.DefineResource(name, description, ResourceAttributes.Public);
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x000FF5ED File Offset: 0x000FD7ED
		[MonoTODO]
		public void DefineUnmanagedResource(byte[] resource)
		{
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			throw new NotImplementedException();
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x000FF604 File Offset: 0x000FD804
		[MonoTODO]
		public void DefineUnmanagedResource(string resourceFileName)
		{
			if (resourceFileName == null)
			{
				throw new ArgumentNullException("resourceFileName");
			}
			if (resourceFileName == string.Empty)
			{
				throw new ArgumentException("resourceFileName");
			}
			if (!File.Exists(resourceFileName) || Directory.Exists(resourceFileName))
			{
				throw new FileNotFoundException("File '" + resourceFileName + "' does not exist or is a directory.");
			}
			throw new NotImplementedException();
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x000FF664 File Offset: 0x000FD864
		public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException("name cannot be empty");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.transient)
			{
				throw new InvalidOperationException("The module is transient");
			}
			if (!this.assemblyb.IsSave)
			{
				throw new InvalidOperationException("The assembly is transient");
			}
			if (this.resources != null)
			{
				MonoResource[] destinationArray = new MonoResource[this.resources.Length + 1];
				Array.Copy(this.resources, destinationArray, this.resources.Length);
				this.resources = destinationArray;
			}
			else
			{
				this.resources = new MonoResource[1];
			}
			int num = this.resources.Length - 1;
			this.resources[num].name = name;
			this.resources[num].attrs = attribute;
			this.resources[num].stream = stream;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public void SetSymCustomAttribute(string name, byte[] data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x000FF752 File Offset: 0x000FD952
		[MonoTODO]
		public void SetUserEntryPoint(MethodInfo entryPoint)
		{
			if (entryPoint == null)
			{
				throw new ArgumentNullException("entryPoint");
			}
			if (entryPoint.DeclaringType.Module != this)
			{
				throw new InvalidOperationException("entryPoint is not contained in this module");
			}
			throw new NotImplementedException();
		}

		// Token: 0x060051AC RID: 20908 RVA: 0x000FF78B File Offset: 0x000FD98B
		public MethodToken GetMethodToken(MethodInfo method)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return new MethodToken(this.GetToken(method));
		}

		// Token: 0x060051AD RID: 20909 RVA: 0x000FF7AD File Offset: 0x000FD9AD
		public MethodToken GetMethodToken(MethodInfo method, IEnumerable<Type> optionalParameterTypes)
		{
			if (method == null)
			{
				throw new ArgumentNullException("method");
			}
			return new MethodToken(this.GetToken(method, optionalParameterTypes));
		}

		// Token: 0x060051AE RID: 20910 RVA: 0x000FF7D0 File Offset: 0x000FD9D0
		public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return this.GetMethodToken(this.GetArrayMethod(arrayClass, methodName, callingConvention, returnType, parameterTypes));
		}

		// Token: 0x060051AF RID: 20911 RVA: 0x000FF7E5 File Offset: 0x000FD9E5
		[ComVisible(true)]
		public MethodToken GetConstructorToken(ConstructorInfo con)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			return new MethodToken(this.GetToken(con));
		}

		// Token: 0x060051B0 RID: 20912 RVA: 0x000FF807 File Offset: 0x000FDA07
		public MethodToken GetConstructorToken(ConstructorInfo constructor, IEnumerable<Type> optionalParameterTypes)
		{
			if (constructor == null)
			{
				throw new ArgumentNullException("constructor");
			}
			return new MethodToken(this.GetToken(constructor, optionalParameterTypes));
		}

		// Token: 0x060051B1 RID: 20913 RVA: 0x000FF82A File Offset: 0x000FDA2A
		public FieldToken GetFieldToken(FieldInfo field)
		{
			if (field == null)
			{
				throw new ArgumentNullException("field");
			}
			return new FieldToken(this.GetToken(field));
		}

		// Token: 0x060051B2 RID: 20914 RVA: 0x000479FC File Offset: 0x00045BFC
		[MonoTODO]
		public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060051B3 RID: 20915 RVA: 0x000FF84C File Offset: 0x000FDA4C
		public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
		{
			if (sigHelper == null)
			{
				throw new ArgumentNullException("sigHelper");
			}
			return new SignatureToken(this.GetToken(sigHelper));
		}

		// Token: 0x060051B4 RID: 20916 RVA: 0x000FF868 File Offset: 0x000FDA68
		public StringToken GetStringConstant(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			return new StringToken(this.GetToken(str));
		}

		// Token: 0x060051B5 RID: 20917 RVA: 0x000FF884 File Offset: 0x000FDA84
		public TypeToken GetTypeToken(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsByRef)
			{
				throw new ArgumentException("type can't be a byref type", "type");
			}
			if (!this.IsTransient() && type.Module is ModuleBuilder && ((ModuleBuilder)type.Module).IsTransient())
			{
				throw new InvalidOperationException("a non-transient module can't reference a transient module");
			}
			return new TypeToken(this.GetToken(type));
		}

		// Token: 0x060051B6 RID: 20918 RVA: 0x000FF8FB File Offset: 0x000FDAFB
		public TypeToken GetTypeToken(string name)
		{
			return this.GetTypeToken(this.GetType(name));
		}

		// Token: 0x060051B7 RID: 20919
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int getUSIndex(ModuleBuilder mb, string str);

		// Token: 0x060051B8 RID: 20920
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int getToken(ModuleBuilder mb, object obj, bool create_open_instance);

		// Token: 0x060051B9 RID: 20921
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int getMethodToken(ModuleBuilder mb, MethodBase method, Type[] opt_param_types);

		// Token: 0x060051BA RID: 20922 RVA: 0x000FF90C File Offset: 0x000FDB0C
		internal int GetToken(string str)
		{
			int usindex;
			if (!this.us_string_cache.TryGetValue(str, out usindex))
			{
				usindex = ModuleBuilder.getUSIndex(this, str);
				this.us_string_cache[str] = usindex;
			}
			return usindex;
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x000FF940 File Offset: 0x000FDB40
		private int GetPseudoToken(MemberInfo member, bool create_open_instance)
		{
			Dictionary<MemberInfo, int> dictionary = create_open_instance ? this.inst_tokens_open : this.inst_tokens;
			int num;
			if (dictionary == null)
			{
				dictionary = new Dictionary<MemberInfo, int>(ReferenceEqualityComparer<MemberInfo>.Instance);
				if (create_open_instance)
				{
					this.inst_tokens_open = dictionary;
				}
				else
				{
					this.inst_tokens = dictionary;
				}
			}
			else if (dictionary.TryGetValue(member, out num))
			{
				return num;
			}
			if (member is TypeBuilderInstantiation || member is SymbolType)
			{
				num = ModuleBuilder.typespec_tokengen--;
			}
			else if (member is FieldOnTypeBuilderInst)
			{
				num = ModuleBuilder.memberref_tokengen--;
			}
			else if (member is ConstructorOnTypeBuilderInst)
			{
				num = ModuleBuilder.memberref_tokengen--;
			}
			else if (member is MethodOnTypeBuilderInst)
			{
				num = ModuleBuilder.memberref_tokengen--;
			}
			else if (member is FieldBuilder)
			{
				num = ModuleBuilder.memberref_tokengen--;
			}
			else if (member is TypeBuilder)
			{
				if (create_open_instance && (member as TypeBuilder).ContainsGenericParameters)
				{
					num = ModuleBuilder.typespec_tokengen--;
				}
				else if (member.Module == this)
				{
					num = ModuleBuilder.typedef_tokengen--;
				}
				else
				{
					num = ModuleBuilder.typeref_tokengen--;
				}
			}
			else
			{
				if (member is EnumBuilder)
				{
					num = this.GetPseudoToken((member as EnumBuilder).GetTypeBuilder(), create_open_instance);
					dictionary[member] = num;
					return num;
				}
				if (member is ConstructorBuilder)
				{
					if (member.Module == this && !(member as ConstructorBuilder).TypeBuilder.ContainsGenericParameters)
					{
						num = ModuleBuilder.methoddef_tokengen--;
					}
					else
					{
						num = ModuleBuilder.memberref_tokengen--;
					}
				}
				else if (member is MethodBuilder)
				{
					MethodBuilder methodBuilder = member as MethodBuilder;
					if (member.Module == this && !methodBuilder.TypeBuilder.ContainsGenericParameters && !methodBuilder.IsGenericMethodDefinition)
					{
						num = ModuleBuilder.methoddef_tokengen--;
					}
					else
					{
						num = ModuleBuilder.memberref_tokengen--;
					}
				}
				else
				{
					if (!(member is GenericTypeParameterBuilder))
					{
						throw new NotImplementedException();
					}
					num = ModuleBuilder.typespec_tokengen--;
				}
			}
			dictionary[member] = num;
			this.RegisterToken(member, num);
			return num;
		}

		// Token: 0x060051BC RID: 20924 RVA: 0x000FFB6E File Offset: 0x000FDD6E
		internal int GetToken(MemberInfo member)
		{
			if (member is ConstructorBuilder || member is MethodBuilder || member is FieldBuilder)
			{
				return this.GetPseudoToken(member, false);
			}
			return ModuleBuilder.getToken(this, member, true);
		}

		// Token: 0x060051BD RID: 20925 RVA: 0x000FFB9C File Offset: 0x000FDD9C
		internal int GetToken(MemberInfo member, bool create_open_instance)
		{
			if (member is TypeBuilderInstantiation || member is FieldOnTypeBuilderInst || member is ConstructorOnTypeBuilderInst || member is MethodOnTypeBuilderInst || member is SymbolType || member is FieldBuilder || member is TypeBuilder || member is ConstructorBuilder || member is MethodBuilder || member is GenericTypeParameterBuilder || member is EnumBuilder)
			{
				return this.GetPseudoToken(member, create_open_instance);
			}
			return ModuleBuilder.getToken(this, member, create_open_instance);
		}

		// Token: 0x060051BE RID: 20926 RVA: 0x000FFC14 File Offset: 0x000FDE14
		internal int GetToken(MethodBase method, IEnumerable<Type> opt_param_types)
		{
			if (method is ConstructorBuilder || method is MethodBuilder)
			{
				return this.GetPseudoToken(method, false);
			}
			if (opt_param_types == null)
			{
				return ModuleBuilder.getToken(this, method, true);
			}
			List<Type> list = new List<Type>(opt_param_types);
			return ModuleBuilder.getMethodToken(this, method, list.ToArray());
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x000FFC5A File Offset: 0x000FDE5A
		internal int GetToken(MethodBase method, Type[] opt_param_types)
		{
			if (method is ConstructorBuilder || method is MethodBuilder)
			{
				return this.GetPseudoToken(method, false);
			}
			return ModuleBuilder.getMethodToken(this, method, opt_param_types);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x000FFC7D File Offset: 0x000FDE7D
		internal int GetToken(SignatureHelper helper)
		{
			return ModuleBuilder.getToken(this, helper, true);
		}

		// Token: 0x060051C1 RID: 20929
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void RegisterToken(object obj, int token);

		// Token: 0x060051C2 RID: 20930
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern object GetRegisteredToken(int token);

		// Token: 0x060051C3 RID: 20931 RVA: 0x000FFC87 File Offset: 0x000FDE87
		internal TokenGenerator GetTokenGenerator()
		{
			if (this.token_gen == null)
			{
				this.token_gen = new ModuleBuilderTokenGenerator(this);
			}
			return this.token_gen;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x000FFCA4 File Offset: 0x000FDEA4
		internal static object RuntimeResolve(object obj)
		{
			if (obj is MethodBuilder)
			{
				return (obj as MethodBuilder).RuntimeResolve();
			}
			if (obj is ConstructorBuilder)
			{
				return (obj as ConstructorBuilder).RuntimeResolve();
			}
			if (obj is FieldBuilder)
			{
				return (obj as FieldBuilder).RuntimeResolve();
			}
			if (obj is GenericTypeParameterBuilder)
			{
				return (obj as GenericTypeParameterBuilder).RuntimeResolve();
			}
			if (obj is FieldOnTypeBuilderInst)
			{
				return (obj as FieldOnTypeBuilderInst).RuntimeResolve();
			}
			if (obj is MethodOnTypeBuilderInst)
			{
				return (obj as MethodOnTypeBuilderInst).RuntimeResolve();
			}
			if (obj is ConstructorOnTypeBuilderInst)
			{
				return (obj as ConstructorOnTypeBuilderInst).RuntimeResolve();
			}
			if (obj is Type)
			{
				return (obj as Type).RuntimeResolve();
			}
			throw new NotImplementedException(obj.GetType().FullName);
		}

		// Token: 0x060051C5 RID: 20933
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void build_metadata(ModuleBuilder mb);

		// Token: 0x060051C6 RID: 20934
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void WriteToFile(IntPtr handle);

		// Token: 0x060051C7 RID: 20935 RVA: 0x000FFD64 File Offset: 0x000FDF64
		private void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map, Dictionary<MemberInfo, int> inst_tokens, bool open)
		{
			foreach (KeyValuePair<MemberInfo, int> keyValuePair in inst_tokens)
			{
				MemberInfo key = keyValuePair.Key;
				int value = keyValuePair.Value;
				MemberInfo memberInfo;
				if (key is TypeBuilderInstantiation || key is SymbolType)
				{
					memberInfo = (key as Type).RuntimeResolve();
				}
				else if (key is FieldOnTypeBuilderInst)
				{
					memberInfo = (key as FieldOnTypeBuilderInst).RuntimeResolve();
				}
				else if (key is ConstructorOnTypeBuilderInst)
				{
					memberInfo = (key as ConstructorOnTypeBuilderInst).RuntimeResolve();
				}
				else if (key is MethodOnTypeBuilderInst)
				{
					memberInfo = (key as MethodOnTypeBuilderInst).RuntimeResolve();
				}
				else if (key is FieldBuilder)
				{
					memberInfo = (key as FieldBuilder).RuntimeResolve();
				}
				else if (key is TypeBuilder)
				{
					memberInfo = (key as TypeBuilder).RuntimeResolve();
				}
				else if (key is EnumBuilder)
				{
					memberInfo = (key as EnumBuilder).RuntimeResolve();
				}
				else if (key is ConstructorBuilder)
				{
					memberInfo = (key as ConstructorBuilder).RuntimeResolve();
				}
				else if (key is MethodBuilder)
				{
					memberInfo = (key as MethodBuilder).RuntimeResolve();
				}
				else
				{
					if (!(key is GenericTypeParameterBuilder))
					{
						throw new NotImplementedException();
					}
					memberInfo = (key as GenericTypeParameterBuilder).RuntimeResolve();
				}
				int value2 = this.GetToken(memberInfo, open);
				token_map[value] = value2;
				member_map[value] = memberInfo;
				this.RegisterToken(memberInfo, value);
			}
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x000FFEF8 File Offset: 0x000FE0F8
		private void FixupTokens()
		{
			Dictionary<int, int> token_map = new Dictionary<int, int>();
			Dictionary<int, MemberInfo> member_map = new Dictionary<int, MemberInfo>();
			if (this.inst_tokens != null)
			{
				this.FixupTokens(token_map, member_map, this.inst_tokens, false);
			}
			if (this.inst_tokens_open != null)
			{
				this.FixupTokens(token_map, member_map, this.inst_tokens_open, true);
			}
			if (this.types != null)
			{
				for (int i = 0; i < this.num_types; i++)
				{
					this.types[i].FixupTokens(token_map, member_map);
				}
			}
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x000FFF68 File Offset: 0x000FE168
		internal void Save()
		{
			if (this.transient && !this.is_main)
			{
				return;
			}
			if (this.types != null)
			{
				for (int i = 0; i < this.num_types; i++)
				{
					if (!this.types[i].is_created)
					{
						throw new NotSupportedException("Type '" + this.types[i].FullName + "' was not completed.");
					}
				}
			}
			this.FixupTokens();
			if (this.global_type != null && this.global_type_created == null)
			{
				this.global_type_created = this.global_type.CreateType();
			}
			if (this.resources != null)
			{
				for (int j = 0; j < this.resources.Length; j++)
				{
					IResourceWriter resourceWriter;
					if (this.resource_writers != null && (resourceWriter = (this.resource_writers[this.resources[j].name] as IResourceWriter)) != null)
					{
						ResourceWriter resourceWriter2 = (ResourceWriter)resourceWriter;
						resourceWriter2.Generate();
						MemoryStream memoryStream = (MemoryStream)resourceWriter2._output;
						this.resources[j].data = new byte[memoryStream.Length];
						memoryStream.Seek(0L, SeekOrigin.Begin);
						memoryStream.Read(this.resources[j].data, 0, (int)memoryStream.Length);
					}
					else
					{
						Stream stream = this.resources[j].stream;
						if (stream != null)
						{
							try
							{
								long length = stream.Length;
								this.resources[j].data = new byte[length];
								stream.Seek(0L, SeekOrigin.Begin);
								stream.Read(this.resources[j].data, 0, (int)length);
							}
							catch
							{
							}
						}
					}
				}
			}
			ModuleBuilder.build_metadata(this);
			string text = this.fqname;
			if (this.assemblyb.AssemblyDir != null)
			{
				text = Path.Combine(this.assemblyb.AssemblyDir, text);
			}
			try
			{
				File.Delete(text);
			}
			catch
			{
			}
			using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write))
			{
				this.WriteToFile(fileStream.Handle);
			}
			File.SetAttributes(text, (FileAttributes)(-2147483648));
			if (this.types != null && this.symbolWriter != null)
			{
				for (int k = 0; k < this.num_types; k++)
				{
					this.types[k].GenerateDebugInfo(this.symbolWriter);
				}
				this.symbolWriter.Close();
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x060051CA RID: 20938 RVA: 0x001001F8 File Offset: 0x000FE3F8
		internal string FileName
		{
			get
			{
				return this.fqname;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (set) Token: 0x060051CB RID: 20939 RVA: 0x00100200 File Offset: 0x000FE400
		internal bool IsMain
		{
			set
			{
				this.is_main = value;
			}
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x00100209 File Offset: 0x000FE409
		internal void CreateGlobalType()
		{
			if (this.global_type == null)
			{
				this.global_type = new TypeBuilder(this, TypeAttributes.NotPublic, 1);
			}
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x00100227 File Offset: 0x000FE427
		internal override Guid GetModuleVersionId()
		{
			return new Guid(this.guid);
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x060051CE RID: 20942 RVA: 0x00100234 File Offset: 0x000FE434
		public override Assembly Assembly
		{
			get
			{
				return this.assemblyb;
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x060051CF RID: 20943 RVA: 0x0010023C File Offset: 0x000FE43C
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x0010023C File Offset: 0x000FE43C
		public override string ScopeName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x060051D1 RID: 20945 RVA: 0x000EF150 File Offset: 0x000ED350
		public override Guid ModuleVersionId
		{
			get
			{
				return this.GetModuleVersionId();
			}
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public override bool IsResource()
		{
			return false;
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x00100244 File Offset: 0x000FE444
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (this.global_type_created == null)
			{
				return null;
			}
			if (types == null)
			{
				return this.global_type_created.GetMethod(name);
			}
			return this.global_type_created.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x0010027B File Offset: 0x000FE47B
		public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveField(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x0010028C File Offset: 0x000FE48C
		public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveMember(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001002A0 File Offset: 0x000FE4A0
		internal MemberInfo ResolveOrGetRegisteredToken(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			ResolveTokenError error;
			MemberInfo memberInfo = RuntimeModule.ResolveMemberToken(this._impl, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out error);
			if (memberInfo != null)
			{
				return memberInfo;
			}
			memberInfo = (this.GetRegisteredToken(metadataToken) as MemberInfo);
			if (memberInfo == null)
			{
				throw RuntimeModule.resolve_token_exception(this.Name, metadataToken, error, "MemberInfo");
			}
			return memberInfo;
		}

		// Token: 0x060051D7 RID: 20951 RVA: 0x001002FD File Offset: 0x000FE4FD
		public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveMethod(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x060051D8 RID: 20952 RVA: 0x0010030E File Offset: 0x000FE50E
		public override string ResolveString(int metadataToken)
		{
			return RuntimeModule.ResolveString(this, this._impl, metadataToken);
		}

		// Token: 0x060051D9 RID: 20953 RVA: 0x0010031D File Offset: 0x000FE51D
		public override byte[] ResolveSignature(int metadataToken)
		{
			return RuntimeModule.ResolveSignature(this, this._impl, metadataToken);
		}

		// Token: 0x060051DA RID: 20954 RVA: 0x0010032C File Offset: 0x000FE52C
		public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
		{
			return RuntimeModule.ResolveType(this, this._impl, metadataToken, genericTypeArguments, genericMethodArguments);
		}

		// Token: 0x060051DB RID: 20955 RVA: 0x0010033D File Offset: 0x000FE53D
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060051DC RID: 20956 RVA: 0x00100346 File Offset: 0x000FE546
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060051DD RID: 20957 RVA: 0x0010034E File Offset: 0x000FE54E
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return base.IsDefined(attributeType, inherit);
		}

		// Token: 0x060051DE RID: 20958 RVA: 0x00100358 File Offset: 0x000FE558
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.GetCustomAttributes(null, inherit);
		}

		// Token: 0x060051DF RID: 20959 RVA: 0x00100364 File Offset: 0x000FE564
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (this.cattrs == null || this.cattrs.Length == 0)
			{
				return Array.Empty<object>();
			}
			if (attributeType is TypeBuilder)
			{
				throw new InvalidOperationException("First argument to GetCustomAttributes can't be a TypeBuilder");
			}
			List<object> list = new List<object>();
			for (int i = 0; i < this.cattrs.Length; i++)
			{
				Type type = this.cattrs[i].Ctor.GetType();
				if (type is TypeBuilder)
				{
					throw new InvalidOperationException("Can't construct custom attribute for TypeBuilder type");
				}
				if (attributeType == null || attributeType.IsAssignableFrom(type))
				{
					list.Add(this.cattrs[i].Invoke());
				}
			}
			return list.ToArray();
		}

		// Token: 0x060051E0 RID: 20960 RVA: 0x00100405 File Offset: 0x000FE605
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			if (this.global_type_created == null)
			{
				throw new InvalidOperationException("Module-level fields cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
			}
			return this.global_type_created.GetField(name, bindingAttr);
		}

		// Token: 0x060051E1 RID: 20961 RVA: 0x0010042D File Offset: 0x000FE62D
		public override FieldInfo[] GetFields(BindingFlags bindingFlags)
		{
			if (this.global_type_created == null)
			{
				throw new InvalidOperationException("Module-level fields cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
			}
			return this.global_type_created.GetFields(bindingFlags);
		}

		// Token: 0x060051E2 RID: 20962 RVA: 0x00100454 File Offset: 0x000FE654
		public override MethodInfo[] GetMethods(BindingFlags bindingFlags)
		{
			if (this.global_type_created == null)
			{
				throw new InvalidOperationException("Module-level methods cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
			}
			return this.global_type_created.GetMethods(bindingFlags);
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x060051E3 RID: 20963 RVA: 0x000F4CCF File Offset: 0x000F2ECF
		public override int MetadataToken
		{
			get
			{
				return RuntimeModule.get_MetadataToken(this);
			}
		}

		// Token: 0x060051E5 RID: 20965 RVA: 0x000173AD File Offset: 0x000155AD
		internal ModuleBuilder()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x040031DA RID: 12762
		internal IntPtr _impl;

		// Token: 0x040031DB RID: 12763
		internal Assembly assembly;

		// Token: 0x040031DC RID: 12764
		internal string fqname;

		// Token: 0x040031DD RID: 12765
		internal string name;

		// Token: 0x040031DE RID: 12766
		internal string scopename;

		// Token: 0x040031DF RID: 12767
		internal bool is_resource;

		// Token: 0x040031E0 RID: 12768
		internal int token;

		// Token: 0x040031E1 RID: 12769
		private UIntPtr dynamic_image;

		// Token: 0x040031E2 RID: 12770
		private int num_types;

		// Token: 0x040031E3 RID: 12771
		private TypeBuilder[] types;

		// Token: 0x040031E4 RID: 12772
		private CustomAttributeBuilder[] cattrs;

		// Token: 0x040031E5 RID: 12773
		private byte[] guid;

		// Token: 0x040031E6 RID: 12774
		private int table_idx;

		// Token: 0x040031E7 RID: 12775
		internal AssemblyBuilder assemblyb;

		// Token: 0x040031E8 RID: 12776
		private MethodBuilder[] global_methods;

		// Token: 0x040031E9 RID: 12777
		private FieldBuilder[] global_fields;

		// Token: 0x040031EA RID: 12778
		private bool is_main;

		// Token: 0x040031EB RID: 12779
		private MonoResource[] resources;

		// Token: 0x040031EC RID: 12780
		private IntPtr unparented_classes;

		// Token: 0x040031ED RID: 12781
		private int[] table_indexes;

		// Token: 0x040031EE RID: 12782
		private TypeBuilder global_type;

		// Token: 0x040031EF RID: 12783
		private Type global_type_created;

		// Token: 0x040031F0 RID: 12784
		private Dictionary<TypeName, TypeBuilder> name_cache;

		// Token: 0x040031F1 RID: 12785
		private Dictionary<string, int> us_string_cache;

		// Token: 0x040031F2 RID: 12786
		private bool transient;

		// Token: 0x040031F3 RID: 12787
		private ModuleBuilderTokenGenerator token_gen;

		// Token: 0x040031F4 RID: 12788
		private Hashtable resource_writers;

		// Token: 0x040031F5 RID: 12789
		private ISymbolWriter symbolWriter;

		// Token: 0x040031F6 RID: 12790
		private static bool has_warned_about_symbolWriter;

		// Token: 0x040031F7 RID: 12791
		private static int typeref_tokengen = 33554431;

		// Token: 0x040031F8 RID: 12792
		private static int typedef_tokengen = 50331647;

		// Token: 0x040031F9 RID: 12793
		private static int typespec_tokengen = 469762047;

		// Token: 0x040031FA RID: 12794
		private static int memberref_tokengen = 184549375;

		// Token: 0x040031FB RID: 12795
		private static int methoddef_tokengen = 117440511;

		// Token: 0x040031FC RID: 12796
		private Dictionary<MemberInfo, int> inst_tokens;

		// Token: 0x040031FD RID: 12797
		private Dictionary<MemberInfo, int> inst_tokens_open;
	}
}

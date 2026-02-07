using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System
{
	// Token: 0x020001A1 RID: 417
	[ComVisible(true)]
	[ComDefaultInterface(typeof(_Type))]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public abstract class Type : MemberInfo, IReflect, _Type
	{
		// Token: 0x06001109 RID: 4361 RVA: 0x000465C0 File Offset: 0x000447C0
		public virtual bool IsEnumDefined(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!this.IsEnum)
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			Type type = value.GetType();
			if (type.IsEnum)
			{
				if (!type.IsEquivalentTo(this))
				{
					throw new ArgumentException(SR.Format("Object must be the same type as the enum. The type passed in was '{0}'; the enum type was '{1}'.", type.ToString(), this.ToString()));
				}
				type = type.GetEnumUnderlyingType();
			}
			if (type == typeof(string))
			{
				object[] enumNames = this.GetEnumNames();
				return Array.IndexOf<object>(enumNames, value) >= 0;
			}
			if (!Type.IsIntegerType(type))
			{
				throw new InvalidOperationException("Unknown enum type.");
			}
			Type enumUnderlyingType = this.GetEnumUnderlyingType();
			if (enumUnderlyingType.GetTypeCodeImpl() != type.GetTypeCodeImpl())
			{
				throw new ArgumentException(SR.Format("Enum underlying type and the object must be same type or object must be a String. Type passed in was '{0}'; the enum underlying type was '{1}'.", type.ToString(), enumUnderlyingType.ToString()));
			}
			return Type.BinarySearch(this.GetEnumRawConstantValues(), value) >= 0;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x000466AC File Offset: 0x000448AC
		public virtual string GetEnumName(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!this.IsEnum)
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			Type type = value.GetType();
			if (!type.IsEnum && !Type.IsIntegerType(type))
			{
				throw new ArgumentException("The value passed in must be an enum base or an underlying type for an enum, such as an Int32.", "value");
			}
			int num = Type.BinarySearch(this.GetEnumRawConstantValues(), value);
			if (num >= 0)
			{
				return this.GetEnumNames()[num];
			}
			return null;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00046724 File Offset: 0x00044924
		public virtual string[] GetEnumNames()
		{
			if (!this.IsEnum)
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			string[] result;
			Array array;
			this.GetEnumData(out result, out array);
			return result;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00046754 File Offset: 0x00044954
		private Array GetEnumRawConstantValues()
		{
			string[] array;
			Array result;
			this.GetEnumData(out array, out result);
			return result;
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x0004676C File Offset: 0x0004496C
		private void GetEnumData(out string[] enumNames, out Array enumValues)
		{
			FieldInfo[] fields = this.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			object[] array = new object[fields.Length];
			string[] array2 = new string[fields.Length];
			for (int i = 0; i < fields.Length; i++)
			{
				array2[i] = fields[i].Name;
				array[i] = fields[i].GetRawConstantValue();
			}
			IComparer @default = Comparer<object>.Default;
			for (int j = 1; j < array.Length; j++)
			{
				int num = j;
				string text = array2[j];
				object obj = array[j];
				bool flag = false;
				while (@default.Compare(array[num - 1], obj) > 0)
				{
					array2[num] = array2[num - 1];
					array[num] = array[num - 1];
					num--;
					flag = true;
					if (num == 0)
					{
						break;
					}
				}
				if (flag)
				{
					array2[num] = text;
					array[num] = obj;
				}
			}
			enumNames = array2;
			enumValues = array;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00046838 File Offset: 0x00044A38
		private static int BinarySearch(Array array, object value)
		{
			ulong[] array2 = new ulong[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = Enum.ToUInt64(array.GetValue(i));
			}
			ulong value2 = Enum.ToUInt64(value);
			return Array.BinarySearch<ulong>(array2, value2);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00046880 File Offset: 0x00044A80
		internal static bool IsIntegerType(Type t)
		{
			return t == typeof(int) || t == typeof(short) || t == typeof(ushort) || t == typeof(byte) || t == typeof(sbyte) || t == typeof(uint) || t == typeof(long) || t == typeof(ulong) || t == typeof(char) || t == typeof(bool);
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00046948 File Offset: 0x00044B48
		public virtual bool IsSerializable
		{
			get
			{
				if ((this.GetAttributeFlagsImpl() & TypeAttributes.Serializable) != TypeAttributes.NotPublic)
				{
					return true;
				}
				Type type = this.UnderlyingSystemType;
				if (type.IsRuntimeImplemented())
				{
					while (!(type == typeof(Delegate)) && !(type == typeof(Enum)))
					{
						type = type.BaseType;
						if (!(type != null))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x000469AC File Offset: 0x00044BAC
		public virtual bool ContainsGenericParameters
		{
			get
			{
				if (this.HasElementType)
				{
					return this.GetRootElementType().ContainsGenericParameters;
				}
				if (this.IsGenericParameter)
				{
					return true;
				}
				if (!this.IsGenericType)
				{
					return false;
				}
				Type[] genericArguments = this.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (genericArguments[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00046A04 File Offset: 0x00044C04
		internal Type GetRootElementType()
		{
			Type type = this;
			while (type.HasElementType)
			{
				type = type.GetElementType();
			}
			return type;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00046A28 File Offset: 0x00044C28
		public bool IsVisible
		{
			get
			{
				if (this.IsGenericParameter)
				{
					return true;
				}
				if (this.HasElementType)
				{
					return this.GetElementType().IsVisible;
				}
				Type type = this;
				while (type.IsNested)
				{
					if (!type.IsNestedPublic)
					{
						return false;
					}
					type = type.DeclaringType;
				}
				if (!type.IsPublic)
				{
					return false;
				}
				if (this.IsGenericType && !this.IsGenericTypeDefinition)
				{
					Type[] genericArguments = this.GetGenericArguments();
					for (int i = 0; i < genericArguments.Length; i++)
					{
						if (!genericArguments[i].IsVisible)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00046AAC File Offset: 0x00044CAC
		public virtual Type[] FindInterfaces(TypeFilter filter, object filterCriteria)
		{
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			Type[] interfaces = this.GetInterfaces();
			int num = 0;
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (!filter(interfaces[i], filterCriteria))
				{
					interfaces[i] = null;
				}
				else
				{
					num++;
				}
			}
			if (num == interfaces.Length)
			{
				return interfaces;
			}
			Type[] array = new Type[num];
			num = 0;
			for (int j = 0; j < interfaces.Length; j++)
			{
				if (interfaces[j] != null)
				{
					array[num++] = interfaces[j];
				}
			}
			return array;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x00046B30 File Offset: 0x00044D30
		public virtual MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
		{
			MethodInfo[] array = null;
			ConstructorInfo[] array2 = null;
			FieldInfo[] array3 = null;
			PropertyInfo[] array4 = null;
			EventInfo[] array5 = null;
			Type[] array6 = null;
			int num = 0;
			if ((memberType & MemberTypes.Method) != (MemberTypes)0)
			{
				array = this.GetMethods(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (!filter(array[i], filterCriteria))
						{
							array[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array.Length;
				}
			}
			if ((memberType & MemberTypes.Constructor) != (MemberTypes)0)
			{
				array2 = this.GetConstructors(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array2.Length; i++)
					{
						if (!filter(array2[i], filterCriteria))
						{
							array2[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array2.Length;
				}
			}
			if ((memberType & MemberTypes.Field) != (MemberTypes)0)
			{
				array3 = this.GetFields(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array3.Length; i++)
					{
						if (!filter(array3[i], filterCriteria))
						{
							array3[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array3.Length;
				}
			}
			if ((memberType & MemberTypes.Property) != (MemberTypes)0)
			{
				array4 = this.GetProperties(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array4.Length; i++)
					{
						if (!filter(array4[i], filterCriteria))
						{
							array4[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array4.Length;
				}
			}
			if ((memberType & MemberTypes.Event) != (MemberTypes)0)
			{
				array5 = this.GetEvents(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array5.Length; i++)
					{
						if (!filter(array5[i], filterCriteria))
						{
							array5[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array5.Length;
				}
			}
			if ((memberType & MemberTypes.NestedType) != (MemberTypes)0)
			{
				array6 = this.GetNestedTypes(bindingAttr);
				if (filter != null)
				{
					for (int i = 0; i < array6.Length; i++)
					{
						if (!filter(array6[i], filterCriteria))
						{
							array6[i] = null;
						}
						else
						{
							num++;
						}
					}
				}
				else
				{
					num += array6.Length;
				}
			}
			MemberInfo[] array7 = new MemberInfo[num];
			num = 0;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						array7[num++] = array[i];
					}
				}
			}
			if (array2 != null)
			{
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] != null)
					{
						array7[num++] = array2[i];
					}
				}
			}
			if (array3 != null)
			{
				for (int i = 0; i < array3.Length; i++)
				{
					if (array3[i] != null)
					{
						array7[num++] = array3[i];
					}
				}
			}
			if (array4 != null)
			{
				for (int i = 0; i < array4.Length; i++)
				{
					if (array4[i] != null)
					{
						array7[num++] = array4[i];
					}
				}
			}
			if (array5 != null)
			{
				for (int i = 0; i < array5.Length; i++)
				{
					if (array5[i] != null)
					{
						array7[num++] = array5[i];
					}
				}
			}
			if (array6 != null)
			{
				for (int i = 0; i < array6.Length; i++)
				{
					if (array6[i] != null)
					{
						array7[num++] = array6[i];
					}
				}
			}
			return array7;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00046E3C File Offset: 0x0004503C
		[ComVisible(true)]
		public virtual bool IsSubclassOf(Type c)
		{
			Type type = this;
			if (type == c)
			{
				return false;
			}
			while (type != null)
			{
				if (type == c)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x00046E74 File Offset: 0x00045074
		public virtual bool IsAssignableFrom(Type c)
		{
			if (c == null)
			{
				return false;
			}
			if (this == c)
			{
				return true;
			}
			Type underlyingSystemType = this.UnderlyingSystemType;
			if (underlyingSystemType.IsRuntimeImplemented())
			{
				return underlyingSystemType.IsAssignableFrom(c);
			}
			if (c.IsSubclassOf(this))
			{
				return true;
			}
			if (this.IsInterface)
			{
				return c.ImplementInterface(this);
			}
			if (this.IsGenericParameter)
			{
				Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
				for (int i = 0; i < genericParameterConstraints.Length; i++)
				{
					if (!genericParameterConstraints[i].IsAssignableFrom(c))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x00046EF8 File Offset: 0x000450F8
		internal bool ImplementInterface(Type ifaceType)
		{
			Type type = this;
			while (type != null)
			{
				Type[] interfaces = type.GetInterfaces();
				if (interfaces != null)
				{
					for (int i = 0; i < interfaces.Length; i++)
					{
						if (interfaces[i] == ifaceType || (interfaces[i] != null && interfaces[i].ImplementInterface(ifaceType)))
						{
							return true;
						}
					}
				}
				type = type.BaseType;
			}
			return false;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x00046F58 File Offset: 0x00045158
		private static bool FilterAttributeImpl(MemberInfo m, object filterCriteria)
		{
			if (filterCriteria == null)
			{
				throw new InvalidFilterCriteriaException("An Int32 must be provided for the filter criteria.");
			}
			MemberTypes memberType = m.MemberType;
			if (memberType != MemberTypes.Constructor)
			{
				if (memberType == MemberTypes.Field)
				{
					FieldAttributes fieldAttributes = FieldAttributes.PrivateScope;
					try
					{
						fieldAttributes = (FieldAttributes)((int)filterCriteria);
					}
					catch
					{
						throw new InvalidFilterCriteriaException("An Int32 must be provided for the filter criteria.");
					}
					FieldAttributes attributes = ((FieldInfo)m).Attributes;
					return ((fieldAttributes & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.FieldAccessMask) == (fieldAttributes & FieldAttributes.FieldAccessMask)) && ((fieldAttributes & FieldAttributes.Static) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope) && ((fieldAttributes & FieldAttributes.InitOnly) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.InitOnly) != FieldAttributes.PrivateScope) && ((fieldAttributes & FieldAttributes.Literal) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.Literal) != FieldAttributes.PrivateScope) && ((fieldAttributes & FieldAttributes.NotSerialized) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.NotSerialized) != FieldAttributes.PrivateScope) && ((fieldAttributes & FieldAttributes.PinvokeImpl) == FieldAttributes.PrivateScope || (attributes & FieldAttributes.PinvokeImpl) != FieldAttributes.PrivateScope);
				}
				if (memberType != MemberTypes.Method)
				{
					return false;
				}
			}
			MethodAttributes methodAttributes = MethodAttributes.PrivateScope;
			try
			{
				methodAttributes = (MethodAttributes)((int)filterCriteria);
			}
			catch
			{
				throw new InvalidFilterCriteriaException("An Int32 must be provided for the filter criteria.");
			}
			MethodAttributes attributes2;
			if (m.MemberType == MemberTypes.Method)
			{
				attributes2 = ((MethodInfo)m).Attributes;
			}
			else
			{
				attributes2 = ((ConstructorInfo)m).Attributes;
			}
			return ((methodAttributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.MemberAccessMask) == (methodAttributes & MethodAttributes.MemberAccessMask)) && ((methodAttributes & MethodAttributes.Static) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.Static) != MethodAttributes.PrivateScope) && ((methodAttributes & MethodAttributes.Final) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.Final) != MethodAttributes.PrivateScope) && ((methodAttributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope) && ((methodAttributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.Abstract) != MethodAttributes.PrivateScope) && ((methodAttributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || (attributes2 & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope);
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x000470D4 File Offset: 0x000452D4
		private static bool FilterNameImpl(MemberInfo m, object filterCriteria)
		{
			if (filterCriteria == null || !(filterCriteria is string))
			{
				throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
			}
			string text = (string)filterCriteria;
			text = text.Trim();
			string text2 = m.Name;
			if (m.MemberType == MemberTypes.NestedType)
			{
				text2 = text2.Substring(text2.LastIndexOf('+') + 1);
			}
			if (text.Length > 0 && text[text.Length - 1] == '*')
			{
				text = text.Substring(0, text.Length - 1);
				return text2.StartsWith(text, StringComparison.Ordinal);
			}
			return text2.Equals(text);
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00047168 File Offset: 0x00045368
		private static bool FilterNameIgnoreCaseImpl(MemberInfo m, object filterCriteria)
		{
			if (filterCriteria == null || !(filterCriteria is string))
			{
				throw new InvalidFilterCriteriaException("A String must be provided for the filter criteria.");
			}
			string text = (string)filterCriteria;
			text = text.Trim();
			string text2 = m.Name;
			if (m.MemberType == MemberTypes.NestedType)
			{
				text2 = text2.Substring(text2.LastIndexOf('+') + 1);
			}
			if (text.Length > 0 && text[text.Length - 1] == '*')
			{
				text = text.Substring(0, text.Length - 1);
				return string.Compare(text2, 0, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
			return string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x00047210 File Offset: 0x00045410
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.TypeInfo;
			}
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00047214 File Offset: 0x00045414
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600111F RID: 4383
		public abstract string Namespace { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06001120 RID: 4384
		public abstract string AssemblyQualifiedName { get; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06001121 RID: 4385
		public abstract string FullName { get; }

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06001122 RID: 4386
		public abstract Assembly Assembly { get; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06001123 RID: 4387
		public new abstract Module Module { get; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x0004721C File Offset: 0x0004541C
		public bool IsNested
		{
			get
			{
				return this.DeclaringType != null;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override Type DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x0000AF5E File Offset: 0x0000915E
		public virtual MethodBase DeclaringMethod
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override Type ReflectedType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06001128 RID: 4392
		public abstract Type UnderlyingSystemType { get; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsTypeDefinition
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600112A RID: 4394 RVA: 0x00047231 File Offset: 0x00045431
		public bool IsArray
		{
			get
			{
				return this.IsArrayImpl();
			}
		}

		// Token: 0x0600112B RID: 4395
		protected abstract bool IsArrayImpl();

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600112C RID: 4396 RVA: 0x00047239 File Offset: 0x00045439
		public bool IsByRef
		{
			get
			{
				return this.IsByRefImpl();
			}
		}

		// Token: 0x0600112D RID: 4397
		protected abstract bool IsByRefImpl();

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600112E RID: 4398 RVA: 0x00047241 File Offset: 0x00045441
		public bool IsPointer
		{
			get
			{
				return this.IsPointerImpl();
			}
		}

		// Token: 0x0600112F RID: 4399
		protected abstract bool IsPointerImpl();

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06001130 RID: 4400 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsConstructedGenericType
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06001132 RID: 4402 RVA: 0x00047249 File Offset: 0x00045449
		public virtual bool IsGenericTypeParameter
		{
			get
			{
				return this.IsGenericParameter && this.DeclaringMethod == null;
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x00047261 File Offset: 0x00045461
		public virtual bool IsGenericMethodParameter
		{
			get
			{
				return this.IsGenericParameter && this.DeclaringMethod != null;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06001134 RID: 4404 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsGenericType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsGenericTypeDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06001136 RID: 4406 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSZArray
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00047279 File Offset: 0x00045479
		public virtual bool IsVariableBoundArray
		{
			get
			{
				return this.IsArray && !this.IsSZArray;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual bool IsByRefLike
		{
			get
			{
				throw new NotSupportedException("Derived classes must provide an implementation.");
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x0004729A File Offset: 0x0004549A
		public bool HasElementType
		{
			get
			{
				return this.HasElementTypeImpl();
			}
		}

		// Token: 0x0600113A RID: 4410
		protected abstract bool HasElementTypeImpl();

		// Token: 0x0600113B RID: 4411
		public abstract Type GetElementType();

		// Token: 0x0600113C RID: 4412 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual int GetArrayRank()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Type GetGenericTypeDefinition()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x000472A2 File Offset: 0x000454A2
		public virtual Type[] GenericTypeArguments
		{
			get
			{
				if (!this.IsGenericType || this.IsGenericTypeDefinition)
				{
					return Array.Empty<Type>();
				}
				return this.GetGenericArguments();
			}
		}

		// Token: 0x0600113F RID: 4415 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Type[] GetGenericArguments()
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x000472C0 File Offset: 0x000454C0
		public virtual int GenericParameterPosition
		{
			get
			{
				throw new InvalidOperationException("Method may only be called on a Type for which Type.IsGenericParameter is true.");
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06001141 RID: 4417 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000472D3 File Offset: 0x000454D3
		public virtual Type[] GetGenericParameterConstraints()
		{
			if (!this.IsGenericParameter)
			{
				throw new InvalidOperationException("Method may only be called on a Type for which Type.IsGenericParameter is true.");
			}
			throw new InvalidOperationException();
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x000472ED File Offset: 0x000454ED
		public TypeAttributes Attributes
		{
			get
			{
				return this.GetAttributeFlagsImpl();
			}
		}

		// Token: 0x06001144 RID: 4420
		protected abstract TypeAttributes GetAttributeFlagsImpl();

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x000472F5 File Offset: 0x000454F5
		public bool IsAbstract
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Abstract) > TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00047306 File Offset: 0x00045506
		public bool IsImport
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Import) > TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x00047317 File Offset: 0x00045517
		public bool IsSealed
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.Sealed) > TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x00047328 File Offset: 0x00045528
		public bool IsSpecialName
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.SpecialName) > TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x00047339 File Offset: 0x00045539
		public bool IsClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.NotPublic && !this.IsValueType;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x00047351 File Offset: 0x00045551
		public bool IsNestedAssembly
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x0004735E File Offset: 0x0004555E
		public bool IsNestedFamANDAssem
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x0004736B File Offset: 0x0004556B
		public bool IsNestedFamily
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00047378 File Offset: 0x00045578
		public bool IsNestedFamORAssem
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.VisibilityMask;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00047385 File Offset: 0x00045585
		public bool IsNestedPrivate
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x00047392 File Offset: 0x00045592
		public bool IsNestedPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x0004739F File Offset: 0x0004559F
		public bool IsNotPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000473AC File Offset: 0x000455AC
		public bool IsPublic
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.VisibilityMask) == TypeAttributes.Public;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000473B9 File Offset: 0x000455B9
		public bool IsAutoLayout
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x000473C7 File Offset: 0x000455C7
		public bool IsExplicitLayout
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x000473D6 File Offset: 0x000455D6
		public bool IsLayoutSequential
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06001155 RID: 4437 RVA: 0x000473E4 File Offset: 0x000455E4
		public bool IsAnsiClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.NotPublic;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x000473F5 File Offset: 0x000455F5
		public bool IsAutoClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.AutoClass;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06001157 RID: 4439 RVA: 0x0004740A File Offset: 0x0004560A
		public bool IsUnicodeClass
		{
			get
			{
				return (this.GetAttributeFlagsImpl() & TypeAttributes.StringFormatMask) == TypeAttributes.UnicodeClass;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06001158 RID: 4440 RVA: 0x0004741F File Offset: 0x0004561F
		public bool IsCOMObject
		{
			get
			{
				return this.IsCOMObjectImpl();
			}
		}

		// Token: 0x06001159 RID: 4441
		protected abstract bool IsCOMObjectImpl();

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600115A RID: 4442 RVA: 0x00047427 File Offset: 0x00045627
		public bool IsContextful
		{
			get
			{
				return this.IsContextfulImpl();
			}
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0004742F File Offset: 0x0004562F
		protected virtual bool IsContextfulImpl()
		{
			return typeof(ContextBoundObject).IsAssignableFrom(this);
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x000040F7 File Offset: 0x000022F7
		public virtual bool IsCollectible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x00047441 File Offset: 0x00045641
		public virtual bool IsEnum
		{
			get
			{
				return this.IsSubclassOf(typeof(Enum));
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00047453 File Offset: 0x00045653
		public bool IsMarshalByRef
		{
			get
			{
				return this.IsMarshalByRefImpl();
			}
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0004745B File Offset: 0x0004565B
		protected virtual bool IsMarshalByRefImpl()
		{
			return typeof(MarshalByRefObject).IsAssignableFrom(this);
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06001160 RID: 4448 RVA: 0x0004746D File Offset: 0x0004566D
		public bool IsPrimitive
		{
			get
			{
				return this.IsPrimitiveImpl();
			}
		}

		// Token: 0x06001161 RID: 4449
		protected abstract bool IsPrimitiveImpl();

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00047475 File Offset: 0x00045675
		public bool IsValueType
		{
			get
			{
				return this.IsValueTypeImpl();
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x0004747D File Offset: 0x0004567D
		protected virtual bool IsValueTypeImpl()
		{
			return this.IsSubclassOf(typeof(ValueType));
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public virtual bool IsSignatureType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecurityCritical
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecuritySafeCritical
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual bool IsSecurityTransparent
		{
			get
			{
				throw NotImplemented.ByDesign;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x0004748F File Offset: 0x0004568F
		public ConstructorInfo TypeInitializer
		{
			get
			{
				return this.GetConstructorImpl(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, CallingConventions.Any, Type.EmptyTypes, null);
			}
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x000474A1 File Offset: 0x000456A1
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(Type[] types)
		{
			return this.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, types, null);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x000474AE File Offset: 0x000456AE
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetConstructor(bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x000474BC File Offset: 0x000456BC
		[ComVisible(true)]
		public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetConstructorImpl(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x0600116D RID: 4461
		protected abstract ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x0600116E RID: 4462 RVA: 0x0004750B File Offset: 0x0004570B
		[ComVisible(true)]
		public ConstructorInfo[] GetConstructors()
		{
			return this.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x0600116F RID: 4463
		[ComVisible(true)]
		public abstract ConstructorInfo[] GetConstructors(BindingFlags bindingAttr);

		// Token: 0x06001170 RID: 4464 RVA: 0x00047515 File Offset: 0x00045715
		public EventInfo GetEvent(string name)
		{
			return this.GetEvent(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001171 RID: 4465
		public abstract EventInfo GetEvent(string name, BindingFlags bindingAttr);

		// Token: 0x06001172 RID: 4466 RVA: 0x00047520 File Offset: 0x00045720
		public virtual EventInfo[] GetEvents()
		{
			return this.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001173 RID: 4467
		public abstract EventInfo[] GetEvents(BindingFlags bindingAttr);

		// Token: 0x06001174 RID: 4468 RVA: 0x0004752A File Offset: 0x0004572A
		public FieldInfo GetField(string name)
		{
			return this.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001175 RID: 4469
		public abstract FieldInfo GetField(string name, BindingFlags bindingAttr);

		// Token: 0x06001176 RID: 4470 RVA: 0x00047535 File Offset: 0x00045735
		public FieldInfo[] GetFields()
		{
			return this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001177 RID: 4471
		public abstract FieldInfo[] GetFields(BindingFlags bindingAttr);

		// Token: 0x06001178 RID: 4472 RVA: 0x0004753F File Offset: 0x0004573F
		public MemberInfo[] GetMember(string name)
		{
			return this.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0004754A File Offset: 0x0004574A
		public virtual MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, MemberTypes.All, bindingAttr);
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00047559 File Offset: 0x00045759
		public MemberInfo[] GetMembers()
		{
			return this.GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600117C RID: 4476
		public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

		// Token: 0x0600117D RID: 4477 RVA: 0x00047563 File Offset: 0x00045763
		public MethodInfo GetMethod(string name)
		{
			return this.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x0004756E File Offset: 0x0004576E
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetMethodImpl(name, bindingAttr, null, CallingConventions.Any, null, null);
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x0004758A File Offset: 0x0004578A
		public MethodInfo GetMethod(string name, Type[] types)
		{
			return this.GetMethod(name, types, null);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x00047595 File Offset: 0x00045795
		public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, types, modifiers);
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000475A3 File Offset: 0x000457A3
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000475B4 File Offset: 0x000457B4
		public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001183 RID: 4483
		protected abstract MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06001184 RID: 4484 RVA: 0x00047613 File Offset: 0x00045813
		public MethodInfo GetMethod(string name, int genericParameterCount, Type[] types)
		{
			return this.GetMethod(name, genericParameterCount, types, null);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0004761F File Offset: 0x0004581F
		public MethodInfo GetMethod(string name, int genericParameterCount, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, genericParameterCount, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, types, modifiers);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0004762F File Offset: 0x0004582F
		public MethodInfo GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetMethod(name, genericParameterCount, bindingAttr, binder, CallingConventions.Any, types, modifiers);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x00047644 File Offset: 0x00045844
		public MethodInfo GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (genericParameterCount < 0)
			{
				throw new ArgumentException("Non-negative number required.", "genericParameterCount");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == null)
				{
					throw new ArgumentNullException("types");
				}
			}
			return this.GetMethodImpl(name, genericParameterCount, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x000472CC File Offset: 0x000454CC
		protected virtual MethodInfo GetMethodImpl(string name, int genericParameterCount, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000476B9 File Offset: 0x000458B9
		public MethodInfo[] GetMethods()
		{
			return this.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600118A RID: 4490
		public abstract MethodInfo[] GetMethods(BindingFlags bindingAttr);

		// Token: 0x0600118B RID: 4491 RVA: 0x000476C3 File Offset: 0x000458C3
		public Type GetNestedType(string name)
		{
			return this.GetNestedType(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600118C RID: 4492
		public abstract Type GetNestedType(string name, BindingFlags bindingAttr);

		// Token: 0x0600118D RID: 4493 RVA: 0x000476CE File Offset: 0x000458CE
		public Type[] GetNestedTypes()
		{
			return this.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600118E RID: 4494
		public abstract Type[] GetNestedTypes(BindingFlags bindingAttr);

		// Token: 0x0600118F RID: 4495 RVA: 0x000476D8 File Offset: 0x000458D8
		public PropertyInfo GetProperty(string name)
		{
			return this.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x000476E3 File Offset: 0x000458E3
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return this.GetPropertyImpl(name, bindingAttr, null, null, null, null);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x000476FF File Offset: 0x000458FF
		public PropertyInfo GetProperty(string name, Type returnType)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			return this.GetPropertyImpl(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, returnType, null, null);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x00047730 File Offset: 0x00045930
		public PropertyInfo GetProperty(string name, Type[] types)
		{
			return this.GetProperty(name, null, types);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004773B File Offset: 0x0004593B
		public PropertyInfo GetProperty(string name, Type returnType, Type[] types)
		{
			return this.GetProperty(name, returnType, types, null);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00047747 File Offset: 0x00045947
		public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, returnType, types, modifiers);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00047757 File Offset: 0x00045957
		public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (types == null)
			{
				throw new ArgumentNullException("types");
			}
			return this.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06001196 RID: 4502
		protected abstract PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

		// Token: 0x06001197 RID: 4503 RVA: 0x00047785 File Offset: 0x00045985
		public PropertyInfo[] GetProperties()
		{
			return this.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x06001198 RID: 4504
		public abstract PropertyInfo[] GetProperties(BindingFlags bindingAttr);

		// Token: 0x06001199 RID: 4505 RVA: 0x0004722A File Offset: 0x0004542A
		public virtual MemberInfo[] GetDefaultMembers()
		{
			throw NotImplemented.ByDesign;
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual RuntimeTypeHandle TypeHandle
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004778F File Offset: 0x0004598F
		public static RuntimeTypeHandle GetTypeHandle(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException(null, "Invalid handle.");
			}
			return o.GetType().TypeHandle;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x000477AC File Offset: 0x000459AC
		public static Type[] GetTypeArray(object[] args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			Type[] array = new Type[args.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (args[i] == null)
				{
					throw new ArgumentNullException();
				}
				array[i] = args[i].GetType();
			}
			return array;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000477F5 File Offset: 0x000459F5
		public static TypeCode GetTypeCode(Type type)
		{
			if (type == null)
			{
				return TypeCode.Empty;
			}
			return type.GetTypeCodeImpl();
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00047808 File Offset: 0x00045A08
		protected virtual TypeCode GetTypeCodeImpl()
		{
			if (this != this.UnderlyingSystemType && this.UnderlyingSystemType != null)
			{
				return Type.GetTypeCode(this.UnderlyingSystemType);
			}
			return TypeCode.Object;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600119F RID: 4511
		public abstract Guid GUID { get; }

		// Token: 0x060011A0 RID: 4512 RVA: 0x00047833 File Offset: 0x00045A33
		public static Type GetTypeFromCLSID(Guid clsid)
		{
			return Type.GetTypeFromCLSID(clsid, null, false);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004783D File Offset: 0x00045A3D
		public static Type GetTypeFromCLSID(Guid clsid, bool throwOnError)
		{
			return Type.GetTypeFromCLSID(clsid, null, throwOnError);
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00047847 File Offset: 0x00045A47
		public static Type GetTypeFromCLSID(Guid clsid, string server)
		{
			return Type.GetTypeFromCLSID(clsid, server, false);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00047851 File Offset: 0x00045A51
		public static Type GetTypeFromProgID(string progID)
		{
			return Type.GetTypeFromProgID(progID, null, false);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004785B File Offset: 0x00045A5B
		public static Type GetTypeFromProgID(string progID, bool throwOnError)
		{
			return Type.GetTypeFromProgID(progID, null, throwOnError);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00047865 File Offset: 0x00045A65
		public static Type GetTypeFromProgID(string progID, string server)
		{
			return Type.GetTypeFromProgID(progID, server, false);
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060011A6 RID: 4518
		public abstract Type BaseType { get; }

		// Token: 0x060011A7 RID: 4519 RVA: 0x00047870 File Offset: 0x00045A70
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, null, null);
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00047890 File Offset: 0x00045A90
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, CultureInfo culture)
		{
			return this.InvokeMember(name, invokeAttr, binder, target, args, null, culture, null);
		}

		// Token: 0x060011A9 RID: 4521
		public abstract object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);

		// Token: 0x060011AA RID: 4522 RVA: 0x000478AE File Offset: 0x00045AAE
		public Type GetInterface(string name)
		{
			return this.GetInterface(name, false);
		}

		// Token: 0x060011AB RID: 4523
		public abstract Type GetInterface(string name, bool ignoreCase);

		// Token: 0x060011AC RID: 4524
		public abstract Type[] GetInterfaces();

		// Token: 0x060011AD RID: 4525 RVA: 0x0004728E File Offset: 0x0004548E
		[ComVisible(true)]
		public virtual InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000478B8 File Offset: 0x00045AB8
		public virtual bool IsInstanceOfType(object o)
		{
			return o != null && this.IsAssignableFrom(o.GetType());
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000478CB File Offset: 0x00045ACB
		public virtual bool IsEquivalentTo(Type other)
		{
			return this == other;
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000478D4 File Offset: 0x00045AD4
		public virtual Type GetEnumUnderlyingType()
		{
			if (!this.IsEnum)
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			FieldInfo[] fields = this.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (fields == null || fields.Length != 1)
			{
				throw new ArgumentException("The Enum type should contain one and only one instance field.", "enumType");
			}
			return fields[0].FieldType;
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00047923 File Offset: 0x00045B23
		public virtual Array GetEnumValues()
		{
			if (!this.IsEnum)
			{
				throw new ArgumentException("Type provided must be an Enum.", "enumType");
			}
			throw NotImplemented.ByDesign;
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual Type MakeArrayType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual Type MakeArrayType(int rank)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual Type MakeByRefType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x0004728E File Offset: 0x0004548E
		public virtual Type MakeGenericType(params Type[] typeArguments)
		{
			throw new NotSupportedException("Derived classes must provide an implementation.");
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x000472CC File Offset: 0x000454CC
		public virtual Type MakePointerType()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00047942 File Offset: 0x00045B42
		public static Type MakeGenericSignatureType(Type genericTypeDefinition, params Type[] typeArguments)
		{
			return new SignatureConstructedGenericType(genericTypeDefinition, typeArguments);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0004794B File Offset: 0x00045B4B
		public static Type MakeGenericMethodParameter(int position)
		{
			if (position < 0)
			{
				throw new ArgumentException("Non-negative number required.", "position");
			}
			return new SignatureGenericMethodParameterType(position);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00047967 File Offset: 0x00045B67
		public override string ToString()
		{
			return "Type: " + this.Name;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00047979 File Offset: 0x00045B79
		public override bool Equals(object o)
		{
			return o != null && this.Equals(o as Type);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x0004798C File Offset: 0x00045B8C
		public override int GetHashCode()
		{
			Type underlyingSystemType = this.UnderlyingSystemType;
			if (underlyingSystemType != this)
			{
				return underlyingSystemType.GetHashCode();
			}
			return base.GetHashCode();
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x000479B1 File Offset: 0x00045BB1
		public virtual bool Equals(Type o)
		{
			return !(o == null) && this.UnderlyingSystemType == o.UnderlyingSystemType;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x000479CC File Offset: 0x00045BCC
		public static Binder DefaultBinder
		{
			get
			{
				if (Type.s_defaultBinder == null)
				{
					DefaultBinder value = new DefaultBinder();
					Interlocked.CompareExchange<Binder>(ref Type.s_defaultBinder, value, null);
				}
				return Type.s_defaultBinder;
			}
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Type.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011BF RID: 4543 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Type.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011C0 RID: 4544 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Type.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x000479FC File Offset: 0x00045BFC
		void _Type.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x00047A03 File Offset: 0x00045C03
		internal virtual Type InternalResolve()
		{
			return this.UnderlyingSystemType;
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual Type RuntimeResolve()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x000040F7 File Offset: 0x000022F7
		internal virtual bool IsUserType
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00047A0B File Offset: 0x00045C0B
		internal virtual MethodInfo GetMethod(MethodInfo fromNoninstanciated)
		{
			throw new InvalidOperationException("can only be called in generic type");
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x00047A0B File Offset: 0x00045C0B
		internal virtual ConstructorInfo GetConstructor(ConstructorInfo fromNoninstanciated)
		{
			throw new InvalidOperationException("can only be called in generic type");
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x00047A0B File Offset: 0x00045C0B
		internal virtual FieldInfo GetField(FieldInfo fromNoninstanciated)
		{
			throw new InvalidOperationException("can only be called in generic type");
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x00047A17 File Offset: 0x00045C17
		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			if (handle.Value == IntPtr.Zero)
			{
				return null;
			}
			return Type.internal_from_handle(handle.Value);
		}

		// Token: 0x060011C9 RID: 4553
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Type internal_from_handle(IntPtr handle);

		// Token: 0x060011CA RID: 4554 RVA: 0x00047A3A File Offset: 0x00045C3A
		internal virtual RuntimeTypeHandle GetTypeHandleInternal()
		{
			return this.TypeHandle;
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual bool IsWindowsRuntimeObjectImpl()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual bool IsExportedToWindowsRuntimeImpl()
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00047A42 File Offset: 0x00045C42
		internal bool IsWindowsRuntimeObject
		{
			get
			{
				return this.IsWindowsRuntimeObjectImpl();
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00047A4A File Offset: 0x00045C4A
		internal bool IsExportedToWindowsRuntime
		{
			get
			{
				return this.IsExportedToWindowsRuntimeImpl();
			}
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal virtual bool HasProxyAttributeImpl()
		{
			return false;
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		internal virtual bool IsSzArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060011D1 RID: 4561 RVA: 0x00047A52 File Offset: 0x00045C52
		internal string FormatTypeName()
		{
			return this.FormatTypeName(false);
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x000479FC File Offset: 0x00045BFC
		internal virtual string FormatTypeName(bool serialization)
		{
			throw new NotImplementedException();
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00047A5C File Offset: 0x00045C5C
		public bool IsInterface
		{
			[SecuritySafeCritical]
			get
			{
				RuntimeType runtimeType = this as RuntimeType;
				if (runtimeType != null)
				{
					return RuntimeTypeHandle.IsInterface(runtimeType);
				}
				return (this.GetAttributeFlagsImpl() & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask;
			}
		}

		// Token: 0x060011D4 RID: 4564 RVA: 0x00047A90 File Offset: 0x00045C90
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.GetType(typeName, throwOnError, ignoreCase, false, ref stackCrawlMark);
		}

		// Token: 0x060011D5 RID: 4565 RVA: 0x00047AAC File Offset: 0x00045CAC
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, bool throwOnError)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.GetType(typeName, throwOnError, false, false, ref stackCrawlMark);
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00047AC8 File Offset: 0x00045CC8
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.GetType(typeName, false, false, false, ref stackCrawlMark);
		}

		// Token: 0x060011D7 RID: 4567 RVA: 0x00047AE4 File Offset: 0x00045CE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return TypeNameParser.GetType(typeName, assemblyResolver, typeResolver, false, false, ref stackCrawlMark);
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00047B00 File Offset: 0x00045D00
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return TypeNameParser.GetType(typeName, assemblyResolver, typeResolver, throwOnError, false, ref stackCrawlMark);
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00047B1C File Offset: 0x00045D1C
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type GetType(string typeName, Func<AssemblyName, Assembly> assemblyResolver, Func<Assembly, string, bool, Type> typeResolver, bool throwOnError, bool ignoreCase)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return TypeNameParser.GetType(typeName, assemblyResolver, typeResolver, throwOnError, ignoreCase, ref stackCrawlMark);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x0002842A File Offset: 0x0002662A
		public static bool operator ==(Type left, Type right)
		{
			return left == right;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00028430 File Offset: 0x00026630
		public static bool operator !=(Type left, Type right)
		{
			return left != right;
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00047B38 File Offset: 0x00045D38
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Type ReflectionOnlyGetType(string typeName, bool throwIfNotFound, bool ignoreCase)
		{
			StackCrawlMark stackCrawlMark = StackCrawlMark.LookForMyCaller;
			return RuntimeType.GetType(typeName, throwIfNotFound, ignoreCase, true, ref stackCrawlMark);
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00047B52 File Offset: 0x00045D52
		public static Type GetTypeFromCLSID(Guid clsid, string server, bool throwOnError)
		{
			return RuntimeType.GetTypeFromCLSIDImpl(clsid, server, throwOnError);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x00047B5C File Offset: 0x00045D5C
		public static Type GetTypeFromProgID(string progID, string server, bool throwOnError)
		{
			return RuntimeType.GetTypeFromProgIDImpl(progID, server, throwOnError);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x00047B68 File Offset: 0x00045D68
		internal string FullNameOrDefault
		{
			get
			{
				if (this.InternalNameIfAvailable == null)
				{
					return "UnknownType";
				}
				string result;
				try
				{
					result = this.FullName;
				}
				catch (MissingMetadataException)
				{
					result = "UnknownType";
				}
				return result;
			}
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x00047BA8 File Offset: 0x00045DA8
		internal bool IsRuntimeImplemented()
		{
			return this.UnderlyingSystemType is RuntimeType;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x00047BB8 File Offset: 0x00045DB8
		internal virtual string InternalGetNameIfAvailable(ref Type rootCauseForFailure)
		{
			return this.Name;
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x00047BC0 File Offset: 0x00045DC0
		internal string InternalNameIfAvailable
		{
			get
			{
				Type type = null;
				return this.InternalGetNameIfAvailable(ref type);
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x00047BD7 File Offset: 0x00045DD7
		internal string NameOrDefault
		{
			get
			{
				return this.InternalNameIfAvailable ?? "UnknownType";
			}
		}

		// Token: 0x0400133D RID: 4925
		private static volatile Binder s_defaultBinder;

		// Token: 0x0400133E RID: 4926
		public static readonly char Delimiter = '.';

		// Token: 0x0400133F RID: 4927
		public static readonly Type[] EmptyTypes = Array.Empty<Type>();

		// Token: 0x04001340 RID: 4928
		public static readonly object Missing = System.Reflection.Missing.Value;

		// Token: 0x04001341 RID: 4929
		public static readonly MemberFilter FilterAttribute = new MemberFilter(Type.FilterAttributeImpl);

		// Token: 0x04001342 RID: 4930
		public static readonly MemberFilter FilterName = new MemberFilter(Type.FilterNameImpl);

		// Token: 0x04001343 RID: 4931
		public static readonly MemberFilter FilterNameIgnoreCase = new MemberFilter(Type.FilterNameIgnoreCaseImpl);

		// Token: 0x04001344 RID: 4932
		private const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;

		// Token: 0x04001345 RID: 4933
		internal RuntimeTypeHandle _impl;

		// Token: 0x04001346 RID: 4934
		internal const string DefaultTypeNameWhenMissingMetadata = "UnknownType";
	}
}

using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020008CE RID: 2254
	public class TypeDelegator : TypeInfo
	{
		// Token: 0x06004AF0 RID: 19184 RVA: 0x00057EF9 File Offset: 0x000560F9
		public override bool IsAssignableFrom(TypeInfo typeInfo)
		{
			return !(typeInfo == null) && this.IsAssignableFrom(typeInfo.AsType());
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x000EFE07 File Offset: 0x000EE007
		protected TypeDelegator()
		{
		}

		// Token: 0x06004AF2 RID: 19186 RVA: 0x000EFE0F File Offset: 0x000EE00F
		public TypeDelegator(Type delegatingType)
		{
			if (delegatingType == null)
			{
				throw new ArgumentNullException("delegatingType");
			}
			this.typeImpl = delegatingType;
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06004AF3 RID: 19187 RVA: 0x000EFE32 File Offset: 0x000EE032
		public override Guid GUID
		{
			get
			{
				return this.typeImpl.GUID;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06004AF4 RID: 19188 RVA: 0x000EFE3F File Offset: 0x000EE03F
		public override int MetadataToken
		{
			get
			{
				return this.typeImpl.MetadataToken;
			}
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x000EFE4C File Offset: 0x000EE04C
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			return this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06004AF6 RID: 19190 RVA: 0x000EFE71 File Offset: 0x000EE071
		public override Module Module
		{
			get
			{
				return this.typeImpl.Module;
			}
		}

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06004AF7 RID: 19191 RVA: 0x000EFE7E File Offset: 0x000EE07E
		public override Assembly Assembly
		{
			get
			{
				return this.typeImpl.Assembly;
			}
		}

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06004AF8 RID: 19192 RVA: 0x000EFE8B File Offset: 0x000EE08B
		public override RuntimeTypeHandle TypeHandle
		{
			get
			{
				return this.typeImpl.TypeHandle;
			}
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06004AF9 RID: 19193 RVA: 0x000EFE98 File Offset: 0x000EE098
		public override string Name
		{
			get
			{
				return this.typeImpl.Name;
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06004AFA RID: 19194 RVA: 0x000EFEA5 File Offset: 0x000EE0A5
		public override string FullName
		{
			get
			{
				return this.typeImpl.FullName;
			}
		}

		// Token: 0x17000C0C RID: 3084
		// (get) Token: 0x06004AFB RID: 19195 RVA: 0x000EFEB2 File Offset: 0x000EE0B2
		public override string Namespace
		{
			get
			{
				return this.typeImpl.Namespace;
			}
		}

		// Token: 0x17000C0D RID: 3085
		// (get) Token: 0x06004AFC RID: 19196 RVA: 0x000EFEBF File Offset: 0x000EE0BF
		public override string AssemblyQualifiedName
		{
			get
			{
				return this.typeImpl.AssemblyQualifiedName;
			}
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06004AFD RID: 19197 RVA: 0x000EFECC File Offset: 0x000EE0CC
		public override Type BaseType
		{
			get
			{
				return this.typeImpl.BaseType;
			}
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x000EFED9 File Offset: 0x000EE0D9
		protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x000EFEED File Offset: 0x000EE0ED
		[ComVisible(true)]
		public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetConstructors(bindingAttr);
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x000EFEFB File Offset: 0x000EE0FB
		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			if (types == null)
			{
				return this.typeImpl.GetMethod(name, bindingAttr);
			}
			return this.typeImpl.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		// Token: 0x06004B01 RID: 19201 RVA: 0x000EFF23 File Offset: 0x000EE123
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMethods(bindingAttr);
		}

		// Token: 0x06004B02 RID: 19202 RVA: 0x000EFF31 File Offset: 0x000EE131
		public override FieldInfo GetField(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetField(name, bindingAttr);
		}

		// Token: 0x06004B03 RID: 19203 RVA: 0x000EFF40 File Offset: 0x000EE140
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetFields(bindingAttr);
		}

		// Token: 0x06004B04 RID: 19204 RVA: 0x000EFF4E File Offset: 0x000EE14E
		public override Type GetInterface(string name, bool ignoreCase)
		{
			return this.typeImpl.GetInterface(name, ignoreCase);
		}

		// Token: 0x06004B05 RID: 19205 RVA: 0x000EFF5D File Offset: 0x000EE15D
		public override Type[] GetInterfaces()
		{
			return this.typeImpl.GetInterfaces();
		}

		// Token: 0x06004B06 RID: 19206 RVA: 0x000EFF6A File Offset: 0x000EE16A
		public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvent(name, bindingAttr);
		}

		// Token: 0x06004B07 RID: 19207 RVA: 0x000EFF79 File Offset: 0x000EE179
		public override EventInfo[] GetEvents()
		{
			return this.typeImpl.GetEvents();
		}

		// Token: 0x06004B08 RID: 19208 RVA: 0x000EFF86 File Offset: 0x000EE186
		protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			if (returnType == null && types == null)
			{
				return this.typeImpl.GetProperty(name, bindingAttr);
			}
			return this.typeImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x06004B09 RID: 19209 RVA: 0x000EFFB8 File Offset: 0x000EE1B8
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetProperties(bindingAttr);
		}

		// Token: 0x06004B0A RID: 19210 RVA: 0x000EFFC6 File Offset: 0x000EE1C6
		public override EventInfo[] GetEvents(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetEvents(bindingAttr);
		}

		// Token: 0x06004B0B RID: 19211 RVA: 0x000EFFD4 File Offset: 0x000EE1D4
		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedTypes(bindingAttr);
		}

		// Token: 0x06004B0C RID: 19212 RVA: 0x000EFFE2 File Offset: 0x000EE1E2
		public override Type GetNestedType(string name, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetNestedType(name, bindingAttr);
		}

		// Token: 0x06004B0D RID: 19213 RVA: 0x000EFFF1 File Offset: 0x000EE1F1
		public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMember(name, type, bindingAttr);
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x000F0001 File Offset: 0x000EE201
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return this.typeImpl.GetMembers(bindingAttr);
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x000F000F File Offset: 0x000EE20F
		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return this.typeImpl.Attributes;
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004B10 RID: 19216 RVA: 0x000F001C File Offset: 0x000EE21C
		public override bool IsTypeDefinition
		{
			get
			{
				return this.typeImpl.IsTypeDefinition;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06004B11 RID: 19217 RVA: 0x000F0029 File Offset: 0x000EE229
		public override bool IsSZArray
		{
			get
			{
				return this.typeImpl.IsSZArray;
			}
		}

		// Token: 0x06004B12 RID: 19218 RVA: 0x000F0036 File Offset: 0x000EE236
		protected override bool IsArrayImpl()
		{
			return this.typeImpl.IsArray;
		}

		// Token: 0x06004B13 RID: 19219 RVA: 0x000F0043 File Offset: 0x000EE243
		protected override bool IsPrimitiveImpl()
		{
			return this.typeImpl.IsPrimitive;
		}

		// Token: 0x06004B14 RID: 19220 RVA: 0x000F0050 File Offset: 0x000EE250
		protected override bool IsByRefImpl()
		{
			return this.typeImpl.IsByRef;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x06004B15 RID: 19221 RVA: 0x000F005D File Offset: 0x000EE25D
		public override bool IsGenericTypeParameter
		{
			get
			{
				return this.typeImpl.IsGenericTypeParameter;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004B16 RID: 19222 RVA: 0x000F006A File Offset: 0x000EE26A
		public override bool IsGenericMethodParameter
		{
			get
			{
				return this.typeImpl.IsGenericMethodParameter;
			}
		}

		// Token: 0x06004B17 RID: 19223 RVA: 0x000F0077 File Offset: 0x000EE277
		protected override bool IsPointerImpl()
		{
			return this.typeImpl.IsPointer;
		}

		// Token: 0x06004B18 RID: 19224 RVA: 0x000F0084 File Offset: 0x000EE284
		protected override bool IsValueTypeImpl()
		{
			return this.typeImpl.IsValueType;
		}

		// Token: 0x06004B19 RID: 19225 RVA: 0x000F0091 File Offset: 0x000EE291
		protected override bool IsCOMObjectImpl()
		{
			return this.typeImpl.IsCOMObject;
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004B1A RID: 19226 RVA: 0x000F009E File Offset: 0x000EE29E
		public override bool IsByRefLike
		{
			get
			{
				return this.typeImpl.IsByRefLike;
			}
		}

		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004B1B RID: 19227 RVA: 0x000F00AB File Offset: 0x000EE2AB
		public override bool IsConstructedGenericType
		{
			get
			{
				return this.typeImpl.IsConstructedGenericType;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06004B1C RID: 19228 RVA: 0x000F00B8 File Offset: 0x000EE2B8
		public override bool IsCollectible
		{
			get
			{
				return this.typeImpl.IsCollectible;
			}
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x000F00C5 File Offset: 0x000EE2C5
		public override Type GetElementType()
		{
			return this.typeImpl.GetElementType();
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x000F00D2 File Offset: 0x000EE2D2
		protected override bool HasElementTypeImpl()
		{
			return this.typeImpl.HasElementType;
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06004B1F RID: 19231 RVA: 0x000F00DF File Offset: 0x000EE2DF
		public override Type UnderlyingSystemType
		{
			get
			{
				return this.typeImpl.UnderlyingSystemType;
			}
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x000F00EC File Offset: 0x000EE2EC
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(inherit);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x000F00FA File Offset: 0x000EE2FA
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.typeImpl.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004B22 RID: 19234 RVA: 0x000F0109 File Offset: 0x000EE309
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.typeImpl.IsDefined(attributeType, inherit);
		}

		// Token: 0x06004B23 RID: 19235 RVA: 0x000F0118 File Offset: 0x000EE318
		[ComVisible(true)]
		public override InterfaceMapping GetInterfaceMap(Type interfaceType)
		{
			return this.typeImpl.GetInterfaceMap(interfaceType);
		}

		// Token: 0x04002F56 RID: 12118
		protected Type typeImpl;
	}
}

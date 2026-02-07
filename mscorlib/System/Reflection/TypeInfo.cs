using System;
using System.Collections.Generic;

namespace System.Reflection
{
	// Token: 0x020008D0 RID: 2256
	public abstract class TypeInfo : Type, IReflectableType
	{
		// Token: 0x06004B29 RID: 19241 RVA: 0x0000270D File Offset: 0x0000090D
		TypeInfo IReflectableType.GetTypeInfo()
		{
			return this;
		}

		// Token: 0x06004B2A RID: 19242 RVA: 0x0000270D File Offset: 0x0000090D
		public virtual Type AsType()
		{
			return this;
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x06004B2B RID: 19243 RVA: 0x000F0126 File Offset: 0x000EE326
		public virtual Type[] GenericTypeParameters
		{
			get
			{
				if (!this.IsGenericTypeDefinition)
				{
					return Type.EmptyTypes;
				}
				return this.GetGenericArguments();
			}
		}

		// Token: 0x06004B2C RID: 19244 RVA: 0x000F013C File Offset: 0x000EE33C
		public virtual EventInfo GetDeclaredEvent(string name)
		{
			return this.GetEvent(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B2D RID: 19245 RVA: 0x000F0147 File Offset: 0x000EE347
		public virtual FieldInfo GetDeclaredField(string name)
		{
			return this.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B2E RID: 19246 RVA: 0x000F0152 File Offset: 0x000EE352
		public virtual MethodInfo GetDeclaredMethod(string name)
		{
			return base.GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B2F RID: 19247 RVA: 0x000F015D File Offset: 0x000EE35D
		public virtual TypeInfo GetDeclaredNestedType(string name)
		{
			Type nestedType = this.GetNestedType(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (nestedType == null)
			{
				return null;
			}
			return nestedType.GetTypeInfo();
		}

		// Token: 0x06004B30 RID: 19248 RVA: 0x000F0173 File Offset: 0x000EE373
		public virtual PropertyInfo GetDeclaredProperty(string name)
		{
			return base.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06004B31 RID: 19249 RVA: 0x000F017E File Offset: 0x000EE37E
		public virtual IEnumerable<MethodInfo> GetDeclaredMethods(string name)
		{
			foreach (MethodInfo methodInfo in this.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (methodInfo.Name == name)
				{
					yield return methodInfo;
				}
			}
			MethodInfo[] array = null;
			yield break;
		}

		// Token: 0x17000C18 RID: 3096
		// (get) Token: 0x06004B32 RID: 19250 RVA: 0x000F0195 File Offset: 0x000EE395
		public virtual IEnumerable<ConstructorInfo> DeclaredConstructors
		{
			get
			{
				return this.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C19 RID: 3097
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x000F019F File Offset: 0x000EE39F
		public virtual IEnumerable<EventInfo> DeclaredEvents
		{
			get
			{
				return this.GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C1A RID: 3098
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x000F01A9 File Offset: 0x000EE3A9
		public virtual IEnumerable<FieldInfo> DeclaredFields
		{
			get
			{
				return this.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C1B RID: 3099
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x000F01B3 File Offset: 0x000EE3B3
		public virtual IEnumerable<MemberInfo> DeclaredMembers
		{
			get
			{
				return this.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C1C RID: 3100
		// (get) Token: 0x06004B36 RID: 19254 RVA: 0x000F01BD File Offset: 0x000EE3BD
		public virtual IEnumerable<MethodInfo> DeclaredMethods
		{
			get
			{
				return this.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C1D RID: 3101
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x000F01C7 File Offset: 0x000EE3C7
		public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
		{
			get
			{
				foreach (Type type in this.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					yield return type.GetTypeInfo();
				}
				Type[] array = null;
				yield break;
			}
		}

		// Token: 0x17000C1E RID: 3102
		// (get) Token: 0x06004B38 RID: 19256 RVA: 0x000F01D7 File Offset: 0x000EE3D7
		public virtual IEnumerable<PropertyInfo> DeclaredProperties
		{
			get
			{
				return this.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
		}

		// Token: 0x17000C1F RID: 3103
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x000F01E1 File Offset: 0x000EE3E1
		public virtual IEnumerable<Type> ImplementedInterfaces
		{
			get
			{
				return this.GetInterfaces();
			}
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x000F01EC File Offset: 0x000EE3EC
		public virtual bool IsAssignableFrom(TypeInfo typeInfo)
		{
			if (typeInfo == null)
			{
				return false;
			}
			if (this == typeInfo)
			{
				return true;
			}
			if (typeInfo.IsSubclassOf(this))
			{
				return true;
			}
			if (base.IsInterface)
			{
				return typeInfo.ImplementInterface(this);
			}
			if (this.IsGenericParameter)
			{
				Type[] genericParameterConstraints = this.GetGenericParameterConstraints();
				for (int i = 0; i < genericParameterConstraints.Length; i++)
				{
					if (!genericParameterConstraints[i].IsAssignableFrom(typeInfo))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04002F57 RID: 12119
		private const BindingFlags DeclaredOnlyLookup = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
	}
}

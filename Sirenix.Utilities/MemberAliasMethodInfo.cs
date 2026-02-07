using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x0200002A RID: 42
	public sealed class MemberAliasMethodInfo : MethodInfo
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x0000C765 File Offset: 0x0000A965
		public MemberAliasMethodInfo(MethodInfo method, string namePrefix)
		{
			this.aliasedMethod = method;
			this.mangledName = namePrefix + "+" + this.aliasedMethod.Name;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000C790 File Offset: 0x0000A990
		public MemberAliasMethodInfo(MethodInfo method, string namePrefix, string separatorString)
		{
			this.aliasedMethod = method;
			this.mangledName = namePrefix + separatorString + this.aliasedMethod.Name;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000C7B7 File Offset: 0x0000A9B7
		public MethodInfo AliasedMethod
		{
			get
			{
				return this.aliasedMethod;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000C7BF File Offset: 0x0000A9BF
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.aliasedMethod.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000C7CC File Offset: 0x0000A9CC
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.aliasedMethod.MethodHandle;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000C7D9 File Offset: 0x0000A9D9
		public override MethodAttributes Attributes
		{
			get
			{
				return this.aliasedMethod.Attributes;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000C7E6 File Offset: 0x0000A9E6
		public override Type ReturnType
		{
			get
			{
				return this.aliasedMethod.ReturnType;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000C7F3 File Offset: 0x0000A9F3
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedMethod.DeclaringType;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000C800 File Offset: 0x0000AA00
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000C808 File Offset: 0x0000AA08
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedMethod.ReflectedType;
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000C815 File Offset: 0x0000AA15
		public override MethodInfo GetBaseDefinition()
		{
			return this.aliasedMethod.GetBaseDefinition();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000C822 File Offset: 0x0000AA22
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedMethod.GetCustomAttributes(inherit);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000C830 File Offset: 0x0000AA30
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedMethod.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000C83F File Offset: 0x0000AA3F
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.aliasedMethod.GetMethodImplementationFlags();
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000C84C File Offset: 0x0000AA4C
		public override ParameterInfo[] GetParameters()
		{
			return this.aliasedMethod.GetParameters();
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000C859 File Offset: 0x0000AA59
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.aliasedMethod.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000C86D File Offset: 0x0000AA6D
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedMethod.IsDefined(attributeType, inherit);
		}

		// Token: 0x0400005D RID: 93
		private const string FAKE_NAME_SEPARATOR_STRING = "+";

		// Token: 0x0400005E RID: 94
		private MethodInfo aliasedMethod;

		// Token: 0x0400005F RID: 95
		private string mangledName;
	}
}

using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D4 RID: 212
	internal sealed class MemberAliasMethodInfo : MethodInfo
	{
		// Token: 0x0600062E RID: 1582 RVA: 0x0002A3AA File Offset: 0x000285AA
		public MemberAliasMethodInfo(MethodInfo method, string namePrefix)
		{
			this.aliasedMethod = method;
			this.mangledName = namePrefix + "+" + this.aliasedMethod.Name;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x0002A3D5 File Offset: 0x000285D5
		public MemberAliasMethodInfo(MethodInfo method, string namePrefix, string separatorString)
		{
			this.aliasedMethod = method;
			this.mangledName = namePrefix + separatorString + this.aliasedMethod.Name;
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0002A3FC File Offset: 0x000285FC
		public MethodInfo AliasedMethod
		{
			get
			{
				return this.aliasedMethod;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x0002A404 File Offset: 0x00028604
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.aliasedMethod.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x0002A411 File Offset: 0x00028611
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.aliasedMethod.MethodHandle;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x0002A41E File Offset: 0x0002861E
		public override MethodAttributes Attributes
		{
			get
			{
				return this.aliasedMethod.Attributes;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002A42B File Offset: 0x0002862B
		public override Type ReturnType
		{
			get
			{
				return this.aliasedMethod.ReturnType;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0002A438 File Offset: 0x00028638
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedMethod.DeclaringType;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000636 RID: 1590 RVA: 0x0002A445 File Offset: 0x00028645
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x0002A44D File Offset: 0x0002864D
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedMethod.ReflectedType;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0002A45A File Offset: 0x0002865A
		public override MethodInfo GetBaseDefinition()
		{
			return this.aliasedMethod.GetBaseDefinition();
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0002A467 File Offset: 0x00028667
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedMethod.GetCustomAttributes(inherit);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0002A475 File Offset: 0x00028675
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedMethod.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0002A484 File Offset: 0x00028684
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.aliasedMethod.GetMethodImplementationFlags();
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0002A491 File Offset: 0x00028691
		public override ParameterInfo[] GetParameters()
		{
			return this.aliasedMethod.GetParameters();
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0002A49E File Offset: 0x0002869E
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.aliasedMethod.Invoke(obj, invokeAttr, binder, parameters, culture);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0002A4B2 File Offset: 0x000286B2
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedMethod.IsDefined(attributeType, inherit);
		}

		// Token: 0x04000225 RID: 549
		private const string FAKE_NAME_SEPARATOR_STRING = "+";

		// Token: 0x04000226 RID: 550
		private MethodInfo aliasedMethod;

		// Token: 0x04000227 RID: 551
		private string mangledName;
	}
}

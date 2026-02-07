using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x0200002B RID: 43
	public sealed class MemberAliasPropertyInfo : PropertyInfo
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0000C87C File Offset: 0x0000AA7C
		public MemberAliasPropertyInfo(PropertyInfo prop, string namePrefix)
		{
			this.aliasedProperty = prop;
			this.mangledName = namePrefix + "+" + this.aliasedProperty.Name;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000C8A7 File Offset: 0x0000AAA7
		public MemberAliasPropertyInfo(PropertyInfo prop, string namePrefix, string separatorString)
		{
			this.aliasedProperty = prop;
			this.mangledName = namePrefix + separatorString + this.aliasedProperty.Name;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000C8CE File Offset: 0x0000AACE
		public PropertyInfo AliasedProperty
		{
			get
			{
				return this.aliasedProperty;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0000C8D6 File Offset: 0x0000AAD6
		public override Module Module
		{
			get
			{
				return this.aliasedProperty.Module;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600020E RID: 526 RVA: 0x0000C8E3 File Offset: 0x0000AAE3
		public override int MetadataToken
		{
			get
			{
				return this.aliasedProperty.MetadataToken;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000C8F0 File Offset: 0x0000AAF0
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000C8F8 File Offset: 0x0000AAF8
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedProperty.DeclaringType;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000C905 File Offset: 0x0000AB05
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedProperty.ReflectedType;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000C912 File Offset: 0x0000AB12
		public override Type PropertyType
		{
			get
			{
				return this.aliasedProperty.PropertyType;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000C91F File Offset: 0x0000AB1F
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.aliasedProperty.Attributes;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000C92C File Offset: 0x0000AB2C
		public override bool CanRead
		{
			get
			{
				return this.aliasedProperty.CanRead;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000C939 File Offset: 0x0000AB39
		public override bool CanWrite
		{
			get
			{
				return this.aliasedProperty.CanWrite;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000C946 File Offset: 0x0000AB46
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedProperty.GetCustomAttributes(inherit);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000C954 File Offset: 0x0000AB54
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedProperty.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000C963 File Offset: 0x0000AB63
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedProperty.IsDefined(attributeType, inherit);
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000C972 File Offset: 0x0000AB72
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return this.aliasedProperty.GetAccessors(nonPublic);
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000C980 File Offset: 0x0000AB80
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return this.aliasedProperty.GetGetMethod(nonPublic);
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C98E File Offset: 0x0000AB8E
		public override ParameterInfo[] GetIndexParameters()
		{
			return this.aliasedProperty.GetIndexParameters();
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C99B File Offset: 0x0000AB9B
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return this.aliasedProperty.GetSetMethod(nonPublic);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C9A9 File Offset: 0x0000ABA9
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return this.aliasedProperty.GetValue(obj, invokeAttr, binder, index, culture);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000C9BD File Offset: 0x0000ABBD
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			this.aliasedProperty.SetValue(obj, value, invokeAttr, binder, index, culture);
		}

		// Token: 0x04000060 RID: 96
		private const string FakeNameSeparatorString = "+";

		// Token: 0x04000061 RID: 97
		private PropertyInfo aliasedProperty;

		// Token: 0x04000062 RID: 98
		private string mangledName;
	}
}

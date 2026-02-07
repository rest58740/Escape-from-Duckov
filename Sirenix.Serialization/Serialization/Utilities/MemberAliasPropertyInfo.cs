using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D5 RID: 213
	internal sealed class MemberAliasPropertyInfo : PropertyInfo
	{
		// Token: 0x0600063F RID: 1599 RVA: 0x0002A4C1 File Offset: 0x000286C1
		public MemberAliasPropertyInfo(PropertyInfo prop, string namePrefix)
		{
			this.aliasedProperty = prop;
			this.mangledName = namePrefix + "+" + this.aliasedProperty.Name;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0002A4EC File Offset: 0x000286EC
		public MemberAliasPropertyInfo(PropertyInfo prop, string namePrefix, string separatorString)
		{
			this.aliasedProperty = prop;
			this.mangledName = namePrefix + separatorString + this.aliasedProperty.Name;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x0002A513 File Offset: 0x00028713
		public PropertyInfo AliasedProperty
		{
			get
			{
				return this.aliasedProperty;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000642 RID: 1602 RVA: 0x0002A51B File Offset: 0x0002871B
		public override Module Module
		{
			get
			{
				return this.aliasedProperty.Module;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0002A528 File Offset: 0x00028728
		public override int MetadataToken
		{
			get
			{
				return this.aliasedProperty.MetadataToken;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0002A535 File Offset: 0x00028735
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x0002A53D File Offset: 0x0002873D
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedProperty.DeclaringType;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000646 RID: 1606 RVA: 0x0002A54A File Offset: 0x0002874A
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedProperty.ReflectedType;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0002A557 File Offset: 0x00028757
		public override Type PropertyType
		{
			get
			{
				return this.aliasedProperty.PropertyType;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x0002A564 File Offset: 0x00028764
		public override PropertyAttributes Attributes
		{
			get
			{
				return this.aliasedProperty.Attributes;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0002A571 File Offset: 0x00028771
		public override bool CanRead
		{
			get
			{
				return this.aliasedProperty.CanRead;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0002A57E File Offset: 0x0002877E
		public override bool CanWrite
		{
			get
			{
				return this.aliasedProperty.CanWrite;
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0002A58B File Offset: 0x0002878B
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedProperty.GetCustomAttributes(inherit);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x0002A599 File Offset: 0x00028799
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedProperty.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x0002A5A8 File Offset: 0x000287A8
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedProperty.IsDefined(attributeType, inherit);
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002A5B7 File Offset: 0x000287B7
		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return this.aliasedProperty.GetAccessors(nonPublic);
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002A5C5 File Offset: 0x000287C5
		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return this.aliasedProperty.GetGetMethod(nonPublic);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0002A5D3 File Offset: 0x000287D3
		public override ParameterInfo[] GetIndexParameters()
		{
			return this.aliasedProperty.GetIndexParameters();
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x0002A5E0 File Offset: 0x000287E0
		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return this.aliasedProperty.GetSetMethod(nonPublic);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002A5EE File Offset: 0x000287EE
		public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			return this.aliasedProperty.GetValue(obj, invokeAttr, binder, index, culture);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0002A602 File Offset: 0x00028802
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
		{
			this.aliasedProperty.SetValue(obj, value, invokeAttr, binder, index, culture);
		}

		// Token: 0x04000228 RID: 552
		private const string FakeNameSeparatorString = "+";

		// Token: 0x04000229 RID: 553
		private PropertyInfo aliasedProperty;

		// Token: 0x0400022A RID: 554
		private string mangledName;
	}
}

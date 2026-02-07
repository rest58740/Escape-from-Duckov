using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Serialization.Utilities
{
	// Token: 0x020000D3 RID: 211
	internal sealed class MemberAliasFieldInfo : FieldInfo
	{
		// Token: 0x0600061E RID: 1566 RVA: 0x0002A29F File Offset: 0x0002849F
		public MemberAliasFieldInfo(FieldInfo field, string namePrefix)
		{
			this.aliasedField = field;
			this.mangledName = namePrefix + "+" + this.aliasedField.Name;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0002A2CA File Offset: 0x000284CA
		public MemberAliasFieldInfo(FieldInfo field, string namePrefix, string separatorString)
		{
			this.aliasedField = field;
			this.mangledName = namePrefix + separatorString + this.aliasedField.Name;
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0002A2F1 File Offset: 0x000284F1
		public FieldInfo AliasedField
		{
			get
			{
				return this.aliasedField;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0002A2F9 File Offset: 0x000284F9
		public override Module Module
		{
			get
			{
				return this.aliasedField.Module;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0002A306 File Offset: 0x00028506
		public override int MetadataToken
		{
			get
			{
				return this.aliasedField.MetadataToken;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x0002A313 File Offset: 0x00028513
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0002A31B File Offset: 0x0002851B
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedField.DeclaringType;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0002A328 File Offset: 0x00028528
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedField.ReflectedType;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0002A335 File Offset: 0x00028535
		public override Type FieldType
		{
			get
			{
				return this.aliasedField.FieldType;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0002A342 File Offset: 0x00028542
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.aliasedField.FieldHandle;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x0002A34F File Offset: 0x0002854F
		public override FieldAttributes Attributes
		{
			get
			{
				return this.aliasedField.Attributes;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002A35C File Offset: 0x0002855C
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedField.GetCustomAttributes(inherit);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0002A36A File Offset: 0x0002856A
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedField.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0002A379 File Offset: 0x00028579
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedField.IsDefined(attributeType, inherit);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x0002A388 File Offset: 0x00028588
		public override object GetValue(object obj)
		{
			return this.aliasedField.GetValue(obj);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0002A396 File Offset: 0x00028596
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.aliasedField.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x04000222 RID: 546
		private const string FAKE_NAME_SEPARATOR_STRING = "+";

		// Token: 0x04000223 RID: 547
		private FieldInfo aliasedField;

		// Token: 0x04000224 RID: 548
		private string mangledName;
	}
}

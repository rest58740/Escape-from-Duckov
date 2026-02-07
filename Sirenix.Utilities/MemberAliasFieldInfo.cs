using System;
using System.Globalization;
using System.Reflection;

namespace Sirenix.Utilities
{
	// Token: 0x02000029 RID: 41
	public sealed class MemberAliasFieldInfo : FieldInfo
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x0000C65A File Offset: 0x0000A85A
		public MemberAliasFieldInfo(FieldInfo field, string namePrefix)
		{
			this.aliasedField = field;
			this.mangledName = namePrefix + "+" + this.aliasedField.Name;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000C685 File Offset: 0x0000A885
		public MemberAliasFieldInfo(FieldInfo field, string namePrefix, string separatorString)
		{
			this.aliasedField = field;
			this.mangledName = namePrefix + separatorString + this.aliasedField.Name;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000C6AC File Offset: 0x0000A8AC
		public FieldInfo AliasedField
		{
			get
			{
				return this.aliasedField;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public override Module Module
		{
			get
			{
				return this.aliasedField.Module;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000C6C1 File Offset: 0x0000A8C1
		public override int MetadataToken
		{
			get
			{
				return this.aliasedField.MetadataToken;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000C6CE File Offset: 0x0000A8CE
		public override string Name
		{
			get
			{
				return this.mangledName;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000C6D6 File Offset: 0x0000A8D6
		public override Type DeclaringType
		{
			get
			{
				return this.aliasedField.DeclaringType;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000C6E3 File Offset: 0x0000A8E3
		public override Type ReflectedType
		{
			get
			{
				return this.aliasedField.ReflectedType;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000C6F0 File Offset: 0x0000A8F0
		public override Type FieldType
		{
			get
			{
				return this.aliasedField.FieldType;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000C6FD File Offset: 0x0000A8FD
		public override RuntimeFieldHandle FieldHandle
		{
			get
			{
				return this.aliasedField.FieldHandle;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000C70A File Offset: 0x0000A90A
		public override FieldAttributes Attributes
		{
			get
			{
				return this.aliasedField.Attributes;
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C717 File Offset: 0x0000A917
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.aliasedField.GetCustomAttributes(inherit);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000C725 File Offset: 0x0000A925
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.aliasedField.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000C734 File Offset: 0x0000A934
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.aliasedField.IsDefined(attributeType, inherit);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000C743 File Offset: 0x0000A943
		public override object GetValue(object obj)
		{
			return this.aliasedField.GetValue(obj);
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000C751 File Offset: 0x0000A951
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
		{
			this.aliasedField.SetValue(obj, value, invokeAttr, binder, culture);
		}

		// Token: 0x0400005A RID: 90
		private const string FAKE_NAME_SEPARATOR_STRING = "+";

		// Token: 0x0400005B RID: 91
		private FieldInfo aliasedField;

		// Token: 0x0400005C RID: 92
		private string mangledName;
	}
}

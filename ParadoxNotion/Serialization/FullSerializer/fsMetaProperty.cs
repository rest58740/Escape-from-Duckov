using System;
using System.Reflection;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000AA RID: 170
	public class fsMetaProperty
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00012F07 File Offset: 0x00011107
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x00012F0F File Offset: 0x0001110F
		public FieldInfo Field { get; private set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00012F18 File Offset: 0x00011118
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x00012F20 File Offset: 0x00011120
		public string JsonName { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00012F29 File Offset: 0x00011129
		public Type StorageType
		{
			get
			{
				return this.Field.FieldType;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00012F36 File Offset: 0x00011136
		public string MemberName
		{
			get
			{
				return this.Field.Name;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x00012F43 File Offset: 0x00011143
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x00012F4B File Offset: 0x0001114B
		public bool ReadOnly { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00012F54 File Offset: 0x00011154
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x00012F5C File Offset: 0x0001115C
		public bool WriteOnly { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x00012F65 File Offset: 0x00011165
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x00012F6D File Offset: 0x0001116D
		public bool AutoInstance { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x00012F76 File Offset: 0x00011176
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00012F7E File Offset: 0x0001117E
		public bool AsReference { get; private set; }

		// Token: 0x06000666 RID: 1638 RVA: 0x00012F88 File Offset: 0x00011188
		internal fsMetaProperty(FieldInfo field)
		{
			this.Field = field;
			fsSerializeAsAttribute fsSerializeAsAttribute = this.Field.RTGetAttribute(true);
			this.JsonName = ((fsSerializeAsAttribute != null && !string.IsNullOrEmpty(fsSerializeAsAttribute.Name)) ? fsSerializeAsAttribute.Name : field.Name);
			this.ReadOnly = this.Field.RTIsDefined(true);
			this.WriteOnly = this.Field.RTIsDefined(true);
			fsAutoInstance fsAutoInstance = this.StorageType.RTGetAttribute(true);
			this.AutoInstance = (fsAutoInstance != null && fsAutoInstance.makeInstance && !this.StorageType.IsAbstract);
			this.AsReference = this.Field.RTIsDefined(true);
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00013038 File Offset: 0x00011238
		public object Read(object context)
		{
			return this.Field.GetValue(context);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00013046 File Offset: 0x00011246
		public void Write(object context, object value)
		{
			this.Field.SetValue(context, value);
		}
	}
}

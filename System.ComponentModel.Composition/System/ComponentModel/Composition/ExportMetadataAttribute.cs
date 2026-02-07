using System;

namespace System.ComponentModel.Composition
{
	// Token: 0x0200003A RID: 58
	[AttributeUsage(1476, AllowMultiple = true, Inherited = false)]
	public sealed class ExportMetadataAttribute : Attribute
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00005955 File Offset: 0x00003B55
		public ExportMetadataAttribute(string name, object value)
		{
			this.Name = (name ?? string.Empty);
			this.Value = value;
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00005974 File Offset: 0x00003B74
		// (set) Token: 0x060001BD RID: 445 RVA: 0x0000597C File Offset: 0x00003B7C
		public string Name { get; private set; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00005985 File Offset: 0x00003B85
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000598D File Offset: 0x00003B8D
		public object Value { get; private set; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00005996 File Offset: 0x00003B96
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000599E File Offset: 0x00003B9E
		public bool IsMultiple { get; set; }
	}
}

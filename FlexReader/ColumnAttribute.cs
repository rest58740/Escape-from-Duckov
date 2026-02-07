using System;

namespace FlexFramework.Excel
{
	// Token: 0x0200001C RID: 28
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
	public sealed class ColumnAttribute : Attribute
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000485D File Offset: 0x00002A5D
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004865 File Offset: 0x00002A65
		public int Column { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000486E File Offset: 0x00002A6E
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004876 File Offset: 0x00002A76
		public object Default { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000CE RID: 206 RVA: 0x0000487F File Offset: 0x00002A7F
		// (set) Token: 0x060000CF RID: 207 RVA: 0x00004887 File Offset: 0x00002A87
		public bool Fallback { get; private set; }

		// Token: 0x060000D0 RID: 208 RVA: 0x00004890 File Offset: 0x00002A90
		public ColumnAttribute(int column)
		{
			this.Column = column;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x0000489F File Offset: 0x00002A9F
		public ColumnAttribute(int column, object @default) : this(column)
		{
			this.Default = @default;
			this.Fallback = true;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000048B6 File Offset: 0x00002AB6
		public ColumnAttribute(string column) : this(Address.ParseColumn(column))
		{
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000048C4 File Offset: 0x00002AC4
		public ColumnAttribute(string column, object @default) : this(Address.ParseColumn(column), @default)
		{
		}
	}
}

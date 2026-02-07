using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000017 RID: 23
	public abstract class Table : ReadOnlyCollection<Row>, ICloneable<Table>
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00003C89 File Offset: 0x00001E89
		public Table(IList<Row> list) : base(list)
		{
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00003C92 File Offset: 0x00001E92
		public IList<Row> Rows
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x1700001B RID: 27
		public Cell this[string address]
		{
			get
			{
				if (!Address.IsValid(address))
				{
					throw new FormatException();
				}
				return this[new Address(address)];
			}
		}

		// Token: 0x1700001C RID: 28
		public Cell this[Address address]
		{
			get
			{
				return this.SelectMany((Row row) => row).First((Cell cell) => cell.Address == address);
			}
		}

		// Token: 0x1700001D RID: 29
		public IEnumerable<Cell> this[Range range]
		{
			get
			{
				return from cell in this.SelectMany((Row row) => row)
				where range.Contains(cell.Address)
				select cell;
			}
		}

		// Token: 0x060000A9 RID: 169
		public abstract Table DeepClone();

		// Token: 0x060000AA RID: 170
		public abstract Table ShallowClone();
	}
}

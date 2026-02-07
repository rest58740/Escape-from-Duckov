using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x02000016 RID: 22
	public class Row : ReadOnlyCollection<Cell>, ICloneable<Row>
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003BB1 File Offset: 0x00001DB1
		public IList<Cell> Cells
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x17000017 RID: 23
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

		// Token: 0x17000018 RID: 24
		public Cell this[Address address]
		{
			get
			{
				return this.First((Cell cell) => cell.Address == address);
			}
		}

		// Token: 0x17000019 RID: 25
		public IEnumerable<Cell> this[Range range]
		{
			get
			{
				return from cell in this
				where range.Contains(cell.Address)
				select cell;
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00003C30 File Offset: 0x00001E30
		public Row(IList<Cell> list) : base(list)
		{
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003C39 File Offset: 0x00001E39
		public Row() : base(new List<Cell>())
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003C46 File Offset: 0x00001E46
		public Row DeepClone()
		{
			return new Row((from cell in base.Items
			select cell.DeepClone()).ToList<Cell>());
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003C7C File Offset: 0x00001E7C
		public Row ShallowClone()
		{
			return (Row)base.MemberwiseClone();
		}
	}
}

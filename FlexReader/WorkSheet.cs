using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x0200001B RID: 27
	public sealed class WorkSheet : Table
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004678 File Offset: 0x00002878
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00004680 File Offset: 0x00002880
		public string ID { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004689 File Offset: 0x00002889
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004691 File Offset: 0x00002891
		public string Name { get; private set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000469A File Offset: 0x0000289A
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x000046A2 File Offset: 0x000028A2
		public ReadOnlyCollection<Range> Spans { get; private set; }

		// Token: 0x060000C5 RID: 197 RVA: 0x000046AB File Offset: 0x000028AB
		public WorkSheet(string id, string name, IList<Row> rows) : base(rows)
		{
			this.ID = id;
			this.Name = name;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000046C2 File Offset: 0x000028C2
		public WorkSheet(string id, string name, IList<Row> rows, IList<Range> spans) : base(rows)
		{
			this.ID = id;
			this.Name = name;
			this.Spans = new ReadOnlyCollection<Range>(spans);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000046E8 File Offset: 0x000028E8
		public void Merge()
		{
			using (IEnumerator<Row> enumerator = base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					foreach (Cell cell in from c in enumerator.Current
					where c.IsSpan
					select c)
					{
						foreach (Range range in this.Spans)
						{
							if (range.Contains(cell.Address))
							{
								cell.Value = base[range.From].Value;
								cell.IsSpan = false;
								break;
							}
						}
					}
				}
			}
			this.Spans = new List<Range>().AsReadOnly();
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x000047F8 File Offset: 0x000029F8
		public override Table DeepClone()
		{
			return new WorkSheet(this.ID, this.Name, (from row in base.Items
			select row.DeepClone()).ToList<Row>(), this.Spans.ToList<Range>());
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004850 File Offset: 0x00002A50
		public override Table ShallowClone()
		{
			return (Table)base.MemberwiseClone();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexFramework.Excel
{
	// Token: 0x0200000E RID: 14
	public static class Extensions
	{
		// Token: 0x06000020 RID: 32 RVA: 0x000028D6 File Offset: 0x00000AD6
		public static object Convert(this Cell cell, Type type)
		{
			return ValueConverter.Convert(type, cell.Text);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000028E4 File Offset: 0x00000AE4
		public static T Convert<T>(this Cell cell)
		{
			return (T)((object)cell.Convert(typeof(T)));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028FB File Offset: 0x00000AFB
		public static T Convert<T>(this Row row, IGenerator<T> generator) where T : new()
		{
			return generator.Instantiate(row);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002904 File Offset: 0x00000B04
		public static T Convert<T>(this Row row, string expression) where T : new()
		{
			return row.Convert(new Mapper<T>().Map(expression));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002917 File Offset: 0x00000B17
		public static T Convert<T>(this Row row, IEnumerable<Cell> cols) where T : new()
		{
			return row.Convert(new Mapper<T>().Map(cols));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000292A File Offset: 0x00000B2A
		public static object Convert(this Row row, Type type, string expression)
		{
			return row.Convert(new Mapper(type).Map(expression));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000293E File Offset: 0x00000B3E
		public static object Convert(this Row row, Type type, IEnumerable<Cell> cols)
		{
			return row.Convert(new Mapper(type).Map(cols));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002952 File Offset: 0x00000B52
		public static object Convert(this Row row, IGenerator generator)
		{
			return generator.Instantiate(row);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000295C File Offset: 0x00000B5C
		public static IEnumerable<T> Convert<T>(this Table table) where T : new()
		{
			TableMapper<T> tableMapper = new TableMapper<T>();
			tableMapper.Extract();
			return table.Convert(tableMapper);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000297C File Offset: 0x00000B7C
		public static IEnumerable<object> Convert(this Table table, Type type)
		{
			TableMapper tableMapper = new TableMapper(type);
			tableMapper.Extract();
			return table.Convert(tableMapper);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000299D File Offset: 0x00000B9D
		public static IEnumerable<T> Convert<T>(this Table table, ITableGenerator<T> generator) where T : new()
		{
			return generator.Instantiate(table);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000029A6 File Offset: 0x00000BA6
		public static IEnumerable<T> Convert<T>(this Table table, int row) where T : new()
		{
			if (row < 1)
			{
				throw new ArgumentException("One-based row index must be greater than 0");
			}
			return ((ITableGenerator<T>)new TableMapper<T>().Map(table[row - 1]).Exclude(new int[]
			{
				row
			})).Instantiate(table);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000029E0 File Offset: 0x00000BE0
		public static IEnumerable<T> Convert<T>(this Table table, string expression, params int[] exclude) where T : new()
		{
			TableMapper<T> tableMapper = new TableMapper<T>().Map(expression);
			if (exclude != null && exclude.Length != 0)
			{
				tableMapper.Exclude(exclude);
			}
			return table.Convert(tableMapper);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A10 File Offset: 0x00000C10
		public static IEnumerable<object> Convert(this Table table, Type type, string expression, params int[] exclude)
		{
			TableMapper tableMapper = new TableMapper(type).Map(expression);
			if (exclude != null && exclude.Length != 0)
			{
				tableMapper.Exclude(exclude);
			}
			return table.Convert(tableMapper);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A40 File Offset: 0x00000C40
		public static IEnumerable<object> Convert(this Table table, ITableGenerator generator)
		{
			return generator.Instantiate(table);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002A49 File Offset: 0x00000C49
		public static IEnumerable<object> Convert(this Table table, Type type, int row)
		{
			if (row < 1)
			{
				throw new ArgumentException("One-based row index must be greater than 0");
			}
			return ((ITableGenerator)new TableMapper(type).Map(table[row - 1]).Exclude(new int[]
			{
				row
			})).Instantiate(table);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A84 File Offset: 0x00000C84
		public static string Dump(this Row row, char delimiter, char enclose)
		{
			return (from c in row.Cells
			select c.Dump(delimiter, enclose)).Aggregate(string.Empty, delegate(string a, string b)
			{
				if (!string.IsNullOrEmpty(a))
				{
					return a + delimiter.ToString() + b;
				}
				return b;
			});
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002AD2 File Offset: 0x00000CD2
		public static string Dump(this Row row)
		{
			return row.Dump(Document.Delimiter, Document.Enclose);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002AE4 File Offset: 0x00000CE4
		public static string Dump(this Cell cell, char delimiter, char enclose)
		{
			string text = cell.Text;
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			if (text.Contains(enclose) || text.Contains(delimiter))
			{
				return enclose.ToString() + text.Replace(enclose.ToString(), enclose.ToString() + enclose.ToString()) + enclose.ToString();
			}
			return text;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B4D File Offset: 0x00000D4D
		public static string Dump(this Cell cell)
		{
			return cell.Dump(Document.Delimiter, Document.Enclose);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B60 File Offset: 0x00000D60
		public static string Dump(this Table table, char delimiter, char enclose)
		{
			return (from r in table
			select r.Dump(delimiter, enclose)).Aggregate(string.Empty, delegate(string a, string b)
			{
				if (!string.IsNullOrEmpty(a))
				{
					return a + Environment.NewLine + b;
				}
				return b;
			});
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BBC File Offset: 0x00000DBC
		public static string Dump(this Table table)
		{
			return table.Dump(Document.Delimiter, Document.Enclose);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002BD0 File Offset: 0x00000DD0
		public static Cell Select(this Row row, string address)
		{
			if (!Address.IsValid(address))
			{
				throw new FormatException();
			}
			Address ad = new Address(address);
			return row.FirstOrDefault((Cell cell) => cell.Address == ad);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002C0F File Offset: 0x00000E0F
		public static IEnumerable<Cell> SelectRange(this Row row, string range)
		{
			if (!Range.IsValid(range))
			{
				throw new FormatException();
			}
			return row[new Range(range)];
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002C2C File Offset: 0x00000E2C
		public static Cell Select(this Table table, string address)
		{
			if (!Address.IsValid(address))
			{
				throw new FormatException();
			}
			Address ad = new Address(address);
			return table.SelectMany((Row row) => row).FirstOrDefault((Cell cell) => cell.Address == ad);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002C8F File Offset: 0x00000E8F
		public static IEnumerable<Cell> SelectRange(this Table table, string range)
		{
			if (!Range.IsValid(range))
			{
				throw new FormatException();
			}
			return table[new Range(range)];
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002CAC File Offset: 0x00000EAC
		public static Cell Select(this WorkBook book, string path)
		{
			string[] args = path.Split(new char[]
			{
				'!'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (args.Length != 2)
			{
				throw new FormatException();
			}
			WorkSheet workSheet = book.FirstOrDefault((WorkSheet s) => s.Name == args[0]);
			if (workSheet != null)
			{
				return workSheet.Select(args[1]);
			}
			return null;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D10 File Offset: 0x00000F10
		public static IEnumerable<Cell> SelectRange(this WorkBook book, string path)
		{
			string[] args = path.Split(new char[]
			{
				'!'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (args.Length != 2)
			{
				throw new FormatException();
			}
			WorkSheet workSheet = book.FirstOrDefault((WorkSheet s) => s.Name == args[0]);
			if (workSheet != null)
			{
				return workSheet.SelectRange(args[1]);
			}
			return null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002D74 File Offset: 0x00000F74
		public static void Recalculate(this Table table)
		{
			for (int i = 0; i < table.Count; i++)
			{
				for (int j = 0; j < table[i].Count; j++)
				{
					table[i][j].Address = new Address(j + 1, i + 1);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public static Table Expand(this Table table)
		{
			Table table2 = table.DeepClone();
			int num = table2.Max((Row row) => row.Count);
			for (int i = 0; i < table2.Count; i++)
			{
				for (int j = num - table2[i].Count; j > 0; j--)
				{
					table2[i].Cells.Add(new Cell());
				}
			}
			table2.Recalculate();
			return table2;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E4C File Offset: 0x0000104C
		public static Table Collapse(this Table table)
		{
			Table table2 = table.DeepClone();
			for (int i = 0; i < table2.Count; i++)
			{
				int num = 0;
				while (num < table2[i].Count && string.IsNullOrEmpty(table2[i][num].Text))
				{
					table2[i].Cells.RemoveAt(num--);
					num++;
				}
				int num2 = table2[i].Count - 1;
				while (num2 >= 0 && string.IsNullOrEmpty(table2[i][num2].Text))
				{
					table2[i].Cells.RemoveAt(num2);
					num2--;
				}
			}
			for (int j = table2.Count - 1; j >= 0; j--)
			{
				if (table2[j].Count == 0)
				{
					table2.Rows.RemoveAt(j);
				}
			}
			table2.Recalculate();
			return table2;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002F3C File Offset: 0x0000113C
		public static Table Rotate(this Table table)
		{
			Table table2 = table.Expand();
			if (table2.Count == 0)
			{
				return table2;
			}
			int count = table2[0].Count;
			if (count == 0)
			{
				return table2;
			}
			int count2 = table2.Count;
			Table table3 = table.DeepClone();
			table3.Rows.Clear();
			for (int i = 0; i < count; i++)
			{
				Row row = new Row();
				for (int j = count2 - 1; j >= 0; j--)
				{
					row.Cells.Add(table2[j][i]);
				}
				table3.Rows.Add(row);
			}
			table3.Recalculate();
			return table3;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FDD File Offset: 0x000011DD
		public static bool IsEmpty(this Row row)
		{
			if (row.Count != 0)
			{
				return row.All((Cell col) => string.IsNullOrEmpty(col.Text));
			}
			return true;
		}
	}
}

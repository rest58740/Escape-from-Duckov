using System;
using System.Text.RegularExpressions;

namespace FlexFramework.Excel
{
	// Token: 0x02000012 RID: 18
	public struct Address : IEquatable<Address>
	{
		// Token: 0x06000051 RID: 81 RVA: 0x00003414 File Offset: 0x00001614
		bool IEquatable<Address>.Equals(Address other)
		{
			return this.Column == other.Column && this.Row == other.Row;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003436 File Offset: 0x00001636
		public override bool Equals(object obj)
		{
			return obj != null && obj is Address && this.Equals((Address)obj);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000345E File Offset: 0x0000165E
		public override int GetHashCode()
		{
			return this.Column * 397 ^ this.Row;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003473 File Offset: 0x00001673
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000347B File Offset: 0x0000167B
		public int Column { readonly get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003484 File Offset: 0x00001684
		// (set) Token: 0x06000057 RID: 87 RVA: 0x0000348C File Offset: 0x0000168C
		public int Row { readonly get; private set; }

		// Token: 0x06000058 RID: 88 RVA: 0x00003498 File Offset: 0x00001698
		public Address(string address)
		{
			Match match = Regex.Match(address, "^[A-Z]+");
			Match match2 = Regex.Match(address, "\\d+$");
			if (!match.Success || !match2.Success)
			{
				throw new FormatException("Invalid address: " + address);
			}
			this.Column = Address.ParseColumn(match.Value);
			this.Row = int.Parse(match2.Value);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003500 File Offset: 0x00001700
		public Address(int column, int row)
		{
			this.Column = column;
			this.Row = row;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003510 File Offset: 0x00001710
		public string ColumnName
		{
			get
			{
				return Address.ParseColumn(this.Column);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000351D File Offset: 0x0000171D
		public override string ToString()
		{
			return string.Format("{0}{1}", this.ColumnName, this.Row);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000353A File Offset: 0x0000173A
		public static bool operator ==(Address address, Address other)
		{
			return address.Row == other.Row && address.Column == other.Column;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000355E File Offset: 0x0000175E
		public static bool operator !=(Address address, Address other)
		{
			return address.Row != other.Row || address.Column != other.Column;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003585 File Offset: 0x00001785
		public static bool operator >=(Address address, Address other)
		{
			return address.Row >= other.Row && address.Column >= other.Column;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000035AC File Offset: 0x000017AC
		public static bool operator <=(Address address, Address other)
		{
			return address.Row <= other.Row && address.Column <= other.Column;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000035D3 File Offset: 0x000017D3
		public static bool operator >(Address address, Address other)
		{
			return address.Row > other.Row && address.Column > other.Column;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000035F7 File Offset: 0x000017F7
		public static bool operator <(Address address, Address other)
		{
			return address.Row < other.Row && address.Column < other.Column;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x0000361B File Offset: 0x0000181B
		public static Range operator +(Address from, Address to)
		{
			return new Range(from, to);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003624 File Offset: 0x00001824
		public static Address operator >>(Address address, int column)
		{
			return new Address(address.Column + column, address.Row);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000363B File Offset: 0x0000183B
		public static Address operator <<(Address address, int column)
		{
			return new Address(address.Column - column, address.Row);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003652 File Offset: 0x00001852
		public static Address operator +(Address address, int row)
		{
			return new Address(address.Column, address.Row + row);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003669 File Offset: 0x00001869
		public static Address operator -(Address address, int row)
		{
			return new Address(address.Column, address.Row - row);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003680 File Offset: 0x00001880
		public static string ParseColumn(int column)
		{
			if (column <= 0)
			{
				throw new ArgumentException("Column value must be greater than 0");
			}
			if (column <= 26)
			{
				return Convert.ToChar(column + 64).ToString();
			}
			int num = column / 26;
			int num2 = column % 26;
			if (num2 != 0)
			{
				return Address.ParseColumn(num) + Address.ParseColumn(num2);
			}
			num2 = 26;
			num--;
			return Address.ParseColumn(num) + Address.ParseColumn(num2);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000036EC File Offset: 0x000018EC
		public static int ParseColumn(string column)
		{
			if (!Regex.IsMatch(column, "^[A-Z]+$"))
			{
				throw new FormatException("Invalid address: " + column);
			}
			int[] array = new int[column.Length];
			for (int i = 0; i < column.Length; i++)
			{
				array[i] = Convert.ToInt32(column[i]) - 64;
			}
			int num = 1;
			int num2 = 0;
			for (int j = array.Length - 1; j >= 0; j--)
			{
				num2 += array[j] * num;
				num *= 26;
			}
			return num2;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000376D File Offset: 0x0000196D
		public static bool IsValid(string address)
		{
			return Regex.IsMatch(address, "^[A-Z]+\\d+$");
		}
	}
}

using System;
using System.Text.RegularExpressions;

namespace FlexFramework.Excel
{
	// Token: 0x02000015 RID: 21
	public struct Range : IEquatable<Range>
	{
		// Token: 0x0600008D RID: 141 RVA: 0x0000398C File Offset: 0x00001B8C
		public bool Equals(Range other)
		{
			return this.From.Equals(other.From) && this.To.Equals(other.To);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000039DD File Offset: 0x00001BDD
		public override bool Equals(object obj)
		{
			return obj != null && obj is Range && this.Equals((Range)obj);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000039FC File Offset: 0x00001BFC
		public override int GetHashCode()
		{
			return this.From.GetHashCode() * 397 ^ this.To.GetHashCode();
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00003A38 File Offset: 0x00001C38
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00003A40 File Offset: 0x00001C40
		public Address From { readonly get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003A49 File Offset: 0x00001C49
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00003A51 File Offset: 0x00001C51
		public Address To { readonly get; private set; }

		// Token: 0x06000094 RID: 148 RVA: 0x00003A5A File Offset: 0x00001C5A
		public Range(Address from, Address to)
		{
			if (from >= to)
			{
				throw new ArgumentException("begin address is larger than or equal to end address");
			}
			this.From = from;
			this.To = to;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003A80 File Offset: 0x00001C80
		public Range(string range)
		{
			string[] array = range.Split(new char[]
			{
				':'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 2)
			{
				throw new FormatException();
			}
			Address address = new Address(array[0]);
			Address address2 = new Address(array[1]);
			if (address >= address2)
			{
				throw new ArgumentException("begin address is larger than or equal to end address");
			}
			this.From = address;
			this.To = address2;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003AE2 File Offset: 0x00001CE2
		public bool Contains(Address address)
		{
			return address >= this.From && address <= this.To;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003B00 File Offset: 0x00001D00
		public bool Contains(Range range)
		{
			return range.From >= this.From && range.To <= this.To;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003B2A File Offset: 0x00001D2A
		public override string ToString()
		{
			return string.Format("{0}:{1}", this.From, this.To);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003B4C File Offset: 0x00001D4C
		public static bool operator ==(Range range, Range other)
		{
			return range.From == other.From && range.To == other.To;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003B78 File Offset: 0x00001D78
		public static bool operator !=(Range range, Range other)
		{
			return range.From != other.From || range.To != other.To;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003BA4 File Offset: 0x00001DA4
		public static bool IsValid(string range)
		{
			return Regex.IsMatch(range, "^[A-Z]+\\d+:[A-Z]+\\d+$");
		}
	}
}

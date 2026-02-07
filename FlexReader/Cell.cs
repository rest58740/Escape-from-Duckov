using System;

namespace FlexFramework.Excel
{
	// Token: 0x02000013 RID: 19
	public class Cell : ICloneable<Cell>
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000377A File Offset: 0x0000197A
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00003782 File Offset: 0x00001982
		public Address Address { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000378B File Offset: 0x0000198B
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00003793 File Offset: 0x00001993
		public object Value { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000379C File Offset: 0x0000199C
		// (set) Token: 0x0600006F RID: 111 RVA: 0x000037A4 File Offset: 0x000019A4
		public bool IsSpan { get; set; }

		// Token: 0x06000070 RID: 112 RVA: 0x000037AD File Offset: 0x000019AD
		public Cell()
		{
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000037B5 File Offset: 0x000019B5
		public Cell(Address address) : this()
		{
			this.Address = address;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000037C4 File Offset: 0x000019C4
		public Cell(string value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000037D4 File Offset: 0x000019D4
		public Cell(bool value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000037E9 File Offset: 0x000019E9
		public Cell(int value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000037FE File Offset: 0x000019FE
		public Cell(long value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003813 File Offset: 0x00001A13
		public Cell(float value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003828 File Offset: 0x00001A28
		public Cell(double value, Address address) : this(address)
		{
			this.Value = value;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000078 RID: 120 RVA: 0x0000383D File Offset: 0x00001A3D
		public virtual string Text
		{
			get
			{
				return Convert.ToString(this.Value) ?? string.Empty;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003853 File Offset: 0x00001A53
		public string String
		{
			get
			{
				if (!this.IsString)
				{
					throw new InvalidCastException();
				}
				return (string)this.Value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600007A RID: 122 RVA: 0x0000386E File Offset: 0x00001A6E
		public int Integer
		{
			get
			{
				if (!this.IsInteger)
				{
					throw new InvalidCastException();
				}
				return (int)this.Value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003889 File Offset: 0x00001A89
		public bool Boolean
		{
			get
			{
				if (!this.IsBoolean)
				{
					throw new InvalidCastException();
				}
				return (bool)this.Value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600007C RID: 124 RVA: 0x000038A4 File Offset: 0x00001AA4
		public float Single
		{
			get
			{
				if (this.IsSingle)
				{
					return (float)this.Value;
				}
				if (!this.IsInteger)
				{
					throw new InvalidCastException();
				}
				return (float)((int)this.Value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600007D RID: 125 RVA: 0x000038D4 File Offset: 0x00001AD4
		public double Double
		{
			get
			{
				if (!this.IsDouble)
				{
					throw new InvalidCastException();
				}
				return (double)this.Value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000038EF File Offset: 0x00001AEF
		public bool IsInteger
		{
			get
			{
				return this.Value is int;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000038FF File Offset: 0x00001AFF
		public bool IsSingle
		{
			get
			{
				return this.Value is float;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000390F File Offset: 0x00001B0F
		public bool IsDouble
		{
			get
			{
				return this.Value is double;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000081 RID: 129 RVA: 0x0000391F File Offset: 0x00001B1F
		public bool IsBoolean
		{
			get
			{
				return this.Value is bool;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000082 RID: 130 RVA: 0x0000392F File Offset: 0x00001B2F
		public bool IsString
		{
			get
			{
				return this.Value is string;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000393F File Offset: 0x00001B3F
		public override string ToString()
		{
			return this.Text;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003947 File Offset: 0x00001B47
		public Cell DeepClone()
		{
			return (Cell)base.MemberwiseClone();
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003954 File Offset: 0x00001B54
		public Cell ShallowClone()
		{
			return (Cell)base.MemberwiseClone();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003961 File Offset: 0x00001B61
		public static implicit operator string(Cell cell)
		{
			return cell.String;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003969 File Offset: 0x00001B69
		public static implicit operator int(Cell cell)
		{
			return cell.Integer;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003971 File Offset: 0x00001B71
		public static implicit operator bool(Cell cell)
		{
			return cell.Boolean;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003979 File Offset: 0x00001B79
		public static implicit operator float(Cell cell)
		{
			return cell.Single;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003981 File Offset: 0x00001B81
		public static implicit operator double(Cell cell)
		{
			return cell.Double;
		}
	}
}

using System;
using System.Collections.Generic;

namespace ParadoxNotion.Serialization.FullSerializer
{
	// Token: 0x020000A4 RID: 164
	public sealed class fsData
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x000116D8 File Offset: 0x0000F8D8
		public fsDataType Type
		{
			get
			{
				if (this._value == null)
				{
					return fsDataType.Null;
				}
				if (this._value is double)
				{
					return fsDataType.Double;
				}
				if (this._value is long)
				{
					return fsDataType.Int64;
				}
				if (this._value is bool)
				{
					return fsDataType.Boolean;
				}
				if (this._value is string)
				{
					return fsDataType.String;
				}
				if (this._value is Dictionary<string, fsData>)
				{
					return fsDataType.Object;
				}
				if (this._value is List<fsData>)
				{
					return fsDataType.Array;
				}
				throw new InvalidOperationException("unknown JSON data type");
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00011753 File Offset: 0x0000F953
		public fsData()
		{
			this._value = null;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00011762 File Offset: 0x0000F962
		public fsData(bool boolean)
		{
			this._value = boolean;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00011776 File Offset: 0x0000F976
		public fsData(double f)
		{
			this._value = f;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001178A File Offset: 0x0000F98A
		public fsData(long i)
		{
			this._value = i;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001179E File Offset: 0x0000F99E
		public fsData(string str)
		{
			this._value = str;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000117AD File Offset: 0x0000F9AD
		public fsData(Dictionary<string, fsData> dict)
		{
			this._value = dict;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x000117BC File Offset: 0x0000F9BC
		public fsData(List<fsData> list)
		{
			this._value = list;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x000117CB File Offset: 0x0000F9CB
		public static fsData CreateDictionary()
		{
			return new fsData(new Dictionary<string, fsData>(fsGlobalConfig.IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase));
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x000117EA File Offset: 0x0000F9EA
		public static fsData CreateList()
		{
			return new fsData(new List<fsData>());
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000117F6 File Offset: 0x0000F9F6
		public static fsData CreateList(int capacity)
		{
			return new fsData(new List<fsData>(capacity));
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00011803 File Offset: 0x0000FA03
		internal void BecomeDictionary()
		{
			this._value = new Dictionary<string, fsData>(StringComparer.Ordinal);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00011815 File Offset: 0x0000FA15
		internal fsData Clone()
		{
			return new fsData
			{
				_value = this._value
			};
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x00011828 File Offset: 0x0000FA28
		public bool IsNull
		{
			get
			{
				return this._value == null;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00011833 File Offset: 0x0000FA33
		public bool IsDouble
		{
			get
			{
				return this._value is double;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00011843 File Offset: 0x0000FA43
		public bool IsInt64
		{
			get
			{
				return this._value is long;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00011853 File Offset: 0x0000FA53
		public bool IsBool
		{
			get
			{
				return this._value is bool;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600061C RID: 1564 RVA: 0x00011863 File Offset: 0x0000FA63
		public bool IsString
		{
			get
			{
				return this._value is string;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00011873 File Offset: 0x0000FA73
		public bool IsDictionary
		{
			get
			{
				return this._value is Dictionary<string, fsData>;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00011883 File Offset: 0x0000FA83
		public bool IsList
		{
			get
			{
				return this._value is List<fsData>;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00011893 File Offset: 0x0000FA93
		public double AsDouble
		{
			get
			{
				return this.Cast<double>();
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001189B File Offset: 0x0000FA9B
		public long AsInt64
		{
			get
			{
				return this.Cast<long>();
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x000118A3 File Offset: 0x0000FAA3
		public bool AsBool
		{
			get
			{
				return this.Cast<bool>();
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x000118AB File Offset: 0x0000FAAB
		public string AsString
		{
			get
			{
				return this.Cast<string>();
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x000118B3 File Offset: 0x0000FAB3
		public Dictionary<string, fsData> AsDictionary
		{
			get
			{
				return this.Cast<Dictionary<string, fsData>>();
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x000118BB File Offset: 0x0000FABB
		public List<fsData> AsList
		{
			get
			{
				return this.Cast<List<fsData>>();
			}
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000118C4 File Offset: 0x0000FAC4
		private T Cast<T>()
		{
			if (this._value is T)
			{
				return (T)((object)this._value);
			}
			string[] array = new string[6];
			array[0] = "Unable to cast <";
			array[1] = ((this != null) ? this.ToString() : null);
			array[2] = "> (with type = ";
			int num = 3;
			Type type = this._value.GetType();
			array[num] = ((type != null) ? type.ToString() : null);
			array[4] = ") to type ";
			int num2 = 5;
			Type typeFromHandle = typeof(T);
			array[num2] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
			throw new InvalidCastException(string.Concat(array));
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00011955 File Offset: 0x0000FB55
		public override string ToString()
		{
			return fsJsonPrinter.CompressedJson(this);
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001195D File Offset: 0x0000FB5D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as fsData);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0001196C File Offset: 0x0000FB6C
		public bool Equals(fsData other)
		{
			if (other == null || this.Type != other.Type)
			{
				return false;
			}
			switch (this.Type)
			{
			case fsDataType.Array:
			{
				List<fsData> asList = this.AsList;
				List<fsData> asList2 = other.AsList;
				if (asList.Count != asList2.Count)
				{
					return false;
				}
				for (int i = 0; i < asList.Count; i++)
				{
					if (!asList[i].Equals(asList2[i]))
					{
						return false;
					}
				}
				return true;
			}
			case fsDataType.Object:
			{
				Dictionary<string, fsData> asDictionary = this.AsDictionary;
				Dictionary<string, fsData> asDictionary2 = other.AsDictionary;
				if (asDictionary.Count != asDictionary2.Count)
				{
					return false;
				}
				foreach (string text in asDictionary.Keys)
				{
					if (!asDictionary2.ContainsKey(text))
					{
						return false;
					}
					if (!asDictionary[text].Equals(asDictionary2[text]))
					{
						return false;
					}
				}
				return true;
			}
			case fsDataType.Double:
				return this.AsDouble == other.AsDouble || Math.Abs(this.AsDouble - other.AsDouble) < double.Epsilon;
			case fsDataType.Int64:
				return this.AsInt64 == other.AsInt64;
			case fsDataType.Boolean:
				return this.AsBool == other.AsBool;
			case fsDataType.String:
				return this.AsString == other.AsString;
			case fsDataType.Null:
				return true;
			default:
				throw new Exception("Unknown data type");
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00011B0C File Offset: 0x0000FD0C
		public static bool operator ==(fsData a, fsData b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.IsDouble && b.IsDouble)
			{
				return Math.Abs(a.AsDouble - b.AsDouble) < double.Epsilon;
			}
			return a.Equals(b);
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00011B5C File Offset: 0x0000FD5C
		public static bool operator !=(fsData a, fsData b)
		{
			return !(a == b);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00011B68 File Offset: 0x0000FD68
		public override int GetHashCode()
		{
			return this._value.GetHashCode();
		}

		// Token: 0x040001E7 RID: 487
		private object _value;

		// Token: 0x040001E8 RID: 488
		public static readonly fsData True = new fsData(true);

		// Token: 0x040001E9 RID: 489
		public static readonly fsData False = new fsData(false);

		// Token: 0x040001EA RID: 490
		public static readonly fsData Null = new fsData();
	}
}

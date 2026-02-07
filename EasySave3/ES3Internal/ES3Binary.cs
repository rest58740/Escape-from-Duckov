using System;
using System.Collections.Generic;

namespace ES3Internal
{
	// Token: 0x020000E6 RID: 230
	internal static class ES3Binary
	{
		// Token: 0x060004E7 RID: 1255 RVA: 0x0001E53C File Offset: 0x0001C73C
		internal static ES3SpecialByte TypeToByte(Type type)
		{
			ES3SpecialByte result;
			if (ES3Binary.TypeToId.TryGetValue(type, out result))
			{
				return result;
			}
			return ES3SpecialByte.Object;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001E55F File Offset: 0x0001C75F
		internal static Type ByteToType(ES3SpecialByte b)
		{
			return ES3Binary.ByteToType((byte)b);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001E568 File Offset: 0x0001C768
		internal static Type ByteToType(byte b)
		{
			Type result;
			if (ES3Binary.IdToType.TryGetValue((ES3SpecialByte)b, out result))
			{
				return result;
			}
			return typeof(object);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001E590 File Offset: 0x0001C790
		internal static bool IsPrimitive(ES3SpecialByte b)
		{
			return b - ES3SpecialByte.Bool <= 14;
		}

		// Token: 0x0400016F RID: 367
		internal const string ObjectTerminator = ".";

		// Token: 0x04000170 RID: 368
		internal static readonly Dictionary<ES3SpecialByte, Type> IdToType = new Dictionary<ES3SpecialByte, Type>
		{
			{
				ES3SpecialByte.Null,
				null
			},
			{
				ES3SpecialByte.Bool,
				typeof(bool)
			},
			{
				ES3SpecialByte.Byte,
				typeof(byte)
			},
			{
				ES3SpecialByte.Sbyte,
				typeof(sbyte)
			},
			{
				ES3SpecialByte.Char,
				typeof(char)
			},
			{
				ES3SpecialByte.Decimal,
				typeof(decimal)
			},
			{
				ES3SpecialByte.Double,
				typeof(double)
			},
			{
				ES3SpecialByte.Float,
				typeof(float)
			},
			{
				ES3SpecialByte.Int,
				typeof(int)
			},
			{
				ES3SpecialByte.Uint,
				typeof(uint)
			},
			{
				ES3SpecialByte.Long,
				typeof(long)
			},
			{
				ES3SpecialByte.Ulong,
				typeof(ulong)
			},
			{
				ES3SpecialByte.Short,
				typeof(short)
			},
			{
				ES3SpecialByte.Ushort,
				typeof(ushort)
			},
			{
				ES3SpecialByte.String,
				typeof(string)
			},
			{
				ES3SpecialByte.ByteArray,
				typeof(byte[])
			}
		};

		// Token: 0x04000171 RID: 369
		internal static readonly Dictionary<Type, ES3SpecialByte> TypeToId = new Dictionary<Type, ES3SpecialByte>
		{
			{
				typeof(bool),
				ES3SpecialByte.Bool
			},
			{
				typeof(byte),
				ES3SpecialByte.Byte
			},
			{
				typeof(sbyte),
				ES3SpecialByte.Sbyte
			},
			{
				typeof(char),
				ES3SpecialByte.Char
			},
			{
				typeof(decimal),
				ES3SpecialByte.Decimal
			},
			{
				typeof(double),
				ES3SpecialByte.Double
			},
			{
				typeof(float),
				ES3SpecialByte.Float
			},
			{
				typeof(int),
				ES3SpecialByte.Int
			},
			{
				typeof(uint),
				ES3SpecialByte.Uint
			},
			{
				typeof(long),
				ES3SpecialByte.Long
			},
			{
				typeof(ulong),
				ES3SpecialByte.Ulong
			},
			{
				typeof(short),
				ES3SpecialByte.Short
			},
			{
				typeof(ushort),
				ES3SpecialByte.Ushort
			},
			{
				typeof(string),
				ES3SpecialByte.String
			},
			{
				typeof(byte[]),
				ES3SpecialByte.ByteArray
			}
		};
	}
}

using System;
using UnityEngine.Scripting;

namespace ES3Types
{
	// Token: 0x0200003D RID: 61
	[Preserve]
	public class ES3Type_enum : ES3Type
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000995D File Offset: 0x00007B5D
		public ES3Type_enum(Type type) : base(type)
		{
			this.isPrimitive = true;
			this.isEnum = true;
			ES3Type_enum.Instance = this;
			this.underlyingType = Enum.GetUnderlyingType(type);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00009988 File Offset: 0x00007B88
		public override void Write(object obj, ES3Writer writer)
		{
			if (this.underlyingType == typeof(int))
			{
				writer.WritePrimitive((int)obj);
				return;
			}
			if (this.underlyingType == typeof(bool))
			{
				writer.WritePrimitive((bool)obj);
				return;
			}
			if (this.underlyingType == typeof(byte))
			{
				writer.WritePrimitive((byte)obj);
				return;
			}
			if (this.underlyingType == typeof(char))
			{
				writer.WritePrimitive((char)obj);
				return;
			}
			if (this.underlyingType == typeof(decimal))
			{
				writer.WritePrimitive((decimal)obj);
				return;
			}
			if (this.underlyingType == typeof(double))
			{
				writer.WritePrimitive((double)obj);
				return;
			}
			if (this.underlyingType == typeof(float))
			{
				writer.WritePrimitive((float)obj);
				return;
			}
			if (this.underlyingType == typeof(long))
			{
				writer.WritePrimitive((long)obj);
				return;
			}
			if (this.underlyingType == typeof(sbyte))
			{
				writer.WritePrimitive((sbyte)obj);
				return;
			}
			if (this.underlyingType == typeof(short))
			{
				writer.WritePrimitive((short)obj);
				return;
			}
			if (this.underlyingType == typeof(uint))
			{
				writer.WritePrimitive((uint)obj);
				return;
			}
			if (this.underlyingType == typeof(ulong))
			{
				writer.WritePrimitive((ulong)obj);
				return;
			}
			if (this.underlyingType == typeof(ushort))
			{
				writer.WritePrimitive((ushort)obj);
				return;
			}
			string[] array = new string[5];
			array[0] = "The underlying type ";
			int num = 1;
			Type type = this.underlyingType;
			array[num] = ((type != null) ? type.ToString() : null);
			array[2] = " of Enum ";
			int num2 = 3;
			Type type2 = this.type;
			array[num2] = ((type2 != null) ? type2.ToString() : null);
			array[4] = " is not supported";
			throw new InvalidCastException(string.Concat(array));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00009BBC File Offset: 0x00007DBC
		public override object Read<T>(ES3Reader reader)
		{
			if (this.underlyingType == typeof(int))
			{
				return Enum.ToObject(this.type, reader.Read_int());
			}
			if (this.underlyingType == typeof(bool))
			{
				return Enum.ToObject(this.type, reader.Read_bool());
			}
			if (this.underlyingType == typeof(byte))
			{
				return Enum.ToObject(this.type, reader.Read_byte());
			}
			if (this.underlyingType == typeof(char))
			{
				return Enum.ToObject(this.type, (ushort)reader.Read_char());
			}
			if (this.underlyingType == typeof(decimal))
			{
				return Enum.ToObject(this.type, reader.Read_decimal());
			}
			if (this.underlyingType == typeof(double))
			{
				return Enum.ToObject(this.type, reader.Read_double());
			}
			if (this.underlyingType == typeof(float))
			{
				return Enum.ToObject(this.type, reader.Read_float());
			}
			if (this.underlyingType == typeof(long))
			{
				return Enum.ToObject(this.type, reader.Read_long());
			}
			if (this.underlyingType == typeof(sbyte))
			{
				return Enum.ToObject(this.type, reader.Read_sbyte());
			}
			if (this.underlyingType == typeof(short))
			{
				return Enum.ToObject(this.type, reader.Read_short());
			}
			if (this.underlyingType == typeof(uint))
			{
				return Enum.ToObject(this.type, reader.Read_uint());
			}
			if (this.underlyingType == typeof(ulong))
			{
				return Enum.ToObject(this.type, reader.Read_ulong());
			}
			if (this.underlyingType == typeof(ushort))
			{
				return Enum.ToObject(this.type, reader.Read_ushort());
			}
			string[] array = new string[5];
			array[0] = "The underlying type ";
			int num = 1;
			Type type = this.underlyingType;
			array[num] = ((type != null) ? type.ToString() : null);
			array[2] = " of Enum ";
			int num2 = 3;
			Type type2 = this.type;
			array[num2] = ((type2 != null) ? type2.ToString() : null);
			array[4] = " is not supported";
			throw new InvalidCastException(string.Concat(array));
		}

		// Token: 0x04000081 RID: 129
		public static ES3Type Instance;

		// Token: 0x04000082 RID: 130
		private Type underlyingType;
	}
}

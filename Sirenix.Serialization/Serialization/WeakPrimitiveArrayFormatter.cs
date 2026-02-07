using System;
using System.Collections.Generic;
using Sirenix.Serialization.Utilities;

namespace Sirenix.Serialization
{
	// Token: 0x02000047 RID: 71
	public sealed class WeakPrimitiveArrayFormatter : WeakMinimalBaseFormatter
	{
		// Token: 0x060002DA RID: 730 RVA: 0x00014D1F File Offset: 0x00012F1F
		public WeakPrimitiveArrayFormatter(Type arrayType, Type elementType) : base(arrayType)
		{
			this.ElementType = elementType;
			if (!WeakPrimitiveArrayFormatter.PrimitiveTypes.TryGetValue(elementType, ref this.PrimitiveType))
			{
				throw new SerializationAbortException("The type '" + elementType.GetNiceFullName() + "' is not a type that can be written as a primitive array, yet the primitive array formatter is being used for it.");
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000EE6B File Offset: 0x0000D06B
		protected override object GetUninitializedObject()
		{
			return null;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00014D60 File Offset: 0x00012F60
		protected override void Read(ref object value, IDataReader reader)
		{
			string text;
			if (reader.PeekEntry(out text) == EntryType.PrimitiveArray)
			{
				switch (this.PrimitiveType)
				{
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_char:
				{
					char[] array;
					reader.ReadPrimitiveArray<char>(out array);
					value = array;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_sbyte:
				{
					sbyte[] array2;
					reader.ReadPrimitiveArray<sbyte>(out array2);
					value = array2;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_short:
				{
					short[] array3;
					reader.ReadPrimitiveArray<short>(out array3);
					value = array3;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_int:
				{
					int[] array4;
					reader.ReadPrimitiveArray<int>(out array4);
					value = array4;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_long:
				{
					long[] array5;
					reader.ReadPrimitiveArray<long>(out array5);
					value = array5;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_byte:
				{
					byte[] array6;
					reader.ReadPrimitiveArray<byte>(out array6);
					value = array6;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ushort:
				{
					ushort[] array7;
					reader.ReadPrimitiveArray<ushort>(out array7);
					value = array7;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_uint:
				{
					uint[] array8;
					reader.ReadPrimitiveArray<uint>(out array8);
					value = array8;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ulong:
				{
					ulong[] array9;
					reader.ReadPrimitiveArray<ulong>(out array9);
					value = array9;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_decimal:
				{
					decimal[] array10;
					reader.ReadPrimitiveArray<decimal>(out array10);
					value = array10;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_bool:
				{
					bool[] array11;
					reader.ReadPrimitiveArray<bool>(out array11);
					value = array11;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_float:
				{
					float[] array12;
					reader.ReadPrimitiveArray<float>(out array12);
					value = array12;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_double:
				{
					double[] array13;
					reader.ReadPrimitiveArray<double>(out array13);
					value = array13;
					break;
				}
				case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_Guid:
				{
					Guid[] array14;
					reader.ReadPrimitiveArray<Guid>(out array14);
					value = array14;
					break;
				}
				default:
					throw new NotImplementedException();
				}
				base.RegisterReferenceID(value, reader);
				return;
			}
			reader.SkipEntry();
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00014EBC File Offset: 0x000130BC
		protected override void Write(ref object value, IDataWriter writer)
		{
			switch (this.PrimitiveType)
			{
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_char:
				writer.WritePrimitiveArray<char>((char[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_sbyte:
				writer.WritePrimitiveArray<sbyte>((sbyte[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_short:
				writer.WritePrimitiveArray<short>((short[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_int:
				writer.WritePrimitiveArray<int>((int[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_long:
				writer.WritePrimitiveArray<long>((long[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_byte:
				writer.WritePrimitiveArray<byte>((byte[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ushort:
				writer.WritePrimitiveArray<ushort>((ushort[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_uint:
				writer.WritePrimitiveArray<uint>((uint[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ulong:
				writer.WritePrimitiveArray<ulong>((ulong[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_decimal:
				writer.WritePrimitiveArray<decimal>((decimal[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_bool:
				writer.WritePrimitiveArray<bool>((bool[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_float:
				writer.WritePrimitiveArray<float>((float[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_double:
				writer.WritePrimitiveArray<double>((double[])value);
				return;
			case WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_Guid:
				writer.WritePrimitiveArray<Guid>((Guid[])value);
				return;
			default:
				return;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00014FD4 File Offset: 0x000131D4
		// Note: this type is marked as 'beforefieldinit'.
		static WeakPrimitiveArrayFormatter()
		{
			Dictionary<Type, WeakPrimitiveArrayFormatter.PrimitiveArrayType> dictionary = new Dictionary<Type, WeakPrimitiveArrayFormatter.PrimitiveArrayType>(FastTypeComparer.Instance);
			dictionary.Add(typeof(char), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_char);
			dictionary.Add(typeof(sbyte), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_sbyte);
			dictionary.Add(typeof(short), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_short);
			dictionary.Add(typeof(int), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_int);
			dictionary.Add(typeof(long), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_long);
			dictionary.Add(typeof(byte), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_byte);
			dictionary.Add(typeof(ushort), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ushort);
			dictionary.Add(typeof(uint), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_uint);
			dictionary.Add(typeof(ulong), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_ulong);
			dictionary.Add(typeof(decimal), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_decimal);
			dictionary.Add(typeof(bool), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_bool);
			dictionary.Add(typeof(float), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_float);
			dictionary.Add(typeof(double), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_double);
			dictionary.Add(typeof(Guid), WeakPrimitiveArrayFormatter.PrimitiveArrayType.PrimitiveArray_Guid);
			WeakPrimitiveArrayFormatter.PrimitiveTypes = dictionary;
		}

		// Token: 0x040000E6 RID: 230
		private static readonly Dictionary<Type, WeakPrimitiveArrayFormatter.PrimitiveArrayType> PrimitiveTypes;

		// Token: 0x040000E7 RID: 231
		private readonly Type ElementType;

		// Token: 0x040000E8 RID: 232
		private readonly WeakPrimitiveArrayFormatter.PrimitiveArrayType PrimitiveType;

		// Token: 0x020000FB RID: 251
		public enum PrimitiveArrayType
		{
			// Token: 0x04000287 RID: 647
			PrimitiveArray_char,
			// Token: 0x04000288 RID: 648
			PrimitiveArray_sbyte,
			// Token: 0x04000289 RID: 649
			PrimitiveArray_short,
			// Token: 0x0400028A RID: 650
			PrimitiveArray_int,
			// Token: 0x0400028B RID: 651
			PrimitiveArray_long,
			// Token: 0x0400028C RID: 652
			PrimitiveArray_byte,
			// Token: 0x0400028D RID: 653
			PrimitiveArray_ushort,
			// Token: 0x0400028E RID: 654
			PrimitiveArray_uint,
			// Token: 0x0400028F RID: 655
			PrimitiveArray_ulong,
			// Token: 0x04000290 RID: 656
			PrimitiveArray_decimal,
			// Token: 0x04000291 RID: 657
			PrimitiveArray_bool,
			// Token: 0x04000292 RID: 658
			PrimitiveArray_float,
			// Token: 0x04000293 RID: 659
			PrimitiveArray_double,
			// Token: 0x04000294 RID: 660
			PrimitiveArray_Guid
		}
	}
}

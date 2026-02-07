using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ConvertUtils
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x00011458 File Offset: 0x0000F658
		public static PrimitiveTypeCode GetTypeCode(Type t)
		{
			bool flag;
			return ConvertUtils.GetTypeCode(t, out flag);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00011470 File Offset: 0x0000F670
		public static PrimitiveTypeCode GetTypeCode(Type t, out bool isEnum)
		{
			PrimitiveTypeCode result;
			if (ConvertUtils.TypeCodeMap.TryGetValue(t, ref result))
			{
				isEnum = false;
				return result;
			}
			if (t.IsEnum())
			{
				isEnum = true;
				return ConvertUtils.GetTypeCode(Enum.GetUnderlyingType(t));
			}
			if (ReflectionUtils.IsNullableType(t))
			{
				Type underlyingType = Nullable.GetUnderlyingType(t);
				if (underlyingType.IsEnum())
				{
					Type t2 = typeof(Nullable).MakeGenericType(new Type[]
					{
						Enum.GetUnderlyingType(underlyingType)
					});
					isEnum = true;
					return ConvertUtils.GetTypeCode(t2);
				}
			}
			isEnum = false;
			return PrimitiveTypeCode.Object;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x000114EA File Offset: 0x0000F6EA
		public static TypeInformation GetTypeInformation(IConvertible convertable)
		{
			return ConvertUtils.PrimitiveTypeCodes[convertable.GetTypeCode()];
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000114F8 File Offset: 0x0000F6F8
		public static bool IsConvertible(Type t)
		{
			return typeof(IConvertible).IsAssignableFrom(t);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001150A File Offset: 0x0000F70A
		public static TimeSpan ParseTimeSpan(string input)
		{
			return TimeSpan.Parse(input, CultureInfo.InvariantCulture);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00011518 File Offset: 0x0000F718
		[NullableContext(2)]
		private static Func<object, object> CreateCastConverter([Nullable(new byte[]
		{
			0,
			1,
			1
		})] StructMultiKey<Type, Type> t)
		{
			Type value = t.Value1;
			Type value2 = t.Value2;
			MethodInfo method;
			if ((method = value2.GetMethod("op_Implicit", new Type[]
			{
				value
			})) == null)
			{
				method = value2.GetMethod("op_Explicit", new Type[]
				{
					value
				});
			}
			MethodInfo methodInfo = method;
			if (methodInfo == null)
			{
				return null;
			}
			MethodCall<object, object> call = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(methodInfo);
			return (object o) => call(null, new object[]
			{
				o
			});
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00011590 File Offset: 0x0000F790
		internal static BigInteger ToBigInteger(object value)
		{
			if (value is BigInteger)
			{
				return (BigInteger)value;
			}
			string text = value as string;
			if (text != null)
			{
				return BigInteger.Parse(text, CultureInfo.InvariantCulture);
			}
			if (value is float)
			{
				float num = (float)value;
				return new BigInteger(num);
			}
			if (value is double)
			{
				double num2 = (double)value;
				return new BigInteger(num2);
			}
			if (value is decimal)
			{
				decimal num3 = (decimal)value;
				return new BigInteger(num3);
			}
			if (value is int)
			{
				int num4 = (int)value;
				return new BigInteger(num4);
			}
			if (value is long)
			{
				long num5 = (long)value;
				return new BigInteger(num5);
			}
			if (value is uint)
			{
				uint num6 = (uint)value;
				return new BigInteger(num6);
			}
			if (value is ulong)
			{
				ulong num7 = (ulong)value;
				return new BigInteger(num7);
			}
			byte[] array = value as byte[];
			if (array != null)
			{
				return new BigInteger(array);
			}
			throw new InvalidCastException("Cannot convert {0} to BigInteger.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00011698 File Offset: 0x0000F898
		public static object FromBigInteger(BigInteger i, Type targetType)
		{
			if (targetType == typeof(decimal))
			{
				return (decimal)i;
			}
			if (targetType == typeof(double))
			{
				return (double)i;
			}
			if (targetType == typeof(float))
			{
				return (float)i;
			}
			if (targetType == typeof(ulong))
			{
				return (ulong)i;
			}
			if (targetType == typeof(bool))
			{
				return i != 0L;
			}
			object result;
			try
			{
				result = System.Convert.ChangeType((long)i, targetType, CultureInfo.InvariantCulture);
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Can not convert from BigInteger to {0}.".FormatWith(CultureInfo.InvariantCulture, targetType), ex);
			}
			return result;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00011784 File Offset: 0x0000F984
		public static object Convert(object initialValue, CultureInfo culture, Type targetType)
		{
			object result;
			switch (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out result))
			{
			case ConvertUtils.ConvertResult.Success:
				return result;
			case ConvertUtils.ConvertResult.CannotConvertNull:
				throw new Exception("Can not convert null {0} into non-nullable {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
			case ConvertUtils.ConvertResult.NotInstantiableType:
				throw new ArgumentException("Target type {0} is not a value type or a non-abstract class.".FormatWith(CultureInfo.InvariantCulture, targetType), "targetType");
			case ConvertUtils.ConvertResult.NoValidConversion:
				throw new InvalidOperationException("Can not convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, initialValue.GetType(), targetType));
			default:
				throw new InvalidOperationException("Unexpected conversion result.");
			}
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00011814 File Offset: 0x0000FA14
		private static bool TryConvert([Nullable(2)] object initialValue, CultureInfo culture, Type targetType, [Nullable(2)] out object value)
		{
			bool result;
			try
			{
				if (ConvertUtils.TryConvertInternal(initialValue, culture, targetType, out value) == ConvertUtils.ConvertResult.Success)
				{
					result = true;
				}
				else
				{
					value = null;
					result = false;
				}
			}
			catch
			{
				value = null;
				result = false;
			}
			return result;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00011850 File Offset: 0x0000FA50
		private static ConvertUtils.ConvertResult TryConvertInternal([Nullable(2)] object initialValue, CultureInfo culture, Type targetType, [Nullable(2)] out object value)
		{
			if (initialValue == null)
			{
				throw new ArgumentNullException("initialValue");
			}
			if (ReflectionUtils.IsNullableType(targetType))
			{
				targetType = Nullable.GetUnderlyingType(targetType);
			}
			Type type = initialValue.GetType();
			if (targetType == type)
			{
				value = initialValue;
				return ConvertUtils.ConvertResult.Success;
			}
			if (ConvertUtils.IsConvertible(initialValue.GetType()) && ConvertUtils.IsConvertible(targetType))
			{
				if (targetType.IsEnum())
				{
					if (initialValue is string)
					{
						value = Enum.Parse(targetType, initialValue.ToString(), true);
						return ConvertUtils.ConvertResult.Success;
					}
					if (ConvertUtils.IsInteger(initialValue))
					{
						value = Enum.ToObject(targetType, initialValue);
						return ConvertUtils.ConvertResult.Success;
					}
				}
				value = System.Convert.ChangeType(initialValue, targetType, culture);
				return ConvertUtils.ConvertResult.Success;
			}
			if (initialValue is DateTime)
			{
				DateTime dateTime = (DateTime)initialValue;
				if (targetType == typeof(DateTimeOffset))
				{
					value = new DateTimeOffset(dateTime);
					return ConvertUtils.ConvertResult.Success;
				}
			}
			byte[] array = initialValue as byte[];
			if (array != null && targetType == typeof(Guid))
			{
				value = new Guid(array);
				return ConvertUtils.ConvertResult.Success;
			}
			if (initialValue is Guid)
			{
				Guid guid = (Guid)initialValue;
				if (targetType == typeof(byte[]))
				{
					value = guid.ToByteArray();
					return ConvertUtils.ConvertResult.Success;
				}
			}
			string text = initialValue as string;
			if (text != null)
			{
				if (targetType == typeof(Guid))
				{
					value = new Guid(text);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(Uri))
				{
					value = new Uri(text, 0);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(TimeSpan))
				{
					value = ConvertUtils.ParseTimeSpan(text);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(byte[]))
				{
					value = System.Convert.FromBase64String(text);
					return ConvertUtils.ConvertResult.Success;
				}
				if (targetType == typeof(Version))
				{
					Version version;
					if (ConvertUtils.VersionTryParse(text, out version))
					{
						value = version;
						return ConvertUtils.ConvertResult.Success;
					}
					value = null;
					return ConvertUtils.ConvertResult.NoValidConversion;
				}
				else if (typeof(Type).IsAssignableFrom(targetType))
				{
					value = Type.GetType(text, true);
					return ConvertUtils.ConvertResult.Success;
				}
			}
			if (targetType == typeof(BigInteger))
			{
				value = ConvertUtils.ToBigInteger(initialValue);
				return ConvertUtils.ConvertResult.Success;
			}
			if (initialValue is BigInteger)
			{
				BigInteger i = (BigInteger)initialValue;
				value = ConvertUtils.FromBigInteger(i, targetType);
				return ConvertUtils.ConvertResult.Success;
			}
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			if (converter != null && converter.CanConvertTo(targetType))
			{
				value = converter.ConvertTo(null, culture, initialValue, targetType);
				return ConvertUtils.ConvertResult.Success;
			}
			TypeConverter converter2 = TypeDescriptor.GetConverter(targetType);
			if (converter2 != null && converter2.CanConvertFrom(type))
			{
				value = converter2.ConvertFrom(null, culture, initialValue);
				return ConvertUtils.ConvertResult.Success;
			}
			if (initialValue == DBNull.Value)
			{
				if (ReflectionUtils.IsNullable(targetType))
				{
					value = ConvertUtils.EnsureTypeAssignable(null, type, targetType);
					return ConvertUtils.ConvertResult.Success;
				}
				value = null;
				return ConvertUtils.ConvertResult.CannotConvertNull;
			}
			else
			{
				if (targetType.IsInterface() || targetType.IsGenericTypeDefinition() || targetType.IsAbstract())
				{
					value = null;
					return ConvertUtils.ConvertResult.NotInstantiableType;
				}
				value = null;
				return ConvertUtils.ConvertResult.NoValidConversion;
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00011B08 File Offset: 0x0000FD08
		[return: Nullable(2)]
		public static object ConvertOrCast([Nullable(2)] object initialValue, CultureInfo culture, Type targetType)
		{
			if (targetType == typeof(object))
			{
				return initialValue;
			}
			if (initialValue == null && ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			object result;
			if (ConvertUtils.TryConvert(initialValue, culture, targetType, out result))
			{
				return result;
			}
			return ConvertUtils.EnsureTypeAssignable(initialValue, ReflectionUtils.GetObjectType(initialValue), targetType);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00011B54 File Offset: 0x0000FD54
		[return: Nullable(2)]
		private static object EnsureTypeAssignable([Nullable(2)] object value, Type initialType, Type targetType)
		{
			if (value != null)
			{
				Type type = value.GetType();
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				Func<object, object> func = ConvertUtils.CastConverters.Get(new StructMultiKey<Type, Type>(type, targetType));
				if (func != null)
				{
					return func.Invoke(value);
				}
			}
			else if (ReflectionUtils.IsNullable(targetType))
			{
				return null;
			}
			throw new ArgumentException("Could not cast or convert from {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, ((initialType != null) ? initialType.ToString() : null) ?? "{null}", targetType));
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00011BC7 File Offset: 0x0000FDC7
		public static bool VersionTryParse(string input, [Nullable(2)] [NotNullWhen(true)] out Version result)
		{
			return Version.TryParse(input, ref result);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		public static bool IsInteger(object value)
		{
			switch (ConvertUtils.GetTypeCode(value.GetType()))
			{
			case PrimitiveTypeCode.SByte:
			case PrimitiveTypeCode.Int16:
			case PrimitiveTypeCode.UInt16:
			case PrimitiveTypeCode.Int32:
			case PrimitiveTypeCode.Byte:
			case PrimitiveTypeCode.UInt32:
			case PrimitiveTypeCode.Int64:
			case PrimitiveTypeCode.UInt64:
				return true;
			}
			return false;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00011C34 File Offset: 0x0000FE34
		public static ParseResult Int32TryParse(char[] chars, int start, int length, out int value)
		{
			value = 0;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			if (length > 10 || (length == 10 && chars[start] - '0' > '\u0002'))
			{
				for (int i = start; i < num; i++)
				{
					int num2 = (int)(chars[i] - '0');
					if (num2 < 0 || num2 > 9)
					{
						return ParseResult.Invalid;
					}
				}
				return ParseResult.Overflow;
			}
			for (int j = start; j < num; j++)
			{
				int num3 = (int)(chars[j] - '0');
				if (num3 < 0 || num3 > 9)
				{
					return ParseResult.Invalid;
				}
				int num4 = 10 * value - num3;
				if (num4 > value)
				{
					for (j++; j < num; j++)
					{
						num3 = (int)(chars[j] - '0');
						if (num3 < 0 || num3 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num4;
			}
			if (!flag)
			{
				if (value == -2147483648)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00011D14 File Offset: 0x0000FF14
		public static ParseResult Int64TryParse(char[] chars, int start, int length, out long value)
		{
			value = 0L;
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int num = start + length;
			if (length > 19)
			{
				for (int i = start; i < num; i++)
				{
					int num2 = (int)(chars[i] - '0');
					if (num2 < 0 || num2 > 9)
					{
						return ParseResult.Invalid;
					}
				}
				return ParseResult.Overflow;
			}
			for (int j = start; j < num; j++)
			{
				int num3 = (int)(chars[j] - '0');
				if (num3 < 0 || num3 > 9)
				{
					return ParseResult.Invalid;
				}
				long num4 = 10L * value - (long)num3;
				if (num4 > value)
				{
					for (j++; j < num; j++)
					{
						num3 = (int)(chars[j] - '0');
						if (num3 < 0 || num3 > 9)
						{
							return ParseResult.Invalid;
						}
					}
					return ParseResult.Overflow;
				}
				value = num4;
			}
			if (!flag)
			{
				if (value == -9223372036854775808L)
				{
					return ParseResult.Overflow;
				}
				value = -value;
			}
			return ParseResult.Success;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00011DF0 File Offset: 0x0000FFF0
		public static ParseResult DecimalTryParse(char[] chars, int start, int length, out decimal value)
		{
			value = default(decimal);
			if (length == 0)
			{
				return ParseResult.Invalid;
			}
			bool flag = chars[start] == '-';
			if (flag)
			{
				if (length == 1)
				{
					return ParseResult.Invalid;
				}
				start++;
				length--;
			}
			int i = start;
			int num = start + length;
			int num2 = num;
			int num3 = num;
			int num4 = 0;
			ulong num5 = 0UL;
			ulong num6 = 0UL;
			int num7 = 0;
			int num8 = 0;
			char? c = default(char?);
			bool? flag2 = default(bool?);
			while (i < num)
			{
				char c2 = chars[i];
				if (c2 == '.')
				{
					goto IL_74;
				}
				if (c2 != 'E' && c2 != 'e')
				{
					if (c2 < '0' || c2 > '9')
					{
						return ParseResult.Invalid;
					}
					if (i == start && c2 == '0')
					{
						i++;
						if (i != num)
						{
							c2 = chars[i];
							if (c2 == '.')
							{
								goto IL_74;
							}
							if (c2 != 'e' && c2 != 'E')
							{
								return ParseResult.Invalid;
							}
							goto IL_91;
						}
					}
					if (num7 < 29)
					{
						if (num7 == 28)
						{
							bool? flag3 = flag2;
							bool valueOrDefault;
							if (flag3 == null)
							{
								flag2 = new bool?(num5 > 7922816251426433759UL || (num5 == 7922816251426433759UL && (num6 > 354395033UL || (num6 == 354395033UL && c2 > '5'))));
								bool? flag4 = flag2;
								valueOrDefault = flag4.GetValueOrDefault();
							}
							else
							{
								valueOrDefault = flag3.GetValueOrDefault();
							}
							if (valueOrDefault)
							{
								goto IL_1FF;
							}
						}
						if (num7 < 19)
						{
							num5 = num5 * 10UL + (ulong)((long)(c2 - '0'));
						}
						else
						{
							num6 = num6 * 10UL + (ulong)((long)(c2 - '0'));
						}
						num7++;
						goto IL_217;
					}
					IL_1FF:
					if (c == null)
					{
						c = new char?(c2);
					}
					num8++;
					goto IL_217;
				}
				IL_91:
				if (i == start)
				{
					return ParseResult.Invalid;
				}
				if (i == num2)
				{
					return ParseResult.Invalid;
				}
				i++;
				if (i == num)
				{
					return ParseResult.Invalid;
				}
				if (num2 < num)
				{
					num3 = i - 1;
				}
				c2 = chars[i];
				bool flag5 = false;
				if (c2 != '+')
				{
					if (c2 == '-')
					{
						flag5 = true;
						i++;
					}
				}
				else
				{
					i++;
				}
				while (i < num)
				{
					c2 = chars[i];
					if (c2 < '0' || c2 > '9')
					{
						return ParseResult.Invalid;
					}
					int num9 = 10 * num4 + (int)(c2 - '0');
					if (num4 < num9)
					{
						num4 = num9;
					}
					i++;
				}
				if (flag5)
				{
					num4 = -num4;
				}
				IL_217:
				i++;
				continue;
				IL_74:
				if (i == start)
				{
					return ParseResult.Invalid;
				}
				if (i + 1 == num)
				{
					return ParseResult.Invalid;
				}
				if (num2 != num)
				{
					return ParseResult.Invalid;
				}
				num2 = i + 1;
				goto IL_217;
			}
			num4 += num8;
			num4 -= num3 - num2;
			if (num7 <= 19)
			{
				value = num5;
			}
			else
			{
				value = num5 / new decimal(1, 0, 0, false, (byte)(num7 - 19)) + num6;
			}
			if (num4 > 0)
			{
				num7 += num4;
				if (num7 > 29)
				{
					return ParseResult.Overflow;
				}
				if (num7 == 29)
				{
					if (num4 > 1)
					{
						value /= new decimal(1, 0, 0, false, (byte)(num4 - 1));
						if (value > 7922816251426433759354395033m)
						{
							return ParseResult.Overflow;
						}
					}
					else if (value == 7922816251426433759354395033m)
					{
						char? c3 = c;
						int? num10 = (c3 != null) ? new int?((int)c3.GetValueOrDefault()) : default(int?);
						int num11 = 53;
						if (num10.GetValueOrDefault() > num11 & num10 != null)
						{
							return ParseResult.Overflow;
						}
					}
					value *= 10m;
				}
				else
				{
					value /= new decimal(1, 0, 0, false, (byte)num4);
				}
			}
			else
			{
				char? c3 = c;
				int? num10 = (c3 != null) ? new int?((int)c3.GetValueOrDefault()) : default(int?);
				int num11 = 53;
				if ((num10.GetValueOrDefault() >= num11 & num10 != null) && num4 >= -28)
				{
					value = ++value;
				}
				if (num4 < 0)
				{
					if (num7 + num4 + 28 <= 0)
					{
						value = (flag ? 0m : 0m);
						return ParseResult.Success;
					}
					if (num4 >= -28)
					{
						value *= new decimal(1, 0, 0, false, (byte)(-(byte)num4));
					}
					else
					{
						value /= 10000000000000000000000000000m;
						value *= new decimal(1, 0, 0, false, (byte)(-num4 - 28));
					}
				}
			}
			if (flag)
			{
				value = -value;
			}
			return ParseResult.Success;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00012285 File Offset: 0x00010485
		public static bool TryConvertGuid(string s, out Guid g)
		{
			return Guid.TryParseExact(s, "D", ref g);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00012294 File Offset: 0x00010494
		public static bool TryHexTextToInt(char[] text, int start, int end, out int value)
		{
			value = 0;
			for (int i = start; i < end; i++)
			{
				char c = text[i];
				int num;
				if (c <= '9' && c >= '0')
				{
					num = (int)(c - '0');
				}
				else if (c <= 'F' && c >= 'A')
				{
					num = (int)(c - '7');
				}
				else
				{
					if (c > 'f' || c < 'a')
					{
						value = 0;
						return false;
					}
					num = (int)(c - 'W');
				}
				value += num << (end - 1 - i) * 4;
			}
			return true;
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00012300 File Offset: 0x00010500
		// Note: this type is marked as 'beforefieldinit'.
		static ConvertUtils()
		{
			Dictionary<Type, PrimitiveTypeCode> dictionary = new Dictionary<Type, PrimitiveTypeCode>();
			dictionary.Add(typeof(char), PrimitiveTypeCode.Char);
			dictionary.Add(typeof(char?), PrimitiveTypeCode.CharNullable);
			dictionary.Add(typeof(bool), PrimitiveTypeCode.Boolean);
			dictionary.Add(typeof(bool?), PrimitiveTypeCode.BooleanNullable);
			dictionary.Add(typeof(sbyte), PrimitiveTypeCode.SByte);
			dictionary.Add(typeof(sbyte?), PrimitiveTypeCode.SByteNullable);
			dictionary.Add(typeof(short), PrimitiveTypeCode.Int16);
			dictionary.Add(typeof(short?), PrimitiveTypeCode.Int16Nullable);
			dictionary.Add(typeof(ushort), PrimitiveTypeCode.UInt16);
			dictionary.Add(typeof(ushort?), PrimitiveTypeCode.UInt16Nullable);
			dictionary.Add(typeof(int), PrimitiveTypeCode.Int32);
			dictionary.Add(typeof(int?), PrimitiveTypeCode.Int32Nullable);
			dictionary.Add(typeof(byte), PrimitiveTypeCode.Byte);
			dictionary.Add(typeof(byte?), PrimitiveTypeCode.ByteNullable);
			dictionary.Add(typeof(uint), PrimitiveTypeCode.UInt32);
			dictionary.Add(typeof(uint?), PrimitiveTypeCode.UInt32Nullable);
			dictionary.Add(typeof(long), PrimitiveTypeCode.Int64);
			dictionary.Add(typeof(long?), PrimitiveTypeCode.Int64Nullable);
			dictionary.Add(typeof(ulong), PrimitiveTypeCode.UInt64);
			dictionary.Add(typeof(ulong?), PrimitiveTypeCode.UInt64Nullable);
			dictionary.Add(typeof(float), PrimitiveTypeCode.Single);
			dictionary.Add(typeof(float?), PrimitiveTypeCode.SingleNullable);
			dictionary.Add(typeof(double), PrimitiveTypeCode.Double);
			dictionary.Add(typeof(double?), PrimitiveTypeCode.DoubleNullable);
			dictionary.Add(typeof(DateTime), PrimitiveTypeCode.DateTime);
			dictionary.Add(typeof(DateTime?), PrimitiveTypeCode.DateTimeNullable);
			dictionary.Add(typeof(DateTimeOffset), PrimitiveTypeCode.DateTimeOffset);
			dictionary.Add(typeof(DateTimeOffset?), PrimitiveTypeCode.DateTimeOffsetNullable);
			dictionary.Add(typeof(decimal), PrimitiveTypeCode.Decimal);
			dictionary.Add(typeof(decimal?), PrimitiveTypeCode.DecimalNullable);
			dictionary.Add(typeof(Guid), PrimitiveTypeCode.Guid);
			dictionary.Add(typeof(Guid?), PrimitiveTypeCode.GuidNullable);
			dictionary.Add(typeof(TimeSpan), PrimitiveTypeCode.TimeSpan);
			dictionary.Add(typeof(TimeSpan?), PrimitiveTypeCode.TimeSpanNullable);
			dictionary.Add(typeof(BigInteger), PrimitiveTypeCode.BigInteger);
			dictionary.Add(typeof(BigInteger?), PrimitiveTypeCode.BigIntegerNullable);
			dictionary.Add(typeof(Uri), PrimitiveTypeCode.Uri);
			dictionary.Add(typeof(string), PrimitiveTypeCode.String);
			dictionary.Add(typeof(byte[]), PrimitiveTypeCode.Bytes);
			dictionary.Add(typeof(DBNull), PrimitiveTypeCode.DBNull);
			ConvertUtils.TypeCodeMap = dictionary;
			ConvertUtils.PrimitiveTypeCodes = new TypeInformation[]
			{
				new TypeInformation(typeof(object), PrimitiveTypeCode.Empty),
				new TypeInformation(typeof(object), PrimitiveTypeCode.Object),
				new TypeInformation(typeof(object), PrimitiveTypeCode.DBNull),
				new TypeInformation(typeof(bool), PrimitiveTypeCode.Boolean),
				new TypeInformation(typeof(char), PrimitiveTypeCode.Char),
				new TypeInformation(typeof(sbyte), PrimitiveTypeCode.SByte),
				new TypeInformation(typeof(byte), PrimitiveTypeCode.Byte),
				new TypeInformation(typeof(short), PrimitiveTypeCode.Int16),
				new TypeInformation(typeof(ushort), PrimitiveTypeCode.UInt16),
				new TypeInformation(typeof(int), PrimitiveTypeCode.Int32),
				new TypeInformation(typeof(uint), PrimitiveTypeCode.UInt32),
				new TypeInformation(typeof(long), PrimitiveTypeCode.Int64),
				new TypeInformation(typeof(ulong), PrimitiveTypeCode.UInt64),
				new TypeInformation(typeof(float), PrimitiveTypeCode.Single),
				new TypeInformation(typeof(double), PrimitiveTypeCode.Double),
				new TypeInformation(typeof(decimal), PrimitiveTypeCode.Decimal),
				new TypeInformation(typeof(DateTime), PrimitiveTypeCode.DateTime),
				new TypeInformation(typeof(object), PrimitiveTypeCode.Empty),
				new TypeInformation(typeof(string), PrimitiveTypeCode.String)
			};
			ConvertUtils.CastConverters = new ThreadSafeStore<StructMultiKey<Type, Type>, Func<object, object>>(new Func<StructMultiKey<Type, Type>, Func<object, object>>(ConvertUtils.CreateCastConverter));
		}

		// Token: 0x040001A9 RID: 425
		private static readonly Dictionary<Type, PrimitiveTypeCode> TypeCodeMap;

		// Token: 0x040001AA RID: 426
		private static readonly TypeInformation[] PrimitiveTypeCodes;

		// Token: 0x040001AB RID: 427
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1,
			2,
			2,
			2
		})]
		private static readonly ThreadSafeStore<StructMultiKey<Type, Type>, Func<object, object>> CastConverters;

		// Token: 0x02000167 RID: 359
		[NullableContext(0)]
		internal enum ConvertResult
		{
			// Token: 0x04000699 RID: 1689
			Success,
			// Token: 0x0400069A RID: 1690
			CannotConvertNull,
			// Token: 0x0400069B RID: 1691
			NotInstantiableType,
			// Token: 0x0400069C RID: 1692
			NoValidConversion
		}
	}
}

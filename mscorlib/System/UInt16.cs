using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x020001A6 RID: 422
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct UInt16 : IComparable, IConvertible, IFormattable, IComparable<ushort>, IEquatable<ushort>, ISpanFormattable
	{
		// Token: 0x060011F4 RID: 4596 RVA: 0x00047D78 File Offset: 0x00045F78
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is ushort)
			{
				return (int)(this - (ushort)value);
			}
			throw new ArgumentException("Object must be of type UInt16.");
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x000225C2 File Offset: 0x000207C2
		public int CompareTo(ushort value)
		{
			return (int)(this - value);
		}

		// Token: 0x060011F6 RID: 4598 RVA: 0x00047D9B File Offset: 0x00045F9B
		public override bool Equals(object obj)
		{
			return obj is ushort && this == (ushort)obj;
		}

		// Token: 0x060011F7 RID: 4599 RVA: 0x00022598 File Offset: 0x00020798
		[NonVersionable]
		public bool Equals(ushort obj)
		{
			return this == obj;
		}

		// Token: 0x060011F8 RID: 4600 RVA: 0x00022829 File Offset: 0x00020A29
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x060011F9 RID: 4601 RVA: 0x00047DB1 File Offset: 0x00045FB1
		public override string ToString()
		{
			return Number.FormatUInt32((uint)this, null, null);
		}

		// Token: 0x060011FA RID: 4602 RVA: 0x00047DC1 File Offset: 0x00045FC1
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, null, provider);
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00047DD1 File Offset: 0x00045FD1
		public string ToString(string format)
		{
			return Number.FormatUInt32((uint)this, format, null);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00047DE1 File Offset: 0x00045FE1
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32((uint)this, format, provider);
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00047DF1 File Offset: 0x00045FF1
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatUInt32((uint)this, format, provider, destination, out charsWritten);
		}

		// Token: 0x060011FE RID: 4606 RVA: 0x00047DFF File Offset: 0x00045FFF
		[CLSCompliant(false)]
		public static ushort Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x060011FF RID: 4607 RVA: 0x00047E1C File Offset: 0x0004601C
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return ushort.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x00047E3F File Offset: 0x0004603F
		[CLSCompliant(false)]
		public static ushort Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return ushort.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x00047E5D File Offset: 0x0004605D
		[CLSCompliant(false)]
		public static ushort Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return ushort.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00047E81 File Offset: 0x00046081
		[CLSCompliant(false)]
		public static ushort Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00047E98 File Offset: 0x00046098
		private static ushort Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
		{
			uint num = 0U;
			try
			{
				num = Number.ParseUInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException("Value was either too large or too small for a UInt16.", innerException);
			}
			if (num > 65535U)
			{
				throw new OverflowException("Value was either too large or too small for a UInt16.");
			}
			return (ushort)num;
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00047EE4 File Offset: 0x000460E4
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ushort result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00047F00 File Offset: 0x00046100
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, out ushort result)
		{
			return ushort.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00047F0F File Offset: 0x0004610F
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ushort result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return ushort.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001207 RID: 4615 RVA: 0x00047F32 File Offset: 0x00046132
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out ushort result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return ushort.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00047F48 File Offset: 0x00046148
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out ushort result)
		{
			result = 0;
			uint num;
			if (!Number.TryParseUInt32(s, style, info, out num))
			{
				return false;
			}
			if (num > 65535U)
			{
				return false;
			}
			result = (ushort)num;
			return true;
		}

		// Token: 0x06001209 RID: 4617 RVA: 0x00047F75 File Offset: 0x00046175
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt16;
		}

		// Token: 0x0600120A RID: 4618 RVA: 0x00047F78 File Offset: 0x00046178
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00047F81 File Offset: 0x00046181
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00047F8A File Offset: 0x0004618A
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x0600120D RID: 4621 RVA: 0x00047F93 File Offset: 0x00046193
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x0600120E RID: 4622 RVA: 0x00047F9C File Offset: 0x0004619C
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00022829 File Offset: 0x00020A29
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00047FA5 File Offset: 0x000461A5
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00047FAE File Offset: 0x000461AE
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00047FB7 File Offset: 0x000461B7
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x00047FC0 File Offset: 0x000461C0
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00047FC9 File Offset: 0x000461C9
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00047FD2 File Offset: 0x000461D2
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00047FDB File Offset: 0x000461DB
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06001217 RID: 4631 RVA: 0x00047FE4 File Offset: 0x000461E4
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "UInt16", "DateTime"));
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x00047FFF File Offset: 0x000461FF
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400135B RID: 4955
		private readonly ushort m_value;

		// Token: 0x0400135C RID: 4956
		public const ushort MaxValue = 65535;

		// Token: 0x0400135D RID: 4957
		public const ushort MinValue = 0;
	}
}

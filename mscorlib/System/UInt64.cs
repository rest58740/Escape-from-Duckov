using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x020001A8 RID: 424
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct UInt64 : IComparable, IConvertible, IFormattable, IComparable<ulong>, IEquatable<ulong>, ISpanFormattable
	{
		// Token: 0x0600123C RID: 4668 RVA: 0x00048264 File Offset: 0x00046464
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is ulong))
			{
				throw new ArgumentException("Object must be of type UInt64.");
			}
			ulong num = (ulong)value;
			if (this < num)
			{
				return -1;
			}
			if (this > num)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0004829F File Offset: 0x0004649F
		public int CompareTo(ulong value)
		{
			if (this < value)
			{
				return -1;
			}
			if (this > value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x000482B0 File Offset: 0x000464B0
		public override bool Equals(object obj)
		{
			return obj is ulong && this == (ulong)obj;
		}

		// Token: 0x0600123F RID: 4671 RVA: 0x000325D6 File Offset: 0x000307D6
		[NonVersionable]
		public bool Equals(ulong obj)
		{
			return this == obj;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x000482C6 File Offset: 0x000464C6
		public override int GetHashCode()
		{
			return (int)this ^ (int)(this >> 32);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x000482D2 File Offset: 0x000464D2
		public override string ToString()
		{
			return Number.FormatUInt64(this, null, null);
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000482E2 File Offset: 0x000464E2
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt64(this, null, provider);
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x000482F2 File Offset: 0x000464F2
		public string ToString(string format)
		{
			return Number.FormatUInt64(this, format, null);
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00048302 File Offset: 0x00046502
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt64(this, format, provider);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00048312 File Offset: 0x00046512
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatUInt64(this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00048320 File Offset: 0x00046520
		[CLSCompliant(false)]
		public static ulong Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x0004833D File Offset: 0x0004653D
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt64(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00048360 File Offset: 0x00046560
		[CLSCompliant(false)]
		public static ulong Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x0004837E File Offset: 0x0004657E
		[CLSCompliant(false)]
		public static ulong Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x000483A2 File Offset: 0x000465A2
		[CLSCompliant(false)]
		public static ulong Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt64(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x000483B7 File Offset: 0x000465B7
		[CLSCompliant(false)]
		public static bool TryParse(string s, out ulong result)
		{
			if (s == null)
			{
				result = 0UL;
				return false;
			}
			return Number.TryParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x000483D4 File Offset: 0x000465D4
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, out ulong result)
		{
			return Number.TryParseUInt64(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x000483E3 File Offset: 0x000465E3
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out ulong result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0UL;
				return false;
			}
			return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00048407 File Offset: 0x00046607
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out ulong result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt64(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0004841D File Offset: 0x0004661D
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt64;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00048421 File Offset: 0x00046621
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x0004842A File Offset: 0x0004662A
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00048433 File Offset: 0x00046633
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0004843C File Offset: 0x0004663C
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00048445 File Offset: 0x00046645
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0004844E File Offset: 0x0004664E
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00048457 File Offset: 0x00046657
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00048460 File Offset: 0x00046660
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00048469 File Offset: 0x00046669
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00032780 File Offset: 0x00030980
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00048472 File Offset: 0x00046672
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x0004847B File Offset: 0x0004667B
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00048484 File Offset: 0x00046684
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x0004848D File Offset: 0x0004668D
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "UInt64", "DateTime"));
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x000484A8 File Offset: 0x000466A8
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001361 RID: 4961
		private readonly ulong m_value;

		// Token: 0x04001362 RID: 4962
		public const ulong MaxValue = 18446744073709551615UL;

		// Token: 0x04001363 RID: 4963
		public const ulong MinValue = 0UL;
	}
}

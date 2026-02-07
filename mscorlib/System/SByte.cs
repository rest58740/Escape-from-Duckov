using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x0200017A RID: 378
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct SByte : IComparable, IConvertible, IFormattable, IComparable<sbyte>, IEquatable<sbyte>, ISpanFormattable
	{
		// Token: 0x06000EE9 RID: 3817 RVA: 0x0003CAA6 File Offset: 0x0003ACA6
		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is sbyte))
			{
				throw new ArgumentException("Object must be of type SByte.");
			}
			return (int)(this - (sbyte)obj);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0003CAC9 File Offset: 0x0003ACC9
		public int CompareTo(sbyte value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0003CACF File Offset: 0x0003ACCF
		public override bool Equals(object obj)
		{
			return obj is sbyte && this == (sbyte)obj;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0003CAE5 File Offset: 0x0003ACE5
		[NonVersionable]
		public bool Equals(sbyte obj)
		{
			return this == obj;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0003CAEC File Offset: 0x0003ACEC
		public override int GetHashCode()
		{
			return (int)this ^ (int)this << 8;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0003CAF5 File Offset: 0x0003ACF5
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, null);
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0003CB05 File Offset: 0x0003AD05
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, provider);
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0003CB15 File Offset: 0x0003AD15
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0003CB20 File Offset: 0x0003AD20
		public string ToString(string format, IFormatProvider provider)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				return Number.FormatUInt32((uint)this & 255U, format, provider);
			}
			return Number.FormatInt32((int)this, format, provider);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0003CB78 File Offset: 0x0003AD78
		public unsafe bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			if (this < 0 && format.Length > 0 && (*format[0] == 88 || *format[0] == 120))
			{
				return Number.TryFormatUInt32((uint)this & 255U, format, provider, destination, out charsWritten);
			}
			return Number.TryFormatInt32((int)this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x0003CBCD File Offset: 0x0003ADCD
		[CLSCompliant(false)]
		public static sbyte Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0003CBEA File Offset: 0x0003ADEA
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x0003CC0D File Offset: 0x0003AE0D
		[CLSCompliant(false)]
		public static sbyte Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x0003CC2B File Offset: 0x0003AE2B
		[CLSCompliant(false)]
		public static sbyte Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x0003CC4F File Offset: 0x0003AE4F
		[CLSCompliant(false)]
		public static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x0003CC64 File Offset: 0x0003AE64
		private static sbyte Parse(string s, NumberStyles style, NumberFormatInfo info)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return sbyte.Parse(s, style, info);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x0003CC80 File Offset: 0x0003AE80
		private static sbyte Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException("Value was either too large or too small for a signed byte.", innerException);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					throw new OverflowException("Value was either too large or too small for a signed byte.");
				}
				return (sbyte)num;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					throw new OverflowException("Value was either too large or too small for a signed byte.");
				}
				return (sbyte)num;
			}
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x0003CCF4 File Offset: 0x0003AEF4
		[CLSCompliant(false)]
		public static bool TryParse(string s, out sbyte result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x0003CD10 File Offset: 0x0003AF10
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, out sbyte result)
		{
			return sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003CD1F File Offset: 0x0003AF1F
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x0003CD42 File Offset: 0x0003AF42
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out sbyte result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return sbyte.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x0003CD58 File Offset: 0x0003AF58
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out sbyte result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 255)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
			else
			{
				if (num < -128 || num > 127)
				{
					return false;
				}
				result = (sbyte)num;
				return true;
			}
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x0003CDA4 File Offset: 0x0003AFA4
		public TypeCode GetTypeCode()
		{
			return TypeCode.SByte;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x0003CDA7 File Offset: 0x0003AFA7
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x0003CDB0 File Offset: 0x0003AFB0
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x0003CDBD File Offset: 0x0003AFBD
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0003CDC6 File Offset: 0x0003AFC6
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x0003CDCF File Offset: 0x0003AFCF
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return (int)this;
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0003CDD8 File Offset: 0x0003AFD8
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0003CDE1 File Offset: 0x0003AFE1
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0003CDEA File Offset: 0x0003AFEA
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0003CDF3 File Offset: 0x0003AFF3
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x0003CDFC File Offset: 0x0003AFFC
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000F0C RID: 3852 RVA: 0x0003CE05 File Offset: 0x0003B005
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x0003CE0E File Offset: 0x0003B00E
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "SByte", "DateTime"));
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0003CE29 File Offset: 0x0003B029
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x040012D8 RID: 4824
		private readonly sbyte m_value;

		// Token: 0x040012D9 RID: 4825
		public const sbyte MaxValue = 127;

		// Token: 0x040012DA RID: 4826
		public const sbyte MinValue = -128;
	}
}

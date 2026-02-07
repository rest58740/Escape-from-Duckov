using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x02000148 RID: 328
	[Serializable]
	public readonly struct Int16 : IComparable, IConvertible, IFormattable, IComparable<short>, IEquatable<short>, ISpanFormattable
	{
		// Token: 0x06000C1E RID: 3102 RVA: 0x00031F93 File Offset: 0x00030193
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (value is short)
			{
				return (int)(this - (short)value);
			}
			throw new ArgumentException("Object must be of type Int16.");
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00031FB6 File Offset: 0x000301B6
		public int CompareTo(short value)
		{
			return (int)(this - value);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00031FBC File Offset: 0x000301BC
		public override bool Equals(object obj)
		{
			return obj is short && this == (short)obj;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00031FD2 File Offset: 0x000301D2
		[NonVersionable]
		public bool Equals(short obj)
		{
			return this == obj;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x00031FD9 File Offset: 0x000301D9
		public override int GetHashCode()
		{
			return (int)((ushort)this) | (int)this << 16;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x00031FE4 File Offset: 0x000301E4
		public override string ToString()
		{
			return Number.FormatInt32((int)this, null, null);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x00031FF4 File Offset: 0x000301F4
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatInt32((int)this, null, provider);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x00032004 File Offset: 0x00030204
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x00032010 File Offset: 0x00030210
		public string ToString(string format, IFormatProvider provider)
		{
			if (this < 0 && format != null && format.Length > 0 && (format[0] == 'X' || format[0] == 'x'))
			{
				return Number.FormatUInt32((uint)this & 65535U, format, provider);
			}
			return Number.FormatInt32((int)this, format, provider);
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x00032068 File Offset: 0x00030268
		public unsafe bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			if (this < 0 && format.Length > 0 && (*format[0] == 88 || *format[0] == 120))
			{
				return Number.TryFormatUInt32((uint)this & 65535U, format, provider, destination, out charsWritten);
			}
			return Number.TryFormatInt32((int)this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x000320BD File Offset: 0x000302BD
		public static short Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000320DA File Offset: 0x000302DA
		public static short Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return short.Parse(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000320FD File Offset: 0x000302FD
		public static short Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0003211B File Offset: 0x0003031B
		public static short Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return short.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0003213F File Offset: 0x0003033F
		public static short Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.Parse(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x00032154 File Offset: 0x00030354
		private static short Parse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info)
		{
			int num = 0;
			try
			{
				num = Number.ParseInt32(s, style, info);
			}
			catch (OverflowException innerException)
			{
				throw new OverflowException("Value was either too large or too small for an Int16.", innerException);
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					throw new OverflowException("Value was either too large or too small for an Int16.");
				}
				return (short)num;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					throw new OverflowException("Value was either too large or too small for an Int16.");
				}
				return (short)num;
			}
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000321CC File Offset: 0x000303CC
		public static bool TryParse(string s, out short result)
		{
			if (s == null)
			{
				result = 0;
				return false;
			}
			return short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x000321E8 File Offset: 0x000303E8
		public static bool TryParse(ReadOnlySpan<char> s, out short result)
		{
			return short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000321F7 File Offset: 0x000303F7
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out short result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0;
				return false;
			}
			return short.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0003221A File Offset: 0x0003041A
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out short result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return short.TryParse(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00032230 File Offset: 0x00030430
		private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out short result)
		{
			result = 0;
			int num;
			if (!Number.TryParseInt32(s, style, info, out num))
			{
				return false;
			}
			if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
			{
				if (num < 0 || num > 65535)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
			else
			{
				if (num < -32768 || num > 32767)
				{
					return false;
				}
				result = (short)num;
				return true;
			}
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00032282 File Offset: 0x00030482
		public TypeCode GetTypeCode()
		{
			return TypeCode.Int16;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00032285 File Offset: 0x00030485
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0003228E File Offset: 0x0003048E
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x00032297 File Offset: 0x00030497
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000322A0 File Offset: 0x000304A0
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x000322A9 File Offset: 0x000304A9
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x000322AD File Offset: 0x000304AD
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x000322B6 File Offset: 0x000304B6
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000322BF File Offset: 0x000304BF
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return Convert.ToUInt32(this);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x000322C8 File Offset: 0x000304C8
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x000322D1 File Offset: 0x000304D1
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x000322DA File Offset: 0x000304DA
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000322E3 File Offset: 0x000304E3
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x000322EC File Offset: 0x000304EC
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000322F5 File Offset: 0x000304F5
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "Int16", "DateTime"));
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00032310 File Offset: 0x00030510
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001252 RID: 4690
		private readonly short m_value;

		// Token: 0x04001253 RID: 4691
		public const short MaxValue = 32767;

		// Token: 0x04001254 RID: 4692
		public const short MinValue = -32768;
	}
}

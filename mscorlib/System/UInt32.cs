using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security;

namespace System
{
	// Token: 0x020001A7 RID: 423
	[CLSCompliant(false)]
	[Serializable]
	public readonly struct UInt32 : IComparable, IConvertible, IFormattable, IComparable<uint>, IEquatable<uint>, ISpanFormattable
	{
		// Token: 0x06001219 RID: 4633 RVA: 0x00048010 File Offset: 0x00046210
		public int CompareTo(object value)
		{
			if (value == null)
			{
				return 1;
			}
			if (!(value is uint))
			{
				throw new ArgumentException("Object must be of type UInt32.");
			}
			uint num = (uint)value;
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

		// Token: 0x0600121A RID: 4634 RVA: 0x0004804B File Offset: 0x0004624B
		public int CompareTo(uint value)
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

		// Token: 0x0600121B RID: 4635 RVA: 0x0004805C File Offset: 0x0004625C
		public override bool Equals(object obj)
		{
			return obj is uint && this == (uint)obj;
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00048072 File Offset: 0x00046272
		[NonVersionable]
		public bool Equals(uint obj)
		{
			return this == obj;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x00048079 File Offset: 0x00046279
		public override int GetHashCode()
		{
			return (int)this;
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0004807D File Offset: 0x0004627D
		public override string ToString()
		{
			return Number.FormatUInt32(this, null, null);
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0004808D File Offset: 0x0004628D
		[SecuritySafeCritical]
		public string ToString(IFormatProvider provider)
		{
			return Number.FormatUInt32(this, null, provider);
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0004809D File Offset: 0x0004629D
		public string ToString(string format)
		{
			return Number.FormatUInt32(this, format, null);
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x000480AD File Offset: 0x000462AD
		[SecuritySafeCritical]
		public string ToString(string format, IFormatProvider provider)
		{
			return Number.FormatUInt32(this, format, provider);
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x000480BD File Offset: 0x000462BD
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default(ReadOnlySpan<char>), IFormatProvider provider = null)
		{
			return Number.TryFormatUInt32(this, format, provider, destination, out charsWritten);
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x000480CB File Offset: 0x000462CB
		[CLSCompliant(false)]
		public static uint Parse(string s)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x000480E8 File Offset: 0x000462E8
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt32(s, style, NumberFormatInfo.CurrentInfo);
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x0004810B File Offset: 0x0004630B
		[CLSCompliant(false)]
		public static uint Parse(string s, IFormatProvider provider)
		{
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00048129 File Offset: 0x00046329
		[CLSCompliant(false)]
		public static uint Parse(string s, NumberStyles style, IFormatProvider provider)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				ThrowHelper.ThrowArgumentNullException(ExceptionArgument.s);
			}
			return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x0004814D File Offset: 0x0004634D
		[CLSCompliant(false)]
		public static uint Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider provider = null)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.ParseUInt32(s, style, NumberFormatInfo.GetInstance(provider));
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00048162 File Offset: 0x00046362
		[CLSCompliant(false)]
		public static bool TryParse(string s, out uint result)
		{
			if (s == null)
			{
				result = 0U;
				return false;
			}
			return Number.TryParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x0004817E File Offset: 0x0004637E
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, out uint result)
		{
			return Number.TryParseUInt32(s, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x0004818D File Offset: 0x0004638D
		[CLSCompliant(false)]
		public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out uint result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			if (s == null)
			{
				result = 0U;
				return false;
			}
			return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x000481B0 File Offset: 0x000463B0
		[CLSCompliant(false)]
		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out uint result)
		{
			NumberFormatInfo.ValidateParseStyleInteger(style);
			return Number.TryParseUInt32(s, style, NumberFormatInfo.GetInstance(provider), out result);
		}

		// Token: 0x0600122C RID: 4652 RVA: 0x000481C6 File Offset: 0x000463C6
		public TypeCode GetTypeCode()
		{
			return TypeCode.UInt32;
		}

		// Token: 0x0600122D RID: 4653 RVA: 0x000481CA File Offset: 0x000463CA
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return Convert.ToBoolean(this);
		}

		// Token: 0x0600122E RID: 4654 RVA: 0x000481D3 File Offset: 0x000463D3
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return Convert.ToChar(this);
		}

		// Token: 0x0600122F RID: 4655 RVA: 0x000481DC File Offset: 0x000463DC
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return Convert.ToSByte(this);
		}

		// Token: 0x06001230 RID: 4656 RVA: 0x000481E5 File Offset: 0x000463E5
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return Convert.ToByte(this);
		}

		// Token: 0x06001231 RID: 4657 RVA: 0x000481EE File Offset: 0x000463EE
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return Convert.ToInt16(this);
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x000481F7 File Offset: 0x000463F7
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return Convert.ToUInt16(this);
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00048200 File Offset: 0x00046400
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return Convert.ToInt32(this);
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00048079 File Offset: 0x00046279
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return this;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00048209 File Offset: 0x00046409
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return Convert.ToInt64(this);
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00048212 File Offset: 0x00046412
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return Convert.ToUInt64(this);
		}

		// Token: 0x06001237 RID: 4663 RVA: 0x0004821B File Offset: 0x0004641B
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return Convert.ToSingle(this);
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x00048224 File Offset: 0x00046424
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return Convert.ToDouble(this);
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0004822D File Offset: 0x0004642D
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return Convert.ToDecimal(this);
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x00048236 File Offset: 0x00046436
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException(SR.Format("Invalid cast from '{0}' to '{1}'.", "UInt32", "DateTime"));
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x00048251 File Offset: 0x00046451
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x0400135E RID: 4958
		private readonly uint m_value;

		// Token: 0x0400135F RID: 4959
		public const uint MaxValue = 4294967295U;

		// Token: 0x04001360 RID: 4960
		public const uint MinValue = 0U;
	}
}

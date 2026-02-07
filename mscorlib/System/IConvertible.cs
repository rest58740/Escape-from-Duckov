using System;

namespace System
{
	// Token: 0x0200013B RID: 315
	[CLSCompliant(false)]
	public interface IConvertible
	{
		// Token: 0x06000BEB RID: 3051
		TypeCode GetTypeCode();

		// Token: 0x06000BEC RID: 3052
		bool ToBoolean(IFormatProvider provider);

		// Token: 0x06000BED RID: 3053
		char ToChar(IFormatProvider provider);

		// Token: 0x06000BEE RID: 3054
		sbyte ToSByte(IFormatProvider provider);

		// Token: 0x06000BEF RID: 3055
		byte ToByte(IFormatProvider provider);

		// Token: 0x06000BF0 RID: 3056
		short ToInt16(IFormatProvider provider);

		// Token: 0x06000BF1 RID: 3057
		ushort ToUInt16(IFormatProvider provider);

		// Token: 0x06000BF2 RID: 3058
		int ToInt32(IFormatProvider provider);

		// Token: 0x06000BF3 RID: 3059
		uint ToUInt32(IFormatProvider provider);

		// Token: 0x06000BF4 RID: 3060
		long ToInt64(IFormatProvider provider);

		// Token: 0x06000BF5 RID: 3061
		ulong ToUInt64(IFormatProvider provider);

		// Token: 0x06000BF6 RID: 3062
		float ToSingle(IFormatProvider provider);

		// Token: 0x06000BF7 RID: 3063
		double ToDouble(IFormatProvider provider);

		// Token: 0x06000BF8 RID: 3064
		decimal ToDecimal(IFormatProvider provider);

		// Token: 0x06000BF9 RID: 3065
		DateTime ToDateTime(IFormatProvider provider);

		// Token: 0x06000BFA RID: 3066
		string ToString(IFormatProvider provider);

		// Token: 0x06000BFB RID: 3067
		object ToType(Type conversionType, IFormatProvider provider);
	}
}

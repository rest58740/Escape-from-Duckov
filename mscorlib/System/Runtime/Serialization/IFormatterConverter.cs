using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000648 RID: 1608
	[CLSCompliant(false)]
	public interface IFormatterConverter
	{
		// Token: 0x06003C3C RID: 15420
		object Convert(object value, Type type);

		// Token: 0x06003C3D RID: 15421
		object Convert(object value, TypeCode typeCode);

		// Token: 0x06003C3E RID: 15422
		bool ToBoolean(object value);

		// Token: 0x06003C3F RID: 15423
		char ToChar(object value);

		// Token: 0x06003C40 RID: 15424
		sbyte ToSByte(object value);

		// Token: 0x06003C41 RID: 15425
		byte ToByte(object value);

		// Token: 0x06003C42 RID: 15426
		short ToInt16(object value);

		// Token: 0x06003C43 RID: 15427
		ushort ToUInt16(object value);

		// Token: 0x06003C44 RID: 15428
		int ToInt32(object value);

		// Token: 0x06003C45 RID: 15429
		uint ToUInt32(object value);

		// Token: 0x06003C46 RID: 15430
		long ToInt64(object value);

		// Token: 0x06003C47 RID: 15431
		ulong ToUInt64(object value);

		// Token: 0x06003C48 RID: 15432
		float ToSingle(object value);

		// Token: 0x06003C49 RID: 15433
		double ToDouble(object value);

		// Token: 0x06003C4A RID: 15434
		decimal ToDecimal(object value);

		// Token: 0x06003C4B RID: 15435
		DateTime ToDateTime(object value);

		// Token: 0x06003C4C RID: 15436
		string ToString(object value);
	}
}

using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x0200010A RID: 266
	[Serializable]
	public sealed class DBNull : ISerializable, IConvertible
	{
		// Token: 0x0600098D RID: 2445 RVA: 0x0000259F File Offset: 0x0000079F
		private DBNull()
		{
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x000258C4 File Offset: 0x00023AC4
		private DBNull(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException("Only one DBNull instance may exist, and calls to DBNull deserialization methods are not allowed.");
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x000258D6 File Offset: 0x00023AD6
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			UnitySerializationHolder.GetUnitySerializationInfo(info, 2);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x000258DF File Offset: 0x00023ADF
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x000258DF File Offset: 0x00023ADF
		public string ToString(IFormatProvider provider)
		{
			return string.Empty;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00015831 File Offset: 0x00013A31
		public TypeCode GetTypeCode()
		{
			return TypeCode.DBNull;
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x000258E6 File Offset: 0x00023AE6
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x000258E6 File Offset: 0x00023AE6
		char IConvertible.ToChar(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x000258E6 File Offset: 0x00023AE6
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x000258E6 File Offset: 0x00023AE6
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x000258E6 File Offset: 0x00023AE6
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x000258E6 File Offset: 0x00023AE6
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x000258E6 File Offset: 0x00023AE6
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000258E6 File Offset: 0x00023AE6
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x000258E6 File Offset: 0x00023AE6
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x000258E6 File Offset: 0x00023AE6
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000258E6 File Offset: 0x00023AE6
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000258E6 File Offset: 0x00023AE6
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x000258E6 File Offset: 0x00023AE6
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x000258E6 File Offset: 0x00023AE6
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			throw new InvalidCastException("Object cannot be cast from DBNull to other types.");
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0001B8BE File Offset: 0x00019ABE
		object IConvertible.ToType(Type type, IFormatProvider provider)
		{
			return Convert.DefaultToType(this, type, provider);
		}

		// Token: 0x04001092 RID: 4242
		public static readonly DBNull Value = new DBNull();
	}
}

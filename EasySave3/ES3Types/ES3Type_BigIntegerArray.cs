using System;
using System.Numerics;

namespace ES3Types
{
	// Token: 0x0200002F RID: 47
	public class ES3Type_BigIntegerArray : ES3ArrayType
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0000966A File Offset: 0x0000786A
		public ES3Type_BigIntegerArray() : base(typeof(BigInteger[]), ES3Type_BigInteger.Instance)
		{
			ES3Type_BigIntegerArray.Instance = this;
		}

		// Token: 0x04000073 RID: 115
		public static ES3Type Instance;
	}
}

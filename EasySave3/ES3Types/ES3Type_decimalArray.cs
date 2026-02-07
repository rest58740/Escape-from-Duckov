using System;

namespace ES3Types
{
	// Token: 0x0200003A RID: 58
	public class ES3Type_decimalArray : ES3ArrayType
	{
		// Token: 0x0600027E RID: 638 RVA: 0x000098DF File Offset: 0x00007ADF
		public ES3Type_decimalArray() : base(typeof(decimal[]), ES3Type_decimal.Instance)
		{
			ES3Type_decimalArray.Instance = this;
		}

		// Token: 0x0400007E RID: 126
		public static ES3Type Instance;
	}
}

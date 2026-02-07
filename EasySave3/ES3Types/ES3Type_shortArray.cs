using System;

namespace ES3Types
{
	// Token: 0x0200004C RID: 76
	public class ES3Type_shortArray : ES3ArrayType
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0000A132 File Offset: 0x00008332
		public ES3Type_shortArray() : base(typeof(short[]), ES3Type_short.Instance)
		{
			ES3Type_shortArray.Instance = this;
		}

		// Token: 0x04000091 RID: 145
		public static ES3Type Instance;
	}
}

using System;

namespace ES3Types
{
	// Token: 0x02000032 RID: 50
	public class ES3Type_boolArray : ES3ArrayType
	{
		// Token: 0x0600026C RID: 620 RVA: 0x00009716 File Offset: 0x00007916
		public ES3Type_boolArray() : base(typeof(bool[]), ES3Type_bool.Instance)
		{
			ES3Type_boolArray.Instance = this;
		}

		// Token: 0x04000076 RID: 118
		public static ES3Type Instance;
	}
}

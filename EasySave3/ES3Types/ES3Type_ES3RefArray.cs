using System;

namespace ES3Types
{
	// Token: 0x0200003F RID: 63
	public class ES3Type_ES3RefArray : ES3ArrayType
	{
		// Token: 0x0600028A RID: 650 RVA: 0x00009EA8 File Offset: 0x000080A8
		public ES3Type_ES3RefArray() : base(typeof(ES3Ref[]), ES3Type_ES3Ref.Instance)
		{
			ES3Type_ES3RefArray.Instance = this;
		}

		// Token: 0x04000084 RID: 132
		public static ES3Type Instance = new ES3Type_ES3RefArray();
	}
}

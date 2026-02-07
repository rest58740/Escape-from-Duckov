using System;

namespace ES3Types
{
	// Token: 0x0200003C RID: 60
	public class ES3Type_doubleArray : ES3ArrayType
	{
		// Token: 0x06000282 RID: 642 RVA: 0x00009940 File Offset: 0x00007B40
		public ES3Type_doubleArray() : base(typeof(double[]), ES3Type_double.Instance)
		{
			ES3Type_doubleArray.Instance = this;
		}

		// Token: 0x04000080 RID: 128
		public static ES3Type Instance;
	}
}

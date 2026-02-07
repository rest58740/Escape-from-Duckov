using System;

namespace ES3Types
{
	// Token: 0x02000038 RID: 56
	public class ES3Type_DateTimeArray : ES3ArrayType
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000987E File Offset: 0x00007A7E
		public ES3Type_DateTimeArray() : base(typeof(DateTime[]), ES3Type_DateTime.Instance)
		{
			ES3Type_DateTimeArray.Instance = this;
		}

		// Token: 0x0400007C RID: 124
		public static ES3Type Instance;
	}
}

using System;

namespace ES3Types
{
	// Token: 0x02000093 RID: 147
	public class ES3Type_GuidArray : ES3ArrayType
	{
		// Token: 0x06000362 RID: 866 RVA: 0x00011330 File Offset: 0x0000F530
		public ES3Type_GuidArray() : base(typeof(Guid[]), ES3Type_Guid.Instance)
		{
			ES3Type_GuidArray.Instance = this;
		}

		// Token: 0x040000D6 RID: 214
		public static ES3Type Instance;
	}
}

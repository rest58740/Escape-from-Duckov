using System;
using System.Collections;

namespace ES3Types
{
	// Token: 0x02000027 RID: 39
	public class ES3UserType_ArrayListArray : ES3ArrayType
	{
		// Token: 0x06000235 RID: 565 RVA: 0x00008928 File Offset: 0x00006B28
		public ES3UserType_ArrayListArray() : base(typeof(ArrayList[]), ES3Type_ArrayList.Instance)
		{
			ES3UserType_ArrayListArray.Instance = this;
		}

		// Token: 0x04000062 RID: 98
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine.UI;

namespace ES3Types
{
	// Token: 0x0200006E RID: 110
	public class ES3Type_RawImageArray : ES3ArrayType
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		public ES3Type_RawImageArray() : base(typeof(RawImage[]), ES3Type_RawImage.Instance)
		{
			ES3Type_RawImageArray.Instance = this;
		}

		// Token: 0x040000AD RID: 173
		public static ES3Type Instance;
	}
}

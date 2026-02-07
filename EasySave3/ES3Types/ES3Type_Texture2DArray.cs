using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000BE RID: 190
	public class ES3Type_Texture2DArray : ES3ArrayType
	{
		// Token: 0x060003E1 RID: 993 RVA: 0x0001940D File Offset: 0x0001760D
		public ES3Type_Texture2DArray() : base(typeof(Texture2D[]), ES3Type_Texture2D.Instance)
		{
			ES3Type_Texture2DArray.Instance = this;
		}

		// Token: 0x04000101 RID: 257
		public static ES3Type Instance;
	}
}

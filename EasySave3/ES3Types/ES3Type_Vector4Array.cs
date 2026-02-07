using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000CB RID: 203
	public class ES3Type_Vector4Array : ES3ArrayType
	{
		// Token: 0x06000402 RID: 1026 RVA: 0x0001A3ED File Offset: 0x000185ED
		public ES3Type_Vector4Array() : base(typeof(Vector4[]), ES3Type_Vector4.Instance)
		{
			ES3Type_Vector4Array.Instance = this;
		}

		// Token: 0x0400010E RID: 270
		public static ES3Type Instance;
	}
}

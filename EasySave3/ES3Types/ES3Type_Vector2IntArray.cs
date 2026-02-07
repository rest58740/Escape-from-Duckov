using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000C5 RID: 197
	public class ES3Type_Vector2IntArray : ES3ArrayType
	{
		// Token: 0x060003F5 RID: 1013 RVA: 0x0001A112 File Offset: 0x00018312
		public ES3Type_Vector2IntArray() : base(typeof(Vector2Int[]), ES3Type_Vector2Int.Instance)
		{
			ES3Type_Vector2IntArray.Instance = this;
		}

		// Token: 0x04000108 RID: 264
		public static ES3Type Instance;
	}
}

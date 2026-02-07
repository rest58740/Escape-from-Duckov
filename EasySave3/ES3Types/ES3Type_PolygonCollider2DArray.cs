using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200006C RID: 108
	public class ES3Type_PolygonCollider2DArray : ES3ArrayType
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x0000D574 File Offset: 0x0000B774
		public ES3Type_PolygonCollider2DArray() : base(typeof(PolygonCollider2D[]), ES3Type_PolygonCollider2D.Instance)
		{
			ES3Type_PolygonCollider2DArray.Instance = this;
		}

		// Token: 0x040000AB RID: 171
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000C9 RID: 201
	public class ES3Type_Vector3IntArray : ES3ArrayType
	{
		// Token: 0x060003FD RID: 1021 RVA: 0x0001A2A5 File Offset: 0x000184A5
		public ES3Type_Vector3IntArray() : base(typeof(Vector3Int[]), ES3Type_Vector3Int.Instance)
		{
			ES3Type_Vector3IntArray.Instance = this;
		}

		// Token: 0x0400010C RID: 268
		public static ES3Type Instance;
	}
}

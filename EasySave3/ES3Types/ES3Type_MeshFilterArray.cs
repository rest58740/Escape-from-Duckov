using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000067 RID: 103
	public class ES3Type_MeshFilterArray : ES3ArrayType
	{
		// Token: 0x060002EB RID: 747 RVA: 0x0000C1FC File Offset: 0x0000A3FC
		public ES3Type_MeshFilterArray() : base(typeof(MeshFilter[]), ES3Type_MeshFilter.Instance)
		{
			ES3Type_MeshFilterArray.Instance = this;
		}

		// Token: 0x040000A6 RID: 166
		public static ES3Type Instance;
	}
}

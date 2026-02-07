using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000078 RID: 120
	public class ES3Type_BoneWeightArray : ES3ArrayType
	{
		// Token: 0x06000314 RID: 788 RVA: 0x0000F2B3 File Offset: 0x0000D4B3
		public ES3Type_BoneWeightArray() : base(typeof(BoneWeight[]), ES3Type_BoneWeight.Instance)
		{
			ES3Type_BoneWeightArray.Instance = this;
		}

		// Token: 0x040000B8 RID: 184
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000A9 RID: 169
	public class ES3Type_QuaternionArray : ES3ArrayType
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x00015678 File Offset: 0x00013878
		public ES3Type_QuaternionArray() : base(typeof(Quaternion[]), ES3Type_Quaternion.Instance)
		{
			ES3Type_QuaternionArray.Instance = this;
		}

		// Token: 0x040000EC RID: 236
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000096 RID: 150
	public class ES3Type_KeyframeArray : ES3ArrayType
	{
		// Token: 0x0600036A RID: 874 RVA: 0x00011594 File Offset: 0x0000F794
		public ES3Type_KeyframeArray() : base(typeof(Keyframe[]), ES3Type_Keyframe.Instance)
		{
			ES3Type_KeyframeArray.Instance = this;
		}

		// Token: 0x040000D9 RID: 217
		public static ES3Type Instance;
	}
}

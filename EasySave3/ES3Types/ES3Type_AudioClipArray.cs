using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000076 RID: 118
	public class ES3Type_AudioClipArray : ES3ArrayType
	{
		// Token: 0x06000310 RID: 784 RVA: 0x0000F0DC File Offset: 0x0000D2DC
		public ES3Type_AudioClipArray() : base(typeof(AudioClip[]), ES3Type_AudioClip.Instance)
		{
			ES3Type_AudioClipArray.Instance = this;
		}

		// Token: 0x040000B6 RID: 182
		public static ES3Type Instance;
	}
}

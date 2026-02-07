using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000AD RID: 173
	public class ES3Type_RenderTextureArray : ES3ArrayType
	{
		// Token: 0x060003AD RID: 941 RVA: 0x0001655A File Offset: 0x0001475A
		public ES3Type_RenderTextureArray() : base(typeof(RenderTexture[]), ES3Type_RenderTexture.Instance)
		{
			ES3Type_RenderTextureArray.Instance = this;
		}

		// Token: 0x040000F0 RID: 240
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x020000B6 RID: 182
	public class ES3Type_SkinnedMeshRendererArray : ES3ArrayType
	{
		// Token: 0x060003CA RID: 970 RVA: 0x00018488 File Offset: 0x00016688
		public ES3Type_SkinnedMeshRendererArray() : base(typeof(SkinnedMeshRenderer[]), ES3Type_SkinnedMeshRenderer.Instance)
		{
			ES3Type_SkinnedMeshRendererArray.Instance = this;
		}

		// Token: 0x040000F9 RID: 249
		public static ES3Type Instance;
	}
}

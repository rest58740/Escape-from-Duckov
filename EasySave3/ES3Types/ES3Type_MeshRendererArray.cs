using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000069 RID: 105
	public class ES3Type_MeshRendererArray : ES3ArrayType
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0000C828 File Offset: 0x0000AA28
		public ES3Type_MeshRendererArray() : base(typeof(MeshRenderer[]), ES3Type_MeshRenderer.Instance)
		{
			ES3Type_MeshRendererArray.Instance = this;
		}

		// Token: 0x040000A8 RID: 168
		public static ES3Type Instance;
	}
}

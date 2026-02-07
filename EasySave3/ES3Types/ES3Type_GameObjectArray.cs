using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200008C RID: 140
	public class ES3Type_GameObjectArray : ES3ArrayType
	{
		// Token: 0x06000352 RID: 850 RVA: 0x0001107F File Offset: 0x0000F27F
		public ES3Type_GameObjectArray() : base(typeof(GameObject[]), ES3Type_GameObject.Instance)
		{
			ES3Type_GameObjectArray.Instance = this;
		}

		// Token: 0x040000CF RID: 207
		public static ES3Type Instance;
	}
}

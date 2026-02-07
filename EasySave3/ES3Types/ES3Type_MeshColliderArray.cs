using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000065 RID: 101
	public class ES3Type_MeshColliderArray : ES3ArrayType
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000C11C File Offset: 0x0000A31C
		public ES3Type_MeshColliderArray() : base(typeof(MeshCollider[]), ES3Type_MeshCollider.Instance)
		{
			ES3Type_MeshColliderArray.Instance = this;
		}

		// Token: 0x040000A4 RID: 164
		public static ES3Type Instance;
	}
}

using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x02000070 RID: 112
	public class ES3UserType_RigidbodyArray : ES3ArrayType
	{
		// Token: 0x060002FE RID: 766 RVA: 0x0000E198 File Offset: 0x0000C398
		public ES3UserType_RigidbodyArray() : base(typeof(Rigidbody[]), ES3Type_Rigidbody.Instance)
		{
			ES3UserType_RigidbodyArray.Instance = this;
		}

		// Token: 0x040000AF RID: 175
		public static ES3Type Instance;
	}
}

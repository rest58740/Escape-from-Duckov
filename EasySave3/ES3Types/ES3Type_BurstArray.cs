using System;
using UnityEngine;

namespace ES3Types
{
	// Token: 0x0200007C RID: 124
	public class ES3Type_BurstArray : ES3ArrayType
	{
		// Token: 0x0600031C RID: 796 RVA: 0x0000F602 File Offset: 0x0000D802
		public ES3Type_BurstArray() : base(typeof(ParticleSystem.Burst[]), ES3Type_Burst.Instance)
		{
			ES3Type_BurstArray.Instance = this;
		}

		// Token: 0x040000BC RID: 188
		public static ES3Type Instance;
	}
}

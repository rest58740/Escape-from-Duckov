using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000016 RID: 22
	public struct ChainIKData
	{
		// Token: 0x04000031 RID: 49
		public Vector3[] positions;

		// Token: 0x04000032 RID: 50
		public float[] lengths;

		// Token: 0x04000033 RID: 51
		public Vector3 target;

		// Token: 0x04000034 RID: 52
		public float tolerance;

		// Token: 0x04000035 RID: 53
		public float maxReach;

		// Token: 0x04000036 RID: 54
		public int maxIterations;
	}
}

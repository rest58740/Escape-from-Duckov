using System;
using Unity.Collections;
using Unity.Jobs;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000022 RID: 34
	public struct KTwoBoneIKJob : IJobParallelFor
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00003BDC File Offset: 0x00001DDC
		public void Execute(int index)
		{
			KTwoBoneIkData value = this.twoBoneIkJobData[index];
			KTwoBoneIK.Solve(ref value);
			this.twoBoneIkJobData[index] = value;
		}

		// Token: 0x04000052 RID: 82
		public NativeArray<KTwoBoneIkData> twoBoneIkJobData;
	}
}

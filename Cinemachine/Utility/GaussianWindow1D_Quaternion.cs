using System;
using UnityEngine;

namespace Cinemachine.Utility
{
	// Token: 0x02000062 RID: 98
	internal class GaussianWindow1D_Quaternion : GaussianWindow1d<Quaternion>
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x000172C2 File Offset: 0x000154C2
		public GaussianWindow1D_Quaternion(float sigma, int maxKernelRadius = 10) : base(sigma, maxKernelRadius)
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x000172CC File Offset: 0x000154CC
		protected override Quaternion Compute(int windowPos)
		{
			Quaternion q = new Quaternion(0f, 0f, 0f, 0f);
			Quaternion quaternion = this.mData[this.mCurrentPos];
			Quaternion lhs = Quaternion.Inverse(quaternion);
			for (int i = 0; i < base.KernelSize; i++)
			{
				float num = this.mKernel[i];
				Quaternion quaternion2 = lhs * this.mData[windowPos];
				if (Quaternion.Dot(Quaternion.identity, quaternion2) < 0f)
				{
					num = -num;
				}
				q.x += quaternion2.x * num;
				q.y += quaternion2.y * num;
				q.z += quaternion2.z * num;
				q.w += quaternion2.w * num;
				if (++windowPos == base.KernelSize)
				{
					windowPos = 0;
				}
			}
			return quaternion * Quaternion.Normalize(q);
		}
	}
}

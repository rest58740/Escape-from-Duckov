using System;
using KINEMATION.KAnimationCore.Runtime.Core;

namespace KINEMATION.KAnimationCore.Runtime.Rig
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public struct KPose
	{
		// Token: 0x04000009 RID: 9
		public KRigElement element;

		// Token: 0x0400000A RID: 10
		public KTransform pose;

		// Token: 0x0400000B RID: 11
		public ESpaceType space;

		// Token: 0x0400000C RID: 12
		public EModifyMode modifyMode;
	}
}

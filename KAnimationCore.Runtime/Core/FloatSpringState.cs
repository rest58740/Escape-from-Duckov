using System;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001D RID: 29
	public struct FloatSpringState
	{
		// Token: 0x0600004E RID: 78 RVA: 0x0000348C File Offset: 0x0000168C
		public void Reset()
		{
			this.Error = (this.Velocity = 0f);
		}

		// Token: 0x04000043 RID: 67
		public float Velocity;

		// Token: 0x04000044 RID: 68
		public float Error;
	}
}

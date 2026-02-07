using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000029 RID: 41
	[Flags]
	public enum InjectPlayerLoopTimings
	{
		// Token: 0x04000040 RID: 64
		All = 65535,
		// Token: 0x04000041 RID: 65
		Standard = 30037,
		// Token: 0x04000042 RID: 66
		Minimum = 8464,
		// Token: 0x04000043 RID: 67
		Initialization = 1,
		// Token: 0x04000044 RID: 68
		LastInitialization = 2,
		// Token: 0x04000045 RID: 69
		EarlyUpdate = 4,
		// Token: 0x04000046 RID: 70
		LastEarlyUpdate = 8,
		// Token: 0x04000047 RID: 71
		FixedUpdate = 16,
		// Token: 0x04000048 RID: 72
		LastFixedUpdate = 32,
		// Token: 0x04000049 RID: 73
		PreUpdate = 64,
		// Token: 0x0400004A RID: 74
		LastPreUpdate = 128,
		// Token: 0x0400004B RID: 75
		Update = 256,
		// Token: 0x0400004C RID: 76
		LastUpdate = 512,
		// Token: 0x0400004D RID: 77
		PreLateUpdate = 1024,
		// Token: 0x0400004E RID: 78
		LastPreLateUpdate = 2048,
		// Token: 0x0400004F RID: 79
		PostLateUpdate = 4096,
		// Token: 0x04000050 RID: 80
		LastPostLateUpdate = 8192,
		// Token: 0x04000051 RID: 81
		TimeUpdate = 16384,
		// Token: 0x04000052 RID: 82
		LastTimeUpdate = 32768
	}
}

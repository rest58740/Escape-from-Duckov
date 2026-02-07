using System;

namespace Cysharp.Threading.Tasks
{
	// Token: 0x02000028 RID: 40
	public enum PlayerLoopTiming
	{
		// Token: 0x0400002F RID: 47
		Initialization,
		// Token: 0x04000030 RID: 48
		LastInitialization,
		// Token: 0x04000031 RID: 49
		EarlyUpdate,
		// Token: 0x04000032 RID: 50
		LastEarlyUpdate,
		// Token: 0x04000033 RID: 51
		FixedUpdate,
		// Token: 0x04000034 RID: 52
		LastFixedUpdate,
		// Token: 0x04000035 RID: 53
		PreUpdate,
		// Token: 0x04000036 RID: 54
		LastPreUpdate,
		// Token: 0x04000037 RID: 55
		Update,
		// Token: 0x04000038 RID: 56
		LastUpdate,
		// Token: 0x04000039 RID: 57
		PreLateUpdate,
		// Token: 0x0400003A RID: 58
		LastPreLateUpdate,
		// Token: 0x0400003B RID: 59
		PostLateUpdate,
		// Token: 0x0400003C RID: 60
		LastPostLateUpdate,
		// Token: 0x0400003D RID: 61
		TimeUpdate,
		// Token: 0x0400003E RID: 62
		LastTimeUpdate
	}
}

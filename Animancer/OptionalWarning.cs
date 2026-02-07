using System;

namespace Animancer
{
	// Token: 0x02000021 RID: 33
	[Flags]
	public enum OptionalWarning
	{
		// Token: 0x0400006E RID: 110
		ProOnly = 1,
		// Token: 0x0400006F RID: 111
		CreateGraphWhileDisabled = 2,
		// Token: 0x04000070 RID: 112
		CreateGraphDuringGuiEvent = 4,
		// Token: 0x04000071 RID: 113
		AnimatorDisabled = 8,
		// Token: 0x04000072 RID: 114
		NativeControllerHumanoid = 16,
		// Token: 0x04000073 RID: 115
		NativeControllerHybrid = 32,
		// Token: 0x04000074 RID: 116
		DuplicateEvent = 64,
		// Token: 0x04000075 RID: 117
		EndEventInterrupt = 128,
		// Token: 0x04000076 RID: 118
		UselessEvent = 256,
		// Token: 0x04000077 RID: 119
		LockedEvents = 512,
		// Token: 0x04000078 RID: 120
		UnsupportedEvents = 1024,
		// Token: 0x04000079 RID: 121
		UnsupportedSpeed = 2048,
		// Token: 0x0400007A RID: 122
		UnsupportedIK = 4096,
		// Token: 0x0400007B RID: 123
		MixerMinChildren = 8192,
		// Token: 0x0400007C RID: 124
		MixerSynchronizeZeroLength = 16384,
		// Token: 0x0400007D RID: 125
		CustomFadeBounds = 32768,
		// Token: 0x0400007E RID: 126
		CustomFadeNotNull = 65536,
		// Token: 0x0400007F RID: 127
		AnimatorSpeed = 131072,
		// Token: 0x04000080 RID: 128
		UnusedNode = 262144,
		// Token: 0x04000081 RID: 129
		PlayableAssetAnimatorBinding = 524288,
		// Token: 0x04000082 RID: 130
		CloneComplexState = 1048576,
		// Token: 0x04000083 RID: 131
		All = -1
	}
}

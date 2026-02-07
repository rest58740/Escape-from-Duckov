using System;

namespace DG.Tweening
{
	// Token: 0x0200000E RID: 14
	public enum LinkBehaviour
	{
		// Token: 0x04000055 RID: 85
		PauseOnDisable,
		// Token: 0x04000056 RID: 86
		PauseOnDisablePlayOnEnable,
		// Token: 0x04000057 RID: 87
		PauseOnDisableRestartOnEnable,
		// Token: 0x04000058 RID: 88
		PlayOnEnable,
		// Token: 0x04000059 RID: 89
		RestartOnEnable,
		// Token: 0x0400005A RID: 90
		KillOnDisable,
		// Token: 0x0400005B RID: 91
		KillOnDestroy,
		// Token: 0x0400005C RID: 92
		CompleteOnDisable,
		// Token: 0x0400005D RID: 93
		CompleteAndKillOnDisable,
		// Token: 0x0400005E RID: 94
		RewindOnDisable,
		// Token: 0x0400005F RID: 95
		RewindAndKillOnDisable
	}
}

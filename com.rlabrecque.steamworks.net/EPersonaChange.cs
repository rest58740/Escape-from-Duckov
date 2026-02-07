using System;

namespace Steamworks
{
	// Token: 0x0200010E RID: 270
	[Flags]
	public enum EPersonaChange
	{
		// Token: 0x04000415 RID: 1045
		k_EPersonaChangeName = 1,
		// Token: 0x04000416 RID: 1046
		k_EPersonaChangeStatus = 2,
		// Token: 0x04000417 RID: 1047
		k_EPersonaChangeComeOnline = 4,
		// Token: 0x04000418 RID: 1048
		k_EPersonaChangeGoneOffline = 8,
		// Token: 0x04000419 RID: 1049
		k_EPersonaChangeGamePlayed = 16,
		// Token: 0x0400041A RID: 1050
		k_EPersonaChangeGameServer = 32,
		// Token: 0x0400041B RID: 1051
		k_EPersonaChangeAvatar = 64,
		// Token: 0x0400041C RID: 1052
		k_EPersonaChangeJoinedSource = 128,
		// Token: 0x0400041D RID: 1053
		k_EPersonaChangeLeftSource = 256,
		// Token: 0x0400041E RID: 1054
		k_EPersonaChangeRelationshipChanged = 512,
		// Token: 0x0400041F RID: 1055
		k_EPersonaChangeNameFirstSet = 1024,
		// Token: 0x04000420 RID: 1056
		k_EPersonaChangeBroadcast = 2048,
		// Token: 0x04000421 RID: 1057
		k_EPersonaChangeNickname = 4096,
		// Token: 0x04000422 RID: 1058
		k_EPersonaChangeSteamLevel = 8192,
		// Token: 0x04000423 RID: 1059
		k_EPersonaChangeRichPresence = 16384
	}
}

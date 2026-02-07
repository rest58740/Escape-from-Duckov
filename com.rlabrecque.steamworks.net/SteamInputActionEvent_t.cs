using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001AB RID: 427
	[Serializable]
	public struct SteamInputActionEvent_t
	{
		// Token: 0x04000AE9 RID: 2793
		public InputHandle_t controllerHandle;

		// Token: 0x04000AEA RID: 2794
		public ESteamInputActionEventType eEventType;

		// Token: 0x04000AEB RID: 2795
		public SteamInputActionEvent_t.OptionValue m_val;

		// Token: 0x020001F9 RID: 505
		[Serializable]
		public struct AnalogAction_t
		{
			// Token: 0x04000B84 RID: 2948
			public InputAnalogActionHandle_t actionHandle;

			// Token: 0x04000B85 RID: 2949
			public InputAnalogActionData_t analogActionData;
		}

		// Token: 0x020001FA RID: 506
		[Serializable]
		public struct DigitalAction_t
		{
			// Token: 0x04000B86 RID: 2950
			public InputDigitalActionHandle_t actionHandle;

			// Token: 0x04000B87 RID: 2951
			public InputDigitalActionData_t digitalActionData;
		}

		// Token: 0x020001FB RID: 507
		[Serializable]
		[StructLayout(LayoutKind.Explicit)]
		public struct OptionValue
		{
			// Token: 0x04000B88 RID: 2952
			[FieldOffset(0)]
			public SteamInputActionEvent_t.AnalogAction_t analogAction;

			// Token: 0x04000B89 RID: 2953
			[FieldOffset(0)]
			public SteamInputActionEvent_t.DigitalAction_t digitalAction;
		}
	}
}

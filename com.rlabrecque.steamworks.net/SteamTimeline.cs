using System;

namespace Steamworks
{
	// Token: 0x02000023 RID: 35
	public static class SteamTimeline
	{
		// Token: 0x060003DB RID: 987 RVA: 0x0000A2C8 File Offset: 0x000084C8
		public static void SetTimelineTooltip(string pchDescription, float flTimeDelta)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchDescription))
			{
				NativeMethods.ISteamTimeline_SetTimelineTooltip(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, flTimeDelta);
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0000A30C File Offset: 0x0000850C
		public static void ClearTimelineTooltip(float flTimeDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_ClearTimelineTooltip(CSteamAPIContext.GetSteamTimeline(), flTimeDelta);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0000A31E File Offset: 0x0000851E
		public static void SetTimelineGameMode(ETimelineGameMode eMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_SetTimelineGameMode(CSteamAPIContext.GetSteamTimeline(), eMode);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0000A330 File Offset: 0x00008530
		public static TimelineEventHandle_t AddInstantaneousTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unIconPriority, float flStartOffsetSeconds = 0f, ETimelineEventClipPriority ePossibleClip = ETimelineEventClipPriority.k_ETimelineEventClipPriority_None)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						result = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_AddInstantaneousTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unIconPriority, flStartOffsetSeconds, ePossibleClip);
					}
				}
			}
			return result;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0000A3B8 File Offset: 0x000085B8
		public static TimelineEventHandle_t AddRangeTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unIconPriority, float flStartOffsetSeconds = 0f, float flDuration = 0f, ETimelineEventClipPriority ePossibleClip = ETimelineEventClipPriority.k_ETimelineEventClipPriority_None)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						result = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_AddRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unIconPriority, flStartOffsetSeconds, flDuration, ePossibleClip);
					}
				}
			}
			return result;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000A444 File Offset: 0x00008644
		public static TimelineEventHandle_t StartRangeTimelineEvent(string pchTitle, string pchDescription, string pchIcon, uint unPriority, float flStartOffsetSeconds, ETimelineEventClipPriority ePossibleClip)
		{
			InteropHelp.TestIfAvailableClient();
			TimelineEventHandle_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						result = (TimelineEventHandle_t)NativeMethods.ISteamTimeline_StartRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority, flStartOffsetSeconds, ePossibleClip);
					}
				}
			}
			return result;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000A4CC File Offset: 0x000086CC
		public static void UpdateRangeTimelineEvent(TimelineEventHandle_t ulEvent, string pchTitle, string pchDescription, string pchIcon, uint unPriority, ETimelineEventClipPriority ePossibleClip)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTitle))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchDescription))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchIcon))
					{
						NativeMethods.ISteamTimeline_UpdateRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent, utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority, ePossibleClip);
					}
				}
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000A550 File Offset: 0x00008750
		public static void EndRangeTimelineEvent(TimelineEventHandle_t ulEvent, float flEndOffsetSeconds)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_EndRangeTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent, flEndOffsetSeconds);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000A563 File Offset: 0x00008763
		public static void RemoveTimelineEvent(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_RemoveTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000A575 File Offset: 0x00008775
		public static SteamAPICall_t DoesEventRecordingExist(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamTimeline_DoesEventRecordingExist(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000A58C File Offset: 0x0000878C
		public static void StartGamePhase()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_StartGamePhase(CSteamAPIContext.GetSteamTimeline());
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000A59D File Offset: 0x0000879D
		public static void EndGamePhase()
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_EndGamePhase(CSteamAPIContext.GetSteamTimeline());
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000A5B0 File Offset: 0x000087B0
		public static void SetGamePhaseID(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				NativeMethods.ISteamTimeline_SetGamePhaseID(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000A5F0 File Offset: 0x000087F0
		public static SteamAPICall_t DoesGamePhaseRecordingExist(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamTimeline_DoesGamePhaseRecordingExist(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
			return result;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000A638 File Offset: 0x00008838
		public static void AddGamePhaseTag(string pchTagName, string pchTagIcon, string pchTagGroup, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchTagName))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchTagIcon))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchTagGroup))
					{
						NativeMethods.ISteamTimeline_AddGamePhaseTag(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, utf8StringHandle3, unPriority);
					}
				}
			}
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000A6B8 File Offset: 0x000088B8
		public static void SetGamePhaseAttribute(string pchAttributeGroup, string pchAttributeValue, uint unPriority)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchAttributeGroup))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchAttributeValue))
				{
					NativeMethods.ISteamTimeline_SetGamePhaseAttribute(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle, utf8StringHandle2, unPriority);
				}
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0000A718 File Offset: 0x00008918
		public static void OpenOverlayToGamePhase(string pchPhaseID)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchPhaseID))
			{
				NativeMethods.ISteamTimeline_OpenOverlayToGamePhase(CSteamAPIContext.GetSteamTimeline(), utf8StringHandle);
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0000A758 File Offset: 0x00008958
		public static void OpenOverlayToTimelineEvent(TimelineEventHandle_t ulEvent)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamTimeline_OpenOverlayToTimelineEvent(CSteamAPIContext.GetSteamTimeline(), ulEvent);
		}
	}
}

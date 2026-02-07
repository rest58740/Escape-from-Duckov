using System;

namespace Steamworks
{
	// Token: 0x02000170 RID: 368
	public enum ESteamNetworkingConfigValue
	{
		// Token: 0x0400098E RID: 2446
		k_ESteamNetworkingConfig_Invalid,
		// Token: 0x0400098F RID: 2447
		k_ESteamNetworkingConfig_TimeoutInitial = 24,
		// Token: 0x04000990 RID: 2448
		k_ESteamNetworkingConfig_TimeoutConnected,
		// Token: 0x04000991 RID: 2449
		k_ESteamNetworkingConfig_SendBufferSize = 9,
		// Token: 0x04000992 RID: 2450
		k_ESteamNetworkingConfig_RecvBufferSize = 47,
		// Token: 0x04000993 RID: 2451
		k_ESteamNetworkingConfig_RecvBufferMessages,
		// Token: 0x04000994 RID: 2452
		k_ESteamNetworkingConfig_RecvMaxMessageSize,
		// Token: 0x04000995 RID: 2453
		k_ESteamNetworkingConfig_RecvMaxSegmentsPerPacket,
		// Token: 0x04000996 RID: 2454
		k_ESteamNetworkingConfig_ConnectionUserData = 40,
		// Token: 0x04000997 RID: 2455
		k_ESteamNetworkingConfig_SendRateMin = 10,
		// Token: 0x04000998 RID: 2456
		k_ESteamNetworkingConfig_SendRateMax,
		// Token: 0x04000999 RID: 2457
		k_ESteamNetworkingConfig_NagleTime,
		// Token: 0x0400099A RID: 2458
		k_ESteamNetworkingConfig_IP_AllowWithoutAuth = 23,
		// Token: 0x0400099B RID: 2459
		k_ESteamNetworkingConfig_IPLocalHost_AllowWithoutAuth = 52,
		// Token: 0x0400099C RID: 2460
		k_ESteamNetworkingConfig_MTU_PacketSize = 32,
		// Token: 0x0400099D RID: 2461
		k_ESteamNetworkingConfig_MTU_DataSize,
		// Token: 0x0400099E RID: 2462
		k_ESteamNetworkingConfig_Unencrypted,
		// Token: 0x0400099F RID: 2463
		k_ESteamNetworkingConfig_SymmetricConnect = 37,
		// Token: 0x040009A0 RID: 2464
		k_ESteamNetworkingConfig_LocalVirtualPort,
		// Token: 0x040009A1 RID: 2465
		k_ESteamNetworkingConfig_DualWifi_Enable,
		// Token: 0x040009A2 RID: 2466
		k_ESteamNetworkingConfig_EnableDiagnosticsUI = 46,
		// Token: 0x040009A3 RID: 2467
		k_ESteamNetworkingConfig_SendTimeSincePreviousPacket = 59,
		// Token: 0x040009A4 RID: 2468
		k_ESteamNetworkingConfig_FakePacketLoss_Send = 2,
		// Token: 0x040009A5 RID: 2469
		k_ESteamNetworkingConfig_FakePacketLoss_Recv,
		// Token: 0x040009A6 RID: 2470
		k_ESteamNetworkingConfig_FakePacketLag_Send,
		// Token: 0x040009A7 RID: 2471
		k_ESteamNetworkingConfig_FakePacketLag_Recv,
		// Token: 0x040009A8 RID: 2472
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Avg = 53,
		// Token: 0x040009A9 RID: 2473
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Max,
		// Token: 0x040009AA RID: 2474
		k_ESteamNetworkingConfig_FakePacketJitter_Send_Pct,
		// Token: 0x040009AB RID: 2475
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Avg,
		// Token: 0x040009AC RID: 2476
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Max,
		// Token: 0x040009AD RID: 2477
		k_ESteamNetworkingConfig_FakePacketJitter_Recv_Pct,
		// Token: 0x040009AE RID: 2478
		k_ESteamNetworkingConfig_FakePacketReorder_Send = 6,
		// Token: 0x040009AF RID: 2479
		k_ESteamNetworkingConfig_FakePacketReorder_Recv,
		// Token: 0x040009B0 RID: 2480
		k_ESteamNetworkingConfig_FakePacketReorder_Time,
		// Token: 0x040009B1 RID: 2481
		k_ESteamNetworkingConfig_FakePacketDup_Send = 26,
		// Token: 0x040009B2 RID: 2482
		k_ESteamNetworkingConfig_FakePacketDup_Recv,
		// Token: 0x040009B3 RID: 2483
		k_ESteamNetworkingConfig_FakePacketDup_TimeMax,
		// Token: 0x040009B4 RID: 2484
		k_ESteamNetworkingConfig_PacketTraceMaxBytes = 41,
		// Token: 0x040009B5 RID: 2485
		k_ESteamNetworkingConfig_FakeRateLimit_Send_Rate,
		// Token: 0x040009B6 RID: 2486
		k_ESteamNetworkingConfig_FakeRateLimit_Send_Burst,
		// Token: 0x040009B7 RID: 2487
		k_ESteamNetworkingConfig_FakeRateLimit_Recv_Rate,
		// Token: 0x040009B8 RID: 2488
		k_ESteamNetworkingConfig_FakeRateLimit_Recv_Burst,
		// Token: 0x040009B9 RID: 2489
		k_ESteamNetworkingConfig_OutOfOrderCorrectionWindowMicroseconds = 51,
		// Token: 0x040009BA RID: 2490
		k_ESteamNetworkingConfig_Callback_ConnectionStatusChanged = 201,
		// Token: 0x040009BB RID: 2491
		k_ESteamNetworkingConfig_Callback_AuthStatusChanged,
		// Token: 0x040009BC RID: 2492
		k_ESteamNetworkingConfig_Callback_RelayNetworkStatusChanged,
		// Token: 0x040009BD RID: 2493
		k_ESteamNetworkingConfig_Callback_MessagesSessionRequest,
		// Token: 0x040009BE RID: 2494
		k_ESteamNetworkingConfig_Callback_MessagesSessionFailed,
		// Token: 0x040009BF RID: 2495
		k_ESteamNetworkingConfig_Callback_CreateConnectionSignaling,
		// Token: 0x040009C0 RID: 2496
		k_ESteamNetworkingConfig_Callback_FakeIPResult,
		// Token: 0x040009C1 RID: 2497
		k_ESteamNetworkingConfig_P2P_STUN_ServerList = 103,
		// Token: 0x040009C2 RID: 2498
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Enable,
		// Token: 0x040009C3 RID: 2499
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Penalty,
		// Token: 0x040009C4 RID: 2500
		k_ESteamNetworkingConfig_P2P_Transport_SDR_Penalty,
		// Token: 0x040009C5 RID: 2501
		k_ESteamNetworkingConfig_P2P_TURN_ServerList,
		// Token: 0x040009C6 RID: 2502
		k_ESteamNetworkingConfig_P2P_TURN_UserList,
		// Token: 0x040009C7 RID: 2503
		k_ESteamNetworkingConfig_P2P_TURN_PassList,
		// Token: 0x040009C8 RID: 2504
		k_ESteamNetworkingConfig_P2P_Transport_ICE_Implementation,
		// Token: 0x040009C9 RID: 2505
		k_ESteamNetworkingConfig_SDRClient_ConsecutitivePingTimeoutsFailInitial = 19,
		// Token: 0x040009CA RID: 2506
		k_ESteamNetworkingConfig_SDRClient_ConsecutitivePingTimeoutsFail,
		// Token: 0x040009CB RID: 2507
		k_ESteamNetworkingConfig_SDRClient_MinPingsBeforePingAccurate,
		// Token: 0x040009CC RID: 2508
		k_ESteamNetworkingConfig_SDRClient_SingleSocket,
		// Token: 0x040009CD RID: 2509
		k_ESteamNetworkingConfig_SDRClient_ForceRelayCluster = 29,
		// Token: 0x040009CE RID: 2510
		k_ESteamNetworkingConfig_SDRClient_DevTicket,
		// Token: 0x040009CF RID: 2511
		k_ESteamNetworkingConfig_SDRClient_ForceProxyAddr,
		// Token: 0x040009D0 RID: 2512
		k_ESteamNetworkingConfig_SDRClient_FakeClusterPing = 36,
		// Token: 0x040009D1 RID: 2513
		k_ESteamNetworkingConfig_SDRClient_LimitPingProbesToNearestN = 60,
		// Token: 0x040009D2 RID: 2514
		k_ESteamNetworkingConfig_LogLevel_AckRTT = 13,
		// Token: 0x040009D3 RID: 2515
		k_ESteamNetworkingConfig_LogLevel_PacketDecode,
		// Token: 0x040009D4 RID: 2516
		k_ESteamNetworkingConfig_LogLevel_Message,
		// Token: 0x040009D5 RID: 2517
		k_ESteamNetworkingConfig_LogLevel_PacketGaps,
		// Token: 0x040009D6 RID: 2518
		k_ESteamNetworkingConfig_LogLevel_P2PRendezvous,
		// Token: 0x040009D7 RID: 2519
		k_ESteamNetworkingConfig_LogLevel_SDRRelayPings,
		// Token: 0x040009D8 RID: 2520
		k_ESteamNetworkingConfig_ECN = 999,
		// Token: 0x040009D9 RID: 2521
		k_ESteamNetworkingConfig_DELETED_EnumerateDevVars = 35,
		// Token: 0x040009DA RID: 2522
		k_ESteamNetworkingConfigValue__Force32Bit = 2147483647
	}
}

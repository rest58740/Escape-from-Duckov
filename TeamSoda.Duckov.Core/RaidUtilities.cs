using System;
using Duckov;
using Saves;
using UnityEngine;

// Token: 0x0200017A RID: 378
public static class RaidUtilities
{
	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06000BB7 RID: 2999 RVA: 0x00032238 File Offset: 0x00030438
	// (remove) Token: 0x06000BB8 RID: 3000 RVA: 0x0003226C File Offset: 0x0003046C
	public static event Action<RaidUtilities.RaidInfo> OnNewRaid;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06000BB9 RID: 3001 RVA: 0x000322A0 File Offset: 0x000304A0
	// (remove) Token: 0x06000BBA RID: 3002 RVA: 0x000322D4 File Offset: 0x000304D4
	public static event Action<RaidUtilities.RaidInfo> OnRaidDead;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06000BBB RID: 3003 RVA: 0x00032308 File Offset: 0x00030508
	// (remove) Token: 0x06000BBC RID: 3004 RVA: 0x0003233C File Offset: 0x0003053C
	public static event Action<RaidUtilities.RaidInfo> OnRaidEnd;

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x00032370 File Offset: 0x00030570
	// (set) Token: 0x06000BBE RID: 3006 RVA: 0x000323BC File Offset: 0x000305BC
	public static RaidUtilities.RaidInfo CurrentRaid
	{
		get
		{
			RaidUtilities.RaidInfo raidInfo = SavesSystem.Load<RaidUtilities.RaidInfo>("RaidInfo");
			raidInfo.totalTime = Time.unscaledTime - raidInfo.raidBeginTime;
			raidInfo.expOnEnd = EXPManager.EXP;
			raidInfo.expGained = raidInfo.expOnEnd - raidInfo.expOnBegan;
			return raidInfo;
		}
		private set
		{
			SavesSystem.Save<RaidUtilities.RaidInfo>("RaidInfo", value);
		}
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x000323CC File Offset: 0x000305CC
	public static void NewRaid()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		RaidUtilities.RaidInfo raidInfo = new RaidUtilities.RaidInfo
		{
			valid = true,
			ID = currentRaid.ID + 1U,
			dead = false,
			ended = false,
			raidBeginTime = Time.unscaledTime,
			raidEndTime = 0f,
			expOnBegan = EXPManager.EXP
		};
		RaidUtilities.CurrentRaid = raidInfo;
		Action<RaidUtilities.RaidInfo> onNewRaid = RaidUtilities.OnNewRaid;
		if (onNewRaid == null)
		{
			return;
		}
		onNewRaid(raidInfo);
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0003244C File Offset: 0x0003064C
	public static void NotifyDead()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.dead = true;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd != null)
		{
			onRaidEnd(currentRaid);
		}
		Action<RaidUtilities.RaidInfo> onRaidDead = RaidUtilities.OnRaidDead;
		if (onRaidDead == null)
		{
			return;
		}
		onRaidDead(currentRaid);
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x000324D8 File Offset: 0x000306D8
	public static void NotifyEnd()
	{
		RaidUtilities.RaidInfo currentRaid = RaidUtilities.CurrentRaid;
		currentRaid.ended = true;
		currentRaid.raidEndTime = Time.unscaledTime;
		currentRaid.totalTime = currentRaid.raidEndTime - currentRaid.raidBeginTime;
		currentRaid.expOnEnd = EXPManager.EXP;
		currentRaid.expGained = currentRaid.expOnEnd - currentRaid.expOnBegan;
		RaidUtilities.CurrentRaid = currentRaid;
		Action<RaidUtilities.RaidInfo> onRaidEnd = RaidUtilities.OnRaidEnd;
		if (onRaidEnd == null)
		{
			return;
		}
		onRaidEnd(currentRaid);
	}

	// Token: 0x04000A10 RID: 2576
	private const string SaveID = "RaidInfo";

	// Token: 0x020004D5 RID: 1237
	[Serializable]
	public struct RaidInfo
	{
		// Token: 0x04001D51 RID: 7505
		public bool valid;

		// Token: 0x04001D52 RID: 7506
		public uint ID;

		// Token: 0x04001D53 RID: 7507
		public bool dead;

		// Token: 0x04001D54 RID: 7508
		public bool ended;

		// Token: 0x04001D55 RID: 7509
		public float raidBeginTime;

		// Token: 0x04001D56 RID: 7510
		public float raidEndTime;

		// Token: 0x04001D57 RID: 7511
		public float totalTime;

		// Token: 0x04001D58 RID: 7512
		public long expOnBegan;

		// Token: 0x04001D59 RID: 7513
		public long expOnEnd;

		// Token: 0x04001D5A RID: 7514
		public long expGained;
	}
}

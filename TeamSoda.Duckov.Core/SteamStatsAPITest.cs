using System;
using Steamworks;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class SteamStatsAPITest : MonoBehaviour
{
	// Token: 0x06000EE6 RID: 3814 RVA: 0x0003C8F1 File Offset: 0x0003AAF1
	private void Awake()
	{
		this.onStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate(this.OnUserStatReceived));
		this.onStatsStoredCallback = Callback<UserStatsStored_t>.Create(new Callback<UserStatsStored_t>.DispatchDelegate(this.OnUserStatStored));
	}

	// Token: 0x06000EE7 RID: 3815 RVA: 0x0003C921 File Offset: 0x0003AB21
	private void OnUserStatStored(UserStatsStored_t param)
	{
		Debug.Log("Stat Stored!");
	}

	// Token: 0x06000EE8 RID: 3816 RVA: 0x0003C930 File Offset: 0x0003AB30
	private void OnUserStatReceived(UserStatsReceived_t param)
	{
		string str = "Stat Fetched:";
		CSteamID steamIDUser = param.m_steamIDUser;
		Debug.Log(str + steamIDUser.ToString() + " " + param.m_nGameID.ToString());
	}

	// Token: 0x06000EE9 RID: 3817 RVA: 0x0003C971 File Offset: 0x0003AB71
	private void Start()
	{
		SteamUserStats.RequestGlobalStats(60);
	}

	// Token: 0x06000EEA RID: 3818 RVA: 0x0003C97C File Offset: 0x0003AB7C
	private void Test()
	{
		int num;
		Debug.Log(SteamUserStats.GetStat("game_finished", ref num).ToString() + " " + num.ToString());
		bool flag = SteamUserStats.SetStat("game_finished", num + 1);
		Debug.Log(string.Format("Set: {0}", flag));
		SteamUserStats.StoreStats();
	}

	// Token: 0x06000EEB RID: 3819 RVA: 0x0003C9DC File Offset: 0x0003ABDC
	private void GetGlobalStat()
	{
		long num;
		if (SteamUserStats.GetGlobalStat("game_finished", ref num))
		{
			Debug.Log(string.Format("game finished: {0}", num));
			return;
		}
		Debug.Log("Failed");
	}

	// Token: 0x04000C69 RID: 3177
	private Callback<UserStatsReceived_t> onStatsReceivedCallback;

	// Token: 0x04000C6A RID: 3178
	private Callback<UserStatsStored_t> onStatsStoredCallback;
}

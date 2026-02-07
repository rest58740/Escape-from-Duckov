using System;
using Saves;
using UnityEngine;

namespace Duckov.Achievements
{
	// Token: 0x0200033A RID: 826
	public class StatisticsManager : MonoBehaviour
	{
		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06001BB0 RID: 7088 RVA: 0x00064B18 File Offset: 0x00062D18
		// (remove) Token: 0x06001BB1 RID: 7089 RVA: 0x00064B4C File Offset: 0x00062D4C
		public static event Action<string, long, long> OnStatisticsChanged;

		// Token: 0x06001BB2 RID: 7090 RVA: 0x00064B7F File Offset: 0x00062D7F
		private static string GetSaveKey(string statisticsKey)
		{
			return "Statistics/" + statisticsKey;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x00064B8C File Offset: 0x00062D8C
		private static long Get(string key)
		{
			StatisticsManager.GetSaveKey(key);
			if (!SavesSystem.KeyExisits(key))
			{
				return 0L;
			}
			return SavesSystem.Load<long>(key);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x00064BA8 File Offset: 0x00062DA8
		private static void Set(string key, long value)
		{
			long arg = StatisticsManager.Get(key);
			StatisticsManager.GetSaveKey(key);
			SavesSystem.Save<long>(key, value);
			Action<string, long, long> onStatisticsChanged = StatisticsManager.OnStatisticsChanged;
			if (onStatisticsChanged == null)
			{
				return;
			}
			onStatisticsChanged(key, arg, value);
		}

		// Token: 0x06001BB5 RID: 7093 RVA: 0x00064BDC File Offset: 0x00062DDC
		public static void Add(string key, long value = 1L)
		{
			long num = StatisticsManager.Get(key);
			checked
			{
				try
				{
					num += value;
				}
				catch (OverflowException ex)
				{
					Debug.LogException(ex);
					Debug.Log("Failed changing statistics of " + key + ". Overflow detected.");
					return;
				}
				StatisticsManager.Set(key, num);
			}
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x00064C2C File Offset: 0x00062E2C
		private void Awake()
		{
			this.RegisterEvents();
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x00064C34 File Offset: 0x00062E34
		private void OnDestroy()
		{
			this.UnregisterEvents();
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x00064C3C File Offset: 0x00062E3C
		private void RegisterEvents()
		{
		}

		// Token: 0x06001BB9 RID: 7097 RVA: 0x00064C3E File Offset: 0x00062E3E
		private void UnregisterEvents()
		{
		}
	}
}

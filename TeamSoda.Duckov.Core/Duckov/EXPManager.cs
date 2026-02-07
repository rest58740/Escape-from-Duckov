using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov.UI;
using Saves;
using SodaCraft.Localizations;
using SodaCraft.StringUtilities;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023E RID: 574
	public class EXPManager : MonoBehaviour, ISaveDataProvider
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600120C RID: 4620 RVA: 0x00046761 File Offset: 0x00044961
		public static EXPManager Instance
		{
			get
			{
				return EXPManager.instance;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00046768 File Offset: 0x00044968
		private string LevelChangeNotificationFormat
		{
			get
			{
				return LocalizationManager.ToPlainText(this.levelChangeNotificationFormatKey);
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600120E RID: 4622 RVA: 0x00046775 File Offset: 0x00044975
		// (set) Token: 0x0600120F RID: 4623 RVA: 0x00046794 File Offset: 0x00044994
		public static long EXP
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.point;
			}
			private set
			{
				if (EXPManager.instance == null)
				{
					return;
				}
				int level = EXPManager.Level;
				EXPManager.instance.point = value;
				Action<long> action = EXPManager.onExpChanged;
				if (action != null)
				{
					action(value);
				}
				int level2 = EXPManager.Level;
				if (level != level2)
				{
					EXPManager.OnLevelChanged(level, level2);
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001210 RID: 4624 RVA: 0x000467E2 File Offset: 0x000449E2
		public static int Level
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0;
				}
				return EXPManager.instance.LevelFromExp(EXPManager.EXP);
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001211 RID: 4625 RVA: 0x00046802 File Offset: 0x00044A02
		public static long CachedExp
		{
			get
			{
				if (EXPManager.instance == null)
				{
					return 0L;
				}
				return EXPManager.instance.cachedExp;
			}
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x0004681E File Offset: 0x00044A1E
		private static void OnLevelChanged(int oldLevel, int newLevel)
		{
			Action<int, int> action = EXPManager.onLevelChanged;
			if (action != null)
			{
				action(oldLevel, newLevel);
			}
			if (EXPManager.Instance == null)
			{
				return;
			}
			NotificationText.Push(StringExtensions.Format(EXPManager.Instance.LevelChangeNotificationFormat, new
			{
				level = newLevel
			}));
		}

		// Token: 0x06001213 RID: 4627 RVA: 0x0004685A File Offset: 0x00044A5A
		public static bool AddExp(int amount)
		{
			if (EXPManager.instance == null)
			{
				return false;
			}
			EXPManager.EXP += (long)amount;
			return true;
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00046879 File Offset: 0x00044A79
		private void CacheExp()
		{
			this.cachedExp = this.point;
		}

		// Token: 0x06001215 RID: 4629 RVA: 0x00046887 File Offset: 0x00044A87
		public object GenerateSaveData()
		{
			return this.point;
		}

		// Token: 0x06001216 RID: 4630 RVA: 0x00046894 File Offset: 0x00044A94
		public void SetupSaveData(object data)
		{
			if (data is long)
			{
				long num = (long)data;
				this.point = num;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001217 RID: 4631 RVA: 0x000468B7 File Offset: 0x00044AB7
		private string realKey
		{
			get
			{
				return "EXP_Value";
			}
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x000468C0 File Offset: 0x00044AC0
		private void Load()
		{
			if (SavesSystem.KeyExisits(this.realKey))
			{
				long num = SavesSystem.Load<long>(this.realKey);
				this.SetupSaveData(num);
			}
		}

		// Token: 0x06001219 RID: 4633 RVA: 0x000468F4 File Offset: 0x00044AF4
		private void Save()
		{
			object obj = this.GenerateSaveData();
			SavesSystem.Save<long>(this.realKey, (long)obj);
		}

		// Token: 0x0600121A RID: 4634 RVA: 0x0004691C File Offset: 0x00044B1C
		private void Awake()
		{
			if (EXPManager.instance == null)
			{
				EXPManager.instance = this;
			}
			else
			{
				Debug.LogWarning("检测到多个ExpManager");
			}
			SavesSystem.OnSetFile += this.Load;
			SavesSystem.OnCollectSaveData += this.Save;
			RaidUtilities.OnNewRaid += this.OnNewRaid;
		}

		// Token: 0x0600121B RID: 4635 RVA: 0x0004697B File Offset: 0x00044B7B
		private void Start()
		{
			this.Load();
			this.CacheExp();
		}

		// Token: 0x0600121C RID: 4636 RVA: 0x00046989 File Offset: 0x00044B89
		private void OnDestroy()
		{
			SavesSystem.OnSetFile -= this.Load;
			SavesSystem.OnCollectSaveData -= this.Save;
			RaidUtilities.OnNewRaid -= this.OnNewRaid;
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x000469BE File Offset: 0x00044BBE
		private void OnNewRaid(RaidUtilities.RaidInfo info)
		{
			this.CacheExp();
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x000469C8 File Offset: 0x00044BC8
		public int LevelFromExp(long exp)
		{
			for (int i = 0; i < this.levelExpDefinition.Count; i++)
			{
				long num = this.levelExpDefinition[i];
				if (exp < num)
				{
					return i - 1;
				}
			}
			return this.levelExpDefinition.Count - 1;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x00046A10 File Offset: 0x00044C10
		[return: TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		public ValueTuple<long, long> GetLevelExpRange(int level)
		{
			int num = this.levelExpDefinition.Count - 1;
			if (level >= num)
			{
				List<long> list = this.levelExpDefinition;
				return new ValueTuple<long, long>(list[list.Count - 1], long.MaxValue);
			}
			long item = this.levelExpDefinition[level];
			long item2 = this.levelExpDefinition[level + 1];
			return new ValueTuple<long, long>(item, item2);
		}

		// Token: 0x04000E04 RID: 3588
		private static EXPManager instance;

		// Token: 0x04000E05 RID: 3589
		[SerializeField]
		private string levelChangeNotificationFormatKey = "UI_LevelChangeNotification";

		// Token: 0x04000E06 RID: 3590
		[SerializeField]
		private List<long> levelExpDefinition;

		// Token: 0x04000E07 RID: 3591
		[SerializeField]
		private long point;

		// Token: 0x04000E08 RID: 3592
		public static Action<long> onExpChanged;

		// Token: 0x04000E09 RID: 3593
		public static Action<int, int> onLevelChanged;

		// Token: 0x04000E0A RID: 3594
		private long cachedExp;

		// Token: 0x04000E0B RID: 3595
		private const string prefixKey = "EXP";

		// Token: 0x04000E0C RID: 3596
		private const string key = "Value";
	}
}

using System;
using Duckov.Achievements;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Duckov
{
	// Token: 0x02000248 RID: 584
	public class SocialManager : MonoBehaviour
	{
		// Token: 0x0600125C RID: 4700 RVA: 0x0004726E File Offset: 0x0004546E
		private void Awake()
		{
			AchievementManager.OnAchievementUnlocked += this.UnlockAchievement;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00047281 File Offset: 0x00045481
		private void Start()
		{
			Social.localUser.Authenticate(new Action<bool>(this.ProcessAuthentication));
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00047299 File Offset: 0x00045499
		private void ProcessAuthentication(bool success)
		{
			if (success)
			{
				this.initialized = true;
				Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
			}
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x000472B6 File Offset: 0x000454B6
		private void ProcessLoadedAchievements(IAchievement[] loadedAchievements)
		{
			this._achievement_cache = loadedAchievements;
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x000472BF File Offset: 0x000454BF
		private void UnlockAchievement(string id)
		{
			if (this.initialized)
			{
				return;
			}
			Social.ReportProgress(id, 100.0, new Action<bool>(this.OnReportProgressResult));
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000472E5 File Offset: 0x000454E5
		private void OnReportProgressResult(bool success)
		{
			Social.LoadAchievements(new Action<IAchievement[]>(this.ProcessLoadedAchievements));
		}

		// Token: 0x04000E2B RID: 3627
		private bool initialized;

		// Token: 0x04000E2C RID: 3628
		private IAchievement[] _achievement_cache;
	}
}

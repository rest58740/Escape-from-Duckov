using System;
using System.Collections;
using UnityEngine;
using WyvrnSDK;

namespace Duckov
{
	// Token: 0x02000241 RID: 577
	public class WyvrnManager : MonoBehaviour
	{
		// Token: 0x1700032B RID: 811
		// (get) Token: 0x0600123F RID: 4671 RVA: 0x00046FD1 File Offset: 0x000451D1
		// (set) Token: 0x06001240 RID: 4672 RVA: 0x00046FD8 File Offset: 0x000451D8
		public static WyvrnManager Instance { get; private set; }

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001241 RID: 4673 RVA: 0x00046FE0 File Offset: 0x000451E0
		public static bool Initialized
		{
			get
			{
				return !(WyvrnManager.Instance == null) && WyvrnManager.Instance.initialized;
			}
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x00046FFB File Offset: 0x000451FB
		private void Awake()
		{
			if (WyvrnManager.Instance != null)
			{
				Debug.LogError("Multiple wyvrn managers detected");
			}
			WyvrnManager.Instance = this;
			HardwareSyncingManager.OnSetEvent += this.OnHardwareSyncingManagerSetEvent;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x0004702C File Offset: 0x0004522C
		private void OnDestroy()
		{
			HardwareSyncingManager.OnSetEvent -= this.OnHardwareSyncingManagerSetEvent;
			if (!WyvrnManager.Initialized)
			{
				return;
			}
			int num = WyvrnAPI.CoreUnInit();
			if (num != 0)
			{
				Debug.LogError(string.Format("[WYVRN] Failed uninitializing wyvrn api. code: {0}", num));
			}
			this.initialized = false;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00047077 File Offset: 0x00045277
		private void OnHardwareSyncingManagerSetEvent(string eventName)
		{
			this.SetEvent(eventName);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x00047080 File Offset: 0x00045280
		public IEnumerator Start()
		{
			if (!WyvrnAPI.IsWyvrnSDKAvailable())
			{
				this._mResult = 6023;
				yield break;
			}
			APPINFOTYPE appinfotype = default(APPINFOTYPE);
			appinfotype.Title = "Escape From Duckov";
			appinfotype.Description = "Escape From Duckov";
			appinfotype.Author_Name = "Team Soda";
			appinfotype.Author_Contact = "https://game.bilibili.com/duckov/";
			appinfotype.Category = 2U;
			this._mResult = WyvrnAPI.CoreInitSDK(ref appinfotype);
			if (this._mResult == 0)
			{
				this.initialized = true;
			}
			yield break;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00047090 File Offset: 0x00045290
		public void SetEvent(string eventName)
		{
			if (!WyvrnManager.Initialized)
			{
				return;
			}
			int num = WyvrnAPI.CoreSetEventName(eventName);
			if (num != 0)
			{
				Debug.LogError(string.Format("[WYVRN] Failed setting event in wyvrn api. code: {0}", num));
			}
		}

		// Token: 0x04000E13 RID: 3603
		private bool initialized;

		// Token: 0x04000E14 RID: 3604
		private int _mResult;
	}
}

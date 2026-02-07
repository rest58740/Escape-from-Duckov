using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Sirenix.Utilities;
using Steamworks;
using UnityEngine;

namespace Duckov.Modding
{
	// Token: 0x0200027A RID: 634
	public class SteamWorkshopManager : MonoBehaviour
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x0600140B RID: 5131 RVA: 0x0004B57C File Offset: 0x0004977C
		// (set) Token: 0x0600140C RID: 5132 RVA: 0x0004B583 File Offset: 0x00049783
		public static SteamWorkshopManager Instance { get; private set; }

		// Token: 0x0600140D RID: 5133 RVA: 0x0004B58B File Offset: 0x0004978B
		private void Awake()
		{
			SteamWorkshopManager.Instance = this;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0004B593 File Offset: 0x00049793
		private void OnEnable()
		{
			ModManager.Rescan();
			this.SendQueryDetailsRequest();
			ModManager.OnScan += this.OnScanMods;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0004B5B1 File Offset: 0x000497B1
		private void OnDisable()
		{
			ModManager.OnScan -= this.OnScanMods;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0004B5C4 File Offset: 0x000497C4
		private void OnScanMods(List<ModInfo> list)
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				PublishedFileId_t nPublishedFileId = steamUGCDetails_t.m_nPublishedFileId;
				EItemState itemState = SteamUGC.GetItemState(nPublishedFileId);
				ulong num;
				string text;
				uint num2;
				if ((itemState | 4) == itemState && SteamUGC.GetItemInstallInfo(nPublishedFileId, ref num, ref text, 1024U, ref num2))
				{
					ModInfo item;
					if (!ModManager.TryProcessModFolder(text, out item, true, nPublishedFileId.m_PublishedFileId))
					{
						Debug.LogError("Mod processing failed! \nPath:" + text);
					}
					else
					{
						list.Add(item);
					}
				}
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0004B66C File Offset: 0x0004986C
		public void SendQueryDetailsRequest()
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			if (this.CRSteamUGCQueryCompleted == null)
			{
				this.CRSteamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.OnSteamUGCQueryCompleted));
			}
			HashSet<PublishedFileId_t> hashSet = new HashSet<PublishedFileId_t>();
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			LinqExtensions.AddRange<PublishedFileId_t>(hashSet, array);
			foreach (ModInfo modInfo in ModManager.modInfos)
			{
				if (modInfo.publishedFileId != 0UL)
				{
					hashSet.Add((PublishedFileId_t)modInfo.publishedFileId);
				}
			}
			SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(SteamUGC.CreateQueryUGCDetailsRequest(hashSet.ToArray<PublishedFileId_t>(), (uint)hashSet.Count));
			this.CRSteamUGCQueryCompleted.Set(steamAPICall_t, null);
			new StringBuilder();
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004B748 File Offset: 0x00049948
		private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t completed, bool bIOFailure)
		{
			if (bIOFailure)
			{
				Debug.LogError("Steam UGC Query failed", base.gameObject);
				ModManager.Instance.ScanAndActivateMods();
				return;
			}
			UGCQueryHandle_t handle = completed.m_handle;
			uint unNumResultsReturned = completed.m_unNumResultsReturned;
			for (uint num = 0U; num < unNumResultsReturned; num += 1U)
			{
				SteamUGCDetails_t item;
				SteamUGC.GetQueryUGCResult(handle, num, ref item);
				SteamWorkshopManager.ugcDetailsCache.Add(item);
			}
			SteamUGC.ReleaseQueryUGCRequest(handle);
			ModManager.Instance.ScanAndActivateMods();
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004B7B4 File Offset: 0x000499B4
		public UniTask<PublishedFileId_t> RequestNewWorkshopItemID()
		{
			SteamWorkshopManager.<RequestNewWorkshopItemID>d__14 <RequestNewWorkshopItemID>d__;
			<RequestNewWorkshopItemID>d__.<>t__builder = AsyncUniTaskMethodBuilder<PublishedFileId_t>.Create();
			<RequestNewWorkshopItemID>d__.<>4__this = this;
			<RequestNewWorkshopItemID>d__.<>1__state = -1;
			<RequestNewWorkshopItemID>d__.<>t__builder.Start<SteamWorkshopManager.<RequestNewWorkshopItemID>d__14>(ref <RequestNewWorkshopItemID>d__);
			return <RequestNewWorkshopItemID>d__.<>t__builder.Task;
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x0004B7F7 File Offset: 0x000499F7
		private void OnCreateItemResult(CreateItemResult_t result, bool bIOFailure)
		{
			Debug.Log("Creat Item Result Fired A");
			this.createItemResultFired = true;
			this.createItemResult = result;
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0004B811 File Offset: 0x00049A11
		// (set) Token: 0x06001416 RID: 5142 RVA: 0x0004B818 File Offset: 0x00049A18
		public static ulong punBytesProcess { get; private set; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0004B820 File Offset: 0x00049A20
		// (set) Token: 0x06001418 RID: 5144 RVA: 0x0004B827 File Offset: 0x00049A27
		public static ulong punBytesTotal { get; private set; }

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0004B82F File Offset: 0x00049A2F
		public static float UploadingProgress
		{
			get
			{
				return (float)(SteamWorkshopManager.punBytesProcess / SteamWorkshopManager.punBytesTotal);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004B841 File Offset: 0x00049A41
		// (set) Token: 0x0600141B RID: 5147 RVA: 0x0004B849 File Offset: 0x00049A49
		public bool UploadSucceed { get; private set; }

		// Token: 0x0600141C RID: 5148 RVA: 0x0004B854 File Offset: 0x00049A54
		public UniTask<bool> UploadWorkshopItem(string path, string changeNote = "Unknown")
		{
			SteamWorkshopManager.<UploadWorkshopItem>d__32 <UploadWorkshopItem>d__;
			<UploadWorkshopItem>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<UploadWorkshopItem>d__.<>4__this = this;
			<UploadWorkshopItem>d__.path = path;
			<UploadWorkshopItem>d__.changeNote = changeNote;
			<UploadWorkshopItem>d__.<>1__state = -1;
			<UploadWorkshopItem>d__.<>t__builder.Start<SteamWorkshopManager.<UploadWorkshopItem>d__32>(ref <UploadWorkshopItem>d__);
			return <UploadWorkshopItem>d__.<>t__builder.Task;
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004B8A8 File Offset: 0x00049AA8
		public static bool IsOwner(ModInfo info)
		{
			if (!SteamManager.Initialized)
			{
				return false;
			}
			if (info.publishedFileId == 0UL)
			{
				return false;
			}
			foreach (SteamUGCDetails_t steamUGCDetails_t in SteamWorkshopManager.ugcDetailsCache)
			{
				if (steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId == info.publishedFileId)
				{
					return steamUGCDetails_t.m_ulSteamIDOwner == SteamUser.GetSteamID().m_SteamID;
				}
			}
			return false;
		}

		// Token: 0x04000EEA RID: 3818
		private CallResult<SteamUGCQueryCompleted_t> CRSteamUGCQueryCompleted;

		// Token: 0x04000EEB RID: 3819
		private CallResult<CreateItemResult_t> CRCreateItemResult;

		// Token: 0x04000EEC RID: 3820
		private UGCQueryHandle_t activeQueryHandle;

		// Token: 0x04000EED RID: 3821
		private static List<SteamUGCDetails_t> ugcDetailsCache = new List<SteamUGCDetails_t>();

		// Token: 0x04000EEE RID: 3822
		private bool createItemResultFired;

		// Token: 0x04000EEF RID: 3823
		private CreateItemResult_t createItemResult;
	}
}

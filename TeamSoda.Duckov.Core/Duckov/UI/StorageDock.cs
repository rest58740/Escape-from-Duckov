using System;
using System.Collections.Generic;
using Duckov.UI.Animations;
using Duckov.Utilities;
using ItemStatsSystem.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Duckov.UI
{
	// Token: 0x020003CA RID: 970
	public class StorageDock : View
	{
		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060022F0 RID: 8944 RVA: 0x0007A359 File Offset: 0x00078559
		public static StorageDock Instance
		{
			get
			{
				return View.GetViewInstance<StorageDock>();
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060022F1 RID: 8945 RVA: 0x0007A360 File Offset: 0x00078560
		private int TotalItemCount
		{
			get
			{
				if (PlayerStorage.IncomingItemBuffer == null)
				{
					return 0;
				}
				return PlayerStorage.IncomingItemBuffer.Count;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060022F2 RID: 8946 RVA: 0x0007A375 File Offset: 0x00078575
		private int MaxPage
		{
			get
			{
				return this.TotalItemCount / 24;
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060022F3 RID: 8947 RVA: 0x0007A380 File Offset: 0x00078580
		private PrefabPool<StorageDockEntry> EntryPool
		{
			get
			{
				if (this._entryPool == null)
				{
					this._entryPool = new PrefabPool<StorageDockEntry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
				}
				return this._entryPool;
			}
		}

		// Token: 0x060022F4 RID: 8948 RVA: 0x0007A3BC File Offset: 0x000785BC
		protected override void Awake()
		{
			base.Awake();
			this.entryTemplate.gameObject.SetActive(false);
			this.btnNextPage.onClick.AddListener(new UnityAction(this.NextPage));
			this.btnPrevPage.onClick.AddListener(new UnityAction(this.PrevPage));
		}

		// Token: 0x060022F5 RID: 8949 RVA: 0x0007A418 File Offset: 0x00078618
		private void OnEnable()
		{
			PlayerStorage.OnTakeBufferItem += this.OnTakeBufferItem;
		}

		// Token: 0x060022F6 RID: 8950 RVA: 0x0007A42B File Offset: 0x0007862B
		private void OnDisable()
		{
			PlayerStorage.OnTakeBufferItem -= this.OnTakeBufferItem;
		}

		// Token: 0x060022F7 RID: 8951 RVA: 0x0007A43E File Offset: 0x0007863E
		private void OnTakeBufferItem()
		{
			this.Refresh();
		}

		// Token: 0x060022F8 RID: 8952 RVA: 0x0007A446 File Offset: 0x00078646
		protected override void OnOpen()
		{
			base.OnOpen();
			if (PlayerStorage.Instance == null)
			{
				base.Close();
				return;
			}
			this.fadeGroup.Show();
			this.Setup();
		}

		// Token: 0x060022F9 RID: 8953 RVA: 0x0007A473 File Offset: 0x00078673
		protected override void OnClose()
		{
			base.OnClose();
			this.fadeGroup.Hide();
		}

		// Token: 0x060022FA RID: 8954 RVA: 0x0007A486 File Offset: 0x00078686
		private void Setup()
		{
			this.Refresh();
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0007A490 File Offset: 0x00078690
		private void Refresh()
		{
			this.EntryPool.ReleaseAll();
			List<ItemTreeData> incomingItemBuffer = PlayerStorage.IncomingItemBuffer;
			int num = this.page * 24;
			int num2 = num + 24;
			if (num2 > incomingItemBuffer.Count)
			{
				num2 = incomingItemBuffer.Count;
			}
			for (int i = num; i < num2; i++)
			{
				ItemTreeData itemTreeData = incomingItemBuffer[i];
				if (itemTreeData != null)
				{
					this.EntryPool.Get(null).Setup(i, itemTreeData);
				}
			}
			this.placeHolder.gameObject.SetActive(incomingItemBuffer.Count <= 0);
			this.pageText.text = string.Format("{0}/{1}", this.page + 1, this.MaxPage + 1);
		}

		// Token: 0x060022FC RID: 8956 RVA: 0x0007A540 File Offset: 0x00078740
		public void NextPage()
		{
			this.GotoPage(this.page + 1);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0007A550 File Offset: 0x00078750
		public void PrevPage()
		{
			this.GotoPage(this.page - 1);
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0007A560 File Offset: 0x00078760
		public void GotoPage(int page)
		{
			if (page < 0)
			{
				page = 0;
			}
			if (page > this.MaxPage)
			{
				page = this.MaxPage;
			}
			this.page = page;
			this.Refresh();
		}

		// Token: 0x060022FF RID: 8959 RVA: 0x0007A587 File Offset: 0x00078787
		internal static void Show()
		{
			if (StorageDock.Instance == null)
			{
				return;
			}
			StorageDock.Instance.Open(null);
		}

		// Token: 0x0400179A RID: 6042
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x0400179B RID: 6043
		[SerializeField]
		private StorageDockEntry entryTemplate;

		// Token: 0x0400179C RID: 6044
		[SerializeField]
		private GameObject placeHolder;

		// Token: 0x0400179D RID: 6045
		[SerializeField]
		private TextMeshProUGUI pageText;

		// Token: 0x0400179E RID: 6046
		[SerializeField]
		private Button btnNextPage;

		// Token: 0x0400179F RID: 6047
		[SerializeField]
		private Button btnPrevPage;

		// Token: 0x040017A0 RID: 6048
		private const int itemsPerPage = 24;

		// Token: 0x040017A1 RID: 6049
		private int page;

		// Token: 0x040017A2 RID: 6050
		private PrefabPool<StorageDockEntry> _entryPool;
	}
}

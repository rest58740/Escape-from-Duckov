using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Duckov.UI.Animations;
using Duckov.Utilities;
using UnityEngine;

namespace Duckov.UI
{
	// Token: 0x020003C5 RID: 965
	public class GameplayUIManager : MonoBehaviour
	{
		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x00079613 File Offset: 0x00077813
		public static GameplayUIManager Instance
		{
			get
			{
				return GameplayUIManager.instance;
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0007961A File Offset: 0x0007781A
		public View ActiveView
		{
			get
			{
				return View.ActiveView;
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x00079624 File Offset: 0x00077824
		public static T GetViewInstance<T>() where T : View
		{
			if (GameplayUIManager.Instance == null)
			{
				return default(T);
			}
			View view;
			if (GameplayUIManager.Instance.viewDic.TryGetValue(typeof(T), out view))
			{
				return view as T;
			}
			View view2 = GameplayUIManager.Instance.views.Find((View e) => e is T);
			if (view2 == null)
			{
				return default(T);
			}
			GameplayUIManager.Instance.viewDic[typeof(T)] = view2;
			return view2 as T;
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000796D8 File Offset: 0x000778D8
		private void Awake()
		{
			if (GameplayUIManager.instance == null)
			{
				GameplayUIManager.instance = this;
			}
			else
			{
				Debug.LogWarning("Duplicate Gameplay UI Manager detected!");
			}
			foreach (View view in this.views)
			{
				view.gameObject.SetActive(true);
			}
			foreach (GameObject gameObject in this.setActiveOnAwake)
			{
				if (!(gameObject == null))
				{
					gameObject.gameObject.SetActive(true);
				}
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000797A0 File Offset: 0x000779A0
		public PrefabPool<ItemDisplay> ItemDisplayPool
		{
			get
			{
				if (this.itemDisplayPool == null)
				{
					this.itemDisplayPool = new PrefabPool<ItemDisplay>(GameplayDataSettings.UIPrefabs.ItemDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.itemDisplayPool;
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000797E4 File Offset: 0x000779E4
		public PrefabPool<SlotDisplay> SlotDisplayPool
		{
			get
			{
				if (this.slotDisplayPool == null)
				{
					this.slotDisplayPool = new PrefabPool<SlotDisplay>(GameplayDataSettings.UIPrefabs.SlotDisplay, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.slotDisplayPool;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x00079828 File Offset: 0x00077A28
		public PrefabPool<InventoryEntry> InventoryEntryPool
		{
			get
			{
				if (this.inventoryEntryPool == null)
				{
					this.inventoryEntryPool = new PrefabPool<InventoryEntry>(GameplayDataSettings.UIPrefabs.InventoryEntry, base.transform, null, null, null, true, 10, 10000, null);
				}
				return this.inventoryEntryPool;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0007986A File Offset: 0x00077A6A
		public SplitDialogue SplitDialogue
		{
			get
			{
				return this._splitDialogue;
			}
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x00079872 File Offset: 0x00077A72
		public static UniTask TemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = false;
			return GameplayUIManager.Instance.fadeGroup.HideAndReturnTask();
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000798A6 File Offset: 0x00077AA6
		public static UniTask ReverseTemporaryHide()
		{
			if (GameplayUIManager.Instance == null)
			{
				return UniTask.CompletedTask;
			}
			GameplayUIManager.Instance.canvasGroup.blocksRaycasts = true;
			return GameplayUIManager.Instance.fadeGroup.ShowAndReturnTask();
		}

		// Token: 0x04001776 RID: 6006
		private static GameplayUIManager instance;

		// Token: 0x04001777 RID: 6007
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04001778 RID: 6008
		[SerializeField]
		private FadeGroup fadeGroup;

		// Token: 0x04001779 RID: 6009
		[SerializeField]
		private List<View> views = new List<View>();

		// Token: 0x0400177A RID: 6010
		[SerializeField]
		private List<GameObject> setActiveOnAwake;

		// Token: 0x0400177B RID: 6011
		private Dictionary<Type, View> viewDic = new Dictionary<Type, View>();

		// Token: 0x0400177C RID: 6012
		private PrefabPool<ItemDisplay> itemDisplayPool;

		// Token: 0x0400177D RID: 6013
		private PrefabPool<SlotDisplay> slotDisplayPool;

		// Token: 0x0400177E RID: 6014
		private PrefabPool<InventoryEntry> inventoryEntryPool;

		// Token: 0x0400177F RID: 6015
		[SerializeField]
		private SplitDialogue _splitDialogue;
	}
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Duckov;
using Duckov.MasterKeys;
using Duckov.Scenes;
using Duckov.Utilities;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using SodaCraft.Localizations;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000DC RID: 220
public class InteractableBase : MonoBehaviour, IProgress
{
	// Token: 0x060006EE RID: 1774 RVA: 0x0001F56C File Offset: 0x0001D76C
	public List<InteractableBase> GetInteractableList()
	{
		this._interactbleList.Clear();
		this._interactbleList.Add(this);
		if (!this.interactableGroup || this.otherInterablesInGroup.Count <= 0)
		{
			return this._interactbleList;
		}
		foreach (InteractableBase interactableBase in this.otherInterablesInGroup)
		{
			if (!(interactableBase == null) && interactableBase.gameObject.activeInHierarchy)
			{
				this._interactbleList.Add(interactableBase);
			}
		}
		return this._interactbleList;
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001F614 File Offset: 0x0001D814
	public float InteractTime
	{
		get
		{
			if (this.requireItem && !this.requireItemUsed)
			{
				return this.interactTime + this.unlockTime;
			}
			return this.interactTime;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0001F63A File Offset: 0x0001D83A
	// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001F65B File Offset: 0x0001D85B
	public string InteractName
	{
		get
		{
			if (this.overrideInteractName)
			{
				return LocalizationManager.ToPlainText(this._overrideInteractNameKey);
			}
			return LocalizationManager.ToPlainText(this.defaultInteractNameKey);
		}
		set
		{
			this.overrideInteractName = true;
			this._overrideInteractNameKey = value;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0001F66B File Offset: 0x0001D86B
	private bool ShowBaseInteractName
	{
		get
		{
			return this.overrideInteractName && this.ShowBaseInteractNameInspector;
		}
	}

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x060006F3 RID: 1779 RVA: 0x0001F67D File Offset: 0x0001D87D
	protected virtual bool ShowBaseInteractNameInspector
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700014A RID: 330
	// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0001F680 File Offset: 0x0001D880
	private ItemMetaData CachedMeta
	{
		get
		{
			if (this._cachedMeta == null)
			{
				this._cachedMeta = new ItemMetaData?(ItemAssetsCollection.GetMetaData(this.requireItemId));
			}
			return this._cachedMeta.Value;
		}
	}

	// Token: 0x1400002D RID: 45
	// (add) Token: 0x060006F5 RID: 1781 RVA: 0x0001F6B0 File Offset: 0x0001D8B0
	// (remove) Token: 0x060006F6 RID: 1782 RVA: 0x0001F6E4 File Offset: 0x0001D8E4
	public static event Action<InteractableBase> OnInteractStartStaticEvent;

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0001F717 File Offset: 0x0001D917
	protected virtual bool ShowUnityEvents
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0001F71A File Offset: 0x0001D91A
	public bool Interacting
	{
		get
		{
			return this.interactCharacter != null;
		}
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0001F728 File Offset: 0x0001D928
	// (set) Token: 0x060006FA RID: 1786 RVA: 0x0001F730 File Offset: 0x0001D930
	public bool MarkerActive
	{
		get
		{
			return this.interactMarkerVisible;
		}
		set
		{
			if (!base.enabled)
			{
				return;
			}
			this.interactMarkerVisible = value;
			if (value)
			{
				this.ActiveMarker();
				return;
			}
			if (this.markerObject)
			{
				this.markerObject.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0001F76C File Offset: 0x0001D96C
	protected virtual void Awake()
	{
		this.requireItemDataKeyCached = this.GetKey();
		if (this.interactCollider == null)
		{
			this.interactCollider = base.GetComponent<Collider>();
			if (this.interactCollider == null)
			{
				this.interactCollider = base.gameObject.AddComponent<BoxCollider>();
				this.interactCollider.enabled = false;
			}
		}
		if (this.interactCollider != null)
		{
			this.interactCollider.gameObject.layer = LayerMask.NameToLayer("Interactable");
		}
		foreach (InteractableBase interactableBase in this.otherInterablesInGroup)
		{
			if (interactableBase)
			{
				interactableBase.MarkerActive = false;
				interactableBase.transform.position = base.transform.position;
				interactableBase.transform.rotation = base.transform.rotation;
				interactableBase.interactMarkerOffset = this.interactMarkerOffset;
			}
		}
		this._interactbleList = new List<InteractableBase>();
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0001F884 File Offset: 0x0001DA84
	protected virtual void Start()
	{
		object obj;
		if (this.requireItem && MultiSceneCore.Instance && MultiSceneCore.Instance.inLevelData.TryGetValue(this.requireItemDataKeyCached, out obj) && obj is bool)
		{
			bool flag = (bool)obj;
			if (flag)
			{
				this.requireItem = false;
				this.requireItemUsed = true;
				UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
				if (onRequiredItemUsedEvent != null)
				{
					onRequiredItemUsedEvent.Invoke();
				}
			}
		}
		this.MarkerActive = this.interactMarkerVisible;
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0001F8FC File Offset: 0x0001DAFC
	private void ActiveMarker()
	{
		if (this.markerObject)
		{
			if (!this.markerObject.gameObject.activeInHierarchy)
			{
				this.markerObject.gameObject.SetActive(true);
			}
			return;
		}
		this.markerObject = Object.Instantiate<InteractMarker>(GameplayDataSettings.Prefabs.InteractMarker, base.transform);
		this.markerObject.transform.localPosition = this.interactMarkerOffset;
		this.CheckInteractable();
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0001F972 File Offset: 0x0001DB72
	public void SetMarkerUsed()
	{
		if (!this.markerObject)
		{
			return;
		}
		this.markerObject.MarkAsUsed();
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0001F990 File Offset: 0x0001DB90
	public bool StartInteract(CharacterMainControl _interactCharacter)
	{
		if (!_interactCharacter)
		{
			return false;
		}
		if (this.requireItem && !this.TryGetRequiredItem(_interactCharacter).Item1)
		{
			return false;
		}
		if (this.interactCharacter == _interactCharacter)
		{
			return false;
		}
		if (!this.CheckInteractable())
		{
			return false;
		}
		if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnStartInteract && !this.UseRequiredItem(_interactCharacter))
		{
			this.StopInteract();
			return false;
		}
		this.interactCharacter = _interactCharacter;
		this.interactTimer = 0f;
		this.timeOut = false;
		UnityEvent<CharacterMainControl, InteractableBase> onInteractStartEvent = this.OnInteractStartEvent;
		if (onInteractStartEvent != null)
		{
			onInteractStartEvent.Invoke(_interactCharacter, this);
		}
		Action<InteractableBase> onInteractStartStaticEvent = InteractableBase.OnInteractStartStaticEvent;
		if (onInteractStartStaticEvent != null)
		{
			onInteractStartStaticEvent(this);
		}
		try
		{
			this.OnInteractStart(_interactCharacter);
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			if (CharacterMainControl.Main)
			{
				CharacterMainControl.Main.PopText("OnInteractStart开始失败，Log Error", -1f);
			}
			return false;
		}
		return true;
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0001FA80 File Offset: 0x0001DC80
	public InteractableBase GetInteractableInGroup(int index)
	{
		if (index == 0)
		{
			return this;
		}
		List<InteractableBase> interactableList = this.GetInteractableList();
		if (index >= interactableList.Count)
		{
			return null;
		}
		return interactableList[index];
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0001FAAB File Offset: 0x0001DCAB
	public void InternalStopInteract()
	{
		this.interactCharacter = null;
		this.lastStopTime = Time.time;
		this.OnInteractStop();
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0001FAC8 File Offset: 0x0001DCC8
	public void StopInteract()
	{
		CharacterMainControl characterMainControl = this.interactCharacter;
		if (characterMainControl && characterMainControl.interactAction.Running && characterMainControl.interactAction.InteractingTarget == this)
		{
			this.interactCharacter.interactAction.StopAction();
			return;
		}
		this.InternalStopInteract();
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0001FB1C File Offset: 0x0001DD1C
	public void UpdateInteract(CharacterMainControl _interactCharacter, float deltaTime)
	{
		this.interactTimer += deltaTime;
		this.OnUpdate(_interactCharacter, deltaTime);
		if (!this.timeOut && this.interactTimer >= this.InteractTime)
		{
			if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnTimeOut && !this.UseRequiredItem(_interactCharacter))
			{
				this.StopInteract();
				return;
			}
			if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.None && !this.requireItemUsed)
			{
				this.requireItemUsed = true;
				UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
				if (onRequiredItemUsedEvent != null)
				{
					onRequiredItemUsedEvent.Invoke();
				}
				if (MultiSceneCore.Instance)
				{
					MultiSceneCore.Instance.inLevelData[this.requireItemDataKeyCached] = true;
					Debug.Log("设置使用过物品为true");
				}
			}
			this.timeOut = true;
			this.OnTimeOut();
			UnityEvent<CharacterMainControl, InteractableBase> onInteractTimeoutEvent = this.OnInteractTimeoutEvent;
			if (onInteractTimeoutEvent != null)
			{
				onInteractTimeoutEvent.Invoke(_interactCharacter, this);
			}
			if (this.finishWhenTimeOut)
			{
				this.FinishInteract(_interactCharacter);
			}
		}
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0001FC0C File Offset: 0x0001DE0C
	public void FinishInteract(CharacterMainControl _interactCharacter)
	{
		if (this.requireItem && this.whenToUseRequireItem == InteractableBase.WhenToUseRequireItemTypes.OnFinshed && !this.UseRequiredItem(_interactCharacter))
		{
			this.StopInteract();
			return;
		}
		try
		{
			this.OnInteractFinished();
			UnityEvent<CharacterMainControl, InteractableBase> onInteractFinishedEvent = this.OnInteractFinishedEvent;
			if (onInteractFinishedEvent != null)
			{
				onInteractFinishedEvent.Invoke(_interactCharacter, this);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		this.StopInteract();
		if (this.disableOnFinish)
		{
			base.enabled = false;
			if (this.markerObject)
			{
				this.markerObject.gameObject.SetActive(false);
			}
			if (this.interactCollider)
			{
				this.interactCollider.enabled = false;
			}
		}
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0001FCBC File Offset: 0x0001DEBC
	protected virtual void OnUpdate(CharacterMainControl _interactCharacter, float deltaTime)
	{
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0001FCBE File Offset: 0x0001DEBE
	protected virtual void OnTimeOut()
	{
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0001FCC0 File Offset: 0x0001DEC0
	private bool UseRequiredItem(CharacterMainControl interactCharacter)
	{
		Debug.Log("尝试使用");
		ValueTuple<bool, Item> valueTuple = this.TryGetRequiredItem(interactCharacter);
		Item item = valueTuple.Item2;
		if (!valueTuple.Item1 || valueTuple.Item2 == null)
		{
			return false;
		}
		if (item.UseDurability)
		{
			Debug.Log("尝试消耗耐久");
			item.Durability -= 1f;
			if (item.Durability <= 0f)
			{
				item.Detach();
				ItemTreeExtensions.DestroyTree(item);
			}
		}
		else if (!item.Stackable)
		{
			Debug.Log("尝试直接消耗掉");
			item.Detach();
			ItemTreeExtensions.DestroyTree(item);
		}
		else
		{
			Debug.Log("尝试消耗堆叠");
			item.StackCount--;
		}
		if (this.requireOnce)
		{
			this.requireItem = false;
			this.requireItemUsed = true;
			UnityEvent onRequiredItemUsedEvent = this.OnRequiredItemUsedEvent;
			if (onRequiredItemUsedEvent != null)
			{
				onRequiredItemUsedEvent.Invoke();
			}
			if (MultiSceneCore.Instance)
			{
				MultiSceneCore.Instance.inLevelData[this.requireItemDataKeyCached] = true;
				Debug.Log("设置使用过物品为true");
			}
		}
		return true;
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0001FDD0 File Offset: 0x0001DFD0
	public bool CheckInteractable()
	{
		if (this.interactCharacter != null)
		{
			if (!(this.interactCharacter.interactAction.InteractingTarget != this))
			{
				return false;
			}
			this.StopInteract();
		}
		return (Time.time - this.lastStopTime >= this.coolTime || this.coolTime <= 0f || this.lastStopTime <= 0f) && this.IsInteractable();
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0001FE43 File Offset: 0x0001E043
	protected virtual bool IsInteractable()
	{
		return true;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0001FE46 File Offset: 0x0001E046
	protected virtual void OnInteractStart(CharacterMainControl interactCharacter)
	{
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0001FE48 File Offset: 0x0001E048
	protected virtual void OnInteractStop()
	{
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0001FE4A File Offset: 0x0001E04A
	protected virtual void OnInteractFinished()
	{
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0001FE4C File Offset: 0x0001E04C
	public string GetInteractName()
	{
		if (this.overrideInteractName)
		{
			return this.InteractName;
		}
		return LocalizationManager.ToPlainText("UI_Interact");
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0001FE68 File Offset: 0x0001E068
	public string GetRequiredItemName()
	{
		if (!this.requireItem)
		{
			return null;
		}
		return this.CachedMeta.DisplayName;
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0001FE8D File Offset: 0x0001E08D
	public Sprite GetRequireditemIcon()
	{
		if (!this.requireItem)
		{
			return null;
		}
		return this.CachedMeta.icon;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0001FEA4 File Offset: 0x0001E0A4
	protected virtual void OnDestroy()
	{
		if (this.Interacting)
		{
			this.StopInteract();
		}
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0001FEB4 File Offset: 0x0001E0B4
	public virtual Progress GetProgress()
	{
		Progress result = default(Progress);
		if (this.Interacting && this.InteractTime > 0f)
		{
			result.inProgress = true;
			result.total = this.InteractTime;
			result.current = this.interactTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0001FF0C File Offset: 0x0001E10C
	[return: TupleElementNames(new string[]
	{
		"hasItem",
		"ItemInstance"
	})]
	public ValueTuple<bool, Item> TryGetRequiredItem(CharacterMainControl fromCharacter)
	{
		if (!this.requireItem)
		{
			return new ValueTuple<bool, Item>(false, null);
		}
		if (!fromCharacter)
		{
			return new ValueTuple<bool, Item>(false, null);
		}
		if (MasterKeysManager.IsActive(this.requireItemId))
		{
			return new ValueTuple<bool, Item>(true, null);
		}
		foreach (Slot slot in fromCharacter.CharacterItem.Slots)
		{
			if (slot.Content && slot.Content.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, slot.Content);
			}
		}
		foreach (Item item in fromCharacter.CharacterItem.Inventory)
		{
			if (item.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, item);
			}
			if (item.Slots != null && item.Slots.Count > 0)
			{
				foreach (Slot slot2 in item.Slots)
				{
					if (slot2.Content != null && slot2.Content.TypeID == this.requireItemId)
					{
						return new ValueTuple<bool, Item>(true, slot2.Content);
					}
				}
			}
		}
		foreach (Item item2 in LevelManager.Instance.PetProxy.Inventory)
		{
			if (item2.TypeID == this.requireItemId)
			{
				return new ValueTuple<bool, Item>(true, item2);
			}
			if (item2.Slots && item2.Slots.Count > 0)
			{
				foreach (Slot slot3 in item2.Slots)
				{
					if (slot3.Content != null && slot3.Content.TypeID == this.requireItemId)
					{
						return new ValueTuple<bool, Item>(true, slot3.Content);
					}
				}
			}
		}
		return new ValueTuple<bool, Item>(false, null);
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002019C File Offset: 0x0001E39C
	private int GetKey()
	{
		if (this.overrideItemUsedKey)
		{
			return this.overrideItemUsedSaveKey.GetHashCode();
		}
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return string.Format("Intact_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x00020214 File Offset: 0x0001E414
	public void InteractWithMainCharacter()
	{
		CharacterMainControl main = CharacterMainControl.Main;
		if (main == null)
		{
			return;
		}
		main.Interact(this);
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x00020226 File Offset: 0x0001E426
	private void OnDrawGizmos()
	{
		if (!this.interactMarkerVisible)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(base.transform.TransformPoint(this.interactMarkerOffset), 0.1f);
	}

	// Token: 0x040006BC RID: 1724
	public bool interactableGroup;

	// Token: 0x040006BD RID: 1725
	[SerializeField]
	private List<InteractableBase> otherInterablesInGroup;

	// Token: 0x040006BE RID: 1726
	public bool zoomIn = true;

	// Token: 0x040006BF RID: 1727
	private List<InteractableBase> _interactbleList = new List<InteractableBase>();

	// Token: 0x040006C0 RID: 1728
	[SerializeField]
	private float interactTime;

	// Token: 0x040006C1 RID: 1729
	public bool finishWhenTimeOut = true;

	// Token: 0x040006C2 RID: 1730
	private float interactTimer;

	// Token: 0x040006C3 RID: 1731
	public Vector3 interactMarkerOffset;

	// Token: 0x040006C4 RID: 1732
	public bool overrideInteractName;

	// Token: 0x040006C5 RID: 1733
	[LocalizationKey("Default")]
	private string defaultInteractNameKey = "UI_Interact";

	// Token: 0x040006C6 RID: 1734
	[LocalizationKey("Interact")]
	public string _overrideInteractNameKey;

	// Token: 0x040006C7 RID: 1735
	public Collider interactCollider;

	// Token: 0x040006C8 RID: 1736
	public bool requireItem;

	// Token: 0x040006C9 RID: 1737
	public bool requireOnce = true;

	// Token: 0x040006CA RID: 1738
	[ItemTypeID]
	public int requireItemId;

	// Token: 0x040006CB RID: 1739
	public float unlockTime;

	// Token: 0x040006CC RID: 1740
	public bool overrideItemUsedKey;

	// Token: 0x040006CD RID: 1741
	public string overrideItemUsedSaveKey;

	// Token: 0x040006CE RID: 1742
	public InteractableBase.WhenToUseRequireItemTypes whenToUseRequireItem;

	// Token: 0x040006CF RID: 1743
	public UnityEvent OnRequiredItemUsedEvent;

	// Token: 0x040006D0 RID: 1744
	private int requireItemDataKeyCached;

	// Token: 0x040006D1 RID: 1745
	private bool requireItemUsed;

	// Token: 0x040006D2 RID: 1746
	private ItemMetaData? _cachedMeta;

	// Token: 0x040006D3 RID: 1747
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractStartEvent;

	// Token: 0x040006D4 RID: 1748
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractTimeoutEvent;

	// Token: 0x040006D5 RID: 1749
	public UnityEvent<CharacterMainControl, InteractableBase> OnInteractFinishedEvent;

	// Token: 0x040006D7 RID: 1751
	public bool disableOnFinish;

	// Token: 0x040006D8 RID: 1752
	public float coolTime;

	// Token: 0x040006D9 RID: 1753
	private float lastStopTime = -1f;

	// Token: 0x040006DA RID: 1754
	protected CharacterMainControl interactCharacter;

	// Token: 0x040006DB RID: 1755
	private bool timeOut;

	// Token: 0x040006DC RID: 1756
	[SerializeField]
	private bool interactMarkerVisible = true;

	// Token: 0x040006DD RID: 1757
	private InteractMarker markerObject;

	// Token: 0x02000481 RID: 1153
	public enum WhenToUseRequireItemTypes
	{
		// Token: 0x04001C01 RID: 7169
		None,
		// Token: 0x04001C02 RID: 7170
		OnFinshed,
		// Token: 0x04001C03 RID: 7171
		OnTimeOut,
		// Token: 0x04001C04 RID: 7172
		OnStartInteract
	}
}

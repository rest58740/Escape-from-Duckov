using System;
using System.Collections.Generic;
using Duckov;
using Duckov.Scenes;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000DA RID: 218
public class Door : MonoBehaviour
{
	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001EE9A File Offset: 0x0001D09A
	public bool IsOpen
	{
		get
		{
			return !this.closed;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001EEA5 File Offset: 0x0001D0A5
	public bool NoRequireItem
	{
		get
		{
			return !this.interact || !this.interact.requireItem;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001EEC4 File Offset: 0x0001D0C4
	public InteractableBase Interact
	{
		get
		{
			return this.interact;
		}
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0001EECC File Offset: 0x0001D0CC
	private void Start()
	{
		if (this._doorClosedDataKeyCached == -1)
		{
			this._doorClosedDataKeyCached = this.GetKey();
		}
		object obj;
		if (!this.ignoreInLevelData && MultiSceneCore.Instance && MultiSceneCore.Instance.inLevelData.TryGetValue(this._doorClosedDataKeyCached, out obj) && obj is bool)
		{
			bool flag = (bool)obj;
			Debug.Log(string.Format("存在门存档信息：{0}", flag));
			this.closed = flag;
		}
		this.targetLerpValue = (this.closedLerpValue = (this.closed ? 1f : 0f));
		this.SyncNavmeshCut();
		this.SetPartsByLerpValue(true);
	}

	// Token: 0x060006DE RID: 1758 RVA: 0x0001EF76 File Offset: 0x0001D176
	private void OnEnable()
	{
		if (this.doorCollider)
		{
			this.doorCollider.isTrigger = true;
		}
	}

	// Token: 0x060006DF RID: 1759 RVA: 0x0001EF91 File Offset: 0x0001D191
	private void OnDisable()
	{
		if (this.doorCollider)
		{
			this.doorCollider.isTrigger = false;
		}
	}

	// Token: 0x060006E0 RID: 1760 RVA: 0x0001EFAC File Offset: 0x0001D1AC
	private void SyncNavmeshCut()
	{
		if (!this.closed)
		{
			bool enabled = this.activeNavmeshCutWhenDoorIsOpen;
			foreach (NavmeshCut navmeshCut in this.navmeshCuts)
			{
				if (navmeshCut)
				{
					navmeshCut.enabled = enabled;
				}
			}
			return;
		}
		if (this.NoRequireItem)
		{
			return;
		}
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0001F028 File Offset: 0x0001D228
	private void Update()
	{
		this.targetLerpValue = (this.closed ? 1f : 0f);
		if (this.targetLerpValue == this.closedLerpValue)
		{
			base.enabled = false;
		}
		this.closedLerpValue = Mathf.MoveTowards(this.closedLerpValue, this.targetLerpValue, Time.deltaTime / this.lerpTime);
		this.SetPartsByLerpValue(this.targetLerpValue == this.closedLerpValue);
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0001F09B File Offset: 0x0001D29B
	public void Switch()
	{
		this.SetClosed(!this.closed, true);
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0001F0AD File Offset: 0x0001D2AD
	public void Open()
	{
		this.SetClosed(false, true);
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0001F0B7 File Offset: 0x0001D2B7
	public void Close()
	{
		this.SetClosed(true, true);
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0001F0C1 File Offset: 0x0001D2C1
	public void ForceSetClosed(bool _closed, bool triggerEvent)
	{
		this.SetClosed(_closed, triggerEvent);
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0001F0CC File Offset: 0x0001D2CC
	private void SetClosed(bool _closed, bool triggerEvent = true)
	{
		if (!LevelManager.LevelInited)
		{
			Debug.LogError("在关卡没有初始化时，不能对门进行设置");
			return;
		}
		if (triggerEvent)
		{
			if (_closed)
			{
				UnityEvent onCloseEvent = this.OnCloseEvent;
				if (onCloseEvent != null)
				{
					onCloseEvent.Invoke();
				}
			}
			else
			{
				UnityEvent onOpenEvent = this.OnOpenEvent;
				if (onOpenEvent != null)
				{
					onOpenEvent.Invoke();
				}
			}
		}
		Debug.Log(string.Format("Set Door Closed:{0}", _closed));
		if (this._doorClosedDataKeyCached == -1)
		{
			this._doorClosedDataKeyCached = this.GetKey();
		}
		this.closed = _closed;
		this.targetLerpValue = (this.closed ? 1f : 0f);
		if (this.closedLerpValue != this.targetLerpValue)
		{
			base.enabled = true;
		}
		if (this.hasSound)
		{
			AudioManager.Post(_closed ? this.closeSound : this.openSound, base.gameObject);
		}
		if (MultiSceneCore.Instance)
		{
			MultiSceneCore.Instance.inLevelData[this._doorClosedDataKeyCached] = this.closed;
		}
		else
		{
			Debug.Log("没有MultiScene Core，无法存储data");
		}
		this.SyncNavmeshCut();
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0001F1D8 File Offset: 0x0001D3D8
	private List<Door.DoorTransformInfo> GetCurrentTransformInfos()
	{
		List<Door.DoorTransformInfo> list = new List<Door.DoorTransformInfo>();
		foreach (Transform transform in this.doorParts)
		{
			Door.DoorTransformInfo item = default(Door.DoorTransformInfo);
			if (transform != null)
			{
				item.target = transform;
				item.localPosition = transform.localPosition;
				item.localRotation = transform.localRotation;
				item.activation = transform.gameObject.activeSelf;
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0001F27C File Offset: 0x0001D47C
	public void SetParts(List<Door.DoorTransformInfo> transforms)
	{
		for (int i = 0; i < transforms.Count; i++)
		{
			Door.DoorTransformInfo doorTransformInfo = transforms[i];
			if (!(doorTransformInfo.target == null))
			{
				doorTransformInfo.target.localPosition = doorTransformInfo.localPosition;
				doorTransformInfo.target.localRotation = doorTransformInfo.localRotation;
				doorTransformInfo.target.gameObject.SetActive(doorTransformInfo.activation);
			}
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0001F2F0 File Offset: 0x0001D4F0
	private void SetPartsByLerpValue(bool setActivation)
	{
		if (this.doorParts.Count != this.closeTransforms.Count || this.doorParts.Count != this.openTransforms.Count)
		{
			return;
		}
		for (int i = 0; i < this.openTransforms.Count; i++)
		{
			Door.DoorTransformInfo doorTransformInfo = this.openTransforms[i];
			Door.DoorTransformInfo doorTransformInfo2 = this.closeTransforms[i];
			if (!(doorTransformInfo.target == null) && !(doorTransformInfo.target != doorTransformInfo2.target))
			{
				doorTransformInfo.target.localPosition = Vector3.Lerp(doorTransformInfo.localPosition, doorTransformInfo2.localPosition, this.closedLerpValue);
				doorTransformInfo.target.localRotation = Quaternion.Lerp(doorTransformInfo.localRotation, doorTransformInfo2.localRotation, this.closedLerpValue);
				if (setActivation)
				{
					if (this.closedLerpValue >= 1f)
					{
						doorTransformInfo.target.gameObject.SetActive(doorTransformInfo2.activation);
					}
					else
					{
						doorTransformInfo.target.gameObject.SetActive(doorTransformInfo.activation);
					}
				}
			}
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0001F418 File Offset: 0x0001D618
	private int GetKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return string.Format("Door_{0}", vector3Int).GetHashCode();
	}

	// Token: 0x040006A8 RID: 1704
	private bool closed = true;

	// Token: 0x040006A9 RID: 1705
	private float closedLerpValue;

	// Token: 0x040006AA RID: 1706
	private float targetLerpValue;

	// Token: 0x040006AB RID: 1707
	[SerializeField]
	private float lerpTime = 0.5f;

	// Token: 0x040006AC RID: 1708
	[SerializeField]
	private List<Transform> doorParts;

	// Token: 0x040006AD RID: 1709
	[SerializeField]
	private List<Door.DoorTransformInfo> closeTransforms;

	// Token: 0x040006AE RID: 1710
	[SerializeField]
	private List<Door.DoorTransformInfo> openTransforms;

	// Token: 0x040006AF RID: 1711
	[SerializeField]
	private DoorTrigger doorTrigger;

	// Token: 0x040006B0 RID: 1712
	[SerializeField]
	private Collider doorCollider;

	// Token: 0x040006B1 RID: 1713
	[SerializeField]
	private List<NavmeshCut> navmeshCuts = new List<NavmeshCut>();

	// Token: 0x040006B2 RID: 1714
	[SerializeField]
	private bool activeNavmeshCutWhenDoorIsOpen = true;

	// Token: 0x040006B3 RID: 1715
	[SerializeField]
	private bool ignoreInLevelData;

	// Token: 0x040006B4 RID: 1716
	private int _doorClosedDataKeyCached = -1;

	// Token: 0x040006B5 RID: 1717
	[SerializeField]
	private InteractableBase interact;

	// Token: 0x040006B6 RID: 1718
	public bool hasSound;

	// Token: 0x040006B7 RID: 1719
	public string openSound = "SFX/Actions/door_normal_open";

	// Token: 0x040006B8 RID: 1720
	public string closeSound = "SFX/Actions/door_normal_close";

	// Token: 0x040006B9 RID: 1721
	public UnityEvent OnOpenEvent;

	// Token: 0x040006BA RID: 1722
	public UnityEvent OnCloseEvent;

	// Token: 0x02000480 RID: 1152
	[Serializable]
	public struct DoorTransformInfo
	{
		// Token: 0x04001BFC RID: 7164
		public Transform target;

		// Token: 0x04001BFD RID: 7165
		public Vector3 localPosition;

		// Token: 0x04001BFE RID: 7166
		public quaternion localRotation;

		// Token: 0x04001BFF RID: 7167
		public bool activation;
	}
}

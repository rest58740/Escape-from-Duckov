using System;
using Duckov.Economy;
using Duckov.Scenes;
using Saves;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D6 RID: 214
public class ConstructionSite : MonoBehaviour
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001E653 File Offset: 0x0001C853
	private Color KeyFieldColor
	{
		get
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				return Color.red;
			}
			return Color.white;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001E670 File Offset: 0x0001C870
	private string SaveKey
	{
		get
		{
			return "ConstructionSite_" + this._key;
		}
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x0001E684 File Offset: 0x0001C884
	private void Awake()
	{
		this.costTaker.onPayed += this.OnBuilt;
		this.Load();
		SavesSystem.OnCollectSaveData += this.Save;
		this.costTaker.SetCost(this.cost);
		this.RefreshGameObjects();
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0001E6D6 File Offset: 0x0001C8D6
	private void OnDestroy()
	{
		SavesSystem.OnCollectSaveData -= this.Save;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0001E6EC File Offset: 0x0001C8EC
	private void Save()
	{
		if (this.dontSave)
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			if (MultiSceneCore.Instance.inLevelData.ContainsKey(inLevelDataKey))
			{
				MultiSceneCore.Instance.inLevelData[inLevelDataKey] = this.wasBuilt;
				return;
			}
			MultiSceneCore.Instance.inLevelData.Add(inLevelDataKey, this.wasBuilt);
			return;
		}
		else
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
				return;
			}
			SavesSystem.Save<bool>(this.SaveKey, this.wasBuilt);
			return;
		}
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0001E788 File Offset: 0x0001C988
	private int GetInLevelDataKey()
	{
		Vector3 vector = base.transform.position * 10f;
		int num = Mathf.RoundToInt(vector.x);
		int num2 = Mathf.RoundToInt(vector.y);
		int num3 = Mathf.RoundToInt(vector.z);
		Vector3Int vector3Int;
		vector3Int..ctor(num, num2, num3);
		return ("ConstSite" + vector3Int.ToString()).GetHashCode();
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0001E7F4 File Offset: 0x0001C9F4
	private void Load()
	{
		if (!this.dontSave)
		{
			if (string.IsNullOrWhiteSpace(this._key))
			{
				Debug.LogError(string.Format("Construction Site {0} 没有配置保存用的key", base.gameObject));
			}
			this.wasBuilt = SavesSystem.Load<bool>(this.SaveKey);
		}
		else
		{
			int inLevelDataKey = this.GetInLevelDataKey();
			object obj;
			MultiSceneCore.Instance.inLevelData.TryGetValue(inLevelDataKey, out obj);
			if (obj != null)
			{
				this.wasBuilt = (bool)obj;
			}
		}
		if (this.wasBuilt)
		{
			this.OnActivate();
			return;
		}
		this.OnDeactivate();
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0001E87C File Offset: 0x0001CA7C
	private void Start()
	{
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0001E880 File Offset: 0x0001CA80
	private void OnBuilt(CostTaker taker)
	{
		this.wasBuilt = true;
		UnityEvent<ConstructionSite> unityEvent = this.onBuilt;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
		foreach (GameObject gameObject in this.setActiveOnBuilt)
		{
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.Save();
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0001E8DA File Offset: 0x0001CADA
	private void OnActivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onActivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0001E8F4 File Offset: 0x0001CAF4
	private void OnDeactivate()
	{
		UnityEvent<ConstructionSite> unityEvent = this.onDeactivate;
		if (unityEvent != null)
		{
			unityEvent.Invoke(this);
		}
		this.RefreshGameObjects();
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0001E910 File Offset: 0x0001CB10
	public void RefreshGameObjects()
	{
		this.costTaker.gameObject.SetActive(!this.wasBuilt);
		foreach (GameObject gameObject in this.notBuiltGameObjects)
		{
			if (gameObject)
			{
				gameObject.SetActive(!this.wasBuilt);
			}
		}
		foreach (GameObject gameObject2 in this.builtGameObjects)
		{
			if (gameObject2)
			{
				gameObject2.SetActive(this.wasBuilt);
			}
		}
	}

	// Token: 0x0400068B RID: 1675
	[SerializeField]
	private string _key;

	// Token: 0x0400068C RID: 1676
	[SerializeField]
	private bool dontSave;

	// Token: 0x0400068D RID: 1677
	private bool saveInMultiSceneCore;

	// Token: 0x0400068E RID: 1678
	[SerializeField]
	private Cost cost;

	// Token: 0x0400068F RID: 1679
	[SerializeField]
	private CostTaker costTaker;

	// Token: 0x04000690 RID: 1680
	[SerializeField]
	private GameObject[] notBuiltGameObjects;

	// Token: 0x04000691 RID: 1681
	[SerializeField]
	private GameObject[] builtGameObjects;

	// Token: 0x04000692 RID: 1682
	[SerializeField]
	private GameObject[] setActiveOnBuilt;

	// Token: 0x04000693 RID: 1683
	[SerializeField]
	private UnityEvent<ConstructionSite> onBuilt;

	// Token: 0x04000694 RID: 1684
	[SerializeField]
	private UnityEvent<ConstructionSite> onActivate;

	// Token: 0x04000695 RID: 1685
	[SerializeField]
	private UnityEvent<ConstructionSite> onDeactivate;

	// Token: 0x04000696 RID: 1686
	private bool wasBuilt;
}

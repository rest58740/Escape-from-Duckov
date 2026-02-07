using System;
using Duckov.Utilities;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class CostTakerHUD : MonoBehaviour
{
	// Token: 0x17000141 RID: 321
	// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0001EBD8 File Offset: 0x0001CDD8
	private PrefabPool<CostTakerHUD_Entry> EntryPool
	{
		get
		{
			if (this._entryPool == null)
			{
				this._entryPool = new PrefabPool<CostTakerHUD_Entry>(this.entryTemplate, null, null, null, null, true, 10, 10000, null);
			}
			return this._entryPool;
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0001EC11 File Offset: 0x0001CE11
	private void Awake()
	{
		this.entryTemplate.gameObject.SetActive(false);
		this.ShowAll();
		CostTaker.OnCostTakerRegistered += this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered += this.OnCostTakerUnregistered;
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0001EC4C File Offset: 0x0001CE4C
	private void OnDestroy()
	{
		CostTaker.OnCostTakerRegistered -= this.OnCostTakerRegistered;
		CostTaker.OnCostTakerUnregistered -= this.OnCostTakerUnregistered;
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0001EC70 File Offset: 0x0001CE70
	private void OnCostTakerRegistered(CostTaker taker)
	{
		this.ShowHUD(taker);
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0001EC79 File Offset: 0x0001CE79
	private void OnCostTakerUnregistered(CostTaker taker)
	{
		this.HideHUD(taker);
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0001EC82 File Offset: 0x0001CE82
	private void Start()
	{
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0001EC84 File Offset: 0x0001CE84
	private void ShowAll()
	{
		this.EntryPool.ReleaseAll();
		foreach (CostTaker costTaker in CostTaker.ActiveCostTakers)
		{
			this.ShowHUD(costTaker);
		}
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0001ECDC File Offset: 0x0001CEDC
	private void ShowHUD(CostTaker costTaker)
	{
		this.EntryPool.Get(null).Setup(costTaker);
	}

	// Token: 0x060006D0 RID: 1744 RVA: 0x0001ECF0 File Offset: 0x0001CEF0
	private void HideHUD(CostTaker costTaker)
	{
		CostTakerHUD_Entry costTakerHUD_Entry = this.EntryPool.Find((CostTakerHUD_Entry e) => e.gameObject.activeSelf && e.Target == costTaker);
		if (costTakerHUD_Entry == null)
		{
			return;
		}
		this.EntryPool.Release(costTakerHUD_Entry);
	}

	// Token: 0x0400069E RID: 1694
	[SerializeField]
	private CostTakerHUD_Entry entryTemplate;

	// Token: 0x0400069F RID: 1695
	private PrefabPool<CostTakerHUD_Entry> _entryPool;
}

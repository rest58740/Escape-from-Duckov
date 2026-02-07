using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Duckov.Economy;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D7 RID: 215
public class CostTaker : InteractableBase
{
	// Token: 0x1700013F RID: 319
	// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0001E99B File Offset: 0x0001CB9B
	public Cost Cost
	{
		get
		{
			return this.cost;
		}
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x060006B8 RID: 1720 RVA: 0x0001E9A4 File Offset: 0x0001CBA4
	// (remove) Token: 0x060006B9 RID: 1721 RVA: 0x0001E9DC File Offset: 0x0001CBDC
	public event Action<CostTaker> onPayed;

	// Token: 0x060006BA RID: 1722 RVA: 0x0001EA11 File Offset: 0x0001CC11
	protected override bool IsInteractable()
	{
		return this.cost.Enough;
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0001EA20 File Offset: 0x0001CC20
	protected override void OnInteractFinished()
	{
		if (!this.cost.Enough)
		{
			return;
		}
		if (this.cost.Pay(true, true))
		{
			Action<CostTaker> action = this.onPayed;
			if (action != null)
			{
				action(this);
			}
			UnityEvent<CostTaker> unityEvent = this.onPayedUnityEvent;
			if (unityEvent == null)
			{
				return;
			}
			unityEvent.Invoke(this);
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0001EA6D File Offset: 0x0001CC6D
	private void OnEnable()
	{
		CostTaker.Register(this);
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0001EA75 File Offset: 0x0001CC75
	private void OnDisable()
	{
		CostTaker.Unregister(this);
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001EA7D File Offset: 0x0001CC7D
	public static ReadOnlyCollection<CostTaker> ActiveCostTakers
	{
		get
		{
			if (CostTaker._activeCostTakers_ReadOnly == null)
			{
				CostTaker._activeCostTakers_ReadOnly = new ReadOnlyCollection<CostTaker>(CostTaker.activeCostTakers);
			}
			return CostTaker._activeCostTakers_ReadOnly;
		}
	}

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x060006BF RID: 1727 RVA: 0x0001EA9C File Offset: 0x0001CC9C
	// (remove) Token: 0x060006C0 RID: 1728 RVA: 0x0001EAD0 File Offset: 0x0001CCD0
	public static event Action<CostTaker> OnCostTakerRegistered;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x060006C1 RID: 1729 RVA: 0x0001EB04 File Offset: 0x0001CD04
	// (remove) Token: 0x060006C2 RID: 1730 RVA: 0x0001EB38 File Offset: 0x0001CD38
	public static event Action<CostTaker> OnCostTakerUnregistered;

	// Token: 0x060006C3 RID: 1731 RVA: 0x0001EB6B File Offset: 0x0001CD6B
	public static void Register(CostTaker costTaker)
	{
		CostTaker.activeCostTakers.Add(costTaker);
		Action<CostTaker> onCostTakerRegistered = CostTaker.OnCostTakerRegistered;
		if (onCostTakerRegistered == null)
		{
			return;
		}
		onCostTakerRegistered(costTaker);
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0001EB88 File Offset: 0x0001CD88
	public static void Unregister(CostTaker costTaker)
	{
		if (CostTaker.activeCostTakers.Remove(costTaker))
		{
			Action<CostTaker> onCostTakerUnregistered = CostTaker.OnCostTakerUnregistered;
			if (onCostTakerUnregistered == null)
			{
				return;
			}
			onCostTakerUnregistered(costTaker);
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0001EBA7 File Offset: 0x0001CDA7
	public void SetCost(Cost cost)
	{
		CostTaker.Unregister(this);
		this.cost = cost;
		if (base.isActiveAndEnabled)
		{
			CostTaker.Register(this);
		}
	}

	// Token: 0x04000697 RID: 1687
	[SerializeField]
	private Cost cost;

	// Token: 0x04000699 RID: 1689
	public UnityEvent<CostTaker> onPayedUnityEvent;

	// Token: 0x0400069A RID: 1690
	private static List<CostTaker> activeCostTakers = new List<CostTaker>();

	// Token: 0x0400069B RID: 1691
	private static ReadOnlyCollection<CostTaker> _activeCostTakers_ReadOnly;
}

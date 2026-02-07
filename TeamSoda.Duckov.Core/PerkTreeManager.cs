using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.PerkTrees;
using UnityEngine;

// Token: 0x020001ED RID: 493
public class PerkTreeManager : MonoBehaviour
{
	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0003C242 File Offset: 0x0003A442
	public static PerkTreeManager Instance
	{
		get
		{
			return PerkTreeManager.instance;
		}
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x0003C249 File Offset: 0x0003A449
	private void Awake()
	{
		if (PerkTreeManager.instance == null)
		{
			PerkTreeManager.instance = this;
			return;
		}
		Debug.LogError("检测到多个PerkTreeManager");
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x0003C26C File Offset: 0x0003A46C
	public static PerkTree GetPerkTree(string id)
	{
		if (PerkTreeManager.instance == null)
		{
			return null;
		}
		PerkTree perkTree = PerkTreeManager.instance.perkTrees.FirstOrDefault((PerkTree e) => e != null && e.ID == id);
		if (perkTree == null)
		{
			Debug.LogError("未找到PerkTree id:" + id);
		}
		return perkTree;
	}

	// Token: 0x04000C48 RID: 3144
	private static PerkTreeManager instance;

	// Token: 0x04000C49 RID: 3145
	public List<PerkTree> perkTrees;
}

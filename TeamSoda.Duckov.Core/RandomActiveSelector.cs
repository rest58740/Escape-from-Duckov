using System;
using System.Collections.Generic;
using Duckov.Scenes;
using UnityEngine;

// Token: 0x020000AD RID: 173
public class RandomActiveSelector : MonoBehaviour
{
	// Token: 0x060005E9 RID: 1513 RVA: 0x0001A9E0 File Offset: 0x00018BE0
	private void Awake()
	{
		foreach (GameObject gameObject in this.selections)
		{
			if (!(gameObject == null))
			{
				gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0001AA3C File Offset: 0x00018C3C
	private void Update()
	{
		if (!this.setted && LevelManager.LevelInited)
		{
			this.Set();
		}
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001AA54 File Offset: 0x00018C54
	private void Set()
	{
		if (MultiSceneCore.Instance == null)
		{
			return;
		}
		object obj;
		if (MultiSceneCore.Instance.inLevelData.TryGetValue(this.guid, out obj))
		{
			this.activeIndex = (int)obj;
		}
		else
		{
			if (Random.Range(0f, 1f) > this.activeChance)
			{
				this.activeIndex = -1;
			}
			else
			{
				this.activeIndex = Random.Range(0, this.selections.Count);
			}
			MultiSceneCore.Instance.inLevelData.Add(this.guid, this.activeIndex);
		}
		if (this.activeIndex >= 0)
		{
			GameObject gameObject = this.selections[this.activeIndex];
			if (gameObject)
			{
				gameObject.SetActive(true);
			}
		}
		this.setted = true;
		base.enabled = false;
	}

	// Token: 0x04000575 RID: 1397
	[Range(0f, 1f)]
	public float activeChance = 1f;

	// Token: 0x04000576 RID: 1398
	private int activeIndex;

	// Token: 0x04000577 RID: 1399
	private int guid;

	// Token: 0x04000578 RID: 1400
	private bool setted;

	// Token: 0x04000579 RID: 1401
	public List<GameObject> selections;
}

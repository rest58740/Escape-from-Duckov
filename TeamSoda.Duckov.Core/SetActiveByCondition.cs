using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov.Quests;
using UnityEngine;

// Token: 0x020000B0 RID: 176
public class SetActiveByCondition : MonoBehaviour
{
	// Token: 0x060005F2 RID: 1522 RVA: 0x0001AC8C File Offset: 0x00018E8C
	private void Update()
	{
		if (!LevelManager.LevelInited && this.requireLevelInited)
		{
			return;
		}
		if (!this.autoCheck)
		{
			return;
		}
		this.Set();
		if (this.update)
		{
			this.CheckAndLoop().Forget();
		}
		base.enabled = false;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0001ACD8 File Offset: 0x00018ED8
	public void Set()
	{
		if (this.targetObject)
		{
			bool flag = this.conditions.Satisfied();
			if (this.inverse)
			{
				flag = !flag;
			}
			this.targetObject.SetActive(flag);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0001AD18 File Offset: 0x00018F18
	private UniTaskVoid CheckAndLoop()
	{
		SetActiveByCondition.<CheckAndLoop>d__9 <CheckAndLoop>d__;
		<CheckAndLoop>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CheckAndLoop>d__.<>4__this = this;
		<CheckAndLoop>d__.<>1__state = -1;
		<CheckAndLoop>d__.<>t__builder.Start<SetActiveByCondition.<CheckAndLoop>d__9>(ref <CheckAndLoop>d__);
		return <CheckAndLoop>d__.<>t__builder.Task;
	}

	// Token: 0x0400057F RID: 1407
	public GameObject targetObject;

	// Token: 0x04000580 RID: 1408
	public bool inverse;

	// Token: 0x04000581 RID: 1409
	public List<Condition> conditions;

	// Token: 0x04000582 RID: 1410
	public bool autoCheck = true;

	// Token: 0x04000583 RID: 1411
	public bool update;

	// Token: 0x04000584 RID: 1412
	public bool requireLevelInited = true;

	// Token: 0x04000585 RID: 1413
	private float checkTimeSpace = 1f;
}

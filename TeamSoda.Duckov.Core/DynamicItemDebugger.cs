using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class DynamicItemDebugger : MonoBehaviour
{
	// Token: 0x060004CD RID: 1229 RVA: 0x00016248 File Offset: 0x00014448
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		this.Add();
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x0001625C File Offset: 0x0001445C
	private void Add()
	{
		foreach (Item item in this.prefabs)
		{
			ItemAssetsCollection.AddDynamicEntry(item);
		}
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000162B0 File Offset: 0x000144B0
	private void CreateCorresponding()
	{
		this.CreateTask().Forget();
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x000162C0 File Offset: 0x000144C0
	private UniTask CreateTask()
	{
		DynamicItemDebugger.<CreateTask>d__4 <CreateTask>d__;
		<CreateTask>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<CreateTask>d__.<>4__this = this;
		<CreateTask>d__.<>1__state = -1;
		<CreateTask>d__.<>t__builder.Start<DynamicItemDebugger.<CreateTask>d__4>(ref <CreateTask>d__);
		return <CreateTask>d__.<>t__builder.Task;
	}

	// Token: 0x04000416 RID: 1046
	[SerializeField]
	private List<Item> prefabs;
}

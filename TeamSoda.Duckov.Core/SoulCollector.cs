using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using ItemStatsSystem;
using ItemStatsSystem.Items;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class SoulCollector : MonoBehaviour
{
	// Token: 0x06000607 RID: 1543 RVA: 0x0001B102 File Offset: 0x00019302
	private void Awake()
	{
		Health.OnDead += this.OnCharacterDie;
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x0001B115 File Offset: 0x00019315
	private void OnDestroy()
	{
		Health.OnDead -= this.OnCharacterDie;
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x0001B128 File Offset: 0x00019328
	private void Update()
	{
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x0001B12C File Offset: 0x0001932C
	private void OnCharacterDie(Health health, DamageInfo dmgInfo)
	{
		if (!health)
		{
			return;
		}
		if (!health.hasSoul)
		{
			return;
		}
		if (!this.selfCharacter && this.selfAgent.Item)
		{
			this.selfCharacter = this.selfAgent.Item.GetCharacterMainControl();
		}
		if (!this.selfCharacter)
		{
			return;
		}
		if (Vector3.Distance(health.transform.position, this.selfCharacter.transform.position) > 40f)
		{
			return;
		}
		int num = Mathf.RoundToInt(health.MaxHealth / 15f);
		if (num < 1)
		{
			num = 1;
		}
		if (LevelManager.Rule.AdvancedDebuffMode)
		{
			num *= 3;
		}
		this.SpawnCubes(health.transform.position + Vector3.up * 0.75f, num).Forget();
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x0001B20C File Offset: 0x0001940C
	private UniTaskVoid SpawnCubes(Vector3 startPoint, int times)
	{
		SoulCollector.<SpawnCubes>d__10 <SpawnCubes>d__;
		<SpawnCubes>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<SpawnCubes>d__.<>4__this = this;
		<SpawnCubes>d__.startPoint = startPoint;
		<SpawnCubes>d__.times = times;
		<SpawnCubes>d__.<>1__state = -1;
		<SpawnCubes>d__.<>t__builder.Start<SoulCollector.<SpawnCubes>d__10>(ref <SpawnCubes>d__);
		return <SpawnCubes>d__.<>t__builder.Task;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0001B260 File Offset: 0x00019460
	public void AddCube()
	{
		this.AddCubeAsync().Forget();
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001B27C File Offset: 0x0001947C
	private UniTaskVoid AddCubeAsync()
	{
		SoulCollector.<AddCubeAsync>d__12 <AddCubeAsync>d__;
		<AddCubeAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<AddCubeAsync>d__.<>4__this = this;
		<AddCubeAsync>d__.<>1__state = -1;
		<AddCubeAsync>d__.<>t__builder.Start<SoulCollector.<AddCubeAsync>d__12>(ref <AddCubeAsync>d__);
		return <AddCubeAsync>d__.<>t__builder.Task;
	}

	// Token: 0x04000593 RID: 1427
	public DuckovItemAgent selfAgent;

	// Token: 0x04000594 RID: 1428
	private CharacterMainControl selfCharacter;

	// Token: 0x04000595 RID: 1429
	[ItemTypeID]
	public int soulCubeID = 1165;

	// Token: 0x04000596 RID: 1430
	private Slot cubeSlot;

	// Token: 0x04000597 RID: 1431
	public GameObject addFx;

	// Token: 0x04000598 RID: 1432
	public SoulCube cubePfb;
}

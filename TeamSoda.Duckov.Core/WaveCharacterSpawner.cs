using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000098 RID: 152
[RequireComponent(typeof(Points))]
public class WaveCharacterSpawner : CharacterSpawnerComponentBase
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x06000544 RID: 1348 RVA: 0x00017EF1 File Offset: 0x000160F1
	private float minDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.minDistanceToPlayer;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000545 RID: 1349 RVA: 0x00017EFE File Offset: 0x000160FE
	private float maxDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.maxDistanceToPlayer;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06000546 RID: 1350 RVA: 0x00017F0B File Offset: 0x0001610B
	private int scene
	{
		get
		{
			return this.spawnerRoot.RelatedScene;
		}
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x00017F18 File Offset: 0x00016118
	private void ShowGizmo()
	{
		WaveCharacterSpawner.currentGizmosTag = this.gizmosTag;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00017F28 File Offset: 0x00016128
	public override void Init(CharacterSpawnerRoot root)
	{
		this.spawnerRoot = root;
		if (this.spawnPoints == null)
		{
			this.spawnPoints = base.GetComponent<Points>();
		}
		this.spawnCountRange = new Vector2Int(Mathf.RoundToInt((float)this.spawnCountRange.x * LevelManager.enemySpawnCountFactor), Mathf.RoundToInt((float)this.spawnCountRange.y * LevelManager.enemySpawnCountFactor));
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x00017F8F File Offset: 0x0001618F
	private void OnDestroy()
	{
		this.destroied = true;
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00017F98 File Offset: 0x00016198
	private CharacterRandomPresetInfo GetAPresetByWeight()
	{
		if (this.totalWeight < 0f)
		{
			this.totalWeight = 0f;
			for (int i = 0; i < this.randomPresetInfos.Count; i++)
			{
				if (this.randomPresetInfos[i].randomPreset == null)
				{
					this.randomPresetInfos.RemoveAt(i);
					i--;
					Debug.Log("Null preset");
				}
				else
				{
					this.totalWeight += this.randomPresetInfos[i].weight;
				}
			}
		}
		float num = Random.Range(0f, this.totalWeight);
		float num2 = 0f;
		for (int j = 0; j < this.randomPresetInfos.Count; j++)
		{
			num2 += this.randomPresetInfos[j].weight;
			if (num < num2)
			{
				return this.randomPresetInfos[j];
			}
		}
		Debug.LogError("权重计算错误", base.gameObject);
		return this.randomPresetInfos[this.randomPresetInfos.Count - 1];
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x000180A0 File Offset: 0x000162A0
	public override void StartSpawn()
	{
		this.CreateAsync().Forget();
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000180BC File Offset: 0x000162BC
	private UniTaskVoid CreateAsync()
	{
		WaveCharacterSpawner.<CreateAsync>d__27 <CreateAsync>d__;
		<CreateAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateAsync>d__.<>4__this = this;
		<CreateAsync>d__.<>1__state = -1;
		<CreateAsync>d__.<>t__builder.Start<WaveCharacterSpawner.<CreateAsync>d__27>(ref <CreateAsync>d__);
		return <CreateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x00018100 File Offset: 0x00016300
	private UniTask<CharacterMainControl> CreateAt(Vector3 point, int scene, CharacterSpawnerGroup group, bool isLeader)
	{
		WaveCharacterSpawner.<CreateAt>d__28 <CreateAt>d__;
		<CreateAt>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateAt>d__.<>4__this = this;
		<CreateAt>d__.point = point;
		<CreateAt>d__.scene = scene;
		<CreateAt>d__.group = group;
		<CreateAt>d__.isLeader = isLeader;
		<CreateAt>d__.<>1__state = -1;
		<CreateAt>d__.<>t__builder.Start<WaveCharacterSpawner.<CreateAt>d__28>(ref <CreateAt>d__);
		return <CreateAt>d__.<>t__builder.Task;
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00018164 File Offset: 0x00016364
	private void OnDrawGizmos()
	{
		if (WaveCharacterSpawner.currentGizmosTag != this.gizmosTag)
		{
			return;
		}
		Gizmos.color = Color.yellow;
		if (this.spawnPoints && this.spawnPoints.points.Count > 0)
		{
			Vector3 point = this.spawnPoints.GetPoint(0);
			Vector3 vector = point + Vector3.up * 20f;
			Gizmos.DrawWireSphere(point, 10f);
			Gizmos.DrawLine(point, vector);
			Gizmos.DrawSphere(vector, 3f);
		}
	}

	// Token: 0x040004CF RID: 1231
	public Points spawnPoints;

	// Token: 0x040004D0 RID: 1232
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x040004D1 RID: 1233
	public CharacterSpawnerGroup masterGroup;

	// Token: 0x040004D2 RID: 1234
	public List<CharacterRandomPresetInfo> randomPresetInfos;

	// Token: 0x040004D3 RID: 1235
	private float delayTime = 1f;

	// Token: 0x040004D4 RID: 1236
	public Vector2Int spawnCountRange;

	// Token: 0x040004D5 RID: 1237
	private float totalWeight = -1f;

	// Token: 0x040004D6 RID: 1238
	public bool isStaticTarget;

	// Token: 0x040004D7 RID: 1239
	public static string currentGizmosTag;

	// Token: 0x040004D8 RID: 1240
	public bool firstIsLeader;

	// Token: 0x040004D9 RID: 1241
	private bool firstCreateStarted;

	// Token: 0x040004DA RID: 1242
	public UnityEvent OnStartCreateEvent;

	// Token: 0x040004DB RID: 1243
	private int targetSpawnCount;

	// Token: 0x040004DC RID: 1244
	private int currentSpawnedCount;

	// Token: 0x040004DD RID: 1245
	private bool destroied;

	// Token: 0x040004DE RID: 1246
	public string gizmosTag;
}

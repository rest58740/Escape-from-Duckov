using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000096 RID: 150
[RequireComponent(typeof(Points))]
public class RandomCharacterSpawner : CharacterSpawnerComponentBase
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000538 RID: 1336 RVA: 0x00017BCF File Offset: 0x00015DCF
	private float minDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.minDistanceToPlayer;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x06000539 RID: 1337 RVA: 0x00017BDC File Offset: 0x00015DDC
	private float maxDistanceToMainCharacter
	{
		get
		{
			return this.spawnerRoot.maxDistanceToPlayer;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600053A RID: 1338 RVA: 0x00017BE9 File Offset: 0x00015DE9
	private int scene
	{
		get
		{
			return this.spawnerRoot.RelatedScene;
		}
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00017BF6 File Offset: 0x00015DF6
	private void ShowGizmo()
	{
		RandomCharacterSpawner.currentGizmosTag = this.gizmosTag;
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00017C04 File Offset: 0x00015E04
	public override void Init(CharacterSpawnerRoot root)
	{
		this.spawnerRoot = root;
		if (this.spawnPoints == null)
		{
			this.spawnPoints = base.GetComponent<Points>();
		}
		this.spawnCountRange = new Vector2Int(Mathf.RoundToInt((float)this.spawnCountRange.x * LevelManager.enemySpawnCountFactor), Mathf.RoundToInt((float)this.spawnCountRange.y * LevelManager.enemySpawnCountFactor));
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00017C6B File Offset: 0x00015E6B
	private void OnDestroy()
	{
		this.destroied = true;
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00017C74 File Offset: 0x00015E74
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

	// Token: 0x0600053F RID: 1343 RVA: 0x00017D7C File Offset: 0x00015F7C
	public override void StartSpawn()
	{
		this.CreateAsync().Forget();
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00017D98 File Offset: 0x00015F98
	private UniTaskVoid CreateAsync()
	{
		RandomCharacterSpawner.<CreateAsync>d__28 <CreateAsync>d__;
		<CreateAsync>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<CreateAsync>d__.<>4__this = this;
		<CreateAsync>d__.<>1__state = -1;
		<CreateAsync>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAsync>d__28>(ref <CreateAsync>d__);
		return <CreateAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00017DDC File Offset: 0x00015FDC
	private UniTask<CharacterMainControl> CreateAt(Vector3 point, int scene, CharacterSpawnerGroup group, bool isLeader)
	{
		RandomCharacterSpawner.<CreateAt>d__29 <CreateAt>d__;
		<CreateAt>d__.<>t__builder = AsyncUniTaskMethodBuilder<CharacterMainControl>.Create();
		<CreateAt>d__.<>4__this = this;
		<CreateAt>d__.point = point;
		<CreateAt>d__.scene = scene;
		<CreateAt>d__.group = group;
		<CreateAt>d__.isLeader = isLeader;
		<CreateAt>d__.<>1__state = -1;
		<CreateAt>d__.<>t__builder.Start<RandomCharacterSpawner.<CreateAt>d__29>(ref <CreateAt>d__);
		return <CreateAt>d__.<>t__builder.Task;
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00017E40 File Offset: 0x00016040
	private void OnDrawGizmos()
	{
		if (RandomCharacterSpawner.currentGizmosTag != this.gizmosTag)
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

	// Token: 0x040004BC RID: 1212
	public Points spawnPoints;

	// Token: 0x040004BD RID: 1213
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x040004BE RID: 1214
	public CharacterSpawnerGroup masterGroup;

	// Token: 0x040004BF RID: 1215
	public float spawnTimeSpace = 0.1f;

	// Token: 0x040004C0 RID: 1216
	public List<CharacterRandomPresetInfo> randomPresetInfos;

	// Token: 0x040004C1 RID: 1217
	private float delayTime = 1f;

	// Token: 0x040004C2 RID: 1218
	public Vector2Int spawnCountRange;

	// Token: 0x040004C3 RID: 1219
	private float totalWeight = -1f;

	// Token: 0x040004C4 RID: 1220
	public bool isStaticTarget;

	// Token: 0x040004C5 RID: 1221
	public static string currentGizmosTag;

	// Token: 0x040004C6 RID: 1222
	public bool firstIsLeader;

	// Token: 0x040004C7 RID: 1223
	private bool firstCreateStarted;

	// Token: 0x040004C8 RID: 1224
	public UnityEvent OnStartCreateEvent;

	// Token: 0x040004C9 RID: 1225
	private int targetSpawnCount;

	// Token: 0x040004CA RID: 1226
	private int currentSpawnedCount;

	// Token: 0x040004CB RID: 1227
	private bool destroied;

	// Token: 0x040004CC RID: 1228
	public string gizmosTag;
}

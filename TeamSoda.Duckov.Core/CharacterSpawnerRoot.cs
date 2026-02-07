using System;
using System.Collections.Generic;
using Duckov.Scenes;
using Duckov.Weathers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// Token: 0x02000095 RID: 149
public class CharacterSpawnerRoot : MonoBehaviour
{
	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600052A RID: 1322 RVA: 0x000175BD File Offset: 0x000157BD
	public int RelatedScene
	{
		get
		{
			return this.relatedScene;
		}
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x000175C8 File Offset: 0x000157C8
	private void Awake()
	{
		if (LevelManager.forceBossSpawn)
		{
			this.spawnChance = 1f;
		}
		if (this.createdCharacters == null)
		{
			this.createdCharacters = new List<CharacterMainControl>();
		}
		if (this.despawningCharacters == null)
		{
			this.despawningCharacters = new List<CharacterMainControl>();
		}
		if (!this.useTimeOfDay && !this.checkWeather)
		{
			this.despawnIfTimingWrong = false;
		}
		if (this.needTrigger && this.trigger)
		{
			this.trigger.triggerOnce = false;
			this.trigger.onlyMainCharacter = true;
			this.trigger.DoOnTriggerEnter.AddListener(new UnityAction(this.DoOnTriggerEnter));
			this.trigger.DoOnTriggerExit.AddListener(new UnityAction(this.DoOnTriggerLeave));
		}
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x0001768C File Offset: 0x0001588C
	private void OnDestroy()
	{
		if (this.needTrigger && this.trigger)
		{
			this.trigger.DoOnTriggerEnter.RemoveListener(new UnityAction(this.DoOnTriggerEnter));
			this.trigger.DoOnTriggerExit.RemoveListener(new UnityAction(this.DoOnTriggerLeave));
		}
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x000176E6 File Offset: 0x000158E6
	private void Start()
	{
		if (LevelManager.Instance && LevelManager.Instance.IsBaseLevel)
		{
			this.minDistanceToPlayer = 0f;
		}
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x0001770C File Offset: 0x0001590C
	private void Update()
	{
		if (!this.inited && LevelManager.LevelInited)
		{
			this.Init();
		}
		bool flag = this.CheckTiming();
		bool flag2 = this.CheckNeedOfTrigger();
		if (this.inited && !this.created && flag && flag2)
		{
			this.StartSpawn();
		}
		if (this.created && !flag && this.despawnIfTimingWrong)
		{
			this.despawningCharacters.AddRange(this.createdCharacters);
			this.createdCharacters.Clear();
			this.created = false;
		}
		this.despawnTickTimer -= Time.deltaTime;
		if (this.despawnTickTimer < 0f && this.despawnIfTimingWrong && this.despawningCharacters.Count > 0)
		{
			this.CheckDespawn();
		}
		if (this.despawnTickTimer < 0f && !this.allDead && this.stillhasAliveCharacters && !this.allDeadEventInvoked)
		{
			if (this.createdCharacters.Count <= 0)
			{
				this.allDead = true;
			}
			else
			{
				this.allDead = true;
				foreach (CharacterMainControl characterMainControl in this.createdCharacters)
				{
					if (characterMainControl != null && characterMainControl.Health && !characterMainControl.Health.IsDead)
					{
						this.allDead = false;
						break;
					}
				}
			}
			if (this.allDead)
			{
				this.stillhasAliveCharacters = false;
				UnityEvent onAllDeadEvent = this.OnAllDeadEvent;
				if (onAllDeadEvent != null)
				{
					onAllDeadEvent.Invoke();
				}
				this.allDeadEventInvoked = true;
			}
		}
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x000178B0 File Offset: 0x00015AB0
	private void CheckDespawn()
	{
		for (int i = 0; i < this.despawningCharacters.Count; i++)
		{
			CharacterMainControl characterMainControl = this.despawningCharacters[i];
			if (!characterMainControl)
			{
				this.despawningCharacters.RemoveAt(i);
				i--;
			}
			else if (!characterMainControl.gameObject.activeInHierarchy)
			{
				Object.Destroy(characterMainControl.gameObject);
				this.despawningCharacters.RemoveAt(i);
				i--;
			}
		}
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00017922 File Offset: 0x00015B22
	private bool CheckNeedOfTrigger()
	{
		return !this.needTrigger || this.playerInTrigger;
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00017938 File Offset: 0x00015B38
	private bool CheckTiming()
	{
		if (LevelManager.Instance == null)
		{
			return false;
		}
		bool flag;
		if (this.useTimeOfDay)
		{
			float num = (float)GameClock.TimeOfDay.TotalHours % 24f;
			flag = ((num >= this.spawnTimeRangeFrom && num <= this.spawnTimeRangeTo) || (this.spawnTimeRangeTo < this.spawnTimeRangeFrom && (num >= this.spawnTimeRangeFrom || num <= this.spawnTimeRangeTo)));
		}
		else
		{
			flag = (LevelManager.Instance.LevelTime >= this.whenToSpawn);
		}
		bool flag2 = true;
		if (this.checkWeather && !this.targetWeathers.Contains(TimeOfDayController.Instance.CurrentWeather))
		{
			flag2 = false;
		}
		return flag && flag2;
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x000179F0 File Offset: 0x00015BF0
	private void Init()
	{
		this.inited = true;
		this.spawnerComponent.Init(this);
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		bool flag = true;
		if (MultiSceneCore.Instance != null)
		{
			flag = MultiSceneCore.Instance.usedCreatorIds.Contains(this.SpawnerGuid);
		}
		if (flag)
		{
			Debug.Log("Contain this spawner");
			Object.Destroy(base.gameObject);
			return;
		}
		this.relatedScene = SceneManager.GetActiveScene().buildIndex;
		base.transform.SetParent(null);
		MultiSceneCore.MoveToMainScene(base.gameObject);
		MultiSceneCore.Instance.usedCreatorIds.Add(this.SpawnerGuid);
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00017AA0 File Offset: 0x00015CA0
	private void StartSpawn()
	{
		if (this.created)
		{
			return;
		}
		this.created = true;
		if (Random.Range(0f, 1f) > this.spawnChance)
		{
			return;
		}
		UnityEvent onStartEvent = this.OnStartEvent;
		if (onStartEvent != null)
		{
			onStartEvent.Invoke();
		}
		if (this.spawnerComponent)
		{
			this.spawnerComponent.StartSpawn();
		}
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00017AFE File Offset: 0x00015CFE
	private void DoOnTriggerEnter()
	{
		this.playerInTrigger = true;
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00017B07 File Offset: 0x00015D07
	private void DoOnTriggerLeave()
	{
		this.playerInTrigger = false;
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00017B10 File Offset: 0x00015D10
	public void AddCreatedCharacter(CharacterMainControl c)
	{
		this.createdCharacters.Add(c);
		this.stillhasAliveCharacters = true;
		if (this.forceTracePlayer && c && c.aiCharacterController && Team.IsEnemy(c.Team, Teams.player))
		{
			c.aiCharacterController.forceTracePlayerDistance = 9999f;
		}
	}

	// Token: 0x040004A0 RID: 1184
	public bool needTrigger;

	// Token: 0x040004A1 RID: 1185
	public OnTriggerEnterEvent trigger;

	// Token: 0x040004A2 RID: 1186
	private bool playerInTrigger;

	// Token: 0x040004A3 RID: 1187
	private bool created;

	// Token: 0x040004A4 RID: 1188
	private bool inited;

	// Token: 0x040004A5 RID: 1189
	[Range(0f, 1f)]
	public float spawnChance = 1f;

	// Token: 0x040004A6 RID: 1190
	public float minDistanceToPlayer = 25f;

	// Token: 0x040004A7 RID: 1191
	public float maxDistanceToPlayer = 9999f;

	// Token: 0x040004A8 RID: 1192
	public bool forceTracePlayer;

	// Token: 0x040004A9 RID: 1193
	public bool useTimeOfDay;

	// Token: 0x040004AA RID: 1194
	public float whenToSpawn;

	// Token: 0x040004AB RID: 1195
	[Range(0f, 24f)]
	public float spawnTimeRangeFrom;

	// Token: 0x040004AC RID: 1196
	[Range(0f, 24f)]
	public float spawnTimeRangeTo;

	// Token: 0x040004AD RID: 1197
	[FormerlySerializedAs("despawnIfOutOfTime")]
	public bool despawnIfTimingWrong;

	// Token: 0x040004AE RID: 1198
	public bool checkWeather;

	// Token: 0x040004AF RID: 1199
	public List<Weather> targetWeathers;

	// Token: 0x040004B0 RID: 1200
	private int relatedScene = -1;

	// Token: 0x040004B1 RID: 1201
	[SerializeField]
	private CharacterSpawnerComponentBase spawnerComponent;

	// Token: 0x040004B2 RID: 1202
	public bool autoRefreshGuid = true;

	// Token: 0x040004B3 RID: 1203
	public int SpawnerGuid;

	// Token: 0x040004B4 RID: 1204
	private List<CharacterMainControl> createdCharacters = new List<CharacterMainControl>();

	// Token: 0x040004B5 RID: 1205
	private List<CharacterMainControl> despawningCharacters = new List<CharacterMainControl>();

	// Token: 0x040004B6 RID: 1206
	private float despawnTickTimer = 1f;

	// Token: 0x040004B7 RID: 1207
	public UnityEvent OnStartEvent;

	// Token: 0x040004B8 RID: 1208
	public UnityEvent OnAllDeadEvent;

	// Token: 0x040004B9 RID: 1209
	private bool allDeadEventInvoked;

	// Token: 0x040004BA RID: 1210
	private bool stillhasAliveCharacters;

	// Token: 0x040004BB RID: 1211
	private bool allDead;
}

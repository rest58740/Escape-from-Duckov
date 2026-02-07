using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class CharacterSpawnerGroup : CharacterSpawnerComponentBase
{
	// Token: 0x17000112 RID: 274
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x00017171 File Offset: 0x00015371
	public AICharacterController LeaderAI
	{
		get
		{
			return this.leaderAI;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00017179 File Offset: 0x00015379
	public void Collect()
	{
		this.spawners = base.GetComponentsInChildren<RandomCharacterSpawner>().ToList<RandomCharacterSpawner>();
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x0001718C File Offset: 0x0001538C
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (randomCharacterSpawner == null)
			{
				Debug.LogError("生成器引用为空：" + base.gameObject.name);
			}
			else
			{
				randomCharacterSpawner.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x0001720C File Offset: 0x0001540C
	public void Awake()
	{
		this.characters = new List<AICharacterController>();
		if (this.hasLeader && Random.Range(0f, 1f) > this.hasLeaderChance)
		{
			this.hasLeader = false;
		}
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00017240 File Offset: 0x00015440
	private void Update()
	{
		if (this.hasLeader && this.leaderAI == null && this.characters.Count > 0)
		{
			for (int i = 0; i < this.characters.Count; i++)
			{
				if (this.characters[i] == null)
				{
					this.characters.RemoveAt(i);
					i--;
				}
				else
				{
					this.leaderAI = this.characters[i];
				}
			}
		}
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x000172BF File Offset: 0x000154BF
	public void AddCharacterSpawned(AICharacterController _character, bool isLeader)
	{
		_character.group = this;
		if (isLeader)
		{
			this.leaderAI = _character;
		}
		else if (this.hasLeader && !this.leaderAI)
		{
			this.leaderAI = _character;
		}
		this.characters.Add(_character);
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x000172FC File Offset: 0x000154FC
	public override void StartSpawn()
	{
		bool flag = true;
		foreach (RandomCharacterSpawner randomCharacterSpawner in this.spawners)
		{
			if (!(randomCharacterSpawner == null))
			{
				randomCharacterSpawner.masterGroup = this;
				if (flag && this.hasLeader)
				{
					randomCharacterSpawner.firstIsLeader = true;
				}
				flag = false;
				randomCharacterSpawner.StartSpawn();
			}
		}
	}

	// Token: 0x04000496 RID: 1174
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x04000497 RID: 1175
	public bool hasLeader;

	// Token: 0x04000498 RID: 1176
	[Range(0f, 1f)]
	public float hasLeaderChance = 1f;

	// Token: 0x04000499 RID: 1177
	public List<RandomCharacterSpawner> spawners;

	// Token: 0x0400049A RID: 1178
	private List<AICharacterController> characters;

	// Token: 0x0400049B RID: 1179
	private AICharacterController leaderAI;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class CharacterSpawnerGroupSelector : CharacterSpawnerComponentBase
{
	// Token: 0x06000524 RID: 1316 RVA: 0x00017388 File Offset: 0x00015588
	public void Collect()
	{
		this.groups = base.GetComponentsInChildren<CharacterSpawnerGroup>().ToList<CharacterSpawnerGroup>();
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			characterSpawnerGroup.Collect();
		}
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x000173EC File Offset: 0x000155EC
	public override void Init(CharacterSpawnerRoot root)
	{
		foreach (CharacterSpawnerGroup characterSpawnerGroup in this.groups)
		{
			if (characterSpawnerGroup == null)
			{
				Debug.LogError("生成器引用为空");
			}
			else
			{
				characterSpawnerGroup.Init(root);
			}
		}
		this.spawnerRoot = root;
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x0001745C File Offset: 0x0001565C
	public override void StartSpawn()
	{
		if (this.spawnGroupCountRange.y > this.groups.Count)
		{
			this.spawnGroupCountRange.y = this.groups.Count;
		}
		if (this.spawnGroupCountRange.x > this.groups.Count)
		{
			this.spawnGroupCountRange.x = this.groups.Count;
		}
		int count = Random.Range(this.spawnGroupCountRange.x, this.spawnGroupCountRange.y);
		this.finalCount = count;
		this.RandomSpawn(count);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x000174EF File Offset: 0x000156EF
	private void OnValidate()
	{
		if (this.groups.Count < 0)
		{
			return;
		}
		if (this.spawnGroupCountRange.x > this.spawnGroupCountRange.y)
		{
			this.spawnGroupCountRange.y = this.spawnGroupCountRange.x;
		}
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00017530 File Offset: 0x00015730
	public void RandomSpawn(int count)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.groups.Count; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < count; j++)
		{
			int index = Random.Range(0, list.Count);
			int index2 = list[index];
			list.RemoveAt(index);
			CharacterSpawnerGroup characterSpawnerGroup = this.groups[index2];
			if (characterSpawnerGroup)
			{
				characterSpawnerGroup.StartSpawn();
			}
		}
	}

	// Token: 0x0400049C RID: 1180
	public CharacterSpawnerRoot spawnerRoot;

	// Token: 0x0400049D RID: 1181
	public List<CharacterSpawnerGroup> groups;

	// Token: 0x0400049E RID: 1182
	public Vector2Int spawnGroupCountRange = new Vector2Int(1, 1);

	// Token: 0x0400049F RID: 1183
	private int finalCount;
}

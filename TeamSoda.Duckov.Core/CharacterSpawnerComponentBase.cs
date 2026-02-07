using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public abstract class CharacterSpawnerComponentBase : MonoBehaviour
{
	// Token: 0x06000519 RID: 1305
	public abstract void Init(CharacterSpawnerRoot root);

	// Token: 0x0600051A RID: 1306
	public abstract void StartSpawn();
}

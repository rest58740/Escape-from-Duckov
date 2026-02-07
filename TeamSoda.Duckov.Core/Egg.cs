using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class Egg : MonoBehaviour
{
	// Token: 0x06000578 RID: 1400 RVA: 0x00018A02 File Offset: 0x00016C02
	private void Start()
	{
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00018A04 File Offset: 0x00016C04
	public void Init(Vector3 spawnPosition, Vector3 spawnVelocity, CharacterMainControl _fromCharacter, CharacterRandomPreset preset, float _life)
	{
		this.characterPreset = preset;
		base.transform.position = spawnPosition;
		if (this.rb)
		{
			this.rb.position = spawnPosition;
			this.rb.velocity = spawnVelocity;
		}
		this.fromCharacter = _fromCharacter;
		this.life = _life;
		this.inited = true;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00018A60 File Offset: 0x00016C60
	private UniTaskVoid Spawn()
	{
		Egg.<Spawn>d__10 <Spawn>d__;
		<Spawn>d__.<>t__builder = AsyncUniTaskVoidMethodBuilder.Create();
		<Spawn>d__.<>4__this = this;
		<Spawn>d__.<>1__state = -1;
		<Spawn>d__.<>t__builder.Start<Egg.<Spawn>d__10>(ref <Spawn>d__);
		return <Spawn>d__.<>t__builder.Task;
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00018AA4 File Offset: 0x00016CA4
	private void Update()
	{
		if (!this.inited)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer > this.life && !this.spawned)
		{
			this.spawned = true;
			this.Spawn().Forget();
		}
	}

	// Token: 0x040004F5 RID: 1269
	public GameObject spawnFx;

	// Token: 0x040004F6 RID: 1270
	public CharacterMainControl fromCharacter;

	// Token: 0x040004F7 RID: 1271
	public Rigidbody rb;

	// Token: 0x040004F8 RID: 1272
	private float life;

	// Token: 0x040004F9 RID: 1273
	private CharacterRandomPreset characterPreset;

	// Token: 0x040004FA RID: 1274
	private bool inited;

	// Token: 0x040004FB RID: 1275
	private float timer;

	// Token: 0x040004FC RID: 1276
	private bool spawned;
}

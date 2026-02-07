using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Duckov;
using ItemStatsSystem;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000070 RID: 112
public class Grenade : MonoBehaviour
{
	// Token: 0x170000FA RID: 250
	// (get) Token: 0x06000442 RID: 1090 RVA: 0x00012DF9 File Offset: 0x00010FF9
	private bool needCustomFx
	{
		get
		{
			return this.fxType == ExplosionFxTypes.custom;
		}
	}

	// Token: 0x06000443 RID: 1091 RVA: 0x00012E04 File Offset: 0x00011004
	private void OnCollisionEnter(Collision collision)
	{
		if (!this.collide)
		{
			this.collide = true;
		}
		Vector3 velocity = this.rb.velocity;
		velocity.x *= 0.5f;
		velocity.z *= 0.5f;
		this.rb.velocity = velocity;
		this.rb.angularVelocity = this.rb.angularVelocity * 0.3f;
		if (this.makeSoundCount > 0 && Time.time - this.makeSoundTimeMarker > 0.3f)
		{
			this.makeSoundCount--;
			this.makeSoundTimeMarker = Time.time;
			AISound sound = default(AISound);
			sound.fromObject = base.gameObject;
			sound.pos = base.transform.position;
			if (this.damageInfo.fromCharacter)
			{
				sound.fromTeam = this.damageInfo.fromCharacter.Team;
			}
			else
			{
				sound.fromTeam = Teams.all;
			}
			sound.soundType = SoundTypes.unknowNoise;
			if (this.isDangerForAi)
			{
				sound.soundType = SoundTypes.grenadeDropSound;
			}
			sound.radius = 20f;
			AIMainBrain.MakeSound(sound);
			if (this.hasCollideSound && this.collideSound != "")
			{
				AudioManager.Post(this.collideSound, base.gameObject);
			}
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00012F64 File Offset: 0x00011164
	public void BindAgent(ItemAgent _agent)
	{
		this.bindAgent = true;
		this.bindedAgent = _agent;
		this.bindedAgent.transform.SetParent(base.transform, false);
		this.bindedAgent.transform.localPosition = Vector3.zero;
		this.bindedAgent.gameObject.SetActive(false);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x00012FBC File Offset: 0x000111BC
	private void Update()
	{
		this.lifeTimer += Time.deltaTime;
		if (!this.delayFromCollide || this.collide)
		{
			this.delayTimer += Time.deltaTime;
		}
		if (!this.bindAgent)
		{
			if (!this.exploded && this.delayTimer > this.delayTime)
			{
				this.exploded = true;
				if (!this.isLandmine)
				{
					this.Explode();
					return;
				}
				this.ActiveLandmine().Forget();
			}
			return;
		}
		if (this.bindedAgent == null)
		{
			Debug.Log("bind  null destroied");
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.lifeTimer > 0.5f && !this.bindedAgent.gameObject.activeInHierarchy)
		{
			this.bindedAgent.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x00013094 File Offset: 0x00011294
	private void Explode()
	{
		if (this.createExplosion)
		{
			this.damageInfo.isExplosion = true;
			LevelManager.Instance.ExplosionManager.CreateExplosion(base.transform.position, this.damageRange, this.damageInfo, this.fxType, this.explosionShakeStrength, this.canHurtSelf);
		}
		if (this.createExplosion && this.needCustomFx && this.fx != null)
		{
			Object.Instantiate<GameObject>(this.fx, base.transform.position, Quaternion.identity);
		}
		if (this.createOnExlode)
		{
			Object.Instantiate<GameObject>(this.createOnExlode, base.transform.position, Quaternion.identity);
		}
		UnityEvent unityEvent = this.onExplodeEvent;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		if (this.rb != null)
		{
			this.rb.constraints = 10;
		}
		if (this.destroyDelay <= 0f)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (this.destroyDelay < 999f)
		{
			this.DestroyOverTime().Forget();
		}
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x000131B0 File Offset: 0x000113B0
	private UniTask DestroyOverTime()
	{
		Grenade.<DestroyOverTime>d__37 <DestroyOverTime>d__;
		<DestroyOverTime>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<DestroyOverTime>d__.<>4__this = this;
		<DestroyOverTime>d__.<>1__state = -1;
		<DestroyOverTime>d__.<>t__builder.Start<Grenade.<DestroyOverTime>d__37>(ref <DestroyOverTime>d__);
		return <DestroyOverTime>d__.<>t__builder.Task;
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x000131F3 File Offset: 0x000113F3
	private void OnDestroy()
	{
		this.destroied = true;
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x000131FC File Offset: 0x000113FC
	private UniTask ActiveLandmine()
	{
		Grenade.<ActiveLandmine>d__40 <ActiveLandmine>d__;
		<ActiveLandmine>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<ActiveLandmine>d__.<>4__this = this;
		<ActiveLandmine>d__.<>1__state = -1;
		<ActiveLandmine>d__.<>t__builder.Start<Grenade.<ActiveLandmine>d__40>(ref <ActiveLandmine>d__);
		return <ActiveLandmine>d__.<>t__builder.Task;
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0001323F File Offset: 0x0001143F
	private void OnLinemineTriggerd()
	{
		if (this.landmineTriggerd)
		{
			return;
		}
		this.landmineTriggerd = true;
		this.Explode();
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x00013257 File Offset: 0x00011457
	public void SetWeaponIdInfo(int typeId)
	{
		this.damageInfo.fromWeaponItemID = typeId;
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x00013268 File Offset: 0x00011468
	public void Launch(Vector3 startPoint, Vector3 velocity, CharacterMainControl fromCharacter, bool canHurtSelf)
	{
		this.canHurtSelf = canHurtSelf;
		this.groundLayer = LayerMask.NameToLayer("Ground");
		this.rb.position = startPoint;
		base.transform.position = startPoint;
		this.rb.velocity = velocity;
		Vector3 angularVelocity = (Random.insideUnitSphere + Vector3.one) * 7f;
		angularVelocity.y = 0f;
		this.rb.angularVelocity = angularVelocity;
		if (fromCharacter != null)
		{
			Collider component = fromCharacter.GetComponent<Collider>();
			Collider component2 = base.GetComponent<Collider>();
			this.selfTeam = fromCharacter.Team;
			this.IgnoreCollisionForSeconds(component, component2, 0.5f).Forget();
		}
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x00013318 File Offset: 0x00011518
	private UniTask IgnoreCollisionForSeconds(Collider col1, Collider col2, float ignoreTime)
	{
		Grenade.<IgnoreCollisionForSeconds>d__44 <IgnoreCollisionForSeconds>d__;
		<IgnoreCollisionForSeconds>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<IgnoreCollisionForSeconds>d__.col1 = col1;
		<IgnoreCollisionForSeconds>d__.col2 = col2;
		<IgnoreCollisionForSeconds>d__.ignoreTime = ignoreTime;
		<IgnoreCollisionForSeconds>d__.<>1__state = -1;
		<IgnoreCollisionForSeconds>d__.<>t__builder.Start<Grenade.<IgnoreCollisionForSeconds>d__44>(ref <IgnoreCollisionForSeconds>d__);
		return <IgnoreCollisionForSeconds>d__.<>t__builder.Task;
	}

	// Token: 0x04000349 RID: 841
	public bool hasCollideSound;

	// Token: 0x0400034A RID: 842
	public string collideSound;

	// Token: 0x0400034B RID: 843
	public int makeSoundCount = 3;

	// Token: 0x0400034C RID: 844
	private float makeSoundTimeMarker = -1f;

	// Token: 0x0400034D RID: 845
	public float damageRange;

	// Token: 0x0400034E RID: 846
	public bool isDangerForAi = true;

	// Token: 0x0400034F RID: 847
	public bool isLandmine;

	// Token: 0x04000350 RID: 848
	public float landmineTriggerRange;

	// Token: 0x04000351 RID: 849
	private bool landmineActived;

	// Token: 0x04000352 RID: 850
	private bool landmineTriggerd;

	// Token: 0x04000353 RID: 851
	public ExplosionFxTypes fxType;

	// Token: 0x04000354 RID: 852
	public GameObject fx;

	// Token: 0x04000355 RID: 853
	public Animator animator;

	// Token: 0x04000356 RID: 854
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000357 RID: 855
	private int groundLayer;

	// Token: 0x04000358 RID: 856
	public bool delayFromCollide;

	// Token: 0x04000359 RID: 857
	public float delayTime = 1f;

	// Token: 0x0400035A RID: 858
	public bool createExplosion = true;

	// Token: 0x0400035B RID: 859
	public float explosionShakeStrength = 1f;

	// Token: 0x0400035C RID: 860
	public DamageInfo damageInfo;

	// Token: 0x0400035D RID: 861
	private bool bindAgent;

	// Token: 0x0400035E RID: 862
	private ItemAgent bindedAgent;

	// Token: 0x0400035F RID: 863
	private float lifeTimer;

	// Token: 0x04000360 RID: 864
	private float delayTimer;

	// Token: 0x04000361 RID: 865
	private Teams selfTeam;

	// Token: 0x04000362 RID: 866
	public GameObject createOnExlode;

	// Token: 0x04000363 RID: 867
	public float destroyDelay;

	// Token: 0x04000364 RID: 868
	public UnityEvent onExplodeEvent;

	// Token: 0x04000365 RID: 869
	private bool exploded;

	// Token: 0x04000366 RID: 870
	private bool canHurtSelf;

	// Token: 0x04000367 RID: 871
	private bool collide;

	// Token: 0x04000368 RID: 872
	private bool destroied;
}

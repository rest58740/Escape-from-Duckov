using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

// Token: 0x02000071 RID: 113
public class Projectile : MonoBehaviour
{
	// Token: 0x0600044F RID: 1103 RVA: 0x000133A9 File Offset: 0x000115A9
	public void SetPool(ObjectPool<Projectile> _pool)
	{
		this.pool = _pool;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x000133B2 File Offset: 0x000115B2
	private void Release()
	{
		if (this.pool == null)
		{
			Debug.Log("Destroy");
			Object.Destroy(base.gameObject);
			return;
		}
		this.pool.Release(this);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x000133DE File Offset: 0x000115DE
	private void Awake()
	{
		if (!this.inited)
		{
			this.inited = true;
			this.Init();
		}
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x000133F8 File Offset: 0x000115F8
	public void Init()
	{
		this.inited = true;
		this.damagedObjects = new List<GameObject>();
		this.damagedObjects.Clear();
		this.traveledDistance = 0f;
		this.dead = false;
		this.overMaxDistance = false;
		this.flyThroughCharacterSoundPlayed = false;
		this.firstFrame = true;
		this.traceLerpValue = 0f;
		this.hitLayers = (GameplayDataSettings.Layers.damageReceiverLayerMask | GameplayDataSettings.Layers.wallLayerMask | GameplayDataSettings.Layers.groundLayerMask);
		if (this.trail)
		{
			this.trail.Clear();
		}
		if (this.randomRotate)
		{
			this.randomRotate.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
		}
		if (this.random)
		{
			Color color = this.colors.GetRandom<Color>();
			if (this.mesh != null)
			{
				this.mesh.material.SetColor(this.colorName, color);
			}
			if (this.randomRotate)
			{
				this.randomRotate.localRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
			}
			if (this.trail != null)
			{
				color.a = 1f;
				this.trail.startColor = color;
				color.a = 0f;
				this.trail.endColor = color;
			}
		}
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x000135A0 File Offset: 0x000117A0
	private void Update()
	{
		if (this.dead)
		{
			this.Release();
			return;
		}
		this.UpdateMoveAndCheck();
		if (this.dead)
		{
			if (this.firstFrame && this.trail)
			{
				this.trail.Clear();
			}
			if (this.context.explosionRange > 0f)
			{
				DamageInfo dmgInfo = new DamageInfo(this.context.fromCharacter);
				dmgInfo.damageValue = this.context.explosionDamage;
				dmgInfo.fromWeaponItemID = this.context.fromWeaponItemID;
				dmgInfo.armorPiercing = this.context.armorPiercing;
				LevelManager.Instance.ExplosionManager.CreateExplosion(base.transform.position, this.context.explosionRange, dmgInfo, ExplosionFxTypes.normal, 1f, true);
			}
			this.Release();
		}
		this.UpdateFlyThroughSound();
		this.firstFrame = false;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x00013688 File Offset: 0x00011888
	private void UpdateFlyThroughSound()
	{
		if (this.dead)
		{
			return;
		}
		if (this.context.team == Teams.player)
		{
			return;
		}
		if (this.flyThroughCharacterSoundPlayed)
		{
			return;
		}
		if (CharacterMainControl.Main == null)
		{
			return;
		}
		if (this.velocity.magnitude < 9f)
		{
			return;
		}
		Vector3 vector = CharacterMainControl.Main.transform.position - base.transform.position;
		vector.y = 0f;
		if (vector.magnitude > 5f)
		{
			return;
		}
		vector.Normalize();
		if (Vector3.Dot(vector, this.velocity) > 0f)
		{
			return;
		}
		this.flyThroughCharacterSoundPlayed = true;
		Action<Vector3> onBulletFlyByCharacter = Projectile.OnBulletFlyByCharacter;
		if (onBulletFlyByCharacter == null)
		{
			return;
		}
		onBulletFlyByCharacter(base.transform.position);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x00013750 File Offset: 0x00011950
	public void Init(ProjectileContext _context)
	{
		this.Init();
		this.context = _context;
		if (this.context.traceTarget != null)
		{
			this.context.ignoreHalfObsticle = true;
			this.context.critRate = 1f;
		}
		this.direction = this.context.direction;
		this.velocity = this.context.speed * this.direction;
		this.gravity = Mathf.Abs(this.context.gravity);
		this.UpdateAimDirection();
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x000137E4 File Offset: 0x000119E4
	private void UpdateMoveAndCheck()
	{
		if (this.firstFrame)
		{
			this.startPoint = base.transform.position;
		}
		float num = Time.deltaTime;
		if (num > 0.04f)
		{
			num = 0.04f;
		}
		this.traceLerpValue = Mathf.MoveTowards(this.traceLerpValue, 1f, this.context.traceAbility * num);
		if (this.context.traceTarget != null)
		{
			Vector3 vector = this.context.traceTarget.characterModel.HelmatSocket.position - base.transform.position;
			vector.Normalize();
			this.direction = Vector3.Lerp(this.context.direction, vector, this.traceLerpValue);
		}
		else
		{
			this.velocity.y = this.velocity.y - num * this.gravity;
			this.direction = this.velocity.normalized;
		}
		this.UpdateAimDirection();
		this._distanceThisFrame = this.velocity.magnitude * num;
		if (this._distanceThisFrame + this.traveledDistance > this.context.distance)
		{
			this._distanceThisFrame = this.context.distance - this.traveledDistance;
			this.overMaxDistance = true;
		}
		Vector3 vector2 = base.transform.position - base.transform.forward * 0.1f;
		if (this.firstFrame && this.context.firstFrameCheck)
		{
			vector2 = this.context.firstFrameCheckStartPoint;
		}
		this.hits = Physics.SphereCastAll(vector2, this.radius, this.direction, this._distanceThisFrame + 0.3f, this.hitLayers, 1).ToList<RaycastHit>();
		int count = this.hits.Count;
		if (count > 0)
		{
			this.hits.Sort(delegate(RaycastHit a, RaycastHit b)
			{
				float num2 = a.distance - b.distance;
				if (num2 > 0f)
				{
					return 1;
				}
				if (num2 < 0f)
				{
					return -1;
				}
				return 0;
			});
			for (int i = 0; i < count; i++)
			{
				RaycastHit raycastHit = this.hits[i];
				this.hitPoint = raycastHit.point;
				if (raycastHit.distance <= 0f)
				{
					this.hitPoint = raycastHit.collider.transform.position;
				}
				if (!this.damagedObjects.Contains(this.hits[i].collider.gameObject) && (!this.context.ignoreHalfObsticle || !GameplayDataSettings.LayersData.IsLayerInLayerMask(this.hits[i].collider.gameObject.layer, GameplayDataSettings.Layers.halfObsticleLayer)))
				{
					this.damagedObjects.Add(this.hits[i].collider.gameObject);
					if ((GameplayDataSettings.Layers.damageReceiverLayerMask & 1 << this.hits[i].collider.gameObject.layer) != 0)
					{
						this._dmgReceiverTemp = this.hits[i].collider.GetComponent<DamageReceiver>();
						if (this._dmgReceiverTemp.Team == this.context.team)
						{
							goto IL_67F;
						}
						if (this._dmgReceiverTemp.isHalfObsticle && this.context.ignoreHalfObsticle)
						{
							goto IL_67F;
						}
					}
					else
					{
						this._dmgReceiverTemp = null;
					}
					if (this._dmgReceiverTemp)
					{
						bool flag = true;
						if (this._dmgReceiverTemp.Team == this.context.team)
						{
							flag = false;
						}
						else if (this._dmgReceiverTemp.health)
						{
							CharacterMainControl characterMainControl = this._dmgReceiverTemp.health.TryGetCharacter();
							if (characterMainControl && this._dmgReceiverTemp.health.TryGetCharacter().Dashing)
							{
								flag = false;
							}
							else if (characterMainControl && characterMainControl == this.context.fromCharacter)
							{
								flag = false;
							}
						}
						if (flag)
						{
							DamageInfo damageInfo = new DamageInfo(this.context.fromCharacter);
							damageInfo.damageValue = this.context.damage;
							if (this.context.halfDamageDistance > 0f && Vector3.Distance(this.startPoint, this.hitPoint) > this.context.halfDamageDistance)
							{
								damageInfo.damageValue *= 0.5f;
							}
							damageInfo.critDamageFactor = this.context.critDamageFactor;
							damageInfo.critRate = this.context.critRate;
							damageInfo.armorPiercing = this.context.armorPiercing;
							damageInfo.armorBreak = this.context.armorBreak;
							damageInfo.AddElementFactor(ElementTypes.physics, this.context.element_Physics);
							damageInfo.AddElementFactor(ElementTypes.fire, this.context.element_Fire);
							damageInfo.AddElementFactor(ElementTypes.poison, this.context.element_Poison);
							damageInfo.AddElementFactor(ElementTypes.electricity, this.context.element_Electricity);
							damageInfo.AddElementFactor(ElementTypes.space, this.context.element_Space);
							damageInfo.AddElementFactor(ElementTypes.ghost, this.context.element_Ghost);
							damageInfo.AddElementFactor(ElementTypes.ice, this.context.element_Ice);
							damageInfo.damagePoint = this.hitPoint;
							damageInfo.buffChance = this.context.buffChance;
							damageInfo.buff = this.context.buff;
							damageInfo.bleedChance = this.context.bleedChance;
							damageInfo.damageType = DamageTypes.normal;
							damageInfo.fromWeaponItemID = this.context.fromWeaponItemID;
							damageInfo.damageNormal = raycastHit.normal.normalized;
							this._dmgReceiverTemp.Hurt(damageInfo);
							this._dmgReceiverTemp.AddBuff(GameplayDataSettings.Buffs.Pain, this.context.fromCharacter);
							this.context.penetrate = this.context.penetrate - 1;
							if (this.context.penetrate < 0)
							{
								base.transform.position = this.hitPoint;
								this.dead = true;
								break;
							}
						}
					}
					else
					{
						this.dead = true;
						base.transform.position = this.hitPoint;
						Vector3 normal = raycastHit.normal;
						if (this.hitFx)
						{
							Object.Instantiate<GameObject>(this.hitFx, this.hitPoint, Quaternion.LookRotation(normal, Vector3.up));
							break;
						}
						Object.Instantiate<GameObject>(GameplayDataSettings.Prefabs.BulletHitObsticleFx, this.hitPoint, Quaternion.LookRotation(normal, Vector3.up));
						break;
					}
				}
				IL_67F:;
			}
		}
		if (this.overMaxDistance)
		{
			this.dead = true;
		}
		if (!this.dead)
		{
			base.transform.position += this.direction * this._distanceThisFrame;
			this.traveledDistance += this._distanceThisFrame;
		}
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00013ECF File Offset: 0x000120CF
	private void UpdateAimDirection()
	{
		base.transform.rotation = Quaternion.LookRotation(this.direction, Vector3.up);
	}

	// Token: 0x04000369 RID: 873
	public ProjectileContext context;

	// Token: 0x0400036A RID: 874
	public float radius;

	// Token: 0x0400036B RID: 875
	private float traveledDistance;

	// Token: 0x0400036C RID: 876
	private List<RaycastHit> hits;

	// Token: 0x0400036D RID: 877
	private LayerMask hitLayers;

	// Token: 0x0400036E RID: 878
	private Vector3 hitPoint;

	// Token: 0x0400036F RID: 879
	private Vector3 hitNormal;

	// Token: 0x04000370 RID: 880
	private bool dead;

	// Token: 0x04000371 RID: 881
	private bool overMaxDistance;

	// Token: 0x04000372 RID: 882
	[SerializeField]
	private GameObject hitFx;

	// Token: 0x04000373 RID: 883
	private Vector3 direction;

	// Token: 0x04000374 RID: 884
	private Vector3 velocity;

	// Token: 0x04000375 RID: 885
	private float gravity;

	// Token: 0x04000376 RID: 886
	[HideInInspector]
	public List<GameObject> damagedObjects;

	// Token: 0x04000377 RID: 887
	public static Action<Vector3> OnBulletFlyByCharacter;

	// Token: 0x04000378 RID: 888
	private bool flyThroughCharacterSoundPlayed;

	// Token: 0x04000379 RID: 889
	private bool firstFrame = true;

	// Token: 0x0400037A RID: 890
	private Vector3 startPoint;

	// Token: 0x0400037B RID: 891
	private ObjectPool<Projectile> pool;

	// Token: 0x0400037C RID: 892
	[SerializeField]
	private TrailRenderer trail;

	// Token: 0x0400037D RID: 893
	[FormerlySerializedAs("spin")]
	public Transform randomRotate;

	// Token: 0x0400037E RID: 894
	public bool random;

	// Token: 0x0400037F RID: 895
	public MeshRenderer mesh;

	// Token: 0x04000380 RID: 896
	public string colorName = "_Tint";

	// Token: 0x04000381 RID: 897
	[ColorUsage(true, true)]
	public List<Color> colors = new List<Color>();

	// Token: 0x04000382 RID: 898
	private float traceLerpValue;

	// Token: 0x04000383 RID: 899
	private bool inited;

	// Token: 0x04000384 RID: 900
	private DamageReceiver _dmgReceiverTemp;

	// Token: 0x04000385 RID: 901
	private float _distanceThisFrame;

	// Token: 0x04000386 RID: 902
	private int _hitCount;
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000067 RID: 103
public class HurtVisual : MonoBehaviour
{
	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060003E3 RID: 995 RVA: 0x00011483 File Offset: 0x0000F683
	public GameObject HitFx
	{
		get
		{
			if (!GameManager.BloodFxOn && this.hitFX_NoBlood != null)
			{
				return this.hitFX_NoBlood;
			}
			return this.hitFX;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060003E4 RID: 996 RVA: 0x000114A7 File Offset: 0x0000F6A7
	public GameObject DeadFx
	{
		get
		{
			if (!GameManager.BloodFxOn && this.deadFx_NoBlood != null)
			{
				return this.deadFx_NoBlood;
			}
			return this.deadFx;
		}
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x000114CC File Offset: 0x0000F6CC
	public void SetHealth(Health _health)
	{
		if (this.useSimpleHealth)
		{
			return;
		}
		if (this.health != null)
		{
			this.health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
		this.health = _health;
		_health.OnHurtEvent.AddListener(new UnityAction<DamageInfo>(this.OnHurt));
		_health.OnDeadEvent.AddListener(new UnityAction<DamageInfo>(this.OnDead));
		this.Init();
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00011564 File Offset: 0x0000F764
	private void Awake()
	{
		if (this.useSimpleHealth && this.simpleHealth != null)
		{
			this.simpleHealth.OnHurtEvent += this.OnHurt;
			this.simpleHealth.OnDeadEvent += this.OnDead;
		}
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x000115B5 File Offset: 0x0000F7B5
	private void Init()
	{
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x000115B8 File Offset: 0x0000F7B8
	private void Update()
	{
		if (this.hurtValue > 0f)
		{
			this.SetRendererValue(this.hurtValue);
			this.hurtValue -= Time.unscaledDeltaTime * this.hurtCoolSpeed;
			if (this.hurtValue <= 0f)
			{
				this.SetRendererValue(0f);
			}
		}
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001160F File Offset: 0x0000F80F
	public void ForceHurt()
	{
		this.hurtValue = 1f;
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001161C File Offset: 0x0000F81C
	private void OnHurt(DamageInfo dmgInfo)
	{
		bool flag = this.health && this.health.Hidden;
		if (this.HitFx && !flag)
		{
			PlayHurtEventProxy component = Object.Instantiate<GameObject>(this.HitFx, dmgInfo.damagePoint, Quaternion.LookRotation(dmgInfo.damageNormal)).GetComponent<PlayHurtEventProxy>();
			if (component)
			{
				component.Play(dmgInfo.crit > 0);
			}
		}
		this.hurtValue = 1f;
		this.SetRendererValue(this.hurtValue);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x000116A8 File Offset: 0x0000F8A8
	private void SetRendererValue(float value)
	{
		int count = this.renderers.Count;
		for (int i = 0; i < count; i++)
		{
			if (!(this.renderers[i] == null))
			{
				if (this.materialPropertyBlock == null)
				{
					this.materialPropertyBlock = new MaterialPropertyBlock();
				}
				this.renderers[i].GetPropertyBlock(this.materialPropertyBlock);
				this.materialPropertyBlock.SetFloat(HurtVisual.hurtHash, value * this.hurtValueMultiplier);
				this.renderers[i].SetPropertyBlock(this.materialPropertyBlock);
			}
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001173C File Offset: 0x0000F93C
	private void OnDead(DamageInfo dmgInfo)
	{
		if (this.DeadFx)
		{
			PlayHurtEventProxy component = Object.Instantiate<GameObject>(this.DeadFx, base.transform.position, base.transform.rotation).GetComponent<PlayHurtEventProxy>();
			if (component)
			{
				component.Play(dmgInfo.crit > 0);
			}
		}
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00011794 File Offset: 0x0000F994
	private void OnDestroy()
	{
		if (this.health)
		{
			this.health.OnHurtEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnHurt));
			this.health.OnDeadEvent.RemoveListener(new UnityAction<DamageInfo>(this.OnDead));
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x000117E6 File Offset: 0x0000F9E6
	private void AutoSet()
	{
		this.renderers = base.GetComponentsInChildren<Renderer>(true).ToList<Renderer>();
		this.renderers.RemoveAll((Renderer e) => e == null || e.GetComponent<ParticleSystem>() != null);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x00011825 File Offset: 0x0000FA25
	public void SetRenderers(List<Renderer> _renderers)
	{
		this.renderers = _renderers;
	}

	// Token: 0x040002FA RID: 762
	public bool useSimpleHealth;

	// Token: 0x040002FB RID: 763
	public HealthSimpleBase simpleHealth;

	// Token: 0x040002FC RID: 764
	private Health health;

	// Token: 0x040002FD RID: 765
	[SerializeField]
	private GameObject hitFX;

	// Token: 0x040002FE RID: 766
	[SerializeField]
	private GameObject hitFX_NoBlood;

	// Token: 0x040002FF RID: 767
	[SerializeField]
	private GameObject deadFx;

	// Token: 0x04000300 RID: 768
	[SerializeField]
	private GameObject deadFx_NoBlood;

	// Token: 0x04000301 RID: 769
	public List<Renderer> renderers;

	// Token: 0x04000302 RID: 770
	public static readonly int hurtHash = Shader.PropertyToID("_HurtValue");

	// Token: 0x04000303 RID: 771
	private MaterialPropertyBlock materialPropertyBlock;

	// Token: 0x04000304 RID: 772
	public float hurtCoolSpeed = 8f;

	// Token: 0x04000305 RID: 773
	public float hurtValueMultiplier = 1f;

	// Token: 0x04000306 RID: 774
	private float hurtValue;
}

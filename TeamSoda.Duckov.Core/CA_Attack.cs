using System;
using Duckov;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class CA_Attack : CharacterActionBase, IProgress
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x060001F5 RID: 501 RVA: 0x00009A2C File Offset: 0x00007C2C
	// (remove) Token: 0x060001F6 RID: 502 RVA: 0x00009A64 File Offset: 0x00007C64
	public event Action OnAttack;

	// Token: 0x17000068 RID: 104
	// (get) Token: 0x060001F7 RID: 503 RVA: 0x00009A99 File Offset: 0x00007C99
	public bool DamageDealed
	{
		get
		{
			return this.damageDealed;
		}
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00009AA1 File Offset: 0x00007CA1
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Attack;
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x00009AA4 File Offset: 0x00007CA4
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00009AA7 File Offset: 0x00007CA7
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x060001FB RID: 507 RVA: 0x00009AAA File Offset: 0x00007CAA
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x060001FC RID: 508 RVA: 0x00009AAD File Offset: 0x00007CAD
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00009AB0 File Offset: 0x00007CB0
	public Progress GetProgress()
	{
		Progress result = default(Progress);
		if (base.Running)
		{
			result.inProgress = true;
			result.total = this.attackActionTime;
			result.current = this.actionTimer;
		}
		else
		{
			result.inProgress = false;
		}
		return result;
	}

	// Token: 0x060001FE RID: 510 RVA: 0x00009AFC File Offset: 0x00007CFC
	public override bool IsReady()
	{
		if (Time.time - this.lastAttackTime < this.cd)
		{
			return false;
		}
		this.meleeWeapon = this.characterController.GetMeleeWeapon();
		return !(this.meleeWeapon == null) && this.meleeWeapon.StaminaCost <= this.characterController.CurrentStamina && !base.Running;
	}

	// Token: 0x060001FF RID: 511 RVA: 0x00009B64 File Offset: 0x00007D64
	protected override bool OnStart()
	{
		if (!this.characterController.CurrentHoldItemAgent)
		{
			return false;
		}
		this.meleeWeapon = this.characterController.GetMeleeWeapon();
		if (!this.meleeWeapon)
		{
			return false;
		}
		this.characterController.UseStamina(this.meleeWeapon.StaminaCost);
		this.dealDamageTime = this.meleeWeapon.DealDamageTime;
		this.damageDealed = false;
		Action onAttack = this.OnAttack;
		if (onAttack != null)
		{
			onAttack();
		}
		this.CreateAttackSound();
		this.HardwareEvent();
		this.lastAttackTime = Time.time;
		this.cd = 1f / this.meleeWeapon.AttackSpeed;
		this.slashFxDelayTime = this.meleeWeapon.slashFxDelayTime;
		this.slashFxSpawned = false;
		return true;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00009C2B File Offset: 0x00007E2B
	private void CreateAttackSound()
	{
		AudioManager.Post("SFX/Combat/Melee/attack_" + this.meleeWeapon.SoundKey.ToLower(), base.gameObject);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00009C53 File Offset: 0x00007E53
	private void HardwareEvent()
	{
		HardwareSyncingManager.SetEvent("Melee_Attack");
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00009C5F File Offset: 0x00007E5F
	protected override void OnStop()
	{
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00009C64 File Offset: 0x00007E64
	protected override void OnUpdateAction(float deltaTime)
	{
		if ((this.actionTimer > this.attackActionTime || !base.Running || this.meleeWeapon == null) && base.StopAction())
		{
			return;
		}
		if (!this.slashFxSpawned && base.ActionTimer > this.slashFxDelayTime && this.meleeWeapon && this.meleeWeapon.slashFx)
		{
			this.slashFxSpawned = true;
			Vector3 vector = this.characterController.transform.position + this.characterController.modelRoot.forward * this.characterController.characterModel.attackFxZOffset;
			vector.y = this.meleeWeapon.transform.position.y;
			Object.Instantiate<GameObject>(this.meleeWeapon.slashFx, vector, Quaternion.LookRotation(this.characterController.modelRoot.forward, Vector3.up)).transform.SetParent(base.transform);
		}
		if (!this.damageDealed && base.ActionTimer > this.dealDamageTime)
		{
			this.damageDealed = true;
			this.meleeWeapon.CheckAndDealDamage();
		}
	}

	// Token: 0x040001B3 RID: 435
	private float attackActionTime = 0.25f;

	// Token: 0x040001B4 RID: 436
	private ItemAgent_MeleeWeapon meleeWeapon;

	// Token: 0x040001B6 RID: 438
	private float dealDamageTime = 0.1f;

	// Token: 0x040001B7 RID: 439
	private bool damageDealed;

	// Token: 0x040001B8 RID: 440
	private float lastAttackTime = -999f;

	// Token: 0x040001B9 RID: 441
	private float cd = -1f;

	// Token: 0x040001BA RID: 442
	private bool slashFxSpawned;

	// Token: 0x040001BB RID: 443
	private float slashFxDelayTime;
}

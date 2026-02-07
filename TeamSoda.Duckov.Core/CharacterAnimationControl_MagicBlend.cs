using System;
using UnityEngine;

// Token: 0x02000059 RID: 89
public class CharacterAnimationControl_MagicBlend : MonoBehaviour
{
	// Token: 0x06000282 RID: 642 RVA: 0x0000B2D0 File Offset: 0x000094D0
	private void Awake()
	{
		if (!this.characterModel)
		{
			this.characterModel = base.GetComponent<CharacterModel>();
		}
		this.characterModel.OnCharacterSetEvent += this.OnCharacterSet;
		if (this.characterModel.characterMainControl)
		{
			this.characterMainControl = this.characterModel.characterMainControl;
		}
		this.characterModel.OnAttackOrShootEvent += this.OnAttack;
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0000B347 File Offset: 0x00009547
	private void OnDestroy()
	{
		if (this.characterModel)
		{
			this.characterModel.OnCharacterSetEvent -= this.OnCharacterSet;
			this.characterModel.OnAttackOrShootEvent -= this.OnAttack;
		}
	}

	// Token: 0x06000284 RID: 644 RVA: 0x0000B384 File Offset: 0x00009584
	private void OnCharacterSet()
	{
		this.characterMainControl = this.characterModel.characterMainControl;
	}

	// Token: 0x06000285 RID: 645 RVA: 0x0000B397 File Offset: 0x00009597
	private void Start()
	{
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
		this.animator.SetLayerWeight(this.attackLayer, 0f);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x0000B3D0 File Offset: 0x000095D0
	private void Update()
	{
		if (!this.characterMainControl)
		{
			return;
		}
		this.animator.SetFloat(this.hash_MoveSpeed, this.characterMainControl.AnimationMoveSpeedValue);
		Vector2 animationLocalMoveDirectionValue = this.characterMainControl.AnimationLocalMoveDirectionValue;
		this.animator.SetFloat(this.hash_MoveDirX, animationLocalMoveDirectionValue.x);
		this.animator.SetFloat(this.hash_MoveDirY, animationLocalMoveDirectionValue.y);
		int num = 0;
		if (!this.holdAgent || !this.holdAgent.isActiveAndEnabled)
		{
			this.holdAgent = this.characterMainControl.CurrentHoldItemAgent;
		}
		else
		{
			num = (int)this.holdAgent.handAnimationType;
		}
		if (this.characterMainControl.carryAction.Running)
		{
			num = -1;
		}
		this.animator.SetInteger(this.hash_HandState, num);
		if (this.holdAgent != null && this.gunAgent == null)
		{
			this.gunAgent = (this.holdAgent as ItemAgent_Gun);
		}
		bool flag = false;
		if (this.gunAgent != null)
		{
			flag = true;
			if (this.gunAgent.IsReloading() || this.gunAgent.BulletCount <= 0)
			{
				flag = false;
			}
		}
		this.animator.SetBool(this.hash_GunReady, flag);
		bool flag2 = this.characterMainControl.Dashing && !this.characterMainControl.DashCanControl;
		this.animator.SetBool(this.hash_Dashing, flag2);
		this.UpdateAttackLayerWeight();
	}

	// Token: 0x06000287 RID: 647 RVA: 0x0000B548 File Offset: 0x00009748
	private void UpdateAttackLayerWeight()
	{
		if (!this.attacking)
		{
			if (this.weight > 0f)
			{
				this.weight = 0f;
				this.animator.SetLayerWeight(this.attackLayer, this.weight);
			}
			return;
		}
		this.attackTimer += Time.deltaTime;
		this.weight = this.attackLayerWeightCurve.Evaluate(this.attackTimer / this.attackTime);
		if (this.attackTimer >= this.attackTime)
		{
			this.attacking = false;
			this.weight = 0f;
		}
		this.animator.SetLayerWeight(this.attackLayer, this.weight);
	}

	// Token: 0x06000288 RID: 648 RVA: 0x0000B5F4 File Offset: 0x000097F4
	public void OnAttack()
	{
		this.attacking = true;
		if (this.attackLayer < 0)
		{
			this.attackLayer = this.animator.GetLayerIndex("MeleeAttack");
		}
		this.animator.SetTrigger(this.hash_Attack);
		this.attackTimer = 0f;
	}

	// Token: 0x040001F6 RID: 502
	public CharacterMainControl characterMainControl;

	// Token: 0x040001F7 RID: 503
	public CharacterModel characterModel;

	// Token: 0x040001F8 RID: 504
	public Animator animator;

	// Token: 0x040001F9 RID: 505
	public float attackTime = 0.3f;

	// Token: 0x040001FA RID: 506
	private int attackLayer = -1;

	// Token: 0x040001FB RID: 507
	private bool attacking;

	// Token: 0x040001FC RID: 508
	private float attackTimer;

	// Token: 0x040001FD RID: 509
	private DuckovItemAgent holdAgent;

	// Token: 0x040001FE RID: 510
	private ItemAgent_Gun gunAgent;

	// Token: 0x040001FF RID: 511
	public AnimationCurve attackLayerWeightCurve;

	// Token: 0x04000200 RID: 512
	private int hash_MoveSpeed = Animator.StringToHash("MoveSpeed");

	// Token: 0x04000201 RID: 513
	private int hash_MoveDirX = Animator.StringToHash("MoveDirX");

	// Token: 0x04000202 RID: 514
	private int hash_MoveDirY = Animator.StringToHash("MoveDirY");

	// Token: 0x04000203 RID: 515
	private int hash_Dashing = Animator.StringToHash("Dashing");

	// Token: 0x04000204 RID: 516
	private int hash_Attack = Animator.StringToHash("Attack");

	// Token: 0x04000205 RID: 517
	private int hash_HandState = Animator.StringToHash("HandState");

	// Token: 0x04000206 RID: 518
	private int hash_GunReady = Animator.StringToHash("GunReady");

	// Token: 0x04000207 RID: 519
	private float weight;
}

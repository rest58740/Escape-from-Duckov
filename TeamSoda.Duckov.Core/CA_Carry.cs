using System;
using Duckov;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class CA_Carry : CharacterActionBase, IProgress
{
	// Token: 0x06000205 RID: 517 RVA: 0x00009DD4 File Offset: 0x00007FD4
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Whatever;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00009DD7 File Offset: 0x00007FD7
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00009DDA File Offset: 0x00007FDA
	public override bool CanRun()
	{
		return false;
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00009DDD File Offset: 0x00007FDD
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00009DE0 File Offset: 0x00007FE0
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00009DE3 File Offset: 0x00007FE3
	public override bool IsReady()
	{
		return this.carryTarget != null;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x00009DF1 File Offset: 0x00007FF1
	public float GetWeight()
	{
		if (!base.Running)
		{
			return 0f;
		}
		if (!this.carringTarget)
		{
			return 0f;
		}
		return this.carringTarget.GetWeight();
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00009E20 File Offset: 0x00008020
	public Progress GetProgress()
	{
		return new Progress
		{
			inProgress = false,
			total = 1f,
			current = 1f
		};
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00009E56 File Offset: 0x00008056
	protected override bool OnStart()
	{
		this.characterController.ChangeHoldItem(null);
		this.carryTarget.Take(this);
		this.carringTarget = this.carryTarget;
		return true;
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00009E7E File Offset: 0x0000807E
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.characterController.CurrentHoldItemAgent != null)
		{
			base.StopAction();
		}
		if (this.carryTarget)
		{
			this.carryTarget.OnCarriableUpdate(deltaTime);
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00009EB3 File Offset: 0x000080B3
	protected override void OnStop()
	{
		this.carryTarget.Drop();
		this.carringTarget = null;
	}

	// Token: 0x040001BC RID: 444
	[HideInInspector]
	public Carriable carryTarget;

	// Token: 0x040001BD RID: 445
	private Carriable carringTarget;

	// Token: 0x040001BE RID: 446
	public Vector3 carryPoint = new Vector3(0f, 1f, 0.8f);
}

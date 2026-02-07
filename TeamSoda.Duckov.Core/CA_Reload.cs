using System;
using Duckov;
using ItemStatsSystem;

// Token: 0x02000053 RID: 83
public class CA_Reload : CharacterActionBase, IProgress
{
	// Token: 0x06000230 RID: 560 RVA: 0x0000A4D0 File Offset: 0x000086D0
	public override CharacterActionBase.ActionPriorities ActionPriority()
	{
		return CharacterActionBase.ActionPriorities.Reload;
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000A4D3 File Offset: 0x000086D3
	public override bool CanMove()
	{
		return true;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000A4D6 File Offset: 0x000086D6
	public override bool CanRun()
	{
		return true;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000A4D9 File Offset: 0x000086D9
	public override bool CanUseHand()
	{
		return false;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000A4DC File Offset: 0x000086DC
	public override bool CanControlAim()
	{
		return true;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000A4DF File Offset: 0x000086DF
	public override bool IsReady()
	{
		this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
		return this.currentGun && !this.currentGun.IsReloading();
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000A518 File Offset: 0x00008718
	protected override bool OnStart()
	{
		this.currentGun = null;
		if (!this.characterController || !this.characterController.CurrentHoldItemAgent)
		{
			return false;
		}
		this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
		this.currentGun.GunItemSetting.PreferdBulletsToLoad = this.preferedBulletToReload;
		this.preferedBulletToReload = null;
		if (this.currentGun != null && this.currentGun.BeginReload())
		{
			if (this.characterController.IsMainCharacter)
			{
				HardwareSyncingManager.SetEvent("Reload");
			}
			return true;
		}
		return false;
	}

	// Token: 0x06000237 RID: 567 RVA: 0x0000A5B5 File Offset: 0x000087B5
	protected override void OnStop()
	{
		if (this.currentGun != null)
		{
			this.currentGun.CancleReload();
		}
	}

	// Token: 0x06000238 RID: 568 RVA: 0x0000A5D0 File Offset: 0x000087D0
	public bool GetGunReloadable()
	{
		if (this.currentGun == null)
		{
			this.currentGun = this.characterController.agentHolder.CurrentHoldGun;
			return false;
		}
		return !base.Running && !this.currentGun.IsFull();
	}

	// Token: 0x06000239 RID: 569 RVA: 0x0000A61F File Offset: 0x0000881F
	public override bool CanEditInventory()
	{
		return true;
	}

	// Token: 0x0600023A RID: 570 RVA: 0x0000A622 File Offset: 0x00008822
	protected override void OnUpdateAction(float deltaTime)
	{
		if (this.currentGun == null)
		{
			base.StopAction();
			return;
		}
		if (!this.currentGun.IsReloading())
		{
			base.StopAction();
		}
	}

	// Token: 0x0600023B RID: 571 RVA: 0x0000A650 File Offset: 0x00008850
	public Progress GetProgress()
	{
		if (this.currentGun != null)
		{
			return this.currentGun.GetReloadProgress();
		}
		return new Progress
		{
			inProgress = false
		};
	}

	// Token: 0x040001CF RID: 463
	public ItemAgent_Gun currentGun;

	// Token: 0x040001D0 RID: 464
	public Item preferedBulletToReload;
}

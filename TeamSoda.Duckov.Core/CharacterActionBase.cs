using System;
using UnityEngine;

// Token: 0x02000057 RID: 87
public abstract class CharacterActionBase : MonoBehaviour
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x06000263 RID: 611 RVA: 0x0000AD8B File Offset: 0x00008F8B
	public bool Running
	{
		get
		{
			return this.running;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x06000264 RID: 612 RVA: 0x0000AD93 File Offset: 0x00008F93
	public float ActionTimer
	{
		get
		{
			return this.actionTimer;
		}
	}

	// Token: 0x06000265 RID: 613
	public abstract CharacterActionBase.ActionPriorities ActionPriority();

	// Token: 0x06000266 RID: 614
	public abstract bool CanMove();

	// Token: 0x06000267 RID: 615
	public abstract bool CanRun();

	// Token: 0x06000268 RID: 616
	public abstract bool CanUseHand();

	// Token: 0x06000269 RID: 617
	public abstract bool CanControlAim();

	// Token: 0x0600026A RID: 618 RVA: 0x0000AD9B File Offset: 0x00008F9B
	public virtual bool CanEditInventory()
	{
		return false;
	}

	// Token: 0x0600026B RID: 619 RVA: 0x0000AD9E File Offset: 0x00008F9E
	public void UpdateAction(float deltaTime)
	{
		this.actionTimer += deltaTime;
		this.OnUpdateAction(deltaTime);
	}

	// Token: 0x0600026C RID: 620 RVA: 0x0000ADB5 File Offset: 0x00008FB5
	protected virtual void OnUpdateAction(float deltaTime)
	{
	}

	// Token: 0x0600026D RID: 621 RVA: 0x0000ADB7 File Offset: 0x00008FB7
	protected virtual bool OnStart()
	{
		return true;
	}

	// Token: 0x0600026E RID: 622 RVA: 0x0000ADBA File Offset: 0x00008FBA
	public virtual bool IsStopable()
	{
		return true;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x0000ADBD File Offset: 0x00008FBD
	protected virtual void OnStop()
	{
	}

	// Token: 0x06000270 RID: 624
	public abstract bool IsReady();

	// Token: 0x06000271 RID: 625 RVA: 0x0000ADBF File Offset: 0x00008FBF
	public bool StartActionByCharacter(CharacterMainControl _character)
	{
		if (!this.IsReady())
		{
			return false;
		}
		this.characterController = _character;
		if (this.OnStart())
		{
			this.actionTimer = 0f;
			this.running = true;
			return true;
		}
		return false;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x0000ADEF File Offset: 0x00008FEF
	public bool StopAction()
	{
		if (!this.running)
		{
			return true;
		}
		if (this.IsStopable())
		{
			this.running = false;
			this.OnStop();
			return true;
		}
		return false;
	}

	// Token: 0x040001DF RID: 479
	private bool running;

	// Token: 0x040001E0 RID: 480
	protected float actionTimer;

	// Token: 0x040001E1 RID: 481
	public bool progressHUD = true;

	// Token: 0x040001E2 RID: 482
	public CharacterMainControl characterController;

	// Token: 0x0200044B RID: 1099
	public enum ActionPriorities
	{
		// Token: 0x04001AE2 RID: 6882
		Whatever,
		// Token: 0x04001AE3 RID: 6883
		Reload,
		// Token: 0x04001AE4 RID: 6884
		Attack,
		// Token: 0x04001AE5 RID: 6885
		usingItem,
		// Token: 0x04001AE6 RID: 6886
		Dash,
		// Token: 0x04001AE7 RID: 6887
		Skills,
		// Token: 0x04001AE8 RID: 6888
		Fishing,
		// Token: 0x04001AE9 RID: 6889
		Interact
	}
}

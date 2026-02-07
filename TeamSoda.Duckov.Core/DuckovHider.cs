using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using FOW;

// Token: 0x02000181 RID: 385
public class DuckovHider : HiderBehavior
{
	// Token: 0x06000BE3 RID: 3043 RVA: 0x00032BEF File Offset: 0x00030DEF
	protected override void Awake()
	{
		base.Awake();
		LevelManager.OnMainCharacterDead += this.OnMainCharacterDie;
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x00032C08 File Offset: 0x00030E08
	private void OnDestroy()
	{
		LevelManager.OnMainCharacterDead -= this.OnMainCharacterDie;
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00032C1B File Offset: 0x00030E1B
	protected override void OnHide()
	{
		if (!LevelManager.Instance || !LevelManager.Instance.IsRaidMap || this.mainCharacterDied)
		{
			return;
		}
		this.targetHide = true;
		this.HideDelay();
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x00032C4C File Offset: 0x00030E4C
	protected override void OnReveal()
	{
		this.targetHide = false;
		this.character.Show();
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x00032C60 File Offset: 0x00030E60
	private UniTask HideDelay()
	{
		DuckovHider.<HideDelay>d__8 <HideDelay>d__;
		<HideDelay>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<HideDelay>d__.<>4__this = this;
		<HideDelay>d__.<>1__state = -1;
		<HideDelay>d__.<>t__builder.Start<DuckovHider.<HideDelay>d__8>(ref <HideDelay>d__);
		return <HideDelay>d__.<>t__builder.Task;
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x00032CA3 File Offset: 0x00030EA3
	private void OnMainCharacterDie(DamageInfo damageInfo)
	{
		this.mainCharacterDied = true;
		this.OnReveal();
	}

	// Token: 0x04000A2E RID: 2606
	public CharacterMainControl character;

	// Token: 0x04000A2F RID: 2607
	private float hideDelay = 0.2f;

	// Token: 0x04000A30 RID: 2608
	private bool targetHide;

	// Token: 0x04000A31 RID: 2609
	private bool mainCharacterDied;
}

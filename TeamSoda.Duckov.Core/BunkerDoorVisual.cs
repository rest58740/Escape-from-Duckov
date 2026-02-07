using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using SodaCraft.Localizations;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class BunkerDoorVisual : MonoBehaviour
{
	// Token: 0x06000BCC RID: 3020 RVA: 0x0003276C File Offset: 0x0003096C
	private void Awake()
	{
		this.animator.SetBool("InRange", this.inRange);
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x00032784 File Offset: 0x00030984
	public void OnEnter()
	{
		if (this.inRange)
		{
			return;
		}
		this.inRange = true;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(LocalizationManager.ToPlainText(this.welcomeText), 0.5f, this.inRange).Forget();
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x000327D8 File Offset: 0x000309D8
	public void OnExit()
	{
		if (!this.inRange)
		{
			return;
		}
		this.inRange = false;
		this.animator.SetBool("InRange", this.inRange);
		this.PopText(LocalizationManager.ToPlainText(this.leaveText), 0f, this.inRange).Forget();
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0003282C File Offset: 0x00030A2C
	private UniTask PopText(string text, float delay, bool _inRange)
	{
		BunkerDoorVisual.<PopText>d__8 <PopText>d__;
		<PopText>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<PopText>d__.<>4__this = this;
		<PopText>d__.text = text;
		<PopText>d__.delay = delay;
		<PopText>d__._inRange = _inRange;
		<PopText>d__.<>1__state = -1;
		<PopText>d__.<>t__builder.Start<BunkerDoorVisual.<PopText>d__8>(ref <PopText>d__);
		return <PopText>d__.<>t__builder.Task;
	}

	// Token: 0x04000A1A RID: 2586
	[LocalizationKey("Dialogues")]
	public string welcomeText;

	// Token: 0x04000A1B RID: 2587
	[LocalizationKey("Dialogues")]
	public string leaveText;

	// Token: 0x04000A1C RID: 2588
	public Transform textBubblePoint;

	// Token: 0x04000A1D RID: 2589
	public bool inRange = true;

	// Token: 0x04000A1E RID: 2590
	public Animator animator;
}

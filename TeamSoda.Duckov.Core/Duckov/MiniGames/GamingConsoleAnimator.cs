using System;
using UnityEngine;

namespace Duckov.MiniGames
{
	// Token: 0x0200028F RID: 655
	public class GamingConsoleAnimator : MonoBehaviour
	{
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0004F01C File Offset: 0x0004D21C
		[SerializeField]
		private MiniGame Game
		{
			get
			{
				if (this.console == null)
				{
					return null;
				}
				return this.console.Game;
			}
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0004F039 File Offset: 0x0004D239
		private void Update()
		{
			this.Tick();
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0004F044 File Offset: 0x0004D244
		private void Tick()
		{
			if (this.Game == null)
			{
				this.Clear();
				return;
			}
			if (CameraMode.Active)
			{
				return;
			}
			this.joyStick_Target = this.Game.GetAxis(0);
			this.joyStick_Current = Vector2.Lerp(this.joyStick_Current, this.joyStick_Target, 0.25f);
			Vector2 vector = this.joyStick_Current;
			this.animator.SetFloat("AxisX", vector.x);
			this.animator.SetFloat("AxisY", vector.y);
			this.animator.SetBool("ButtonA", this.Game.GetButton(MiniGame.Button.A));
			this.animator.SetBool("ButtonB", this.Game.GetButton(MiniGame.Button.B));
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0004F108 File Offset: 0x0004D308
		private void Clear()
		{
			this.animator.SetBool("ButtonA", false);
			this.animator.SetBool("ButtonB", false);
			this.animator.SetFloat("AxisX", 0f);
			this.animator.SetFloat("AxisY", 0f);
		}

		// Token: 0x04000F8E RID: 3982
		[SerializeField]
		private Animator animator;

		// Token: 0x04000F8F RID: 3983
		[SerializeField]
		private GamingConsole console;

		// Token: 0x04000F90 RID: 3984
		private Vector2 joyStick_Current;

		// Token: 0x04000F91 RID: 3985
		private Vector2 joyStick_Target;
	}
}

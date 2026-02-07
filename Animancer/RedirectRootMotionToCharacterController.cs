using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200004D RID: 77
	[AddComponentMenu("Animancer/Redirect Root Motion To Character Controller")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/RedirectRootMotionToCharacterController")]
	public class RedirectRootMotionToCharacterController : RedirectRootMotion<CharacterController>
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x0000D37C File Offset: 0x0000B57C
		protected unsafe override void OnAnimatorMove()
		{
			if (!base.ApplyRootMotion)
			{
				return;
			}
			base.Target->Move(base.Animator->deltaPosition);
			base.Target->transform.rotation *= base.Animator->deltaRotation;
		}
	}
}

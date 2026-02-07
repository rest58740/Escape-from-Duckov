using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200004E RID: 78
	[AddComponentMenu("Animancer/Redirect Root Motion To Rigidbody")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/RedirectRootMotionToRigidbody")]
	public class RedirectRootMotionToRigidbody : RedirectRootMotion<Rigidbody>
	{
		// Token: 0x060004B5 RID: 1205 RVA: 0x0000D3DC File Offset: 0x0000B5DC
		protected unsafe override void OnAnimatorMove()
		{
			if (!base.ApplyRootMotion)
			{
				return;
			}
			base.Target->MovePosition(base.Target->position + base.Animator->deltaPosition);
			base.Target->MoveRotation(base.Target->rotation * base.Animator->deltaRotation);
		}
	}
}

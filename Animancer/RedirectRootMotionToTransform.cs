using System;
using UnityEngine;

namespace Animancer
{
	// Token: 0x0200004F RID: 79
	[AddComponentMenu("Animancer/Redirect Root Motion To Transform")]
	[HelpURL("https://kybernetik.com.au/animancer/api/Animancer/RedirectRootMotionToTransform")]
	public class RedirectRootMotionToTransform : RedirectRootMotion<Transform>
	{
		// Token: 0x060004B7 RID: 1207 RVA: 0x0000D44C File Offset: 0x0000B64C
		protected unsafe override void OnAnimatorMove()
		{
			if (!base.ApplyRootMotion)
			{
				return;
			}
			base.Target->position += base.Animator->deltaPosition;
			base.Target->rotation *= base.Animator->deltaRotation;
		}
	}
}

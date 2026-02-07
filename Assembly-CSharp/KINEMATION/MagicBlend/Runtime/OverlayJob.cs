using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using Unity.Collections;
using UnityEngine.Animations;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000057 RID: 87
	public struct OverlayJob : IAnimationJob
	{
		// Token: 0x0600032A RID: 810 RVA: 0x0000D854 File Offset: 0x0000BA54
		public void ProcessAnimation(AnimationStream stream)
		{
			if (!this.alwaysAnimate && !this.cachePose)
			{
				return;
			}
			BlendStreamAtom value = this.atoms[0];
			this.atoms[0] = value;
			KTransform ktransform = new KTransform
			{
				rotation = value.handle.GetRotation(stream),
				position = value.handle.GetPosition(stream)
			};
			int length = this.atoms.Length;
			for (int i = 1; i < length; i++)
			{
				BlendStreamAtom value2 = this.atoms[i];
				KTransform ktransform2 = new KTransform
				{
					rotation = value2.handle.GetRotation(stream),
					position = value2.handle.GetPosition(stream)
				};
				ktransform2 = ktransform.GetRelativeTransform(ktransform2, false);
				value2.activePose.overlayPose = ktransform2;
				value2.activePose.overlayPose.position = value2.handle.GetLocalPosition(stream);
				value2.activePose.localOverlayRotation = value2.handle.GetLocalRotation(stream);
				this.atoms[i] = value2;
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000D982 File Offset: 0x0000BB82
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x04000204 RID: 516
		[ReadOnly]
		public bool alwaysAnimate;

		// Token: 0x04000205 RID: 517
		[ReadOnly]
		public bool cachePose;

		// Token: 0x04000206 RID: 518
		public NativeArray<BlendStreamAtom> atoms;
	}
}

using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using Unity.Collections;
using UnityEngine.Animations;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000056 RID: 86
	public struct PoseJob : IAnimationJob
	{
		// Token: 0x06000328 RID: 808 RVA: 0x0000D748 File Offset: 0x0000B948
		public void ProcessAnimation(AnimationStream stream)
		{
			if (!this.alwaysAnimate && !this.readPose)
			{
				return;
			}
			BlendStreamAtom blendStreamAtom = this.atoms[0];
			KTransform ktransform = new KTransform
			{
				rotation = blendStreamAtom.handle.GetRotation(stream),
				position = blendStreamAtom.handle.GetPosition(stream)
			};
			int length = this.atoms.Length;
			for (int i = 1; i < length; i++)
			{
				BlendStreamAtom value = this.atoms[i];
				KTransform ktransform2 = new KTransform
				{
					position = value.handle.GetPosition(stream),
					rotation = value.handle.GetRotation(stream)
				};
				ktransform2 = ktransform.GetRelativeTransform(ktransform2, false);
				value.activePose.basePose = ktransform2;
				value.activePose.basePose.position = value.handle.GetLocalPosition(stream);
				this.atoms[i] = value;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000D850 File Offset: 0x0000BA50
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x04000201 RID: 513
		[ReadOnly]
		public bool alwaysAnimate;

		// Token: 0x04000202 RID: 514
		[ReadOnly]
		public bool readPose;

		// Token: 0x04000203 RID: 515
		public NativeArray<BlendStreamAtom> atoms;
	}
}

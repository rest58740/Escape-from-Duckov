using System;
using KINEMATION.KAnimationCore.Runtime.Rig;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x0200005C RID: 92
	public class MagicBlendLibrary
	{
		// Token: 0x06000330 RID: 816 RVA: 0x0000DD24 File Offset: 0x0000BF24
		public static NativeArray<BlendStreamAtom> SetupBlendAtoms(Animator animator, KRigComponent rigComponent)
		{
			Transform[] rigTransforms = rigComponent.GetRigTransforms();
			int num = rigTransforms.Length + 1;
			NativeArray<BlendStreamAtom> result = new NativeArray<BlendStreamAtom>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			for (int i = 0; i < num; i++)
			{
				Transform transform = animator.transform;
				if (i > 0)
				{
					transform = rigTransforms[i - 1];
				}
				result[i] = new BlendStreamAtom
				{
					handle = animator.BindStreamTransform(transform)
				};
			}
			return result;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		public static void ConnectPose(AnimationScriptPlayable playable, PlayableGraph graph, AnimationClip pose)
		{
			if (playable.GetInput(0).IsValid<Playable>())
			{
				playable.DisconnectInput(0);
			}
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(graph, pose);
			animationClipPlayable.SetSpeed(0.0);
			animationClipPlayable.SetApplyFootIK(false);
			playable.ConnectInput(0, animationClipPlayable, 0, 1f);
		}
	}
}

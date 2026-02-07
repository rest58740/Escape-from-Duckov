using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;

namespace KINEMATION.MagicBlend.Runtime
{
	// Token: 0x02000058 RID: 88
	public struct LayeringJob : IAnimationJob
	{
		// Token: 0x0600032C RID: 812 RVA: 0x0000D984 File Offset: 0x0000BB84
		public void ProcessAnimation(AnimationStream stream)
		{
			BlendStreamAtom blendStreamAtom = this.atoms[0];
			KTransform ktransform = new KTransform
			{
				rotation = blendStreamAtom.handle.GetRotation(stream),
				position = blendStreamAtom.handle.GetPosition(stream)
			};
			int length = this.atoms.Length;
			for (int i = 1; i < length; i++)
			{
				BlendStreamAtom blendStreamAtom2 = this.atoms[i];
				KTransform worldTransform = new KTransform
				{
					rotation = blendStreamAtom2.handle.GetRotation(stream),
					position = blendStreamAtom2.handle.GetPosition(stream),
					scale = Vector3.one
				};
				blendStreamAtom2.meshStreamPose = ktransform.GetRelativeTransform(worldTransform, false);
				blendStreamAtom2.meshStreamPose.position = blendStreamAtom2.handle.GetLocalPosition(stream);
				blendStreamAtom2.activePose.additiveWeight = blendStreamAtom2.additiveWeight;
				blendStreamAtom2.activePose.baseWeight = blendStreamAtom2.baseWeight;
				blendStreamAtom2.activePose.localWeight = blendStreamAtom2.localWeight;
				this.atoms[i] = blendStreamAtom2;
			}
			for (int j = 1; j < length; j++)
			{
				BlendStreamAtom blendStreamAtom3 = this.atoms[j];
				AtomPose blendedAtomPose = blendStreamAtom3.GetBlendedAtomPose(this.blendWeight);
				if (this.cachePose)
				{
					blendStreamAtom3.cachedPose = blendedAtomPose;
					this.atoms[j] = blendStreamAtom3;
				}
				KTransform basePose = blendedAtomPose.basePose;
				KTransform overlayPose = blendedAtomPose.overlayPose;
				Quaternion localOverlayRotation = blendedAtomPose.localOverlayRotation;
				float additiveWeight = blendedAtomPose.additiveWeight;
				float baseWeight = blendedAtomPose.baseWeight;
				float localWeight = blendedAtomPose.localWeight;
				KTransform ktransform2 = new KTransform
				{
					rotation = blendStreamAtom3.meshStreamPose.rotation * Quaternion.Inverse(basePose.rotation),
					position = blendStreamAtom3.meshStreamPose.position - basePose.position
				};
				Quaternion quaternion = ktransform2.rotation * overlayPose.rotation;
				quaternion = Quaternion.Slerp(overlayPose.rotation, quaternion, additiveWeight);
				quaternion = Quaternion.Slerp(blendStreamAtom3.meshStreamPose.rotation, quaternion, baseWeight);
				quaternion = ktransform.rotation * quaternion;
				Vector3 vector = overlayPose.position + ktransform2.position * additiveWeight;
				vector = Vector3.Lerp(blendStreamAtom3.meshStreamPose.position, vector, baseWeight);
				blendStreamAtom3.handle.SetRotation(stream, quaternion);
				quaternion = Quaternion.Slerp(blendStreamAtom3.handle.GetLocalRotation(stream), localOverlayRotation, localWeight);
				blendStreamAtom3.handle.SetLocalRotation(stream, quaternion);
				vector = Vector3.Lerp(vector, overlayPose.position, localWeight);
				blendStreamAtom3.handle.SetLocalPosition(stream, vector);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000DC61 File Offset: 0x0000BE61
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x04000207 RID: 519
		[ReadOnly]
		public float blendWeight;

		// Token: 0x04000208 RID: 520
		[ReadOnly]
		public bool cachePose;

		// Token: 0x04000209 RID: 521
		public NativeArray<BlendStreamAtom> atoms;
	}
}

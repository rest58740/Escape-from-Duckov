using System;
using KINEMATION.KAnimationCore.Runtime.Rig;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x02000015 RID: 21
	public class KAnimationMath
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002B24 File Offset: 0x00000D24
		public static Quaternion RotateInSpace(Quaternion space, Quaternion target, Quaternion rotation, float alpha)
		{
			return Quaternion.Slerp(target, space * rotation * (Quaternion.Inverse(space) * target), alpha);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B45 File Offset: 0x00000D45
		public static Quaternion RotateInSpace(KTransform space, KTransform target, Quaternion offset, float alpha)
		{
			return KAnimationMath.RotateInSpace(space.rotation, target.rotation, offset, alpha);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B5A File Offset: 0x00000D5A
		public static void RotateInSpace(Transform space, Transform target, Quaternion offset, float alpha)
		{
			target.rotation = KAnimationMath.RotateInSpace(space.rotation, target.rotation, offset, alpha);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B75 File Offset: 0x00000D75
		public static Vector3 MoveInSpace(KTransform space, KTransform target, Vector3 offset, float alpha)
		{
			return target.position + (space.TransformPoint(offset, false) - space.position) * alpha;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B9C File Offset: 0x00000D9C
		public static void MoveInSpace(Transform space, Transform target, Vector3 offset, float alpha)
		{
			target.position += (space.TransformPoint(offset) - space.position) * alpha;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BC7 File Offset: 0x00000DC7
		public static bool IsWeightFull(float weight)
		{
			return Mathf.Approximately(weight, 1f);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public static bool IsWeightRelevant(float weight)
		{
			return !Mathf.Approximately(weight, 0f);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002BE4 File Offset: 0x00000DE4
		public static void ModifyTransform(Transform component, Transform target, in KPose pose, float alpha = 1f)
		{
			if (pose.modifyMode == EModifyMode.Add)
			{
				KAnimationMath.AddTransform(component, target, pose, alpha);
				return;
			}
			KAnimationMath.ReplaceTransform(component, target, pose, alpha);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002C04 File Offset: 0x00000E04
		private static void AddTransform(Transform component, Transform target, in KPose pose, float alpha = 1f)
		{
			if (pose.space == ESpaceType.BoneSpace)
			{
				KAnimationMath.MoveInSpace(target, target, pose.pose.position, alpha);
				KAnimationMath.RotateInSpace(target, target, pose.pose.rotation, alpha);
				return;
			}
			if (pose.space == ESpaceType.ParentBoneSpace)
			{
				Transform parent = target.parent;
				KAnimationMath.MoveInSpace(parent, target, pose.pose.position, alpha);
				KAnimationMath.RotateInSpace(parent, target, pose.pose.rotation, alpha);
				return;
			}
			if (pose.space == ESpaceType.ComponentSpace)
			{
				KAnimationMath.MoveInSpace(component, target, pose.pose.position, alpha);
				KAnimationMath.RotateInSpace(component, target, pose.pose.rotation, alpha);
				return;
			}
			Vector3 position = target.position;
			Quaternion rotation = target.rotation;
			target.position = Vector3.Lerp(position, position + pose.pose.position, alpha);
			target.rotation = Quaternion.Slerp(rotation, rotation * pose.pose.rotation, alpha);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002CF0 File Offset: 0x00000EF0
		private static void ReplaceTransform(Transform component, Transform target, in KPose pose, float alpha = 1f)
		{
			if (pose.space == ESpaceType.BoneSpace || pose.space == ESpaceType.ParentBoneSpace)
			{
				target.localPosition = Vector3.Lerp(target.localPosition, pose.pose.position, alpha);
				target.localRotation = Quaternion.Slerp(target.localRotation, pose.pose.rotation, alpha);
				return;
			}
			if (pose.space == ESpaceType.ComponentSpace)
			{
				target.position = Vector3.Lerp(target.position, component.TransformPoint(pose.pose.position), alpha);
				target.rotation = Quaternion.Slerp(target.rotation, component.rotation * pose.pose.rotation, alpha);
				return;
			}
			target.position = Vector3.Lerp(target.position, pose.pose.position, alpha);
			target.rotation = Quaternion.Slerp(target.rotation, pose.pose.rotation, alpha);
		}
	}
}

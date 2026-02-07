using System;
using UnityEngine;

namespace KINEMATION.KAnimationCore.Runtime.Core
{
	// Token: 0x0200001F RID: 31
	[Serializable]
	public struct KTransform
	{
		// Token: 0x06000051 RID: 81 RVA: 0x0000352F File Offset: 0x0000172F
		public KTransform(Vector3 newPos, Quaternion newRot, Vector3 newScale)
		{
			this.position = newPos;
			this.rotation = newRot;
			this.scale = newScale;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003546 File Offset: 0x00001746
		public KTransform(Vector3 newPos, Quaternion newRot)
		{
			this.position = newPos;
			this.rotation = newRot;
			this.scale = Vector3.one;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003564 File Offset: 0x00001764
		public KTransform(Transform t, bool worldSpace = true)
		{
			if (worldSpace)
			{
				this.position = t.position;
				this.rotation = t.rotation;
			}
			else
			{
				this.position = t.localPosition;
				this.rotation = t.localRotation;
			}
			this.scale = t.localScale;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000035B4 File Offset: 0x000017B4
		public static KTransform Lerp(KTransform a, KTransform b, float alpha)
		{
			Vector3 newPos = Vector3.Lerp(a.position, b.position, alpha);
			Quaternion newRot = Quaternion.Slerp(a.rotation, b.rotation, alpha);
			Vector3 newScale = Vector3.Lerp(a.scale, a.scale, alpha);
			return new KTransform(newPos, newRot, newScale);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003600 File Offset: 0x00001800
		public static KTransform EaseLerp(KTransform a, KTransform b, float alpha, EaseMode easeMode)
		{
			return KTransform.Lerp(a, b, KCurves.Ease(0f, 1f, alpha, easeMode));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000361A File Offset: 0x0000181A
		public static KTransform ExpDecay(KTransform a, KTransform b, float speed, float deltaTime)
		{
			return KTransform.Lerp(a, b, KMath.ExpDecayAlpha(speed, deltaTime));
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000362C File Offset: 0x0000182C
		public bool Equals(KTransform other, bool useScale)
		{
			bool flag = this.position.Equals(other.position) && this.rotation.Equals(other.rotation);
			if (useScale)
			{
				flag = (flag && this.scale.Equals(other.scale));
			}
			return flag;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003680 File Offset: 0x00001880
		public Vector3 InverseTransformPoint(Vector3 worldPosition, bool useScale)
		{
			Vector3 vector = Quaternion.Inverse(this.rotation) * (worldPosition - this.position);
			if (useScale)
			{
				vector = Vector3.Scale(this.scale, vector);
			}
			return vector;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000036BC File Offset: 0x000018BC
		public Vector3 InverseTransformVector(Vector3 worldDirection, bool useScale)
		{
			Vector3 vector = Quaternion.Inverse(this.rotation) * worldDirection;
			if (useScale)
			{
				vector = Vector3.Scale(this.scale, vector);
			}
			return vector;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000036EC File Offset: 0x000018EC
		public Vector3 TransformPoint(Vector3 localPosition, bool useScale)
		{
			if (useScale)
			{
				localPosition = Vector3.Scale(this.scale, localPosition);
			}
			return this.position + this.rotation * localPosition;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003716 File Offset: 0x00001916
		public Vector3 TransformVector(Vector3 localDirection, bool useScale)
		{
			if (useScale)
			{
				localDirection = Vector3.Scale(this.scale, localDirection);
			}
			return this.rotation * localDirection;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003738 File Offset: 0x00001938
		public KTransform GetRelativeTransform(KTransform worldTransform, bool useScale)
		{
			return new KTransform
			{
				position = this.InverseTransformPoint(worldTransform.position, useScale),
				rotation = Quaternion.Inverse(this.rotation) * worldTransform.rotation,
				scale = Vector3.Scale(this.scale, worldTransform.scale)
			};
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003798 File Offset: 0x00001998
		public KTransform GetWorldTransform(KTransform localTransform, bool useScale)
		{
			return new KTransform
			{
				position = this.TransformPoint(localTransform.position, useScale),
				rotation = this.rotation * localTransform.rotation,
				scale = Vector3.Scale(this.scale, localTransform.scale)
			};
		}

		// Token: 0x04000045 RID: 69
		public static KTransform Identity = new KTransform(Vector3.zero, Quaternion.identity, Vector3.one);

		// Token: 0x04000046 RID: 70
		public Vector3 position;

		// Token: 0x04000047 RID: 71
		public Quaternion rotation;

		// Token: 0x04000048 RID: 72
		public Vector3 scale;
	}
}
